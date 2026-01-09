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
            if ( disposing && (components != null) )
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
            components = new System.ComponentModel.Container();
            txtSearchFolder = new TextBox();
            lblSearchFolder = new Label();
            btnSearchFolder = new Button();
            lstFilelist = new ListBox();
            chkKeepChanges = new CheckBox();
            grpExportTypes = new GroupBox();
            rdoSpriteSheet = new RadioButton();
            rdoEveryFrame = new RadioButton();
            btnExport = new Button();
            grpExportOptions = new GroupBox();
            chkEveryLayer = new CheckBox();
            chkAllLayers = new CheckBox();
            toolTip = new ToolTip(components);
            lstLayerList = new ListBox();
            lblScale = new Label();
            nudScale = new NumericUpDown();
            lblFileList = new Label();
            lblLayerList = new Label();
            lstDebug = new ListBox();
            lblDebug = new Label();
            btnResetOutput = new Button();
            grpSpritesheetOptions = new GroupBox();
            lblSplit = new Label();
            nudSplit = new NumericUpDown();
            lblExportType = new Label();
            cmbSheetExportType = new ComboBox();
            chkExportJson = new CheckBox();
            lblOutputName = new Label();
            txtOutputName = new TextBox();
            nudFrameRangeMax = new NumericUpDown();
            grpFrameRangeOptions = new GroupBox();
            chkFrameRange = new CheckBox();
            lblFrameRangeMin = new Label();
            nudFrameRangeMin = new NumericUpDown();
            lblFrameRangeMax = new Label();
            btnResetFileListSelection = new Button();
            btnResetLayerListSelection = new Button();
            grpExportTypes.SuspendLayout();
            grpExportOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudScale).BeginInit();
            grpSpritesheetOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudSplit).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudFrameRangeMax).BeginInit();
            grpFrameRangeOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudFrameRangeMin).BeginInit();
            SuspendLayout();
            // 
            // txtSearchFolder
            // 
            txtSearchFolder.Enabled = false;
            txtSearchFolder.Location = new Point(146, 6);
            txtSearchFolder.Name = "txtSearchFolder";
            txtSearchFolder.Size = new Size(395, 23);
            txtSearchFolder.TabIndex = 1;
            // 
            // lblSearchFolder
            // 
            lblSearchFolder.AutoSize = true;
            lblSearchFolder.Location = new Point(12, 9);
            lblSearchFolder.Name = "lblSearchFolder";
            lblSearchFolder.Size = new Size(78, 15);
            lblSearchFolder.TabIndex = 0;
            lblSearchFolder.Text = "Search Folder";
            // 
            // btnSearchFolder
            // 
            btnSearchFolder.Location = new Point(547, 6);
            btnSearchFolder.Name = "btnSearchFolder";
            btnSearchFolder.Size = new Size(75, 23);
            btnSearchFolder.TabIndex = 2;
            btnSearchFolder.Text = "Search";
            btnSearchFolder.UseVisualStyleBackColor = true;
            btnSearchFolder.Click += BtnSearchFolder_Click;
            // 
            // lstFilelist
            // 
            lstFilelist.FormattingEnabled = true;
            lstFilelist.ItemHeight = 15;
            lstFilelist.Location = new Point(12, 104);
            lstFilelist.Name = "lstFilelist";
            lstFilelist.SelectionMode = SelectionMode.MultiExtended;
            lstFilelist.Size = new Size(449, 199);
            lstFilelist.TabIndex = 3;
            lstFilelist.SelectedIndexChanged += LstFilelist_SelectedIndexChanged;
            // 
            // chkKeepChanges
            // 
            chkKeepChanges.AutoSize = true;
            chkKeepChanges.Location = new Point(666, 9);
            chkKeepChanges.Name = "chkKeepChanges";
            chkKeepChanges.Size = new Size(106, 19);
            chkKeepChanges.TabIndex = 4;
            chkKeepChanges.Text = "Keep Changes?";
            toolTip.SetToolTip(chkKeepChanges, "Persist changes.");
            chkKeepChanges.UseVisualStyleBackColor = true;
            chkKeepChanges.CheckedChanged += BasicControl_Changed;
            // 
            // grpExportTypes
            // 
            grpExportTypes.Controls.Add(rdoSpriteSheet);
            grpExportTypes.Controls.Add(rdoEveryFrame);
            grpExportTypes.Location = new Point(12, 309);
            grpExportTypes.Name = "grpExportTypes";
            grpExportTypes.Size = new Size(449, 56);
            grpExportTypes.TabIndex = 5;
            grpExportTypes.TabStop = false;
            grpExportTypes.Text = "Export Types";
            // 
            // rdoSpriteSheet
            // 
            rdoSpriteSheet.AutoSize = true;
            rdoSpriteSheet.Location = new Point(101, 22);
            rdoSpriteSheet.Name = "rdoSpriteSheet";
            rdoSpriteSheet.Size = new Size(87, 19);
            rdoSpriteSheet.TabIndex = 1;
            rdoSpriteSheet.TabStop = true;
            rdoSpriteSheet.Text = "Sprite Sheet";
            toolTip.SetToolTip(rdoSpriteSheet, "Export the file as a spritesheet.");
            rdoSpriteSheet.UseVisualStyleBackColor = true;
            rdoSpriteSheet.CheckedChanged += BasicControl_Changed;
            // 
            // rdoEveryFrame
            // 
            rdoEveryFrame.AutoSize = true;
            rdoEveryFrame.Location = new Point(6, 22);
            rdoEveryFrame.Name = "rdoEveryFrame";
            rdoEveryFrame.Size = new Size(89, 19);
            rdoEveryFrame.TabIndex = 0;
            rdoEveryFrame.TabStop = true;
            rdoEveryFrame.Text = "Every Frame";
            toolTip.SetToolTip(rdoEveryFrame, "Export every frame of the selected files.");
            rdoEveryFrame.UseVisualStyleBackColor = true;
            rdoEveryFrame.CheckedChanged += BasicControl_Changed;
            // 
            // btnExport
            // 
            btnExport.Location = new Point(593, 592);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(179, 25);
            btnExport.TabIndex = 6;
            btnExport.Text = "Export!";
            btnExport.UseVisualStyleBackColor = true;
            btnExport.Click += BtnExport_Click;
            // 
            // grpExportOptions
            // 
            grpExportOptions.Controls.Add(chkEveryLayer);
            grpExportOptions.Controls.Add(chkAllLayers);
            grpExportOptions.Location = new Point(12, 371);
            grpExportOptions.Name = "grpExportOptions";
            grpExportOptions.Size = new Size(449, 50);
            grpExportOptions.TabIndex = 7;
            grpExportOptions.TabStop = false;
            grpExportOptions.Text = "Export Options";
            // 
            // chkEveryLayer
            // 
            chkEveryLayer.AutoSize = true;
            chkEveryLayer.Location = new Point(88, 22);
            chkEveryLayer.Name = "chkEveryLayer";
            chkEveryLayer.Size = new Size(85, 19);
            chkEveryLayer.TabIndex = 1;
            chkEveryLayer.Text = "Every Layer";
            toolTip.SetToolTip(chkEveryLayer, "Export each frame of each layer into a different file.");
            chkEveryLayer.UseVisualStyleBackColor = true;
            chkEveryLayer.CheckedChanged += BasicControl_Changed;
            // 
            // chkAllLayers
            // 
            chkAllLayers.AutoSize = true;
            chkAllLayers.Location = new Point(6, 22);
            chkAllLayers.Name = "chkAllLayers";
            chkAllLayers.Size = new Size(76, 19);
            chkAllLayers.TabIndex = 0;
            chkAllLayers.Text = "All Layers";
            toolTip.SetToolTip(chkAllLayers, "Force all layers to be visible.");
            chkAllLayers.UseVisualStyleBackColor = true;
            chkAllLayers.CheckedChanged += BasicControl_Changed;
            // 
            // lstLayerList
            // 
            lstLayerList.FormattingEnabled = true;
            lstLayerList.ItemHeight = 15;
            lstLayerList.Location = new Point(483, 104);
            lstLayerList.Name = "lstLayerList";
            lstLayerList.SelectionMode = SelectionMode.MultiExtended;
            lstLayerList.Size = new Size(289, 199);
            lstLayerList.TabIndex = 10;
            toolTip.SetToolTip(lstLayerList, "List of visible layers, if you see nothing then check \"All Layers\" and try again.");
            lstLayerList.SelectedIndexChanged += LstLayerList_SelectedIndexChanged;
            // 
            // lblScale
            // 
            lblScale.AutoSize = true;
            lblScale.Location = new Point(593, 562);
            lblScale.Name = "lblScale";
            lblScale.Size = new Size(37, 15);
            lblScale.TabIndex = 8;
            lblScale.Text = "Scale:";
            // 
            // nudScale
            // 
            nudScale.Location = new Point(636, 560);
            nudScale.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudScale.Name = "nudScale";
            nudScale.Size = new Size(136, 23);
            nudScale.TabIndex = 9;
            nudScale.Value = new decimal(new int[] { 1, 0, 0, 0 });
            nudScale.ValueChanged += BasicControl_Changed;
            // 
            // lblFileList
            // 
            lblFileList.AutoSize = true;
            lblFileList.Location = new Point(12, 78);
            lblFileList.Name = "lblFileList";
            lblFileList.Size = new Size(49, 15);
            lblFileList.TabIndex = 11;
            lblFileList.Text = "File List:";
            // 
            // lblLayerList
            // 
            lblLayerList.AutoSize = true;
            lblLayerList.Location = new Point(483, 78);
            lblLayerList.Name = "lblLayerList";
            lblLayerList.Size = new Size(59, 15);
            lblLayerList.TabIndex = 12;
            lblLayerList.Text = "Layer List:";
            // 
            // lstDebug
            // 
            lstDebug.FormattingEnabled = true;
            lstDebug.ItemHeight = 15;
            lstDebug.Location = new Point(12, 445);
            lstDebug.Name = "lstDebug";
            lstDebug.Size = new Size(575, 139);
            lstDebug.TabIndex = 13;
            // 
            // lblDebug
            // 
            lblDebug.AutoSize = true;
            lblDebug.Location = new Point(14, 425);
            lblDebug.Name = "lblDebug";
            lblDebug.Size = new Size(48, 15);
            lblDebug.TabIndex = 14;
            lblDebug.Text = "Output:";
            // 
            // btnResetOutput
            // 
            btnResetOutput.Location = new Point(12, 590);
            btnResetOutput.Name = "btnResetOutput";
            btnResetOutput.Size = new Size(107, 25);
            btnResetOutput.TabIndex = 15;
            btnResetOutput.Text = "Reset Output";
            btnResetOutput.UseVisualStyleBackColor = true;
            btnResetOutput.Click += BtnResetOutput_Click;
            // 
            // grpSpritesheetOptions
            // 
            grpSpritesheetOptions.Controls.Add(lblSplit);
            grpSpritesheetOptions.Controls.Add(nudSplit);
            grpSpritesheetOptions.Controls.Add(lblExportType);
            grpSpritesheetOptions.Controls.Add(cmbSheetExportType);
            grpSpritesheetOptions.Controls.Add(chkExportJson);
            grpSpritesheetOptions.Location = new Point(483, 309);
            grpSpritesheetOptions.Name = "grpSpritesheetOptions";
            grpSpritesheetOptions.Size = new Size(289, 112);
            grpSpritesheetOptions.TabIndex = 8;
            grpSpritesheetOptions.TabStop = false;
            grpSpritesheetOptions.Text = "Spritesheet Options";
            // 
            // lblSplit
            // 
            lblSplit.AutoSize = true;
            lblSplit.Location = new Point(154, 24);
            lblSplit.Name = "lblSplit";
            lblSplit.Size = new Size(100, 15);
            lblSplit.TabIndex = 18;
            lblSplit.Text = "lblSplit - see code";
            // 
            // nudSplit
            // 
            nudSplit.Location = new Point(154, 43);
            nudSplit.Name = "nudSplit";
            nudSplit.Size = new Size(120, 23);
            nudSplit.TabIndex = 17;
            nudSplit.ValueChanged += BasicControl_Changed;
            // 
            // lblExportType
            // 
            lblExportType.AutoSize = true;
            lblExportType.Location = new Point(11, 24);
            lblExportType.Name = "lblExportType";
            lblExportType.Size = new Size(71, 15);
            lblExportType.TabIndex = 16;
            lblExportType.Text = "Export Type:";
            // 
            // cmbSheetExportType
            // 
            cmbSheetExportType.FormattingEnabled = true;
            cmbSheetExportType.Location = new Point(11, 43);
            cmbSheetExportType.Name = "cmbSheetExportType";
            cmbSheetExportType.Size = new Size(121, 23);
            cmbSheetExportType.TabIndex = 1;
            cmbSheetExportType.SelectedIndexChanged += BasicControl_Changed;
            // 
            // chkExportJson
            // 
            chkExportJson.AutoSize = true;
            chkExportJson.Location = new Point(11, 79);
            chkExportJson.Name = "chkExportJson";
            chkExportJson.Size = new Size(86, 19);
            chkExportJson.TabIndex = 0;
            chkExportJson.Text = "Export Json";
            chkExportJson.UseVisualStyleBackColor = true;
            chkExportJson.CheckedChanged += BasicControl_Changed;
            // 
            // lblOutputName
            // 
            lblOutputName.AutoSize = true;
            lblOutputName.Location = new Point(12, 38);
            lblOutputName.Name = "lblOutputName";
            lblOutputName.Size = new Size(83, 15);
            lblOutputName.TabIndex = 16;
            lblOutputName.Text = "Output Name:";
            // 
            // txtOutputName
            // 
            txtOutputName.Location = new Point(146, 35);
            txtOutputName.Name = "txtOutputName";
            txtOutputName.Size = new Size(395, 23);
            txtOutputName.TabIndex = 17;
            txtOutputName.TextChanged += BasicControl_Changed;
            // 
            // nudFrameRangeMax
            // 
            nudFrameRangeMax.Location = new Point(45, 81);
            nudFrameRangeMax.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudFrameRangeMax.Name = "nudFrameRangeMax";
            nudFrameRangeMax.Size = new Size(128, 23);
            nudFrameRangeMax.TabIndex = 18;
            nudFrameRangeMax.Value = new decimal(new int[] { 1, 0, 0, 0 });
            nudFrameRangeMax.ValueChanged += BasicControl_Changed;
            // 
            // grpFrameRangeOptions
            // 
            grpFrameRangeOptions.Controls.Add(chkFrameRange);
            grpFrameRangeOptions.Controls.Add(lblFrameRangeMin);
            grpFrameRangeOptions.Controls.Add(nudFrameRangeMin);
            grpFrameRangeOptions.Controls.Add(lblFrameRangeMax);
            grpFrameRangeOptions.Controls.Add(nudFrameRangeMax);
            grpFrameRangeOptions.Location = new Point(593, 437);
            grpFrameRangeOptions.Name = "grpFrameRangeOptions";
            grpFrameRangeOptions.Size = new Size(179, 114);
            grpFrameRangeOptions.TabIndex = 19;
            grpFrameRangeOptions.TabStop = false;
            grpFrameRangeOptions.Text = "Frame Range Options";
            // 
            // chkFrameRange
            // 
            chkFrameRange.AutoSize = true;
            chkFrameRange.Location = new Point(9, 22);
            chkFrameRange.Name = "chkFrameRange";
            chkFrameRange.Size = new Size(133, 19);
            chkFrameRange.TabIndex = 19;
            chkFrameRange.Text = "Enable Frame Range";
            chkFrameRange.UseVisualStyleBackColor = true;
            chkFrameRange.CheckedChanged += BasicControl_Changed;
            // 
            // lblFrameRangeMin
            // 
            lblFrameRangeMin.AutoSize = true;
            lblFrameRangeMin.Location = new Point(6, 54);
            lblFrameRangeMin.Name = "lblFrameRangeMin";
            lblFrameRangeMin.Size = new Size(31, 15);
            lblFrameRangeMin.TabIndex = 22;
            lblFrameRangeMin.Text = "Min:";
            // 
            // nudFrameRangeMin
            // 
            nudFrameRangeMin.Location = new Point(45, 52);
            nudFrameRangeMin.Name = "nudFrameRangeMin";
            nudFrameRangeMin.Size = new Size(128, 23);
            nudFrameRangeMin.TabIndex = 21;
            nudFrameRangeMin.ValueChanged += BasicControl_Changed;
            // 
            // lblFrameRangeMax
            // 
            lblFrameRangeMax.AutoSize = true;
            lblFrameRangeMax.Location = new Point(6, 83);
            lblFrameRangeMax.Name = "lblFrameRangeMax";
            lblFrameRangeMax.Size = new Size(33, 15);
            lblFrameRangeMax.TabIndex = 20;
            lblFrameRangeMax.Text = "Max:";
            // 
            // btnResetFileListSelection
            // 
            btnResetFileListSelection.Location = new Point(311, 73);
            btnResetFileListSelection.Name = "btnResetFileListSelection";
            btnResetFileListSelection.Size = new Size(150, 25);
            btnResetFileListSelection.TabIndex = 20;
            btnResetFileListSelection.Text = "Reset File List Selection";
            btnResetFileListSelection.UseVisualStyleBackColor = true;
            btnResetFileListSelection.Click += BtnResetFileListSelection_Click;
            // 
            // btnResetLayerListSelection
            // 
            btnResetLayerListSelection.Location = new Point(622, 73);
            btnResetLayerListSelection.Name = "btnResetLayerListSelection";
            btnResetLayerListSelection.Size = new Size(150, 25);
            btnResetLayerListSelection.TabIndex = 21;
            btnResetLayerListSelection.Text = "Reset Layer List Selection";
            btnResetLayerListSelection.UseVisualStyleBackColor = true;
            btnResetLayerListSelection.Click += BtnResetLayerListSelection_Click;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 629);
            Controls.Add(btnResetLayerListSelection);
            Controls.Add(btnResetFileListSelection);
            Controls.Add(grpFrameRangeOptions);
            Controls.Add(txtOutputName);
            Controls.Add(lblOutputName);
            Controls.Add(grpSpritesheetOptions);
            Controls.Add(btnResetOutput);
            Controls.Add(lblDebug);
            Controls.Add(lstDebug);
            Controls.Add(lblLayerList);
            Controls.Add(lblFileList);
            Controls.Add(lstLayerList);
            Controls.Add(nudScale);
            Controls.Add(lblScale);
            Controls.Add(grpExportOptions);
            Controls.Add(btnExport);
            Controls.Add(grpExportTypes);
            Controls.Add(chkKeepChanges);
            Controls.Add(lstFilelist);
            Controls.Add(btnSearchFolder);
            Controls.Add(lblSearchFolder);
            Controls.Add(txtSearchFolder);
            MaximizeBox = false;
            MinimumSize = new Size(800, 600);
            Name = "Main";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Aseprite Multiple Export";
            Load += Main_Load;
            grpExportTypes.ResumeLayout(false);
            grpExportTypes.PerformLayout();
            grpExportOptions.ResumeLayout(false);
            grpExportOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudScale).EndInit();
            grpSpritesheetOptions.ResumeLayout(false);
            grpSpritesheetOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudSplit).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudFrameRangeMax).EndInit();
            grpFrameRangeOptions.ResumeLayout(false);
            grpFrameRangeOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudFrameRangeMin).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtSearchFolder;
        private Label lblSearchFolder;
        private Button btnSearchFolder;
        private ListBox lstFilelist;
        private CheckBox chkKeepChanges;
        private GroupBox grpExportTypes;
        private RadioButton rdoSpriteSheet;
        private RadioButton rdoEveryFrame;
        private Button btnExport;
        private GroupBox grpExportOptions;
        private CheckBox chkAllLayers;
        private CheckBox chkEveryLayer;
        private ToolTip toolTip;
        private Label lblScale;
        private NumericUpDown nudScale;
        private ListBox lstLayerList;
        private Label lblFileList;
        private Label lblLayerList;
        private ListBox lstDebug;
        private Label lblDebug;
        private Button btnResetOutput;
        private GroupBox grpSpritesheetOptions;
        private CheckBox chkExportJson;
        private Label lblSplit;
        private NumericUpDown nudSplit;
        private Label lblExportType;
        private ComboBox cmbSheetExportType;
        private Label lblOutputName;
        private TextBox txtOutputName;
        private NumericUpDown nudFrameRangeMax;
        private GroupBox grpFrameRangeOptions;
        private Label lblFrameRangeMax;
        private CheckBox chkFrameRange;
        private Label lblFrameRangeMin;
        private NumericUpDown nudFrameRangeMin;
        private Button btnResetFileListSelection;
        private Button btnResetLayerListSelection;
    }
}
