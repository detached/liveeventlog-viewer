using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace LogViewerDaemon
{
    public class MSMQ
    {
       public static MessageQueue Create(string name, string permissions) {
           MessageQueue queue;
 
           if (!MessageQueue.Exists(name))
           {
                Trace.WriteLine(string.Format("Add MessageQueue {0}", name), TraceEventType.Verbose.ToString());
                queue = MessageQueue.Create(name);
           }
           else {
               queue = new MessageQueue(name);
           }

           //queue.Authenticate = true;
           queue.SetPermissions(permissions, MessageQueueAccessRights.GenericRead);
           return queue;
       }
    }
}
