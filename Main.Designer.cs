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
            txtSearchFolder = new TextBox();
            lblSearchFolder = new Label();
            btnSearchFolder = new Button();
            lstFilelist = new ListBox();
            chkKeepChanges = new CheckBox();
            grpExportTypes = new GroupBox();
            rdoSpriteSheet = new RadioButton();
            rdoEveryFrame = new RadioButton();
            btnExport = new Button();
            grpExportTypes.SuspendLayout();
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
            lstFilelist.FormattingEnabled = true;
            lstFilelist.ItemHeight = 15;
            lstFilelist.Location = new Point(12, 35);
            lstFilelist.Name = "lstFilelist";
            lstFilelist.SelectionMode = SelectionMode.MultiSimple;
            lstFilelist.Size = new Size(529, 229);
            lstFilelist.TabIndex = 3;
            // 
            // chkKeepChanges
            // 
            chkKeepChanges.AutoSize = true;
            chkKeepChanges.Location = new Point(865, 9);
            chkKeepChanges.Name = "chkKeepChanges";
            chkKeepChanges.Size = new Size(106, 19);
            chkKeepChanges.TabIndex = 4;
            chkKeepChanges.Text = "Keep Changes?";
            chkKeepChanges.UseVisualStyleBackColor = true;
            chkKeepChanges.CheckedChanged += chkKeepChanges_CheckedChanged;
            // 
            // grpExportTypes
            // 
            grpExportTypes.Controls.Add(rdoSpriteSheet);
            grpExportTypes.Controls.Add(rdoEveryFrame);
            grpExportTypes.Location = new Point(12, 270);
            grpExportTypes.Name = "grpExportTypes";
            grpExportTypes.Size = new Size(529, 56);
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
            rdoSpriteSheet.UseVisualStyleBackColor = true;
            rdoSpriteSheet.CheckedChanged += rdoSpriteSheet_CheckedChanged;
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
            rdoEveryFrame.UseVisualStyleBackColor = true;
            rdoEveryFrame.CheckedChanged += rdoEveryFrame_CheckedChanged;
            // 
            // btnExport
            // 
            btnExport.Location = new Point(896, 587);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(75, 23);
            btnExport.TabIndex = 6;
            btnExport.Text = "Export!";
            btnExport.UseVisualStyleBackColor = true;
            btnExport.Click += btnExport_Click;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(983, 622);
            Controls.Add(btnExport);
            Controls.Add(grpExportTypes);
            Controls.Add(chkKeepChanges);
            Controls.Add(lstFilelist);
            Controls.Add(btnSearchFolder);
            Controls.Add(lblSearchFolder);
            Controls.Add(txtSearchFolder);
            MinimumSize = new Size(800, 600);
            Name = "Main";
            Text = "Aseprite Multiple Export";
            Load += Main_Load;
            grpExportTypes.ResumeLayout(false);
            grpExportTypes.PerformLayout();
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
    }
}
