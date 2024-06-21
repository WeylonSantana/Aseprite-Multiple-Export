import { Component } from 'react';
import { open } from '@tauri-apps/api/dialog';
import { exists, readTextFile, writeTextFile, BaseDirectory, createDir, readDir, FileEntry } from '@tauri-apps/api/fs';

interface AppState {
  keepConfig: boolean;
  asepritePath: string;
  fileListPath: string;
  fileList?: FileEntry[];
  selectedFile?: FileEntry;
}

export default class App extends Component<any, AppState> {
  constructor(props: any) {
    super(props);
    this.state = {
      keepConfig: false,
      asepritePath: '',
      fileListPath: '',
    };

    this.updateConfig = this.updateConfig.bind(this);
    this.searchAsepritePath = this.searchAsepritePath.bind(this);
    this.searchAsepriteFiles = this.searchAsepriteFiles.bind(this);
    this.loadFileList = this.loadFileList.bind(this);
  }

  async componentDidMount() {
    try {
      if (!(await exists('', { dir: BaseDirectory.AppConfig }))) {
        await createDir('', { dir: BaseDirectory.AppConfig });
        await writeTextFile('config.json', JSON.stringify({}), { dir: BaseDirectory.AppConfig });
      }

      var state = JSON.parse(await readTextFile('config.json', { dir: BaseDirectory.AppConfig }));
      this.setState({
        keepConfig: state?.keepConfig ?? false,
        asepritePath: state?.asepritePath ?? '',
        fileListPath: state?.fileListPath ?? '',
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
      this.setState((prevState: AppState) => ({
        ...prevState,
        [key]: value,
      }));

      if (this.state.keepConfig || (key === 'keepConfig' && value)) {
        var state = JSON.parse(await readTextFile('config.json', { dir: BaseDirectory.AppConfig }));
        state[key] = value;
        await writeTextFile('config.json', JSON.stringify(state, undefined, 2), { dir: BaseDirectory.AppConfig });
      }

      if (!this.state.keepConfig) {
        await writeTextFile('config.json', JSON.stringify({}), { dir: BaseDirectory.AppConfig });
      }
    } catch (error) {
      console.error(error);
    }
  }

  async searchAsepritePath() {
    try {
      var file = await open({ multiple: false, directory: false, filters: [{ name: 'executável', extensions: ['exe'] }] });
      if (!file || Array.isArray(file)) return;
      this.updateConfig('asepritePath', file);
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
      this.setState({ fileList });
    } catch (error) {
      console.error(error);
    }
  }

  render() {
    const { keepConfig, asepritePath, fileListPath, selectedFile } = this.state;

    return (
      <div>
        <div className='flex gap-2 mt-6 ml-4 items-center absolute right-4 top-0'>
          <input id='keep-config' type='checkbox' className='checkbox' checked={keepConfig} onChange={() => this.updateConfig('keepConfig', !keepConfig)} />
          <label htmlFor='keep-config'>Keep Changes?</label>
        </div>

        {/* Aseprite Path Search */}
        <div className='flex gap-2 mt-4 ml-4 items-center'>
          <label className='label'>Aseprite Path:</label>
          <input className='input input-sm w-[700px]' type='text' value={asepritePath} disabled />
          <button className='btn btn-sm btn-success' onClick={this.searchAsepritePath}>
            Search Aseprite.exe
          </button>
        </div>

        {/* Aseprite Path Filelist Search */}
        <div className='flex gap-2 ml-4 items-center'>
          <label className='label'>Ase Files Path:</label>
          <input className='input input-sm w-[700px]' type='text' value={fileListPath} disabled />
          <button className='btn btn-sm btn-success' onClick={this.searchAsepriteFiles}>
            Search .ase/.aseprite files
          </button>
        </div>

        {/* Aseprite File List */}
        <div className='max-w-[800px] max-h-[300px] bg-base-300 rounded-md mt-2 ml-4 overflow-y-scroll'>
          <table className='table w-full'>
            <thead>
              <tr>
                <th className='text-base'>File Name</th>
              </tr>
            </thead>
            <tbody>
              {this.state.fileList?.map((file, index) => (
                <tr key={index} className={`${selectedFile?.name === file.name ? 'bg-base-200' : ''}`}>
                  <td
                    className='cursor-pointer select-none'
                    onClick={() => this.setState({ selectedFile: selectedFile?.name === file.name ? undefined : file })}>
                    {file.name}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    );
  }
}
