﻿namespace Aseprite_Multiple_Export
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
            ctxMenuFileList = new ContextMenuStrip(components);
            seeLayersMenuItem = new ToolStripMenuItem();
            chkKeepChanges = new CheckBox();
            grpExportTypes = new GroupBox();
            rdoSpriteSheet = new RadioButton();
            rdoEveryFrame = new RadioButton();
            btnExport = new Button();
            grpExportOptions = new GroupBox();
            chkEveryLayer = new CheckBox();
            chkAllLayers = new CheckBox();
            toolTip = new ToolTip(components);
            lblScale = new Label();
            nudScale = new NumericUpDown();
            lstLayerList = new ListBox();
            lblFileList = new Label();
            lblLayerList = new Label();
            ctxMenuFileList.SuspendLayout();
            grpExportTypes.SuspendLayout();
            grpExportOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) nudScale).BeginInit();
            SuspendLayout();
            // 
            // txtSearchFolder
            // 
            txtSearchFolder.Enabled = false;
            txtSearchFolder.Location = new Point(96, 6);
            txtSearchFolder.Name = "txtSearchFolder";
            txtSearchFolder.Size = new Size(445, 23);
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
            btnSearchFolder.Click += btnSearchFolder_Click;
            // 
            // lstFilelist
            // 
            lstFilelist.ContextMenuStrip = ctxMenuFileList;
            lstFilelist.FormattingEnabled = true;
            lstFilelist.ItemHeight = 15;
            lstFilelist.Location = new Point(12, 57);
            lstFilelist.Name = "lstFilelist";
            lstFilelist.SelectionMode = SelectionMode.MultiSimple;
            lstFilelist.Size = new Size(387, 214);
            lstFilelist.TabIndex = 3;
            lstFilelist.SelectedIndexChanged += lstFilelist_SelectedIndexChanged;
            // 
            // ctxMenuFileList
            // 
            ctxMenuFileList.Items.AddRange(new ToolStripItem[] { seeLayersMenuItem });
            ctxMenuFileList.Name = "ctxMenuFileList";
            ctxMenuFileList.Size = new Size(201, 26);
            // 
            // seeLayersMenuItem
            // 
            seeLayersMenuItem.Name = "seeLayersMenuItem";
            seeLayersMenuItem.Size = new Size(200, 22);
            seeLayersMenuItem.Text = "Select a file to see layers";
            seeLayersMenuItem.Click += seeLayersMenuItem_Click;
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
            chkKeepChanges.CheckedChanged += basicControl_Changed;
            // 
            // grpExportTypes
            // 
            grpExportTypes.Controls.Add(rdoSpriteSheet);
            grpExportTypes.Controls.Add(rdoEveryFrame);
            grpExportTypes.Location = new Point(12, 277);
            grpExportTypes.Name = "grpExportTypes";
            grpExportTypes.Size = new Size(387, 56);
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
            rdoSpriteSheet.CheckedChanged += basicControl_Changed;
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
            rdoEveryFrame.CheckedChanged += basicControl_Changed;
            // 
            // btnExport
            // 
            btnExport.Location = new Point(697, 524);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(75, 25);
            btnExport.TabIndex = 6;
            btnExport.Text = "Export!";
            btnExport.UseVisualStyleBackColor = true;
            btnExport.Click += btnExport_Click;
            // 
            // grpExportOptions
            // 
            grpExportOptions.Controls.Add(chkEveryLayer);
            grpExportOptions.Controls.Add(chkAllLayers);
            grpExportOptions.Location = new Point(12, 339);
            grpExportOptions.Name = "grpExportOptions";
            grpExportOptions.Size = new Size(387, 50);
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
            chkEveryLayer.CheckedChanged += basicControl_Changed;
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
            chkAllLayers.CheckedChanged += basicControl_Changed;
            // 
            // lblScale
            // 
            lblScale.AutoSize = true;
            lblScale.Location = new Point(593, 527);
            lblScale.Name = "lblScale";
            lblScale.Size = new Size(37, 15);
            lblScale.TabIndex = 8;
            lblScale.Text = "Scale:";
            // 
            // nudScale
            // 
            nudScale.Location = new Point(636, 525);
            nudScale.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudScale.Name = "nudScale";
            nudScale.Size = new Size(55, 23);
            nudScale.TabIndex = 9;
            nudScale.Value = new decimal(new int[] { 1, 0, 0, 0 });
            nudScale.ValueChanged += basicControl_Changed;
            // 
            // lstLayerList
            // 
            lstLayerList.FormattingEnabled = true;
            lstLayerList.ItemHeight = 15;
            lstLayerList.Location = new Point(405, 57);
            lstLayerList.Name = "lstLayerList";
            lstLayerList.Size = new Size(217, 214);
            lstLayerList.TabIndex = 10;
            // 
            // lblFileList
            // 
            lblFileList.AutoSize = true;
            lblFileList.Location = new Point(12, 36);
            lblFileList.Name = "lblFileList";
            lblFileList.Size = new Size(49, 15);
            lblFileList.TabIndex = 11;
            lblFileList.Text = "File List:";
            // 
            // lblLayerList
            // 
            lblLayerList.AutoSize = true;
            lblLayerList.Location = new Point(405, 36);
            lblLayerList.Name = "lblLayerList";
            lblLayerList.Size = new Size(59, 15);
            lblLayerList.TabIndex = 12;
            lblLayerList.Text = "Layer List:";
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 561);
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
            ctxMenuFileList.ResumeLayout(false);
            grpExportTypes.ResumeLayout(false);
            grpExportTypes.PerformLayout();
            grpExportOptions.ResumeLayout(false);
            grpExportOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) nudScale).EndInit();
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
        private ContextMenuStrip ctxMenuFileList;
        private ToolStripMenuItem seeLayersMenuItem;
    }
}
