using AleShopEscritorio.Controlador;
using AleShopEscritorio.Modelo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.IO;
using System.Runtime.InteropServices;
using iTextSharp.text;
using iTextSharp.text.pdf;




namespace AleShopEscritorio
{
    public partial class Ventas : Form
    {      
        List<Carrito> carrito;
        List<Usuario> usuarios;
        private Menu menuForm;
        public Ventas(Menu menu)
        {
            InitializeComponent();
            menuForm = menu;
            mostrarProductosCarrito();

            usuarios = UsuarioControlador.obtenerUsuarioClientes();
            box_cliente.Items.Clear();

            foreach (Usuario usuario in usuarios)
            {
                box_cliente.Items.Add(usuario.nombre);
            }
          
        }
        [DllImport("user32.dll", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int Iparam);
        private void mostrarProductosCarrito()
        {
            carrito = CarritoControlador.obtenerListaCarrito();
            tablaventa.Rows.Clear();
            List<Producto> productos = ProductoControlador.obtenerProductos();
            

            foreach (Carrito carritos in carrito)
            {
                int rowIndex = tablaventa.Rows.Add();
                Producto producto = productos.FirstOrDefault(p => p.id == carritos.id_producto);
                int subtotal = carritos.cantidad * carritos.preciounitraio;

                tablaventa.Rows[rowIndex].Cells[0].Value = carritos.id_producto.ToString();
                tablaventa.Rows[rowIndex].Cells[1].Value = producto.nombre;
                tablaventa.Rows[rowIndex].Cells[2].Value = carritos.cantidad.ToString();
                tablaventa.Rows[rowIndex].Cells[3].Value = carritos.preciounitraio.ToString();
                tablaventa.Rows[rowIndex].Cells[4].Value = subtotal;
                tablaventa.Rows[rowIndex].Cells[5].Value = "ELIMINAR";
               // Console.WriteLine($"Producto: {producto}, Cantidad: {carritos.cantidad}, Precio Unitario: {carritos.preciounitraio}, Subtotal: {subtotal}");
            }
            
            int total = 0;

            foreach (DataGridViewRow row in tablaventa.Rows)
            {
                
                if (!row.IsNewRow && row.Cells[4].Value != null)
                {
                    
                    int subtotal = Convert.ToInt32(row.Cells[4].Value);
                    total += subtotal;
                }
            }

            lbl_total.Text = total.ToString();
           
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            CarritoControlador.EliminarCarritoSinUsuario();
            menuForm.Show();
            this.Dispose();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow filaSeleccionada = tablaventa.Rows[e.RowIndex];
            int idProducto = Convert.ToInt32(filaSeleccionada.Cells[0].Value);

            if (tablaventa.Columns[e.ColumnIndex].Name == "eliminar" && e.RowIndex >= 0)
            {
                CarritoControlador carrito = new CarritoControlador();
                carrito.EliminarProductoCarrito(idProducto);
                mostrarProductosCarrito();
            }

        }

        private void btn_agregar_Click(object sender, EventArgs e)
        {
            ListaProductos listaprod = new ListaProductos(this);
            var result = listaprod.ShowDialog(); 

            if (result == DialogResult.OK)
            {                
                mostrarProductosCarrito();
            }          
        }

        private void lbl_total_Click(object sender, EventArgs e)
        {
            
        }
        private int ObtenerStockProducto(int idProducto)
        {
            // Obtener el stock del producto desde la base de datos
            Producto producto = ProductoControlador.ObtenerProductoPorId(idProducto);
            return producto.stock;
        }
        private void tablaventa_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Verificar que se haya modificado la celda en la columna de cantidad
            if (e.ColumnIndex == 2 && e.RowIndex >= 0)
            {
                // Obtener el valor modificado en la celda
                int nuevaCantidad = Convert.ToInt32(tablaventa.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                int idProducto = Convert.ToInt32(tablaventa.Rows[e.RowIndex].Cells[0].Value);

                // Obtener el stock actual del producto
                int stockProducto = ObtenerStockProducto(idProducto);

                if (nuevaCantidad > stockProducto)
                {
                    MessageBox.Show("La cantidad modificada supera el stock del producto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    // Restaurar la cantidad original en la celda
                    tablaventa.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = stockProducto;
                }
                else
                {
                    // Actualizar la cantidad en la base de datos
                    CarritoControlador carritoControlador = new CarritoControlador();
                    carritoControlador.ActualizarCantidadCarrito(idProducto, nuevaCantidad);

                    // Volver a calcular el total y mostrarlo en el lbl_total
                    mostrarProductosCarrito();
                }
            }
        }

        private void lbl_nombrecliente_Click(object sender, EventArgs e)
        {
         
        }

        private void box_cliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            string nombreClienteSeleccionada = box_cliente.SelectedItem.ToString();
            int idCliente = usuarios.Where(cat => cat.nombre == nombreClienteSeleccionada).FirstOrDefault().id;


            Usuario usuario1 = UsuarioControlador.obtenerUsuarioEditar(idCliente);

            lbl_nombrecliente.Text = usuario1.nombre.ToString();
            lbl_apellidocliente.Text = usuario1.apellido.ToString();
        }
        private void GenerarTabla(Document doc)
        {
            mostrarProductosCarrito();

            DateTime fechaActual = DateTime.Now;

            // Agregar espacio en blanco antes de la tabla (encabezado)
            Paragraph espaciador1 = new Paragraph("\n", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12));
            doc.Add(espaciador1);

            // Agregar el encabezado
            Paragraph header = new Paragraph("ALESHOP - " + fechaActual.ToString("dd/MM/yyyy"), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 24, iTextSharp.text.Font.BOLD));
            header.Alignment = Element.ALIGN_CENTER;
            doc.Add(header);

            // Agregar espacio en blanco después del encabezado y antes de la tabla
            Paragraph espaciador2 = new Paragraph("\n", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12));
            doc.Add(espaciador2);

            PdfPTable table = new PdfPTable(4);
            table.WidthPercentage = 100;
            table.HorizontalAlignment = Element.ALIGN_LEFT;
            

            table.AddCell(new PdfPCell(new Phrase("Producto", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD))));
            table.AddCell(new PdfPCell(new Phrase("Cantidad", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD))));
            table.AddCell(new PdfPCell(new Phrase("Precio", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD))));
            table.AddCell(new PdfPCell(new Phrase("Subtotal", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD))));

            
            foreach (DataGridViewRow row in tablaventa.Rows)
            {
                
                if (!row.IsNewRow && row.Cells[0].Value != null && row.Cells[2].Value != null)
                {
                    Console.WriteLine("Producto: " + row.Cells[1].Value.ToString());
                    Console.WriteLine("Cantidad: " + row.Cells[2].Value.ToString());
                    Console.WriteLine("Precio: " + row.Cells[3].Value.ToString());
                    Console.WriteLine("Subtotal: " + row.Cells[4].Value.ToString());

                    PdfPCell productoCell = new PdfPCell(new Phrase(row.Cells[1].Value.ToString(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10)));
                    PdfPCell cantidadCell = new PdfPCell(new Phrase(row.Cells[2].Value.ToString(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10)));
                    PdfPCell precioCell = new PdfPCell(new Phrase(row.Cells[3].Value.ToString(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10)));
                    PdfPCell subtotalCell = new PdfPCell(new Phrase(row.Cells[4].Value.ToString(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10)));

                   
                    // Agregar las celdas a la fila
                    table.AddCell(productoCell);
                    table.AddCell(cantidadCell);
                    table.AddCell(precioCell);
                    table.AddCell(subtotalCell);
                }
            }

            doc.Add(table);

            Paragraph totalText = new Paragraph("Total: " + lbl_total.Text, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD));
            totalText.Alignment = Element.ALIGN_RIGHT;
            doc.Add(totalText);
        }
        private void btn_venta_Click(object sender, EventArgs e)
        {
            string nombreClienteSeleccionada = box_cliente.SelectedItem.ToString();
            int idCliente = usuarios.Where(cat => cat.nombre == nombreClienteSeleccionada).FirstOrDefault().id;
            int total = Convert.ToInt32(lbl_total.Text);

            Venta venta = new Venta(total, idCliente);

            try
            {
                // Creamos la venta y obtenemos el ID de la venta recién creada
                int idVenta = VentaControlador.CrearVenta(venta);

                // Si la venta se creó exitosamente, actualizamos el stock de los productos vendidos
                if (idVenta > 0)
                {
                    foreach (DataGridViewRow row in tablaventa.Rows)
                    {
                        if (!row.IsNewRow && row.Cells[0].Value != null && row.Cells[2].Value != null)
                        {
                            int idProducto = Convert.ToInt32(row.Cells[0].Value);
                            int cantidadVendida = Convert.ToInt32(row.Cells[2].Value);
                            ProductoControlador.ActualizarStockProducto(idProducto, cantidadVendida);

                            int precio = Convert.ToInt32(row.Cells[4].Value);

                            Detalle_Venta detalle_Venta = new Detalle_Venta(idVenta, idProducto, cantidadVendida, precio);

                            if (VentaControlador.CrearDetalleVenta(detalle_Venta))
                            {                               
                                mostrarProductosCarrito();
                            }
                        }
                    }

                    string rutaGuardadoPDF = @"D:\DavinciClases\4tocuatrimestre\plataformadesarrollo\PDF\" + idVenta + ".pdf";
                    Document doc = new Document();
                    PdfWriter.GetInstance(doc, new FileStream(rutaGuardadoPDF,FileMode.Create));
                    doc.Open();                  

                    GenerarTabla(doc);

                    doc.Close();

                    System.Diagnostics.Process.Start(rutaGuardadoPDF);
                    MessageBox.Show("Venta realizada con éxito. ID de venta: " + idVenta, "Venta exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (CarritoControlador.EliminarCarritoSinUsuario())
                    {
                        mostrarProductosCarrito();
                    }                  
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al realizar la venta: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
    }
}
