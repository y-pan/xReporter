namespace xReporter
{
    partial class xReporter
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.txtRemoteRoof1 = new System.Windows.Forms.TextBox();
            this.txtRemoteBin1 = new System.Windows.Forms.TextBox();
            this.cheDownloadResults1 = new System.Windows.Forms.CheckBox();
            this.txtRemoteResults1 = new System.Windows.Forms.TextBox();
            this.txtRemoteTests1 = new System.Windows.Forms.TextBox();
            this.btnGenerateReports = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtResultsFrom1 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtResultsTo1 = new System.Windows.Forms.TextBox();
            this.btnEmptyLocalResults = new System.Windows.Forms.Button();
            this.btnDownloadResults = new System.Windows.Forms.Button();
            this.btnOpenLocal = new System.Windows.Forms.Button();
            this.gbVM1 = new System.Windows.Forms.GroupBox();
            this.comFeature1 = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.rdVM1 = new System.Windows.Forms.RadioButton();
            this.rdVM2 = new System.Windows.Forms.RadioButton();
            this.gbVM2 = new System.Windows.Forms.GroupBox();
            this.comFeature2 = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtRemoteRoof2 = new System.Windows.Forms.TextBox();
            this.txtRemoteBin2 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtRemoteResults2 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtRemoteTests2 = new System.Windows.Forms.TextBox();
            this.cheDownloadResults2 = new System.Windows.Forms.CheckBox();
            this.txtResultsFrom2 = new System.Windows.Forms.TextBox();
            this.txtResultsTo2 = new System.Windows.Forms.TextBox();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.lbxResults = new System.Windows.Forms.ListBox();
            this.lbStat = new System.Windows.Forms.Label();
            this.cheFailedOnly = new System.Windows.Forms.CheckBox();
            this.cheFindParent = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.lbInfo = new System.Windows.Forms.Label();
            this.gbVM1.SuspendLayout();
            this.gbVM2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtRemoteRoof1
            // 
            this.txtRemoteRoof1.Location = new System.Drawing.Point(119, 25);
            this.txtRemoteRoof1.Name = "txtRemoteRoof1";
            this.txtRemoteRoof1.Size = new System.Drawing.Size(439, 26);
            this.txtRemoteRoof1.TabIndex = 3;
            this.txtRemoteRoof1.Text = "s:\\Documents\\rlogger\\Roof";
            this.txtRemoteRoof1.TextChanged += new System.EventHandler(this.txtRemoteRoof_TextChanged);
            this.txtRemoteRoof1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.txt2Open_MouseDoubleClick);
            // 
            // txtRemoteBin1
            // 
            this.txtRemoteBin1.Location = new System.Drawing.Point(119, 99);
            this.txtRemoteBin1.Name = "txtRemoteBin1";
            this.txtRemoteBin1.Size = new System.Drawing.Size(439, 26);
            this.txtRemoteBin1.TabIndex = 5;
            this.txtRemoteBin1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.txt2Open_MouseDoubleClick);
            // 
            // cheDownloadResults1
            // 
            this.cheDownloadResults1.AutoSize = true;
            this.cheDownloadResults1.Location = new System.Drawing.Point(119, 218);
            this.cheDownloadResults1.Name = "cheDownloadResults1";
            this.cheDownloadResults1.Size = new System.Drawing.Size(164, 24);
            this.cheDownloadResults1.TabIndex = 4;
            this.cheDownloadResults1.Text = "Download Results";
            this.cheDownloadResults1.UseVisualStyleBackColor = true;
            this.cheDownloadResults1.CheckedChanged += new System.EventHandler(this.cheDownloadResults_CheckedChanged);
            // 
            // txtRemoteResults1
            // 
            this.txtRemoteResults1.Location = new System.Drawing.Point(119, 173);
            this.txtRemoteResults1.Name = "txtRemoteResults1";
            this.txtRemoteResults1.Size = new System.Drawing.Size(439, 26);
            this.txtRemoteResults1.TabIndex = 7;
            this.txtRemoteResults1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.txt2Open_MouseDoubleClick);
            // 
            // txtRemoteTests1
            // 
            this.txtRemoteTests1.Location = new System.Drawing.Point(119, 136);
            this.txtRemoteTests1.Name = "txtRemoteTests1";
            this.txtRemoteTests1.Size = new System.Drawing.Size(439, 26);
            this.txtRemoteTests1.TabIndex = 9;
            this.txtRemoteTests1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.txt2Open_MouseDoubleClick);
            // 
            // btnGenerateReports
            // 
            this.btnGenerateReports.Location = new System.Drawing.Point(139, 491);
            this.btnGenerateReports.Name = "btnGenerateReports";
            this.btnGenerateReports.Size = new System.Drawing.Size(173, 45);
            this.btnGenerateReports.TabIndex = 13;
            this.btnGenerateReports.Text = "Generate Report";
            this.btnGenerateReports.UseVisualStyleBackColor = true;
            this.btnGenerateReports.Click += new System.EventHandler(this.btnGenerateReports_Click);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(1121, 463);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 38);
            this.btnTest.TabIndex = 14;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(54, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 20);
            this.label1.TabIndex = 15;
            this.label1.Text = "Roof :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(66, 103);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 20);
            this.label2.TabIndex = 16;
            this.label2.Text = "Bin :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(50, 141);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 20);
            this.label3.TabIndex = 16;
            this.label3.Text = "Tests :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(35, 179);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 20);
            this.label4.TabIndex = 16;
            this.label4.Text = "Results :";
            // 
            // txtResultsFrom1
            // 
            this.txtResultsFrom1.Enabled = false;
            this.txtResultsFrom1.Location = new System.Drawing.Point(119, 248);
            this.txtResultsFrom1.Name = "txtResultsFrom1";
            this.txtResultsFrom1.Size = new System.Drawing.Size(439, 26);
            this.txtResultsFrom1.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(2, 255);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 20);
            this.label5.TabIndex = 16;
            this.label5.Text = "Result From :";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(21, 305);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(85, 20);
            this.label6.TabIndex = 16;
            this.label6.Text = "Result To :";
            // 
            // txtResultsTo1
            // 
            this.txtResultsTo1.Enabled = false;
            this.txtResultsTo1.Location = new System.Drawing.Point(119, 297);
            this.txtResultsTo1.Name = "txtResultsTo1";
            this.txtResultsTo1.Size = new System.Drawing.Size(439, 26);
            this.txtResultsTo1.TabIndex = 7;
            // 
            // btnEmptyLocalResults
            // 
            this.btnEmptyLocalResults.BackColor = System.Drawing.Color.Tomato;
            this.btnEmptyLocalResults.ForeColor = System.Drawing.Color.Black;
            this.btnEmptyLocalResults.Location = new System.Drawing.Point(16, 542);
            this.btnEmptyLocalResults.Name = "btnEmptyLocalResults";
            this.btnEmptyLocalResults.Size = new System.Drawing.Size(296, 45);
            this.btnEmptyLocalResults.TabIndex = 13;
            this.btnEmptyLocalResults.Text = "Clear Local Results";
            this.btnEmptyLocalResults.UseVisualStyleBackColor = false;
            this.btnEmptyLocalResults.Click += new System.EventHandler(this.btnEmptyLocalResults_Click);
            // 
            // btnDownloadResults
            // 
            this.btnDownloadResults.ForeColor = System.Drawing.Color.Blue;
            this.btnDownloadResults.Location = new System.Drawing.Point(16, 439);
            this.btnDownloadResults.Name = "btnDownloadResults";
            this.btnDownloadResults.Size = new System.Drawing.Size(296, 45);
            this.btnDownloadResults.TabIndex = 18;
            this.btnDownloadResults.Text = "Download Results";
            this.btnDownloadResults.UseVisualStyleBackColor = true;
            this.btnDownloadResults.Click += new System.EventHandler(this.btnDownloadResults_Click);
            // 
            // btnOpenLocal
            // 
            this.btnOpenLocal.ForeColor = System.Drawing.Color.DarkGreen;
            this.btnOpenLocal.Location = new System.Drawing.Point(16, 387);
            this.btnOpenLocal.Name = "btnOpenLocal";
            this.btnOpenLocal.Size = new System.Drawing.Size(296, 45);
            this.btnOpenLocal.TabIndex = 14;
            this.btnOpenLocal.Text = "Open Local Results";
            this.btnOpenLocal.UseVisualStyleBackColor = true;
            this.btnOpenLocal.Click += new System.EventHandler(this.btnOpenLocal_Click);
            // 
            // gbVM1
            // 
            this.gbVM1.Controls.Add(this.comFeature1);
            this.gbVM1.Controls.Add(this.label14);
            this.gbVM1.Controls.Add(this.txtRemoteRoof1);
            this.gbVM1.Controls.Add(this.txtRemoteBin1);
            this.gbVM1.Controls.Add(this.label3);
            this.gbVM1.Controls.Add(this.label4);
            this.gbVM1.Controls.Add(this.label6);
            this.gbVM1.Controls.Add(this.txtRemoteResults1);
            this.gbVM1.Controls.Add(this.label5);
            this.gbVM1.Controls.Add(this.label2);
            this.gbVM1.Controls.Add(this.label1);
            this.gbVM1.Controls.Add(this.txtRemoteTests1);
            this.gbVM1.Controls.Add(this.cheDownloadResults1);
            this.gbVM1.Controls.Add(this.txtResultsFrom1);
            this.gbVM1.Controls.Add(this.txtResultsTo1);
            this.gbVM1.ForeColor = System.Drawing.Color.Black;
            this.gbVM1.Location = new System.Drawing.Point(16, 41);
            this.gbVM1.Name = "gbVM1";
            this.gbVM1.Size = new System.Drawing.Size(575, 336);
            this.gbVM1.TabIndex = 19;
            this.gbVM1.TabStop = false;
            this.gbVM1.Text = "VM 1";
            // 
            // comFeature1
            // 
            this.comFeature1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comFeature1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comFeature1.FormattingEnabled = true;
            this.comFeature1.Location = new System.Drawing.Point(119, 60);
            this.comFeature1.Name = "comFeature1";
            this.comFeature1.Size = new System.Drawing.Size(439, 28);
            this.comFeature1.TabIndex = 22;
            this.comFeature1.SelectedIndexChanged += new System.EventHandler(this.comFeature_Changed);
            this.comFeature1.TextChanged += new System.EventHandler(this.comFeature_Changed);
            this.comFeature1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.txt2Open_MouseDoubleClick);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(33, 65);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(73, 20);
            this.label14.TabIndex = 20;
            this.label14.Text = "Feature :";
            // 
            // rdVM1
            // 
            this.rdVM1.AutoSize = true;
            this.rdVM1.Location = new System.Drawing.Point(518, 12);
            this.rdVM1.Name = "rdVM1";
            this.rdVM1.Size = new System.Drawing.Size(67, 24);
            this.rdVM1.TabIndex = 17;
            this.rdVM1.TabStop = true;
            this.rdVM1.Text = "VM1";
            this.rdVM1.UseVisualStyleBackColor = true;
            this.rdVM1.CheckedChanged += new System.EventHandler(this.rdVM1_CheckedChanged);
            // 
            // rdVM2
            // 
            this.rdVM2.AutoSize = true;
            this.rdVM2.Location = new System.Drawing.Point(636, 12);
            this.rdVM2.Name = "rdVM2";
            this.rdVM2.Size = new System.Drawing.Size(67, 24);
            this.rdVM2.TabIndex = 17;
            this.rdVM2.TabStop = true;
            this.rdVM2.Text = "VM2";
            this.rdVM2.UseVisualStyleBackColor = true;
            this.rdVM2.CheckedChanged += new System.EventHandler(this.rdVM2_CheckedChanged);
            // 
            // gbVM2
            // 
            this.gbVM2.Controls.Add(this.comFeature2);
            this.gbVM2.Controls.Add(this.label13);
            this.gbVM2.Controls.Add(this.txtRemoteRoof2);
            this.gbVM2.Controls.Add(this.txtRemoteBin2);
            this.gbVM2.Controls.Add(this.label7);
            this.gbVM2.Controls.Add(this.label8);
            this.gbVM2.Controls.Add(this.label9);
            this.gbVM2.Controls.Add(this.txtRemoteResults2);
            this.gbVM2.Controls.Add(this.label10);
            this.gbVM2.Controls.Add(this.label11);
            this.gbVM2.Controls.Add(this.label12);
            this.gbVM2.Controls.Add(this.txtRemoteTests2);
            this.gbVM2.Controls.Add(this.cheDownloadResults2);
            this.gbVM2.Controls.Add(this.txtResultsFrom2);
            this.gbVM2.Controls.Add(this.txtResultsTo2);
            this.gbVM2.Location = new System.Drawing.Point(636, 41);
            this.gbVM2.Name = "gbVM2";
            this.gbVM2.Size = new System.Drawing.Size(575, 336);
            this.gbVM2.TabIndex = 20;
            this.gbVM2.TabStop = false;
            this.gbVM2.Text = "VM 2";
            // 
            // comFeature2
            // 
            this.comFeature2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comFeature2.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comFeature2.FormattingEnabled = true;
            this.comFeature2.Location = new System.Drawing.Point(121, 62);
            this.comFeature2.Name = "comFeature2";
            this.comFeature2.Size = new System.Drawing.Size(439, 28);
            this.comFeature2.TabIndex = 23;
            this.comFeature2.SelectedIndexChanged += new System.EventHandler(this.comFeature_Changed);
            this.comFeature2.TextChanged += new System.EventHandler(this.comFeature_Changed);
            this.comFeature2.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.txt2Open_MouseDoubleClick);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(35, 68);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(73, 20);
            this.label13.TabIndex = 17;
            this.label13.Text = "Feature :";
            // 
            // txtRemoteRoof2
            // 
            this.txtRemoteRoof2.Location = new System.Drawing.Point(121, 25);
            this.txtRemoteRoof2.Name = "txtRemoteRoof2";
            this.txtRemoteRoof2.Size = new System.Drawing.Size(439, 26);
            this.txtRemoteRoof2.TabIndex = 3;
            this.txtRemoteRoof2.TextChanged += new System.EventHandler(this.txtRemoteRoof_TextChanged);
            this.txtRemoteRoof2.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.txt2Open_MouseDoubleClick);
            // 
            // txtRemoteBin2
            // 
            this.txtRemoteBin2.Location = new System.Drawing.Point(121, 99);
            this.txtRemoteBin2.Name = "txtRemoteBin2";
            this.txtRemoteBin2.Size = new System.Drawing.Size(439, 26);
            this.txtRemoteBin2.TabIndex = 5;
            this.txtRemoteBin2.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.txt2Open_MouseDoubleClick);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(52, 139);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 20);
            this.label7.TabIndex = 16;
            this.label7.Text = "Tests :";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(37, 176);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(71, 20);
            this.label8.TabIndex = 16;
            this.label8.Text = "Results :";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(23, 300);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(85, 20);
            this.label9.TabIndex = 16;
            this.label9.Text = "Result To :";
            // 
            // txtRemoteResults2
            // 
            this.txtRemoteResults2.Location = new System.Drawing.Point(121, 173);
            this.txtRemoteResults2.Name = "txtRemoteResults2";
            this.txtRemoteResults2.Size = new System.Drawing.Size(439, 26);
            this.txtRemoteResults2.TabIndex = 7;
            this.txtRemoteResults2.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.txt2Open_MouseDoubleClick);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(4, 251);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(104, 20);
            this.label10.TabIndex = 16;
            this.label10.Text = "Result From :";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(68, 102);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(40, 20);
            this.label11.TabIndex = 16;
            this.label11.Text = "Bin :";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(56, 28);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(52, 20);
            this.label12.TabIndex = 15;
            this.label12.Text = "Roof :";
            // 
            // txtRemoteTests2
            // 
            this.txtRemoteTests2.Location = new System.Drawing.Point(121, 136);
            this.txtRemoteTests2.Name = "txtRemoteTests2";
            this.txtRemoteTests2.Size = new System.Drawing.Size(439, 26);
            this.txtRemoteTests2.TabIndex = 9;
            this.txtRemoteTests2.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.txt2Open_MouseDoubleClick);
            // 
            // cheDownloadResults2
            // 
            this.cheDownloadResults2.AutoSize = true;
            this.cheDownloadResults2.Location = new System.Drawing.Point(121, 218);
            this.cheDownloadResults2.Name = "cheDownloadResults2";
            this.cheDownloadResults2.Size = new System.Drawing.Size(164, 24);
            this.cheDownloadResults2.TabIndex = 4;
            this.cheDownloadResults2.Text = "Download Results";
            this.cheDownloadResults2.UseVisualStyleBackColor = true;
            this.cheDownloadResults2.CheckedChanged += new System.EventHandler(this.cheDownloadResults_CheckedChanged);
            // 
            // txtResultsFrom2
            // 
            this.txtResultsFrom2.Enabled = false;
            this.txtResultsFrom2.Location = new System.Drawing.Point(121, 248);
            this.txtResultsFrom2.Name = "txtResultsFrom2";
            this.txtResultsFrom2.Size = new System.Drawing.Size(439, 26);
            this.txtResultsFrom2.TabIndex = 7;
            // 
            // txtResultsTo2
            // 
            this.txtResultsTo2.Enabled = false;
            this.txtResultsTo2.Location = new System.Drawing.Point(121, 297);
            this.txtResultsTo2.Name = "txtResultsTo2";
            this.txtResultsTo2.Size = new System.Drawing.Size(439, 26);
            this.txtResultsTo2.TabIndex = 7;
            // 
            // textBox9
            // 
            this.textBox9.Location = new System.Drawing.Point(135, 10);
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(377, 26);
            this.textBox9.TabIndex = 21;
            // 
            // textBox10
            // 
            this.textBox10.Location = new System.Drawing.Point(757, 10);
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new System.Drawing.Size(439, 26);
            this.textBox10.TabIndex = 21;
            // 
            // lbxResults
            // 
            this.lbxResults.BackColor = System.Drawing.SystemColors.Info;
            this.lbxResults.FormattingEnabled = true;
            this.lbxResults.HorizontalScrollbar = true;
            this.lbxResults.ItemHeight = 20;
            this.lbxResults.Location = new System.Drawing.Point(320, 387);
            this.lbxResults.Name = "lbxResults";
            this.lbxResults.Size = new System.Drawing.Size(731, 204);
            this.lbxResults.TabIndex = 22;
            this.lbxResults.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.LbxResults_MouseDoubleClick);
            // 
            // lbStat
            // 
            this.lbStat.AutoSize = true;
            this.lbStat.Location = new System.Drawing.Point(469, 601);
            this.lbStat.Name = "lbStat";
            this.lbStat.Size = new System.Drawing.Size(43, 20);
            this.lbStat.TabIndex = 23;
            this.lbStat.Text = "Stat:";
            // 
            // cheFailedOnly
            // 
            this.cheFailedOnly.AutoSize = true;
            this.cheFailedOnly.Checked = true;
            this.cheFailedOnly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cheFailedOnly.Location = new System.Drawing.Point(320, 597);
            this.cheFailedOnly.Name = "cheFailedOnly";
            this.cheFailedOnly.Size = new System.Drawing.Size(116, 24);
            this.cheFailedOnly.TabIndex = 24;
            this.cheFailedOnly.Text = "View Failed";
            this.cheFailedOnly.UseVisualStyleBackColor = true;
            this.cheFailedOnly.CheckedChanged += new System.EventHandler(this.cheFailedOnly_CheckedChanged);
            // 
            // cheFindParent
            // 
            this.cheFindParent.AutoSize = true;
            this.cheFindParent.Checked = true;
            this.cheFindParent.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cheFindParent.Location = new System.Drawing.Point(16, 502);
            this.cheFindParent.Name = "cheFindParent";
            this.cheFindParent.Size = new System.Drawing.Size(117, 24);
            this.cheFindParent.TabIndex = 25;
            this.cheFindParent.Text = "Find Parent";
            this.cheFindParent.UseVisualStyleBackColor = true;
            // 
            // toolTip1
            // 
            this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            // 
            // lbInfo
            // 
            this.lbInfo.AutoSize = true;
            this.lbInfo.Location = new System.Drawing.Point(18, 597);
            this.lbInfo.Name = "lbInfo";
            this.lbInfo.Size = new System.Drawing.Size(45, 20);
            this.lbInfo.TabIndex = 17;
            this.lbInfo.Text = "Info :";
            // 
            // xReporter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1223, 677);
            this.Controls.Add(this.cheFindParent);
            this.Controls.Add(this.cheFailedOnly);
            this.Controls.Add(this.lbStat);
            this.Controls.Add(this.lbxResults);
            this.Controls.Add(this.textBox10);
            this.Controls.Add(this.textBox9);
            this.Controls.Add(this.gbVM2);
            this.Controls.Add(this.rdVM2);
            this.Controls.Add(this.rdVM1);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.gbVM1);
            this.Controls.Add(this.btnDownloadResults);
            this.Controls.Add(this.lbInfo);
            this.Controls.Add(this.btnOpenLocal);
            this.Controls.Add(this.btnEmptyLocalResults);
            this.Controls.Add(this.btnGenerateReports);
            this.Name = "xReporter";
            this.Text = "xReporter";
            this.Load += new System.EventHandler(this.xReporter_Load);
            this.gbVM1.ResumeLayout(false);
            this.gbVM1.PerformLayout();
            this.gbVM2.ResumeLayout(false);
            this.gbVM2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtRemoteRoof1;
        private System.Windows.Forms.TextBox txtRemoteBin1;
        private System.Windows.Forms.CheckBox cheDownloadResults1;
        private System.Windows.Forms.TextBox txtRemoteResults1;
        private System.Windows.Forms.TextBox txtRemoteTests1;
        private System.Windows.Forms.Button btnGenerateReports;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtResultsFrom1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtResultsTo1;
        private System.Windows.Forms.Button btnEmptyLocalResults;
        private System.Windows.Forms.Button btnDownloadResults;
        private System.Windows.Forms.Button btnOpenLocal;
        private System.Windows.Forms.GroupBox gbVM1;
        private System.Windows.Forms.RadioButton rdVM1;
        private System.Windows.Forms.RadioButton rdVM2;
        private System.Windows.Forms.GroupBox gbVM2;
        private System.Windows.Forms.TextBox txtRemoteRoof2;
        private System.Windows.Forms.TextBox txtRemoteBin2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtRemoteResults2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtRemoteTests2;
        private System.Windows.Forms.TextBox txtResultsFrom2;
        private System.Windows.Forms.TextBox txtResultsTo2;
        private System.Windows.Forms.CheckBox cheDownloadResults2;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.TextBox textBox10;
        private System.Windows.Forms.ComboBox comFeature1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox comFeature2;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ListBox lbxResults;
        private System.Windows.Forms.Label lbStat;
        private System.Windows.Forms.CheckBox cheFailedOnly;
        private System.Windows.Forms.CheckBox cheFindParent;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label lbInfo;
    }
}

