using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AleShopEscritorio.Modelo
{
    public class Categoria
    {
       public int id { get; set; }
       public string nombre { get; set; }

       public Categoria (int id, string nombre) { 
            this.id = id;
            this.nombre = nombre;
       }

        public Categoria()
        {
        }

        public override string ToString()
        {
            return nombre;
        }
    }
}
