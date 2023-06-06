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
            txtDefaultOutputName = new TextBox();
            lblDefaultOutputName = new Label();
            chkExportData = new CheckBox();
            btnExport = new Button();
            chkOriginalFilename = new CheckBox();
            btnSettings = new Button();
            chkEveryLayer = new CheckBox();
            grpOptions = new GroupBox();
            grpExportedItems = new GroupBox();
            lstExportedItems = new ListBox();
            chkExportLayers = new CheckBox();
            chkExportTags = new CheckBox();
            btnRemoveFrames = new Button();
            grpSuffixList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) nudScale).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudDefaultColumns).BeginInit();
            grpOptions.SuspendLayout();
            grpExportedItems.SuspendLayout();
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
            grpSuffixList.Controls.Add(txtDefaultOutputName);
            grpSuffixList.Controls.Add(lblDefaultOutputName);
            grpSuffixList.Location = new Point(12, 365);
            grpSuffixList.Name = "grpSuffixList";
            grpSuffixList.Size = new Size(760, 54);
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
            grpOptions.Controls.Add(grpExportedItems);
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
            // grpExportedItems
            // 
            grpExportedItems.Controls.Add(lstExportedItems);
            grpExportedItems.Location = new Point(6, 257);
            grpExportedItems.Name = "grpExportedItems";
            grpExportedItems.Size = new Size(177, 280);
            grpExportedItems.TabIndex = 40;
            grpExportedItems.TabStop = false;
            grpExportedItems.Text = "Exported Items";
            // 
            // lstExportedItems
            // 
            lstExportedItems.FormattingEnabled = true;
            lstExportedItems.ItemHeight = 15;
            lstExportedItems.Location = new Point(6, 28);
            lstExportedItems.Name = "lstExportedItems";
            lstExportedItems.Size = new Size(165, 244);
            lstExportedItems.TabIndex = 10;
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
            // btnRemoveFrames
            // 
            btnRemoveFrames.Location = new Point(459, 519);
            btnRemoveFrames.Name = "btnRemoveFrames";
            btnRemoveFrames.Size = new Size(100, 30);
            btnRemoveFrames.TabIndex = 41;
            btnRemoveFrames.Text = "Remove Frames";
            btnRemoveFrames.UseVisualStyleBackColor = true;
            btnRemoveFrames.Click += btnRemoveFrames_Click;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(971, 561);
            Controls.Add(btnRemoveFrames);
            Controls.Add(grpOptions);
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
            grpOptions.ResumeLayout(false);
            grpOptions.PerformLayout();
            grpExportedItems.ResumeLayout(false);
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
        private CheckBox chkExportData;
        private Button btnExport;
        private NumericUpDown nudDefaultColumns;
        private Label label1;
        private CheckBox chkOriginalFilename;
        private Button btnSettings;
        private NumericUpDown nudScale;
        private Label label2;
        private CheckBox chkEveryLayer;
        private GroupBox grpOptions;
        private CheckBox chkExportLayers;
        private CheckBox chkExportTags;
        private GroupBox grpExportedItems;
        private ListBox lstExportedItems;
        private Button btnRemoveFrames;
    }
}