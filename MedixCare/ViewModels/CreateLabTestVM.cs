using Microsoft.AspNetCore.Mvc.Rendering;

namespace MedixCare.ViewModels
{
    public class CreateLabTestVM
    {
  
        public string TestName { get; set; } = string.Empty;

        public TestType testType { get; set; }


        [DataType(DataType.DateTime)]
        public DateTime TestDate { get; set; } = DateTime.UtcNow;

   
        public string LabName { get; set; } = string.Empty;

        [DataType(DataType.MultilineText)]
        public string Summary { get; set; } = string.Empty;

        public int AppointmentId { get; set; }

    
        public int PatientId { get; set; }

    }
}
