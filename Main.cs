using System.Diagnostics;

namespace Aseprite_Multiple_Export
{
    public partial class Main : Form
    {
        private bool KeepChanges = false;
        private string FolderPath;
        private List<string> Filelist;

        private Process process;
        private bool _isLoading = false;

        public Main()
        {
            InitializeComponent();
            string output = ProcessCommand("--version");
            if ( output.Length == 0 )
            {
                string error = "Aseprite not found. Please insert the path of Aseprite folder to the Path Variable.\n\n";
                error += "Step 1: Open the folder where Aseprite is installed.\n";
                error += "Step 2: Copy the path of the folder.\n";
                error += "Step 3: Type \"Variable\" in the Windows search bar and click on \"Edit the system environment variables\".\n";
                error += "Step 4: Click on \"Environment Variables\".\n";
                error += "Step 5: In the \"System variables\" section, click on \"Path\" and then click on \"Edit\".\n";
                error += "Step 6: Click on \"New\" and paste the path of the Aseprite folder.\n";
                error += "Step 7: Click on \"OK\" and then click on \"OK\" again.\n";
                error += "Step 8: Restart this application.\n";
                error += "Step 9: If the error persists, restart your computer.";
                MessageBox.Show(error, "Error - Aseprite not Found!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
        }

        private string ProcessCommand(string command)
        {
            process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WorkingDirectory = FolderPath;
            process.StartInfo.Arguments = $"/C \"Aseprite {command}\"";
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            return output;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            _isLoading = true;
            chkKeepChanges.Checked = Properties.Settings.Default.KeepChanges;
            txtSearchFolder.Text = Properties.Settings.Default.FolderPath;

            UpdateForm();
            _isLoading = false;
        }

        private void Main_Save()
        {
            if (_isLoading) return;
            Properties.Settings.Default.KeepChanges = KeepChanges;
            Properties.Settings.Default.FolderPath = KeepChanges ? FolderPath : string.Empty;

            Properties.Settings.Default.Save();
        }

        private void UpdateForm()
        {
            KeepChanges = chkKeepChanges.Checked;

            //Updating file list
            FolderPath = txtSearchFolder.Text;
            if ( !string.IsNullOrEmpty(FolderPath) )
            {
                if ( Directory.Exists(FolderPath) )
                {
                    List<string> files =
                    [
                        .. Directory.EnumerateFiles(FolderPath, "*.ase").ToList(),
                        .. Directory.EnumerateFiles(FolderPath, "*.aseprite").ToList(),
                    ];
                    lstFilelist.Items.Clear();
                    if ( files.Count > 0 )
                    {
                        foreach ( var file in files )
                        {
                            lstFilelist.Items.Add(Path.GetFileName(file));
                        }
                    }
                    Filelist = files;
                }
                else
                {
                    txtSearchFolder.Text = "";
                    FolderPath = "";
                    lstFilelist.Items.Clear();
                }
            }

            Main_Save();
        }

        private void chkKeepChanges_CheckedChanged(object sender, EventArgs e)
        {
            KeepChanges = chkKeepChanges.Checked;
            Main_Save();
        }

        private void btnSearchFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog openFolder = new FolderBrowserDialog();
            if ( openFolder.ShowDialog() == DialogResult.OK )
            {
                txtSearchFolder.Text = openFolder.SelectedPath;
                UpdateForm();
            }
        }
    }
}
