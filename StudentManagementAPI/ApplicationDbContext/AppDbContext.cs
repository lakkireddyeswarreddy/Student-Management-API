using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentManagementAPI.Model;

namespace StudentManagementAPI.ApplicationDbContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }

        public DbSet<Teacher> Teachers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Student>().HasData(new Student { StudentId = 27, StudentName = "Eswar Reddy", Age = 23, Grade = "A" }, new Student { StudentId = 56, StudentName = "Deeksha banoth", Age = 23, Grade = "B" });
        }


    }
}
