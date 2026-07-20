
namespace MedixCare.DataAccess.ModelConfigurations
{
    public class PatientEntityTypeConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(p => p.MobileNumber)
                .IsRequired()
                .HasMaxLength(11); 

            builder.Property(p => p.DateOfBirth)
                .IsRequired();

            builder.Property(p => p.CreatedAt)
                .IsRequired();

            builder.Property(p => p.Gender)
                .IsRequired();
        }
    }
}
