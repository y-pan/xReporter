using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xReporter
{
    class FromToDT
    {
        public DateTime from;
        public DateTime to;
        public bool hasTo = false;  // LastWriteTime
        public bool hasFrom = false; // CreationTime
        public void setTo(DateTime to) { this.to = to; this.hasTo = true; }
        public void setFrom(DateTime from) { this.from = from; this.hasFrom = true; }
        public void init() { this.hasFrom = false; this.hasTo = false; }
        //public FromToDT() { }
    }
}
