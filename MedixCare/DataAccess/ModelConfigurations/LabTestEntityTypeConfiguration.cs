using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedixCare.DataAccess.ModelConfigurations
{
    public class LabTestEntityTypeConfiguration : IEntityTypeConfiguration<LabTest>
    {
        public void Configure(EntityTypeBuilder<LabTest> builder)
        {
            builder.HasKey(lt => lt.Id);

            builder.Property(lt => lt.TestName)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(lt => lt.TestType)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(lt => lt.LabName)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(lt => lt.Summary)
                   .HasMaxLength(1000);

            builder.Property(lt => lt.TestFileName)
                   .HasMaxLength(255); 

            builder.Property(lt => lt.TestDate)
                   .IsRequired();

            builder.HasOne(lt => lt.Patient)
                   .WithMany()
                   .HasForeignKey(lt => lt.PatientId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(lt => lt.Appointment)
                   .WithMany(lt => lt.LabTests)
                   .HasForeignKey(lt => lt.AppointmentId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
