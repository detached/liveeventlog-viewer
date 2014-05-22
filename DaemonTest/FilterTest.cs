using LogViewerDaemon;
using LogViewerObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace DaemonTest
{
    [TestClass]
    public class FilterTest
    {
        [TestMethod]
        public void Filter_Test()
        {
            Filter f = new Filter(
                (x) => { return x.InstanceId.ToString(); },
                (x, y) => { return x.Equals(y); },
                "666");

            MSMQMessage msg = new MSMQMessage();
            msg.InstanceId = 666;

            Assert.IsTrue(f.doFilter(msg));

            msg.InstanceId = 999;
            Assert.IsFalse(f.doFilter(msg));
        }

        [TestMethod]
        public void Factory_Test()
        {
            List<Filter> f = FilterFactory.LoadFilter("./FilterTest.conf");

            Assert.AreEqual(f.Count, 11);

            MSMQMessage testMsg = new MSMQMessage();

            testMsg.Category = "blaa111blubb";
            testMsg.CategoryNumber = 123;
            testMsg.EntryType = System.Diagnostics.EventLogEntryType.Warning;
            testMsg.Index = 123;
            testMsg.InstanceId = 555;
            testMsg.MachineName = "tester 666";
            testMsg.Message = "tester 777";
            testMsg.Source = "888 tester";
            testMsg.TimeGenerated = new System.DateTime(2013, 04, 07, 19, 47, 51);
            testMsg.TimeWritten = new System.DateTime(2013, 04, 07, 19, 47, 51);
            testMsg.UserName = "test Banana";

            foreach (var filter in f)
            {
                Assert.IsTrue(filter.doFilter(testMsg));
            }

            testMsg.Category = "blaa222blaa";
            testMsg.CategoryNumber = 12221;
            testMsg.EntryType = System.Diagnostics.EventLogEntryType.Error;
            testMsg.Index = 444;
            testMsg.InstanceId = 666;
            testMsg.MachineName = "666 blaaa";
            testMsg.Message = "777 tester";
            testMsg.Source = "rababare 888";
            testMsg.TimeWritten = new System.DateTime(2014, 04, 07, 19, 47, 51);
            testMsg.TimeGenerated = new System.DateTime(2014, 04, 07, 19, 47, 51);
            testMsg.UserName = "donkey test";

            foreach (var filter in f)
            {
                Assert.IsFalse(filter.doFilter(testMsg));
            }
        }
    }
}
