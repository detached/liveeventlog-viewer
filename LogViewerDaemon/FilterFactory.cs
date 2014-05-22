using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogViewerDaemon
{
    public static class FilterFactory
    {
        public static List<Filter> LoadFilter(string filterPath)
        {
            List<Filter> filters = new List<Filter>();

            if (!File.Exists(filterPath)) {
                return filters;
            }

            foreach (var line in File.ReadLines(filterPath, Encoding.UTF8))
            {
                string[] splitted = line.Split('|');
                if (splitted.Length < 3) 
                {
                    Trace.WriteLine(string.Concat("Filter syntax error at: ", line), TraceEventType.Error.ToString());
                    continue;
                }

                try
                {
                    var input = ParseInput(splitted[0]);
                    var operation = ParseOperator(splitted[1]);
                    Trace.WriteLine(string.Concat("Add filter: ", line), TraceEventType.Verbose.ToString());
                    filters.Add(new Filter(input, operation, splitted[2]));
                }
                catch (NotSupportedException e) 
                {
                    Trace.WriteLine(e.ToString(), TraceEventType.Error.ToString());
                    continue;
                }
            }

            return filters;
        }

        private static FilterDelegate ParseOperator(string p)
        {
            switch (p) 
            {
                case "contains": return (i, r) => { return i.Contains(r); };
                case "notContains": return (i, r) => { return !i.Contains(r); };
                case "equals": return (i, r) => { return i.Equals(r); };
                case "notEquals": return (i, r) => { return !i.Equals(r); };
                case "startsWith": return (i, r) => { return i.StartsWith(r); };
                case "notStartsWith": return (i, r) => { return !i.StartsWith(r); };
                case "endsWith": return (i, r) => { return i.EndsWith(r); };
                case "notEndsWith": return (i, r) => { return !i.EndsWith(r); };
                default: throw new NotSupportedException(string.Format("Don't know the operation {0}", p));
            }
        }

        private static GetInputDelegate ParseInput(string p)
        {
            switch (p) 
            {
                case "Category": return (e) => { return e.Category; };
                case "CategoryNumber": return (e) => { return e.CategoryNumber.ToString(); };
                case "EntryType": return (e) => { return e.EntryType.ToString(); };
                case "Index": return (e) => { return e.Index.ToString(); };
                case "InstanceId": return (e) => { return e.InstanceId.ToString(); };
                case "MachineName": return (e) => { return e.MachineName; };
                case "Message": return (e) => { return e.Message; };
                case "Source": return (e) => { return e.Source; };
                case "TimeGenerated": return (e) => { return e.TimeGenerated.ToString();  };
                case "TimeWritten": return (e) => { return e.TimeWritten.ToString(); };
                case "UserName": return (e) => { return e.UserName; };
                default: throw new NotSupportedException(string.Format("Don't know the property {0}" , p));
            }
        }
    }
}
