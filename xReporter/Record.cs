using System;
using System.Collections.Generic;
using System.IO;
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
        public string parent;
        public string[] data;

        public Record(string resultName, string resultPath, string binFolderPath)
        {
            this.name = MyLib.FixResultName(resultName);
            this.path = resultPath;
            this.parent = "";
            this.parseData(); // will get this.data
            this.setIsPass();
            this.setParent(binFolderPath);
        }
        public void setParent(string binFolderPath) {
            if(binFolderPath.Length > 0 && Directory.Exists(binFolderPath))
            {
                //1. goto bin folder, find target line from bat files
                string[] batPath = Directory.GetFiles(binFolderPath, "*.bat");
                string targetLine = "";
                foreach (var b in batPath)
                {
                    string[] all = File.ReadAllLines(b);
                    foreach (string line in all)
                    {
                        string str = MyLib.dumpBatComment(line);
                        if (str.ToLower().Contains(this.name.ToLower()) && MyLib.isBatFormat(str))
                        {
                            targetLine = str;
                            
                            break;
                        }
                    }
                }

                // 2. extra from targetLine: CALL ..\..\Roof.bat "-DseleniumTestCases=%ROOF_LOCAL%\RDM\SetUIVars_RDM.htm;RDM\tests\FVT\Administration\RDM3642_CompoundKeys\RDM3642_CompoundKeys_RDM799.htm"
                targetLine = targetLine.Replace('/', '\\').Replace("\"", "|").Replace("\'", "|").Trim();
                string[] targetLineArray = targetLine.Replace('/', '\\').Split('\\','|');
                for (int i = targetLineArray.Length - 1; i > 0; i--) // if i=0 then parent not valid
                {
                    if (targetLineArray[i] == this.name+".htm") { this.parent = targetLineArray[i-1]; break; }
                }
                // need to deal with parent=SetUIVars_RDM.htm;%RDM50169_PATH%
            }
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
        public string ToString(bool includeParent=true) {
            //Parent,Child,Status,StepsExecuted,PassVal,FailVal,Time\n"
            string result = "";
            if (!includeParent)
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
