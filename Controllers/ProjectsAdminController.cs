using AlloyEPI.Business.ProjectsAdmin.Models;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using System.Linq;
using System.Web.Mvc;

namespace AlloyEPI.Business.ProjectsAdmin.Controllers
{
    [Authorize(Roles = "Administrators, WebAdmins, WebEditors")]
    public class ProjectsAdminController : Controller
    {
        private readonly IProjectResolver projectResolver;

        private readonly ProjectRepository projectRepository;

        private readonly IContentVersionRepository contentVersionRepository;

        private readonly IContentLoader contentLoader;

        public ProjectsAdminController(IProjectResolver projectResolver, ProjectRepository projectRepository, IContentVersionRepository contentVersionRepository, IContentLoader contentLoader)
        {
            this.projectResolver = projectResolver;
            this.projectRepository = projectRepository;
            this.contentVersionRepository = contentVersionRepository;
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
                var items = this.projectRepository.ListItems(item.ID)
                    .Select(x => this.contentLoader.Get<IContent>(x.ContentLink, new LoaderOptions { new ProjectLoaderOption { ProjectIds = new[] { item.ID } } }));
                var projectItem = new ProjectsItem(item)
                {
                    Count = items.Count(),
                    PendingPublish = items
                        .OfType<IVersionable>()
                        .Where(x => x.Status == VersionStatus.CheckedIn || x.IsPendingPublish)
                        .Count(),
                    LastUpdated = items
                        .Select(x => this.contentVersionRepository.Load(x.ContentLink))
                        .OrderBy(x => x.Saved)
                        .FirstOrDefault(version => version.IsMasterLanguageBranch).Saved
                };

                if (item.CreatedBy.Equals(User.Identity.Name, System.StringComparison.OrdinalIgnoreCase))
                    model.MyListings.Add(projectItem);
                else
                    model.OtherListings.Add(projectItem);
            }

            return View("~/Business/ProjectsAdmin/Views/Index.cshtml", model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (id != 0)
            {
                this.projectRepository.Delete(id);
            }
            return this.RedirectToAction("Index");
        }
    }
}