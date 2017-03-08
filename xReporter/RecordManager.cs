using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xReporter
{
    class RecordManager
    {
        private int count;
        private int ecount;
        public List<Record> records;
        public List<Record> eRecords;
        public int getCount() { return this.count; }
        public int getECount() { return this.ecount; }
        public RecordManager() 
        {
            this.count = 0;
            this.ecount = 0;
            this.records = new List<Record>();
            this.eRecords = new List<Record>();
        }
        public void Clear() {
            this.count = 0;
            this.ecount = 0;
            this.records.Clear();
            this.eRecords.Clear();
        }
        public void addRecord(Record rec) {
            this.records.Add(rec);
            this.count++;
            if(!rec.isPass) { this.eRecords.Add(rec); this.ecount++; }
        }

        public string getPathByIndex(int index, bool forFailed = false)
        {
            if (index < 0) { throw new Exception("Error: Record index not found!"); }
            if (!forFailed)
            {
                return this.records[index].path;
            }
            else
            {
                return this.eRecords[index].path;
            }
        }
        public string getNameByIndex(int index, bool forFailed = false)
        {
            if(index <0) { throw new Exception("Error: Record index not found!"); }
            if (!forFailed)
            {
                return this.records[index].name;
            }
            else
            {
                return this.eRecords[index].name; 
            }
        }
        public Record getRecord(int index)
        {
            return this.records[index];
        }
        public string getStat(bool isFailStat=true)
        {
            if (isFailStat)
            {
                int percentage = this.ecount * 100 / this.count;
                return "failed "+this.ecount + "/" + this.count + "="+percentage+"%";
            }
            else
            {
                int pc = this.count - this.ecount;
                int percentage = this.count * 100 / this.count;
                return  "passed "+pc + "/" + this.count + "=" + percentage+"%";
            }
        }



    }
}

