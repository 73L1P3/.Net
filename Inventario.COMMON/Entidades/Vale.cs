using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventario.COMMON.Entidades
{
    public class Vale:Base
    {
        public DateTime FechaHoraSolicitud { get; set; }
        public DateTime FechaEntrega { get; set;}
        public DateTime? FechaEntregaReal { get; set;}
        public List<Material> 
            MaterialesPrestados { get; set; }
        public Empleado Solicitante { get; set; }
        public Empleado EncargadoDeAlmacen { get; set; }

        public override string ToString()
        {
            return String.Format("{0} {1} debe {2} articulos y los deberia de entregar el {3}", Solicitante.Nombre, Solicitante.Apellidos, MaterialesPrestados.Count, FechaEntrega.ToShortDateString());
        }
    }
}
