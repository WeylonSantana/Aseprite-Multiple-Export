using Aseprite_Multiple_Export.Properties;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Aseprite_Multiple_Export
{
    public partial class Main : Form
    {
        private string Aseprite;
        private string FolderPath;
        private string[] FileList;
        private string[] LayerList;
        private string SuffixCondition1;
        private string SuffixCondition2;
        private string SuffixCondition3;
        private string Suffix1;
        private string Suffix2;
        private string Suffix3;
        private int Columns1;
        private int Columns2;
        private int Columns3;
        private string OutputName;
        private int DefaultColums = 4;
        private bool ExportData = true;
        private bool KeepOriginalFilename = false;
        private OutputWindow outputWindow;
        private int Scale;

        public Main()
        {
            InitializeComponent();
            outputWindow = new OutputWindow();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            txtAsepriteSearch.Text = Settings.Default.Aseprite;
            txtFolderSearch.Text = Settings.Default.FolderPath;
            txtLayerList.Text = Settings.Default.LayerList;
            txtDefaultOutputName.Text = Settings.Default.OutputName;
            txtCondition1.Text = Settings.Default.Condition1;
            txtCondition2.Text = Settings.Default.Condition2;
            txtCondition3.Text = Settings.Default.Condition3;
            txtSuffix1.Text = Settings.Default.Suffix1;
            txtSuffix2.Text = Settings.Default.Suffix2;
            txtSuffix3.Text = Settings.Default.Suffix3;
            nudDefaultColumns.Value = Settings.Default.Columns;
            nudColumns1.Value = Settings.Default.ConditionColumn1;
            nudColumns2.Value = Settings.Default.ConditionColumn2;
            nudColumns3.Value = Settings.Default.ConditionColumn3;
            chkExportData.Checked = Settings.Default.ExportJson;
            chkOriginalFilename.Checked = Settings.Default.KeepOriginalName;
            nudScale.Value = Settings.Default.Scale;

            UpdateForm();
        }

        private void UpdateForm()
        {
            Aseprite = txtAsepriteSearch.Text;
            FolderPath = txtFolderSearch.Text;
            OutputName = txtDefaultOutputName.Text;
            SuffixCondition1 = txtCondition1.Text;
            SuffixCondition2 = txtCondition2.Text;
            SuffixCondition3 = txtCondition3.Text;
            Suffix1 = txtSuffix1.Text;
            Suffix2 = txtSuffix2.Text;
            Suffix3 = txtSuffix3.Text;
            Columns1 = (int) nudColumns1.Value;
            Columns2 = (int) nudColumns2.Value;
            Columns3 = (int) nudColumns3.Value;
            DefaultColums = (int) nudDefaultColumns.Value;
            ExportData = chkExportData.Checked;
            KeepOriginalFilename = chkOriginalFilename.Checked;
            Scale = (int) nudScale.Value;

            if(!string.IsNullOrEmpty(txtLayerList.Text))
            {
                LayerList = txtLayerList.Text.Split(",");
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
                if(Directory.Exists(FolderPath))
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

        private void btnExport_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(Aseprite))
            {
                MessageBox.Show("No Aseprite Selected", "Error Detected");
                return;
            }

            if(string.IsNullOrEmpty(FolderPath))
            {
                MessageBox.Show("No Folder Path Selected", "Error Detected");
                return;
            }

            if(string.IsNullOrEmpty(OutputName) && !KeepOriginalFilename)
            {
                MessageBox.Show("Default Output Filename can not be empty!", "Error Detected");
                return;
            }

            if(FileList.Length < 1)
            {
                string title = "Error Detected in Input";
                string msg = "There are no files to convert";
                MessageBox.Show(msg, title);
                return;
            }

            //Let's start structuring our command
            int index = 0;
            foreach(var file in FileList)
            {
                string suffix = "";
                int columns = DefaultColums;
                string command = "";

                string[]? filePath = file.Split("\\");
                string fileName = "";
                if (filePath.Length > 0)
                {
                    fileName = filePath[filePath.Length - 1];
                }

                //Lets check conditions
                if (!string.IsNullOrEmpty(SuffixCondition1) &&
                    file.Contains(SuffixCondition1) &&
                    !string.IsNullOrEmpty(Suffix1) &&
                    Columns1 > 0)
                {
                    suffix = Suffix1;
                    columns = Columns1;
                }

                if (!string.IsNullOrEmpty(SuffixCondition2) &&
                    file.Contains(SuffixCondition2) &&
                    !string.IsNullOrEmpty(Suffix2) &&
                    Columns2 > 0)
                {
                    suffix = Suffix2;
                    columns = Columns2;
                }

                if (!string.IsNullOrEmpty(SuffixCondition3) &&
                    file.Contains(SuffixCondition3) &&
                    !string.IsNullOrEmpty(Suffix3) &&
                    Columns3 > 0)
                {
                    suffix = Suffix3;
                    columns = Columns3;
                }

                if(KeepOriginalFilename)
                {
                    OutputName = fileName.Replace(".aseprite", "");
                }

                string finalOutputName = "";
                if(LayerList != null && LayerList.Length > 0)
                {
                    for(int i = 0; i < LayerList.Length; i++)
                    {
                        finalOutputName = $"{OutputName}_{LayerList[i]}{suffix}";
                        command = $"-b --layer \"{LayerList[i]}\" {fileName} --scale {Scale} --sheet-columns {columns} --sheet {Scale}x/{finalOutputName}.png";

                        if(ExportData)
                        {
                            command += $" --data {Scale}x/{finalOutputName}.json";
                        }

                        Export(command, finalOutputName);
                    }
                }
                else
                {
                    if(index == 0 || KeepOriginalFilename || suffix.Length > 0)
                    {
                        finalOutputName = $"{OutputName}{suffix}";
                    }
                    else
                    {
                        finalOutputName = $"{OutputName}{index}{suffix}";
                    }

                    command = $"-b --all-layers {fileName} --scale {Scale} --sheet-columns {columns} --sheet {Scale}x/{finalOutputName}.png";
                    if (ExportData)
                    {
                        command += $" --data {Scale}x/{finalOutputName}.json";
                    }

                    Export(command, finalOutputName);
                }

                index++;
            }

            MessageBox.Show("The files have been exported!", "Successfully exported", MessageBoxButtons.OK);
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
            outputWindow.UpdateData(fileName);
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            char[] charsToTrim = { '*', ' ', '\'', ';', '.' };
            txtLayerList.Text = Regex.Replace(txtLayerList.Text, @"[^0-9a-zA-Z,]+", "");
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

        private void nudScale_ValueChanged(object sender, EventArgs e)
        {
            UpdateForm();
        }

        private void btnExportDebug_Click(object sender, EventArgs e)
        {
            if (!outputWindow.Visible)
            {
                outputWindow.Show();
                btnExportDebug.Text = "Hide Export Debug";
            }
            else
            {
                outputWindow.Hide();
                btnExportDebug.Text = "Show Export Debug";
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            Settings.Default.Aseprite = txtAsepriteSearch.Text;
            Settings.Default.FolderPath = txtFolderSearch.Text;
            Settings.Default.LayerList = txtLayerList.Text;
            Settings.Default.OutputName = txtDefaultOutputName.Text;
            Settings.Default.Columns = (int) nudDefaultColumns.Value;
            Settings.Default.Condition1 = txtCondition1.Text;
            Settings.Default.Condition2 = txtCondition2.Text;
            Settings.Default.Condition3 = txtCondition3.Text;
            Settings.Default.Suffix1 = txtSuffix1.Text;
            Settings.Default.Suffix2 = txtSuffix2.Text;
            Settings.Default.Suffix3 = txtSuffix3.Text;
            Settings.Default.ConditionColumn1 = (int) nudColumns1.Value;
            Settings.Default.ConditionColumn2 = (int) nudColumns2.Value;
            Settings.Default.ConditionColumn3 = (int) nudColumns3.Value;
            Settings.Default.ExportJson = chkExportData.Checked;
            Settings.Default.KeepOriginalName = chkOriginalFilename.Checked;
            Settings.Default.Save();
            MessageBox.Show("Settings Saved", "Save Settings", MessageBoxButtons.OK);
        }
        #endregion
    }
}