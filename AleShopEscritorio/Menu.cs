using AleShopEscritorio.Modelo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AleShopEscritorio
{
    public partial class Menu : Form
    {           
        public Menu(Usuario usuario)
        {
            InitializeComponent();
            lbl_email.Text = usuario.email.ToString();
            lbl_nombre.Text = usuario.nombre.ToString();
            
            lbl_id.Text = usuario.id_rol.ToString();
            Console.WriteLine("El valor de id_usuario es: " + lbl_id.Text);
            lbl_id.Hide();

            CultureInfo Español = new CultureInfo("es-ES");
            System.Threading.Thread.CurrentThread.CurrentCulture = Español;
            System.Threading.Thread.CurrentThread.CurrentUICulture = Español;

            int id = Convert.ToInt32(lbl_id.Text);

            if (id != 1002)
            {
                button2.Enabled = false;
            }
        }
       
        [DllImport("user32.dll", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int Iparam);

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(lbl_id.Text);
            ListaProductos productos = new ListaProductos(this,id);
            productos.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Main main = new Main();
            main.Show();
            this.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {           
            ListaUsuarios usuarios = new ListaUsuarios(this);
            usuarios.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Ventas venta = new Ventas(this);
            venta.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Historial historial = new Historial(this);
            historial.Show();
            this.Hide();
        }

        private void horafecha_Tick(object sender, EventArgs e)
        {
            lbl_hora.Text = DateTime.Now.ToLongTimeString();
            lbl_fecha.Text = DateTime.Now.ToLongDateString();
        }
    }
}
