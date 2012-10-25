using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchedulingBenchmarking
{
    partial class DAO
    {


        public void getLastXDays(int x)
        {

            using (var dbContext = new Model1Container())
            {
                DateTime span = DateTime.Today.AddDays(-x);
                var lastTenDays = from db in dbContext.DbLogs
                                  where (db.timeStamp > span)
                                  select db.user;
            }

        }


    }
}
