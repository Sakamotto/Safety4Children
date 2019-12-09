using Safety4Children.Repository.IdentityEntities;

namespace Safety4Children.Entities
{
    public class UsuarioFilho : AppUser
    {
        public char Sexo { get; set; }

        public int Idade { get; set; }

        public int UsuarioPaiId { get; set; }

        public virtual UsuarioPai UsuarioPai { get; set; }
    }
}
