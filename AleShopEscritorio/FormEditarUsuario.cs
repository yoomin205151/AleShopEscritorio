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

        private bool EsFormatoEmailValido(string email)
        {
            // Expresión regular para validar el formato del email
            string patronEmail = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            // Comprobar si el email coincide con el patrón de la expresión regular
            return System.Text.RegularExpressions.Regex.IsMatch(email, patronEmail);
        }

        private void btn_modificar_Click(object sender, EventArgs e)
        {
            string nombre = txt_nombre.Text;
            string apellido = txt_apellido.Text;
            string email = txt_email.Text;
            string nombreRolSeleccionado = box_rol.SelectedItem?.ToString();
            string contrasenia = txt_contrasenia.Text;
            int id = Convert.ToInt32(lbl_id.Text);

            if (string.IsNullOrEmpty(nombreRolSeleccionado))
            {
                MessageBox.Show("Debe seleccionar un rol.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int idRol = roles.Where(rol => rol.tipo == nombreRolSeleccionado).FirstOrDefault().id;

            if (!(nombre.Length >=4 && nombre.Length <= 20))
            {
                MessageBox.Show("El nombre debe tener entre 4 y 20 caracteres.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!(apellido.Length >= 4 && nombre.Length <= 20))
            {
                MessageBox.Show("El nombre debe tener entre 4 y 20 caracteres.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!EsFormatoEmailValido(email))
            {
                MessageBox.Show("El email no tiene un formato válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!(contrasenia.Length >= 4 && nombre.Length <= 20))
            {
                MessageBox.Show("La contrasenia debe tener entre 4 y 20 caracteres.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


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