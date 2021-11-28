using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.Models.Domains
{
    public class ProjectStatus
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Lp")]
        public int OrdinalNumber { get; set; }

        [Required]
        [Display(Name = "Nazwa Statusu")]
        public string Name { get; set; }


        [Required]
        [Display(Name = "Skrót")]
        public string Abbrev { get; set; }


        [Display(Name = "Opis")]
        public string Description { get; set; }


        public ICollection<Project> Projects = new Collection<Project>();

    }
}
