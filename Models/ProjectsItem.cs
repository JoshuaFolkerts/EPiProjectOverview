using EPiServer;
using EPiServer.DataAbstraction;
using EPiServer.Globalization;
using System;
using System.Globalization;
using System.Web.UI.WebControls;

namespace AlloyEPI.Business.ProjectsAdmin.Models
{
    public class ProjectsItem
    {
        public ProjectsItem()
        {
        }

        public ProjectsItem(Project project)
        {
            this.Project = project;
        }

        public string Url
        {
            get
            {
                CultureInfo currentCulture = ContentLanguage.PreferredCulture;
                return $"{UriSupport.UIUrl}/#context=epi.cms.project:///{this.Project.ID}&viewsetting=viewlanguage:///{currentCulture.Name}";
            }
        }

        public Project Project { get; set; }

        public int Count { get; set; }

        public int PendingPublish { get; set; }

        public DateTime LastUpdated { get; set; }

        public double PercentComplete
        {
            get
            {
                return (double)this.PendingPublish / this.Count;
            }
        }
    }
}