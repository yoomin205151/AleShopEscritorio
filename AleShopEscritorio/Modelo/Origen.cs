using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AleShopEscritorio.Modelo
{
    public class Origen
    {
        public int id { get; set; }
        public string tipo { get; set; }

        public Origen(int id, string tipo)
        {
            this.id = id;
            this.tipo = tipo;
        }
    }
}
