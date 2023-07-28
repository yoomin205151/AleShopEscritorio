using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AleShopEscritorio.Modelo
{
    public class Rol
    {
        public int id {  get; set; }
        public string tipo { get; set; }

        public Rol(int id, string tipo) { 
            this.id = id;
            this.tipo = tipo;
        }
        public Rol(string tipo)
        {
            this.tipo = tipo;
        }

        public Rol(){}

        public override string ToString()
        {
            return tipo;
        }
    }
}
