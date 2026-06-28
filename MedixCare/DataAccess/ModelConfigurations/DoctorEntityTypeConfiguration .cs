using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedixCare.DataAccess.ModelConfigurations
{
    public class DoctorEntityTypeConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.HasKey(d => d.Id);
            builder.Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(d => d.Specialty).IsRequired() .HasMaxLength(100);
            builder.Property(d => d.ExaminationFee).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(d => d.FollowUpFee).IsRequired().HasColumnType("decimal(18,2)");

            builder.HasMany(d => d.Appointments)
                .WithOne(a => a.Doctor)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(d => d.DoctorSchedules)
                .WithOne(ds => ds.Doctor)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(d => d.DoctorLeaves)
                .WithOne(dl => dl.Doctor).
                OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => d.Clinic)
                .WithMany(c => c.Doctors)
                .HasForeignKey(d => d.ClinicId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
