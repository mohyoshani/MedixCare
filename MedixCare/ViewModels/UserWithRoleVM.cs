namespace MedixCare.ViewModels
{
    public class UserWithRoleVM
    {
        public string Id { get; set; } = null!;
        public ApplicationUser ApplicationUser { get; set; } = null!;
        public string RoleName { get; set; } = null!;
        public IEnumerable<IdentityRole> IdentityRoles { get; set; } = [];
    }
}
