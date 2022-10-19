namespace Aseprite_Multiple_Export
{
    partial class Main
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtAsepriteSearch = new System.Windows.Forms.TextBox();
            this.lblAsepriteSearch = new System.Windows.Forms.Label();
            this.btnAsepriteSearch = new System.Windows.Forms.Button();
            this.btnFolderSearch = new System.Windows.Forms.Button();
            this.lblFolderSearch = new System.Windows.Forms.Label();
            this.txtFolderSearch = new System.Windows.Forms.TextBox();
            this.lblFileList = new System.Windows.Forms.Label();
            this.lstFileList = new System.Windows.Forms.ListBox();
            this.lblLayerList = new System.Windows.Forms.Label();
            this.txtLayerList = new System.Windows.Forms.TextBox();
            this.txtWarning = new System.Windows.Forms.Label();
            this.grpSuffixList = new System.Windows.Forms.GroupBox();
            this.nudScale = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.nudDefaultColumns = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.nudColumns3 = new System.Windows.Forms.NumericUpDown();
            this.nudColumns2 = new System.Windows.Forms.NumericUpDown();
            this.nudColumns1 = new System.Windows.Forms.NumericUpDown();
            this.lblSuffixCondition9 = new System.Windows.Forms.Label();
            this.lblSuffixCondition6 = new System.Windows.Forms.Label();
            this.lblSuffixCondition3 = new System.Windows.Forms.Label();
            this.txtDefaultOutputName = new System.Windows.Forms.TextBox();
            this.lblDefaultOutputName = new System.Windows.Forms.Label();
            this.txtSuffix3 = new System.Windows.Forms.TextBox();
            this.lblSuffixCondition8 = new System.Windows.Forms.Label();
            this.txtCondition3 = new System.Windows.Forms.TextBox();
            this.lblSuffixCondition7 = new System.Windows.Forms.Label();
            this.txtSuffix2 = new System.Windows.Forms.TextBox();
            this.lblSuffixCondition5 = new System.Windows.Forms.Label();
            this.txtCondition2 = new System.Windows.Forms.TextBox();
            this.lblSuffixCondition4 = new System.Windows.Forms.Label();
            this.txtSuffix1 = new System.Windows.Forms.TextBox();
            this.lblSuffixCondition2 = new System.Windows.Forms.Label();
            this.txtCondition1 = new System.Windows.Forms.TextBox();
            this.lblSuffixCondition1 = new System.Windows.Forms.Label();
            this.chkExportData = new System.Windows.Forms.CheckBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.chkOriginalFilename = new System.Windows.Forms.CheckBox();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnExportDebug = new System.Windows.Forms.Button();
            this.chkEveryLayer = new System.Windows.Forms.CheckBox();
            this.grpSuffixList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDefaultColumns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudColumns3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudColumns2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudColumns1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtAsepriteSearch
            // 
            this.txtAsepriteSearch.Enabled = false;
            this.txtAsepriteSearch.Location = new System.Drawing.Point(156, 6);
            this.txtAsepriteSearch.Name = "txtAsepriteSearch";
            this.txtAsepriteSearch.Size = new System.Drawing.Size(649, 23);
            this.txtAsepriteSearch.TabIndex = 1;
            // 
            // lblAsepriteSearch
            // 
            this.lblAsepriteSearch.AutoSize = true;
            this.lblAsepriteSearch.Location = new System.Drawing.Point(12, 9);
            this.lblAsepriteSearch.Name = "lblAsepriteSearch";
            this.lblAsepriteSearch.Size = new System.Drawing.Size(138, 15);
            this.lblAsepriteSearch.TabIndex = 0;
            this.lblAsepriteSearch.Text = "Select aseprite \".exe\" file:";
            // 
            // btnAsepriteSearch
            // 
            this.btnAsepriteSearch.Location = new System.Drawing.Point(812, 6);
            this.btnAsepriteSearch.Name = "btnAsepriteSearch";
            this.btnAsepriteSearch.Size = new System.Drawing.Size(100, 23);
            this.btnAsepriteSearch.TabIndex = 2;
            this.btnAsepriteSearch.Text = "Search Aseprite";
            this.btnAsepriteSearch.UseVisualStyleBackColor = true;
            this.btnAsepriteSearch.Click += new System.EventHandler(this.btnAsepriteSearch_Click);
            // 
            // btnFolderSearch
            // 
            this.btnFolderSearch.Location = new System.Drawing.Point(812, 35);
            this.btnFolderSearch.Name = "btnFolderSearch";
            this.btnFolderSearch.Size = new System.Drawing.Size(100, 23);
            this.btnFolderSearch.TabIndex = 5;
            this.btnFolderSearch.Text = "Search Folder";
            this.btnFolderSearch.UseVisualStyleBackColor = true;
            this.btnFolderSearch.Click += new System.EventHandler(this.btnFolderSearch_Click);
            // 
            // lblFolderSearch
            // 
            this.lblFolderSearch.AutoSize = true;
            this.lblFolderSearch.Location = new System.Drawing.Point(12, 38);
            this.lblFolderSearch.Name = "lblFolderSearch";
            this.lblFolderSearch.Size = new System.Drawing.Size(213, 15);
            this.lblFolderSearch.TabIndex = 3;
            this.lblFolderSearch.Text = "Select folder with \".ase / .aseprite\" files:";
            // 
            // txtFolderSearch
            // 
            this.txtFolderSearch.Location = new System.Drawing.Point(231, 35);
            this.txtFolderSearch.Name = "txtFolderSearch";
            this.txtFolderSearch.Size = new System.Drawing.Size(574, 23);
            this.txtFolderSearch.TabIndex = 4;
            this.txtFolderSearch.TextChanged += new System.EventHandler(this.txtFolderSearch_TextChanged);
            // 
            // lblFileList
            // 
            this.lblFileList.AutoSize = true;
            this.lblFileList.Location = new System.Drawing.Point(12, 60);
            this.lblFileList.Name = "lblFileList";
            this.lblFileList.Size = new System.Drawing.Size(66, 15);
            this.lblFileList.TabIndex = 6;
            this.lblFileList.Text = "List of files:";
            // 
            // lstFileList
            // 
            this.lstFileList.FormattingEnabled = true;
            this.lstFileList.ItemHeight = 15;
            this.lstFileList.Location = new System.Drawing.Point(12, 79);
            this.lstFileList.Name = "lstFileList";
            this.lstFileList.Size = new System.Drawing.Size(900, 229);
            this.lstFileList.TabIndex = 7;
            // 
            // lblLayerList
            // 
            this.lblLayerList.AutoSize = true;
            this.lblLayerList.Location = new System.Drawing.Point(12, 317);
            this.lblLayerList.Name = "lblLayerList";
            this.lblLayerList.Size = new System.Drawing.Size(304, 15);
            this.lblLayerList.TabIndex = 8;
            this.lblLayerList.Text = "Comma separated layer list (e.g: helmet,armor,gloves...):";
            // 
            // txtLayerList
            // 
            this.txtLayerList.Location = new System.Drawing.Point(322, 314);
            this.txtLayerList.Name = "txtLayerList";
            this.txtLayerList.Size = new System.Drawing.Size(590, 23);
            this.txtLayerList.TabIndex = 9;
            this.txtLayerList.TextChanged += new System.EventHandler(this.txtLayerList_TextChanged);
            // 
            // txtWarning
            // 
            this.txtWarning.AutoSize = true;
            this.txtWarning.Location = new System.Drawing.Point(12, 341);
            this.txtWarning.Name = "txtWarning";
            this.txtWarning.Size = new System.Drawing.Size(661, 15);
            this.txtWarning.TabIndex = 10;
            this.txtWarning.Text = "Words with accents or special characters are not supported! if empty, only one im" +
    "age will be exported with all layers at once.";
            // 
            // grpSuffixList
            // 
            this.grpSuffixList.Controls.Add(this.nudScale);
            this.grpSuffixList.Controls.Add(this.label2);
            this.grpSuffixList.Controls.Add(this.nudDefaultColumns);
            this.grpSuffixList.Controls.Add(this.label1);
            this.grpSuffixList.Controls.Add(this.nudColumns3);
            this.grpSuffixList.Controls.Add(this.nudColumns2);
            this.grpSuffixList.Controls.Add(this.nudColumns1);
            this.grpSuffixList.Controls.Add(this.lblSuffixCondition9);
            this.grpSuffixList.Controls.Add(this.lblSuffixCondition6);
            this.grpSuffixList.Controls.Add(this.lblSuffixCondition3);
            this.grpSuffixList.Controls.Add(this.txtDefaultOutputName);
            this.grpSuffixList.Controls.Add(this.lblDefaultOutputName);
            this.grpSuffixList.Controls.Add(this.txtSuffix3);
            this.grpSuffixList.Controls.Add(this.lblSuffixCondition8);
            this.grpSuffixList.Controls.Add(this.txtCondition3);
            this.grpSuffixList.Controls.Add(this.lblSuffixCondition7);
            this.grpSuffixList.Controls.Add(this.txtSuffix2);
            this.grpSuffixList.Controls.Add(this.lblSuffixCondition5);
            this.grpSuffixList.Controls.Add(this.txtCondition2);
            this.grpSuffixList.Controls.Add(this.lblSuffixCondition4);
            this.grpSuffixList.Controls.Add(this.txtSuffix1);
            this.grpSuffixList.Controls.Add(this.lblSuffixCondition2);
            this.grpSuffixList.Controls.Add(this.txtCondition1);
            this.grpSuffixList.Controls.Add(this.lblSuffixCondition1);
            this.grpSuffixList.Location = new System.Drawing.Point(12, 365);
            this.grpSuffixList.Name = "grpSuffixList";
            this.grpSuffixList.Size = new System.Drawing.Size(900, 143);
            this.grpSuffixList.TabIndex = 11;
            this.grpSuffixList.TabStop = false;
            this.grpSuffixList.Text = "Output Filename Control";
            // 
            // nudScale
            // 
            this.nudScale.Location = new System.Drawing.Point(645, 21);
            this.nudScale.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudScale.Name = "nudScale";
            this.nudScale.Size = new System.Drawing.Size(109, 23);
            this.nudScale.TabIndex = 35;
            this.nudScale.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudScale.ValueChanged += new System.EventHandler(this.nudScale_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(561, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 15);
            this.label2.TabIndex = 34;
            this.label2.Text = "Default Scale:";
            // 
            // nudDefaultColumns
            // 
            this.nudDefaultColumns.Location = new System.Drawing.Point(433, 22);
            this.nudDefaultColumns.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudDefaultColumns.Name = "nudDefaultColumns";
            this.nudDefaultColumns.Size = new System.Drawing.Size(114, 23);
            this.nudDefaultColumns.TabIndex = 33;
            this.nudDefaultColumns.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.nudDefaultColumns.ValueChanged += new System.EventHandler(this.nudDefaultColumns_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(287, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 15);
            this.label1.TabIndex = 32;
            this.label1.Text = "Default Output Columns:";
            // 
            // nudColumns3
            // 
            this.nudColumns3.Location = new System.Drawing.Point(512, 110);
            this.nudColumns3.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudColumns3.Name = "nudColumns3";
            this.nudColumns3.Size = new System.Drawing.Size(155, 23);
            this.nudColumns3.TabIndex = 31;
            this.nudColumns3.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudColumns3.ValueChanged += new System.EventHandler(this.nudColumns3_ValueChanged);
            // 
            // nudColumns2
            // 
            this.nudColumns2.Location = new System.Drawing.Point(512, 81);
            this.nudColumns2.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudColumns2.Name = "nudColumns2";
            this.nudColumns2.Size = new System.Drawing.Size(155, 23);
            this.nudColumns2.TabIndex = 25;
            this.nudColumns2.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudColumns2.ValueChanged += new System.EventHandler(this.nudColumns2_ValueChanged);
            // 
            // nudColumns1
            // 
            this.nudColumns1.Location = new System.Drawing.Point(512, 52);
            this.nudColumns1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudColumns1.Name = "nudColumns1";
            this.nudColumns1.Size = new System.Drawing.Size(155, 23);
            this.nudColumns1.TabIndex = 19;
            this.nudColumns1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudColumns1.ValueChanged += new System.EventHandler(this.nudColumns1_ValueChanged);
            // 
            // lblSuffixCondition9
            // 
            this.lblSuffixCondition9.AutoSize = true;
            this.lblSuffixCondition9.Location = new System.Drawing.Point(407, 113);
            this.lblSuffixCondition9.Name = "lblSuffixCondition9";
            this.lblSuffixCondition9.Size = new System.Drawing.Size(99, 15);
            this.lblSuffixCondition9.TabIndex = 30;
            this.lblSuffixCondition9.Text = "and set Columns:";
            // 
            // lblSuffixCondition6
            // 
            this.lblSuffixCondition6.AutoSize = true;
            this.lblSuffixCondition6.Location = new System.Drawing.Point(407, 84);
            this.lblSuffixCondition6.Name = "lblSuffixCondition6";
            this.lblSuffixCondition6.Size = new System.Drawing.Size(99, 15);
            this.lblSuffixCondition6.TabIndex = 24;
            this.lblSuffixCondition6.Text = "and set Columns:";
            // 
            // lblSuffixCondition3
            // 
            this.lblSuffixCondition3.AutoSize = true;
            this.lblSuffixCondition3.Location = new System.Drawing.Point(407, 55);
            this.lblSuffixCondition3.Name = "lblSuffixCondition3";
            this.lblSuffixCondition3.Size = new System.Drawing.Size(99, 15);
            this.lblSuffixCondition3.TabIndex = 18;
            this.lblSuffixCondition3.Text = "and set Columns:";
            // 
            // txtDefaultOutputName
            // 
            this.txtDefaultOutputName.Location = new System.Drawing.Point(152, 22);
            this.txtDefaultOutputName.Name = "txtDefaultOutputName";
            this.txtDefaultOutputName.Size = new System.Drawing.Size(129, 23);
            this.txtDefaultOutputName.TabIndex = 13;
            this.txtDefaultOutputName.TextChanged += new System.EventHandler(this.txtDefaultOutputName_TextChanged);
            // 
            // lblDefaultOutputName
            // 
            this.lblDefaultOutputName.AutoSize = true;
            this.lblDefaultOutputName.Location = new System.Drawing.Point(6, 25);
            this.lblDefaultOutputName.Name = "lblDefaultOutputName";
            this.lblDefaultOutputName.Size = new System.Drawing.Size(140, 15);
            this.lblDefaultOutputName.TabIndex = 12;
            this.lblDefaultOutputName.Text = "Default Output Filename:";
            // 
            // txtSuffix3
            // 
            this.txtSuffix3.Location = new System.Drawing.Point(287, 110);
            this.txtSuffix3.Name = "txtSuffix3";
            this.txtSuffix3.Size = new System.Drawing.Size(114, 23);
            this.txtSuffix3.TabIndex = 29;
            this.txtSuffix3.TextChanged += new System.EventHandler(this.txtSuffix3_TextChanged);
            // 
            // lblSuffixCondition8
            // 
            this.lblSuffixCondition8.AutoSize = true;
            this.lblSuffixCondition8.Location = new System.Drawing.Point(219, 113);
            this.lblSuffixCondition8.Name = "lblSuffixCondition8";
            this.lblSuffixCondition8.Size = new System.Drawing.Size(62, 15);
            this.lblSuffixCondition8.TabIndex = 28;
            this.lblSuffixCondition8.Text = "add suffix:";
            // 
            // txtCondition3
            // 
            this.txtCondition3.Location = new System.Drawing.Point(99, 110);
            this.txtCondition3.Name = "txtCondition3";
            this.txtCondition3.Size = new System.Drawing.Size(114, 23);
            this.txtCondition3.TabIndex = 27;
            this.txtCondition3.TextChanged += new System.EventHandler(this.txtCondition3_TextChanged);
            // 
            // lblSuffixCondition7
            // 
            this.lblSuffixCondition7.AutoSize = true;
            this.lblSuffixCondition7.Location = new System.Drawing.Point(6, 113);
            this.lblSuffixCondition7.Name = "lblSuffixCondition7";
            this.lblSuffixCondition7.Size = new System.Drawing.Size(87, 15);
            this.lblSuffixCondition7.TabIndex = 26;
            this.lblSuffixCondition7.Text = "If filename has:";
            // 
            // txtSuffix2
            // 
            this.txtSuffix2.Location = new System.Drawing.Point(287, 81);
            this.txtSuffix2.Name = "txtSuffix2";
            this.txtSuffix2.Size = new System.Drawing.Size(114, 23);
            this.txtSuffix2.TabIndex = 23;
            this.txtSuffix2.TextChanged += new System.EventHandler(this.txtSuffix2_TextChanged);
            // 
            // lblSuffixCondition5
            // 
            this.lblSuffixCondition5.AutoSize = true;
            this.lblSuffixCondition5.Location = new System.Drawing.Point(219, 84);
            this.lblSuffixCondition5.Name = "lblSuffixCondition5";
            this.lblSuffixCondition5.Size = new System.Drawing.Size(62, 15);
            this.lblSuffixCondition5.TabIndex = 22;
            this.lblSuffixCondition5.Text = "add suffix:";
            // 
            // txtCondition2
            // 
            this.txtCondition2.Location = new System.Drawing.Point(99, 81);
            this.txtCondition2.Name = "txtCondition2";
            this.txtCondition2.Size = new System.Drawing.Size(114, 23);
            this.txtCondition2.TabIndex = 21;
            this.txtCondition2.TextChanged += new System.EventHandler(this.txtCondition2_TextChanged);
            // 
            // lblSuffixCondition4
            // 
            this.lblSuffixCondition4.AutoSize = true;
            this.lblSuffixCondition4.Location = new System.Drawing.Point(6, 84);
            this.lblSuffixCondition4.Name = "lblSuffixCondition4";
            this.lblSuffixCondition4.Size = new System.Drawing.Size(87, 15);
            this.lblSuffixCondition4.TabIndex = 20;
            this.lblSuffixCondition4.Text = "If filename has:";
            // 
            // txtSuffix1
            // 
            this.txtSuffix1.Location = new System.Drawing.Point(287, 52);
            this.txtSuffix1.Name = "txtSuffix1";
            this.txtSuffix1.Size = new System.Drawing.Size(114, 23);
            this.txtSuffix1.TabIndex = 17;
            this.txtSuffix1.TextChanged += new System.EventHandler(this.txtSuffix1_TextChanged);
            // 
            // lblSuffixCondition2
            // 
            this.lblSuffixCondition2.AutoSize = true;
            this.lblSuffixCondition2.Location = new System.Drawing.Point(219, 55);
            this.lblSuffixCondition2.Name = "lblSuffixCondition2";
            this.lblSuffixCondition2.Size = new System.Drawing.Size(62, 15);
            this.lblSuffixCondition2.TabIndex = 16;
            this.lblSuffixCondition2.Text = "add suffix:";
            // 
            // txtCondition1
            // 
            this.txtCondition1.Location = new System.Drawing.Point(99, 52);
            this.txtCondition1.Name = "txtCondition1";
            this.txtCondition1.Size = new System.Drawing.Size(114, 23);
            this.txtCondition1.TabIndex = 15;
            this.txtCondition1.TextChanged += new System.EventHandler(this.txtCondition1_TextChanged);
            // 
            // lblSuffixCondition1
            // 
            this.lblSuffixCondition1.AutoSize = true;
            this.lblSuffixCondition1.Location = new System.Drawing.Point(6, 55);
            this.lblSuffixCondition1.Name = "lblSuffixCondition1";
            this.lblSuffixCondition1.Size = new System.Drawing.Size(87, 15);
            this.lblSuffixCondition1.TabIndex = 14;
            this.lblSuffixCondition1.Text = "If filename has:";
            // 
            // chkExportData
            // 
            this.chkExportData.AutoSize = true;
            this.chkExportData.Checked = true;
            this.chkExportData.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkExportData.Location = new System.Drawing.Point(19, 526);
            this.chkExportData.Name = "chkExportData";
            this.chkExportData.Size = new System.Drawing.Size(149, 19);
            this.chkExportData.TabIndex = 32;
            this.chkExportData.Text = "Export with JSON Data?";
            this.chkExportData.UseVisualStyleBackColor = true;
            this.chkExportData.CheckedChanged += new System.EventHandler(this.chkExportData_CheckedChanged);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(812, 519);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(100, 30);
            this.btnExport.TabIndex = 33;
            this.btnExport.Text = "Export!";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // chkOriginalFilename
            // 
            this.chkOriginalFilename.AutoSize = true;
            this.chkOriginalFilename.Location = new System.Drawing.Point(174, 526);
            this.chkOriginalFilename.Name = "chkOriginalFilename";
            this.chkOriginalFilename.Size = new System.Drawing.Size(149, 19);
            this.chkOriginalFilename.TabIndex = 34;
            this.chkOriginalFilename.Text = "Keep original filename?";
            this.chkOriginalFilename.UseVisualStyleBackColor = true;
            this.chkOriginalFilename.CheckedChanged += new System.EventHandler(this.chkOriginalFilename_CheckedChanged_1);
            // 
            // btnSettings
            // 
            this.btnSettings.Location = new System.Drawing.Point(705, 519);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(100, 30);
            this.btnSettings.TabIndex = 35;
            this.btnSettings.Text = "Save Settings";
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // btnExportDebug
            // 
            this.btnExportDebug.Location = new System.Drawing.Point(559, 519);
            this.btnExportDebug.Name = "btnExportDebug";
            this.btnExportDebug.Size = new System.Drawing.Size(140, 30);
            this.btnExportDebug.TabIndex = 36;
            this.btnExportDebug.Text = "Show Export Debug";
            this.btnExportDebug.UseVisualStyleBackColor = true;
            this.btnExportDebug.Click += new System.EventHandler(this.btnExportDebug_Click);
            // 
            // chkEveryLayer
            // 
            this.chkEveryLayer.AutoSize = true;
            this.chkEveryLayer.Location = new System.Drawing.Point(329, 526);
            this.chkEveryLayer.Name = "chkEveryLayer";
            this.chkEveryLayer.Size = new System.Drawing.Size(87, 19);
            this.chkEveryLayer.TabIndex = 37;
            this.chkEveryLayer.Text = "Every layer?";
            this.chkEveryLayer.UseVisualStyleBackColor = true;
            this.chkEveryLayer.CheckedChanged += new System.EventHandler(this.chkEveryLayer_CheckedChanged);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(924, 561);
            this.Controls.Add(this.chkEveryLayer);
            this.Controls.Add(this.btnExportDebug);
            this.Controls.Add(this.btnSettings);
            this.Controls.Add(this.chkOriginalFilename);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.chkExportData);
            this.Controls.Add(this.grpSuffixList);
            this.Controls.Add(this.txtWarning);
            this.Controls.Add(this.txtLayerList);
            this.Controls.Add(this.lblLayerList);
            this.Controls.Add(this.lstFileList);
            this.Controls.Add(this.lblFileList);
            this.Controls.Add(this.btnFolderSearch);
            this.Controls.Add(this.lblFolderSearch);
            this.Controls.Add(this.txtFolderSearch);
            this.Controls.Add(this.btnAsepriteSearch);
            this.Controls.Add(this.lblAsepriteSearch);
            this.Controls.Add(this.txtAsepriteSearch);
            this.Name = "Main";
            this.Text = "Aseprite Multiple Export";
            this.Load += new System.EventHandler(this.Main_Load);
            this.grpSuffixList.ResumeLayout(false);
            this.grpSuffixList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDefaultColumns)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudColumns3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudColumns2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudColumns1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox txtAsepriteSearch;
        private Label lblAsepriteSearch;
        private Button btnAsepriteSearch;
        private Button btnFolderSearch;
        private Label lblFolderSearch;
        private TextBox txtFolderSearch;
        private Label lblFileList;
        private ListBox lstFileList;
        private Label lblLayerList;
        private TextBox txtLayerList;
        private Label txtWarning;
        private GroupBox grpSuffixList;
        private TextBox txtDefaultOutputName;
        private Label lblDefaultOutputName;
        private TextBox txtSuffix3;
        private Label lblSuffixCondition8;
        private TextBox txtCondition3;
        private Label lblSuffixCondition7;
        private TextBox txtSuffix2;
        private Label lblSuffixCondition5;
        private TextBox txtCondition2;
        private Label lblSuffixCondition4;
        private TextBox txtSuffix1;
        private Label lblSuffixCondition2;
        private TextBox txtCondition1;
        private Label lblSuffixCondition1;
        private Label lblSuffixCondition9;
        private Label lblSuffixCondition6;
        private Label lblSuffixCondition3;
        private CheckBox chkExportData;
        private Button btnExport;
        private NumericUpDown nudColumns3;
        private NumericUpDown nudColumns2;
        private NumericUpDown nudColumns1;
        private NumericUpDown nudDefaultColumns;
        private Label label1;
        private CheckBox chkOriginalFilename;
        private Button btnSettings;
        private NumericUpDown nudScale;
        private Label label2;
        private Button btnExportDebug;
        private CheckBox chkEveryLayer;
    }
}