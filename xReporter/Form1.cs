using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xReporter
{

    public partial class xReporter : Form
    {
        string remoteBin;
        string remoteResults;
        string remoteTests;
        string localResults;
        string localTmp;
        string btnOpenLResultsText = "Open Local Results";
        bool isRunning;
        int resultFolderCount;
        int htmFileCount;
        List<string> filesFound;
        public xReporter()
        {
            InitializeComponent();
            
        }
        private void xReporter_Load(object sender, EventArgs e)
        {
            localResults = @".\roof\results";
            localTmp = @".\roof\tmp";
            createFolders();
            rdVM1.Checked = true;
            isRunning = false;

            refreshCount();
            
        }
        private void createFolders() {
            
            System.IO.Directory.CreateDirectory(localResults);
            System.IO.Directory.CreateDirectory(localTmp);
        }

        private void createFile(string pathString) {
            if (!System.IO.File.Exists(pathString))
            {
                using (System.IO.FileStream fs = System.IO.File.Create(pathString))
                {
                    for (byte i = 0; i < 100; i++)
                    {
                        fs.WriteByte(i);
                    }
                }
            }
            else
            {
                MessageBox.Show("File \"{0}\" already exists.", pathString);
            }
        }




        private void btnEmptyLocalResults_Click(object sender, EventArgs e)
        {
            if (!isRunning && resultFolderCount > 0 && MessageBox.Show("Are you sure to empty local results?", "Empty Local Results", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                isRunning = true;
                lbInfo.Text = "Info: empty results ...";
                MyLib.RemoveFolder(localResults);
                System.Threading.Thread.Sleep(100);
                MyLib.CreateFolder(localResults);
                refreshCount();
                isRunning = false;
                lbInfo.Text = "Info: results emptied!";
            }
        }

        private void btnGenerateReports_Click(object sender, EventArgs e)
        {
            isRunning = true;
            rdVM1.Enabled = false;
            rdVM2.Enabled = false;
            // step 1 : get full list of local results: 
            //     s.Name -> show, 
            //     s.FullName -> shotcut to open
            //     
            DirectoryInfo directoryInfo = new DirectoryInfo(@"roof\results");
            var result = directoryInfo.GetFiles("Result_*.htm", SearchOption.AllDirectories).OrderBy(t => t.LastWriteTime).ToList();
            htmFileCount = result.Count;
            lbHtmCount.Text = htmFileCount.ToString();

            foreach (var s in result)
            {
                lbxResults.Items.Add(s.Name);
            }
            //lbxResults.MouseDoubleClick += LbxResults_MouseDoubleClick;

            // step 2: loop through all file and find info, store in List<data>
            // data is a class object of parent,child,STATUS,STEPS,PassVal,FailVal,TIME
            // step 3: generate csv from List<Array>
        }


        // ---------------------------------- EVENTS --------------------------
        private void rdVM1_CheckedChanged(object sender, EventArgs e)
        {
            gbVM2.ForeColor = Color.Black;
            rdVM2.ForeColor = Color.Black;
            gbVM1.ForeColor = Color.Blue;
            rdVM1.ForeColor = Color.Blue;
        }

        private void rdVM2_CheckedChanged(object sender, EventArgs e)
        {
            gbVM1.ForeColor = Color.Black;
            rdVM1.ForeColor = Color.Black;
            gbVM2.ForeColor = Color.Blue;
            rdVM2.ForeColor = Color.Blue;
        }




        private void txtRemoteRoof_TextChanged(object sender, EventArgs e)
        {
            // s:\Documents\rlogger\Roof
            TextBox tb = (TextBox)sender;
            bool isOne = (tb == txtRemoteRoof1) ? true : false;
            bool isResultsFound = false;
            string rootPath = string.Format(""+tb.Text);

            if (Directory.Exists(rootPath))
            {
                string[] subfolders = Directory.GetDirectories(rootPath);
                // before loop
                if (isOne) {
                    // Feature
                    comFeature1.Items.Clear();
                }
                else
                {
                    comFeature2.Items.Clear();
                }
                // in loop
                foreach (string p in subfolders)
                {
                    if (isOne)
                    {
                        comFeature1.Items.Add(p);
                        if (MyLib.PathIsFolder(p, "results") && !isResultsFound) { txtRemoteResults1.Text = p; isResultsFound = true; }
                    }
                    else
                    {
                        comFeature2.Items.Add(p);
                        if (MyLib.PathIsFolder(p, "results") && !isResultsFound) { txtRemoteResults2.Text = p; isResultsFound = true; }
                    }
                }
                // after loop
                if (isOne)
                {
                    // Feature
                    if(comFeature1.Items.Count > 0) comFeature1.SelectedIndex = 0;
                    else comFeature1.Text = "";

                }
                else
                {
                    if (comFeature2.Items.Count > 0) comFeature2.SelectedIndex = 0;
                    else comFeature2.Text = "";
                }
            }
            else  // roof not exists
            {
                // results
                if (isOne) {
                    comFeature1.Items.Clear();
                    comFeature1.Text = "";
                    txtRemoteResults1.Text = "";
                }
                else
                {
                    comFeature2.Items.Clear();
                    comFeature2.Text = "";
                    txtRemoteResults2.Text = "";
                }
            }
        }


        private void txt2Open_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            string path = string.Format(""+tb.Text);
            if (Directory.Exists(path))
            {
                Process.Start(path);
            }
        }

        private void comFeature_Changed(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;

            string featurePath = string.Format("" + cb.SelectedItem);
            string binPath = featurePath + @"\bin";
            string testsPath = featurePath + @"\tests";

            bool isBinFound = false;
            bool isTestsFound = false;
            bool isOne = (cb == comFeature1) ? true : false;
            if (Directory.Exists(featurePath))
            {
                string[] subfolders = Directory.GetDirectories(featurePath);
                
                foreach( string p in subfolders)
                {
                    if(!isBinFound && MyLib.PathIsFolder(p,"bin"))
                    {
                        if (isOne) txtRemoteBin1.Text = p;
                        else txtRemoteBin2.Text = p; 
                        isBinFound = true;
                    }
                    if(!isTestsFound && MyLib.PathIsFolder(p, "tests"))
                    {
                        if (isOne) txtRemoteTests1.Text = p;
                        else txtRemoteTests2.Text = p;
                        isTestsFound = true;
                    }

                }
                if (!isBinFound) {
                    if(isOne)txtRemoteBin1.Text = "";
                    else txtRemoteBin2.Text = "";
                }
                if (!isTestsFound) {
                    if(isOne)txtRemoteTests1.Text = "";
                    else txtRemoteTests2.Text = "";
                }
            }
            else
            {
                if (isOne) txtRemoteBin1.Text = "";
                else txtRemoteBin2.Text = "";
                if (isOne) txtRemoteTests1.Text = "";
                else txtRemoteTests2.Text = "";
            }
        }

        private void btnDownloadResults_Click(object sender, EventArgs e)
        {
            int whichResults = cheDownloadResults1.Checked ? 1 : 2;

            if (!isRunning) {
                isRunning = true;
                lbInfo.Text = "Info: download results ...";

                if (rdVM1.Checked && whichResults == 1)
                {
                    string path = txtRemoteResults1.Text;
                    if (Directory.Exists(path)) { MyLib.CopyAll(path, localResults); lbInfo.Text = "Info: results downloaded!"; }
                    else lbInfo.Text = "Error: download failed due to VM results path not valid!";
                }
                else if(rdVM2.Checked && whichResults == 2)
                {
                    string path = txtRemoteResults2.Text;
                    if (Directory.Exists(path)) { MyLib.CopyAll(path, localResults); lbInfo.Text = "Info: results downloaded!"; }
                    else lbInfo.Text = "Error: download failed due to VM results path not valid!";
                }
                else
                {
                    lbInfo.Text = "Error: download failed due to VM results path not valid!";
                }
                refreshCount();
                isRunning = false;
            }
            

        }

        private void btnOpenLocal_Click(object sender, EventArgs e)
        {
            Process.Start(localResults);
            refreshCount();
        }
        private void refreshCount()
        {
            resultFolderCount = MyLib.FolderCount(localResults);
            btnOpenLocal.Text = btnOpenLResultsText + "(" + resultFolderCount + ")";
        }

        private void cheDownloadResults_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            int which = (cb == cheDownloadResults1) ? 1 : 2;
            bool isEnabled = (cb.Checked) ? true : false;
            switch(which)
            {
                case 1:
                    txtResultsFrom1.Enabled = isEnabled ? true : false;
                    txtResultsTo1.Enabled = isEnabled ? true : false;
                    break;
                case 2:
                    txtResultsFrom2.Enabled = isEnabled ? true : false;
                    txtResultsTo2.Enabled = isEnabled ? true : false;
                    break;
                default: break;

            }
        }


        private void btnTest_Click(object sender, EventArgs e)
        {
            /*
            lbInfo.Text = MyLib.GetTime(txtResultsFrom1.Text).ToString();
            filesFound = new List<string>();
            FindAll(@"roof\results");
            lbInfo.Text = filesFound.Count.ToString();
            */

            //1 get results .htm recursively and sorted ascending by LastWriteTime
            DirectoryInfo directoryInfo = new DirectoryInfo(@"roof\results");
            var result = directoryInfo.GetFiles("Result_*.htm", SearchOption.AllDirectories).OrderBy(t => t.LastWriteTime).ToList();
            htmFileCount = result.Count;
            lbHtmCount.Text = htmFileCount.ToString();

            foreach(var s in result)
            {
                lbxResults.Items.Add(s.Name);
            }
            lbxResults.MouseDoubleClick += LbxResults_MouseDoubleClick;
            
        }

        private void LbxResults_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // easy to open htm
            ListBox lb = (ListBox)sender;
            int index = lb.SelectedIndex;
            MessageBox.Show("select:"+index.ToString());

            //throw new NotImplementedException();
        }

        private void FindAll(string path)
        {
            foreach(string d in Directory.GetDirectories(path))
            {
                foreach(string f in Directory.GetFiles(d,"*.htm"))
                {
                    filesFound.Add(f);
                    lbxResults.Items.Add(f);
                }
                FindAll(d);
            }
        }
    }
}
