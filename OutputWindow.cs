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
    }
}
