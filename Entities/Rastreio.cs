using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Safety4Children.Entities
{
    public class Rastreio
    {
        public UsuarioFilho Filho { get; set; }

        public DateTime DataUltimaLocalizacao { get; set; }

        public int LatitudeAtual { get; set; }

        public int LongitudeAtual { get; set; }
    }
}
