using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SchedulingBenchmarking
{
    [TestClass]
    public class JobTest
    {
        Job job1;

        [TestInitialize]
        public void Setup()
        {
            job1 = new Job((string[] arg) => { return arg.Length.ToString(); }, new Owner("tester"), 3, 35);
        }

        [TestMethod]
        public void TestProcess()
        {

            string[] args = new string[] { "task1", "task2", "task3" };

            // Job1 should return the length of the supplied array as a string
            Assert.AreEqual("3", job1.Process(args));
            Assert.AreEqual("0", job1.Process(new string[] { }));
        }


    }
}
