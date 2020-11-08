using AlloyEPI.Business.ProjectsAdmin.Models;
using EPiServer;
using EPiServer.Cms.Shell.UI.Rest.Projects;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace AlloyEPI.Business.ProjectsAdmin.Controllers
{
    [Authorize(Roles = "Administrators, WebAdmins, WebEditors")]
    public class ProjectsAdminController : Controller
    {
        private readonly IProjectResolver projectResolver;

        private readonly ProjectRepository projectRepository;

        private readonly IProjectService projectService;

        private readonly IContentLoader contentLoader;

        public ProjectsAdminController(IProjectResolver projectResolver, ProjectRepository projectRepository, IProjectService projectService, IContentLoader contentLoader)
        {
            this.projectResolver = projectResolver;
            this.projectRepository = projectRepository;
            this.projectService = projectService;
            this.contentLoader = contentLoader;
        }

        public ActionResult Index()
        {
            var list = this.projectRepository.List()
                .OrderBy(x => x.CreatedBy.Equals(User.Identity.Name))
                .ThenBy(x => x.Created);

            var model = new ProjectsItemListing();

            foreach (var item in list)
            {
                var items = this.projectRepository.ListItems(item.ID);
                var projectItem = new ProjectsItem(item)
                {
                    Count = items.Count(),
                    PendingPublish = items.Select(x => this.contentLoader.Get<IContent>(x.ContentLink, new LoaderOptions { new ProjectLoaderOption { ProjectIds = new[] { item.ID } } }))
                        .Where(x => (x is IVersionable versionable) && ((versionable.Status == VersionStatus.CheckedIn || versionable.IsPendingPublish)))
                        .Count()
                };

                if (item.CreatedBy.Equals(User.Identity.Name, System.StringComparison.OrdinalIgnoreCase))
                    model.MyListings.Add(projectItem);
                else
                    model.OtherListings.Add(projectItem);
            }

            return View("~/Business/ProjectsAdmin/Views/Index.cshtml", model);
        }
    }
}