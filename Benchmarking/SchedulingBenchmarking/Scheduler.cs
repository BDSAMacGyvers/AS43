using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SchedulingBenchmarking
{
    /// <summary>
    /// An enum describing the the max running times of jobs in each category (queue)
    /// </summary>
    public enum JobTimes
    {
        quick = 500, 
        medium = 2000, 
        vLong = 5000
    }

    /// <summary>
    /// Internal class that handles scheduling of tasks. 
    /// </summary>
    /// <todo>
    /// Jobs can only be run is the required amount of CPUs is available
    /// Allow for job delayed when waiting for ressources. Described here:
    /// Jobs should, in general, be run according to their submission time. However, if the next job requires more CPUs that are available, you should delay the current job and select the next one that can actually be run. 
    /// A job can be delayed at most 2 times.
    /// </todo>
    public class Scheduler
    {
        Queue<Job> ShortQueue;
        Queue<Job> MediumQueue;
        Queue<Job> LongQueue;

        Queue<Job> delayedOnceQueue = new Queue<Job>();
        Queue<Job> delayedTwiceQueue = new Queue<Job>();

        HashSet<Job> removedJobs;
        private int JobCounter = 0;


        //singleton field
        private static Scheduler instance = new Scheduler();

        private Scheduler()
        {
            ShortQueue = new Queue<Job>();
            MediumQueue = new Queue<Job>();
            LongQueue = new Queue<Job>();
            removedJobs = new HashSet<Job>();
        }

        public void addJob(Job job)
        {
            job.jobId = JobCounter++;
            if (JobCounter > int.MaxValue - 2) JobCounter = 0;
            int time = job.ExpectedRuntime;

            if (time < (int)JobTimes.quick)
                ShortQueue.Enqueue(job);

            if (time >= (int)JobTimes.quick && time < (int)JobTimes.vLong)
                MediumQueue.Enqueue(job);

            if (time >= (int)JobTimes.vLong)
                LongQueue.Enqueue(job);
        }

        /// <summary>
        /// Method to get the newest job between the three queues. 
        /// It does so by simoply querying for their respective newest job
        /// NOTE: henter den ikke "OldestJob" altså det job, der blev added først???? I så fald er det en misvisende titel.
        /// </summary>
        /// <todo> Could be a one-liner ?</todo>
        /// <returns> The newest job </returns>
        private Job getNewestJob()
        {        
            /// Create a list of the three times
            List<Job> timedJob = new List<Job>();
            if (ShortQueue.Count > 0) 
                timedJob.Add(ShortQueue.ElementAt(0));
            if (MediumQueue.Count > 0)
                timedJob.Add(MediumQueue.ElementAt(0));
            if (LongQueue.Count > 0)
                timedJob.Add(LongQueue.ElementAt(0));

            /// Return the most recent of the previously found three values
            return timedJob.OrderBy(job => job.jobId).ElementAtOrDefault(0);
        }

        /// <summary>
        /// "Remove a job" in the sense that we are not really removing anything at this stage.
        /// Simply mark it as removed and check for it when popping
        /// </summary>
        /// <param name="job"> Job to remove</param>
        public void removeJob(Job job)
        {
            removedJobs.Add(job);
        }
       

#if DEBUG
        public Job popJob()
        {
            return popJob(30);
        }
#endif
        /// <summary>
        /// Function to "pop", return and remove the newest element
        /// Do so by finding the newest job between the three queues. 
        /// Check if we have marked that job for removal. If so do not return it, but instead return the next job
        /// 
        /// </summary>
        /// <returns> The popped job or null if there's no job to return</returns>
        public Job popJob(int cores, Boolean isDelayed=false)
        {
            Job job = null;
            if (!isDelayed && delayedTwiceQueue.Count > 0)
                return delayedTwiceQueue.Dequeue();// Always return the oldest job that has been delayed twice
            else if (!isDelayed && delayedOnceQueue.Count > 0)
            {
                job = delayedOnceQueue.Dequeue();
                if (job.CPUsNeeded > cores)
                {
                    delayedTwiceQueue.Enqueue(job);
                    return popJob(cores, true);
                }
                else
                    return job;
            }

            Job newestJob = getNewestJob();

            // Object that we will return
            Job popped = null;

            /*
             * Look at the time! And determine which queue is suitable
             */
            if (newestJob.ExpectedRuntime < (int)JobTimes.quick)
                popped = ShortQueue.Dequeue();

            else if (newestJob.ExpectedRuntime >= (int)JobTimes.quick && newestJob.ExpectedRuntime < (int)JobTimes.vLong)
                popped = MediumQueue.Dequeue();

            else if (newestJob.ExpectedRuntime >= (int)JobTimes.vLong)
                popped = LongQueue.Dequeue();

            /// If found jobs requires more cpus than we have
            if (popped.CPUsNeeded > cores)
            {
                delayedOnceQueue.Enqueue(popped);
                return popJob(cores, true);
            }
            // Check if the popped job is actually a removed one. 
            // If so we should remove the mark and recursively return the next job in line
            if (removedJobs.Contains(popped))
            {
                removedJobs.Remove(job);
                return popJob(cores);
            }

            return popped;
        }

        /// <summary>
        /// Method to check whether the three queues are all empty
        /// </summary>
        /// <returns> Boolean if all queues are empty</returns>
        public bool Empty()
        {
            Job job1, job2, job3;
            try
            {
                job1 = (ShortQueue.Count > 0) ? ShortQueue.Peek() : null;
                job2 = (MediumQueue.Count > 0) ? ShortQueue.Peek() : null;
                job3 = (LongQueue.Count > 0) ? LongQueue.Peek() : null;
            }
            catch (InvalidOperationException e)
            {
                //Console.WriteLine("something wrong with peek in Empty() in Scheduler");
                return false;
            }
            if (job1 == null && job2 == null && job3 == null) return true;
            else
            {
                if (removedJobs.Contains(job1))
                {
                    removedJobs.Remove(job1);
                    ShortQueue.Dequeue();
                    return Empty();
                }
                else if (removedJobs.Contains(job2))
                {
                    removedJobs.Remove(job2);
                    MediumQueue.Dequeue();
                    return Empty();
                }
                else if (removedJobs.Contains(job3))
                {
                    removedJobs.Remove(job3);
                    MediumQueue.Dequeue();
                    return Empty();
                }
                return false;
            }


            return false;
        }

        public static Scheduler getInstance()
        {
#if DEBUG
            return new Scheduler();
#else
            return instance;
#endif
        }
    }
}
