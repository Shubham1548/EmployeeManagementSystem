using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.LoginModels
{
    public class LoginVM
    {
        [Required(ErrorMessage ="Please Enter vaild username")]
        public string? userName { get; set; }

        [Required(ErrorMessage = "Please Enter vaild password")]
        [DataType(DataType.Password)]
        public string? password { get; set; }
        
    }
}
