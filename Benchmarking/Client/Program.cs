using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.BenchService;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Service1Client client = new Service1Client())
            {
                Console.WriteLine("Requesting status");
                int status = client.RequestStatus();
                Console.WriteLine("Current CPUS available: {0}", status);
                Console.WriteLine("Submitting Job");
                bool success = client.ReceiveJob(5, 5000, "Young Ellen");
                Console.WriteLine("Job proccesed successfully: {0}", success);
                Console.ReadKey();
                Console.WriteLine("Requesting status");
                status = client.RequestStatus();
                Console.WriteLine("Current CPUS available: {0}", status);
                success = client.ReceiveJob(5, 5000, "Young Ellen");
                Console.WriteLine("Job proccesed successfully: {0}", success);
                Console.ReadKey();
                Console.WriteLine("Requesting status");
                status = client.RequestStatus();
                Console.WriteLine("Current CPUS available: {0}", status);
                success = client.ReceiveJob(5, 5000, "Young Ellen");
                Console.WriteLine("Job proccesed successfully: {0}", success);
                Console.ReadKey();
                Console.WriteLine("Requesting status");
                status = client.RequestStatus();
                Console.WriteLine("Current CPUS available: {0}", status);
                success = client.ReceiveJob(5, 5000, "Young Ellen");
                Console.WriteLine("Job proccesed successfully: {0}", success);
                Console.ReadKey();
            }
        }
    }
}
