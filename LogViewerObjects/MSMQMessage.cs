using System;
using System.Diagnostics;

namespace LogViewerObjects
{
    [Serializable]
    public class MSMQMessage
    {
        public static MSMQMessage Wrap(System.Diagnostics.EventLogEntry eventLogEntry)
        {
            MSMQMessage e = new MSMQMessage();
            e.Category = eventLogEntry.Category;
            e.CategoryNumber = eventLogEntry.CategoryNumber;
            e.Data = eventLogEntry.Data;
            e.EntryType = eventLogEntry.EntryType;
            e.Index = eventLogEntry.Index;
            e.InstanceId = eventLogEntry.InstanceId;
            e.MachineName = eventLogEntry.MachineName;
            e.Message = eventLogEntry.Message;
            e.Source = eventLogEntry.Source;
            e.TimeGenerated = eventLogEntry.TimeGenerated;
            e.TimeWritten = eventLogEntry.TimeWritten;
            e.UserName = eventLogEntry.UserName;
            return e;
        }

        public string Category { get; set; }

        public short CategoryNumber { get; set; }

        public byte[] Data { get; set; }

        public System.Diagnostics.EventLogEntryType EntryType { get; set; }

        public int Index { get; set; }

        public long InstanceId { get; set; }

        public string MachineName { get; set; }

        public string Message { get; set; }

        public string Source { get; set; }

        public DateTime TimeGenerated { get; set; }

        public DateTime TimeWritten { get; set; }

        public string UserName { get; set; }
    }
}
