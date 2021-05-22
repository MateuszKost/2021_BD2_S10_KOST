using Microsoft.EntityFrameworkCore;
using SmartCollection.Server.Models.Configuration;

namespace SmartCollection.Server.Models
{
    public class SmartCollectionDb : DbContext
    {
        public SmartCollectionDb(DbContextOptions<SmartCollectionDb> options) : base(options)
        { 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) =>
            modelBuilder.ApplyConfiguration(new ImageDetailsConfig());
    }
}
