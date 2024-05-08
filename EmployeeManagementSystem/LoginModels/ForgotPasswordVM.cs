using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.LoginModels
{
    public class ForgotPasswordVM
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
