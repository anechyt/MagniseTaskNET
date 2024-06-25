using MagniseTaskNET.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace MagniseTaskNET.Persistence
{
    public class MagniseTaskNETContext : DbContext
    {
        public MagniseTaskNETContext(DbContextOptions<MagniseTaskNETContext> contextOptions) : base(contextOptions) { }

        public virtual DbSet<Asset> Assets { get; set; }
        public virtual DbSet<Mapping> Mappings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
