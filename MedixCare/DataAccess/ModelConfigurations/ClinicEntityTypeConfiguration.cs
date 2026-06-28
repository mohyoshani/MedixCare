using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedixCare.DataAccess.ModelConfigurations
{
    public class ClinicEntityTypeConfiguration : IEntityTypeConfiguration<Clinic>
    {
        public void Configure(EntityTypeBuilder<Clinic> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(150);
            builder.HasMany(c => c.Doctors).WithOne(d => d.Clinic);
        }
    }
}
