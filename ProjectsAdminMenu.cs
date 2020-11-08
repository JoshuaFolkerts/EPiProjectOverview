using EPiServer;
using EPiServer.Security;
using EPiServer.Shell.Navigation;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace AlloyEPI.Business.ProjectsAdmin
{
    [MenuProvider]
    public class ProjectsAdminMenuProvider : IMenuProvider
    {
        public IEnumerable<MenuItem> GetMenuItems()
        {
            var menuItems = new List<MenuItem>
            {
                new UrlMenuItem("Projects", MenuPaths.Global + "/cms" + "/cmsMenuItem", "/projectsadmin/index")
                {
                    SortIndex = SortIndex.Last,
                    IsAvailable = (request) => PrincipalInfo.HasAdminAccess
                }
            };

            return menuItems;
        }
    }
}