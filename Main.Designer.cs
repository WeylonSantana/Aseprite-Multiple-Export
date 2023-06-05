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
            txtAsepriteSearch = new TextBox();
            lblAsepriteSearch = new Label();
            btnAsepriteSearch = new Button();
            btnFolderSearch = new Button();
            lblFolderSearch = new Label();
            txtFolderSearch = new TextBox();
            lblFileList = new Label();
            lstFileList = new ListBox();
            lblLayerList = new Label();
            txtLayerList = new TextBox();
            txtWarning = new Label();
            grpSuffixList = new GroupBox();
            nudScale = new NumericUpDown();
            label2 = new Label();
            nudDefaultColumns = new NumericUpDown();
            label1 = new Label();
            nudColumns3 = new NumericUpDown();
            nudColumns2 = new NumericUpDown();
            nudColumns1 = new NumericUpDown();
            lblSuffixCondition9 = new Label();
            lblSuffixCondition6 = new Label();
            lblSuffixCondition3 = new Label();
            txtDefaultOutputName = new TextBox();
            lblDefaultOutputName = new Label();
            txtSuffix3 = new TextBox();
            lblSuffixCondition8 = new Label();
            txtCondition3 = new TextBox();
            lblSuffixCondition7 = new Label();
            txtSuffix2 = new TextBox();
            lblSuffixCondition5 = new Label();
            txtCondition2 = new TextBox();
            lblSuffixCondition4 = new Label();
            txtSuffix1 = new TextBox();
            lblSuffixCondition2 = new Label();
            txtCondition1 = new TextBox();
            lblSuffixCondition1 = new Label();
            chkExportData = new CheckBox();
            btnExport = new Button();
            chkOriginalFilename = new CheckBox();
            btnSettings = new Button();
            btnExportDebug = new Button();
            chkEveryLayer = new CheckBox();
            grpOptions = new GroupBox();
            chkExportLayers = new CheckBox();
            chkExportTags = new CheckBox();
            grpSuffixList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) nudScale).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudDefaultColumns).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudColumns3).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudColumns2).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudColumns1).BeginInit();
            grpOptions.SuspendLayout();
            SuspendLayout();
            // 
            // txtAsepriteSearch
            // 
            txtAsepriteSearch.Enabled = false;
            txtAsepriteSearch.Location = new Point(156, 6);
            txtAsepriteSearch.Name = "txtAsepriteSearch";
            txtAsepriteSearch.Size = new Size(510, 23);
            txtAsepriteSearch.TabIndex = 1;
            // 
            // lblAsepriteSearch
            // 
            lblAsepriteSearch.AutoSize = true;
            lblAsepriteSearch.Location = new Point(12, 9);
            lblAsepriteSearch.Name = "lblAsepriteSearch";
            lblAsepriteSearch.Size = new Size(138, 15);
            lblAsepriteSearch.TabIndex = 0;
            lblAsepriteSearch.Text = "Select aseprite \".exe\" file:";
            // 
            // btnAsepriteSearch
            // 
            btnAsepriteSearch.Location = new Point(672, 6);
            btnAsepriteSearch.Name = "btnAsepriteSearch";
            btnAsepriteSearch.Size = new Size(100, 23);
            btnAsepriteSearch.TabIndex = 2;
            btnAsepriteSearch.Text = "Search Aseprite";
            btnAsepriteSearch.UseVisualStyleBackColor = true;
            btnAsepriteSearch.Click += btnAsepriteSearch_Click;
            // 
            // btnFolderSearch
            // 
            btnFolderSearch.Location = new Point(672, 35);
            btnFolderSearch.Name = "btnFolderSearch";
            btnFolderSearch.Size = new Size(100, 23);
            btnFolderSearch.TabIndex = 5;
            btnFolderSearch.Text = "Search Folder";
            btnFolderSearch.UseVisualStyleBackColor = true;
            btnFolderSearch.Click += btnFolderSearch_Click;
            // 
            // lblFolderSearch
            // 
            lblFolderSearch.AutoSize = true;
            lblFolderSearch.Location = new Point(12, 38);
            lblFolderSearch.Name = "lblFolderSearch";
            lblFolderSearch.Size = new Size(213, 15);
            lblFolderSearch.TabIndex = 3;
            lblFolderSearch.Text = "Select folder with \".ase / .aseprite\" files:";
            // 
            // txtFolderSearch
            // 
            txtFolderSearch.Enabled = false;
            txtFolderSearch.Location = new Point(231, 35);
            txtFolderSearch.Name = "txtFolderSearch";
            txtFolderSearch.Size = new Size(435, 23);
            txtFolderSearch.TabIndex = 4;
            // 
            // lblFileList
            // 
            lblFileList.AutoSize = true;
            lblFileList.Location = new Point(12, 60);
            lblFileList.Name = "lblFileList";
            lblFileList.Size = new Size(66, 15);
            lblFileList.TabIndex = 6;
            lblFileList.Text = "List of files:";
            // 
            // lstFileList
            // 
            lstFileList.FormattingEnabled = true;
            lstFileList.ItemHeight = 15;
            lstFileList.Location = new Point(12, 79);
            lstFileList.Name = "lstFileList";
            lstFileList.Size = new Size(760, 229);
            lstFileList.TabIndex = 7;
            // 
            // lblLayerList
            // 
            lblLayerList.AutoSize = true;
            lblLayerList.Location = new Point(12, 317);
            lblLayerList.Name = "lblLayerList";
            lblLayerList.Size = new Size(304, 15);
            lblLayerList.TabIndex = 8;
            lblLayerList.Text = "Comma separated layer list (e.g: helmet,armor,gloves...):";
            // 
            // txtLayerList
            // 
            txtLayerList.Location = new Point(322, 314);
            txtLayerList.Name = "txtLayerList";
            txtLayerList.Size = new Size(450, 23);
            txtLayerList.TabIndex = 9;
            txtLayerList.TextChanged += txtLayerList_TextChanged;
            // 
            // txtWarning
            // 
            txtWarning.AutoSize = true;
            txtWarning.Location = new Point(12, 341);
            txtWarning.Name = "txtWarning";
            txtWarning.Size = new Size(661, 15);
            txtWarning.TabIndex = 10;
            txtWarning.Text = "Words with accents or special characters are not supported! if empty, only one image will be exported with all layers at once.";
            // 
            // grpSuffixList
            // 
            grpSuffixList.Controls.Add(nudScale);
            grpSuffixList.Controls.Add(label2);
            grpSuffixList.Controls.Add(nudDefaultColumns);
            grpSuffixList.Controls.Add(label1);
            grpSuffixList.Controls.Add(nudColumns3);
            grpSuffixList.Controls.Add(nudColumns2);
            grpSuffixList.Controls.Add(nudColumns1);
            grpSuffixList.Controls.Add(lblSuffixCondition9);
            grpSuffixList.Controls.Add(lblSuffixCondition6);
            grpSuffixList.Controls.Add(lblSuffixCondition3);
            grpSuffixList.Controls.Add(txtDefaultOutputName);
            grpSuffixList.Controls.Add(lblDefaultOutputName);
            grpSuffixList.Controls.Add(txtSuffix3);
            grpSuffixList.Controls.Add(lblSuffixCondition8);
            grpSuffixList.Controls.Add(txtCondition3);
            grpSuffixList.Controls.Add(lblSuffixCondition7);
            grpSuffixList.Controls.Add(txtSuffix2);
            grpSuffixList.Controls.Add(lblSuffixCondition5);
            grpSuffixList.Controls.Add(txtCondition2);
            grpSuffixList.Controls.Add(lblSuffixCondition4);
            grpSuffixList.Controls.Add(txtSuffix1);
            grpSuffixList.Controls.Add(lblSuffixCondition2);
            grpSuffixList.Controls.Add(txtCondition1);
            grpSuffixList.Controls.Add(lblSuffixCondition1);
            grpSuffixList.Location = new Point(12, 365);
            grpSuffixList.Name = "grpSuffixList";
            grpSuffixList.Size = new Size(760, 143);
            grpSuffixList.TabIndex = 11;
            grpSuffixList.TabStop = false;
            grpSuffixList.Text = "Output Filename Control";
            // 
            // nudScale
            // 
            nudScale.Location = new Point(645, 21);
            nudScale.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudScale.Name = "nudScale";
            nudScale.Size = new Size(109, 23);
            nudScale.TabIndex = 35;
            nudScale.Value = new decimal(new int[] { 1, 0, 0, 0 });
            nudScale.ValueChanged += nudScale_ValueChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(561, 25);
            label2.Name = "label2";
            label2.Size = new Size(78, 15);
            label2.TabIndex = 34;
            label2.Text = "Default Scale:";
            // 
            // nudDefaultColumns
            // 
            nudDefaultColumns.Location = new Point(433, 22);
            nudDefaultColumns.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudDefaultColumns.Name = "nudDefaultColumns";
            nudDefaultColumns.Size = new Size(114, 23);
            nudDefaultColumns.TabIndex = 33;
            nudDefaultColumns.Value = new decimal(new int[] { 4, 0, 0, 0 });
            nudDefaultColumns.ValueChanged += nudDefaultColumns_ValueChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(287, 25);
            label1.Name = "label1";
            label1.Size = new Size(140, 15);
            label1.TabIndex = 32;
            label1.Text = "Default Output Columns:";
            // 
            // nudColumns3
            // 
            nudColumns3.Location = new Point(512, 110);
            nudColumns3.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudColumns3.Name = "nudColumns3";
            nudColumns3.Size = new Size(155, 23);
            nudColumns3.TabIndex = 31;
            nudColumns3.Value = new decimal(new int[] { 1, 0, 0, 0 });
            nudColumns3.ValueChanged += nudColumns3_ValueChanged;
            // 
            // nudColumns2
            // 
            nudColumns2.Location = new Point(512, 81);
            nudColumns2.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudColumns2.Name = "nudColumns2";
            nudColumns2.Size = new Size(155, 23);
            nudColumns2.TabIndex = 25;
            nudColumns2.Value = new decimal(new int[] { 1, 0, 0, 0 });
            nudColumns2.ValueChanged += nudColumns2_ValueChanged;
            // 
            // nudColumns1
            // 
            nudColumns1.Location = new Point(512, 52);
            nudColumns1.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudColumns1.Name = "nudColumns1";
            nudColumns1.Size = new Size(155, 23);
            nudColumns1.TabIndex = 19;
            nudColumns1.Value = new decimal(new int[] { 1, 0, 0, 0 });
            nudColumns1.ValueChanged += nudColumns1_ValueChanged;
            // 
            // lblSuffixCondition9
            // 
            lblSuffixCondition9.AutoSize = true;
            lblSuffixCondition9.Location = new Point(407, 113);
            lblSuffixCondition9.Name = "lblSuffixCondition9";
            lblSuffixCondition9.Size = new Size(99, 15);
            lblSuffixCondition9.TabIndex = 30;
            lblSuffixCondition9.Text = "and set Columns:";
            // 
            // lblSuffixCondition6
            // 
            lblSuffixCondition6.AutoSize = true;
            lblSuffixCondition6.Location = new Point(407, 84);
            lblSuffixCondition6.Name = "lblSuffixCondition6";
            lblSuffixCondition6.Size = new Size(99, 15);
            lblSuffixCondition6.TabIndex = 24;
            lblSuffixCondition6.Text = "and set Columns:";
            // 
            // lblSuffixCondition3
            // 
            lblSuffixCondition3.AutoSize = true;
            lblSuffixCondition3.Location = new Point(407, 55);
            lblSuffixCondition3.Name = "lblSuffixCondition3";
            lblSuffixCondition3.Size = new Size(99, 15);
            lblSuffixCondition3.TabIndex = 18;
            lblSuffixCondition3.Text = "and set Columns:";
            // 
            // txtDefaultOutputName
            // 
            txtDefaultOutputName.Location = new Point(152, 22);
            txtDefaultOutputName.Name = "txtDefaultOutputName";
            txtDefaultOutputName.Size = new Size(129, 23);
            txtDefaultOutputName.TabIndex = 13;
            txtDefaultOutputName.TextChanged += txtDefaultOutputName_TextChanged;
            // 
            // lblDefaultOutputName
            // 
            lblDefaultOutputName.AutoSize = true;
            lblDefaultOutputName.Location = new Point(6, 25);
            lblDefaultOutputName.Name = "lblDefaultOutputName";
            lblDefaultOutputName.Size = new Size(140, 15);
            lblDefaultOutputName.TabIndex = 12;
            lblDefaultOutputName.Text = "Default Output Filename:";
            // 
            // txtSuffix3
            // 
            txtSuffix3.Location = new Point(287, 110);
            txtSuffix3.Name = "txtSuffix3";
            txtSuffix3.Size = new Size(114, 23);
            txtSuffix3.TabIndex = 29;
            txtSuffix3.TextChanged += txtSuffix3_TextChanged;
            // 
            // lblSuffixCondition8
            // 
            lblSuffixCondition8.AutoSize = true;
            lblSuffixCondition8.Location = new Point(219, 113);
            lblSuffixCondition8.Name = "lblSuffixCondition8";
            lblSuffixCondition8.Size = new Size(62, 15);
            lblSuffixCondition8.TabIndex = 28;
            lblSuffixCondition8.Text = "add suffix:";
            // 
            // txtCondition3
            // 
            txtCondition3.Location = new Point(99, 110);
            txtCondition3.Name = "txtCondition3";
            txtCondition3.Size = new Size(114, 23);
            txtCondition3.TabIndex = 27;
            txtCondition3.TextChanged += txtCondition3_TextChanged;
            // 
            // lblSuffixCondition7
            // 
            lblSuffixCondition7.AutoSize = true;
            lblSuffixCondition7.Location = new Point(6, 113);
            lblSuffixCondition7.Name = "lblSuffixCondition7";
            lblSuffixCondition7.Size = new Size(87, 15);
            lblSuffixCondition7.TabIndex = 26;
            lblSuffixCondition7.Text = "If filename has:";
            // 
            // txtSuffix2
            // 
            txtSuffix2.Location = new Point(287, 81);
            txtSuffix2.Name = "txtSuffix2";
            txtSuffix2.Size = new Size(114, 23);
            txtSuffix2.TabIndex = 23;
            txtSuffix2.TextChanged += txtSuffix2_TextChanged;
            // 
            // lblSuffixCondition5
            // 
            lblSuffixCondition5.AutoSize = true;
            lblSuffixCondition5.Location = new Point(219, 84);
            lblSuffixCondition5.Name = "lblSuffixCondition5";
            lblSuffixCondition5.Size = new Size(62, 15);
            lblSuffixCondition5.TabIndex = 22;
            lblSuffixCondition5.Text = "add suffix:";
            // 
            // txtCondition2
            // 
            txtCondition2.Location = new Point(99, 81);
            txtCondition2.Name = "txtCondition2";
            txtCondition2.Size = new Size(114, 23);
            txtCondition2.TabIndex = 21;
            txtCondition2.TextChanged += txtCondition2_TextChanged;
            // 
            // lblSuffixCondition4
            // 
            lblSuffixCondition4.AutoSize = true;
            lblSuffixCondition4.Location = new Point(6, 84);
            lblSuffixCondition4.Name = "lblSuffixCondition4";
            lblSuffixCondition4.Size = new Size(87, 15);
            lblSuffixCondition4.TabIndex = 20;
            lblSuffixCondition4.Text = "If filename has:";
            // 
            // txtSuffix1
            // 
            txtSuffix1.Location = new Point(287, 52);
            txtSuffix1.Name = "txtSuffix1";
            txtSuffix1.Size = new Size(114, 23);
            txtSuffix1.TabIndex = 17;
            txtSuffix1.TextChanged += txtSuffix1_TextChanged;
            // 
            // lblSuffixCondition2
            // 
            lblSuffixCondition2.AutoSize = true;
            lblSuffixCondition2.Location = new Point(219, 55);
            lblSuffixCondition2.Name = "lblSuffixCondition2";
            lblSuffixCondition2.Size = new Size(62, 15);
            lblSuffixCondition2.TabIndex = 16;
            lblSuffixCondition2.Text = "add suffix:";
            // 
            // txtCondition1
            // 
            txtCondition1.Location = new Point(99, 52);
            txtCondition1.Name = "txtCondition1";
            txtCondition1.Size = new Size(114, 23);
            txtCondition1.TabIndex = 15;
            txtCondition1.TextChanged += txtCondition1_TextChanged;
            // 
            // lblSuffixCondition1
            // 
            lblSuffixCondition1.AutoSize = true;
            lblSuffixCondition1.Location = new Point(6, 55);
            lblSuffixCondition1.Name = "lblSuffixCondition1";
            lblSuffixCondition1.Size = new Size(87, 15);
            lblSuffixCondition1.TabIndex = 14;
            lblSuffixCondition1.Text = "If filename has:";
            // 
            // chkExportData
            // 
            chkExportData.AutoSize = true;
            chkExportData.Checked = true;
            chkExportData.CheckState = CheckState.Checked;
            chkExportData.Location = new Point(6, 72);
            chkExportData.Name = "chkExportData";
            chkExportData.Size = new Size(149, 19);
            chkExportData.TabIndex = 32;
            chkExportData.Text = "Export with JSON Data?";
            chkExportData.UseVisualStyleBackColor = true;
            chkExportData.CheckedChanged += chkExportData_CheckedChanged;
            // 
            // btnExport
            // 
            btnExport.Location = new Point(672, 519);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(100, 30);
            btnExport.TabIndex = 33;
            btnExport.Text = "Export!";
            btnExport.UseVisualStyleBackColor = true;
            btnExport.Click += btnExport_Click;
            // 
            // chkOriginalFilename
            // 
            chkOriginalFilename.AutoSize = true;
            chkOriginalFilename.Location = new Point(6, 47);
            chkOriginalFilename.Name = "chkOriginalFilename";
            chkOriginalFilename.Size = new Size(149, 19);
            chkOriginalFilename.TabIndex = 34;
            chkOriginalFilename.Text = "Keep original filename?";
            chkOriginalFilename.UseVisualStyleBackColor = true;
            chkOriginalFilename.CheckedChanged += chkOriginalFilename_CheckedChanged_1;
            // 
            // btnSettings
            // 
            btnSettings.Location = new Point(565, 519);
            btnSettings.Name = "btnSettings";
            btnSettings.Size = new Size(100, 30);
            btnSettings.TabIndex = 35;
            btnSettings.Text = "Save Settings";
            btnSettings.UseVisualStyleBackColor = true;
            btnSettings.Click += btnSettings_Click;
            // 
            // btnExportDebug
            // 
            btnExportDebug.Location = new Point(419, 519);
            btnExportDebug.Name = "btnExportDebug";
            btnExportDebug.Size = new Size(140, 30);
            btnExportDebug.TabIndex = 36;
            btnExportDebug.Text = "Show Export Debug";
            btnExportDebug.UseVisualStyleBackColor = true;
            btnExportDebug.Click += btnExportDebug_Click;
            // 
            // chkEveryLayer
            // 
            chkEveryLayer.AutoSize = true;
            chkEveryLayer.Location = new Point(6, 22);
            chkEveryLayer.Name = "chkEveryLayer";
            chkEveryLayer.Size = new Size(87, 19);
            chkEveryLayer.TabIndex = 37;
            chkEveryLayer.Text = "Every layer?";
            chkEveryLayer.UseVisualStyleBackColor = true;
            chkEveryLayer.CheckedChanged += chkEveryLayer_CheckedChanged;
            // 
            // grpOptions
            // 
            grpOptions.Controls.Add(chkExportLayers);
            grpOptions.Controls.Add(chkExportTags);
            grpOptions.Controls.Add(chkExportData);
            grpOptions.Controls.Add(chkEveryLayer);
            grpOptions.Controls.Add(chkOriginalFilename);
            grpOptions.Location = new Point(778, 6);
            grpOptions.Name = "grpOptions";
            grpOptions.Size = new Size(190, 543);
            grpOptions.TabIndex = 38;
            grpOptions.TabStop = false;
            grpOptions.Text = "Extra Options";
            // 
            // chkExportLayers
            // 
            chkExportLayers.AutoSize = true;
            chkExportLayers.Checked = true;
            chkExportLayers.CheckState = CheckState.Checked;
            chkExportLayers.Location = new Point(6, 122);
            chkExportLayers.Name = "chkExportLayers";
            chkExportLayers.Size = new Size(177, 19);
            chkExportLayers.TabIndex = 39;
            chkExportLayers.Text = "Export with LAYERS in JSON?";
            chkExportLayers.UseVisualStyleBackColor = true;
            chkExportLayers.CheckedChanged += chkExportLayers_CheckedChanged;
            // 
            // chkExportTags
            // 
            chkExportTags.AutoSize = true;
            chkExportTags.Checked = true;
            chkExportTags.CheckState = CheckState.Checked;
            chkExportTags.Location = new Point(6, 97);
            chkExportTags.Name = "chkExportTags";
            chkExportTags.Size = new Size(165, 19);
            chkExportTags.TabIndex = 38;
            chkExportTags.Text = "Export with TAGS in JSON?";
            chkExportTags.UseVisualStyleBackColor = true;
            chkExportTags.CheckedChanged += chkExportTags_CheckedChanged;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(971, 561);
            Controls.Add(grpOptions);
            Controls.Add(btnExportDebug);
            Controls.Add(btnSettings);
            Controls.Add(btnExport);
            Controls.Add(grpSuffixList);
            Controls.Add(txtWarning);
            Controls.Add(txtLayerList);
            Controls.Add(lblLayerList);
            Controls.Add(lstFileList);
            Controls.Add(lblFileList);
            Controls.Add(btnFolderSearch);
            Controls.Add(lblFolderSearch);
            Controls.Add(txtFolderSearch);
            Controls.Add(btnAsepriteSearch);
            Controls.Add(lblAsepriteSearch);
            Controls.Add(txtAsepriteSearch);
            Name = "Main";
            Text = "Aseprite Multiple Export";
            Load += Main_Load;
            grpSuffixList.ResumeLayout(false);
            grpSuffixList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) nudScale).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudDefaultColumns).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudColumns3).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudColumns2).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudColumns1).EndInit();
            grpOptions.ResumeLayout(false);
            grpOptions.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
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
        private GroupBox grpOptions;
        private CheckBox chkExportLayers;
        private CheckBox chkExportTags;
    }
}