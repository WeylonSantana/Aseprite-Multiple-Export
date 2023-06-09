using Aseprite_Multiple_Export.Properties;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Aseprite_Multiple_Export
{
    public partial class Main: Form
    {
        private string Aseprite;
        private string FolderPath;
        private string[] FileList;
        private string[] LayerList;
        private string OutputName;
        private int OutputColums = 4;
        private bool ExportData = true;
        private bool KeepOriginalFilename = false;
        private bool EveryLayer = false;
        private bool ExportTags = false;
        private bool ExportLayers = false;
        private int SheetScale;

        private Process process;

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            txtAsepriteSearch.Text = Settings.Default.Aseprite;
            txtFolderSearch.Text = Settings.Default.FolderPath;
            txtLayerList.Text = Settings.Default.LayerList;
            txtDefaultOutputName.Text = Settings.Default.OutputName;
            nudDefaultColumns.Value = Settings.Default.OutputColumns;
            chkExportData.Checked = Settings.Default.ExportJson;
            chkOriginalFilename.Checked = Settings.Default.KeepOriginalName;
            chkEveryLayer.Checked = Settings.Default.EveryLayer;
            nudScale.Value = Settings.Default.Scale;

            UpdateForm();
        }

        private void UpdateForm()
        {
            Aseprite = txtAsepriteSearch.Text;
            FolderPath = txtFolderSearch.Text;
            OutputName = txtDefaultOutputName.Text;
            OutputColums = (int) nudDefaultColumns.Value;
            ExportData = chkExportData.Checked;
            KeepOriginalFilename = chkOriginalFilename.Checked;
            EveryLayer = chkEveryLayer.Checked;
            txtLayerList.Enabled = EveryLayer ? false : true;
            SheetScale = (int) nudScale.Value;


            if (!string.IsNullOrEmpty(txtLayerList.Text))
            {
                LayerList = txtLayerList.Text.Split(",");
                chkEveryLayer.Enabled = false;
            }
            else
            {
                LayerList = new string[0];
                chkEveryLayer.Enabled = true;
            }

            if (KeepOriginalFilename)
            {
                txtDefaultOutputName.Enabled = false;
            }
            else
            {
                txtDefaultOutputName.Enabled = true;
            }

            if (!string.IsNullOrEmpty(FolderPath))
            {
                if (Directory.Exists(FolderPath))
                {
                    string[] files = Directory.GetFiles(FolderPath, "*.aseprite");
                    lstFileList.Items.Clear();
                    if (files.Length > 0)
                    {
                        foreach (var file in files)
                        {
                            var filePath = file.Split("\\");
                            string filename = "";
                            if (filePath.Length > 0)
                            {
                                filename = filePath[filePath.Length - 1];
                            }

                            lstFileList.Items.Add(filename);
                        }
                    }
                    FileList = files;
                }
                else
                {
                    txtFolderSearch.Text = "";
                    FolderPath = "";
                    lstFileList.Items.Clear();
                }
            }
        }

        private bool checkBasicErrors()
        {
            if (string.IsNullOrEmpty(Aseprite))
            {
                MessageBox.Show("No Aseprite Selected", "Error Detected");
                return false;
            }

            if (string.IsNullOrEmpty(FolderPath))
            {
                MessageBox.Show("No Folder Path Selected", "Error Detected");
                return false;
            }

            if (string.IsNullOrEmpty(OutputName) && !KeepOriginalFilename)
            {
                MessageBox.Show("Default Output Filename can not be empty!", "Error Detected");
                return false;
            }

            if (FileList.Length < 1)
            {
                string title = "Error Detected in Input";
                string msg = "There are no files to convert";
                MessageBox.Show(msg, title);
                return false;
            }

            return true;
        }

        private string ProcessCommand(string command)
        {
            process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WorkingDirectory = FolderPath;
            process.StartInfo.Arguments = $"/C \"\"{Aseprite}\" {command}\"";
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            return output;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (!checkBasicErrors())
            {
                return;
            }

            lstExportedItems.Items.Clear();
            string scriptPath = Path.Combine(Application.StartupPath, "Export.script.lua");

            foreach (var file in FileList)
            {
                string[]? filePath = file.Split("\\");
                string filename = filePath[filePath.Length - 1];

                if (KeepOriginalFilename)
                {
                    OutputName = filename.Replace(".aseprite", "");
                }

                string command = "-b";
                command += $" --script-param filename={filename}";
                command += $" --script-param scale={SheetScale}";
                command += $" --script-param columns={OutputColums}";
                command += $" --script-param outputName={OutputName}";

                if (EveryLayer)
                {
                    command += $" --script-param everyLayer=True";
                }
                else
                {
                    if (LayerList.Length != 0)
                    {
                        command += $" --script-param layerList={string.Join(',', LayerList)}";
                    }
                }

                if (ExportData)
                {
                    command += $" --script-param exportData=True";
                }

                command += $" --script {scriptPath}";

                ProcessCommand(command);
                lstExportedItems.Items.Add(filename);
            }

            MessageBox.Show("The files have been exported!", "Successfully exported", MessageBoxButtons.OK);
        }

        private void btnRemoveFrames_Click(object sender, EventArgs e)
        {
            if (!checkBasicErrors())
            {
                return;
            }

            #region Create input form
            Form form = new Form();
            form.Text = "Remove frames from files";
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.FormBorderStyle = FormBorderStyle.FixedSingle;
            form.Size = new Size(240, 130);
            form.StartPosition = FormStartPosition.CenterScreen;

            Label title = new Label();
            title.Text = "Remove selected frames from all files";
            title.Location = new Point(10, 10);
            title.AutoSize = true;
            form.Controls.Add(title);

            Label label = new Label();
            label.Text = "Frame numbers:";
            label.Location = new Point(10, 30);
            label.AutoSize = true;
            form.Controls.Add(label);

            TextBox textBox = new TextBox();
            textBox.Location = new Point(110, 28);
            textBox.TextChanged += (s, e) =>
            {
                textBox.Text = Regex.Replace(textBox.Text, "[^0-9,]", "");
            };
            form.Controls.Add(textBox);

            Button button = new Button();
            button.Text = "OK";
            button.Location = new Point(135, 60);
            button.DialogResult = DialogResult.OK;
            form.Controls.Add(button);

            form.AcceptButton = button;
            #endregion

            if (form.ShowDialog() == DialogResult.OK)
            {
                string frames = textBox.Text;
                string[] frameNumbers = frames.Split(',');
                HashSet<string> uniqueFrameNumbers = new HashSet<string>(frameNumbers);
                string uniqueFrames = string.Join(",", uniqueFrameNumbers);

                if (uniqueFrames.Length == 0)
                {
                    string msgTitle = "Error Detected in Input";
                    MessageBox.Show("Frame number field can not be empty!", msgTitle);
                    return;
                }

                string scriptPath = Path.Combine(Application.StartupPath, "RemoveFrames.script.lua");

                foreach (var file in FileList)
                {
                    string[]? filePath = file.Split("\\");
                    string filename = "";
                    if (filePath.Length > 0)
                    {
                        filename = filePath[filePath.Length - 1];
                    }

                    string command = $"-b --script-param filename={filename} --script-param frames={uniqueFrames} --script {scriptPath}";
                    ProcessCommand(command);
                }

                MessageBox.Show("Frames removidos com sucesso!", Application.ProductName);
            }
        }

        private void btnAddTags_Click(object sender, EventArgs e)
        {
            if (!checkBasicErrors())
            {
                return;
            }

            #region Create input form
            Form form = new Form();
            form.Text = "Add tags in files";
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.FormBorderStyle = FormBorderStyle.FixedSingle;
            form.Size = new Size(300, 180);
            form.StartPosition = FormStartPosition.CenterScreen;

            Label label = new Label();
            label.Text = "Tags:";
            label.Location = new Point(10, 10);
            label.AutoSize = true;
            form.Controls.Add(label);

            TextBox textBox = new TextBox();
            textBox.Location = new Point(50, 8);
            textBox.Width = 200;
            form.Controls.Add(textBox);

            Label separatorLabel = new Label();
            separatorLabel.Text = "All tags will be separated by comma";
            separatorLabel.Location = new Point(10, 40);
            separatorLabel.AutoSize = true;
            form.Controls.Add(separatorLabel);

            CheckBox removeTagsCheckbox = new CheckBox();
            removeTagsCheckbox.Text = "Remove all tags";
            removeTagsCheckbox.Location = new Point(10, 70);
            removeTagsCheckbox.AutoSize = true;
            form.Controls.Add(removeTagsCheckbox);

            Button button = new Button();
            button.Text = "OK";
            button.Location = new Point(200, 110);
            button.DialogResult = DialogResult.OK;
            form.Controls.Add(button);

            form.AcceptButton = button;
            #endregion

            if (form.ShowDialog() == DialogResult.OK)
            {
                string[] tags = textBox.Text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(tag => tag.Trim())
                            .Distinct()
                            .ToArray();

                if (tags.Length == 0)
                {
                    string msgTitle = "Error Detected in Input";
                    MessageBox.Show("Frame number field can not be empty!", msgTitle);
                    return;
                }

                string scriptPath = Path.Combine(Application.StartupPath, "AddTag.script.lua");

                foreach (var file in FileList)
                {
                    string[]? filePath = file.Split("\\");
                    string filename = filePath[filePath.Length - 1];
                    string command = $"-b";
                    command += $" --script-param filename={filename}";
                    command += $" --script-param tags={string.Join(',', tags)}";
                    if (removeTagsCheckbox.Checked)
                    {
                        command += $" --script-param clean={removeTagsCheckbox.Checked}";
                    }
                    command += $" --script {scriptPath}";
                    string output = ProcessCommand(command);
                }

                MessageBox.Show("Tags adicionadas com sucesso!", Application.ProductName);
            }
        }

        #region InputsChanged
        private void btnAsepriteSearch_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.DefaultExt = ".exe";
            openFile.Filter = "exe files (*.exe)|*.exe";
            openFile.CheckFileExists = true;
            openFile.CheckPathExists = true;
            openFile.Multiselect = false;

            if (openFile.ShowDialog() == DialogResult.OK && openFile.FileName.Contains("exe"))
            {
                string tempFilename = openFile.FileName.ToLower();
                if (!tempFilename.Contains("aseprite"))
                {
                    string title = "Error Detected in Input";
                    string msg = "File or folder does not contain the name \"aseprite\" confirm?";
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    DialogResult result;
                    result = MessageBox.Show(msg, title, buttons);
                    if (result == DialogResult.Yes)
                    {
                        txtAsepriteSearch.Text = openFile.FileName;
                    }
                }
                else
                {
                    txtAsepriteSearch.Text = openFile.FileName;
                }

                UpdateForm();
            }
            else if (!string.IsNullOrEmpty(openFile.FileName) && !openFile.FileName.Contains("exe"))
            {
                string title = "Error Detected in Input";
                string msg = "Invalid File";
                MessageBox.Show(msg, title);
            }
        }

        private void btnFolderSearch_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog openFolder = new FolderBrowserDialog();
            if (openFolder.ShowDialog() == DialogResult.OK)
            {
                txtFolderSearch.Text = openFolder.SelectedPath;
                UpdateForm();
            }
        }

        private void txtLayerList_TextChanged(object sender, EventArgs e)
        {
            UpdateForm();
        }

        private void txtDefaultOutputName_TextChanged(object sender, EventArgs e)
        {
            UpdateForm();
        }

        private void txtCondition1_TextChanged(object sender, EventArgs e)
        {
            UpdateForm();
        }

        private void txtCondition2_TextChanged(object sender, EventArgs e)
        {
            UpdateForm();
        }

        private void txtCondition3_TextChanged(object sender, EventArgs e)
        {
            UpdateForm();
        }

        private void txtSuffix1_TextChanged(object sender, EventArgs e)
        {
            UpdateForm();
        }

        private void txtSuffix2_TextChanged(object sender, EventArgs e)
        {
            UpdateForm();
        }

        private void txtSuffix3_TextChanged(object sender, EventArgs e)
        {
            UpdateForm();
        }

        private void nudColumns1_ValueChanged(object sender, EventArgs e)
        {
            UpdateForm();
        }

        private void nudColumns2_ValueChanged(object sender, EventArgs e)
        {
            UpdateForm();
        }

        private void nudColumns3_ValueChanged(object sender, EventArgs e)
        {
            UpdateForm();
        }

        private void chkExportData_CheckedChanged(object sender, EventArgs e)
        {
            UpdateForm();
        }

        private void nudDefaultColumns_ValueChanged(object sender, EventArgs e)
        {
            UpdateForm();
        }

        private void chkOriginalFilename_CheckedChanged_1(object sender, EventArgs e)
        {
            UpdateForm();
        }

        private void chkEveryLayer_CheckedChanged(object sender, EventArgs e)
        {
            UpdateForm();
        }

        private void chkExportTags_CheckedChanged(object sender, EventArgs e)
        {
            UpdateForm();
        }

        private void chkExportLayers_CheckedChanged(object sender, EventArgs e)
        {
            UpdateForm();
        }

        private void nudScale_ValueChanged(object sender, EventArgs e)
        {
            UpdateForm();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            Settings.Default.Aseprite = txtAsepriteSearch.Text;
            Settings.Default.FolderPath = txtFolderSearch.Text;
            Settings.Default.LayerList = txtLayerList.Text;
            Settings.Default.OutputName = txtDefaultOutputName.Text;
            Settings.Default.OutputColumns = (int) nudDefaultColumns.Value;
            Settings.Default.ExportJson = chkExportData.Checked;
            Settings.Default.KeepOriginalName = chkOriginalFilename.Checked;
            Settings.Default.EveryLayer = chkEveryLayer.Checked;
            Settings.Default.Save();
            MessageBox.Show("Settings Saved", "Save Settings", MessageBoxButtons.OK);
        }
        #endregion
    }
}