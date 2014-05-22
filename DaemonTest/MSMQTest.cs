using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LogViewerDaemon;
using System.Messaging;

namespace DaemonTest
{
    [TestClass]
    public class MSMQTest
    {
        private string queueName = ".\\Private$\\logViewer_test";
        
        [TestMethod]
        public void Create_Test()
        {
            MessageQueue msg = MSMQ.Create(queueName, "Benutzer");

            Assert.IsTrue(MessageQueue.Exists(queueName));

            Assert.IsTrue(msg.CanRead);
            Assert.IsTrue(msg.CanWrite);
        }

        [TestCleanup]
        public void CleanUp() {
            MessageQueue.Delete(queueName);

            if (MessageQueue.Exists(queueName)) {
                throw new AssertFailedException("Can't delete queue.");
            }
        }
    }
}
