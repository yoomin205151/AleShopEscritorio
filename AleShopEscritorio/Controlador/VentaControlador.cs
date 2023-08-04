using AleShopEscritorio.Modelo;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AleShopEscritorio.Controlador
{
    internal class VentaControlador
    {
        public static int CrearVenta(Venta venta)
        {
            venta.fecha = DateTime.Now;

            string query = "INSERT INTO venta (fecha, total, id_origen, id_usuario) VALUES (@fecha, @total, 2, @id_usuario); SELECT SCOPE_IDENTITY();";

            SqlCommand cmd = new SqlCommand(query, DBControlador.conexion);
            cmd.Parameters.AddWithValue("@fecha", venta.fecha);
            cmd.Parameters.AddWithValue("@total", venta.total);
            cmd.Parameters.AddWithValue("@id_usuario", venta.id_usuario);

            try
            {
                DBControlador.conexion.Open();
                // Ejecutamos el comando y recuperamos el ID de la venta insertada
                int idVentaInsertada = Convert.ToInt32(cmd.ExecuteScalar());
                DBControlador.conexion.Close();
                return idVentaInsertada;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar la venta en la base de datos: " + ex.Message);
            }
        }

        public static bool CrearDetalleVenta(Detalle_Venta venta)
        {
            string query = "INSERT INTO detalle_venta (id_venta,id_producto, cantidad, precio) VALUES (@id_venta, @id_producto,@cantidad, @precio);";

            SqlCommand cmd = new SqlCommand(query, DBControlador.conexion);
            cmd.Parameters.AddWithValue("@id_venta", venta.id_venta);
            cmd.Parameters.AddWithValue("@id_producto", venta.id_producto);
            cmd.Parameters.AddWithValue("@cantidad", venta.cantidad);
            cmd.Parameters.AddWithValue("@precio", venta.precio);

            try
            {
                DBControlador.conexion.Open();
                // Ejecutamos el comando y recuperamos el ID de la venta insertada
                cmd.ExecuteNonQuery();
                DBControlador.conexion.Close();
                return true;
            }
            catch (Exception ex)
            {                               
                throw new Exception("Error al insertar el detalle_venta en la base de datos: " + ex.Message);
            }
        }

        public static List<Venta> obtenerVentas()
        {
            List<Venta> list = new List<Venta>();
            string query = "select id, fecha, total, id_usuario from venta where id_origen = 2 ORDER BY fecha DESC";

            SqlCommand cmd = new SqlCommand(query, DBControlador.conexion);

            try
            {
                DBControlador.conexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    DateTime fecha = reader.GetDateTime(1); 
                    int total = reader.GetInt32(2);
                    int idUsuario = reader.GetInt32(3);

                    list.Add(new Venta(id, fecha, total, idUsuario));
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

        

    }
}
