using EPiServer.DataAbstraction;
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

        public Project Project { get; set; }

        public int Count { get; set; }

        public int PendingPublish { get; set; }

        public double PercentComplete
        {
            get
            {
                return (double)this.PendingPublish / this.Count;
            }
        }
    }
}