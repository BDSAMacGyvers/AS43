using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SchedulingBenchmarking;

namespace SchedulingBenchmarkingTest
{
    [TestClass]
    public class SchedulerTest
    {
        internal Scheduler scheduler;
        internal Job job1;
        internal Job job2;
        internal Job job3;


        [TestInitialize]
        public void Initialize()
        {
            scheduler = Scheduler.getInstance();
            job1 = new Job((string[] args) => { foreach (string s in args) { Console.Out.WriteLine(s); } return ""; }, new Owner("owner1"), 2, 3);
            job2 = new Job((string[] args) => { foreach (string s in args) { Console.Out.WriteLine(s); } return ""; }, new Owner("owner2"), 2, 45);
            job3 = new Job((string[] args) => { foreach (string s in args) { Console.Out.WriteLine(s); } return ""; }, new Owner("owner3"), 2, 200);
        }

        [TestMethod]
        public void TestAddJob()
        {
            scheduler.addJob(job1);
            scheduler.addJob(job2);
            scheduler.addJob(job3);
            Assert.AreEqual(scheduler.popJob().Owner.Name, job1.Owner.Name);
            Assert.AreEqual(scheduler.popJob().Owner.Name, job2.Owner.Name);
            Assert.AreEqual(scheduler.popJob().Owner.Name, job3.Owner.Name);
        }

        [TestMethod]
        public void TestEmpty()
        {
            Assert.IsTrue(scheduler.Empty());
            scheduler.addJob(job1);
            Assert.IsFalse(scheduler.Empty());
            scheduler.popJob();
            Assert.IsTrue(scheduler.Empty());
            scheduler.addJob(job2);
            scheduler.addJob(job3);
            Assert.IsFalse(scheduler.Empty());
            scheduler.popJob();
            scheduler.popJob();
            Assert.IsTrue(scheduler.Empty());
        }

        [TestMethod]
        public void TestRemoveJob()
        {
            scheduler.addJob(job1);
            scheduler.addJob(job2);
            scheduler.addJob(job3);
            scheduler.removeJob(job2);
            Assert.AreEqual(scheduler.popJob().Owner.Name, job1.Owner.Name);
            Assert.AreEqual(scheduler.popJob().Owner.Name, job3.Owner.Name);
            Assert.IsTrue(scheduler.Empty());
        }

        [TestMethod]
        public void PopJob()
        {
            Assert.IsTrue(scheduler.Empty());
            // Adding our three premade jobs
            scheduler.addJob(job1);
            scheduler.addJob(job2);
            scheduler.addJob(job3);
            // popping them and verify that they appear in the same order.
            Assert.AreEqual(scheduler.popJob().Owner.Name, job1.Owner.Name);
            Assert.AreEqual(scheduler.popJob().Owner.Name, job2.Owner.Name);
            Assert.AreEqual(scheduler.popJob().Owner.Name, job3.Owner.Name);
            //create more jobs, add them and verify that they appear in same order
            Job job4 = new Job((string[] args) => { foreach (string s in args) { Console.Out.WriteLine(s); } return ""; }, new Owner("owner4"), 2, 3);
            Job job5 = new Job((string[] args) => { foreach (string s in args) { Console.Out.WriteLine(s); } return ""; }, new Owner("owner5"), 2, 45);
            Job job6 = new Job((string[] args) => { foreach (string s in args) { Console.Out.WriteLine(s); } return ""; }, new Owner("owner6"), 2, 200);
            Job job7 = new Job((string[] args) => { foreach (string s in args) { Console.Out.WriteLine(s); } return ""; }, new Owner("owner7"), 2, 45);
            Job job8 = new Job((string[] args) => { foreach (string s in args) { Console.Out.WriteLine(s); } return ""; }, new Owner("owner8"), 2, 45);
            Job job9 = new Job((string[] args) => { foreach (string s in args) { Console.Out.WriteLine(s); } return ""; }, new Owner("owner9"), 2, 200);
            Job job10 = new Job((string[] args) => { foreach (string s in args) { Console.Out.WriteLine(s); } return ""; }, new Owner("owner10"), 2, 3);
            Job job11 = new Job((string[] args) => { foreach (string s in args) { Console.Out.WriteLine(s); } return ""; }, new Owner("owner11"), 2, 45);
            Job job12 = new Job((string[] args) => { foreach (string s in args) { Console.Out.WriteLine(s); } return ""; }, new Owner("owner12"), 2, 200);


            scheduler.addJob(job4);
            System.Threading.Thread.Sleep(1000);
            scheduler.addJob(job5);
            System.Threading.Thread.Sleep(1000);
            scheduler.addJob(job6);
            System.Threading.Thread.Sleep(1000);
            scheduler.addJob(job7);
            System.Threading.Thread.Sleep(1000);
            scheduler.addJob(job8);
            System.Threading.Thread.Sleep(1000);
            scheduler.addJob(job9);
            System.Threading.Thread.Sleep(1000);
            scheduler.addJob(job10);
            System.Threading.Thread.Sleep(1000);
            scheduler.addJob(job11);
            System.Threading.Thread.Sleep(1000);
            scheduler.addJob(job12);

            Assert.AreEqual(scheduler.popJob().Owner.Name, job4.Owner.Name);
            Assert.AreEqual(job5.Owner.Name, scheduler.popJob().Owner.Name);
            Assert.AreEqual(scheduler.popJob().Owner.Name, job6.Owner.Name);
            Assert.AreEqual(scheduler.popJob().Owner.Name, job7.Owner.Name);
            Assert.AreEqual(scheduler.popJob().Owner.Name, job8.Owner.Name);
            Assert.AreEqual(scheduler.popJob().Owner.Name, job9.Owner.Name);
            Assert.AreEqual(scheduler.popJob().Owner.Name, job10.Owner.Name);
            Assert.AreEqual(scheduler.popJob().Owner.Name, job11.Owner.Name);
            Assert.AreEqual(scheduler.popJob().Owner.Name, job12.Owner.Name);
            Assert.IsTrue(scheduler.Empty());
        }
    }
}
