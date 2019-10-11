using Inventario.BIZ;
using Inventario.COMMON.Entidades;
using Inventario.COMMON.Interfaces;
using Inventario.DAL;
using System;
using System.Collections.Generic;
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

        accion accionEmpleados;
        public MainWindow()
        {
            InitializeComponent();

            manejadorEmpleados = new ManejadorEmpleados(new RepositorioDeEmpleados());

            PonerBotonesDeEmpleadosEnEdicion(false);
            LimpiarCamposDeEmpleados();
            ActualizarTablaEmpleados();
        }

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
                tbxEmpleadosId.Text = emp.Id;
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
                }else
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
                    if(manejadorEmpleados.Eliminar(emp.Id))
                    {
                        MessageBox.Show("Empleado Eliminado", "Inventarios" ,MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
