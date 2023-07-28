using AleShopEscritorio.Controlador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AleShopEscritorio
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            DBControlador.iniciarConexion();
           // if (validarConexion())
            //{
               // MessageBox.Show("Se conecto correctamenet a la base de datos");
           // }

            Application.Run(new Main());

            
        }
        public static bool validarConexion()
        {
            try
            {
                DBControlador.conexion.Open();
                DBControlador.conexion.Close();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar a la base de datos" + ex.Message);
                return false;
            }
        }
    }
}
