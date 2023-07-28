using AleShopEscritorio.Modelo;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AleShopEscritorio.Controlador
{
    internal class CarritoControlador
    {
        public bool AgregarProductoCarrito(int id_prod, Carrito carrito)
        {
            // Verificar si el producto ya existe en el carrito
            string checkQuery = "SELECT COUNT(*) FROM carrito_pc WHERE id_producto = @id_producto;";
            SqlCommand checkCmd = new SqlCommand(checkQuery, DBControlador.conexion);
            checkCmd.Parameters.AddWithValue("@id_producto", id_prod);

            try
            {
                DBControlador.conexion.Open();
                int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                DBControlador.conexion.Close();

                if (count > 0)
                {
                    // El producto ya existe en el carrito, así que actualiza la cantidad
                    string updateQuery = "UPDATE carrito_pc SET cantidad = cantidad + @cantidad WHERE id_producto = @id_producto;";
                    SqlCommand updateCmd = new SqlCommand(updateQuery, DBControlador.conexion);
                    updateCmd.Parameters.AddWithValue("@id_producto", id_prod);
                    updateCmd.Parameters.AddWithValue("@cantidad", carrito.cantidad);

                    DBControlador.conexion.Open();
                    updateCmd.ExecuteNonQuery();
                    DBControlador.conexion.Close();
                }
                else
                {
                    // El producto no existe en el carrito, así que agrégalo
                    string insertQuery = "INSERT INTO carrito_pc (id_producto, cantidad, preciounitario) VALUES (@id_producto, @cantidad, @preciounitario);";
                    SqlCommand insertCmd = new SqlCommand(insertQuery, DBControlador.conexion);
                    insertCmd.Parameters.AddWithValue("@id_producto", id_prod);
                    insertCmd.Parameters.AddWithValue("@cantidad", carrito.cantidad);
                    insertCmd.Parameters.AddWithValue("@preciounitario", carrito.preciounitraio);

                    DBControlador.conexion.Open();
                    insertCmd.ExecuteNonQuery();
                    DBControlador.conexion.Close();
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Hay un error en la query: " + ex.Message);
            }
        }


        public static List<Carrito> obtenerListaCarrito()
        {
            List<Carrito> list = new List<Carrito>();
            string query = "select id_producto, cantidad, preciounitario from carrito_pc";

            SqlCommand cmd = new SqlCommand(query, DBControlador.conexion);

            try
            {
                DBControlador.conexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    {
                                              
                        list.Add(new Carrito(reader.GetInt32(0), 
                                             reader.GetInt32(1),
                                             reader.GetInt32(2) 
                                             ));
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

        public bool EliminarProductoCarrito(int idProducto)
        {
            string query = "delete from carrito_pc where id_producto = @id_producto;";
            SqlCommand cmd = new SqlCommand(query, DBControlador.conexion);
            cmd.Parameters.AddWithValue("@id_producto", idProducto);

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

        public bool ActualizarCantidadCarrito(int idProducto, int nuevaCantidad)
        {
            string query = "UPDATE carrito_pc SET cantidad = @nuevaCantidad WHERE id_producto = @idProducto;";
            SqlCommand cmd = new SqlCommand(query, DBControlador.conexion);
            cmd.Parameters.AddWithValue("@nuevaCantidad", nuevaCantidad);
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
                throw new Exception("Error al actualizar la cantidad del producto en el carrito: " + ex.Message);
            }
        }

        public static Producto ObtenerProductoPorId(int idProducto)
        {
            Producto prod = new Producto();
            string query = "SELECT id, nombre, stock FROM producto WHERE id = @idProducto;";
            SqlCommand cmd = new SqlCommand(query, DBControlador.conexion);
            cmd.Parameters.AddWithValue("@idProducto", idProducto);

            try
            {
                DBControlador.conexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    prod = new Producto(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2));
                }
                reader.Close();
                DBControlador.conexion.Close();
                return prod;

            }
            catch (Exception ex)
            {
                throw new Exception("Error en la query" + ex.Message);
            }
        }

        public static bool EliminarCarritoSinUsuario()
        {
            string query = "DELETE FROM carrito_pc ";
            SqlCommand cmd = new SqlCommand(query, DBControlador.conexion);

            try
            {
                DBControlador.conexion.Open();
                cmd.ExecuteNonQuery();
                DBControlador.conexion.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar los productos del carrito sin usuario: " + ex.Message);
            }
        }
    }
}
