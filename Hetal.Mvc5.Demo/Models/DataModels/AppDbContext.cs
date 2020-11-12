using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Hetal.Mvc5.Demo.Models.DataModels
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("name=default")
        {

        }

        public DbSet<UserMaster> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }
}