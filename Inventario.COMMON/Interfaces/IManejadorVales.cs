using Inventario.COMMON.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Inventario.COMMON.Interfaces
{
    public interface IManejadorVales:IManejadorGenerico<Vale>
    {
        List<Vale> ValesPorLiquidar();
        List<Vale> ValesEnIntervalo(DateTime inicio, DateTime fin);
        IEnumerable BuscarNoEntregadosPorEmpleado(Empleado empleado);
    } 
}
