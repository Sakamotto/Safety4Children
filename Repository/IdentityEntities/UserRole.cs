using Microsoft.AspNetCore.Identity;

namespace Safety4Children.Repository.IdentityEntities
{
    public class UserRole : IdentityUserRole<int>
    {
        public AppUser User { get; set; }
        public Role Role { get; set; }
    }
}
