using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Safety4Children.Repository.IdentityEntities
{
    public class AppUser : IdentityUser<int>
    {
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        [NotMapped]
        public string NomeCompleto {
            get
            {
                return $"{Nome} {Sobrenome}";
            }
        }
        public string UrlAvatar { get; set; }
        public List<UserRole> UserRoles { get; set; }
    }
}
