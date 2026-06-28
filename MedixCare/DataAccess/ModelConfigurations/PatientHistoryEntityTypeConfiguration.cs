namespace MedixCare.DataAccess.ModelConfigurations
{
    public class PatientHistoryEntityTypeConfiguration : IEntityTypeConfiguration<PatientHistory>
    {
        public void Configure(EntityTypeBuilder<PatientHistory> builder)
        {
            builder.HasKey(ph => ph.Id);

            builder.Property(ph => ph.EntryDate)
                .IsRequired();

            builder.Property(ph => ph.Description)
                .IsRequired()
                .HasMaxLength(1000); 

            builder.Property(ph => ph.Notes)
                .HasMaxLength(2000); 

      
            builder.HasOne(ph => ph.Patient)
                .WithMany(p => p.PatientHistories)
                .HasForeignKey(ph => ph.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

           
            builder.HasOne(ph => ph.Doctor)
                .WithMany() 
                .HasForeignKey(ph => ph.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    
    }
}
