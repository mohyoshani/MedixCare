
namespace MedixCare.DataAccess.ModelConfigurations
{
    public class DoctorLeavesEntityTypeConfiguration : IEntityTypeConfiguration<DoctorLeave>
    {
        public void Configure(EntityTypeBuilder<DoctorLeave> builder)
        {
           builder.HasKey(dl => dl.Id);
            builder.Property(dl => dl.Reason)
                .IsRequired()
                .HasMaxLength(500);
            builder.Property(dl => dl.LeaveFrom)
                .IsRequired();
            builder.Property(dl => dl.LeaveTo)
                .IsRequired();
            builder.HasOne(dl => dl.Doctor).WithMany(dl=>dl.DoctorLeaves)
                .HasForeignKey(dl => dl.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
