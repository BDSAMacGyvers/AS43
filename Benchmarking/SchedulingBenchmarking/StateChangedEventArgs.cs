using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchedulingBenchmarking
{
    public class StateChangedEventArgs : EventArgs
    {
        public State State
        {
            get;
            set;
        }

        public Job Job
        {
            get;
            set;
        }
    }	
}
