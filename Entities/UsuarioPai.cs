using Safety4Children.Repository.IdentityEntities;
using System.Collections.Generic;

namespace Safety4Children.Entities
{
    public class UsuarioPai : AppUser
    {
        public string Cpf { get; set; }

        public ICollection<UsuarioFilho> Filhos { get; set; }
    }
}
