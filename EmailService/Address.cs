using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailService
{
    public class Address
    {
        public string Name { get; set; }
       
        [Display(Name = "Adres e-mail")]
        [Required(ErrorMessage = "Pole adres jest wymagane !!!")]
        [EmailAddress(ErrorMessage = "Nieprawidłowy adres e-mail")]
        public string Email { get; set; }
    }
}
