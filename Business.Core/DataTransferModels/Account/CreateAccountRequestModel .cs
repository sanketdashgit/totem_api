using System;
using System.ComponentModel.DataAnnotations;

namespace Totem.Business.Core.DataTransferModels.Account
{

    public class CreateAccountRequestModel
    {
        [Required(ErrorMessage = "Firstname is required field")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Lastname is required field")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email address is required field")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
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
        [Required(ErrorMessage = "Password is required field")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!% *? &])[A-Za-z\d@$!%*?&]{8,15}$", ErrorMessage = "The password should be at least 8 characters long, at most 15 characters including at least one capital and small letter, a special character and a number.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Role is required field")]
        public int Role { get; set; }
    }
    public class DateMinimumAgeAttribute : ValidationAttribute
    {
        public DateMinimumAgeAttribute(int minimumAge)
        {
            MinimumAge = minimumAge;
            ErrorMessage = "{0} must be someone at least {1} years of age";
        }

        public override bool IsValid(object value)
        {
            DateTime date;
            if ((value != null && DateTime.TryParse(value.ToString(), out date)))
            {
                return date.AddYears(MinimumAge) < DateTime.UtcNow;
            }

            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name, MinimumAge);
        }

        public int MinimumAge { get; }
    }
    //public class CreateAccountRequestModel
    //{
    //    [Required(ErrorMessage = "Firstname is required field")]
    //    public string FirstName { get; set; }
    //    [Required(ErrorMessage = "Lastname is required field")]
    //    public string LastName { get; set; }
    //    [Required(ErrorMessage = "Email address is required field")]
    //    public string Email { get; set; }
    //    [Required(ErrorMessage = "Phonenumber is required field")]
    //    public string Phone { get; set; }
    //    [Required(ErrorMessage = "Username is required field")]
    //    public string Username { get; set; }
    //    [Required(ErrorMessage = "Password is required field")]
    //    public string Password { get; set; }
    //    [Required(ErrorMessage = "IsAccountant is required field")]
    //    public int Role { get; set; }
    //}
}
