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
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(983, 622);
            Controls.Add(chkKeepChanges);
            Controls.Add(lstFilelist);
            Controls.Add(btnSearchFolder);
            Controls.Add(lblSearchFolder);
            Controls.Add(txtSearchFolder);
            MinimumSize = new Size(800, 600);
            Name = "Main";
            Text = "Aseprite Multiple Export";
            Load += Main_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtSearchFolder;
        private Label lblSearchFolder;
        private Button btnSearchFolder;
        private ListBox lstFilelist;
        private CheckBox chkKeepChanges;
    }
}
