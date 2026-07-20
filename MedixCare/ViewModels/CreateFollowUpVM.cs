namespace MedixCare.ViewModels
{
    public class CreateFollowUpVM
    {
     
            public int? parentId { get; set; }

            public int patientId { get; set; }

            public string? PatientName { get; set; }

            public int doctorId { get; set; }
            public string? DoctorName { get; set; }

            public DateTime AppointmentDate { get; set; } = DateTime.UtcNow.AddDays(7);

        }
    }

