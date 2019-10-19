using Inventario.BIZ;
using Inventario.COMMON.Entidades;
using Inventario.COMMON.Interfaces;
using Inventario.DAL;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Inventario.GUI.Administrador
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        enum accion
        {
            Nuevo,
            Editar
        }

        IManejadorEmpleados manejadorEmpleados;
        IManejadorMateriales manejadorMateriales;

        accion accionEmpleados;
        accion accionMateriales;
        public MainWindow()
        {
            //Main Function
            InitializeComponent();
            //use db 
            manejadorEmpleados = new ManejadorEmpleados(new RepositorioGenerico<Empleado>());
            manejadorMateriales = new ManejadorMateriales(new RepositorioGenerico<Material>());
            //Functions
            //Empleados
            PonerBotonesDeEmpleadosEnEdicion(false);
            LimpiarCamposDeEmpleados();
            ActualizarTablaEmpleados();
            //Materiales
            PonerBotonesDeMaterialesEnEdicion(false);
            LimpiarCamposDeMateriales();
            ActualizarTablaMateriales();
        }


        //Empleados
        private void ActualizarTablaEmpleados()
        {
            dtgEmpleados.ItemsSource = null;
            dtgEmpleados.ItemsSource = manejadorEmpleados.Listar;
        }

        private void LimpiarCamposDeEmpleados()
        {
            tbxEmpleadosApellidos.Clear();
            tbxEmpleadosArea.Clear();
            tbxEmpleadosId.Text = "";
            tbxEmpleadosNombre.Clear();
        }

        private void PonerBotonesDeEmpleadosEnEdicion(bool v)
        {
            btnEmpleadosCancelar.IsEnabled = v;
            btnEmpleadosEditar.IsEnabled = !v;
            btnEmpleadosEliminar.IsEnabled = !v;
            btnEmpleadosGuardar.IsEnabled = v;
            btnEmpleadosNuevo.IsEnabled = !v;
        }

        //Botones
        private void btnEmpleadosNuevo_Click(object sender, RoutedEventArgs e)
        {
            LimpiarCamposDeEmpleados();
            PonerBotonesDeEmpleadosEnEdicion(true);
            accionEmpleados = accion.Nuevo;
        }

        private void btnEmpleadosEditar_Click(object sender, RoutedEventArgs e)
        {
            Empleado emp = dtgEmpleados.SelectedItem as Empleado;
            if (emp != null)
            {
                tbxEmpleadosId.Text = emp.Id.ToString();
                tbxEmpleadosApellidos.Text = emp.Apellidos;
                tbxEmpleadosArea.Text = emp.Area;
                tbxEmpleadosNombre.Text = emp.Nombre;
                accionEmpleados = accion.Editar;
                PonerBotonesDeEmpleadosEnEdicion(true);
            }
        }

        private void btnEmpleadosGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (accionEmpleados == accion.Nuevo)
            {
                Empleado emp = new Empleado()
                {
                    Nombre = tbxEmpleadosNombre.Text,
                    Apellidos = tbxEmpleadosApellidos.Text,
                    Area = tbxEmpleadosArea.Text
                };
                if (manejadorEmpleados.Agregar(emp))
                {
                    MessageBox.Show("Empleado agregado correctamente", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Information);
                    LimpiarCamposDeEmpleados();
                    ActualizarTablaEmpleados();
                    PonerBotonesDeEmpleadosEnEdicion(false);
                }
                else
                {
                    MessageBox.Show("El empleado no se puedo agregar", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                Empleado emp = dtgEmpleados.SelectedItem as Empleado;
                emp.Apellidos = tbxEmpleadosApellidos.Text;
                emp.Area = tbxEmpleadosArea.Text;
                emp.Nombre = tbxEmpleadosNombre.Text;
                if (manejadorEmpleados.Modificar(emp))
                {
                    MessageBox.Show("Empleado modificado correctamente", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Information);
                    LimpiarCamposDeEmpleados();
                    ActualizarTablaEmpleados();
                    PonerBotonesDeEmpleadosEnEdicion(false);
                }
                else
                {
                    MessageBox.Show("El empleado no se puedo actualizar", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnEmpleadosCancelar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarCamposDeEmpleados();
            PonerBotonesDeEmpleadosEnEdicion(false);
        }

        private void btnEmpleadosEliminar_Click(object sender, RoutedEventArgs e)
        {
            Empleado emp = dtgEmpleados.SelectedItem as Empleado;
            if (emp != null)
            {
                if (MessageBox.Show("Realmente deseas eliminar este empleado?", "Inventarios", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if (manejadorEmpleados.Eliminar(emp.Id))
                    {
                        MessageBox.Show("Empleado Eliminado", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Information);
                        ActualizarTablaEmpleados();

                    }
                    else
                    {
                        MessageBox.Show("No se pudo eliminar el empleado", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    ;
                };
            }
        }


        //Materiales
        private void ActualizarTablaMateriales()
        {
            lsvMateriales.ItemsSource = null;//vaciamos la lista
            lsvMateriales.ItemsSource = manejadorMateriales.Listar.OrderBy(p => p.Nombre);//los ordenamos por el nombre
        }

        private void LimpiarCamposDeMateriales()
        {
            tbxMaterialesId.Text = "";
            tbxMaterialesNombre.Clear();
            tbxMaterialesCategoria.Clear();
            tbxMaterialesDescripcion.Clear();      
        }

        private void PonerBotonesDeMaterialesEnEdicion(bool value)
        {
            btnMaterialesCancelar.IsEnabled = value;
            btnMaterialesEditar.IsEnabled = !value;
            btnMaterialesEliminar.IsEnabled = !value;
            btnMaterialesGuardar.IsEnabled = value;
            btnMaterialesNuevo.IsEnabled = !value;
        }

        //Botones
        private void btnMaterialesNombre_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnMaterialesCategoria_Click(object sender, RoutedEventArgs e)
        {

        }


        //Botones 2
        private void btnMaterialesNuevo_Click(object sender, RoutedEventArgs e)
        {
            LimpiarCamposDeMateriales();
            accionMateriales = accion.Nuevo;
            PonerBotonesDeMaterialesEnEdicion(true);

        }

        private void btnMaterialesEditar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarCamposDeMateriales();
            accionMateriales = accion.Editar;
            PonerBotonesDeMaterialesEnEdicion(true);

            Material m = lsvMateriales.SelectedItem as Material;
            if (m != null)
            {
                tbxMaterialesCategoria.Text = m.Categoria;
                tbxMaterialesDescripcion.Text = m.Categoria;
                tbxMaterialesNombre.Text = m.Nombre;
                tbxMaterialesId.Text = m.Id.ToString();
                imgFoto.Source = ByteToImage(m.Fotografia);
            }
        }

        public void btnMaterialesGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (accionMateriales == accion.Nuevo)
            {
                Material m = new Material()
                {
                    Categoria = tbxMaterialesCategoria.Text,
                    Nombre = tbxMaterialesNombre.Text,
                    Descripcion = tbxMaterialesDescripcion.Text,
                    Fotografia = ImageToByte(imgFoto.Source)
                };
                if (manejadorMateriales.Agregar(m))
                {
                    MessageBox.Show("Material agregado correctamente", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Information);
                    LimpiarCamposDeMateriales();
                    ActualizarTablaMateriales();
                    PonerBotonesDeMaterialesEnEdicion(false);
                }
                else
                {
                    MessageBox.Show("Algo salio mal", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                Material m = lsvMateriales.SelectedItem as Material;
                m.Categoria = tbxMaterialesCategoria.Text;
                m.Descripcion = tbxMaterialesDescripcion.Text;
                m.Nombre = tbxMaterialesNombre.Text;
                m.Fotografia = ImageToByte(imgFoto.Source);
                if (manejadorMateriales.Modificar(m))
                {
                    MessageBox.Show("Material correctamente modificado", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Information);
                    LimpiarCamposDeMateriales();
                    ActualizarTablaMateriales();
                    PonerBotonesDeMaterialesEnEdicion(false);
                }
                else
                {
                    MessageBox.Show("Algo salio mal", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public byte[] ImageToByte(ImageSource image)
        {
            if (image != null)
            {
                MemoryStream memStream = new MemoryStream();
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image as BitmapSource));
                encoder.Save(memStream);
                return memStream.ToArray();
            }
            else
            {
                return null;
            }
        }

        public ImageSource ByteToImage(byte[] imageData)
        {
            if (imageData == null)
            {
                return null;
            }
            else
            {
                BitmapImage biImg = new BitmapImage();
                MemoryStream ms = new MemoryStream(imageData);
                biImg.BeginInit();
                biImg.StreamSource = ms;
                biImg.EndInit();
                ImageSource imgSrc = biImg as ImageSource;
                return imgSrc;
            }
        }

        private void btnMaterialesCancelar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarCamposDeMateriales();
            PonerBotonesDeMaterialesEnEdicion(false);
        }

        private void btnMaterialesEliminar_Click(object sender, RoutedEventArgs e)
        {
            Material m = lsvMateriales.SelectedItem as Material;
            if (m != null)
            {
                if (MessageBox.Show("Realmente deseas eliminar este material", "Inventarios", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if (manejadorMateriales.Eliminar(m.Id))
                    {
                        MessageBox.Show("Material eliminado correctamente", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Information);
                        ActualizarTablaMateriales();
                    }
                    else
                    {
                        MessageBox.Show("Algo salio mal", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnCargarFoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialogo = new OpenFileDialog();
            dialogo.Title = "Seleccione una foto";
            dialogo.Filter = "Archivos de imagen|*.jpg; *.gif; *.png";
            if (dialogo.ShowDialog().Value)
            {
                imgFoto.Source = new BitmapImage(new Uri(dialogo.FileName));
            }
        }

    }
}
