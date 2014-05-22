using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace LogViewerDaemon
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        static void Main()
        {
            try
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] 
            { 
                new LogViewerService() 
            };
                ServiceBase.Run(ServicesToRun);
            }
            catch(Exception e)
            {
                Trace.WriteLine(e.ToString(), TraceEventType.Critical.ToString());       
            }
        }
    }
}
