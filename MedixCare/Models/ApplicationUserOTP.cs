namespace MedixCare.Models
{
    public class ApplicationUserOTP
    {
        public int Id { get; set; }
        [Required]
        public string OTP { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpireIn { get; set; } = DateTime.UtcNow.AddMinutes(60);

        public bool IsUsed { get; set; } = false;
        public string ApplicationUserId { get; set; } = string.Empty;
        public ApplicationUser ApplicationUser { get; set; } = null!;
        public bool IsValid => (ExpireIn - DateTime.UtcNow).TotalMinutes > 0 && !IsUsed;
    }
}
