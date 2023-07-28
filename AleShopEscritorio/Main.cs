using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using AleShopEscritorio.Controlador;
using AleShopEscritorio.Modelo;

namespace AleShopEscritorio
{
    public partial class Main : Form
    {
        UsuarioControlador usuarioControlador = new UsuarioControlador();
        public Main()
        {           
            InitializeComponent();
            this.KeyPreview = true;
            this.KeyPress += txt_contrasenia_KeyPress;
            this.KeyPress += txt_usuario_KeyPress;
        }
        [DllImport("user32.dll", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int Iparam);
        private void txt_usuario_Enter(object sender, EventArgs e)
        {
            if (txt_usuario.Text == "USUARIO")
            {
                txt_usuario.Text = "";
                txt_usuario.ForeColor = Color.LightGray;
            }
        }

        private void txt_usuario_Leave(object sender, EventArgs e)
        {
            if (txt_usuario.Text == "")
            {
                txt_usuario.Text = "USUARIO";
                txt_usuario.ForeColor = Color.DimGray;
            }
        }

        private void txt_contrasenia_Enter(object sender, EventArgs e)
        {
            if(txt_contrasenia.Text == "CONTRASENIA")
            {
                txt_contrasenia.Text = "";
                txt_contrasenia.ForeColor = Color.LightGray;
                txt_contrasenia.UseSystemPasswordChar = true;
            }
        }

        private void txt_contrasenia_Leave(object sender, EventArgs e)
        {
            if (txt_contrasenia.Text == "")
            {
                txt_contrasenia.Text = "CONTRASENIA";
                txt_contrasenia.ForeColor = Color.DimGray;
                txt_contrasenia.UseSystemPasswordChar = false;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void Main_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            String email = txt_usuario.Text;
            String contrasenia = txt_contrasenia.Text;
           
            usuarioControlador.Loguin(email, contrasenia);
            this.Hide();
            
        }

        private void txt_usuario_TextChanged(object sender, EventArgs e)
        {

        }

        private void btn_login_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void btn_login_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txt_contrasenia_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                btn_login.PerformClick();
            }
        }

        private void txt_usuario_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                btn_login.PerformClick();
            }
        }
    }
}
