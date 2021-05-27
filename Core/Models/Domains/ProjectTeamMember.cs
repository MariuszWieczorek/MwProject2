using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.Models.Domains
{
    public class ProjectTeamMember
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }

        [Display(Name = "Lp")]
        public int OrdinalNumber { get; set; }

        [Display(Name = "Rola w zespole, przydzielone zadania")]
        public string Description { get; set; }

        [Display(Name = "Członek zespołu")]
        [Required(ErrorMessage = "Pole członek zespołu jest wymagane.")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
