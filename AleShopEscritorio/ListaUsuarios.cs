using AleShopEscritorio.Controlador;
using AleShopEscritorio.Modelo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AleShopEscritorio
{
    public partial class ListaUsuarios : Form
    {
        private Menu menuForm;       
        List<Usuario> usuarios;
        public ListaUsuarios(Menu menu)
        {
            InitializeComponent();          
            mostrarUsuarios();           
            menuForm = menu;
            
        }
        [DllImport("user32.dll", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int Iparam);
        private void mostrarUsuarios()
        {
            usuarios = UsuarioControlador.obtenerUsuarios();
            tablaUsuario.Rows.Clear();

            foreach (Usuario usuario in usuarios)
            {
                int rowIndex = tablaUsuario.Rows.Add();

                Rol rolUsuario = UsuarioControlador.obtenerRol(usuario.id_rol);
                

                tablaUsuario.Rows[rowIndex].Cells[0].Value = usuario.id.ToString();
                tablaUsuario.Rows[rowIndex].Cells[1].Value = usuario.email.ToString();
                tablaUsuario.Rows[rowIndex].Cells[2].Value = usuario.nombre.ToString();
                tablaUsuario.Rows[rowIndex].Cells[3].Value = usuario.apellido.ToString();
                tablaUsuario.Rows[rowIndex].Cells[4].Value = usuario.contrasenia.ToString();
                tablaUsuario.Rows[rowIndex].Cells[5].Value = rolUsuario.tipo;
                tablaUsuario.Rows[rowIndex].Cells[6].Value = "MODIFICAR";
              
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            menuForm.Show();
            this.Dispose();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            int id = int.Parse(tablaUsuario.Rows[e.RowIndex].Cells[0].Value.ToString());

            if (senderGrid.Columns[e.ColumnIndex].Name == "modificar" && e.RowIndex >= 0)
            {
                Usuario usuarioEditar = UsuarioControlador.obtenerUsuarioEditar(id);

                FormEditarUsuario formEditarUsuario = new FormEditarUsuario(usuarioEditar, this);

                DialogResult dialogResult = formEditarUsuario.ShowDialog();

                if (dialogResult == DialogResult.OK)
                {
                    Trace.WriteLine("OK");
                    mostrarUsuarios();
                }
                
            }

            
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
    }
}
