using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SchedulingBenchmarking;
using System.Threading.Tasks;


namespace BenchService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class Service1 : IService1
    {
       
        public int RequestStatus()
        {
            return Bench.Bench.BS.Cores;
        }

        public bool ReceiveJob(int cpus, int duration, string ownername)
        {
            Job job = new Job((string[] arg) => { foreach (string s in arg) { } return ""; }, new Owner(ownername), cpus, duration);
            Bench.Bench.BS.Submit(job);

            Task task = Task.Factory.StartNew(()=>Bench.Bench.BS.ExecuteAll());

            return true;
        }
    }
}
