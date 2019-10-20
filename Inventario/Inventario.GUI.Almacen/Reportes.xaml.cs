using Inventario.BIZ;
using Inventario.COMMON.Entidades;
using Inventario.COMMON.Interfaces;
using Inventario.COMMON.Modelos;
using Inventario.DAL;
using Microsoft.Reporting.WinForms;
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
            manejadorEmpleado = new ManejadorEmpleados(new RepositorioGenerico<Empleado>());
            manejadorVales = new ManejadorVales(new RepositorioGenerico<Vale>());
            cmbPersona.ItemsSource = manejadorEmpleado.Listar;

        }

        private void cmbPersona_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPersona.SelectedItem != null)
            {
                lstVales.ItemsSource = null;
                lstVales.ItemsSource = manejadorVales.BuscarNoEntregadosPorEmpleado((cmbPersona.SelectedItem as Empleado));
            }
        }

        private void btnImprimirPorPersona_Click(object sender, RoutedEventArgs e)
        {
            List<ReportDataSource> datos = new List<ReportDataSource>();
            ReportDataSource vales = new ReportDataSource();
            List<ModelValesParaReporte> valesporentregar = new List<ModelValesParaReporte>();
            foreach (var item in manejadorVales.ValesPorLiquidar())
            {
                valesporentregar.Add(new ModelValesParaReporte(item));
            }
            vales.Value = valesporentregar;
            vales.Name = "DataSet1";
            datos.Add(vales);

            Reporteador ventana = new Reporteador("Inventario.GUI.Almacen.Reportes.SinParametros.rdlc", datos);
            ventana.ShowDialog();
        }
    }
}
