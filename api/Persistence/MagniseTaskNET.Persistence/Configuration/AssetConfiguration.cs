using MagniseTaskNET.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MagniseTaskNET.Persistence.Configuration
{
    public class AssetConfiguration : IEntityTypeConfiguration<Asset>
    {
        public void Configure(EntityTypeBuilder<Asset> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Symbol).IsRequired();
            builder.Property(x => x.TickSize).IsRequired();
            builder.Property(x => x.Currency).IsRequired();
            builder.Property(x => x.BaseCurrency).IsRequired();

            builder.HasMany(a => a.Mappings)
              .WithOne(m => m.Asset) 
              .HasForeignKey(m => m.AssetId)       
              .HasConstraintName("FK_Mappings_AssetId");
        }
    }
}
