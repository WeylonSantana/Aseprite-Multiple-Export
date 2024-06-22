import { Component } from 'react';
import { open } from '@tauri-apps/api/dialog';
import { exists, readTextFile, writeTextFile, BaseDirectory, createDir, readDir, FileEntry } from '@tauri-apps/api/fs';
import { Command } from '@tauri-apps/api/shell';
import { dialog, process } from '@tauri-apps/api';

interface AppState {
  // Config State
  keepConfig: boolean;
  fileListPath: string;
  exportType: ExportTypes;
  scale: number;
  options: {
    exportJson: boolean;
    sheetType: SheetTypes;
    sheetColumns?: number;
    sheetRows?: number;
    splitLayers?: boolean;
  };

  // Local State
  fileList?: FileEntry[];
  layerList?: string[];
  selectedFiles?: FileEntry[];
  selectedLayers?: string[];
  exportLoading?: boolean;
  layersLoading?: boolean;
}

enum ExportTypes {
  EveryFrame,
  SheetExport,
}

enum SheetTypes {
  Horizontal,
  Vertical,
  Rows,
  Columns,
  Packed,
}

export default class App extends Component<any, AppState> {
  constructor(props: any) {
    super(props);
    this.state = {
      keepConfig: false,
      fileListPath: '',
      exportType: ExportTypes.EveryFrame,
      scale: 1,
      options: {
        exportJson: false,
        sheetType: SheetTypes.Horizontal,
      },
    };

    this.checkForAseprite = this.checkForAseprite.bind(this);
    this.updateConfig = this.updateConfig.bind(this);
    this.searchAsepriteFiles = this.searchAsepriteFiles.bind(this);
    this.loadFileList = this.loadFileList.bind(this);
    this.loadLayerList = this.loadLayerList.bind(this);
    this.handleExport = this.handleExport.bind(this);
  }

  async checkForAseprite() {
    try {
      const command = new Command('Aseprite', ['--version']);
      await command.spawn();
      return true;
    } catch (error) {
      if (error === 'program not found') {
        await dialog.message(
          `Please register Aseprite in your system PATH variable.
          Step 1: Open Aseprite installation folder.
          Step 2: Copy the path of the folder.
          Step 3: Type 'Variables' in the Windows search bar.
          Step 4: Click on 'Edit the system environment variables'.
          Step 5: Click on 'Environment Variables' on the end.
          Step 6: Click on 'Path' on the 'System variables' list.
          Step 7: Click on 'Edit'.
          Step 8: Click on 'New'.
          Step 9: Paste the Aseprite folder path.
          Must be something like:
            'C:\\Program Files\\Aseprite'.

          Step 10: Click on 'Ok'.
          Step 11: Click on 'Ok' again.
          Step 12: Restart this program and try again.
          `,
          { type: 'error', title: 'Aseprite Multiple Export - Aseprite not found!' }
        );

        await process.exit(1);
        return false;
      }

      console.error(error);
      return false;
    }
  }

  async componentDidMount() {
    try {
      if (!(await this.checkForAseprite())) return;

      if (!(await exists('', { dir: BaseDirectory.AppConfig }))) {
        await createDir('', { dir: BaseDirectory.AppConfig });
        await writeTextFile('config.json', JSON.stringify({}), { dir: BaseDirectory.AppConfig });
      }

      var state = JSON.parse(await readTextFile('config.json', { dir: BaseDirectory.AppConfig }));
      this.setState({
        keepConfig: state?.keepConfig ?? false,
        fileListPath: state?.fileListPath ?? '',
        exportType: state?.exportType ?? ExportTypes.EveryFrame,
        scale: state?.scale ?? 1,
        options: state?.options ?? { exportJson: false, sheetType: SheetTypes.Horizontal },
      });

      if (state?.fileListPath && (await exists(state.fileListPath))) {
        this.loadFileList(state.fileListPath);
      }
    } catch (error) {
      console.error(error);
    }
  }

  async updateConfig<K extends keyof AppState>(key: K, value: AppState[K]) {
    try {
      // Using a type-safe way to update the state
      this.setState(
        (prevState: AppState) => ({
          ...prevState,
          [key]: value,
        }),
        async () => {
          if (this.state.keepConfig) {
            await writeTextFile(
              'config.json',
              JSON.stringify(
                {
                  keepConfig: true,
                  fileListPath: this.state.fileListPath,
                  exportType: this.state.exportType,
                  scale: this.state.scale,
                  options: this.state.options,
                },
                undefined,
                2
              ),
              { dir: BaseDirectory.AppConfig }
            );
          }

          if (!this.state.keepConfig || (key === 'keepConfig' && !value)) {
            await writeTextFile('config.json', JSON.stringify({}), { dir: BaseDirectory.AppConfig });
          }
        }
      );
    } catch (error) {
      console.error(error);
    }
  }

  async searchAsepriteFiles() {
    try {
      var dir = await open({ multiple: false, directory: true });
      if (!dir || Array.isArray(dir)) return;
      this.updateConfig('fileListPath', dir);
      this.loadFileList(dir);
    } catch (error) {
      console.error(error);
    }
  }

  async loadFileList(path: string) {
    try {
      var fileList = (await readDir(path)).filter((f) => f.name?.endsWith('.ase') || f.name?.endsWith('.aseprite'));
      if (!fileList?.length) return;
      this.setState({ fileList, selectedFiles: [fileList[0]] }, async () => {
        // Load Layer List
        await this.loadLayerList();
      });
    } catch (error) {
      console.error(error);
    }
  }

  async loadLayerList() {
    try {
      this.setState({ layerList: this.state.layerList ?? [], layersLoading: true, selectedLayers: undefined });
      const layerList: string[] = [];

      if (this.state.selectedFiles?.length === 0) return this.setState({ layerList: [], layersLoading: false });

      this.state.selectedFiles?.forEach(async (file) => {
        const command = new Command('Aseprite', ['-b', '--list-layers', file.path]);
        command.stdout.on('data', (data) => layerList.push(`${file?.name?.split('.')[0] ?? 'Err'} - ${data}`));
        command.on('close', () => this.setState({ layerList: layerList.sort((a, b) => a.localeCompare(b)), layersLoading: false }));
        await command.spawn();
      });
    } catch (error) {
      console.error(error);
    }
  }

  async handleExport() {
    const { selectedFiles } = this.state;
    if (!selectedFiles?.length) {
      await dialog.message('Please select a file to export.', { type: 'error', title: 'Aseprite Multiple Export - No file selected!' });
      return;
    }

    this.setState({ exportLoading: true });
    const promisses: Promise<void>[] = [];
    selectedFiles.forEach((_, index) => promisses.push(this.exportFile(index)));
    await Promise.all(promisses);
    dialog.message('Exported all selected files successfully!', { type: 'info', title: 'Aseprite Multiple Export - Exported!' });
    this.setState({ exportLoading: false });
  }

  async exportFile(index: number) {
    const { fileListPath, selectedFiles, selectedLayers, exportType, scale, options } = this.state;
    const { exportJson, sheetType, sheetColumns, sheetRows, splitLayers } = options;
    if (!selectedFiles?.length) return;

    const file = selectedFiles[index];
    if (!file?.name) return;
    const outputName = this.getAsepriteOutputName(index);

    const exportTypesArgs: string[][] = [
      // export every frame as a separate file
      ['-b', file.name, '--save-as', `${outputName}`],
      // export sheet
      ['-b', file.name, '--sheet', `${outputName}`],
    ];

    const args = exportTypesArgs[exportType];

    selectedLayers?.forEach((layer) => {
      var layerArg = ['--layer', layer.split(' - ')[1].trim()];
      // we need to insert the layer argument before the save-as or sheet argument
      args.splice(1, 0, ...layerArg);
    });

    if (exportType === ExportTypes.SheetExport) {
      args.push('--sheet-type', SheetTypes[sheetType].toLowerCase());
      if (sheetType === SheetTypes.Columns && sheetRows) args.push('--sheet-rows', sheetRows.toString());
      if (sheetType === SheetTypes.Rows && sheetColumns) args.push('--sheet-columns', sheetColumns.toString());

      if (exportJson) args.push('--data', `${outputName.replace('.png', '.json')}`, '--format', 'json-array', '--list-layers', '--list-tags');
    }

    // both options (save as and sheet)
    if (splitLayers) args.splice(1, 0, '--split-layers');
    args.push('--scale', `${scale}`);
    console.log('Exporting with args:', args.join(' '));

    try {
      const command = new Command('Aseprite', args, { cwd: fileListPath });
      command.on('close', () => {});
      await command.spawn();
    } catch (error) {
      console.error(error);
    }
  }

  getAsepriteOutputName(index: number) {
    const { selectedFiles, scale } = this.state;
    if (!selectedFiles?.length) return 'ERR';

    const selectedFile = selectedFiles[index];
    if (selectedFile.name) {
      const ext = selectedFile.name.split('.').pop();
      return `${scale}x/${selectedFile.name.replace(`.${ext}`, '.png')}`;
    }

    const fileName = selectedFile.path.split('\\').pop();
    if (fileName && fileName.length) {
      const ext = fileName.split('.').pop();
      return `${scale}x/${fileName.replace(`.${ext}`, '.png')}`;
    }

    return 'ERR';
  }

  render() {
    const { keepConfig, fileListPath, fileList, layerList, exportType, options } = this.state;
    const { selectedFiles, selectedLayers } = this.state;
    const { exportLoading, layersLoading } = this.state;
    const { exportJson, sheetType, splitLayers } = options;

    return (
      <div className='overflow-hidden'>
        {exportLoading && (
          <div className='absolute flex gap-1 items-center justify-center z-10 w-[100vw] h-[100vh] bg-[rgba(0,0,0,0.5)]'>
            <label className='text-2xl label'>Exporting</label>
            <div className='mt-4 loading loading-dots'></div>
          </div>
        )}

        {/* Keep Config Checkbox */}
        <div className='absolute top-0 flex items-center gap-2 mt-6 ml-4 right-4'>
          <input id='keep-config' type='checkbox' className='checkbox' checked={keepConfig} onChange={() => this.updateConfig('keepConfig', !keepConfig)} />
          <label htmlFor='keep-config'>Keep Changes?</label>
        </div>

        {/* Aseprite Path Filelist Search */}
        <div className='flex items-center gap-2 mt-4 ml-4'>
          <label className='label'>Ase Files Path:</label>
          <input className='input input-sm w-[700px]' type='text' value={fileListPath} disabled />
          <button className='btn btn-sm btn-success' onClick={this.searchAsepriteFiles}>
            Search .ase/.aseprite files
          </button>
        </div>

        {/* Aseprite File List */}
        {fileList?.length && (
          // File List
          <div className='flex gap-2'>
            <div className='min-w-[800px] max-h-[400px] bg-base-300 rounded-md mt-2 ml-4 overflow-y-auto'>
              <table className='table w-full'>
                <thead>
                  <tr>
                    <th className='text-base'>File Name</th>
                  </tr>
                </thead>
                <tbody>
                  {fileList?.map((file, index) => (
                    <tr key={index} className={`${selectedFiles?.includes(file) ? 'bg-base-200' : ''}`}>
                      <td
                        className='cursor-pointer select-none'
                        onClick={() => {
                          if (layersLoading || !selectedFiles) return;

                          const newFiles = selectedFiles;

                          // we are trying to add?
                          if (!newFiles.includes(file)) {
                            newFiles.push(file);
                          } else {
                            newFiles.splice(newFiles.indexOf(file), 1);
                          }

                          this.setState({ selectedFiles: newFiles }, async () => {
                            try {
                              await this.loadLayerList();
                            } catch (error) {
                              console.error(error);
                            }
                          });
                        }}>
                        {file.name}
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>

            {/* Layers List */}
            <div className='min-w-[300px] max-h-[400px] bg-base-300 rounded-md mt-2 ml-4 overflow-y-auto'>
              {layersLoading ? (
                <div className='flex items-center justify-center h-full'>
                  Loading Layers
                  <span className='ml-2 loading loading-dots'></span>
                </div>
              ) : (
                <>
                  {!layerList?.length ? (
                    <div className='flex items-center justify-center h-full'>No Layers to show.</div>
                  ) : (
                    <>
                      <table className='table w-full'>
                        <thead>
                          <tr>
                            <th className='text-base'>Layer List</th>
                          </tr>
                        </thead>
                        <tbody>
                          {layerList?.map((layer, index) => {
                            if (layer.endsWith('.ase') || layer.endsWith('.aseprite'))
                              return (
                                <tr key={index} className='select-none'>
                                  <td className='text-base text-gray-300 font-black'>{layer}</td>
                                </tr>
                              );

                            return (
                              <tr key={index} className={`${selectedLayers?.includes(layer) ? 'bg-base-200' : ''}`}>
                                <td
                                  className='cursor-pointer select-none'
                                  onClick={() =>
                                    this.setState({
                                      selectedLayers: selectedLayers?.includes(layer)
                                        ? selectedLayers?.filter((l) => l !== layer)
                                        : [...(selectedLayers ?? []), layer],
                                    })
                                  }>
                                  {layer}
                                </td>
                              </tr>
                            );
                          })}
                        </tbody>
                      </table>
                    </>
                  )}
                </>
              )}
            </div>
          </div>
        )}

        {/* Export Options */}
        <div className='flex justify-start'>
          {/* Every Frame */}
          <div className='flex items-center justify-center gap-2 p-2 mt-2 ml-4'>
            <input
              id='every-frame'
              type='radio'
              className='radio'
              checked={exportType === ExportTypes.EveryFrame}
              onChange={() => {
                this.updateConfig('exportType', ExportTypes.EveryFrame);
              }}
            />
            <label htmlFor='every-frame'>Every Frame?</label>
          </div>

          {/* Sheet Export */}
          <div className='flex items-center justify-center gap-2 p-2 mt-2 ml-4'>
            <input
              id='every-frame'
              type='radio'
              className='radio'
              checked={exportType === ExportTypes.SheetExport}
              onChange={() => {
                this.updateConfig('exportType', ExportTypes.SheetExport);
              }}
            />
            <label htmlFor='every-frame'>Sheet Export?</label>
          </div>
        </div>

        {/* Sheet Export Options */}
        <div className='flex items-center gap-2 ml-4 p-2'>
          {/* BOTH Options (save as and sheet) */}
          {/* Split Layers */}
          <div className='flex items-center gap-2'>
            <input
              id='export-json'
              type='checkbox'
              className='checkbox'
              checked={splitLayers ?? false}
              onChange={() => this.updateConfig('options', { ...options, splitLayers: !splitLayers })}
            />
            <label htmlFor='export-json'>Split Layers?</label>
          </div>

          {exportType === ExportTypes.SheetExport && (
            <>
              {/* Export JSON */}
              <div className='flex items-center gap-2'>
                <input
                  id='export-json'
                  type='checkbox'
                  className='checkbox'
                  checked={exportJson ?? false}
                  onChange={() => this.updateConfig('options', { ...options, exportJson: !exportJson })}
                />
                <label htmlFor='export-json'>Export JSON?</label>
              </div>

              {/* Sheet Type */}
              <div className='flex items-center justify-center gap-2'>
                <label htmlFor='sheet-type' className='label text-nowrap'>
                  Sheet Type:
                </label>
                <select
                  name='sheet-type'
                  id='sheet-type'
                  className='select select-bordered select-sm w-full max-w-xs'
                  value={options.sheetType}
                  onChange={(e) => this.updateConfig('options', { ...options, sheetType: parseInt(e.target.value) })}>
                  <option value={SheetTypes.Horizontal}>Horizontal</option>
                  <option value={SheetTypes.Vertical}>Vertical</option>
                  <option value={SheetTypes.Rows}>Rows</option>
                  <option value={SheetTypes.Columns}>Columns</option>
                  <option value={SheetTypes.Packed}>Packed</option>
                </select>
              </div>

              {/* Sheet Column */}
              {sheetType === SheetTypes.Columns && (
                <div className='flex items-center gap-2'>
                  <label htmlFor='sheet-type' className='label text-nowrap'>
                    Sheet Columns:
                  </label>
                  <input
                    type='number'
                    className='input input-sm input-bordered'
                    min={1}
                    value={options.sheetRows ?? 0}
                    onChange={(e) => this.updateConfig('options', { ...options, sheetRows: parseInt(e.target.value) })}
                  />
                </div>
              )}

              {/* Sheet Rows */}
              {sheetType === SheetTypes.Rows && (
                <div className='flex items-center gap-2'>
                  <label htmlFor='sheet-type' className='label text-nowrap'>
                    Sheet Rows:
                  </label>
                  <input
                    type='number'
                    className='input input-sm input-bordered'
                    min={1}
                    value={options.sheetColumns ?? 0}
                    onChange={(e) => this.updateConfig('options', { ...options, sheetColumns: parseInt(e.target.value) })}
                  />
                </div>
              )}
            </>
          )}
        </div>

        {/* Scale */}
        <div className='flex items-center justify-center absolute bottom-4 right-24 gap-2'>
          <label htmlFor='scale'>Scale:</label>
          <input
            type='number'
            id='scale'
            className='input input-sm input-bordered  w-[75px]'
            value={this.state.scale}
            min={1}
            max={20}
            onChange={(e) => this.updateConfig('scale', parseInt(e.target.value))}
          />
        </div>

        {/* Aseprite Button Export */}
        <button className='absolute btn btn-sm btn-error bottom-4 right-4' onClick={this.handleExport}>
          {exportLoading && <span className='loading loading-spinner'></span>}
          Export!
        </button>
      </div>
    );
  }
}
