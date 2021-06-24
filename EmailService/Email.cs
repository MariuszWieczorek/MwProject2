using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailService
{
    public class Email
    {
        public Email()
        {
            EmailRecipients = new Collection<Address>();
        }
        public int Id { get; set; }

        [Required]
        [Display(Name = "Temat")]
        public string Subject { get; set; }

        [Display(Name = "Data Utworzenia")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Data Wysłania")]
        public DateTime? SentDate { get; set; }

        [Display(Name = "Treść Wiadomości")]
        public string Message { get; set; }

        public ICollection<Address> EmailRecipients { get; set; }
    }

}
