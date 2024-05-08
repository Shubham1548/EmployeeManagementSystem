using EmployeeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {    
        }

        public DbSet<UserInfo> users { get; set; }
        public DbSet<ForgotPassword> forgots { get; set; }
    }
}
