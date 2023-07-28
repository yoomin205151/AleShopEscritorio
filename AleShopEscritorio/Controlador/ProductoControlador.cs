using AleShopEscritorio.Modelo;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AleShopEscritorio.Controlador
{
    internal class ProductoControlador
    {
        public static List<Producto> obtenerProductos()
        {
            List<Producto> list = new List<Producto>();
            string query = "SELECT id, nombre, activo, stock, id_categoria, precio, " +
                           "CONVERT(NVARCHAR(MAX), img, 1) AS img_base64 " +
                           "FROM producto";

            SqlCommand cmd = new SqlCommand(query, DBControlador.conexion);

            try
            {
                DBControlador.conexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    {
                        string imgBase64 = reader.GetString(6);
                        Console.WriteLine("Valor de imgBase64: " + imgBase64); // Agregar esta línea para verificar el valor
                        list.Add(new Producto(reader.GetInt32(0), reader.GetString(1),
                                              reader.GetString(2), reader.GetInt32(3),
                                              reader.GetInt32(4), reader.GetInt32(5),
                                              imgBase64));
                    }
                    
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



        public static bool cambiarEstadoProducto(int id, string estado)
        {
            string query = "update producto set activo = @estado where id = @id;";

            SqlCommand cmd = new SqlCommand(query, DBControlador.conexion);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@estado", estado);

            try
            {
                DBControlador.conexion.Open();
                cmd.ExecuteNonQuery();
                DBControlador.conexion.Close();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Hay un error en la query: " + ex.Message);
            }
        }

        public static Categoria obtenerCategoria(int id)
        {
            Categoria categoria = new Categoria();
            string query = "select * from categoria where id = @id;";

            SqlCommand cmd = new SqlCommand(query, DBControlador.conexion);
            cmd.Parameters.AddWithValue("@id", id);

            try
            {
                DBControlador.conexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {

                    categoria = new Categoria(reader.GetInt32(0), reader.GetString(1));
                }
                reader.Close();
                DBControlador.conexion.Close();
                return categoria;

            }
            catch (Exception ex)
            {
                throw new Exception("Error en la query" + ex.Message);
            }
        }

        public static List<Producto> FiltrarProductosPorNombre(string textoBusqueda)
        {
            List<Producto> productosFiltrados = new List<Producto>();

            string query = "SELECT id, nombre, activo, stock, id_categoria, precio, " +
                           "CONVERT(NVARCHAR(MAX), img, 1) AS img_base64 " +
                           "FROM producto " +
                           "WHERE LOWER(nombre) LIKE @textoBusqueda";

            SqlCommand cmd = new SqlCommand(query, DBControlador.conexion);
            cmd.Parameters.AddWithValue("@textoBusqueda", "%" + textoBusqueda.ToLower() + "%");

            try
            {
                DBControlador.conexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string imgBase64 = reader.GetString(6);
                    productosFiltrados.Add(new Producto(reader.GetInt32(0), reader.GetString(1),
                                      reader.GetString(2), reader.GetInt32(3),
                                      reader.GetInt32(4), reader.GetInt32(5),
                                      imgBase64));
                }

                reader.Close();
                DBControlador.conexion.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Hay un error en la query: " + ex.Message);
            }

            return productosFiltrados;
        }

        public static List<Categoria> obtenerCategorias()
        {
            List<Categoria> list = new List<Categoria>();
            string query = "select * from categoria";

            SqlCommand cmd = new SqlCommand(query, DBControlador.conexion);

            try
            {
                DBControlador.conexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int id = reader.GetFieldValue<int?>(0) ?? 0;
                    string nombre = reader.GetFieldValue<string>(1);



                    list.Add(new Categoria(id, nombre));
                    Trace.WriteLine("se trajo correctamente la categoria" + reader.GetString(1));
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

        public bool CrearProducto(Producto producto)
        {
            string query = "insert into producto(nombre, activo, stock, id_categoria, precio, img) values(@nombre, @activo, @stock, @id_categoria, @precio, @img);";

            SqlCommand cmd = new SqlCommand(query, DBControlador.conexion);
            cmd.Parameters.AddWithValue("@nombre", producto.nombre);
            cmd.Parameters.AddWithValue("@activo", producto.activo);
            cmd.Parameters.AddWithValue("@stock", producto.stock);
            cmd.Parameters.AddWithValue("@id_categoria", producto.id_categoria);
            cmd.Parameters.AddWithValue("@precio", producto.precio);
            cmd.Parameters.AddWithValue("@img", producto.img);

            try
            {
                DBControlador.conexion.Open();
                cmd.ExecuteNonQuery();
                DBControlador.conexion.Close();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Hay un error en la query: " + ex.Message);
            }
        }

        public bool ModificarProducto(Producto producto)
        {
            string query = "update producto set nombre = @nombre, stock = @stock, id_categoria = @id_categoria, precio = @precio, img = @img  where id = @id;";


            SqlCommand cmd = new SqlCommand(query, DBControlador.conexion);
            cmd.Parameters.AddWithValue("@nombre", producto.nombre);
            cmd.Parameters.AddWithValue("@stock", producto.stock);
            cmd.Parameters.AddWithValue("@id_categoria", producto.id_categoria);
            cmd.Parameters.AddWithValue("@precio", producto.precio);
            cmd.Parameters.AddWithValue("@img", producto.img);
            cmd.Parameters.AddWithValue("@id", producto.id);


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

        public bool EliminarProducto(int idProducto)
        {
            string query = "DELETE FROM producto WHERE id = @idProducto;";
            SqlCommand cmd = new SqlCommand(query, DBControlador.conexion);
            cmd.Parameters.AddWithValue("@idProducto", idProducto);

            try
            {
                DBControlador.conexion.Open();
                cmd.ExecuteNonQuery();
                DBControlador.conexion.Close();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el producto: " + ex.Message);
            }
        }

        public static Producto ObtenerProductoPorId(int idProducto)
        {
            string query = "SELECT id, nombre, stock FROM producto WHERE id = @idProducto;";
            SqlCommand cmd = new SqlCommand(query, DBControlador.conexion);
            cmd.Parameters.AddWithValue("@idProducto", idProducto);

            try
            {
                DBControlador.conexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    Producto producto = new Producto
                    {
                        id = reader.GetInt32(0),
                        nombre = reader.GetString(1),
                        stock = reader.GetInt32(2)
                    };

                    reader.Close();
                    DBControlador.conexion.Close();
                    return producto;
                }
                else
                {
                    reader.Close();
                    DBControlador.conexion.Close();
                    return null; 
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Hay un error en la query: " + ex.Message);
            }
        }

        public static bool ActualizarStockProducto(int idProducto, int cantidadVendida)
        {
            string query = "UPDATE producto SET stock = stock - @cantidadVendida WHERE id = @idProducto;";
            SqlCommand cmd = new SqlCommand(query, DBControlador.conexion);
            cmd.Parameters.AddWithValue("@cantidadVendida", cantidadVendida);
            cmd.Parameters.AddWithValue("@idProducto", idProducto);

            try
            {
                DBControlador.conexion.Open();
                cmd.ExecuteNonQuery();
                DBControlador.conexion.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el stock del producto: " + ex.Message);
            }
        }

    }
}
