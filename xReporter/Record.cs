﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xReporter
{
    class Record
    {
        public bool isPass;
        public string name;
        public string path;
        public string parent = "";
        public int index;
        public int eIndex;
        public string[] data;

        public Record(string name, string path, int index)
        {
            this.name = MyLib.FixResultName(name);
            this.path = path;
            this.index = index;
            

            this.parseData(); // will get this.data
            this.setIsPass();
            this.setParent();
        }
        public void setParent() {
            // to do
        }
        public void setEIndex(int eIndex) {
            this.eIndex = eIndex;
        }

        public void parseData()
        {
            // path -> data: [PASSED,112,15,0,198.15]
            this.data = MyLib.GetDataArray(this.path);
            
        }
        public void setIsPass() {
            if (this.data.Length == 5 && this.data[0] == "PASSED")
            {
                this.isPass = true;
            }
            
            else {
                this.isPass = false;
                if(this.data.Length < 5)
                {
                    this.data[0] = "ERROR";
                }
            }
        }
        public string getDataStr()
        {
            return string.Format("{0},{1},{2},{3},{4}", this.data[0], this.data[1], this.data[2], this.data[3], this.data[4]);
        }
        public string ToString(bool omitParent = false) {
            //Parent,Child,Status,StepsExecuted,PassVal,FailVal,Time\n"
            string result = "";
            if (omitParent)
            {
                result = string.Format(",{0},{1}",this.name,this.getDataStr());
            }
            else
            {
                result = string.Format("{0},{1},{2}", this.parent,this.name,this.getDataStr());
            }
            return result;
        }
        
    }
}
