using LogViewerObjects;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace LogViewerClient
{
    public partial class Tray : Form, ISubject
    {
        private NotifyIcon trayIcon;
        private LogViewerClient client;
        private DetailWindow details;
        private int currentMsgIndex;

        public Tray()
        {
            this.currentMsgIndex = 0;
            this.details = new DetailWindow();
            ContextMenu trayMenue = new ContextMenu();
            trayMenue.MenuItems.Add("Details", ShowDetails);
            trayMenue.MenuItems.Add("Exit", onExit);

            this.trayIcon = new NotifyIcon();
            this.trayIcon.Text = "LiveLogViewer";
            this.trayIcon.Icon = Properties.Resources.LogViewer;
            this.trayIcon.ContextMenu = trayMenue;
            this.trayIcon.Visible = true;
            this.trayIcon.BalloonTipClicked += trayIcon_BalloonTipClicked;
            this.trayIcon.Click += ShowDetails;
        }

        public void IncomingLog(object sender, MSMQMessage e)
        {
            ToolTipIcon icon;

            switch(e.EntryType)
            {
                case EventLogEntryType.Error: icon = ToolTipIcon.Error; break;
                case EventLogEntryType.Information: icon = ToolTipIcon.Info; break;
                case EventLogEntryType.Warning: icon = ToolTipIcon.Warning; break;
                default: icon = ToolTipIcon.None; break;
            }
            this.currentMsgIndex = e.Index;
            this.trayIcon.ShowBalloonTip(3000, string.Format("{0} - {1}", e.Source.ToString(), e.InstanceId.ToString()), e.Message, icon);
        }

        public void SetClient(LogViewerClient c)
        {
            this.client = c;
            this.details.SetClient(c);
            this.client.RegisterSubject(this.details);
        }

        protected override void OnLoad(EventArgs e)
        {
            this.Visible = false;
            this.ShowInTaskbar = false;

            base.OnLoad(e);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.trayIcon.Dispose();
            }

            base.Dispose(disposing);
        }

        private void trayIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            this.details.ShowMessage(this.currentMsgIndex);
            this.ShowDetails(null, null);
        }

        private void onExit(object sender, EventArgs e)
        {
            this.client.Exit();
            Application.Exit();
        }

        private void ShowDetails(object sender, EventArgs e)
        {
            if (!this.details.Visible)
            {
                this.details.ShowDialog();
            }
            else 
            {
                this.details.Activate();
            }
        }
    }
}
