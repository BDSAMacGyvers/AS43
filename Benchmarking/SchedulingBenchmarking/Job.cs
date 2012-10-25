using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SchedulingBenchmarking
{
    /// <summary>
    /// This class represents a job to be executed.
    /// </summary>
    public class Job
    {
        // a delegate for the processing method
        #if DEBUG
        public Func<string[], string> del;
        #else
        private Func<string[], string> del;
        #endif
                
        public int jobId;
        
        public int ExpectedRuntime;

        public State State;

        public Owner Owner;

        private int cpusneeded;
        public int CPUsNeeded
        {
            get 
            { 
                return cpusneeded; 
            }
            set 
            {
                if (value >= 1 && value <= 10)
                    cpusneeded = value;
                else throw new ArgumentOutOfRangeException("CPUsNeeded", "The number of CPUs needed should be between 1 and 10. \nYou specified: " + value);
            }
        }

        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="del">The function that are executed when job.process is called</param>
        /// <param name="owner"> the owner of the job</param>
        /// <param name="cpus">number of cpus required</param>
        /// <param name="expectedRuntime">the expected runtime in minutes</param>

        public Job(Func<string[], string> del, Owner owner, int cpus, int expectedRuntime)
        {
            this.del = del;
            this.Owner = owner;
            this.CPUsNeeded = cpus;
            this.ExpectedRuntime = expectedRuntime;
        }

        public Job()
        {
        }

        /// <summary>
        /// This method calls the supplied delegate method. 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public string Process(string[] args)
        {
            Thread.Sleep(ExpectedRuntime);
            return del(args);
        }

        // representation to be used with status array
        public override string ToString() 
        { 
            return "Job added: " +jobId+" owner: "+ Owner.Name;
        }    
    }    
}
