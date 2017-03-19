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
        public string resultPath;
        public string batPath = "";
        public string testPath = "";

        public string resultFolderPath = "";
        public string testFolderPath = "";
        
        public string parent = "";
        public string[] data;
        public int index;

        public string getTestFolderPath() { return this.testFolderPath; }
        public string getTestPath()
        {
            return this.testPath;
        }
        public Record(string folderpath, string resultName, string resultPath, string binFolderPath, int index)
        {
            this.name = MyLib.FixResultName(resultName);
            this.resultPath = resultPath;
            this.resultFolderPath = folderpath;
            this.parseData(); // will get this.data
            this.setIsPass();
            
            this.setPaths(binFolderPath);
            this.index = index;
            
        }

        public void setPaths(string binFolderPath) {
            if(binFolderPath.Length > 0 && Directory.Exists(binFolderPath))
            {
                //1. find batPath, targetLine (containing this.name)
                string[] batPaths = Directory.GetFiles(binFolderPath, "*.bat");
                string targetLine = "";
                
                foreach (var b in batPaths)
                {
                    string[] all = File.ReadAllLines(b);
                    foreach (string line in all)
                    {
                        string str = MyLib.dumpBatComment(line);
                        //if (str.ToLower().Contains(this.name.ToLower()) && MyLib.isBatFormat(str))
                        if (str.ToLower().Contains("\\"+this.name.ToLower()+".htm") && MyLib.isBatFormat(str))
                        {
                            targetLine = str;
                            this.batPath = b;
                            break;
                        }
                    }
                }

                //  extract from targetLine: CALL ..\..\Roof.bat "-DseleniumTestCases=%ROOF_LOCAL%\RDM\SetUIVars_RDM.htm;RDM\tests\FVT\Administration\RDM3642_CompoundKeys\RDM3642_CompoundKeys_RDM799.htm"

                //2. find testPath
                this.testPath = targetLine;
                this.testPath = this.testPath.Replace('/', '\\').Replace("\"", "").Replace("\'", "").Replace("%ROOF_LOCAL%","");
                string[] arr = this.testPath.Split(' ',';');
                for (int i = arr.Length - 1; i >= 0; i--)
                {
                    if (arr[i].EndsWith(this.name + ".htm"))
                    {
                        this.testPath = arr[i];
                        break;
                    }
                }
                // in case of %RDM12345_PATH%
                if (this.testPath.Contains("%")) this.testPath = MyLib.replaceVarsInLine(this.testPath, this.batPath);

               
                // 3. find testFolderPath  File.Exists(this.testPath) always returns false even though testPath exist and valid, better to fix it

                if (this.testPath.Length > 0) this.testFolderPath = Path.GetDirectoryName(this.testPath);  // relative    like in bat \RDM\XXX\YYY\
                
                
                //3. find parent ( use it in csv )
                targetLine = MyLib.replaceVarsInLine(targetLine, this.batPath);
                targetLine = targetLine.Replace('/', '\\').Replace("\"", "|").Replace("\'", "|").Replace(".htm", "|").Replace(";", "").Trim();


                string[] targetLineArray = targetLine.Replace('/', '\\').Split('\\','|');
                for (int i = targetLineArray.Length - 1; i > 0; i--) // if i=0 then parent not valid
                {
                    if (targetLineArray[i] == this.name) { this.parent = targetLineArray[i - 1]; break; }
                }

                //if (this.parent.Contains("%") && this.batPath != "") this.parent = this.findVariableValue(this.batPath);

            }
        }

        

        public void parseData()
        {
            // path -> data: [PASSED,112,15,0,198.15]
            this.data = MyLib.GetDataArray(this.resultPath);
            
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
                    //this.data[0] = "ERROR";
                    this.data = new string[] { "ERROR", "", "", "", "" };
                }
            }
        }
        public string getDataStr()
        {
            string str="";
            int len = this.data.Length;
            for(int i= 0; i< this.data.Length; i++ )
            {
                str += this.data[i];
                if (i < this.data.Length - 1) { str += ","; }
            }
            return str;
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


        // ------------------- not in use, MyLib.replaceVarsInLine will do similar and better job --------------------
        private string findVariableValue(string batFilePath)
        {
            string parentVar = this.parent.Replace("%", "");
            string[] all = File.ReadAllLines(batFilePath);
            string varDefinitionLine = "";
            foreach (string line in all)
            {
                string str = MyLib.dumpBatComment(line);
                if (str.ToLower().Contains(parentVar.ToLower()) && str.Contains("="))
                {
                    varDefinitionLine = str.Replace("\"", "").Replace("'", "").Replace("/", "\\").Trim();
                    string[] strs = varDefinitionLine.Split('=', ' '); // space in line would be trouble
                    if (strs.Length > 1)
                    {
                        for (int i = 0; i < strs.Length; i++)
                        {
                            if (strs[i] == parentVar && i < strs.Length - 1)
                            {
                                return MyLib.getLastPartInPath(strs[i + 1]);
                            }
                        }
                    }
                }
            }
            return "";
        }
        
    }
}
