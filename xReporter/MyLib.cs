using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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




    }
}
