using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AleShopEscritorio.Modelo
{
    public class Usuario
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string email { get; set; }
        public string contrasenia { get; set; }
        public int id_rol { get; set; }

        public Usuario(int id, string nombre, string apellido, string email, string contrasenia , int id_rol) {
            this.id = id;
            this.nombre = nombre;
            this.apellido = apellido;
            this.email = email;
            this.contrasenia = contrasenia;
            this.id_rol = id_rol;

        }
        public Usuario(string nombre, string apellido, string email, string contrasenia, int id_rol)
        {
            this.nombre = nombre;
            this.apellido = apellido;
            this.email = email;
            this.contrasenia = contrasenia;
            this.id_rol = id_rol;

        }

        public Usuario(string nombre)
        {
            this.nombre = nombre;

        }
        public Usuario(string nombre, string apellido)
        {
            this.nombre = nombre;
            this.apellido= apellido;
        }
        public Usuario() { }

        public override string ToString()
        {
            return nombre;
        }
    }
}
