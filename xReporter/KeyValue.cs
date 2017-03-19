using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xReporter
{
    class KeyValue
    {
        public string key = "";
        public string value = "";
        public KeyValue(string key, string value)
        {
            this.key = key;
            this.value = value;
        }
        public KeyValue(string key)
        {
            this.key = key;

        }
    }
}
