using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Safety4Children.Repository.IdentityEntities
{
    public class Role : IdentityRole<int>
    {
        public List<UserRole> UserRoles { get; set; }
    }
}
