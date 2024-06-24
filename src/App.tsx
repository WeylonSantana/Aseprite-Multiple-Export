import { Component } from 'react';
import { open } from '@tauri-apps/api/dialog';
import { exists, readTextFile, writeTextFile, BaseDirectory, createDir, readDir, FileEntry, removeFile } from '@tauri-apps/api/fs';
import { Command } from '@tauri-apps/api/shell';
import { dialog, process } from '@tauri-apps/api';
import Filelist from './components/Filelist';
import { ExportTypes, SheetTypes } from './types';
import Checkbox from './components/Checkbox';
import { BuildLayerTree, FormatLayerTree, GetAsepriteOutputName } from './utilities/Utilities';
import CommandHandler from './utilities/CommandHandler';

export interface AppState {
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
    allLayers?: boolean;
  };

  // Local State
  fileList?: FileEntry[];
  layerList?: string[];
  selectedFiles?: FileEntry[];
  selectedLayers?: string[];
  exportLoading?: boolean;
  layersLoading?: boolean;
}

export default class App extends Component<any, AppState> {
  private commandHandler: CommandHandler;

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

    this.commandHandler = new CommandHandler();

    this.checkForAseprite = this.checkForAseprite.bind(this);
    this.updateConfig = this.updateConfig.bind(this);
    this.searchAsepriteFiles = this.searchAsepriteFiles.bind(this);
    this.loadFileList = this.loadFileList.bind(this);
    this.loadLayerList = this.loadLayerList.bind(this);
    this.handleExport = this.handleExport.bind(this);
  }

  async checkForAseprite() {
    try {
      await this.commandHandler.executeCommand('Aseprite', ['--version']);
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
        await this.loadLayerList(fileList[0]);
      });
    } catch (error) {
      console.error(error);
    }
  }

  async loadLayerList(file?: FileEntry) {
    const { fileListPath, options } = this.state;

    try {
      const startGettingLayers = async () => {
        if (!file?.name) return this.setState({ layerList: [], layersLoading: false });

        // first we need to create aseprite json and then read the layers from it
        var jsonName = file.name.includes('.ase') ? file.name.replace('.ase', '.json') : file.name.replace('.aseprite', '.json');
        const args = ['-b'];
        if (options.allLayers) args.push('--all-layers');
        args.push('--list-layers', file.path, '--data', jsonName, '--format', 'json-array');

        try {
          //just create the json file to get the layers
          await this.commandHandler.executeCommand('Aseprite', args, fileListPath);
          const asepriteJson = JSON.parse(await readTextFile(`${fileListPath}\\${jsonName}`));
          // delete the json file
          await removeFile(`${fileListPath}\\${jsonName}`);

          this.setState({ layerList: FormatLayerTree(BuildLayerTree(asepriteJson.meta.layers)), layersLoading: false });
        } catch (error) {
          console.error(error);
        }
      };

      this.setState({ layerList: [], layersLoading: true, selectedLayers: undefined }, startGettingLayers);
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
    const { exportJson, sheetType, sheetColumns, sheetRows, splitLayers, allLayers } = options;
    if (!selectedFiles?.length) return;

    const file = selectedFiles[index];
    if (!file?.name) return;
    const outputName = GetAsepriteOutputName(selectedFiles, index, scale);

    const exportTypesArgs: string[][] = [
      // export every frame as a separate file
      ['-b', file.name, '--save-as', `${outputName}`],
      // export sheet
      ['-b', file.name, '--sheet', `${outputName}`],
    ];

    const args = exportTypesArgs[exportType];

    selectedLayers?.forEach((layer) => {
      var layerArg = ['--layer', layer];
      // we need to insert the layer argument before the save-as or sheet argument
      args.splice(1, 0, ...layerArg);
    });

    if (exportType === ExportTypes.SheetExport) {
      args.push('--sheet-type', SheetTypes[sheetType].toLowerCase());
      if (sheetType === SheetTypes.Columns && sheetRows) args.push('--sheet-rows', sheetRows.toString());
      if (sheetType === SheetTypes.Rows && sheetColumns) args.push('--sheet-columns', sheetColumns.toString());

      if (exportJson) args.push('--data', outputName.replace('.png', '.json'), '--format', 'json-array', '--list-layers', '--list-tags');
    }

    // both options (save as and sheet)
    if (splitLayers) args.splice(1, 0, '--split-layers');
    if (allLayers) args.splice(1, 0, '--all-layers');
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

  render() {
    const { keepConfig, fileListPath, fileList, layerList, exportType, options } = this.state;
    const { selectedFiles, selectedLayers } = this.state;
    const { exportLoading, layersLoading } = this.state;
    const { exportJson, sheetType, splitLayers, allLayers } = options;

    return (
      <div className='overflow-hidden'>
        {exportLoading && (
          <div className='absolute flex gap-1 items-center justify-center z-10 w-[100vw] h-[100vh] bg-[rgba(0,0,0,0.5)]'>
            <label className='text-2xl label'>Exporting</label>
            <div className='mt-4 loading loading-dots'></div>
          </div>
        )}

        {/* Keep Config Checkbox */}
        <Checkbox
          id='keep-config'
          label='Keep Changes?'
          checked={keepConfig}
          className='absolute top-0 flex items-center gap-2 mt-6 ml-4 right-4'
          onChange={() => this.updateConfig('keepConfig', !keepConfig)}
        />

        {/* Aseprite Path Filelist Search */}
        <div className='flex items-center gap-2 mt-4 ml-4'>
          <label className='label'>Ase Files Path:</label>
          <input className='input input-sm w-[700px]' type='text' value={fileListPath} disabled />
          <button className='btn btn-sm btn-success' onClick={this.searchAsepriteFiles}>
            Search .ase/.aseprite files
          </button>
        </div>

        {/* Aseprite File List */}
        <Filelist
          fileList={fileList}
          layerList={layerList}
          selectedFiles={selectedFiles}
          selectedLayers={selectedLayers}
          layersLoading={layersLoading}
          loadLayerList={(file) => this.loadLayerList(file)}
          selectFiles={(files) => this.setState({ selectedFiles: files })}
        />

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
        <div className='flex items-center gap-2 p-2 ml-4'>
          {/* BOTH Options (save as and sheet) */}
          {/* Split Layers */}
          <Checkbox
            id='split-layers'
            label='Split Layers?'
            checked={splitLayers ?? false}
            onChange={() => this.updateConfig('options', { ...options, splitLayers: !splitLayers })}
          />

          <Checkbox
            id='all-layers'
            label='All Layers?'
            checked={allLayers ?? false}
            onChange={async () => {
              await this.updateConfig('options', { ...options, allLayers: !allLayers });
              await this.loadLayerList();
            }}
          />

          {exportType === ExportTypes.SheetExport && (
            <>
              {/* Export JSON */}
              <Checkbox
                id='export-json'
                label='Export JSON?'
                checked={exportJson ?? false}
                onChange={() => this.updateConfig('options', { ...options, exportJson: !exportJson })}
              />

              {/* Sheet Type */}
              <div className='flex items-center justify-center gap-2'>
                <label htmlFor='sheet-type' className='label text-nowrap'>
                  Sheet Type:
                </label>
                <select
                  name='sheet-type'
                  id='sheet-type'
                  className='w-full max-w-xs select select-bordered select-sm'
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
        <div className='absolute flex items-center justify-center gap-2 bottom-4 right-24'>
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
