using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LogViewerObjects;
using System.Messaging;
using System.Threading;
using LogViewerClient;

namespace ClientTest
{
    [TestClass]
    public class ClientTest: ISubject
    {
        private MSMQMessage msg;
        private string queueName = ".\\Private$\\clientTest";

        [TestMethod]
        public void RaiseEvent()
        {
            MessageQueue msgQueue = MessageQueue.Create(queueName);

            LogViewerClient.LogViewerClient client = new LogViewerClient.LogViewerClient(queueName);
            client.RegisterSubject(this);
            client.StartListen();

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

            msgQueue.Send(testMsg);
            Thread.Sleep(500);
            client.Exit();

            Assert.IsNotNull(msg);
            Assert.AreEqual(555, msg.InstanceId);
        }

        [TestCleanup]
        public void CleanUp()
        {
            MessageQueue.Delete(queueName);

            if (MessageQueue.Exists(queueName))
            {
                throw new AssertFailedException("Can't delete queue.");
            }
        }

        public void IncomingLog(object sender, MSMQMessage e)
        {
            this.msg = e;
        }

        public void SetClient(LogViewerClient.LogViewerClient c)
        {
        }

        public void Dispose()
        {
        }
    }
}
