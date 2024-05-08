using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.LoginModels
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "Please enter valid uniquekey")]
        [Display(Name="Unique Key")]
        public string? UniqueKey { get; set; }

        public string? Role { get; set; }

        [Required(ErrorMessage = "Please enter first name")]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Display(Name = "Middle Name")]
        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "Please enter last name ")]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Please enter email address")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Id")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Please enter password")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Please enter confirm password")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [Display(Name = "Confirm Password")]
        public string? confirmPassword { get; set; }
    }
}
