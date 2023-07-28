using AleShopEscritorio.Controlador;
using AleShopEscritorio.Modelo;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AleShopEscritorio
{
    public partial class Historial : Form
    {
        private Menu menuForm;
        List<Venta> ventas;
        public Historial(Menu menu)
        {
            InitializeComponent();
            mostrarVentas();
            menuForm = menu;
        }
        [DllImport("user32.dll", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int Iparam);
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void mostrarVentas()
        {
            ventas = VentaControlador.obtenerVentas();
            tablahitorialventa.Rows.Clear();

            foreach (Venta venta in ventas)
            {
                int rowIndex = tablahitorialventa.Rows.Add();

                Usuario idUsuario = UsuarioControlador.obtenerCliente(venta.id_usuario);


                tablahitorialventa.Rows[rowIndex].Cells[0].Value = venta.id.ToString();
                tablahitorialventa.Rows[rowIndex].Cells[1].Value = venta.fecha.ToString();
                tablahitorialventa.Rows[rowIndex].Cells[2].Value = idUsuario.nombre.ToString();
                tablahitorialventa.Rows[rowIndex].Cells[3].Value = idUsuario.apellido.ToString();
                tablahitorialventa.Rows[rowIndex].Cells[4].Value = venta.total.ToString();
                tablahitorialventa.Rows[rowIndex].Cells[5].Value = "DETALLE";

            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            menuForm.Show();
            this.Dispose();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void tablahitorialventa_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex].Name == "detalle" && e.RowIndex >= 0)
            {
                int idVenta = Convert.ToInt32(tablahitorialventa.Rows[e.RowIndex].Cells[0].Value);
                string rutaGuardadoPDF = @"D:\DavinciClases\4tocuatrimestre\plataformadesarrollo\PDF\" + idVenta + ".pdf";

                if (File.Exists(rutaGuardadoPDF))
                {
                    
                    Process.Start(rutaGuardadoPDF);
                }
                else
                {
                    MessageBox.Show("El archivo PDF no existe para esta venta.", "Archivo no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void txt_boxfiltro_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void txt_buscador_TextChanged(object sender, EventArgs e)
        {
            string textoBusqueda = txt_buscador.Text.ToLower();

            //List<Usuario> usuarioFiltrados = UsuarioControlador.FiltrarUsuario(textoBusqueda);
            ventas = VentaControlador.obtenerVentas();

            tablahitorialventa.Rows.Clear();

            foreach (Venta venta in ventas)
            {
                Usuario idUsuario = UsuarioControlador.obtenerCliente(venta.id_usuario);

                // Filtrar por nombre y apellido del usuario
                if (idUsuario.nombre.ToLower().Contains(textoBusqueda) || idUsuario.apellido.ToLower().Contains(textoBusqueda))
                {
                    int rowIndex = tablahitorialventa.Rows.Add();
                    

                    tablahitorialventa.Rows[rowIndex].Cells[0].Value = venta.id.ToString();
                    tablahitorialventa.Rows[rowIndex].Cells[1].Value = venta.fecha.ToString();
                    tablahitorialventa.Rows[rowIndex].Cells[2].Value = idUsuario.nombre.ToString();
                    tablahitorialventa.Rows[rowIndex].Cells[3].Value = idUsuario.apellido.ToString();
                    tablahitorialventa.Rows[rowIndex].Cells[4].Value = venta.total.ToString();
                    tablahitorialventa.Rows[rowIndex].Cells[5].Value = "DETALLE";
                }
            }
        }
    }
}
