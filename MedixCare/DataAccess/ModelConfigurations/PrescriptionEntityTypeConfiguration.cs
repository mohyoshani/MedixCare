namespace MedixCare.DataAccess.ModelConfigurations
{
    public class PrescriptionEntityTypeConfiguration : IEntityTypeConfiguration<Prescription>
    {
        public void Configure(EntityTypeBuilder<Prescription> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Diagnosis)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(p => p.CreatedAt)
                .IsRequired();

            builder.HasOne(p => p.Appointment)
                .WithOne(a => a.Prescription)
                .HasForeignKey<Prescription>(p => p.AppointmentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
