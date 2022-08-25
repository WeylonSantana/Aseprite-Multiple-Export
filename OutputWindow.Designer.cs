namespace Aseprite_Multiple_Export
{
    partial class OutputWindow
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
            this.lstFileList = new System.Windows.Forms.ListBox();
            this.lblFileList = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lstFileList
            // 
            this.lstFileList.FormattingEnabled = true;
            this.lstFileList.ItemHeight = 15;
            this.lstFileList.Location = new System.Drawing.Point(12, 27);
            this.lstFileList.Name = "lstFileList";
            this.lstFileList.Size = new System.Drawing.Size(472, 229);
            this.lstFileList.TabIndex = 9;
            // 
            // lblFileList
            // 
            this.lblFileList.AutoSize = true;
            this.lblFileList.Location = new System.Drawing.Point(12, 8);
            this.lblFileList.Name = "lblFileList";
            this.lblFileList.Size = new System.Drawing.Size(116, 15);
            this.lblFileList.TabIndex = 8;
            this.lblFileList.Text = "List of exported files:";
            // 
            // OutputWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 265);
            this.ControlBox = false;
            this.Controls.Add(this.lstFileList);
            this.Controls.Add(this.lblFileList);
            this.MaximizeBox = false;
            this.MdiChildrenMinimizedAnchorBottom = false;
            this.MinimizeBox = false;
            this.Name = "OutputWindow";
            this.ShowIcon = false;
            this.Text = "OutputWindow";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ListBox lstFileList;
        private Label lblFileList;
        private Button btnClose;
    }
}