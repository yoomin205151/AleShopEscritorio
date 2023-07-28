using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AleShopEscritorio.Controlador
{
    public class DBControlador
    {
        private static string conexString = "";
        public static SqlConnection conexion;

        public static void iniciarConexion()
        {
            var builder = new SqlConnectionStringBuilder();

            builder.DataSource = "(localdb)\\ALEJANDRO";
            builder.InitialCatalog = "ALESHOPWEB";
            builder.IntegratedSecurity = true;

            conexString = builder.ToString();
            conexion = new SqlConnection(conexString);

        }

        public static void cerrarConexion()
        {
            conexion.Close();
        }
    }
}
