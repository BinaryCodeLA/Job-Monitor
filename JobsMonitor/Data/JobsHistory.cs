using System;
using System.Collections.Generic;
using System.Net.Security;
using System.Text;

namespace JobsMonitor.Data
{
    public class JobsHistory
    {
        public string Instancia { get; set; }
        public string Nombre_Job { get; set; }
        public string Fecha_ejecucion { get; set; }
        public  string Estado { get; set; }
    }
}
