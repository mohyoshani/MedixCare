namespace MedixCare.ViewModels
{
    public class UpdateLabTestVM
    {

        public int Id { get; set; }
        public string TestName { get; set; } = string.Empty;

   
        public TestType testType { get; set; }

        public DateTime TestDate { get; set; }

        public string LabName { get; set; } = string.Empty;

        public string Summary { get; set; } = string.Empty;

        public List<LabTestAttachment>? ExistingAttachments { get; set; } 

        [Display(Name = "Upload New Test/Scan Documents")]
        public List<IFormFile>? TestFiles { get; set; } 


        public int AppointmentId { get; set; }
        public int PatientId { get; set; }

        //for readonly
        public string? PatientName { get; set; }
        public string? DoctorName { get; set; }
    }
}
