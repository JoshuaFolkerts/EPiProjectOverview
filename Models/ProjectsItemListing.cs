using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlloyEPI.Business.ProjectsAdmin.Models
{
    public class ProjectsItemListing
    {
        public List<ProjectsItem> MyListings { get; set; } = new List<ProjectsItem>();

        public List<ProjectsItem> OtherListings { get; set; } = new List<ProjectsItem>();
    }
}