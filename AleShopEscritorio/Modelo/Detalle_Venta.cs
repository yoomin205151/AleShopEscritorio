using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AleShopEscritorio.Modelo
{
    public class Detalle_Venta
    {
        public int id { get; set; }
        public int id_venta { get; set; }
        public int id_producto { get; set; }
        public int cantidad { get; set; }
        public int precio { get; set; }

        public Detalle_Venta(int id_venta, int id_producto, int cantidad, int precio) {
            this.id_venta = id_venta;
            this.id_producto = id_producto;
            this.cantidad = cantidad;
            this.precio = precio;
        }
        public Detalle_Venta() { }
    }
}
