using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogViewerClient
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (!System.Messaging.MessageQueue.Exists(Properties.Settings.Default.MessageQueue)) 
            {
                return;
            }

            LogViewerClient client = new LogViewerClient(Properties.Settings.Default.MessageQueue);

            Tray trayApp = new Tray();
            //DetailWindow dw = new DetailWindow();

            client.RegisterSubject(trayApp);
            //client.RegisterSubject(dw);
            client.StartListen();

            Application.Run(trayApp);
            //Application.Run(dw);
        }
    }
}
