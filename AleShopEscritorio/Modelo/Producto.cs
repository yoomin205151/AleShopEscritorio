using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AleShopEscritorio.Modelo
{
    public class Producto
    {
        public int id {  get; set; }
        public string nombre { get; set; }
        public string activo { get; set; }
        public int stock { get; set; }
        public int id_categoria { get; set; }
        public int precio { get; set; }
        public string img { get; set; }
        public string ImagenBase64 { get; set; }

        public Producto(int id, string nombre, string activo, int stock, int id_categoria, int precio, string img_base64)
        {
            this.id = id;
            this.nombre = nombre;
            this.activo = activo;
            this.stock = stock;
            this.id_categoria = id_categoria;
            this.precio = precio;
            this.ImagenBase64 = img_base64; // Cambiar "img" a "ImagenBase64"
        }
        
        public Producto(string nombre, string activo, int stock, int id_categoria, int precio, string img)
        {            
            this.nombre = nombre;
            this.activo = activo;
            this.stock = stock;
            this.id_categoria = id_categoria;
            this.precio = precio;
            this.img = img;
        }

        public Producto(string nombre, int stock, int id_categoria, int precio, string img)
        {
            this.nombre = nombre;
            this.stock = stock;
            this.id_categoria = id_categoria;
            this.precio = precio;
            this.img = img;
        }

        public Producto(int id,string nombre, int stock, int id_categoria, int precio, string img)
        {
            this.id = id;
            this.nombre = nombre;
            this.stock = stock;
            this.id_categoria = id_categoria;
            this.precio = precio;
            this.img = img;
        }
        public Producto () { }
        public Producto(int id, string nombre, int stock)
        {
            this.id = id;
            this.nombre = nombre;
 
        }

        public override string ToString()
        {
            return nombre;
        }
    }
}
