using LogViewerObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Messaging;
using System.ServiceProcess;

namespace LogViewerDaemon
{
    public partial class LogViewerService : ServiceBase
    {
        private List<EventLog> logsToMonitor;
        private List<Filter> filters;
        private MessageQueue queue;

        public LogViewerService()
        {
            InitializeComponent();
            logsToMonitor = new List<System.Diagnostics.EventLog>();
            filters = new List<Filter>();
        }

        public void OnEntryWritten(object source, EntryWrittenEventArgs e)
        {
            try
            {
                Trace.WriteLine(string.Format("New Event: {0}", e.Entry.Index), TraceEventType.Verbose.ToString());

                MSMQMessage msg = MSMQMessage.Wrap(e.Entry);

                foreach (Filter f in this.filters)
                {
                    if (!f.doFilter(msg))
                    {
                        return;
                    }
                }

                queue.Send(msg);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString(), TraceEventType.Critical.ToString());
            }
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                Trace.WriteLine(DateTime.Now.ToString(), TraceEventType.Start.ToString());

                this.queue = MSMQ.Create(Properties.Settings.Default.MessageQueue,
                    Properties.Settings.Default.MessageQueuePermissions);

                this.filters = FilterFactory.LoadFilter(Properties.Settings.Default.FilterScript);

                foreach (var source in Properties.Settings.Default.Eventlogs)
                {
                    Trace.WriteLine(string.Format(string.Format("Attache to {0} log", source), TraceEventType.Verbose.ToString()));
                    EventLog newLog = new EventLog(source);
                    newLog.EntryWritten += new EntryWrittenEventHandler(OnEntryWritten);
                    newLog.EnableRaisingEvents = true;
                    this.logsToMonitor.Add(newLog);
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString(), TraceEventType.Critical.ToString());
            }
        }

        protected override void OnStop()
        {
            try
            {
                Trace.WriteLine(DateTime.Now.ToString(), TraceEventType.Stop.ToString());

                foreach (var source in logsToMonitor)
                {
                    source.Close();
                }

                logsToMonitor.Clear();
                filters.Clear();
                this.queue.Close();
                MessageQueue.Delete(Properties.Settings.Default.MessageQueue);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString(), TraceEventType.Critical.ToString());
            }
        }

        protected override void OnPause()
        {
            RaisingEvents(false);
        }

        protected override void OnContinue()
        {
            RaisingEvents(true);
        }

        private void RaisingEvents(bool state) {
            foreach (var l in this.logsToMonitor)
            {
                l.EnableRaisingEvents = state;
            }
        }
    }
}
