namespace MedixCare.DataAccess.ModelConfigurations
{
    public class PrescriptionItemsEntityTypeConfiguration : IEntityTypeConfiguration<PrescriptionItem>
    {
        public void Configure(EntityTypeBuilder<PrescriptionItem> builder)
        {
            builder.HasKey(pi => pi.Id);

            builder.Property(pi => pi.MedicineName)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(pi => pi.Dosage)
                .IsRequired()
                .HasMaxLength(100); 

            builder.Property(pi => pi.MedicineFrequency)
                .IsRequired(); 

            builder.Property(pi => pi.DurationInDays)
                .IsRequired(); 

           
            builder.HasOne(pi => pi.Prescription)
                .WithMany(p => p.PrescriptionItems)
                .HasForeignKey(pi => pi.PrescriptionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    
    }
}
