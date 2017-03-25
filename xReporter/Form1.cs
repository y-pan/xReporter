using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xReporter
{
    /*
     * Author: panyunkui@gmail.com
     * Description: Tool to generate reports, and quickly access files/folders(result, bat, test, ... ).
     * Date: 2017-03-25
     */
    public partial class xReporter : Form
    {
        int tabIndex;
        string featurePath, featureName, binFP, testsFP, dataFP, sqlFP,
            exportFP, csvGoldFP, xmlGoldFP, DownloadsFP, importFP, csvFP, xmlFP, setUIVarsP;
        string lbStatText0 = "Stat: ";
        Color busyBgColor = Color.OrangeRed;
        Color normalBgColor = SystemColors.Control;
        bool isRunning;
        
        string reportVM1,reportVM2;
        string reportVM1x, reportVM2x;
        string localResults1,localResults2;
        int bwShowHideSignal = 0; // 1 : copied, 2 : Found, -2 : not found
        string remoteBin1,remoteBin2;

        int resultFolderCount1, resultFolderCount2;
        int htmFileCount1, htmFileCount2;
        int currentFiltedIndex1, currentFiltedIndex2;

        RecordManager recordMng1, recordMng2;

        public xReporter()
        {
            InitializeComponent();
        }

        private void xReporter_Load(object sender, EventArgs e)
        {
            localResults1 = @".\r1";
            localResults2 = @".\r2";
            createLocalResultFolder(0);
            createLocalResultFolder(1);
            
            tabVMs.SelectedIndex = tabLists.SelectedIndex = 0;
            isRunning = false;
            reportVM1 = "xReportVM1.csv"; // simple
            reportVM2 = "xReportVM2.csv";
            reportVM1x = "xReportVM1x.csv"; // complex
            reportVM2x = "xReportVM2x.csv";
            refreshCount(0);
            //refreshCount(1);
            recordMng1 = new RecordManager();
            recordMng2 = new RecordManager();

            setToolTip();
            clearBtnTag(0,true);
            clearBtnTag(0,false);
            clearBtnTag(1,true);
            clearBtnTag(1,false);
        }
        private void setToolTip() {

            toolTip1.SetToolTip(this.cheFailedOnly, "Check it to view Failed tests only, doesn't affect the report");
            toolTip1.SetToolTip(this.btnOpenLocalResultContainer, "To open local result folder, the number is for folder count, might not be the same with number in stat");
            toolTip1.SetToolTip(this.cheNotepadpp, "Check it, will use notepad++ to open bat file, otherwise use notepad");
            toolTip1.SetToolTip(this.txtJustALabel1, "Any thing you like, I don't care that~");
            toolTip1.SetToolTip(this.txtJustALabel2, "Any thing you like, I don't care that~");
            toolTip1.SetToolTip(this.btnDownloadResults, "Download result from selected VM's result");
            toolTip1.SetToolTip(this.btnOpenRoot, "Open root directory, where csv reports will be");

            toolTip1.SetToolTip(this.btnEmptyLocalResults, "Empty local results, either r1 or r2 base on which tab you are at");
            toolTip1.SetToolTip(this.btnGenerateReports, "Generate csv report from local results(r1, or r2), and Good Luck~");

            toolTip1.SetToolTip(this.btnOpenBat, "Open bat file base on selected result in list");
            toolTip1.SetToolTip(this.btnOpenTest, "Open test file base on selected result in list");
            toolTip1.SetToolTip(this.btnOpenTestFolder, "Open test folder file base on selected result in list");
            toolTip1.SetToolTip(this.btnOpenResult, "Open result file base on selected result in list");
            toolTip1.SetToolTip(this.btnOpenSelectedResultFolder, "Open result folder base on selected result in list");
            toolTip1.SetToolTip(this.cheNotepadpp, @"Check it, will open bat/test file using notepad++, otherwise using window's notepad");

            toolTip1.SetToolTip(this.btnCopyTestFolderPath, "Copy specific test folder path to clipboard base on selected result in list");

            toolTip1.SetToolTip(this.btnOpenRemoteBin, "Open remote bin folder");
            toolTip1.SetToolTip(this.cheExact1, "For filter to match the whole test name only, not case sensitive");
            toolTip1.SetToolTip(this.cheExact2, "For filter to match the whole test name only, not case sensitive");
            toolTip1.SetToolTip(this.btnOpenReport, "Open report file, either " + reportVM1x + " or " + reportVM2x + "\nThe different between " + reportVM1+ " and " +reportVM1x + " is: 1st one has only 1 side, and the 2nd one has more where same tests having same parent will be on the same row" );

            toolTip1.SetToolTip(this.txtFilter1, "Type in the name you want to search, then press ENTER or click Go button, not case sensitive");
            toolTip1.SetToolTip(this.txtFilter2, "Type in the name you want to search, then press ENTER or click Go button, not case sensitive");
            toolTip1.SetToolTip(this.btnFilterGo1, "Search base on filter input, pressing ENTER while focusing on input box does the same thing");
            toolTip1.SetToolTip(this.btnFilterGo2, "Search base on filter input, pressing ENTER while focusing on input box does the same thing");

            toolTip1.SetToolTip(this.btnFilterNext1, "Find next one if applicable, will get enabled after clicking Go button or pressing ENTER on input box");
            toolTip1.SetToolTip(this.btnFilterNext2, "Find next one if applicable, will get enabled after clicking Go button or pressing ENTER on input box");
            toolTip1.SetToolTip(this.btnFilterPrevious2, "Find previous one if applicable, will get enabled after clicking Go button or pressing ENTER on input box if match found");
            toolTip1.SetToolTip(this.btnFilterPrevious1, "Find previous one if applicable, will get enabled after clicking Go button or pressing ENTER on input box if match found");
            toolTip1.SetToolTip(this.btnCopyStat, "Copy statistics to clipboard");
            toolTip1.SetToolTip(this.txtRemoteRoof1, "Path to the VM's Roof folder. (Double-click on input box to open the specific folder)");
            toolTip1.SetToolTip(this.txtRemoteRoof2, "Path to the VM's Roof folder. (Double-click on input box to open the specific folder)");
            toolTip1.SetToolTip(this.comFeature1, "Select the feature under remote Roof folder, like RDM");
            toolTip1.SetToolTip(this.comFeature2, "Select the feature under remote Roof folder, like RDM");
            toolTip1.SetToolTip(this.txtRemoteTests1, "The path where to download results to local, normally the remote result folder, under Roof. However, you can change it to other folder manually");
            toolTip1.SetToolTip(this.txtRemoteTests2, "The path where to download results to local, normally the remote result folder, under Roof. However, you can change it to other folder manually");
            toolTip1.SetToolTip(this.txtRemoteBin1, "Path to remote bin folder under roof, needed for finding parent of tests in generating csv");
            toolTip1.SetToolTip(this.txtRemoteBin2, "Path to remote bin folder under roof, needed for finding parent of tests in generating csv");
            toolTip1.SetToolTip(this.txtRemoteTests1, "Path to remote test folder under roof, needed in generating csv");
            toolTip1.SetToolTip(this.txtRemoteTests2, "Path to remote test folder under roof, needed in generating csv");
            toolTip1.SetToolTip(this.lbxResults1, "List of results, double-click on item to open the specific result htm file in local result folder");
            toolTip1.SetToolTip(this.lbxResults2, "List of results, double-click on item to open the specific result htm file in local result folder");
            toolTip1.Active = false;
        }
        private void btnEmptyLocalResults_Click(object sender, EventArgs e)
        {
            if (isRunning || bw_emptyResults.IsBusy) return;
            string path ="";
            int fcount=0;
            switch(tabIndex)
            {
                case 0: path = localResults1; fcount = resultFolderCount1; break;
                case 1: path = localResults2; fcount = resultFolderCount2; break;
                default: break;
            }

            try
            {
                if (fcount <= 0) throw new Exception("Already emptied.");
                if (MessageBox.Show("Are you sure to empty local results?", "Empty Local Results", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    isRunning = true;
                    btnEmptyLocalResults.BackColor = busyBgColor;
                    btnEmptyLocalResults.ForeColor = Color.Yellow;
                    disableUIEnableProgressBar(isRunning);
                    lbInfo.Text = "Info: start to dump local results";
                    bw_emptyResults.RunWorkerAsync(path);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void btnGenerateReports_Click(object sender, EventArgs e)
        {
            
            if (isRunning || bw_generateReport.IsBusy) { return; }
            refreshCount(tabIndex);
            
            try {

                switch(tabIndex)
                {
                    case 0:
                        if (resultFolderCount1 <= 0) { throw new Exception("No result folder found!"); }
                        if (File.Exists(reportVM1)) { File.Delete(reportVM1); }
                        if (File.Exists(reportVM1x)) { File.Delete(reportVM1x); }
                        remoteBin1 = getRemoteBin(tabIndex);
                        lbxResults1.Items.Clear();
                        break;
                    case 1:
                        if (resultFolderCount2 <= 0) { throw new Exception("No result folder found!"); }
                        if (File.Exists(reportVM2)) { File.Delete(reportVM2); }
                        if (File.Exists(reportVM2x)) { File.Delete(reportVM2x); }
                        remoteBin2 = getRemoteBin(tabIndex);
                        lbxResults2.Items.Clear();
                        break;
                    default: break;
                }
                
                
                isRunning = true;
                disableUIEnableProgressBar(isRunning);
                btnGenerateReports.BackColor = busyBgColor;
                lbInfo.Text = "Info: Generating started.";

                bw_generateReport.RunWorkerAsync();
                
            }
            catch(Exception ex) {
                lbInfo.Text = "Error!";
                MessageBox.Show(ex.Message);
            }
            
        }
 

        // ---------------------------------- EVENTS --------------------------

        private void txtRemoteRoof_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            
            string roofPath = string.Format(""+tb.Text);
            clearBtnTag(tabIndex, false);

            if (!Directory.Exists(roofPath))
            {
                switch (tabIndex)
                {
                    case 0:
                        comFeature1.Items.Clear();
                        comFeature1.Text = "";
                        txtRemoteResults1.Text = "";
                        break;
                    case 1:
                        comFeature2.Items.Clear();
                        comFeature2.Text = "";
                        txtRemoteResults2.Text = "";
                        break;
                    default: break;
                }
                return;
            }

            string[] subfolders = Directory.GetDirectories(roofPath); // one level down
            string resultsFP = roofPath + "\\results";
            string roofProFP = roofPath + "\\properties";
            string roofProP = roofProFP + "\\RoofTest.properties";

            if (!Directory.Exists(resultsFP)) resultsFP = "";
            if (!Directory.Exists(roofProFP)) roofProFP = "";
            if (!File.Exists(roofProP)) roofProP = "";


            switch (tabIndex)
            {
                case 0:
                    comFeature1.Items.Clear();
                    txtRemoteResults1.Text = resultsFP;
                    btnRoofPropertiesFolder1.Tag = roofProFP;
                    btnRoofProperties1.Tag = roofProP;
                    if (resultsFP != "") txtRemoteResults1.ForeColor = Color.Black;
                    if (roofProP != "") btnRoofProperties1.ForeColor = Color.Black;
                    if (roofProFP != "") btnRoofPropertiesFolder1.ForeColor = Color.Black;
                    foreach (string p in subfolders)
                    {
                        comFeature1.Items.Add(p);
                    }
                    
                    if (comFeature1.Items.Count > 0) comFeature1.SelectedIndex = 0;
                    else comFeature1.Text = "";
                    
                    break;
                case 1:
                    comFeature2.Items.Clear();
                    txtRemoteResults2.Text = resultsFP;
                    btnRoofPropertiesFolder2.Tag = roofProFP;
                    btnRoofProperties2.Tag = roofProP;
                    if (resultsFP != "") txtRemoteResults2.ForeColor = Color.Black;
                    if (roofProP != "") btnRoofProperties2.ForeColor = Color.Black;
                    if (roofProFP != "") btnRoofPropertiesFolder2.ForeColor = Color.Black;
                    foreach (string p in subfolders)
                    {                       
                        comFeature2.Items.Add(p);

                    }
                    if (comFeature2.Items.Count > 0) comFeature2.SelectedIndex = 0;
                    else comFeature2.Text = "";                    
                    break;
                default: break;
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
            if (isRunning || bw_mapFeature.IsBusy) return;

            ComboBox cb = (ComboBox)sender;

            featurePath = string.Format("" + cb.SelectedItem);  // start
            clearBtnTag(tabIndex, true);  // tabVMs eventListener will set tabIndex

            if (!Directory.Exists(featurePath))
            {
                switch (tabIndex)
                {
                    case 1: txtRemoteBin1.Text = ""; txtRemoteTests1.Text = ""; break;
                    case 2: txtRemoteBin2.Text = ""; txtRemoteTests2.Text = ""; break;
                }
                return;
            }

            isRunning = true;
            //disableUIEnableProgressBar(isRunning);
            bw_mapFeature.RunWorkerAsync();

        }

        private void clearBtnTag(int num, bool isFeature = true) 
        {
            switch(num){
                case 0:
                    if (isFeature)
                    {
                        btnData1.Tag = "";
                        btnSql1.Tag = "";
                        btnExport1.Tag = "";
                        btnImport1.Tag = "";
                        btnCsvGold1.Tag = "";
                        btnXmlGold1.Tag = "";
                        btnDownloads1.Tag = "";
                        btnCsv1.Tag = "";
                        btnXml1.Tag = "";
                        btnData1.ForeColor = Color.Gray;
                        btnSql1.ForeColor = Color.Gray;
                        btnExport1.ForeColor = Color.Gray;
                        btnImport1.ForeColor = Color.Gray;
                        btnCsvGold1.ForeColor = Color.Gray;
                        btnXmlGold1.ForeColor = Color.Gray;
                        btnDownloads1.ForeColor = Color.Gray;
                        btnCsv1.ForeColor = Color.Gray;
                        btnXml1.ForeColor = Color.Gray;
                        btnSetUIVars1.Tag = "";
                        btnSetUIVarsFolder1.Tag = "";
                        btnSetUIVars1.ForeColor = Color.Gray;
                    }
                    else
                    {
                        btnRoofProperties1.Tag = "";
                        btnRoofPropertiesFolder1.Tag = "";
                        btnRoofProperties1.ForeColor = Color.Gray;
                    }

                    break;
                case 1:
                    if (isFeature)
                    {
                        btnData2.Tag = "";
                        btnSql2.Tag = "";
                        btnExport2.Tag = "";
                        btnImport2.Tag = "";
                        btnCsvGold2.Tag = "";
                        btnXmlGold2.Tag = "";
                        btnDownloads2.Tag = "";
                        btnCsv2.Tag = "";
                        btnXml2.Tag = "";
                        btnData2.ForeColor = Color.Gray;
                        btnSql2.ForeColor = Color.Gray;
                        btnExport2.ForeColor = Color.Gray;
                        btnImport2.ForeColor = Color.Gray;
                        btnCsvGold2.ForeColor = Color.Gray;
                        btnXmlGold2.ForeColor = Color.Gray;
                        btnDownloads2.ForeColor = Color.Gray;
                        btnCsv2.ForeColor = Color.Gray;
                        btnXml2.ForeColor = Color.Gray;

                        btnSetUIVars2.Tag = "";
                        btnSetUIVarsFolder2.Tag = "";
                        btnSetUIVars2.ForeColor = Color.Gray;
                    }
                    else
                    {
                        btnRoofProperties2.Tag = "";
                        btnRoofPropertiesFolder2.Tag = "";
                        btnRoofProperties2.ForeColor = Color.Gray;
                    }
                    break;
                default:break;
            }
            
        }
        private void btnDownloadResults_Click(object sender, EventArgs e)
        {
            if (isRunning || bw_download.IsBusy) { return; }
            
            try
            {
                string rpath="", lpath="", fromFolderName="", toFolderName="";
                switch(tabIndex)
                {
                    case 0:
                        rpath = txtRemoteResults1.Text;
                        lpath = localResults1;                        
                        break;
                    case 1:
                        rpath = txtRemoteResults2.Text;
                        lpath = localResults2;
                        break;
                    default: break;
                }

                if (fromFolderName != "" && !Directory.Exists(rpath + "\\" + fromFolderName)) throw new Exception("Invalid input for \"Result From\": " + fromFolderName);
                if (toFolderName != "" && !Directory.Exists(rpath + "\\" + toFolderName)) throw new Exception("Invalid input for \"Result To\": "+ toFolderName);
                                

                if (!Directory.Exists(lpath)) { createLocalResultFolder(tabIndex); Thread.Sleep(100); }
                if (Directory.Exists(rpath))
                {
                    isRunning = true;
                    disableUIEnableProgressBar(isRunning);
                    lbInfo.Text = "Info: Download started.";
                    btnDownloadResults.BackColor = busyBgColor;
                    string[] args = new string[] { rpath, lpath };

                    bw_download.RunWorkerAsync(args);

                }
                else throw new Exception("Error: download failed due to VM results path not valid!");
               
                //refreshCount(); 
             
            }
            catch(Exception ex)
            {
                //lbInfo.Text = "Error!";
                MessageBox.Show(ex.Message);
            }
            
        }


        private void btnOpenLocal_Click(object sender, EventArgs e)
        {
            try
            {
                string path = "";
                switch(tabIndex)
                {
                    case 0:
                        path = localResults1;
                        break;
                    case 1:
                        path = localResults2;
                        break;
                    default: break;
                }
            
                if (path.Length <= 0) throw new Exception("Error: empty path");
                if (!Directory.Exists(path)) { createLocalResultFolder(tabIndex); Thread.Sleep(100); }
                Process.Start(path);
                refreshCount(tabIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void LbxResults_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            if (isRunning) return;
           
            try
            {
                int index=0;
                 switch(tabIndex)
                {
                     case 0:
                        if (lbxResults1.Items.Count <= 0) throw new Exception("No result available!");
                        index = lbxResults1.SelectedIndex;
                        if (index < 0) throw new Exception("You need to select a result from list first!");
                        Process.Start(recordMng1.getPathByIndex(index, cheFailedOnly.Checked));
                        break;
                     case 1:
                        if (lbxResults2.Items.Count <= 0) throw new Exception("No result available!");
                        index = lbxResults2.SelectedIndex;
                        if (index < 0) throw new Exception("You need to select a result from list first!");
                        Process.Start(recordMng2.getPathByIndex(index, cheFailedOnly.Checked));
                        break;
                     default: break;

                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        private void cheFailedOnly_CheckedChanged(object sender, EventArgs e)
        {
            int i = 0;

            lbxResults1.Items.Clear();
            lbxResults2.Items.Clear();
            
            if (cheFailedOnly.Checked)
            {
                lbxResults1.ForeColor = Color.Red;
                lbxResults2.ForeColor = Color.Red;
                if (recordMng1.getECount() > 0)
                {
                    i = 0;
                    foreach (Record rec in recordMng1.eRecords)
                    {
                        if (!rec.isPass) { lbxResults1.Items.Add((++i) + " | " + rec.name); }
                    }
                }
                if (recordMng2.getECount() > 0)
                {
                    i = 0;
                    foreach (Record rec in recordMng2.eRecords)
                    {
                        if (!rec.isPass) { lbxResults2.Items.Add((++i) + " | " + rec.name); }
                    }
                }
                
            }
            else
            {
                lbxResults1.ForeColor = Color.Black;
                lbxResults2.ForeColor = Color.Black;
                if (recordMng1.getCount() > 0)
                {
                    i = 0;
                    foreach (Record rec in recordMng1.records)
                    {
                        lbxResults1.Items.Add((++i) + " | " + rec.name);
                    }
                }
                if (recordMng2.getCount() > 0)
                {
                    i = 0;
                    foreach (Record rec in recordMng2.records)
                    {
                        lbxResults2.Items.Add((++i) + " | " + rec.name);
                    }
                }
                
            }

            
        }
        public void createLocalResultFolder(int tabIndex)
        {
            switch(tabIndex)
            {
                case 0: System.IO.Directory.CreateDirectory(localResults1); break;
                case 1: System.IO.Directory.CreateDirectory(localResults2); break;
                default: break;
            }
            
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

        
        private void refreshCount(int tabId)
        {
            switch (tabId)
            {
                case 0: 
                    resultFolderCount1 = MyLib.FolderCount(localResults1);
                    btnOpenLocalResultContainer.Text = resultFolderCount1.ToString();
                    break;
                case 1: 
                    resultFolderCount2 = MyLib.FolderCount(localResults2); 
                    btnOpenLocalResultContainer.Text = resultFolderCount2.ToString();
                    break;
            }
        }

        
        private string getRemoteBin(int tabId) {

            string path = "";
            switch(tabId)
            {
                case 0:
                    path = txtRemoteBin1.Text;
                    break;
                case 1:
                    path = txtRemoteBin2.Text;
                    break;
                default: break;
            }

            if (path.Length >0 && Directory.Exists(path)) return path;
            else throw new Exception("Error: invalid remote bin path");
        }

        private void cheEnableTooltip_CheckedChanged(object sender, EventArgs e)
        {
            if (cheEnableTooltip.Checked)
            {
                toolTip1.Active = true;
                cheEnableTooltip.ForeColor = Color.FromArgb(0, 0, 0);
            }
            else
            {
                toolTip1.Active = false;
                cheEnableTooltip.ForeColor = Color.FromArgb(110, 110, 110);
            }
        }



        private void btnOpenSelectedResultFolder_Click(object sender, EventArgs e)
        {
            if (isRunning) return;
            
            try
            {
                int index;
                string rpath;
                switch(tabIndex)
                {
                    case 0:
                        if (lbxResults1.Items.Count <= 0) throw new Exception("Cannot process due to no result available!");
                        index = lbxResults1.SelectedIndex;
                        if (index < 0) throw new Exception("You need to select a result from list first!");
                        rpath = recordMng1.getFolderPathByIndex(lbxResults1.SelectedIndex, cheFailedOnly.Checked);
                        if (!Directory.Exists(rpath)) throw new Exception("Invalid result folder path !");
                        if (lbxResults1.Items.Count > 0)
                        {
                            Process.Start("explorer.exe", rpath);
                        }
                        break;
                    case 1:
                        if (lbxResults2.Items.Count <= 0) throw new Exception("Cannot process due to no result available!");
                        index = lbxResults2.SelectedIndex;
                        if (index < 0) throw new Exception("You need to select a result from list first!");
                        rpath = recordMng2.getFolderPathByIndex(lbxResults2.SelectedIndex, cheFailedOnly.Checked);
                        if (!Directory.Exists(rpath)) throw new Exception("Invalid result folder path !");
                        if (lbxResults2.Items.Count > 0)
                        {
                            Process.Start("explorer.exe", rpath);
                        }
                        break;
                    default: break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnOpenTest_Click(object sender, EventArgs e)
        {
            if (isRunning) return;
            int index;
            bool isFaileOnly = cheFailedOnly.Checked ? true : false;
            Record rec;
            string roof="";
            string testPart="";
            try
            {
                switch(tabIndex)
                {
                    case 0:
                        if (lbxResults1.Items.Count <= 0) throw new Exception("Cannot process due to no result available!");
                        index = lbxResults1.SelectedIndex;
                        if (index < 0) throw new Exception("You need to select a result to open a bat file");
                        rec = isFaileOnly ? recordMng1.getERecord(index) : recordMng1.getRecord(index);
                        roof = txtRemoteRoof1.Text;
                        testPart = rec.getTestPath();
                        break;
                    case 1:
                        if (lbxResults2.Items.Count <= 0) throw new Exception("Cannot process due to no result available!");
                        index = lbxResults2.SelectedIndex;
                        if (index < 0) throw new Exception("You need to select a result to open a bat file");
                        rec = isFaileOnly ? recordMng2.getERecord(index) : recordMng2.getRecord(index);
                        roof = txtRemoteRoof2.Text;
                        testPart = rec.getTestPath();
                        break;
                    default: break;
                }
                if (!testPart.StartsWith("\\")) testPart = "\\" + testPart;
                string fullTestPath = roof + testPart;
                if (!File.Exists(fullTestPath)) { throw new Exception("Invalid test path: " + fullTestPath); }
                if (cheNotepadpp.Checked) { Process.Start("notepad++.exe", fullTestPath); }
                else { Process.Start("notepad.exe", fullTestPath); }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }

        private void btnOpenBat_Click(object sender, EventArgs e)
        {
            if (isRunning) return;
            int index;
            bool isFaileOnly = cheFailedOnly.Checked ? true : false;
            Record rec;
            string batPath="";
            try
            {
                switch(tabIndex)
                {
                    case 0:
                        if (lbxResults1.Items.Count <= 0) throw new Exception("Cannot process due to no result available!");
                        index = lbxResults1.SelectedIndex;
                        if (index < 0) throw new Exception("You need to select a result to open a bat file");
                        rec = isFaileOnly ? recordMng1.getERecord(index) : recordMng1.getRecord(index);
                        batPath = rec.batPath;
                        break;
                    case 1:
                        if (lbxResults2.Items.Count <= 0) throw new Exception("Cannot process due to no result available!");
                        index = lbxResults2.SelectedIndex;
                        if (index < 0) throw new Exception("You need to select a result to open a bat file");
                        rec = isFaileOnly ? recordMng2.getERecord(index) : recordMng2.getRecord(index);
                        batPath = rec.batPath;
                        break;
                    default: break;
                }
                if (!File.Exists(batPath)) { throw new Exception("Invalid bat path: " + batPath); }
                if (cheNotepadpp.Checked) { Process.Start("notepad++.exe", batPath); }
                else { Process.Start("notepad.exe", batPath); }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
            

        private void btnOpenTestFolder_Click(object sender, EventArgs e)
        {
            if (isRunning) return;
            int index = 0;
            bool isFaileOnly = cheFailedOnly.Checked ? true : false;
            Record rec;
            string roof = "";
            string folderPath = "";
            try
            {
                switch (tabIndex)
                {
                    case 0:
                        if (lbxResults1.Items.Count <= 0) throw new Exception("Cannot process due to no result available!");
                        index = lbxResults1.SelectedIndex;
                        if (index < 0) throw new Exception("You need to select a result from list first!");
                        rec = isFaileOnly ? recordMng1.getERecord(index) : recordMng1.getRecord(index);
                        roof = txtRemoteRoof1.Text;
                        folderPath = rec.getTestFolderPath();
                        break;
                    case 1:
                        if (lbxResults2.Items.Count <= 0) throw new Exception("Cannot process due to no result available!");
                        index = lbxResults2.SelectedIndex;
                        if (index < 0) throw new Exception("You need to select a result from list first!");
                        rec = isFaileOnly ? recordMng2.getERecord(index) : recordMng2.getRecord(index);
                        roof = txtRemoteRoof2.Text;
                        folderPath = rec.getTestFolderPath();
                        break;
                    default: break;
                }

                if (folderPath.Length <= 0) { throw new Exception("Invalid test path: path is empty"); }
                if (!folderPath.StartsWith("\\")) folderPath = "\\" + folderPath;
                string fullPath = roof + folderPath;
                Process.Start("explorer.exe", fullPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnOpenResult_Click(object sender, EventArgs e)
        {
            if (isRunning) return;
            int index = 0;
            string path = "";
            try
            {
                switch(tabIndex)
                {
                    case 0:
                        if (lbxResults1.Items.Count <= 0) throw new Exception("No result available!");
                        index = lbxResults1.SelectedIndex;
                        if (index < 0) throw new Exception("You need to select a result from list first!");
                        path = recordMng1.getPathByIndex(index, cheFailedOnly.Checked);
                        break;
                    case 1:
                        if (lbxResults2.Items.Count <= 0) throw new Exception("No result available!");
                        index = lbxResults2.SelectedIndex;
                        if (index < 0) throw new Exception("You need to select a result from list first!");
                        path = recordMng2.getPathByIndex(index, cheFailedOnly.Checked);
                        break;
                    default: break;
                }

                if (path.Length <= 0 || !File.Exists(path)) throw new Exception("Invalid path: " + path);
                Process.Start(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnOpenRoot_Click(object sender, EventArgs e)
        {
            Process.Start(".");
        }

        private void bw_download_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            //isRunning = true;
            string[] args = (string[])e.Argument;
            MyLib.CopyAll(args[0], args[1]);
        }
        private void bw_emptyResults_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            string path = (string)e.Argument;
            MyLib.RemoveFolder(path);
            System.Threading.Thread.Sleep(100);
            MyLib.CreateFolder(path);
        }
        private void bw_generateReport_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            string path = "";
            DirectoryInfo directoryInfo;
            int count;
            List<string> outRecStrings = new List<string>();
            List<string> outNames = new List<string>();
            string oldParent = "";
            bool isIncludeParent = false; ;
            switch(tabIndex)
            {
                case 0:
                    recordMng1.Clear();
                    path = localResults1;
                    directoryInfo = new DirectoryInfo(path);
                    var result = directoryInfo.GetFiles("Result_*.htm", SearchOption.AllDirectories).Where(file => !file.Name.StartsWith("Result_SetUIVars_")).OrderBy(t => t.LastWriteTime).ToList();
                    htmFileCount1 = result.Count;
                    if (htmFileCount1 <= 0) { throw new Exception("No result record found!"); }
                    for (int i = 0; i < result.Count; i++)
                    {
                        Record rec = new Record(result[i].DirectoryName, result[i].Name, result[i].FullName, remoteBin1, i);
                        recordMng1.addRecord(rec);
                    }
                    count = recordMng1.getCount();
                    if (count <= 0) { throw new Exception("No result record found!"); }

                    // write to simple csv(reportVM1), and prepare outRecStrings list for complex csv (reportVM1x)
                    File.WriteAllText(reportVM1, "Parent,Child,Status,StepsExecuted,PassVal,FailVal,Time\n");

                    for (int j = 0; j < count; j++)
                    {
                        Record rec = recordMng1.getRecord(j);
                        // for simple csv
                        isIncludeParent = false;
                        if (oldParent != rec.parent)
                        {
                            oldParent = rec.parent;
                            isIncludeParent = true;
                        }
                        File.AppendAllText(reportVM1, rec.ToString(isIncludeParent) + "\n");

                        // for complex csv
                        if (outNames.Contains(rec.name))
                        {
                            int index = outNames.IndexOf(rec.name);
                            outRecStrings[index] += ",,||," + rec.ToString(isIncludeParent);
                        }
                        else
                        {
                            outRecStrings.Add(rec.ToString(isIncludeParent));
                            outNames.Add(rec.name);
                        }
                    }
                    // write to complexLog
                    if (outRecStrings.Count > 0)
                    {
                        File.WriteAllText(reportVM1x, "Parent,Child,Status,StepsExecuted,PassVal,FailVal,Time,,||,Parent,Child,Status,StepsExecuted,PassVal,FailVal,Time\n");
                        foreach (string s in outRecStrings)
                        {
                            File.AppendAllText(reportVM1x, s + "\n");
                        }
                    }

                    break;
                case 1:
                    recordMng2.Clear();
                    path = localResults2;
                    directoryInfo = new DirectoryInfo(path);
                    var result2 = directoryInfo.GetFiles("Result_*.htm", SearchOption.AllDirectories).Where(file => !file.Name.StartsWith("Result_SetUIVars_")).OrderBy(t => t.LastWriteTime).ToList();
                    htmFileCount2 = result2.Count;
                    if (htmFileCount2 <= 0) { throw new Exception("No result record found!"); }
                    for (int i = 0; i < result2.Count; i++)
                    {
                        Record rec = new Record(result2[i].DirectoryName, result2[i].Name, result2[i].FullName, remoteBin2, i);
                        recordMng2.addRecord(rec);
                    }
                    count = recordMng2.getCount();
                    if (count <= 0) { throw new Exception("No result record found!"); }

                    // write to simple csv(reportVM2), and prepare outRecStrings list for complex csv (reportVM2x)
                    File.WriteAllText(reportVM1, "Parent,Child,Status,StepsExecuted,PassVal,FailVal,Time\n");

                    for (int j = 0; j < count; j++)
                    {
                        Record rec = recordMng2.getRecord(j);
                        // for simple csv
                        isIncludeParent = false;
                        if (oldParent != rec.parent)
                        {
                            oldParent = rec.parent;
                            isIncludeParent = true;
                        }
                        File.AppendAllText(reportVM2, rec.ToString(isIncludeParent) + "\n");

                        // for complex csv
                        if (outNames.Contains(rec.name))
                        {
                            int index = outNames.IndexOf(rec.name);
                            outRecStrings[index] += ",,||," + rec.ToString(isIncludeParent);
                        }
                        else
                        {
                            outRecStrings.Add(rec.ToString(isIncludeParent));
                            outNames.Add(rec.name);
                        }
                    }
                    // write to complexLog
                    if (outRecStrings.Count > 0)
                    {
                        File.WriteAllText(reportVM2x, "Parent,Child,Status,StepsExecuted,PassVal,FailVal,Time,,||,Parent,Child,Status,StepsExecuted,PassVal,FailVal,Time\n");
                        foreach (string s in outRecStrings)
                        {
                            File.AppendAllText(reportVM2x, s + "\n");
                        }
                    }
                    break;
                default: break;
            }
           
        }
        private void bw_mapFeature_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            featureName = MyLib.getLastPartInPath(featurePath);
            binFP = featurePath + @"\bin";
            testsFP = featurePath + @"\tests";
            dataFP = featurePath + @"\data";
            sqlFP = featurePath + @"\sql";
            exportFP = dataFP + @"\Export";
            importFP = dataFP + @"\Import";
            csvGoldFP = exportFP + @"\csvGold";
            xmlGoldFP = exportFP + @"\xmlGold";
            DownloadsFP = exportFP + @"\Downloads";
            csvFP = importFP + @"\csv";
            xmlFP = importFP + @"\xml";
            setUIVarsP = featurePath + "\\SetUIVars_" + featureName + ".htm";
            
            if (!Directory.Exists(binFP)) binFP = "";
            if (!Directory.Exists(testsFP)) testsFP = "";
            if (!Directory.Exists(dataFP)) dataFP = "";
            if (!Directory.Exists(sqlFP)) sqlFP = "";
            if (!Directory.Exists(exportFP)) exportFP = "";
            if (!Directory.Exists(importFP)) importFP = "";
            if (!Directory.Exists(csvGoldFP)) csvGoldFP = "";
            if (!Directory.Exists(xmlGoldFP)) xmlGoldFP = "";
            if (!Directory.Exists(DownloadsFP)) DownloadsFP = "";
            if (!Directory.Exists(csvFP)) csvFP = "";
            if (!Directory.Exists(xmlFP)) xmlFP = "";
            if (!File.Exists(setUIVarsP)) setUIVarsP = "";

        }

        private void bw_mapFeature_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            // add data to UI
            switch(tabIndex)
            {
                case 0:
                    txtRemoteBin1.Text = binFP;
                    txtRemoteTests1.Text = testsFP;
                    btnData1.Tag = dataFP;
                    btnSql1.Tag = sqlFP;
                    btnExport1.Tag = exportFP;
                    btnImport1.Tag = importFP;
                    btnCsvGold1.Tag = csvGoldFP;
                    btnXmlGold1.Tag = xmlGoldFP;
                    btnDownloads1.Tag = DownloadsFP;
                    btnCsv1.Tag = csvFP;
                    btnXml1.Tag = xmlFP;
                    btnSetUIVars1.Tag = setUIVarsP;
                    btnSetUIVarsFolder1.Tag = featurePath;

                    if (dataFP != "") btnData1.ForeColor = Color.Black;
                    if (sqlFP != "") btnSql1.ForeColor = Color.Black;
                    if (exportFP != "") btnExport1.ForeColor = Color.Black;
                    if (importFP != "") btnImport1.ForeColor = Color.Black;
                    if (csvGoldFP != "") btnCsvGold1.ForeColor = Color.Black;
                    if (xmlGoldFP != "") btnXmlGold1.ForeColor = Color.Black;
                    if (DownloadsFP != "") btnDownloads1.ForeColor = Color.Black;
                    if (csvFP != "") btnCsv1.ForeColor = Color.Black;
                    if (xmlFP != "") btnXml1.ForeColor = Color.Black;
                    if (setUIVarsP != "") btnSetUIVars1.ForeColor = Color.Black;
                    
                    break;
                case 1:
                    txtRemoteBin2.Text = binFP;
                    txtRemoteTests2.Text = testsFP;
                    btnData2.Tag = dataFP;
                    btnSql2.Tag = sqlFP;
                    btnExport2.Tag = exportFP;
                    btnImport2.Tag = importFP;
                    btnCsvGold2.Tag = csvGoldFP;
                    btnXmlGold2.Tag = xmlGoldFP;
                    btnDownloads2.Tag = DownloadsFP;
                    btnCsv2.Tag = csvFP;
                    btnXml2.Tag = xmlFP;
                    btnSetUIVars2.Tag = setUIVarsP;
                    btnSetUIVarsFolder2.Tag = featurePath;

                    if (dataFP != "") btnData2.ForeColor = Color.Black;
                    if (sqlFP != "") btnSql2.ForeColor = Color.Black;
                    if (exportFP != "") btnExport2.ForeColor = Color.Black;
                    if (importFP != "") btnImport2.ForeColor = Color.Black;
                    if (csvGoldFP != "") btnCsvGold2.ForeColor = Color.Black;
                    if (xmlGoldFP != "") btnXmlGold2.ForeColor = Color.Black;
                    if (DownloadsFP != "") btnDownloads2.ForeColor = Color.Black;
                    if (csvFP != "") btnCsv2.ForeColor = Color.Black;
                    if (xmlFP != "") btnXml2.ForeColor = Color.Black;
                    if (setUIVarsP != "") btnSetUIVars2.ForeColor = Color.Black;
                    break;
                default: break;
            }

            isRunning = false;
            //disableUIEnableProgressBar(isRunning);
        }

        private void bw_generateReport_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            int i = 0;
            switch(tabIndex)
            {
                case 0:
                    // add data to UI
                    if (cheFailedOnly.Checked)
                    {
                        lbxResults1.ForeColor = Color.Red;
                      
                        foreach (Record rec in recordMng1.eRecords)
                        {
                            if (!rec.isPass) { lbxResults1.Items.Add((++i) + " | " + rec.name); }
                        }
                    }
                    else
                    {
                        lbxResults1.ForeColor = Color.Black;
               
                        foreach (Record rec in recordMng1.records)
                        {
                            lbxResults1.Items.Add((++i) + " | " + rec.name);
                        }
                    }
                    break;

                case 1:
                    if (cheFailedOnly.Checked)
                    {
                        lbxResults2.ForeColor = Color.Red;
                        foreach (Record rec in recordMng2.eRecords) { if (!rec.isPass) { lbxResults2.Items.Add((++i) + " | " + rec.name); } }
                    }
                    else
                    {
                        lbxResults2.ForeColor = Color.Black;
                        foreach (Record rec in recordMng2.records) { lbxResults2.Items.Add((++i) + " | " + rec.name); }
                    }
                    break;
                default: break;
            }
            
            int type;
            if (e.Error != null) { type = 0; }
            else if (e.Cancelled) { type = 1; }
            else { type = 2; }
            UIShowGenerateEnd(type);

            isRunning = false;
            disableUIEnableProgressBar(isRunning);
        }
        private void bw_download_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            int type;
            if (e.Error != null) { type = 0; MessageBox.Show(e.Error.Message); }
            else if (e.Cancelled) { type = 1; }
            else { type=2; }
            UIShowDownloadEnd(type);

            isRunning = false;
            disableUIEnableProgressBar(isRunning);
        }

        private void bw_emptyResults_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            int type;
            if (e.Error != null) { type = 0; }
            else if (e.Cancelled) { type = 1; }
            else { type = 2; }
            UIShowEmptyResultsEnd(type);

            isRunning = false;
            disableUIEnableProgressBar(isRunning);
        }

        private void UIShowEmptyResultsEnd(int type) 
        {
            //progressBar1.Visible = false;
            //progressBar1.Enabled = false;
            switch (type)
            {
                case 0: lbInfo.Text = "Info: Error in dumping local results."; break;
                case 1: lbInfo.Text = "Info: Cancelled removing local results."; break;
                case 2: lbInfo.Text = "Info: Local results removed."; break;
                default: break;
            }
            refreshCount(tabIndex);
            btnEmptyLocalResults.BackColor = normalBgColor;
            btnEmptyLocalResults.ForeColor = Color.Red;
        }

        private void UIShowDownloadEnd(int type)
        {
            //progressBar1.Visible = false;
            //progressBar1.Enabled = false;
            switch (type)
            {
                case 0: lbInfo.Text = "Info: Error in download."; break;
                case 1: lbInfo.Text = "Info: Download cancelled."; break;
                case 2: lbInfo.Text = "Info: Download completed!"; break;
                default: break;
            }
            refreshCount(tabIndex);
            btnDownloadResults.BackColor = normalBgColor;
        }

        private void UIShowGenerateEnd(int type)
        {
            switch(tabIndex)
            {
                case 0:
                    lbStat.Text = lbStatText0 + recordMng1.getStat();
                    break;
                case 1:
                    lbStat.Text = lbStatText0 + recordMng2.getStat();
                    break;
                default: break;
            }

            switch (type)
            {
                case 0: lbInfo.Text = "Info: Error in generating report."; break;
                case 1: lbInfo.Text = "Info: Report cancelled."; break;
                case 2: lbInfo.Text = "Info: Report completed!"; break;
                default: break;
            }
            btnGenerateReports.BackColor = normalBgColor;
        }

        

        private void disableUIEnableProgressBar(bool isRunning)
        {
            cheEnableTooltip.Enabled = !isRunning;
            
            tabVMs.Enabled = !isRunning;
            tabLists.Enabled = !isRunning;
            cheFailedOnly.Enabled = !isRunning;

            progressBar.Enabled = isRunning;
            progressBar.Visible = isRunning;

            btnOpenLocalResultContainer.Enabled = !isRunning;
            btnOpenBat.Enabled = !isRunning;
            btnOpenTest.Enabled = !isRunning;
            btnOpenTestFolder.Enabled = !isRunning;
            btnOpenResult.Enabled = !isRunning;
            btnOpenSelectedResultFolder.Enabled = !isRunning;
            btnOpenRemoteBin.Enabled = !isRunning;
           
        }

        private void btnCopyTestFolderPath_Click(object sender, EventArgs e)
        {
            
            if (isRunning) return;
            int index = 0;
            bool isFaileOnly = cheFailedOnly.Checked ? true : false;
            string roof = "";
            string folderPath = "";
            string fullPath = "";
            Record rec;
            try
            {
                switch(tabIndex)
                {
                    case 0:
                        if (lbxResults1.Items.Count <= 0) throw new Exception("Cannot process due to no result available!");
                        index = lbxResults1.SelectedIndex;
                        if (index < 0) throw new Exception("You need to select a result from list first!");
                        rec = isFaileOnly ? recordMng1.getERecord(index) : recordMng1.getRecord(index);
                        roof = txtRemoteRoof1.Text;
                        folderPath = rec.getTestFolderPath();
                        break;
                    case 1:
                        if (lbxResults2.Items.Count <= 0) throw new Exception("Cannot process due to no result available!");
                        index = lbxResults2.SelectedIndex;
                        if (index < 0) throw new Exception("You need to select a result from list first!");
                        rec = isFaileOnly ? recordMng2.getERecord(index) : recordMng2.getRecord(index);
                        roof = txtRemoteRoof2.Text;
                        folderPath = rec.getTestFolderPath();
                        break;
                    default: break;
                }
                
                if (folderPath.Length <= 0) { throw new Exception("Invalid test path: path is empty"); }

                if (!folderPath.StartsWith("\\")) folderPath = "\\" + folderPath;
                fullPath = roof + folderPath;

                System.Windows.Forms.Clipboard.SetText(fullPath);
                lbCopied.Visible = true;
                bwShowHideSignal = 1;
                bw_showHide.RunWorkerAsync();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bw_showHide_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            isRunning = true;
            Thread.Sleep(400);
        }

        private void bw_showHide_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            switch(bwShowHideSignal)
            {
                case 1: lbCopied.Visible = false; break;
                case 2: 
                    if (tabIndex == 0) lbFilterFound1.Visible = false;
                    else if (tabIndex == 1) lbFilterFound2.Visible = false;
                    break;

                case -2:
                    if (tabIndex == 0) lbFilterNotFound1.Visible = false;
                    else if (tabIndex == 1) lbFilterNotFound2.Visible = false;
                    break;
                default: break;
            }
            isRunning = false;
            
        }

        private void btnOpenFolderByTag_Click(object sender, EventArgs e)
        {
            
            Button but = (Button)sender;
            if(but.Tag == null ){ but.ForeColor = Color.Red; return;}
            string tag = but.Tag.ToString();
            if (Directory.Exists(tag)) Process.Start(tag);
        }

        private void btnOpenFileByTag_Click(object sender, EventArgs e)
        {
            Button but = (Button)sender;
            if (but.Tag == null) { but.ForeColor = Color.Red; return; }
            string tag = but.Tag.ToString();
            if (File.Exists(tag)) 
            {
                if (cheNotepadpp.Checked) { Process.Start("notepad++.exe", tag); }
                else { Process.Start("notepad.exe", tag); }
            }
        }


        private void btnOpenRemoteBin_Click(object sender, EventArgs e)
        {
            try
            {
                string path = tabVMs.SelectedIndex == 0 ? txtRemoteBin1.Text : txtRemoteBin2.Text;
                if (path.Length <= 0) throw new Exception("Empty path");
                if (!Directory.Exists(path)) throw new Exception("Invalid path: " + path);

                Process.Start(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

        }

        private void tabVMs_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl tb = (TabControl)sender;
            tabIndex = tb.SelectedIndex;
            tabVMs.SelectedIndex = tabIndex;
            tabLists.SelectedIndex = tabIndex;
            
            refreshCount(tabIndex);
            switch (tabIndex)
            {
                case 0:
                    lbStat.Text = lbStatText0 + recordMng1.getStat();
                    
                    break;
                case 1:
                    lbStat.Text = lbStatText0 + recordMng2.getStat();
                    break;
                default: break;
            }
            
        }

        private void btnOpenReport_Click(object sender, EventArgs e)
        {
            if(isRunning) return;
            try
            {
                switch (tabIndex)
                {
                    case 0:
                        if (!File.Exists(reportVM1x)) throw new Exception("Error: " + reportVM1x + " not found!");
                        System.Diagnostics.Process.Start(reportVM1x);
                        //Process.Start("notepad++.exe", reportVM1x);
                        break;
                    case 1:
                        if (!File.Exists(reportVM2x)) throw new Exception("Error: " + reportVM1x + " not found!");
                        System.Diagnostics.Process.Start(reportVM2x);
                        //Process.Start("notepad++.exe", reportVM1x);
                        break;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void btnCopyStat_Click(object sender, EventArgs e)
        {
            if (isRunning) return;
            System.Windows.Forms.Clipboard.SetText(lbStat.Text);
            lbCopied.Visible = true;
            bw_showHide.RunWorkerAsync();
        }

        private void btnFilterNext_Click(object sender, EventArgs e)
        {
            if (isRunning) return;
            string tname = "";
            int prevFiltedIndex = -1;
            int listCount= 0;
            try
            {
                switch (tabIndex)
                {
                    case 0:
                        listCount = lbxResults1.Items.Count;
                        if (currentFiltedIndex1 < 0 || currentFiltedIndex1 >= listCount-1) return;
                        tname = txtFilter1.Text.ToLower();
                        if (tname.Length <= 0) return;
                        
                        if (listCount <= 0) return;

                        prevFiltedIndex = currentFiltedIndex1;
                        for (int i = currentFiltedIndex1 + 1; i < listCount ; i++)
                        {
                            if (cheExact1.Checked)
                            {
                                if (lbxResults1.Items[i].ToString().ToLower().EndsWith(" | " + tname)) { currentFiltedIndex1 = i; break; }
                            }
                            else
                            {
                                if (lbxResults1.Items[i].ToString().ToLower().Contains(tname)) { currentFiltedIndex1 = i; break; }
                            }
                            
                        }
                        if (currentFiltedIndex1 == prevFiltedIndex)
                        {
                            lbFilterNotFound1.Visible = true;
                            bwShowHideSignal = -2; // -2 for not_found
                            bw_showHide.RunWorkerAsync();
                            return;
                        }
                        
                        if (currentFiltedIndex1 >= 0)
                        {
                            lbxResults1.SelectedIndex = currentFiltedIndex1;
                            //lbFilterFound1.Visible = true;
                            //bwShowHideSignal = 2; // 2 for found
                            //bw_showHide.RunWorkerAsync();
                        }
                        
                        break;
                    case 1:
                        listCount = lbxResults2.Items.Count;
                        if (currentFiltedIndex2 < 0 || currentFiltedIndex2 >= listCount-1) return;
                        tname = txtFilter2.Text.ToLower();
                        if (tname.Length <= 0) return;
                        
                        if (listCount <= 0) return;
                        if (currentFiltedIndex2 >= listCount - 1) { MessageBox.Show("Reached the end of list"); return; }
                        prevFiltedIndex = currentFiltedIndex2;
                        for (int i = currentFiltedIndex2 + 1; i < listCount ; i++)
                        {
                            if (cheExact2.Checked)
                            {
                                if (lbxResults2.Items[i].ToString().ToLower().EndsWith(" | " + tname)) { currentFiltedIndex2 = i; break; }
                            }
                            else
                            {
                                if (lbxResults2.Items[i].ToString().ToLower().Contains(tname)) { currentFiltedIndex2 = i; break; }
                            }

                            
                        }
                        if (currentFiltedIndex2 == prevFiltedIndex)
                        {
                            lbFilterNotFound2.Visible = true;
                            bwShowHideSignal = -2; // -2 for not_found
                            bw_showHide.RunWorkerAsync();
                            return;
                        }
                        if (currentFiltedIndex2 >= 0)
                        {
                            lbxResults2.SelectedIndex = currentFiltedIndex2;
                            //lbFilterFound2.Visible = true;
                            //bwShowHideSignal = 2; // 2 for found
                            //bw_showHide.RunWorkerAsync();
                        }
                        break;
                    default: break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnFilterPrevious_Click(object sender, EventArgs e)
        {
            if (isRunning) return;
            string tname = "";
            int prevFiltedIndex = -1;
            int listCount = 0;
            try
            {
                switch (tabIndex)
                {
                    case 0:
                        listCount = lbxResults1.Items.Count;
                        if (currentFiltedIndex1 <= 0 || currentFiltedIndex1 > listCount - 1) return;
                        tname = txtFilter1.Text.ToLower();
                        if (tname.Length <= 0) return;
                        
                        if (listCount <= 0) return;

                        prevFiltedIndex = currentFiltedIndex1;
                        for (int i = currentFiltedIndex1 -1; i >= 0; i--)
                        {
                            if (cheExact1.Checked)
                            {
                                if (lbxResults1.Items[i].ToString().ToLower().EndsWith(" | " + tname)) { currentFiltedIndex1 = i; break; }
                            }
                            else
                            {
                                if (lbxResults1.Items[i].ToString().ToLower().Contains(tname)) { currentFiltedIndex1 = i; break; }
                            }
                            
                        }
                        if (currentFiltedIndex1 == prevFiltedIndex) return;
                        if (currentFiltedIndex1 >= 0)
                        {
                            lbxResults1.SelectedIndex = currentFiltedIndex1;
                            //lbFilterFound1.Visible = true;
                            //bwShowHideSignal = 2; // 2 for found
                            //bw_showHide.RunWorkerAsync();
                        }

                        break;
                    case 1:
                        listCount = lbxResults2.Items.Count;
                        if (currentFiltedIndex2 <= 0 || currentFiltedIndex2 > listCount-1) return;
                        tname = txtFilter2.Text.ToLower();
                        if (tname.Length <= 0) return;
                        
                        if (listCount <= 0) return;
                        if (currentFiltedIndex2 >= listCount - 1 || currentFiltedIndex2 < 1) { MessageBox.Show("Reached the beginning of list"); return; }
                        prevFiltedIndex = currentFiltedIndex2;
                        for (int i = currentFiltedIndex2 -1; i >= 0; i--)
                        {
                            if (cheExact2.Checked)
                            {
                                if (lbxResults2.Items[i].ToString().ToLower().EndsWith(" | " + tname)) { currentFiltedIndex2 = i; break; }
                            }
                            else
                            {
                                if (lbxResults2.Items[i].ToString().ToLower().Contains(tname)) { currentFiltedIndex2 = i; break; }
                            }
                            
                        }
                        if (currentFiltedIndex2 == prevFiltedIndex) return;
                        if (currentFiltedIndex2 >= 0)
                        {
                            lbxResults2.SelectedIndex = currentFiltedIndex2;
                            //lbFilterFound2.Visible = true;
                            //bwShowHideSignal = 2; // 2 for found
                            //bw_showHide.RunWorkerAsync();
                        }
                        break;
                    default: break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        private void txtFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (isRunning) return;
            if (e.KeyCode != Keys.Enter) return;
            switch (tabIndex)
            {
                case 0: btnFilterGo1.PerformClick(); break;
                case 1: btnFilterGo2.PerformClick(); break;
                default: break;
            }
            
        }

        private void btnFilterGo_Click(object sender, EventArgs e)
        {

            if (isRunning) return;
            string tname = "";
            
            try
            {
                switch (tabIndex)
                {
                    case 0:
                        currentFiltedIndex1 = -1;
                        tname = txtFilter1.Text.ToLower();
                        if (tname.Length <= 0) throw new Exception("Error: name is empty");
                        if (lbxResults1.Items.Count <= 0) throw new Exception("Error: list is empty");

                        for (int i = 0; i < lbxResults1.Items.Count; i++)
                        {
                            if (cheExact1.Checked)
                            {
                                if (lbxResults1.Items[i].ToString().ToLower().EndsWith(" | " + tname)) { currentFiltedIndex1 = i; break; }
                            }
                            else
                            {
                                if (lbxResults1.Items[i].ToString().ToLower().Contains(tname)) { currentFiltedIndex1 = i; break; }
                            }
                            
                        }

                        if (currentFiltedIndex1 >= 0)
                        {
                            lbxResults1.SelectedIndex = currentFiltedIndex1;
                            lbFilterFound1.Visible = true;
                            bwShowHideSignal = 2; // 2 for found
                            bw_showHide.RunWorkerAsync();
                            btnFilterNext1.Enabled = true;
                            btnFilterPrevious1.Enabled = true;
                        }
                        else
                        {
                            lbFilterNotFound1.Visible = true;
                            bwShowHideSignal = -2; // -2 for not_found
                            bw_showHide.RunWorkerAsync();
                        }
                        break;
                    case 1:
                        currentFiltedIndex2 = -1;
                        tname = txtFilter2.Text.ToLower();
                        if (tname.Length <= 0) throw new Exception("Error: name is empty");
                        if (lbxResults2.Items.Count <= 0) throw new Exception("Error: list is empty");

                        for (int i = 0; i < lbxResults2.Items.Count; i++)
                        {
                            if (cheExact2.Checked)
                            {
                                if (lbxResults2.Items[i].ToString().ToLower().EndsWith(" | " + tname)) { currentFiltedIndex2 = i; break; }
                            }
                            else
                            {
                                if (lbxResults2.Items[i].ToString().ToLower().Contains(tname)) { currentFiltedIndex2 = i; break; }
                            }
                            
                        }

                        if (currentFiltedIndex2 >= 0)
                        {
                            lbxResults2.SelectedIndex = currentFiltedIndex2;
                            lbFilterFound2.Visible = true;
                            bwShowHideSignal = 2; // 2 for found
                            bw_showHide.RunWorkerAsync();
                            btnFilterNext2.Enabled = true;
                            btnFilterPrevious2.Enabled = true;
                        }
                        else
                        {
                            lbFilterNotFound2.Visible = true;
                            bwShowHideSignal = -2; // -2 for not_found
                            bw_showHide.RunWorkerAsync();
                        }
                        break;

                    default: break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            switch(tabIndex)
            {
                case 0: btnFilterNext1.Enabled = false; btnFilterPrevious1.Enabled = false; break;
                case 1: btnFilterNext2.Enabled = false; btnFilterPrevious2.Enabled = false; break;
                default: break;

            }
        }

    }
}
