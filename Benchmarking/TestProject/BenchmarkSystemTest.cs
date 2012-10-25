using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace SchedulingBenchmarking
{
    [TestClass]
    public class BenchmarkSystemTest
    {
        BenchmarkSystem BS;
        Job job;

        [TestInitialize]
        public void setup()
        {
            BenchmarkSystem BS = new BenchmarkSystem();
            Job job = new Job((string[] arg) => { return arg.Length.ToString(); }, new Owner("tester"), 3, 35);
        }

        [TestMethod]
        public void TestSubmit()
        {
            BenchmarkSystem BS = new BenchmarkSystem();
            Job job = new Job((string[] arg) => { return arg.Length.ToString(); }, new Owner("tester"), 3, 35);

            Assert.AreEqual(0, BS.Status.Count);
            Assert.IsTrue(BS.scheduler.Empty());

            BS.Submit(job);
            Assert.AreEqual(1, BS.Status.Count);
            Assert.IsTrue(BS.Status.Contains(job));
            Assert.IsFalse(BS.scheduler.Empty());
        }

        [TestMethod]
        public void TestCancel()
        {
            BenchmarkSystem BS = new BenchmarkSystem();
            Job job = new Job((string[] arg) => { return arg.Length.ToString(); }, new Owner("tester"), 3, 35);

            Assert.AreEqual(0, BS.Status.Count);
            Assert.AreEqual(true, BS.scheduler.Empty());

            BS.Submit(job);
            Assert.AreEqual(1, BS.Status.Count);
            Assert.IsTrue(BS.Status.Contains(job));
            Assert.IsFalse(BS.scheduler.Empty());

            BS.Cancel(job);
            Assert.AreEqual(0, BS.Status.Count);
            Assert.IsFalse(BS.Status.Contains(job));
            Assert.IsTrue(BS.scheduler.Empty());
        }

        [TestMethod]
        public void TestUpdateStatus()
        {
            BenchmarkSystem BS = new BenchmarkSystem();

            // tatus should have 0 elements in its set to begin with
            Assert.AreEqual(0, BS.Status.Count);

            Job job = new Job((string[] arg) => { return arg.Length.ToString(); }, new Owner("tester"), 3, 35);

            // submit job - Status should have 1 element in its set
            job.State = State.Submitted;
            BS.updateStatus(job);
            Assert.AreEqual(1, BS.Status.Count);

            // cancel job - Status should have 0 elements in its set
            job.State = State.Cancelled;
            BS.updateStatus(job);
            Assert.AreEqual(0, BS.Status.Count);

            // add again
            job.State = State.Submitted;
            BS.updateStatus(job);
            Assert.AreEqual(1, BS.Status.Count);

            // run it - Status shouldn't change
            job.State = State.Running;
            BS.updateStatus(job);
            Assert.AreEqual(1, BS.Status.Count);

            // fail it - Status should have 0 elements
            job.State = State.Failed;
            BS.updateStatus(job);
            Assert.AreEqual(0, BS.Status.Count);

            // add again
            job.State = State.Submitted;
            BS.updateStatus(job);
            Assert.AreEqual(1, BS.Status.Count);

            // terminate it - Status should have 0 elements
            job.State = State.Terminated;
            BS.updateStatus(job);
            Assert.AreEqual(0, BS.Status.Count);
        }

        [TestMethod]
        public void changeState()
        {
            BenchmarkSystem BS = new BenchmarkSystem();
            Job job = new Job((string[] arg) => { return arg.Length.ToString(); }, new Owner("tester"), 3, 35);

            BS.Submit(job);
            BS.changeState(job, State.Running);

            Assert.IsTrue(job.State == State.Running);
        }

    }
}
