using AleShopEscritorio.Controlador;
using AleShopEscritorio.Modelo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AleShopEscritorio
{
    public partial class FormEditarUsuario : Form
    {
        private ListaUsuarios menuForm;
        List<Rol> roles;
        public FormEditarUsuario(Usuario usuarioEditar, ListaUsuarios listausuarios)
        {
            InitializeComponent();
            menuForm = listausuarios;
            lbl_id.Text = usuarioEditar.id.ToString();
            lbl_id.Hide();
            txt_apellido.Text = usuarioEditar.apellido.ToString();
            txt_contrasenia.Text = usuarioEditar.contrasenia.ToString();
            txt_email.Text = usuarioEditar.email.ToString();
            txt_nombre.Text = usuarioEditar.nombre.ToString();
            

            roles = UsuarioControlador.obtenerRoles();
            box_rol.Items.Clear();

            foreach (Rol rol in roles)
            {
                box_rol.Items.Add(rol);

            }

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            menuForm.Show();
            this.Dispose();
        }

        private void btn_modificar_Click(object sender, EventArgs e)
        {
            string nombre = txt_nombre.Text;
            string apellido = txt_apellido.Text;
            string email = txt_email.Text;
            string nombreRolSeleccionado = box_rol.SelectedItem.ToString();
            string contrasenia = txt_contrasenia.Text;
            int id = Convert.ToInt32(lbl_id.Text);

            int idRol = roles.Where(rol => rol.tipo == nombreRolSeleccionado).FirstOrDefault().id;

            Usuario usuario1 = new Usuario(id, nombre, apellido, email, contrasenia, idRol);

            if (UsuarioControlador.modificarUsuario(usuario1))
            {
                MessageBox.Show("Usuario modificado con éxito");
                menuForm.Show();
                this.Dispose();
            }
            else
            {
                MessageBox.Show("Error al modificar el usuario");
                menuForm.Show();
                this.Dispose();
            }
        }
    }
}