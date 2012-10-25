using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace SchedulingBenchmarking
{
    [TestClass]
    public class DAOtest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            // Delete contents of table
            DAO.DropTable();

            // Insert test contents
            // DateTime is in format year, month, day, hour, minute, second
            DAO.AddEntry(new DateTime(2012, 10, 9, 12, 15, 45), "Submitted", "morten", 1);
            DAO.AddEntry(new DateTime(2012, 10, 9, 12, 16, 45), "Cancelled", "morten", 1);

            DAO.AddEntry(new DateTime(2012, 5, 29, 15, 45, 9), "Submitted", "ellen", 2);
            DAO.AddEntry(new DateTime(2012, 5, 29, 15, 47, 3), "Running", "ellen", 2);
            DAO.AddEntry(new DateTime(2012, 5, 29, 15, 48, 15), "Failed", "ellen", 2);

            DAO.AddEntry(new DateTime(2012, 2, 16, 8, 38, 29), "Submitted", "christoffer", 3);
            DAO.AddEntry(new DateTime(2012, 2, 16, 8, 45, 9), "Running", "christoffer", 3);
            DAO.AddEntry(new DateTime(2012, 2, 16, 8, 50, 14), "Terminated", "christoffer", 3);
        }

        [TestMethod]
        public void TestFindUsers()
        {
            List<string> users = DAO.FindUsers();

            Assert.IsTrue(users.Contains("morten"));
            Assert.IsTrue(users.Contains("ellen"));
            Assert.IsTrue(users.Contains("christoffer"));
            Assert.IsFalse(users.Contains("thejulemanden"));
        }

        [TestMethod]
        public void TestFindAllJobs()
        {

            int preCount = DAO.FindAllJobs("christoffer").Count();
            DAO.AddEntry(new DateTime(2012, 2, 16, 8, 38, 29), "Submitted", "christoffer", 3);
            DAO.AddEntry(new DateTime(2012, 2, 16, 8, 45, 9), "Running", "christoffer", 3);
            DAO.AddEntry(new DateTime(2012, 2, 16, 8, 50, 14), "Terminated", "christoffer", 3);
            int postCount = DAO.FindAllJobs("christoffer").Count();
            Assert.AreEqual(preCount + 3, postCount);
            Assert.IsFalse(preCount == postCount);

        }

        [TestMethod]
        public void TestFindAllSubmitsWithin()
        {
            DateTime start = new DateTime(2010, 2, 16, 8, 38, 29);
            DateTime end = new DateTime(2013, 2, 16, 8, 45, 9);
            int preCount = DAO.FindAllSubmitsWithin("christoffer", start, end).Count();

            DAO.AddEntry(new DateTime(2011, 2, 16, 8, 38, 29), "Submitted", "christoffer", 3);
            DAO.AddEntry(new DateTime(2012, 2, 16, 8, 45, 9), "Running", "christoffer", 3);
            DAO.AddEntry(new DateTime(2009, 2, 16, 8, 50, 14), "Terminated", "christoffer", 3);

            int postCount = DAO.FindAllSubmitsWithin("christoffer", start, end).Count();
            Assert.AreEqual(preCount + 1, postCount);
            Assert.IsFalse(preCount == postCount);
        }

        [TestMethod]
        public void TestGetLastXDays()
        {
            List<int> jobsForUser = DAO.GetLastXDays(120, "morten");

            Assert.IsTrue(jobsForUser.Contains(1));
            Assert.IsFalse(jobsForUser.Contains(3));
        }

        [TestMethod]
        public void TestNrOfJobsWithin()
        {
            Dictionary<string, int> result = DAO.NrOfJobsWithin(new DateTime(2012, 2, 1, 0, 0, 0), new DateTime(2012, 10, 10, 0, 0, 0));

            if (result.ContainsKey("Submitted"))
            {
                int nrOfSubmittedJobs = result["Submitted"];
                Assert.IsTrue(nrOfSubmittedJobs == 3);
            }

            if (result.ContainsKey("Running"))
            {
                int nrOfRunningJobs = result["Running"];
                Assert.IsTrue(nrOfRunningJobs == 2);
            }

            if (result.ContainsKey("Terminated"))
            {
                int nrOfTerminatedJobs = result["Terminated"];
                Assert.IsTrue(nrOfTerminatedJobs == 1);
            }

            if (result.ContainsKey("Failed"))
            {
                int nrOfFailedJobs = result["Failed"];
                Assert.IsTrue(nrOfFailedJobs == 1);
            }

            if (result.ContainsKey("Cancelled"))
            {
                int nrOfCancelledJobs = result["Cancelled"];
                Assert.IsTrue(nrOfCancelledJobs == 1);
            }
        }

        [TestMethod]
        public void NrOfJobsWithinOne()
        {
            Dictionary<string, int> result1 = DAO.NrOfJobsWithinOne(new DateTime(2012, 2, 1, 0, 0, 0), new DateTime(2012, 10, 10, 0, 0, 0), "morten");

            if (result1.ContainsKey("Submitted"))
            {
                int nrOfSubmittedJobs = result1["Submitted"];
                Assert.IsTrue(nrOfSubmittedJobs == 1);
            }

            if (result1.ContainsKey("Running"))
            {
                int nrOfRunningJobs = result1["Running"];
                Assert.IsTrue(nrOfRunningJobs == 0);
            }

            if (result1.ContainsKey("Terminated"))
            {
                int nrOfTerminatedJobs = result1["Terminated"];
                Assert.IsTrue(nrOfTerminatedJobs == 0);
            }

            if (result1.ContainsKey("Failed"))
            {
                int nrOfFailedJobs = result1["Failed"];
                Assert.IsTrue(nrOfFailedJobs == 0);
            }

            if (result1.ContainsKey("Cancelled"))
            {
                int nrOfCancelledJobs = result1["Cancelled"];
                Assert.IsTrue(nrOfCancelledJobs == 1);
            }

            Dictionary<string, int> result2 = DAO.NrOfJobsWithinOne(new DateTime(2012, 2, 1, 0, 0, 0), new DateTime(2012, 10, 10, 0, 0, 0), "ellen");

            if (result2.ContainsKey("Submitted"))
            {
                int nrOfSubmittedJobs = result2["Submitted"];
                Assert.IsTrue(nrOfSubmittedJobs == 1);
            }

            if (result2.ContainsKey("Running"))
            {
                int nrOfRunningJobs = result2["Running"];
                Assert.IsTrue(nrOfRunningJobs == 1);
            }

            if (result2.ContainsKey("Terminated"))
            {
                int nrOfTerminatedJobs = result2["Terminated"];
                Assert.IsTrue(nrOfTerminatedJobs == 0);
            }

            if (result2.ContainsKey("Failed"))
            {
                int nrOfFailedJobs = result2["Failed"];
                Assert.IsTrue(nrOfFailedJobs == 1);
            }

            if (result2.ContainsKey("Cancelled"))
            {
                int nrOfCancelledJobs = result2["Cancelled"];
                Assert.IsTrue(nrOfCancelledJobs == 1);
            }

            Dictionary<string, int> result3 = DAO.NrOfJobsWithinOne(new DateTime(2012, 2, 1, 0, 0, 0), new DateTime(2012, 10, 10, 0, 0, 0), "christoffer");

            if (result3.ContainsKey("Submitted"))
            {
                int nrOfSubmittedJobs = result3["Submitted"];
                Assert.IsTrue(nrOfSubmittedJobs == 1);
            }

            if (result3.ContainsKey("Running"))
            {
                int nrOfRunningJobs = result3["Running"];
                Assert.IsTrue(nrOfRunningJobs == 1);
            }

            if (result3.ContainsKey("Terminated"))
            {
                int nrOfTerminatedJobs = result3["Terminated"];
                Assert.IsTrue(nrOfTerminatedJobs == 1);
            }

            if (result3.ContainsKey("Failed"))
            {
                int nrOfFailedJobs = result3["Failed"];
                Assert.IsTrue(nrOfFailedJobs == 0);
            }

            if (result3.ContainsKey("Cancelled"))
            {
                int nrOfCancelledJobs = result3["Cancelled"];
                Assert.IsTrue(nrOfCancelledJobs == 0);
            }


        }

    }
}
