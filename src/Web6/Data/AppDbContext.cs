using Microsoft.EntityFrameworkCore;
using Web6.Data.Models;

namespace Web6.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }


        public DbSet<ClassInfo> Classes { get; set; }

        public DbSet<GradeInfo> Grades { get; set; }

        public DbSet<StudentInfo> Students { get; set; }

        public DbSet<MajorInfo> Majors { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
