using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace GenerateEvent
{
    class Program
    {
        static void Main(string[] args)
        {
            var msg = "Test Message";
            var source = "MsiInstaller";

            EventLog.WriteEntry(source, msg, EventLogEntryType.Information, 11707);
        }
    }
}
