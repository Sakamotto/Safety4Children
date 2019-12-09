using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Safety4Children.Entities
{
    public class Cerca
    {
        public int Latitude { get; set; }

        public int Longitude { get; set; }

        public int Raio { get; set; }

        public TimeSpan HorarioInicio { get; set; }

        public TimeSpan HorarioTermino { get; set; }
    }
}
