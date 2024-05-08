using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models
{
    public class ForgotPassword
    {
        [Key]
        public int ID { get; set; }
        public string? TempOtp { get; set; }
        public string? TempMail { get; set; }
    }
}
