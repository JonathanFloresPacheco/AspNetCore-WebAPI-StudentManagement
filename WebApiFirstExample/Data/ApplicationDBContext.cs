using Microsoft.EntityFrameworkCore;
using WebApiFirstExample.Model;

namespace WebApiFirstExample.Data
{
    public class ApplicationDBContext: DbContext
    {
        // Contains and receives all the configuration of the database
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }
        // Add the class to represent to the model tables
        public DbSet<Student> Students { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Department> Departments { get; set; }
    }
}
