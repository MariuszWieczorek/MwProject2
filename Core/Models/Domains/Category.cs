using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.Models.Domains
{
    public class Category
    {
        public Category()
        {
            Projects = new Collection<Project>();
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public ICollection<Project> Projects;

    }
}
