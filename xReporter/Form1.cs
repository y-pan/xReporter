using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xReporter
{

    public partial class xReporter : Form
    {
        string log;
        string localResults;
        string localTmp;
        string btnOpenLResultsText0 = "Open Local Results";
        string lbStat0 = "Stat: ";
        bool isRunning;
        int resultFolderCount;
        int htmFileCount;

        RecordManager recordMng;

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
            log = "xReport.csv";
            refreshCount();
            recordMng = new RecordManager();
            toolTip1.SetToolTip(this.cheFindParent, "Check it if you need to see parent name of test in the report");
            toolTip1.SetToolTip(this.cheFailedOnly, "Check it to view Failed test only, doesn't affect the report");
        }

        private void btnEmptyLocalResults_Click(object sender, EventArgs e)
        {
            try
            {
                if (resultFolderCount <= 0) throw new Exception("Already emptied.");
                if (!isRunning && MessageBox.Show("Are you sure to empty local results?", "Empty Local Results", MessageBoxButtons.YesNo) == DialogResult.Yes)
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
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            
        }
        private void btnGenerateReports_Click(object sender, EventArgs e)
        {
            if (isRunning) { return; }

            try {
                isRunning = true;
                btn_GenerateReports.BackColor = Color.Yellow;
                rdVM1.Enabled = false;
                rdVM2.Enabled = false;
                lbxResults.Items.Clear();

                recordMng.Clear();
                string remoteBin = cheFindParent.Checked ? getRemoteBin() : "";
                
                DirectoryInfo directoryInfo = new DirectoryInfo(@"roof\results");
                var result = directoryInfo.GetFiles("Result_*.htm", SearchOption.AllDirectories).OrderBy(t => t.LastWriteTime).ToList(); //LastWriteTime would be same with file on VM
                htmFileCount = result.Count;
                lbStat.Text = htmFileCount.ToString();

                for (int i = 0; i < result.Count; i++)
                {
                    Record rec = new Record(result[i].Name, result[i].FullName, remoteBin);
                    if(i<5) MessageBox.Show(rec.parent);
                    if (cheFailedOnly.Checked)
                    {
                        if (!rec.isPass) lbxResults.Items.Add(rec.name);
                    }
                    else
                    { lbxResults.Items.Add(rec.name); }
                    recordMng.addRecord(rec);
                }

                int count = recordMng.getCount();
                if (count <= 0) { throw new Exception("No result record found!"); }
                MessageBox.Show( "1st: "+recordMng.getRecord(0).ToString());
                // write to log using data from recordMng
                if (File.Exists(log)) { File.Delete(log); }
                File.WriteAllText(log, "Parent,Child,Status,StepsExecuted,PassVal,FailVal,Time\n");
                for (int i = 0; i < count; i++)
                {
                    File.AppendAllText(log, recordMng.getRecord(i).ToString() + "\n");
                }
                
                lbInfo.Text = "Done!";
                lbStat.Text = lbStat0 + recordMng.getStat();
            }
            catch(Exception ex) {
                lbInfo.Text = "Error!";
                MessageBox.Show(ex.Message);
            }
            finally
            {
                isRunning = false;
                rdVM1.Enabled = true;
                rdVM2.Enabled = true;
                btn_GenerateReports.BackColor = SystemColors.Control;

            }
            
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
            try
            {
                if (!isRunning)
                {
                    isRunning = true;
                    lbInfo.Text = "Info: download results ...";

                    if (rdVM1.Checked && whichResults == 1)
                    {
                        string path = txtRemoteResults1.Text;
                        if (Directory.Exists(path)) { MyLib.CopyAll(path, localResults); lbInfo.Text = "Info: results downloaded!"; }
                        else throw new Exception("Error: download failed due to VM results path not valid!");
                    }
                    else if (rdVM2.Checked && whichResults == 2)
                    {
                        string path = txtRemoteResults2.Text;
                        if (Directory.Exists(path)) { MyLib.CopyAll(path, localResults); lbInfo.Text = "Info: results downloaded!"; }
                        else throw new Exception("Error: download failed due to VM results path not valid!");
                    }
                    else
                    {
                        throw new Exception("Error: download failed due to VM results path not valid!");
                    }
                    refreshCount();
                    isRunning = false;
                }
            }
            catch(Exception ex)
            {
                lbInfo.Text = "Error!";
                MessageBox.Show(ex.Message);
            }
            finally
            {
                isRunning = false;
            }
            
        }

        private void btnOpenLocal_Click(object sender, EventArgs e)
        {
            Process.Start(localResults);
            refreshCount();
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


        private void LbxResults_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // easy to open htm
            ListBox lb = (ListBox)sender;
            int index = lb.SelectedIndex;
            if(lb.Items.Count >0)
            {
                Process.Start(recordMng.getPathByIndex(lb.SelectedIndex, cheFailedOnly.Checked));
            }
        }


        private void cheFailedOnly_CheckedChanged(object sender, EventArgs e)
        {
            lbxResults.Items.Clear();
            if (cheFailedOnly.Checked)
            {
                lbxResults.ForeColor = Color.Red;
                foreach ( Record rec in recordMng.eRecords)
                {
                    if (!rec.isPass) { lbxResults.Items.Add(rec.name); }
                }
            }
            else
            {
                lbxResults.ForeColor = Color.Black;
                foreach (Record rec in recordMng.records)
                {
                    lbxResults.Items.Add(rec.name);
                }
            }
        }
        public void createFolders()
        {
            System.IO.Directory.CreateDirectory(localResults);
            System.IO.Directory.CreateDirectory(localTmp);
        }

        private void createFile(string pathString)
        {
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

        private void refreshCount()
        {
            resultFolderCount = MyLib.FolderCount(localResults);
            btnOpenLocal.Text = btnOpenLResultsText0 + "(" + resultFolderCount + ")";
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            string remoteBin = @"s:\Documents\rlogger\Roof\RDM\bin";
            string target = "RDM3642_CompoundKeys_RDM798.htm";

            //1. goto bin folder, find target line from bat files
            string[] batPath = Directory.GetFiles(remoteBin, "*.bat");
            string targetLine = "";
            foreach (var b in batPath)
            {
                string[] all = File.ReadAllLines(b);
                foreach( string line in all)
                {
                    string str = MyLib.dumpBatComment(line);
                    if (str.ToLower().Contains(target.ToLower()) && MyLib.isBatFormat(str))
                    {
                        targetLine = str;
                        break;
                    }
                }
            }

            // 2. extra from targetLine: CALL ..\..\Roof.bat "-DseleniumTestCases=%ROOF_LOCAL%\RDM\SetUIVars_RDM.htm;RDM\tests\FVT\Administration\RDM3642_CompoundKeys\RDM3642_CompoundKeys_RDM799.htm"
            targetLine = targetLine.Replace('/', '\\').Replace(".htm\"",".htm").Replace(".htm\'", ".htm").Trim();
            string[] targetLineArray = targetLine.Replace('/', '\\').Split('\\');
            string parent = "";
            for(int i = targetLineArray.Length-1; i > 0; i--) // if i=0 then parent not valid
            {
                if(targetLineArray[i] == target) { parent = targetLineArray[--i]; break; }
            }
            MessageBox.Show(parent);

        }

        
        private string getRemoteBin() {
            string path="";
            if (rdVM1.Checked) path = txtRemoteBin1.Text;
            else path = txtRemoteBin2.Text;

            if (Directory.Exists(path)) return path;
            else throw new Exception("Error: invalid remote bin path");
        }


    }
}
