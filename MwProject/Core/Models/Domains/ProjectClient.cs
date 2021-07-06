using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.Models.Domains
{
    public class ProjectClient
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }

        [Display(Name = "Lp")]
        public int OrdinalNumber { get; set; }

        [Display(Name = "Nazwa")]
        public string Name { get; set; }

    }
}
