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
        int resultsCount;
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
            if (resultsCount > 0 && MessageBox.Show("Are you sure to empty local results?", "Empty Local Results", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                MyLib.RemoveFolder(localResults);
                System.Threading.Thread.Sleep(100);
                MyLib.CreateFolder(localResults);
                refreshCount();
            }
        }

        private void btnGenerateReports_Click(object sender, EventArgs e)
        {
            isRunning = true;
            rdVM1.Enabled = false;
            rdVM2.Enabled = false;
            // do report
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

        private void btnTest_Click(object sender, EventArgs e)
        {
            
        }


        private void txtRemoteRoof_TextChanged(object sender, EventArgs e)
        {
            // s:\Documents\rlogger\Roof
            TextBox tb = (TextBox)sender;
            bool isOne = (tb == txtRemoteRoof1) ? true : false;
            bool isResultsFound = false;
            string rootPath = string.Format(@""+tb.Text);

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
                    comFeature1.SelectedIndex=0;
                }
                else
                {
                    comFeature2.SelectedIndex = 0;
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
            string path = string.Format(@""+tb.Text);
            if (Directory.Exists(path))
            {
                Process.Start(path);
            }
        }

        private void comFeature_Changed(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            string featurePath = string.Format(@"" + cb.SelectedItem);
            string binPath = string.Format(@""+ featurePath + @"\bin");
            string testsPath = string.Format(@"" + featurePath + @"\tests");
            
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

            if (rdVM1.Checked)
            {
                string path = @"" + txtRemoteResults1.Text;
                if(Directory.Exists(path)) MyLib.CopyAll(@""+path, @""+localResults);
            }
            else
            {
                string path = @"" + txtRemoteResults2.Text;
                if (Directory.Exists(path)) MyLib.CopyAll(@"" + path, @"" + localResults);
            }
            refreshCount();

        }

        private void btnOpenLocal_Click(object sender, EventArgs e)
        {
            Process.Start(localResults);
            refreshCount();
        }
        private void refreshCount()
        {
            resultsCount = MyLib.FolderCount(localResults);
            btnOpenLocal.Text = btnOpenLResultsText + "(" + resultsCount + ")";
        }
    }
}
