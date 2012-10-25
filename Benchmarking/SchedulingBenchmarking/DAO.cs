using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;


namespace SchedulingBenchmarking
{
    /// <summary>
    /// Struct used by the last two linq querys to hold the information from two columns
    /// </summary>
    public struct Entry
    {
        public String State { get; set; }
        public int Count { get; set; }
    }

    public partial class DAO
    {
        // add entry 
        public static void AddEntry(DateTime timeStamp, string jobState, string user, int jobId)
        {
            using (var dbContext = new Model1Container())
            {
                dbContext.Database.Connection.Open();
                DbLog logEntry = new DbLog();

                logEntry.timeStamp = timeStamp;
                logEntry.jobState = jobState;
                logEntry.user = user;
                logEntry.jobId = jobId;

                dbContext.DbLogs.Add(logEntry);
                dbContext.SaveChanges();
            }
        }

        //select all users
        public static List<string> FindUsers()
        {
            using (var dbContext = new Model1Container())
            {
                dbContext.Database.Connection.Open();

                IEnumerable<string> users = (from db in dbContext.DbLogs select db.user).Distinct();

                return users.ToList();
            }
        }

        //select all jobs from a user
        public static List<int> FindAllJobs(String name)
        {
            using (var dbContext = new Model1Container())
            {
                dbContext.Database.Connection.Open();

                IEnumerable<int> jobs = from db in dbContext.DbLogs where db.user == name select db.jobId;

                return jobs.ToList();
            }
        }


        //select all jobs from a user within the past X days
        public static List<int> GetLastXDays(int x, string name)
        {
            using (var dbContext = new Model1Container())
            {
                dbContext.Database.Connection.Open();
                DateTime span = DateTime.Today.AddDays(-x);
                var lastTenDays = from db in dbContext.DbLogs
                                  where (db.timeStamp > span) && db.user == name
                                  select db.jobId;

                return lastTenDays.ToList();
            }

        }

        //select all jobs submitted by a user within a given time period (this includes both the time and the date)
        public static List<int> FindAllSubmitsWithin(string user, DateTime start, DateTime end)
        {
            using (var dbContext = new Model1Container())
            {
                dbContext.Database.Connection.Open();

                IEnumerable<int> submits = from db in dbContext.DbLogs
                                           where
                                               db.user == user && start < db.timeStamp && db.timeStamp < end
                                               && db.jobState == "Submitted"
                                           select db.jobId;

                Console.WriteLine(user + " has within " + start + " and " + end + ":");

                return submits.ToList();
            }

        }
        //return the number of jobs within a given period grouped by their status (queued,running,ended, error). Here the activity log can be useful.
        public static Dictionary<string, int> NrOfJobsWithin(DateTime start, DateTime end)
        {
            using (var dbContext = new Model1Container())
            {
                dbContext.Database.Connection.Open();
                var entries = from db in dbContext.DbLogs
                              where start < db.timeStamp && db.timeStamp < end
                              group db by db.jobState into g
                              select new { State = g.Key, Jobs = g };

                Dictionary<string, int> d = new Dictionary<string, int>();
                foreach (var g in entries)
                {
                    int count = 0;
                    foreach (var job in g.Jobs)
                    {
                        count++;
                    }
                    d.Add(g.State, count);
                }
                return d;
            }
        }

        //perform the same query as above but restricting the query to only one user
        public static Dictionary<string, int> NrOfJobsWithinOne(DateTime start, DateTime end, string user)
        {
            using (var dbContext = new Model1Container())
            {
                dbContext.Database.Connection.Open();
                var entries = from db in dbContext.DbLogs
                              where start < db.timeStamp && db.timeStamp < end && user == db.user
                              group db by db.jobState into g
                              select new { State = g.Key, Jobs = g };

                Dictionary<string, int> d = new Dictionary<string, int>();
                foreach (var g in entries)
                {
                    int count = 0;
                    foreach (var job in g.Jobs)
                    {
                        count++;
                    }
                    d.Add(g.State, count);
                }
                return d;
            }
        }

#if DEBUG
        public static void DropTable()
        {
            using (var dbContext = new Model1Container())
            {
                dbContext.Database.Connection.Open();
                dbContext.Database.ExecuteSqlCommand("truncate table DBLogs");
            }
        }
#else
        // doesn't exist
#endif

    }
}
