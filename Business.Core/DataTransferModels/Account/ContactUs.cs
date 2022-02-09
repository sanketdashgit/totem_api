using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Totem.Business.Core.DataTransferModels.Account
{
    public class ContactUs
    {
           
            public string Firstname { get; set; }
            public string Lastname { get; set; }

        [Required(ErrorMessage = "Email address is required field")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }
            public string Phone { get; set; }
            public string Message { get; set; }           
       
    }
}
