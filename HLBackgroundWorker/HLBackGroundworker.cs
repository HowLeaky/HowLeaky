using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowLeaky
{
    public class HLBackGroundWorker : BackgroundWorker
    {
        public Simulation Sim { get; set; }

        public HLBackGroundWorker(): base() { }
    }
}
