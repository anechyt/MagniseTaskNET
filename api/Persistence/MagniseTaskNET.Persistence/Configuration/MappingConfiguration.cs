using MagniseTaskNET.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MagniseTaskNET.Persistence.Configuration
{
    public class MappingConfiguration : IEntityTypeConfiguration<Mapping>
    {
        public void Configure(EntityTypeBuilder<Mapping> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
