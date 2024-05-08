using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystem.Models
{
    [Table("UserInfo")]
    public class UserInfo
    {
        [Key]
        public int UserId  { get; set; }

        [Required(ErrorMessage ="Please enter first name")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "Please enter last name")]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter employee code")]
        [StringLength(15)]
        public string UserCode { get; set; }

        [Required(ErrorMessage = "Please enter email id")]
        [StringLength(50)]
        [EmailAddress]
        public string EmailId { get; set; }

        [Required(ErrorMessage = "Please enter mobile number")]
        [MaxLength(10)]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "Please select user role")]
        [StringLength(10)]
        public string UserRole { get; set; }

        [Required(ErrorMessage = "Please enter user password")]
        [StringLength(25)]
        public string UserPassword { get; set; }

        [StringLength(25)]
        public string? City { get; set; }

    }
}
