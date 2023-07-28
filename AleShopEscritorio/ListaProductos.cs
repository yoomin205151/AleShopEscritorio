using AleShopEscritorio.Controlador;
using AleShopEscritorio.Modelo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
    
    public partial class ListaProductos : Form
    {
        Image File;
        private Menu menuForm;
        List<Producto> productos;
        List<Categoria> categorias;
        private Ventas FormProd;
        public ListaProductos(Menu menu, int id)
        {
            InitializeComponent();
            menuForm = menu;
            
            mostrarProductos();
            categorias = ProductoControlador.obtenerCategorias();
            box_categoria.Items.Clear();
            lblidprod.Hide();
            //id para usarlo en permisos en los botones del abm producto
            int id_usuario = id;
            Console.WriteLine("El valor de id_usuario es: " + id_usuario);
            if (id_usuario == 1002)
            {
                btn_modificar.Enabled = true;
                button1.Enabled = true;
                btnmodificar.Enabled = true;
                btneliminar.Enabled = true;
            }
            else
            {
                btn_modificar.Enabled = false;
                button1.Enabled = false;
                btnmodificar.Enabled = false;
                btneliminar.Enabled = false;
            }
            foreach (Categoria categoria in categorias)
            {
                box_categoria.Items.Add(categoria.nombre);
            }                            
        }
        [DllImport("user32.dll", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int Iparam);
        public ListaProductos(Ventas formprod)
        {
            InitializeComponent();
            
            FormProd = formprod;
            mostrarProductos();
            categorias = ProductoControlador.obtenerCategorias();
            box_categoria.Items.Clear();
            lblidprod.Hide();
            //id para usarlo en permisos en los botones del abm producto
            

            foreach (Categoria categoria in categorias)
            {
                box_categoria.Items.Add(categoria.nombre);

            }
        }

        private void mostrarProductos()
        {
            productos = ProductoControlador.obtenerProductos();
            tablaProductos.Rows.Clear();

            foreach (Producto producto in productos)
            {
                int rowIndex = tablaProductos.Rows.Add();
                Categoria categoriaProducto = ProductoControlador.obtenerCategoria(producto.id_categoria);

                tablaProductos.Rows[rowIndex].Height = 150;
               

                Image image;
                // Se toma el valor de la propiedad ImagenBase64 del objeto producto,
                // que debe ser una cadena en formato base64 que representa la imagen.
                // Luego, se utiliza el método estático Convert.FromBase64String()
                // para convertir esa cadena en un array de bytes (byte[]), lo cual
                // es necesario para poder trabajar con la imagen.
                byte[] imageBytes = Convert.FromBase64String(producto.ImagenBase64);
                
                //Se utiliza la sentencia using con un MemoryStream para poder crear un flujo
                //de memoria (MemoryStream) a partir del array de bytes imageBytes.
                //El MemoryStream actúa como un contenedor temporal para los datos de la imagen.
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    image = Image.FromStream(ms);
                }
                //Utilizando el método estático Image.FromStream(),
                //se convierten los datos del MemoryStream en un objeto Image. 

                tablaProductos.Rows[rowIndex].Cells[0].Value = producto.id.ToString();
                tablaProductos.Rows[rowIndex].Cells[1].Value = image;
                tablaProductos.Rows[rowIndex].Cells[2].Value = producto.nombre.ToString();
                tablaProductos.Rows[rowIndex].Cells[3].Value = producto.precio.ToString();
                tablaProductos.Rows[rowIndex].Cells[4].Value = producto.stock.ToString();
                tablaProductos.Rows[rowIndex].Cells[5].Value = categoriaProducto.nombre;
                tablaProductos.Rows[rowIndex].Cells[6].Value = producto.activo;
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
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "PNG files (*.png)|*.png";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                File = Image.FromFile(ofd.FileName);
                pictureBox.Image = File;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 6 && e.RowIndex >= 0)
            {
                
                string estadoActual = tablaProductos.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                
                string nuevoEstado = (estadoActual == "activo") ? "inactivo" : "activo";
                
                int idProducto = int.Parse(tablaProductos.Rows[e.RowIndex].Cells[0].Value.ToString());
                
                bool cambioExitoso = ProductoControlador.cambiarEstadoProducto(idProducto, nuevoEstado);

                if (cambioExitoso)
                {
                    
                    tablaProductos.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = nuevoEstado;
                }
                else
                {
                   
                    MessageBox.Show("Error al cambiar el estado del producto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

           
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            string textoBusqueda = textBox3.Text.ToLower();

            List<Producto> productosFiltrados = ProductoControlador.FiltrarProductosPorNombre(textoBusqueda);
       
            tablaProductos.Rows.Clear();
          
            foreach (Producto producto in productosFiltrados)
            {
                int rowIndex = tablaProductos.Rows.Add();
                Categoria categoriaProducto = ProductoControlador.obtenerCategoria(producto.id_categoria);

                tablaProductos.Rows[rowIndex].Height = 150;

                Image image;
                byte[] imageBytes = Convert.FromBase64String(producto.ImagenBase64);
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    image = Image.FromStream(ms);
                }

                tablaProductos.Rows[rowIndex].Cells[0].Value = producto.id.ToString();
                tablaProductos.Rows[rowIndex].Cells[1].Value = image;
                tablaProductos.Rows[rowIndex].Cells[2].Value = producto.nombre.ToString();
                tablaProductos.Rows[rowIndex].Cells[3].Value = producto.precio.ToString();
                tablaProductos.Rows[rowIndex].Cells[4].Value = producto.stock.ToString();
                tablaProductos.Rows[rowIndex].Cells[5].Value = categoriaProducto.nombre;
                tablaProductos.Rows[rowIndex].Cells[6].Value = producto.activo;
            }
        }

        private void tablaProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) 
            {
                
                DataGridViewRow filaSeleccionada = tablaProductos.Rows[e.RowIndex];

                int idProducto = Convert.ToInt32(filaSeleccionada.Cells[0].Value);
                string nombreProducto = filaSeleccionada.Cells[2].Value.ToString();
                int stockProducto = Convert.ToInt32(filaSeleccionada.Cells[4].Value);
                int precioProducto = Convert.ToInt32(filaSeleccionada.Cells[3].Value);
                Image imagenProducto = (Image)filaSeleccionada.Cells[1].Value;
                string nombreCategoria = filaSeleccionada.Cells[5].Value.ToString();

                lblidprod.Text = idProducto.ToString();
                txt_nombre.Text = nombreProducto;
                txt_stock.Text = stockProducto.ToString();
                txt_precio.Text = precioProducto.ToString();
                pictureBox.Image = imagenProducto;
                int indexCategoria = box_categoria.FindStringExact(nombreCategoria);
                if (indexCategoria != -1)
                {
                    box_categoria.SelectedIndex = indexCategoria;
                }
            }
        }
        private string ImageToBase64(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                byte[] imageBytes = ms.ToArray();
                return Convert.ToBase64String(imageBytes);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string nombreCategoriaSeleccionada = box_categoria.SelectedItem.ToString();
            int idCategoria = categorias.Where(cat => cat.nombre == nombreCategoriaSeleccionada).FirstOrDefault().id;
            string activo = "activo";

            string imagenBase64 = ImageToBase64(pictureBox.Image);

            Producto producto = new Producto(txt_nombre.Text, activo, Convert.ToInt32(txt_stock.Text),idCategoria, Convert.ToInt32(txt_precio.Text), imagenBase64);
            ProductoControlador productoControlador = new ProductoControlador();

            productoControlador.CrearProducto(producto);

            mostrarProductos();
        }

        private void btnmodificar_Click(object sender, EventArgs e)
        {
            
            string nombreCategoriaSeleccionada = box_categoria.SelectedItem.ToString();
            int idCategoria = categorias.Where(cat => cat.nombre == nombreCategoriaSeleccionada).FirstOrDefault().id;
            string imagenBase64 = ImageToBase64(pictureBox.Image);

            Producto producto = new Producto(Convert.ToInt32(lblidprod.Text), txt_nombre.Text, Convert.ToInt32(txt_stock.Text), idCategoria, Convert.ToInt32(txt_precio.Text), imagenBase64);
            ProductoControlador productoControlador = new ProductoControlador();

            productoControlador.ModificarProducto(producto);

            mostrarProductos();
        }

        private void btneliminar_Click(object sender, EventArgs e)
        {
            int idProdEliminar = Convert.ToInt32(lblidprod.Text);
            ProductoControlador productoControlador = new ProductoControlador();

            productoControlador.EliminarProducto(idProdEliminar);

            mostrarProductos();
        }

        private void tablaProductos_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow filaSeleccionada = tablaProductos.Rows[e.RowIndex];
                int idProducto = Convert.ToInt32(filaSeleccionada.Cells[0].Value);
                int cantidadProducto = 1;
                int precioProducto = Convert.ToInt32(filaSeleccionada.Cells[3].Value);
                int stockProducto = Convert.ToInt32(filaSeleccionada.Cells[4].Value); 

                if (stockProducto > 0) 
                {
                    Carrito carrito = new Carrito(cantidadProducto, precioProducto);

                    CarritoControlador carritoControlador = new CarritoControlador();
                    carritoControlador.AgregarProductoCarrito(idProducto, carrito);

                    
                    this.DialogResult = DialogResult.OK;
                    this.Dispose();
                }
                else
                {
                    MessageBox.Show("No se puede agregar el producto al carrito porque el stock es cero.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
