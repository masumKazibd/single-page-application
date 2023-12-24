using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace EmployeeInfoTask.Models
{
    public enum Status
    {
        Active,
        Inactive
    }
    public enum Gender
    {
        Male,
        Female,
        Other
    }
    public enum BloodGroup
    {
        A_Positive,
        A_Negative,
        B_Positive,
        B_Negative,
        AB_Positive,
        AB_Negative,
        O_Positive,
        O_Negative
    }
    public enum EmployeeStatus
    {
        Active,
        Inactive
    }
    public enum EducationLevel
    {
        SSC,
        HSC,
        Graduate
    }

    public class Employee
    {
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [MinLength(4, ErrorMessage = "Name must be at least 4 characters.")]
        public string Name { get; set; } = default!;

        [Required(ErrorMessage = "Father's Name is required.")]
        [MinLength(4, ErrorMessage = "Father's Name must be at least 4 characters.")]
        public string FatherName { get; set; } = default!;

        [Required(ErrorMessage = "Mother's Name is required.")]
        [MinLength(4, ErrorMessage = "Mother's Name must be at least 4 characters.")]
        public string MotherName { get; set; } = default!;

        public string Username { get; set; } = default!;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } = default!;

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^(?:\+88|01)?\d{11}$", ErrorMessage = "Enter Bangladeshi mobile number.")]
        public string Phone { get; set; } = default!;

        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; } = default!;

        [Required(ErrorMessage = "Blood Group is required.")]
        public string BloodGroup { get; set; } = default!;
        [Required, Column(TypeName = "date"), Display(Name = "Date of Birth"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public string Status { get; set; } = default!;
        [ForeignKey("Department")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        public string Education { get; set; } = default!;

        public virtual Department? Department { get; set; }
    }
    public class Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = default!;
        public string ShortName { get; set; } = default!;
        public int Serial { get; set; }
        public virtual ICollection<Employee>? Employees { get; set; }
    }

    public class EmployeeDbContext : DbContext
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options) { }

        public DbSet<Employee> Employee { get; set; } = default!;
        public DbSet<Department> Department { get; set; } = default!;
    }

}
