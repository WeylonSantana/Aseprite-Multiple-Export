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
    private string _selectedLayerFile = string.Empty;
    private int _layerLoadToken = 0;
    private CancellationTokenSource? _layerLoadCts;
    private bool _suppressFileSelectionChanged = false;
    private CancellationTokenSource? _exportCts;

    public Main()
    {
        InitializeComponent();
        lstDebug.Items.Insert(0, "Starting Aseprite Multiple Exporter...");
        string output = ProcessCommand("--version");

        if (output.Length == 0)
        {
            string error = "Aseprite not found. Please insert the path of Aseprite folder to the Path Variable of your OS.\n\n";
            error += "Make sure you can use 'Aseprite.exe --version' command in your terminal/command prompt.\n\n";
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
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.UseShellExecute = false;
        _ = process.Start();
        Task<string> outputTask = process.StandardOutput.ReadToEndAsync();
        Task<string> errorTask = process.StandardError.ReadToEndAsync();
        process.WaitForExit();
        string output = outputTask.Result;
        string error = errorTask.Result;
        if (!string.IsNullOrEmpty(error))
            output = $"{output}{Environment.NewLine}{error}";
        return output;
    }

    private string ProcessCommandCancelable(string command, CancellationToken cancellationToken)
    {
        using Process process = new();
        process.StartInfo.FileName = "Aseprite.exe";
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.WorkingDirectory = FolderPath;
        process.StartInfo.Arguments = command;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.UseShellExecute = false;

        _ = process.Start();
        Task<string> outputTask = process.StandardOutput.ReadToEndAsync();
        Task<string> errorTask = process.StandardError.ReadToEndAsync();
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

        process.WaitForExit();
        string output = outputTask.Result;
        string error = errorTask.Result;
        if (!string.IsNullOrEmpty(error))
            output = $"{output}{Environment.NewLine}{error}";
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
        lblSplit.Text = SheetExportType == SheetExportType.Rows ? "Rows" : "Columns";

        CustomOutputName = txtOutputName.Text;
        FrameRangeEnabled = chkFrameRange.Checked;
        StartFrame = (int)nudFrameRangeMin.Value;
        EndFrame = (int)nudFrameRangeMax.Value;

        nudFrameRangeMin.Enabled = FrameRangeEnabled;
        nudFrameRangeMax.Enabled = FrameRangeEnabled;

        // Saving settings
        if (_isLoading) return;

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
        Properties.Settings.Default.StartFrame = KeepChanges ? (int)nudFrameRangeMin.Value : 1;
        Properties.Settings.Default.EndFrame = KeepChanges ? (int)nudFrameRangeMax.Value : 2;

        Properties.Settings.Default.Save();
        if (KeepChanges) lstDebug.Items.Insert(0, "Settings saved successfully.");
    }

    private async void BtnExport_Click(object sender, EventArgs e)
    {
        if (_exportCts != null)
        {
            _exportCts.Cancel();
            SafeLog("Canceling export...");
            return;
        }

        UpdateForm();
        lstDebug.Items.Insert(0, "Exporting files...");
        if (_files.Count == 0)
        {
            _ = MessageBox.Show(
                "Please select at least one file to export.",
                "Error - No File Selected!",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
            return;
        }

        if (_layers.Count > 0 && string.IsNullOrEmpty(_selectedLayerFile))
        {
            _ = MessageBox.Show(
                "Please select a single file to export layers from.",
                "Error - No File Selected!",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
            return;
        }

        _exportCts = new CancellationTokenSource();
        SetExportingState(true);
        try
        {
            IReadOnlyCollection<string> selectedLayers = _layers.ToList();
            List<string> selectedFiles = _files.ToList();
            string selectedLayerFile = _selectedLayerFile;
            CancellationToken token = _exportCts.Token;

            await Task.Run(() =>
            {
                token.ThrowIfCancellationRequested();
                if (selectedLayers.Count > 0)
                {
                    ExportFile(selectedLayerFile, selectedLayers, token);
                }
                else
                {
                    foreach (string file in selectedFiles)
                    {
                        token.ThrowIfCancellationRequested();
                        ExportFile(file, [], token);
                    }
                }
            }, token);

            if (token.IsCancellationRequested)
                throw new OperationCanceledException(token);

            lstDebug.Items.Insert(0, $"Export completed successfully for type {ExportType}.");
            _ = MessageBox.Show(
                "Export completed successfully.",
                "Success - Export Completed!",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }
        catch (OperationCanceledException)
        {
            SafeLog("Export canceled.");
        }
        finally
        {
            SetExportingState(false);
            _exportCts.Dispose();
            _exportCts = null;
        }
    }

    private void ExportFile(string file, IReadOnlyCollection<string> layers, CancellationToken token)
    {
        if (string.IsNullOrEmpty(file)) return;

        bool hasLayers = layers.Count > 0;
        string scriptName;

        if (EveryLayer)
        {
            scriptName = ExportType == ExportType.EveryFrame
                ? "export_every_layer_frames.lua"
                : "export_sheet_per_layer.lua";
        }
        else if (ExportType == ExportType.EveryFrame)
        {
            scriptName = hasLayers ? "export_selected_layers.lua" : "export_every_frame.lua";
        }
        else
        {
            scriptName = hasLayers ? "export_selected_layers.lua" : "export_sprite_sheet.lua";
        }

        Dictionary<string, string> parameters = [];
        string baseName = Path.GetFileNameWithoutExtension(file);
        string scaleFolder = $"{Scale}x";
        bool isSheet = ExportType == ExportType.SpriteSheet;
        string frameRangeValue = string.Empty;

        string outPattern;
        if (!string.IsNullOrEmpty(CustomOutputName))
        {
            if (ExportType == ExportType.EveryFrame)
            {
                outPattern = EveryLayer
                    ? $"{CustomOutputName}_{{layer}}_{{frame}}.png"
                    : $"{CustomOutputName}_{{frame}}.png";
            }
            else
            {
                outPattern = EveryLayer
                    ? Path.Combine(CustomOutputName, "{layer}", $"{CustomOutputName}.png")
                    : $"{CustomOutputName}.png";
            }
        }
        else
        {
            if (ExportType == ExportType.EveryFrame)
            {
                outPattern = EveryLayer
                    ? Path.Combine(scaleFolder, "{layer}_{frame}.png")
                    : Path.Combine(scaleFolder, $"{baseName}_{{frame}}.png");
            }
            else
            {
                outPattern = EveryLayer
                    ? Path.Combine(scaleFolder, baseName, "{layer}.png")
                    : Path.Combine(scaleFolder, $"{baseName}.png");
            }
        }

        parameters["out"] = outPattern.Replace("\\", "/");

        if (scriptName == "export_selected_layers.lua")
            parameters["mode"] = ExportType == ExportType.EveryFrame ? "frames" : "sheet";

        if (EveryLayer || hasLayers)
        {
            string layerValue = string.Join("|", layers.Select(layer => layer.Trim().Replace("\\", "/")));
            if (!string.IsNullOrEmpty(layerValue))
                parameters["layers"] = layerValue;
        }

        if (AllLayers && scriptName != "export_selected_layers.lua")
            parameters["includeHidden"] = "true";

        if (FrameRangeEnabled)
        {
            int from = Math.Min(StartFrame, EndFrame);
            int to = Math.Max(StartFrame, EndFrame);
            frameRangeValue = $"{from},{to}";
            if (ExportType == ExportType.EveryFrame)
            {
                parameters["fromFrame"] = from.ToString();
                parameters["toFrame"] = to.ToString();
            }
            if (StartFrame > EndFrame)
                SafeLog("Frame range values were inverted. Using the corrected range.");
        }

        if (scriptName is "export_every_frame.lua" or "export_every_layer_frames.lua" or "export_selected_layers.lua")
            parameters["scale"] = Scale.ToString();

        if (isSheet)
        {
            string sheetType = Enum.GetName(typeof(SheetExportType), SheetExportType)?.ToLowerInvariant() ?? "packed";

            if (SheetExportType == SheetExportType.Rows)
            {
                sheetType = "columns";
                parameters["rows"] = SheetSplitCount.ToString();
            }

            if (SheetExportType == SheetExportType.Columns)
            {
                sheetType = "rows";
                parameters["columns"] = SheetSplitCount.ToString();
            }

            parameters["type"] = sheetType;

            if (ExportJson)
            {
                string dataPattern = Path.ChangeExtension(outPattern, ".json").Replace("\\", "/");
                parameters["data"] = dataPattern;
            }

            parameters["scale"] = Scale.ToString();
        }

        string scriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "scripts", scriptName).Replace("\\", "/");
        List<string> args = ["-b"];
        if (!string.IsNullOrEmpty(frameRangeValue))
            args.Add($"--frame-range {frameRangeValue}");
        args.Add($"\"{file}\"");

        foreach ((string key, string value) in parameters)
            if (!string.IsNullOrEmpty(value)) args.Add($"--script-param {key}=\"{value}\"");

        args.Add($"--script \"{scriptPath}\"");
        string commandLine = string.Join(" ", args);
        _ = ProcessCommandCancelable(commandLine, token);

        SafeLog($"Exported {file}.");
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

    private void LstFilelist_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_suppressFileSelectionChanged || !lstFilelist.Enabled) return;

        _files.Clear();
        foreach (object? item in lstFilelist.SelectedItems)
        {
            if (item.ToString() != default) _files.Add(item.ToString()!);
        }

        if (lstFilelist.SelectedItems.Count == 1)
        {
            string file = lstFilelist.SelectedItems[0]?.ToString() ?? string.Empty;
            if (!string.IsNullOrEmpty(file)) QueueLayerLoad(file);
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
        if (string.IsNullOrEmpty(file)) return;

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
            if (token != _layerLoadToken) return;

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
                lstLayerList.Items.AddRange([.. task.Result.Lines]);
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

        lstFilelist.Enabled = _layers.Count == 0;
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

        if (!File.Exists(outputPath)) throw new FileNotFoundException("Layer list file not found.", outputPath);

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


    private void SetExportingState(bool isExporting)
    {
        btnExport.Enabled = true;
        btnExport.Text = isExporting ? "Cancel" : "Export!";

        lstFilelist.Enabled = !isExporting;
        lstLayerList.Enabled = !isExporting && lblLayerList.Text != "Layer List (Loading...)";
        btnSearchFolder.Enabled = !isExporting;
        btnResetOutput.Enabled = !isExporting;
        btnResetFileListSelection.Enabled = !isExporting;
        btnResetLayerListSelection.Enabled = !isExporting;

        chkKeepChanges.Enabled = !isExporting;
        rdoEveryFrame.Enabled = !isExporting;
        rdoSpriteSheet.Enabled = !isExporting;
        chkEveryLayer.Enabled = !isExporting;
        chkAllLayers.Enabled = !isExporting;
        nudScale.Enabled = !isExporting;
        cmbSheetExportType.Enabled = !isExporting;
        nudSplit.Enabled = !isExporting;
        chkExportJson.Enabled = !isExporting;
        txtOutputName.Enabled = !isExporting;
        chkFrameRange.Enabled = !isExporting;
        nudFrameRangeMin.Enabled = !isExporting && chkFrameRange.Checked;
        nudFrameRangeMax.Enabled = !isExporting && chkFrameRange.Checked;
        UseWaitCursor = isExporting;
    }

    private void SafeLog(string message)
    {
        if (InvokeRequired)
        {
            _ = BeginInvoke(() => lstDebug.Items.Insert(0, message));
        }
        else
        {
            lstDebug.Items.Insert(0, message);
        }
    }
}
