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

        [Display(Name = "Lp")]
        public int OrdinalNumber { get; set; }

        [Display(Name = "Użytkownik")]
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
