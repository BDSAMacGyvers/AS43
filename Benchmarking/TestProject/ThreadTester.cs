using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SchedulingBenchmarking;
using System.Threading;
using System.Threading.Tasks;

namespace SchedulingBenchmarking
{
    [TestClass]
    public class ThreadTester
    {
        [TestMethod]
        public void TestDontRunIfCoresNotAvailable()
        {

            BenchmarkSystem Bs = new BenchmarkSystem();
            //job that needs 3 cpus, should execute with no problems.
            Job job = new Job((string[] arg) => { return arg.Length.ToString(); }, new Owner("morten"), 3, 1);
            Assert.AreEqual(State.Created, job.State);
            Bs.Submit(job);
            Task task = Task.Factory.StartNew(() => Bs.ExecuteAll());
            Thread.Sleep(50);
            Assert.AreEqual(State.Terminated, job.State);



        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void TestDontRunWithTooManyCores()
        {
            BenchmarkSystem Bs = new BenchmarkSystem();
            //job that requires 50 cores, should not get the state terminated. And should throw exception before that.
            Job job2 = new Job((string[] arg) => { return arg.Length.ToString(); }, new Owner("morten"), 50, 1);
            Assert.AreEqual(State.Created, job2.State);
            Bs.Submit(job2);
            Task task2 = Task.Factory.StartNew(() => Bs.ExecuteAll());
            Thread.Sleep(50);
            Assert.AreNotEqual(State.Terminated, job2.State);

        }

        [TestMethod]
        public void TestDontRunAtFirstIfCoresNotAvailable()
        {

            BenchmarkSystem Bs = new BenchmarkSystem();
            //jobs that needs in all 27 cpus, should execute with no problems.
            Job job1 = new Job((string[] arg) => { return arg.Length.ToString(); }, new Owner("morten12"), 9, 10000);
            Job job2 = new Job((string[] arg) => { return arg.Length.ToString(); }, new Owner("morten22"), 9, 10000);
            Job job3 = new Job((string[] arg) => { return arg.Length.ToString(); }, new Owner("morten32"), 9, 10000);
            Bs.Submit(job1);
            Bs.Submit(job2);
            Bs.Submit(job3);
            //this job requires too many cores and should therefore not run immediately
            Job job4 = new Job((string[] arg) => { return arg.Length.ToString(); }, new Owner("morten4"), 9, 10000);
            Bs.Submit(job4);
            Task task = Task.Factory.StartNew(() => Bs.ExecuteAll());

            Thread.Sleep(1000);
            Assert.AreEqual(State.Running, job1.State);
            Assert.AreEqual(State.Running, job2.State);
            Assert.AreEqual(State.Running, job3.State);
            // this job should not be running as there aren't enough cores for it to run.
            Assert.AreEqual(State.Submitted, job4.State);
            //it should run after the cores become available
            Thread.Sleep(12000);
            Assert.AreEqual(State.Running, job4.State);


            // NOTE: this test fails because the first three submitted jobs dont run simultaneusly. The factory method of Task should run them in separate threads, but somehow choses to 
            //      run them chronological instead. Maybe you can explain why? Thank you in advance.
        }
    }
}