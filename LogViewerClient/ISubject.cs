using LogViewerObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogViewerClient
{
    public delegate void IncomingLogEventHandler(object sender, MSMQMessage e);

    public interface ISubject: IDisposable
    {
        void IncomingLog(object sender, MSMQMessage e);
        void SetClient(LogViewerClient c);
    }
}
