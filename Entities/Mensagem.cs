using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Safety4Children.Entities
{
    public class Mensagem
    {
        public string Texto { get; set; }

        public DateTime DataEnvio { get; set; }

        public string Origem { get; set; }

        public string Destino { get; set; }
    }
}
