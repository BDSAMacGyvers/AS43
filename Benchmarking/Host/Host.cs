using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchService;
using System.ServiceModel;
using SchedulingBenchmarking;

namespace Host
{
    class Host
    {
        public static BenchmarkSystem BS = new BenchmarkSystem();
        
        static void Main(string[] args)
        {
                
            using(ServiceHost host = new ServiceHost(typeof(Service1))){
                host.Open();

                Console.WriteLine("Webservice ready for action");
                Console.ReadKey();

                


                }
        }
    }
}
