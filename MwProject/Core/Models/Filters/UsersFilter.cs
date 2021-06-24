using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.Models.Filters
{
    public class UsersFilter
    {
        [Display(Name = "Nazwa użytkownika")]
        public string UserName { get; set; }
   
    }
}
