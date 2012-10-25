
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace SchedulingBenchmarking
{   
    /// <summary>
    /// This class processes created jobs, fires events at job-start,end,fail and houses the job scheduler.
    /// Currently it doesn't really perform any benchmarks.
    /// 
    /// </summary>
    public class BenchmarkSystem
    {
        private int cores = 30;
        public int Cores
        {
            get { return cores; }
        }

        //The sheduler that holds incoming jobs. 
        #if DEBUG
        public Scheduler scheduler = Scheduler.getInstance();
        #else
        internal Scheduler scheduler = Scheduler.getInstance();
        #endif
        
        // Eventhandler that fires event on stateChange
        public event EventHandler<StateChangedEventArgs> StateChanged;        

        // HashSet that contains a list of all submitted and running jobs, 
        // but not cancelled, failed, or terminated jobs
        public HashSet<Job> Status;

        static void Main(String[] args) 
        {
            
            BenchmarkSystem system = new BenchmarkSystem();

            // get the logger to subscribe to BenchmarkSystem
            system.StateChanged += Logger.OnStateChanged;
            /*
            Job job1 = new Job((string[] arg) => { foreach (string s in arg) { Console.Out.WriteLine(s); } return ""; }, new Owner("owner1"), 2, 45);
            Job job2 = new Job((string[] arg) => { foreach (string s in arg) { Console.Out.WriteLine(s); } return ""; }, new Owner("owner2"), 2, 3);
            Job job3 = new Job((string[] arg) => { foreach (string s in arg) { Console.Out.WriteLine(s); } return ""; }, new Owner("owner3"), 2, 200);
            Job job4 = new Job((string[] arg) => { foreach (string s in arg) { Console.Out.WriteLine(s); } return ""; }, new Owner("owner4"), 2, 3);
            Job job5 = new Job((string[] arg) => { foreach (string s in arg) { Console.Out.WriteLine(s); } return ""; }, new Owner("owner5"), 2, 45);
            Job job6 = new Job((string[] arg) => { foreach (string s in arg) { Console.Out.WriteLine(s); } return ""; }, new Owner("owner6"), 2, 200);
            Job job7 = new Job((string[] arg) => { foreach (string s in arg) { Console.Out.WriteLine(s); } return ""; }, new Owner("owner7"), 2, 45);
            Job job8 = new Job((string[] arg) => { foreach (string s in arg) { Console.Out.WriteLine(s); } return ""; }, new Owner("owner8"), 2, 45);
            Job job9 = new Job((string[] arg) => { foreach (string s in arg) { Console.Out.WriteLine(s); } return ""; }, new Owner("owner9"), 2, 200);
            Job job10 = new Job((string[] arg) => { foreach (string s in arg) { Console.Out.WriteLine(s); } return ""; }, new Owner("owner10"), 2, 3);
            Job job11 = new Job((string[] arg) => { foreach (string s in arg) { Console.Out.WriteLine(s); } return ""; }, new Owner("owner11"), 2, 45);
            Job job12 = new Job((string[] arg) => { foreach (string s in arg) { Console.Out.WriteLine(s); } return ""; }, new Owner("owner12"), 2, 200);
            system.Submit(job1);
            system.Submit(job2);
            system.Submit(job3);
            system.Submit(job4);
            system.Submit(job5);
            system.Submit(job6);
            system.Submit(job7);
            system.Submit(job8);
            system.Submit(job9);
            system.Submit(job10);
            system.Submit(job11);
            system.Submit(job12);
            */
            
            Simulator sim = new Simulator(system.scheduler);

            Task.Factory.StartNew(()=>sim.run());

            system.ExecuteAll();           

            Console.ReadKey();
        }

        public BenchmarkSystem()
        {
            Status = new HashSet<Job>();
        }

        /*** Methods ***/
        
        public void Submit(Job job)
        {
            changeState(job, State.Submitted);
            scheduler.addJob(job);
        }

        public void Cancel(Job job)
        {
            changeState(job, State.Cancelled);
            scheduler.removeJob(job);
        }

        private string ExecuteJob(Job job)
        {
            Console.WriteLine("Executing " + job);

            // start job
            changeState(job, State.Running);

            String result = job.Process(
                new string[] { "Processing job with id " + job.jobId + " owner: " + job.Owner.Name }
                );
            
            // if failed
            if (result == null)
            {
                changeState(job, State.Failed);
                return "Job " + job.jobId + " Failed :(";
            }

            // when finished
            else
            {
                changeState(job, State.Terminated);
                return "Job " + job.jobId + " Succeeded";
            }
        }

        public void ExecuteAll()
        {
            while(true)
                if(!scheduler.Empty()) {            
                    // get job from scheduler
                    Job job = scheduler.popJob(cores);

                    while (cores < job.CPUsNeeded)
                        Thread.Sleep(100);

                    Console.WriteLine("Popped " + job);

                    Task<string> task = Task.Factory.StartNew<string>(()=>ExecuteJob(job));
                }    
        }

        public void changeState(Job job, State state)
        {
            job.State = state;
            fireEvent(new StateChangedEventArgs() { State = state, Job = job });
            updateStatus(job);
        }

        private void fireEvent(StateChangedEventArgs e)
        {
            if (StateChanged != null) StateChanged(this, e);
        }

        public void updateStatus(Job job)
        {
            State state = job.State;
            if (state == State.Submitted)
            {
                Status.Add(job);
            }
            if (state == State.Running)
            {
                cores -= job.CPUsNeeded;
            }
            else if (state == State.Cancelled)
            {
                cores += job.CPUsNeeded;
                Status.Remove(job);
            }
            else if (state == State.Failed)
            {
                cores += job.CPUsNeeded;
                Status.Remove(job);
            }
            else if (state == State.Terminated)
            {
                cores += job.CPUsNeeded;
                Status.Remove(job);
            }
            // if state changes from submitted to running, the object 
            // will change state, but the HashSet won't need updating.
        }     
    }
}
