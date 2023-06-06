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
            chkExportTags.Checked = Settings.Default.ExportTags;
            chkExportLayers.Checked = Settings.Default.ExportLayers;
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
            ExportTags = chkExportTags.Checked;
            ExportLayers = chkExportLayers.Checked;
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

            if (ExportData)
            {
                chkExportTags.Enabled = true;
                chkExportLayers.Enabled = true;
            }
            else
            {
                chkExportTags.Checked = false;
                chkExportTags.Enabled = false;
                chkExportLayers.Checked = false;
                chkExportLayers.Enabled = false;
                ExportTags = false;
                ExportLayers = false;
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
                            string fileName = "";
                            if (filePath.Length > 0)
                            {
                                fileName = filePath[filePath.Length - 1];
                            }

                            lstFileList.Items.Add(fileName);
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

        private void btnExport_Click(object sender, EventArgs e)
        {
            if(!checkBasicErrors())
            {
                return;
            }

            lstExportedItems.Items.Clear();

            //Let's start structuring our command
            int index = 0;
            foreach (var file in FileList)
            {
                string command = "";

                string[]? filePath = file.Split("\\");
                string fileName = "";
                if (filePath.Length > 0)
                {
                    fileName = filePath[filePath.Length - 1];
                }

                if (KeepOriginalFilename)
                {
                    OutputName = fileName.Replace(".aseprite", "");
                }

                string finalOutputName = "";
                if (LayerList != null && LayerList.Length > 0)
                {
                    for (int i = 0; i < LayerList.Length; i++)
                    {
                        finalOutputName = $"{OutputName}-{LayerList[i]}";
                        command = $"-b --layer \"{LayerList[i]}\" {fileName} --scale {SheetScale} --sheet-columns {OutputColums} --ignore-empty --sheet {SheetScale}x/{finalOutputName}.png";

                        if (ExportData)
                        {
                            if (ExportTags)
                            {
                                command += " --list-tags";
                            }

                            if (ExportLayers)
                            {
                                command += " --list-layers";
                            }

                            command += $" --data {SheetScale}x/{finalOutputName}.json";
                        }

                        Export(command, finalOutputName);
                    }
                }
                else
                {
                    if (index == 0 || KeepOriginalFilename)
                    {
                        finalOutputName = $"{OutputName}";
                    }
                    else
                    {
                        finalOutputName = $"{OutputName}{index}";
                    }

                    if (EveryLayer)
                    {
                        LayerList = GetLayers(fileName);
                        btnExport_Click(null, null);
                        return;
                    }
                    else
                    {
                        command = $"-b --all-layers {fileName} --scale {SheetScale} --sheet-columns {OutputColums} --ignore-empty --sheet {SheetScale}x/{finalOutputName}.png";
                    }

                    if (ExportData)
                    {
                        if (ExportTags)
                        {
                            command += " --list-tags";
                        }

                        if (ExportLayers)
                        {
                            command += " --list-layers";
                        }

                        command += $" --data {SheetScale}x/{finalOutputName}.json";
                    }

                    Export(command, finalOutputName);
                }

                index++;
            }

            MessageBox.Show("The files have been exported!", "Successfully exported", MessageBoxButtons.OK);
        }

        private string[] GetLayers(string fileName)
        {
            string finalCommand = $"/C \"\"{Aseprite}\" -b --list-layers {fileName}\"";
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WorkingDirectory = FolderPath;
            process.StartInfo.Arguments = finalCommand;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            string[] layers = output.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            return layers;
        }

        private void Export(string command, string fileName)
        {
            string finalCommand = $"/C \"\"{Aseprite}\" {command}\"";
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WorkingDirectory = FolderPath;
            process.StartInfo.Arguments = finalCommand;
            process.Start();
            process.WaitForExit();

            lstExportedItems.Items.Add(fileName);
        }

        private void RemoveFrames()
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
                    MessageBox.Show("Frame number field can not be empty!");
                    return;
                }

                Process process = new Process();
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.WorkingDirectory = FolderPath;

                string scriptPath = Path.Combine(Application.StartupPath, "RemoveFrames.script.lua");

                foreach (var file in FileList)
                {
                    string[]? filePath = file.Split("\\");
                    string fileName = "";
                    if (filePath.Length > 0)
                    {
                        fileName = filePath[filePath.Length - 1];
                    }

                    string command = $"-b --script-param filename={fileName} --script-param frames={uniqueFrames} --script {scriptPath}";
                    string finalCommand = $"/C \"\"{Aseprite}\" {command}\"";
                    process.StartInfo.Arguments = finalCommand;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.Start();
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                }

                MessageBox.Show("Frames removidos com sucesso!");
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

        private void btnRemoveFrames_Click(object sender, EventArgs e)
        {
            RemoveFrames();
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
            Settings.Default.ExportTags = chkExportTags.Checked;
            Settings.Default.ExportLayers = chkExportLayers.Checked;
            Settings.Default.Save();
            MessageBox.Show("Settings Saved", "Save Settings", MessageBoxButtons.OK);
        }
        #endregion
    }
}