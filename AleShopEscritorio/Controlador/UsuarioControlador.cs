using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AleShopEscritorio.Modelo;
using System.Diagnostics;

namespace AleShopEscritorio.Controlador
{
    public class UsuarioControlador
    {
        public bool Loguin(String email, String contrasenia)
        {
            string query = "select nombre,id_rol,email,contrasenia from usuario where email = @email and contrasenia = @contrasenia ";

            SqlCommand cmd = new SqlCommand(query, DBControlador.conexion);

            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@contrasenia", contrasenia);

            try
            {
                DBControlador.conexion.Open();
                cmd.ExecuteNonQuery();

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                if (dt.Rows.Count == 1)
                {
                    Usuario usuario = new Usuario
                    {
                        id_rol = Convert.ToInt32(dt.Rows[0]["id_rol"]),
                        email = dt.Rows[0]["email"].ToString(),
                        nombre = dt.Rows[0]["nombre"].ToString(),
                        // Asigna más propiedades si es necesario
                    };
                    //MessageBox.Show("Logueado correctamente");

                    Menu UsuarioLogueado = new Menu(usuario);
                    UsuarioLogueado.Show();

                }
                else
                {
                    MessageBox.Show("Usuario y/o Contrasenia incorrecta");
                    Main main = new Main();
                    main.Show();

                }
                DBControlador.conexion.Close();
                return true;

            }
            catch (Exception ex)
            {
                throw new Exception("Hay un error en la query: " + ex.Message);
            }
        }

        public static List<Usuario> obtenerUsuarios()
        {
            List<Usuario> list = new List<Usuario>();
            string query = "select * from usuario";

            SqlCommand cmd = new SqlCommand(query, DBControlador.conexion);

            try
            {
                DBControlador.conexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new Usuario(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4),
                        reader.GetInt32(5)));
                }
                reader.Close();
                DBControlador.conexion.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Hay un error en la query: " + ex.Message);
            }

            return list;
        }

        public static List<Usuario> obtenerUsuarioClientes()
        {
            List<Usuario> list = new List<Usuario>();
            string query = "select * from usuario where id_rol = 2";

            SqlCommand cmd = new SqlCommand(query, DBControlador.conexion);

            try
            {
                DBControlador.conexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new Usuario(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4),
                        reader.GetInt32(5)));
                }
                reader.Close();
                DBControlador.conexion.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Hay un error en la query: " + ex.Message);
            }

            return list;
        }

        public static Usuario obtenerUsuarioEditar(int id)
        {
            Usuario usuario = new Usuario();
            string query = "select * from usuario where id = @id;";

            SqlCommand cmd = new SqlCommand(query, DBControlador.conexion);
            cmd.Parameters.AddWithValue("@id", id);

            try
            {
                DBControlador.conexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {

                    usuario = new Usuario(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4),
                        reader.GetInt32(5));
                }
                reader.Close();
                DBControlador.conexion.Close();
                return usuario;

            }
            catch (Exception ex)
            {
                throw new Exception("Error en la query" + ex.Message);
            }
        }

        public static bool modificarUsuario(Usuario usuario)
        {
            string query = "update usuario set nombre = @nombre, apellido = @apellido, email = @email, contrasenia = @contrasenia , id_rol = @id_rol  where id = @id;";


            SqlCommand cmd = new SqlCommand(query, DBControlador.conexion);
            cmd.Parameters.AddWithValue("@nombre", usuario.nombre);
            cmd.Parameters.AddWithValue("@apellido", usuario.apellido);
            cmd.Parameters.AddWithValue("@email", usuario.email);
            cmd.Parameters.AddWithValue("@contrasenia", usuario.contrasenia);
            cmd.Parameters.AddWithValue("@id_rol", usuario.id_rol);
            cmd.Parameters.AddWithValue("@id", usuario.id);


            try
            {
                DBControlador.conexion.Open();
                cmd.ExecuteNonQuery();
                DBControlador.conexion.Close();
                //MessageBox.Show("Modificado Correctamente");
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Hay un error en la query: " + ex.Message);
            }
        }

        public static List<Rol> obtenerRoles()
        {
            List<Rol> list = new List<Rol>();
            string query = "select * from rol";

            SqlCommand cmd = new SqlCommand(query, DBControlador.conexion);

            try
            {
                DBControlador.conexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int id = reader.GetFieldValue<int?>(0) ?? 0;
                    string tipo = reader.GetFieldValue<string>(1);
                    


                    list.Add(new Rol(id, tipo));
                    Trace.WriteLine("se trajo correctamente el rol" + reader.GetString(1));
                }

                reader.Close();
                DBControlador.conexion.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Hay un error en la query: " + ex.Message);
            }

            return list;
        }

        public static Rol obtenerRol(int id)
        {
            Rol rol = new Rol();
            string query = "select * from rol where id = @id;";

            SqlCommand cmd = new SqlCommand(query, DBControlador.conexion);
            cmd.Parameters.AddWithValue("@id", id);

            try
            {
                DBControlador.conexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {

                    rol = new Rol(reader.GetInt32(0), reader.GetString(1));
                }
                reader.Close();
                DBControlador.conexion.Close();
                return rol;

            }
            catch (Exception ex)
            {
                throw new Exception("Error en la query" + ex.Message);
            }
        }

        public static Usuario obtenerCliente(int id)
        {
            Usuario usuario = new Usuario();
            string query = "select nombre,apellido from usuario where id = @id;";

            SqlCommand cmd = new SqlCommand(query, DBControlador.conexion);
            cmd.Parameters.AddWithValue("@id", id);

            try
            {
                DBControlador.conexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {

                    usuario = new Usuario(reader.GetString(0), reader.GetString(1));
                }
                reader.Close();
                DBControlador.conexion.Close();
                return usuario;

            }
            catch (Exception ex)
            {
                throw new Exception("Error en la query" + ex.Message);
            }
        }

        public static List<Usuario> FiltrarUsuario(string textoBusqueda)
        {
            List<Usuario> UsuarioFiltrados = new List<Usuario>();

            string query = "SELECT * " +
                           "FROM usuario " +
                           "WHERE LOWER(nombre) LIKE @textoBusqueda OR LOWER(apellido) LIKE @textoBusqueda";

            SqlCommand cmd = new SqlCommand(query, DBControlador.conexion);
            cmd.Parameters.AddWithValue("@textoBusqueda", "%" + textoBusqueda.ToLower() + "%");

            try
            {
                DBControlador.conexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    UsuarioFiltrados.Add(new Usuario(reader.GetInt32(0), reader.GetString(1),
                                      reader.GetString(2), reader.GetString(3),
                                      reader.GetString(4), reader.GetInt32(5)));
                }

                reader.Close();
                DBControlador.conexion.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Hay un error en la query: " + ex.Message);
            }

            return UsuarioFiltrados;
        }

    }
}
