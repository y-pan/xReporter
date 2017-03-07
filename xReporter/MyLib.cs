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
            if (Directory.Exists(path)) Directory.Delete(path, true);
        }
        public static void CreateFolder(string path) {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        }
        public static bool PathIsFolder(string path, string folder) {
            string[] array = path.Split('\\');
            if(array.Last() == folder) { return true; }
            else { return false; }
        }
        public static bool PathHasSubFolder(string path, string subfolder) {

            return false;
        }
        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            /*if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }*/

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            /*if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }*/

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }


        public static void CopyAll(string sourcePath, string destPath) { 
            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", 
                SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(sourcePath, destPath));

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", 
                SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(sourcePath, destPath), true);

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


        // not in use , put it in main
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
        public static string dumpBatComment(string target)
        {
            string[] dumpPtns = new string[] { @"^\s*rem\s+.*$", @"\s+rem\s+.*$", @"^\s*::.*$", @"\s+::.*$" };
            string str = Regex.Replace(target, string.Join("|", dumpPtns), "");
            return str;
        }
        public static bool isBatFormat(string target)
        {
            string batFormat = @"^\s*CALL.*Roof[.]bat\s+.*";
            return Regex.IsMatch(target, batFormat, RegexOptions.IgnoreCase);
        }

        public static string getLastPartInPath(string path)
        {
            path = path.Replace("\"", "").Replace("'","").Trim();
            string[] parts = path.Split('\\','/');
            return parts.Last();
        }
    }
}
