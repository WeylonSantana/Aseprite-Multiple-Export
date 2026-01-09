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
    private int _layerLoadToken = 0;
    private CancellationTokenSource? _layerLoadCts;

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
        nudScale.Value = Math.Max(nudScale.Minimum, Properties.Settings.Default.Scale);
        chkExportJson.Checked = Properties.Settings.Default.ExportJson;
        nudSplit.Value = Math.Max(nudSplit.Minimum, Properties.Settings.Default.SheetSplitCount);
        txtOutputName.Text = Properties.Settings.Default.CustomOutputName;
        chkFrameRange.Checked = Properties.Settings.Default.EnableFrameRange;
        nudFrameRangeMin.Value = Math.Max(nudFrameRangeMin.Minimum, Properties.Settings.Default.StartFrame);
        nudFrameRangeMax.Value = Math.Max(nudFrameRangeMax.Minimum, Properties.Settings.Default.EndFrame);

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
        if (_layers.Count == 0 && !EveryLayer)
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
        if (_layers.Count == 0 && EveryLayer)
        {
            string fileForLayers = _selectedLayerFile;
            if (string.IsNullOrEmpty(fileForLayers))
            {
                if (_files.Count == 1)
                    fileForLayers = _files[0];
                else
                {
                    _ = MessageBox.Show("Please select a single file to export layers from.", "Error - No File Selected!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            List<string> lines = LoadLayerLines(fileForLayers, AllLayers, CancellationToken.None);
            _layers.AddRange(lines);
            lstLayerList.Items.Clear();
        }

        foreach (string layer in _layers)
        {
            string exportedPath = Path.Combine($"{Scale}x", $"{layer}.png");
            if (ExportType == ExportType.EveryFrame) exportedPath = exportedPath.Replace(".png", "_{frame}.png");

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
        }
    }

    private void LstFilelist_SelectedIndexChanged(object sender, EventArgs e)
    {
        _files.Clear();
        foreach (object? item in lstFilelist.SelectedItems)
        {
            if (item.ToString() != default) _files.Add(item.ToString()!);
        }

        if (lstFilelist.SelectedItems.Count == 1)
        {
            string file = lstFilelist.SelectedItems[0]?.ToString() ?? string.Empty;
            if (!string.IsNullOrEmpty(file))
                QueueLayerLoad(file);
        }
        else
        {
            Interlocked.Increment(ref _layerLoadToken);
            _layerLoadCts?.Cancel();
            lstLayerList.Items.Clear();
            _selectedLayerFile = string.Empty;
            SetLayerLoadingState(false);
        }
    }

    private void QueueLayerLoad(string file)
    {
        if (string.IsNullOrEmpty(file))
            return;

        _layerLoadCts?.Cancel();
        _layerLoadCts?.Dispose();
        _layerLoadCts = new CancellationTokenSource();

        int token = Interlocked.Increment(ref _layerLoadToken);
        lstLayerList.Items.Clear();
        SetLayerLoadingState(true);
        lstDebug.Items.Insert(0, $"Loading layers of {file}...");

        _ = Task.Run(() =>
        {
            try
            {
                List<string> lines = LoadLayerLines(file, AllLayers, _layerLoadCts.Token);
                return (Lines: lines, Error: (Exception?)null);
            }
            catch (Exception ex)
            {
                return (Lines: new List<string>(), Error: ex);
            }
        }).ContinueWith(task =>
        {
            if (token != _layerLoadToken)
                return;

            if (task.Result.Error != null)
            {
                if (task.Result.Error is OperationCanceledException)
                {
                    lstDebug.Items.Insert(0, "Layer loading canceled.");
                }
                else
                {
                    lstDebug.Items.Insert(0, $"Failed to load layers: {task.Result.Error.Message}");
                }

                SetLayerLoadingState(false);
                return;
            }

            lstLayerList.Items.Clear();
            if (lstFilelist.SelectedItems.Count == 1 &&
                string.Equals(lstFilelist.SelectedItems[0]?.ToString(), file, StringComparison.Ordinal))
            {
                lstLayerList.Items.AddRange(task.Result.Lines.ToArray());
                _selectedLayerFile = file;
            }
            lstDebug.Items.Insert(0, "Layers loaded successfully.");
            SetLayerLoadingState(false);
        }, TaskScheduler.FromCurrentSynchronizationContext());
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

    private List<string> LoadLayerLines(string file, bool includeHidden, CancellationToken cancellationToken)
    {
        string outputName = file.Replace(Path.GetExtension(file), ".layers.json");
        string outputPath = Path.Combine(FolderPath, outputName);
        string scriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "scripts", "list_layers.lua");
        string includeHiddenValue = includeHidden ? "true" : "false";

        string finalCommand = $"-b \"{file}\" --script-param out=\"{outputPath.Replace("\\", "/")}\"";
        finalCommand += $" --script-param includeHidden={includeHiddenValue}";
        finalCommand += $" --script \"{scriptPath.Replace("\\", "/")}\"";
        _ = ProcessCommandCancelable(finalCommand, cancellationToken);

        if (!File.Exists(outputPath))
            throw new FileNotFoundException("Layer list file not found.", outputPath);

        string fileData = File.ReadAllText(outputPath);
        AsepriteJsonFile json = JObject.Parse(fileData).ToObject<AsepriteJsonFile>()!;
        File.Delete(outputPath);

        List<AsepriteLayer> layers = json.meta.layers;
        List<LayerNode> nodes = Utilities.BuildLayerTree(layers);
        return Utilities.FormatLayerTree(nodes);
    }

    private void SetLayerLoadingState(bool isLoading)
    {
        lblLayerList.Text = isLoading ? "Layer List (Loading...)" : "Layer List:";
        lstLayerList.Enabled = !isLoading;
    }

    private string ProcessCommandCancelable(string command, CancellationToken cancellationToken)
    {
        using Process process = new();
        process.StartInfo.FileName = "Aseprite.exe";
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.WorkingDirectory = FolderPath;
        process.StartInfo.Arguments = command;
        process.StartInfo.RedirectStandardOutput = true;

        _ = process.Start();
        using CancellationTokenRegistration registration = cancellationToken.Register(() =>
        {
            try
            {
                if (!process.HasExited)
                    process.Kill(true);
            }
            catch
            {
                // Best effort cancellation.
            }
        });

        string output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        cancellationToken.ThrowIfCancellationRequested();
        return output;
    }
}
