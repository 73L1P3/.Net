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
using System.Windows.Shapes;

namespace Inventario.GUI.Almacen
{
    /// <summary>
    /// Lógica de interacción para Reportes.xaml
    /// </summary>
    public partial class Reportes : Window
    {
        IManejadorEmpleados manejadorEmpleado;
        IManejadorVales manejadorVales;

        public Reportes()
        {
            InitializeComponent();
            manejadorEmpleado = new ManejadorEmpleados(new RepositorioDeEmpleados());
            manejadorVales = new ManejadorVales(new RepositorioDeVales());
            cmbPersona.ItemsSource = manejadorEmpleado.Listar;

        }

        private void cmbPersona_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPersona.SelectedItem != null)
            {
                dtgTablaVales.ItemsSource = null;
                dtgTablaVales.ItemsSource = manejadorVales.BuscarNoEntregadosPorEmpleado((cmbPersona.SelectedItem as Empleado));
            }
        }
    }
}
