using LogViewerObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Messaging;
using System.Threading;
using System.Linq;

namespace LogViewerClient
{
    public class LogViewerClient
    {
        private List<ISubject> subjects;
        private event IncomingLogEventHandler IncomingLog;
        private bool stop;
        private string queueName;
        private List<MSMQMessage> messages;

        public LogViewerClient(string msgQueue) {
            this.queueName = msgQueue;
            this.messages = new List<MSMQMessage>();
            this.stop = false;
            this.IncomingLog = delegate { };
            this.subjects = new List<ISubject>();
        }

        public void RegisterSubject(ISubject o)
        {
            o.SetClient(this);
            this.IncomingLog += o.IncomingLog;
            this.subjects.Add(o);
        }

        public void RemoveSubject(ISubject o)
        {
            if (this.subjects.Contains(o)) 
            {
                this.IncomingLog -= o.IncomingLog;
                this.subjects.Remove(o);
            }
        }

        public MSMQMessage GetMessages(int index) {
            lock (this.messages)
            {
                return this.messages.First(m => m.Index == index);
            }
        }

        private void AddMessage(MSMQMessage m) 
        {
            lock (this.messages) 
            {
                this.messages.Add(m);
            }
        }

        public void StartListen() {
            Thread worker = new Thread(() =>
            {
                MessageQueue queue = new MessageQueue(this.queueName, QueueAccessMode.Receive);
                queue.Formatter = new XmlMessageFormatter(
                    new Type[] { typeof(MSMQMessage) });

                TimeSpan timeout = new TimeSpan(0, 0, 5);

                while (!stop)
                {
                    MessageEnumerator me = queue.GetMessageEnumerator2();

                    while (me.MoveNext(timeout))
                    {
                        Message msg = me.RemoveCurrent(timeout);

                        if (msg != null)
                        {
                            MSMQMessage entry = null;

                            try
                            {
                                entry = (MSMQMessage)msg.Body;
                            }
                            catch (InvalidCastException)
                            {
                                continue;
                            }

                            this.AddMessage(entry);
                            IncomingLog.Invoke(this, entry);
                        }
                    }
                }
            });
            worker.Start();
        }

        public void Exit()
        {
            this.stop = true;
            foreach (ISubject s in this.subjects)
            {
                s.Dispose();
            }
        }
    }
}
