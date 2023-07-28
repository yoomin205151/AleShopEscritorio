using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AleShopEscritorio.Modelo
{
    public class Venta
    {
        public int id {  get; set; }
        public DateTime fecha { get; set; }
        public int total { get; set; }
        public int id_origen  {get; set; }
        public int id_usuario { get; set; }

        public Venta (int id, DateTime fecha, int total, int id_origen, int id_usuario)
        {
            this.id = id;
            this.fecha = fecha;
            this.total = total;
            this.id_origen = id_origen;
            this.id_usuario = id_usuario;
        }
        public Venta(int id, DateTime fecha, int total,int id_usuario)
        {
            this.id = id;
            this.fecha = fecha;
            this.total = total;
            this.id_usuario = id_usuario;
        }
        public Venta(DateTime fecha, int total, int id_origen, int id_usuario)
        {            
            this.fecha = fecha;
            this.total = total;
            this.id_origen = id_origen;
            this.id_usuario = id_usuario;
        }

        public Venta(int total, int id_usuario)
        {
            this.total = total;
            this.id_usuario = id_usuario;
        }
        public Venta () { }
    }
}
