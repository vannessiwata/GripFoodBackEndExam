using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GripFoodExamBackEnd.Entities.DesignTime
{
    internal class DesignTimeApplicationDbContext : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            //optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BelajarNextJs;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            optionsBuilder.UseSqlite("Data Source=local.db");
            optionsBuilder.UseOpenIddict();

            var db = new ApplicationDbContext(optionsBuilder.Options);
            return db;
        }
    }
}
