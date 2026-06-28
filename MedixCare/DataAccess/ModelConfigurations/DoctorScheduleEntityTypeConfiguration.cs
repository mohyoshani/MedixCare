using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedixCare.DataAccess.ModelConfigurations
{
    public class DoctorScheduleEntityTypeConfiguration : IEntityTypeConfiguration<DoctorSchedule>
    {
        public void Configure(EntityTypeBuilder<DoctorSchedule> builder)
        {
            builder.HasKey(ds => ds.Id);
            builder.Property(ds => ds.StartTime)
                   .IsRequired();
            builder.Property(ds => ds.EndTime).IsRequired();
            builder.Property(ds => ds.DayOfWeek).IsRequired();

            builder.HasOne(ds => ds.Doctor)
                   .WithMany(d => d.DoctorSchedules)
                   .HasForeignKey(ds => ds.DoctorId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
