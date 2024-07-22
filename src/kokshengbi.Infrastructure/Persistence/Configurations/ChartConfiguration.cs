using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using kokshengbi.Domain.ChartAggregate;
using kokshengbi.Domain.ChartAggregate.ValueObjects;

namespace kokshengbi.Infrastructure.Persistence.Configurations
{
    public class ChartConfiguration : IEntityTypeConfiguration<Chart>
    {
        public void Configure(EntityTypeBuilder<Chart> builder)
        {
            ConfigureChartsTable(builder);
        }
        private void ConfigureChartsTable(EntityTypeBuilder<Chart> builder)
        {
            builder.ToTable("Charts");
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Id)
                   .HasConversion(
                       id => id.Value,
                       value => ChartId.Create(value))
                   .ValueGeneratedOnAdd() // Ensure ID is generated on add
                   .IsRequired();

            //builder.Property(m => m.name).HasMaxLength(1000);
        }
    }
}
