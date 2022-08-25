using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aseprite_Multiple_Export
{
    public partial class OutputWindow : Form
    {
        public OutputWindow()
        {
            InitializeComponent();
            lstFileList.Items.Clear();
        }

        public void UpdateData(string fileName)
        {
            lstFileList.Items.Add(fileName);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }
    }
}
