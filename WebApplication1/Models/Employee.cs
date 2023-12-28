using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("tblEmployee")]
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string? ImagePath { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }




    public class MyDb : DbContext
    {
        public DbSet<Employee> Employees { get; set; }


        public MyDb(DbContextOptions opt):base(opt)
        {
            
        }
    }
}
