using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace Aseprite_Multiple_Export
{
    public partial class Main : Form
    {
        private bool KeepChanges;
        private string FolderPath;
        private ExportType ExportType;
        private bool AllLayers;
        private bool EveryLayer;
        private int Scale;
        private bool ExportJson;
        private SheetExportType SheetExportType;
        private int SheetSplitCount;

        private Process process;

        private List<string> _files = new List<string>();
        private List<string> _layers = new List<string>();
        private bool _isLoading = false;
        private string _lastSelectedItem = string.Empty;
        private string _selectedLayerFile = string.Empty;

        public Main()
        {
            InitializeComponent();
            lstDebug.Items.Add("Starting Aseprite Multiple Exporter...");
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
            lstDebug.Items.Add(output);
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
            lstDebug.Items.Add("Loading settings...");
            _isLoading = true;
            chkKeepChanges.Checked = Properties.Settings.Default.KeepChanges;
            txtSearchFolder.Text = Properties.Settings.Default.FolderPath;
            rdoEveryFrame.Checked = Properties.Settings.Default.ExportType == (int) ExportType.EveryFrame;
            rdoSpriteSheet.Checked = Properties.Settings.Default.ExportType == (int) ExportType.SpriteSheet;
            chkAllLayers.Checked = Properties.Settings.Default.AllLayers;
            chkEveryLayer.Checked = Properties.Settings.Default.EveryLayer;
            nudScale.Value = Properties.Settings.Default.Scale;
            chkExportJson.Checked = Properties.Settings.Default.ExportJson;
            nudSplit.Value = Properties.Settings.Default.SheetSplitCount;

            cmbSheetExportType.Items.AddRange(Enum.GetNames(typeof(SheetExportType)));
            cmbSheetExportType.SelectedIndex = Properties.Settings.Default.SheetExportType;

            UpdateForm();
            _isLoading = false;
            lstDebug.Items.Add("Settings loaded successfully.");
        }

        private void Main_Save()
        {
            if ( _isLoading )
                return;

            Properties.Settings.Default.KeepChanges = KeepChanges;
            Properties.Settings.Default.FolderPath = KeepChanges ? FolderPath : string.Empty;
            Properties.Settings.Default.ExportType = KeepChanges ? (int) ExportType : default;
            Properties.Settings.Default.AllLayers = KeepChanges ? chkAllLayers.Checked : default;
            Properties.Settings.Default.EveryLayer = KeepChanges ? chkEveryLayer.Checked : default;
            Properties.Settings.Default.Scale = KeepChanges ? (int) nudScale.Value : default;
            Properties.Settings.Default.ExportJson = KeepChanges ? chkExportJson.Checked : default;
            Properties.Settings.Default.SheetExportType = KeepChanges ? cmbSheetExportType.SelectedIndex : default;
            Properties.Settings.Default.SheetSplitCount = KeepChanges ? (int) nudSplit.Value : default;

            Properties.Settings.Default.Save();
            if ( KeepChanges )
                lstDebug.Items.Add("Settings saved successfully.");
        }

        private void UpdateForm()
        {
            KeepChanges = chkKeepChanges.Checked;

            //Updating file list
            FolderPath = txtSearchFolder.Text;
            if ( !string.IsNullOrEmpty(FolderPath) && lstFilelist.Items.Count == 0 )
            {
                if ( Directory.Exists(FolderPath) )
                {
                    List<string> files =
                    [
                        .. Directory.EnumerateFiles(FolderPath, "*.ase").ToList(),
                        .. Directory.EnumerateFiles(FolderPath, "*.aseprite").ToList(),
                    ];

                    if ( files.Count > 0 )
                    {
                        foreach ( var file in files )
                        {
                            lstFilelist.Items.Add(Path.GetFileName(file));
                        }
                    }
                }
                else
                {
                    txtSearchFolder.Text = "";
                    FolderPath = "";
                    lstFilelist.Items.Clear();
                }
            }

            ExportType = rdoEveryFrame.Checked ? ExportType.EveryFrame : ExportType.SpriteSheet;

            AllLayers = chkAllLayers.Checked;
            EveryLayer = chkEveryLayer.Checked;
            Scale = (int) nudScale.Value;

            SheetExportType = (SheetExportType) cmbSheetExportType.SelectedIndex;
            SheetSplitCount = (int) nudSplit.Value;
            ExportJson = chkExportJson.Checked;

            grpSpritesheetOptions.Visible = ExportType == ExportType.SpriteSheet;
            nudSplit.Visible = SheetExportType == SheetExportType.Rows || SheetExportType == SheetExportType.Columns;
            lblSplit.Visible = SheetExportType == SheetExportType.Rows || SheetExportType == SheetExportType.Columns;
            lblSplit.Text = SheetExportType == SheetExportType.Rows ? "Columns" : "Rows";

            Main_Save();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            lstDebug.Items.Add("Exporting files...");
            if ( _files.Count == 0 )
            {
                MessageBox.Show("Please select at least one file to export.", "Error - No File Selected!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if ( _layers.Count == 0 || (_layers.Count > 0 && !EveryLayer) )
            {
                foreach ( var file in _files )
                {
                    List<string> command = new List<string>() { "Aseprite", "-b" };

                    string filename = file.ToString();
                    if ( filename == default )
                        continue;

                    if ( AllLayers && _layers.Count == 0 )
                    {
                        command.Add("--all-layers");
                    }
                    else if ( _layers.Count > 0 && !EveryLayer )
                    {
                        foreach (var layer in _layers)
                        {
                            command.Add($"--layer \"{layer.Replace("\\", "/")}\"");
                        }
                    }

                    command.Add($"\"{filename}\"");
                    command.Add($"--scale {Scale}");
                    command.Add(ExportType == ExportType.EveryFrame ? "--save-as" : "--sheet");

                    string outputName = UpdateOutputName(filename);
                    command.Add($"\"{outputName}\"");

                    if ( ExportType  == ExportType.SpriteSheet)
                    {
                        command.Add($"--sheet-type {Enum.GetName(typeof(SheetExportType), SheetExportType).ToLowerInvariant()}");
                        if (SheetExportType == SheetExportType.Rows)
                            command.Add($"--sheet-columns {SheetSplitCount}");

                        if (SheetExportType == SheetExportType.Columns)
                            command.Add($"--sheet-rows {SheetSplitCount}");

                        if (ExportJson)
                            command.Add($"--list-layers --list-tags --data \"{outputName.Replace(".png", ".json")}\" --format json-array");
                    }

                    ProcessCommand(string.Join(" ", command));
                    lstDebug.Items.Add($"Exported {filename} to {outputName} successfully.");
                }
            }
            else
            {
                foreach ( var layer in _layers )
                {
                    List<string> command = new List<string>() { "Aseprite", "-b" };
                    var exportedPath = Path.Combine($"{Scale}x", $"{layer}.png");
                    if ( ExportType == ExportType.EveryFrame )
                        exportedPath = exportedPath.Replace(".png", "_{frame}.png");

                    command.Add($"--layer \"{layer.Replace("\\", "/")}\"");
                    command.Add($"\"{_selectedLayerFile}\"");
                    command.Add($"--scale {Scale}");
                    command.Add(ExportType == ExportType.EveryFrame ? "--save-as" : "--sheet");
                    command.Add($"\"{exportedPath}\"");

                    if ( ExportType == ExportType.SpriteSheet )
                    {
                        command.Add($"--sheet-type {Enum.GetName(typeof(SheetExportType), SheetExportType).ToLowerInvariant()}");
                        if ( SheetExportType == SheetExportType.Rows )
                            command.Add($"--sheet-columns {SheetSplitCount}");

                        if ( SheetExportType == SheetExportType.Columns )
                            command.Add($"--sheet-rows {SheetSplitCount}");

                        if ( ExportJson )
                            command.Add($"--list-layers --list-tags --data \"{exportedPath.Replace(".png", ".json")}\" --format json-array");
                    }

                    lstDebug.Items.Add($"Exported layer {layer} from {_selectedLayerFile} to {exportedPath} successfully.");
                    ProcessCommand(string.Join(" ", command));
                }
            }

            lstDebug.Items.Add($"Export completed successfully for type {ExportType}.");
            MessageBox.Show("Export completed successfully.", "Success - Export Completed!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private string UpdateOutputName(string filename)
        {
            string ext = Path.GetExtension(filename);
            filename = filename.Replace(ext, ".png");
            ext = ".png";

            if ( ExportType == ExportType.EveryFrame )
            {
                if ( EveryLayer )
                    filename = filename.Replace(filename, "{layer}_{frame}.png");
                else
                    filename = filename.Replace(ext, "_{frame}.png");
            }

            filename = Path.Combine($"{Scale}x", filename);
            return filename;
        }

        private void basicControl_Changed(object sender, EventArgs e)
        {
            UpdateForm();
        }

        private void btnSearchFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog openFolder = new FolderBrowserDialog();
            if ( openFolder.ShowDialog() == DialogResult.OK )
            {
                lstFilelist.Items.Clear();
                txtSearchFolder.Text = openFolder.SelectedPath;
                UpdateForm();
            }
        }

        private void lstFilelist_MouseDown(object sender, MouseEventArgs e)
        {
            Point mousePos = e.Location;
            int index = lstFilelist.IndexFromPoint(mousePos);

            if ( index != ListBox.NoMatches )
            {
                _lastSelectedItem = lstFilelist.Items[index].ToString();
                seeLayersMenuItem.Text = $"See Layers of {_lastSelectedItem}";
            }
            else
            {
                seeLayersMenuItem.Text = "Select a file to see layers";
            }
        }

        private void lstFilelist_SelectedIndexChanged(object sender, EventArgs e)
        {
            _files.Clear();
            foreach ( var item in lstFilelist.SelectedItems )
            {
                _files.Add(item.ToString());
            }
        }

        private void seeLayersMenuItem_Click(object sender, EventArgs e)
        {
            if ( string.IsNullOrEmpty(_lastSelectedItem) )
                return;

            //lets create only the json file for we see the layers
            lstDebug.Items.Add($"Getting layers of {_lastSelectedItem}...");
            string outputName = _lastSelectedItem.Replace(Path.GetExtension(_lastSelectedItem), ".json");
            string finalCommand = $"-b --list-layers";
            if ( AllLayers )
                finalCommand += " --all-layers ";
            finalCommand += $" {_lastSelectedItem} --data {outputName} --format json-array";
            lstDebug.Items.Add("Generating json file...");
            ProcessCommand(finalCommand);

            lstDebug.Items.Add("Json file generated successfully.");
            var fileData = File.ReadAllText(Path.Combine(FolderPath, outputName));
            AsepriteJsonFile json = JObject.Parse(fileData).ToObject<AsepriteJsonFile>();
            // remove file
            File.Delete(Path.Combine(FolderPath, outputName));
            lstDebug.Items.Add("Json file deleted successfully.");

            List<AsepriteLayer> layers = json.meta.layers;
            List<LayerNode> nodes = Utilities.BuildLayerTree(layers);
            List<string> lines = Utilities.FormatLayerTree(nodes);
            lstDebug.Items.Add("Layers obtained successfully.");
            lstLayerList.Items.Clear();
            lstLayerList.Items.AddRange(lines.ToArray());
            _selectedLayerFile = _lastSelectedItem;
            lstDebug.Items.Add("Layers loaded successfully.");
        }

        private void lstLayerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            _layers.Clear();
            foreach ( var item in lstLayerList.SelectedItems )
            {
                _layers.Add(item.ToString());
            }

            if ( _layers.Count == 0 )
            {
                lstFilelist.Enabled = true;
            }
            else
            {
                lstFilelist.SelectedItems.Clear();
                lstFilelist.SelectedItem = _selectedLayerFile;
                lstFilelist.Enabled = false;
            }
        }

        private void btnResetOutput_Click(object sender, EventArgs e)
        {
            lstDebug.Items.Clear();
        }
    }
}
