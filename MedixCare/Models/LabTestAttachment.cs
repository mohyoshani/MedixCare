namespace MedixCare.Models
{
    public class LabTestAttachment
    {
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public int LabTestId { get; set; }
        public LabTest? LabTest { get; set; }
    }
}
