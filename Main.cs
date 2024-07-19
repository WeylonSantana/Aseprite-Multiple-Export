using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace Aseprite_Multiple_Export;

public partial class Main : Form
{
    private bool KeepChanges;
    private string FolderPath = null!;
    private ExportType ExportType;
    private bool AllLayers;
    private bool EveryLayer;
    private new int Scale;
    private bool ExportJson;
    private SheetExportType SheetExportType;
    private int SheetSplitCount;
    private string CustomOutputName = null!;
    private bool FrameRangeEnabled;
    private int StartFrame;
    private int EndFrame;

    private readonly List<string> _files = [];
    private readonly List<string> _layers = [];
    private bool _isLoading = false;
    private string _lastSelectedItem = string.Empty;
    private string _selectedLayerFile = string.Empty;

    public Main()
    {
        InitializeComponent();
        lstDebug.Items.Insert(0, "Starting Aseprite Multiple Exporter...");
        string output = ProcessCommand("--version");
        if (output.Length == 0)
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
            _ = MessageBox.Show(error, "Error - Aseprite not Found!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Environment.Exit(0);
        }

        lstDebug.Items.Insert(0, output);
    }

    private string ProcessCommand(string command)
    {
        Process process = new();
        process.StartInfo.FileName = "Aseprite.exe";
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.WorkingDirectory = FolderPath;
        process.StartInfo.Arguments = command;
        process.StartInfo.RedirectStandardOutput = true;
        _ = process.Start();
        string output = process.StandardOutput.ReadToEnd();
        return output;
    }

    private void Main_Load(object sender, EventArgs e)
    {
        lstDebug.Items.Insert(0, "Loading settings...");
        _isLoading = true;
        chkKeepChanges.Checked = Properties.Settings.Default.KeepChanges;
        txtSearchFolder.Text = Properties.Settings.Default.FolderPath;
        rdoEveryFrame.Checked = Properties.Settings.Default.ExportType == (int)ExportType.EveryFrame;
        rdoSpriteSheet.Checked = Properties.Settings.Default.ExportType == (int)ExportType.SpriteSheet;
        chkAllLayers.Checked = Properties.Settings.Default.AllLayers;
        chkEveryLayer.Checked = Properties.Settings.Default.EveryLayer;
        nudScale.Value = Properties.Settings.Default.Scale;
        chkExportJson.Checked = Properties.Settings.Default.ExportJson;
        nudSplit.Value = Properties.Settings.Default.SheetSplitCount;
        txtOutputName.Text = Properties.Settings.Default.CustomOutputName;
        chkFrameRange.Checked = Properties.Settings.Default.EnableFrameRange;
        nudFrameRangeMin.Value = Properties.Settings.Default.StartFrame >= nudFrameRangeMin.Minimum
            ? Properties.Settings.Default.StartFrame : nudFrameRangeMin.Minimum;
        nudFrameRangeMax.Value = Properties.Settings.Default.EndFrame >= nudFrameRangeMax.Minimum
            ? Properties.Settings.Default.EndFrame : nudFrameRangeMax.Minimum;

        cmbSheetExportType.Items.AddRange(Enum.GetNames(typeof(SheetExportType)));
        cmbSheetExportType.SelectedIndex = Properties.Settings.Default.SheetExportType;

        UpdateForm();
        _isLoading = false;
        lstDebug.Items.Insert(0, "Settings loaded successfully.");
    }

    private void Main_Save()
    {
        if (_isLoading)
            return;

        Properties.Settings.Default.KeepChanges = KeepChanges;
        Properties.Settings.Default.FolderPath = KeepChanges ? FolderPath : string.Empty;
        Properties.Settings.Default.ExportType = KeepChanges ? (int)ExportType : default;
        Properties.Settings.Default.AllLayers = KeepChanges && chkAllLayers.Checked;
        Properties.Settings.Default.EveryLayer = KeepChanges && chkEveryLayer.Checked;
        Properties.Settings.Default.Scale = KeepChanges ? (int)nudScale.Value : default;
        Properties.Settings.Default.ExportJson = KeepChanges && chkExportJson.Checked;
        Properties.Settings.Default.SheetExportType = KeepChanges ? cmbSheetExportType.SelectedIndex : default;
        Properties.Settings.Default.SheetSplitCount = KeepChanges ? (int)nudSplit.Value : default;
        Properties.Settings.Default.CustomOutputName = KeepChanges ? txtOutputName.Text : string.Empty;
        Properties.Settings.Default.EnableFrameRange = KeepChanges && chkFrameRange.Checked;
        Properties.Settings.Default.StartFrame = KeepChanges ? (int)nudFrameRangeMax.Value : 1;
        Properties.Settings.Default.EndFrame = KeepChanges ? (int)nudFrameRangeMin.Value : 2;

        Properties.Settings.Default.Save();
        if (KeepChanges)
            lstDebug.Items.Insert(0, "Settings saved successfully.");
    }

    private void UpdateForm()
    {
        KeepChanges = chkKeepChanges.Checked;

        //Updating file list
        FolderPath = txtSearchFolder.Text;
        if (!string.IsNullOrEmpty(FolderPath) && lstFilelist.Items.Count == 0)
        {
            if (Directory.Exists(FolderPath))
            {
                List<string> files =
                [
                    .. Directory.EnumerateFiles(FolderPath, "*.ase").ToList(),
                    .. Directory.EnumerateFiles(FolderPath, "*.aseprite").ToList(),
                ];

                if (files.Count > 0)
                {
                    foreach (string file in files)
                    {
                        _ = lstFilelist.Items.Add(Path.GetFileName(file));
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
        Scale = (int)nudScale.Value;

        SheetExportType = (SheetExportType)cmbSheetExportType.SelectedIndex;
        SheetSplitCount = (int)nudSplit.Value;
        ExportJson = chkExportJson.Checked;

        grpSpritesheetOptions.Visible = ExportType == ExportType.SpriteSheet;
        nudSplit.Visible = SheetExportType is SheetExportType.Rows or SheetExportType.Columns;
        lblSplit.Visible = SheetExportType is SheetExportType.Rows or SheetExportType.Columns;
        lblSplit.Text = SheetExportType == SheetExportType.Rows ? "Columns" : "Rows";

        CustomOutputName = txtOutputName.Text;
        FrameRangeEnabled = chkFrameRange.Checked;
        StartFrame = (int)nudFrameRangeMin.Value;
        EndFrame = (int)nudFrameRangeMax.Value;

        nudFrameRangeMin.Enabled = FrameRangeEnabled;
        nudFrameRangeMax.Enabled = FrameRangeEnabled;

        Main_Save();
    }

    private void BtnExport_Click(object sender, EventArgs e)
    {
        lstDebug.Items.Insert(0, "Exporting files...");
        if (_files.Count == 0)
        {
            _ = MessageBox.Show("Please select at least one file to export.", "Error - No File Selected!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        // export files if we have no layers or we want to export specific layers
        if (_layers.Count == 0 || (_layers.Count > 0 && !EveryLayer))
        {
            ExportFiles();
        }
        else
        {
            ExportLayers();
        }

        lstDebug.Items.Insert(0, $"Export completed successfully for type {ExportType}.");
        _ = MessageBox.Show("Export completed successfully.", "Success - Export Completed!", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void ExportFiles()
    {
        foreach (string file in _files)
        {
            if (file == default)
                continue;

            string command = BuildCommand(file);
            _ = ProcessCommand(string.Join(" ", command));

            lstDebug.Items.Insert(0, $"Exported {file} to {UpdateOutputName(file)} successfully.");
        }
    }

    private void ExportLayers()
    {
        foreach (string layer in _layers)
        {
            string exportedPath = Path.Combine($"{Scale}x", $"{layer}.png");
            if (ExportType == ExportType.EveryFrame)
                exportedPath = exportedPath.Replace(".png", "_{frame}.png");

            string command = BuildCommand(_selectedLayerFile, layer);
            _ = ProcessCommand(string.Join(" ", command));

            lstDebug.Items.Insert(0, $"Exported layer {layer} from {_selectedLayerFile} to {exportedPath} successfully.");
        }
    }

    private string BuildCommand(string file, string layer = "")
    {
        List<string> command = ["Aseprite", "-b"];

        if (_layers.Count == 0 && AllLayers)
        {
            command.Add("--all-layers");
        }

        if (ExportType == ExportType.SpriteSheet && FrameRangeEnabled && StartFrame < EndFrame)
        {
            command.Add($"--frame-range {StartFrame},{EndFrame}");
        }
        else if (ExportType == ExportType.SpriteSheet && FrameRangeEnabled && StartFrame >= EndFrame)
        {
            lstDebug.Items.Insert(0, "Frame range is invalid. Please check the values. Ignoring frame range...");
        }

        if (!string.IsNullOrEmpty(layer) && _layers.Count == 0)
        {
            command.Add($"--layer \"{layer.Replace("\\", "/")}\"");
        }
        else if (_layers.Count > 0)
        {
            if (!string.IsNullOrEmpty(layer) && EveryLayer)
            {
                command.Add($"--layer \"{layer.Replace("\\", "/")}\"");
            }
            else if (!EveryLayer)
            {
                foreach (string l in _layers)
                {
                    command.Add($"--layer \"{l.Replace("\\", "/")}\"");
                }
            }
        }

        command.Add($"\"{file}\"");
        command.Add($"--scale {Scale}");
        command.Add(ExportType == ExportType.EveryFrame ? "--save-as" : "--sheet");

        string outputName = UpdateOutputName(file, layer);
        command.Add($"\"{outputName}\"");

        if (ExportType == ExportType.SpriteSheet)
        {
            command.Add($"--sheet-type {Enum.GetName(typeof(SheetExportType), SheetExportType)?.ToLowerInvariant()}");

            if (SheetExportType == SheetExportType.Rows)
                command.Add($"--sheet-columns {SheetSplitCount}");

            if (SheetExportType == SheetExportType.Columns)
                command.Add($"--sheet-rows {SheetSplitCount}");

            if (ExportJson)
                command.Add($"--list-layers --list-tags --data \"{outputName.Replace(".png", ".json")}\" --format json-array");
        }

        return string.Join(" ", command);
    }

    private string UpdateOutputName(string filename, string layer = "")
    {
        if (!string.IsNullOrEmpty(CustomOutputName))
        {
            if (ExportType == ExportType.EveryFrame)
            {
                return EveryLayer && string.IsNullOrEmpty(layer)
                    ? $"{CustomOutputName}_{{layer}}_{{frame}}.png"
                    : string.IsNullOrEmpty(layer) ? $"{CustomOutputName}_{{frame}}.png" : $"{CustomOutputName}_{layer}_{{frame}}.png";
            }
            else
            {
                if (!string.IsNullOrEmpty(layer))
                {
                    return Path.Combine(CustomOutputName, layer, $"{CustomOutputName}.png");
                }
            }

            return $"{CustomOutputName}.png";
        }

        string ext = Path.GetExtension(filename);
        filename = filename.Replace(ext, ".png");
        ext = ".png";

        if (ExportType == ExportType.EveryFrame)
        {
            filename = EveryLayer && string.IsNullOrEmpty(layer)
                ? filename.Replace(filename, "{layer}_{frame}.png")
                : string.IsNullOrEmpty(layer) ? filename.Replace(ext, "_{frame}.png") : $"{layer}_{{frame}}.png";
        }
        else
        {
            if (!string.IsNullOrEmpty(layer))
            {
                filename = Path.Combine(Path.GetFileNameWithoutExtension(filename), layer);
                filename += ext;
            }
        }

        filename = Path.Combine($"{Scale}x", filename);
        return filename;
    }

    private void BasicControl_Changed(object sender, EventArgs e) => UpdateForm();

    private void BtnSearchFolder_Click(object sender, EventArgs e)
    {
        FolderBrowserDialog openFolder = new();
        if (openFolder.ShowDialog() == DialogResult.OK)
        {
            lstFilelist.Items.Clear();
            txtSearchFolder.Text = openFolder.SelectedPath;
            UpdateForm();
        }
    }

    private void LstFilelist_MouseDown(object sender, MouseEventArgs e)
    {
        Point mousePos = e.Location;
        int index = lstFilelist.IndexFromPoint(mousePos);

        if (index != ListBox.NoMatches)
        {
            _lastSelectedItem = lstFilelist.Items[index].ToString() ?? string.Empty;
            seeLayersMenuItem.Text = $"See Layers of {_lastSelectedItem}";
        }
        else
        {
            seeLayersMenuItem.Text = "Select a file to see layers";
        }
    }

    private void LstFilelist_SelectedIndexChanged(object sender, EventArgs e)
    {
        _files.Clear();
        foreach (object? item in lstFilelist.SelectedItems)
        {
            if (item.ToString() != default)
                _files.Add(item.ToString()!);
        }
    }

    private void SeeLayersMenuItem_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(_lastSelectedItem))
            return;

        //lets create only the json file for we see the layers
        lstDebug.Items.Insert(0, $"Getting layers of {_lastSelectedItem}...");
        string outputName = _lastSelectedItem.Replace(Path.GetExtension(_lastSelectedItem), ".json");
        string finalCommand = $"-b --list-layers";
        if (AllLayers)
            finalCommand += " --all-layers ";
        finalCommand += $" {_lastSelectedItem} --data {outputName} --format json-array";
        lstDebug.Items.Insert(0, "Generating json file...");
        _ = ProcessCommand(finalCommand);

        lstDebug.Items.Insert(0, "Json file generated successfully.");
        string fileData = File.ReadAllText(Path.Combine(FolderPath, outputName));
        AsepriteJsonFile json = JObject.Parse(fileData).ToObject<AsepriteJsonFile>()!;
        // remove file
        File.Delete(Path.Combine(FolderPath, outputName));
        lstDebug.Items.Insert(0, "Json file deleted successfully.");

        List<AsepriteLayer> layers = json.meta.layers;
        List<LayerNode> nodes = Utilities.BuildLayerTree(layers);
        List<string> lines = Utilities.FormatLayerTree(nodes);
        lstDebug.Items.Insert(0, "Layers obtained successfully.");
        lstLayerList.Items.Clear();
        lstLayerList.Items.AddRange(lines.ToArray());
        _selectedLayerFile = _lastSelectedItem;
        lstDebug.Items.Insert(0, "Layers loaded successfully.");
    }

    private void LstLayerList_SelectedIndexChanged(object sender, EventArgs e)
    {
        _layers.Clear();
        foreach (object? item in lstLayerList.SelectedItems)
        {
            if(item.ToString() != default) _layers.Add(item.ToString()!);
        }

        if (_layers.Count == 0)
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

    private void BtnResetOutput_Click(object sender, EventArgs e) => lstDebug.Items.Clear();

    private void BtnResetFileListSelection_Click(object sender, EventArgs e) => lstFilelist.SelectedItems.Clear();

    private void BtnResetLayerListSelection_Click(object sender, EventArgs e) => lstLayerList.SelectedItems.Clear();
}
