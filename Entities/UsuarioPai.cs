using Safety4Children.Repository.IdentityEntities;

namespace Safety4Children.Entities
{
    public class UsuarioPai : AppUser
    {
        public string Nome { get; set; }
        public string Cpf { get; set; }
    }
}
