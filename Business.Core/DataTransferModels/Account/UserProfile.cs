using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Totem.Business.Core.DataTransferModels.Account
{
    public class UserProfile 
    {
        [Required(ErrorMessage = "Firstname is required field")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Lastname is required field")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email address is required field")]
        //[RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Phonenumber is required field")]
        // [RegularExpression(@"^\d{3}-\d{3}-\d{4}$", ErrorMessage = "Please Enter Valid Phone number")]

        public string Phone { get; set; }
        [Required(ErrorMessage = "Birth Date is required field")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        // [DateMinimumAge(18, ErrorMessage = "Birth Date must be at least 18 years of age")]
        public string BirthDate { get; set; }

        [Required(ErrorMessage = "Username is required field")]
        public string Username { get; set; }       
        public int Id { get; set; }
        public string Address { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public string Bio { get; set; }
        //public string Image { get; set; }
    }

    public class AdminUserProfile 
    {
        [Required(ErrorMessage = "Firstname is required field")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Lastname is required field")]
        public string LastName { get; set; }

        public string Email { get; set; }
    }

}
