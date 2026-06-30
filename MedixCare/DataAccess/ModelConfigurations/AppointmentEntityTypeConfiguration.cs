using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedixCare.DataAccess.ModelConfigurations
{
    public class AppointmentEntityTypeConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.AppointmentDate).IsRequired();
            builder.Property(a => a.CreatedAt).IsRequired();
            builder.Property(a => a.BookingChannel).IsRequired();
            builder.Property(a => a.VisitType).IsRequired();

            builder.HasOne(a => a.Doctor)
                   .WithMany(d => d.Appointments)
                   .HasForeignKey(a => a.DoctorId)
                   .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(a => a.Patient)
                   .WithMany(p => p.Appointments)
                   .HasForeignKey(a => a.PatientId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.ParentAppointment)
                   .WithMany()
                   .HasForeignKey(a => a.ParentAppointmentId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
