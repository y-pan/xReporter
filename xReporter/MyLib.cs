using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace xReporter
{
    class MyLib
    {
        public static void RemoveFolder(string path) {
            //if (Directory.Exists(path)) Directory.Delete(path, true);  // may throw folder not empty exception

            string[] files = Directory.GetFiles(path);
            string[] dirs = Directory.GetDirectories(path);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                RemoveFolder(dir);
            }

            Directory.Delete(path, false);
        }

        public static void CreateFolder(string path) {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        }


        public static DateTime getFileTime(string containerPath, string targetFolderName, bool isFrom)
        {
            // containerPath is remote results folder path, like: x:\Roof\results
            // targetFolderName is like: RDM72831_PR00_CreateFolder_20170307112524043
            string targetFolderPath = containerPath + "\\" + targetFolderName;
            if (!Directory.Exists(containerPath) || !Directory.Exists(targetFolderPath)) return DateTime.MinValue;

            DirectoryInfo di = new DirectoryInfo(targetFolderPath);
            var f= di.GetFiles().Where(file => file.Name.StartsWith("Result_" + targetFolderName.Substring(0, targetFolderName.Length - 18))).First();
            return isFrom ? f.CreationTime : f.LastWriteTime;
        }

        public static int FolderCount(string path) {
            int count = 0;
            if (Directory.Exists(path))
            {
                string[] fs = Directory.GetDirectories(path);
                count = fs.Count();
            }
            return count;
        }

        public static long GetTime(string name)
        {
            if (name.Length < 17) return 0;
            long t;
            bool isOk = long.TryParse(name.Substring(name.Length - 17, 17), out t);
            return isOk ? t : 0;
        }
        public static string GetData(string path)
        {
            string data="";
            if (File.Exists(path))
            {
                data = ReadEndTokens(path, 1, UTF8Encoding.ASCII, "\n");  // read last line of file
                // data is like : </tbody><tfoot><tr><th bgcolor='#33FF00'>PASSED</th><th>Steps:65</th><th>Passed Validations:4 &nbsp; Failed Validations:0</th><th>&nbsp;</th><th>27.17 seconds</th><th>&nbsp;</th><th>&nbsp;</th></tr></tfoot></table>
                data = data.Replace("</tbody><tfoot><tr><th bgcolor='#33FF00'>", "");
                data = data.Replace("</tbody><tfoot><tr><th bgcolor='red'>", "");
                data = data.Replace("seconds</th><th>&nbsp;</th><th>&nbsp;</th></tr></tfoot></table>","");
                data = data.Replace("</th><th>Steps:", "|");
                data = data.Replace("</th><th>Passed Validations:", "|");
                data = data.Replace("&nbsp; Failed Validations:", "|");
                data = data.Replace("</th><th>&nbsp;</th><th>", "|");
                data = data.Replace(" ", "");
                data = data.Replace(",", ""); // in case of number like: 1,234.56
                data = data.Replace("|", ","); // now make it csv format
            }
            return data;
        }

        public static string[] GetDataArray(string path)
        {
            //will return result like: [PASSED,112,15,0,198.15]

            // this is for situation that no steps executed
            if (!(File.ReadAllText(path).Contains(@"</td></tr>"))) return new string[] {"ERROR","0","0","0","0"};
            string data = "";
            
            if (File.Exists(path))
            {
                data = ReadEndTokens(path, 1, UTF8Encoding.ASCII, "\n");  // read last line of file
                // data is like : </tbody><tfoot><tr><th bgcolor='#33FF00'>PASSED</th><th>Steps:65</th><th>Passed Validations:4 &nbsp; Failed Validations:0</th><th>&nbsp;</th><th>27.17 seconds</th><th>&nbsp;</th><th>&nbsp;</th></tr></tfoot></table>
                data = data.Replace("</tbody><tfoot><tr><th bgcolor='#33FF00'>", "");
                data = data.Replace("</tbody><tfoot><tr><th bgcolor='red'>", "");
                data = data.Replace("seconds</th><th>&nbsp;</th><th>&nbsp;</th></tr></tfoot></table>", "");
                data = data.Replace("</th><th>Steps:", "|");
                data = data.Replace("</th><th>Passed Validations:", "|");
                data = data.Replace("&nbsp; Failed Validations:", "|");
                data = data.Replace("</th><th>&nbsp;</th><th>", "|");
                data = data.Replace(" ", "");
                data = data.Replace(",", ""); // in case of number like: 1,234.56
                //data = data.Replace("|", ","); // now make it csv format
            }
            string[] dataArray = data.Split('|');
            return dataArray;
        }


        public static string ReadEndTokens(string path, Int64 numOfTokens, Encoding encoding, string tokenSeparator)
        {
            int sizeOfChar = encoding.GetByteCount("\n");
            byte[] buffer = encoding.GetBytes(tokenSeparator);
            using(FileStream fs = new FileStream(path, FileMode.Open))
            {
                Int64 tokenCount = 0;
                Int64 endPosition = fs.Length / sizeOfChar;
                
                for(Int64 pos = sizeOfChar; pos < endPosition; pos+=sizeOfChar)
                {
                    fs.Seek(-pos, SeekOrigin.End);
                    fs.Read(buffer, 0, buffer.Length);

                    if(encoding.GetString(buffer) == tokenSeparator)
                    {
                        tokenCount++;
                        if(tokenCount == numOfTokens)
                        {
                            byte[] returnBuffer = new byte[fs.Length - fs.Position];
                            fs.Read(returnBuffer, 0, returnBuffer.Length);
                            return encoding.GetString(returnBuffer);
                        }
                    }
                }
                // handle case where number of tokens in file is less than numberOfTokens
                fs.Seek(0, SeekOrigin.Begin);
                buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                return encoding.GetString(buffer);

            }
        }
        public static string FixResultName(string name) {
            name = name.Substring(7);  // remove the leading: Result_
            name = name.Substring(0,name.Length - 4); // remove ending: .htm
            return name;
        }

        public static string dumpBatComment(string target)
        {
            string[] dumpPtns = new string[] { @"^\s*rem\s+.*$", @"\s+rem\s+.*$", @"^\s*::.*$", @"\s+::.*$" };
            string str = Regex.Replace(target, string.Join("|", dumpPtns), "");
            str = str.Replace("/", "\\");
            return str;
        }
        public static bool isBatFormat(string target)
        {
            string batFormat = @"^\s*CALL.*Roof[.]bat\s+.*";
            return Regex.IsMatch(target, batFormat, RegexOptions.IgnoreCase);
        }

        public static void CopyAll(string sourcePath, string destPath)
        {
            //Now Create all of the directories

            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourcePath, destPath));
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories))
            { File.Copy(newPath, newPath.Replace(sourcePath, destPath), true); }

        }

        public static string getLastPartInPath(string path)
        {
            path = path.Replace("\"", "").Replace("'","").Trim();
            string[] parts = path.Split('\\','/');
            return parts.Last();
        }
        public static string getSecondLastPartInPath(string path)
        {
            path = path.Replace("\"", "").Replace("'", "").Trim();
            string[] parts = path.Split('\\', '/');
            if (parts.Length >= 2) return parts[parts.Length - 2];
            else return "";
            
        }

        public static string replaceVarsInLine(string line, string sourcePath)
        {
            // pass in: "%VAR1%\%VAR3%%VAR4%\%VAR2%\dummydummy\file.htm"  ".\test.bat"
            if (!line.Contains("%")) return line;

            string str1 = line;

            // know vars
            MatchCollection matches = Regex.Matches(str1, "%[^%]*%");
            List<KeyValue> kvs = new List<KeyValue>();
            foreach (var m in matches)
            {
                KeyValue kv = new KeyValue(m.ToString().Replace("%", ""));
                kvs.Add(kv);
            }

            // know values

            string[] allLines = File.ReadAllLines(sourcePath);

            foreach (var kv in kvs)
            {
                foreach (string l in allLines)
                {
                    string s = MyLib.dumpBatComment(l);
                    if (s.ToLower().Contains(kv.key.ToLower()) && s.Contains("="))
                    {
                        s = s.Replace("\"", "").Replace("'", "").Replace("/", "\\").Trim(); // reformat
                        string[] arr = s.Split('=', ' '); // space in line would be trouble
                        if (arr.Length > 1)
                        {
                            for (int i = 0; i < arr.Length; i++)
                            {
                                if (arr[i] == kv.key.ToString() && i < arr.Length - 1)
                                {
                                    kv.value = arr[i + 1];
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            foreach (var kv in kvs)
            {
                if (kv.value != "") str1 = str1.Replace("%" + kv.key + "%", kv.value);
            }

            return str1;

        }

        /*
        public static string getTimeStampFrom(string name, bool isFolderName = true)
        {
            string ts;
            string _name;
            if (!isFolderName) _name = MyLib.getSecondLastPartInPath(name);
            else _name = name;

            if (_name.Length <= 17) return "0";
            //ts = _name.Substring(_name.Length - 17,14);
            ts = _name.Substring(_name.Length - 17);

            return ts;

        }
        public static bool CheckPass(string data)
        {   // data is like: Defect82111_TC7_AdHierCKNodeCKLeaf,PASSED,112,15,0,198.15
            string[] arr = data.Split(',');
            bool isPass = false;
            if(arr.Length == 6 && arr[1] == "PASSED")
            {
                isPass = true;
            }
            return isPass;
        }
                 
        public static void GenerateCSV(List<string> namelist,List<string> fullnamelist, string csvPath)
        {
            int count = fullnamelist.Count;
            if (count <= 0) { return; }

            if (File.Exists(csvPath)) { File.Delete(csvPath); }
            File.WriteAllText(csvPath,"Child,Status,StepsExecuted,PassVal,FailVal,Time\n");
            //Defect82111_TC7_AdHierCKNodeCKLeaf,PASSED,112,15,0,198.15
            for (int i = 0; i<count; i++)
            {
                string child = namelist[i];
                string data = MyLib.GetData(fullnamelist[i]);
                File.AppendAllText(csvPath, child +","+data + "\n");
                
            }

        }
                 public static bool PathIsFolder(string path, string folder) {
            string[] array = path.Split('\\');
            if(array.Last() == folder) { return true; }
            else { return false; }
        }
        public static bool PathIsFolder(string path, string folder, string parentFolderName)
        {
            string[] array = path.Split('\\');
            if (array.Last() == folder && array[array.Length - 2] == parentFolderName) { return true; }
            else { return false; }
        }
        public static bool PathHasSubFolder(string path, string subfolder) {

            return false;
        }
         */
    }
}
