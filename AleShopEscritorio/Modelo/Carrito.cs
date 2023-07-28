using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AleShopEscritorio.Modelo
{
    public class Carrito
    {
        public int id_producto { get; set;}
        public int cantidad { get; set;}       
        public int preciounitraio { get; set;}
        public int id {get; set;}

        public Carrito() { }

        public Carrito(int cantidad, int preciounitario)
        {
            this.cantidad = cantidad;
            this.preciounitraio = preciounitario;
        }

        public Carrito(int id_producto,int cantidad, int preciounitario)
        {
            this.id_producto= id_producto;
            this.cantidad = cantidad;
            this.preciounitraio = preciounitario;
        }
    }
}
