import { Component } from 'react';
import { open } from '@tauri-apps/api/dialog';
import { exists, readTextFile, writeTextFile, BaseDirectory, createDir, readDir, FileEntry } from '@tauri-apps/api/fs';
import { Command } from '@tauri-apps/api/shell';
import { dialog, process } from '@tauri-apps/api';

interface AppState {
  // Config State
  keepConfig: boolean;
  fileListPath: string;

  // Local State
  fileList?: FileEntry[];
  selectedFile?: FileEntry;
  loading?: boolean;
}

export default class App extends Component<any, AppState> {
  constructor(props: any) {
    super(props);
    this.state = {
      keepConfig: false,
      fileListPath: '',
    };

    this.checkForAseprite = this.checkForAseprite.bind(this);
    this.updateConfig = this.updateConfig.bind(this);
    this.searchAsepriteFiles = this.searchAsepriteFiles.bind(this);
    this.loadFileList = this.loadFileList.bind(this);
    this.handleExport = this.handleExport.bind(this);
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
      });

      if (state?.fileListPath && (await exists(state.fileListPath))) {
        this.loadFileList(state.fileListPath);
      }
    } catch (error) {
      console.error(error);
    }
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

  async updateConfig<K extends keyof AppState>(key: K, value: AppState[K]) {
    try {
      // Using a type-safe way to update the state
      this.setState((prevState: AppState) => ({
        ...prevState,
        [key]: value,
      }));

      if (this.state.keepConfig || (key === 'keepConfig' && value)) {
        await writeTextFile(
          'config.json',
          JSON.stringify(
            {
              keepConfig: true,
              fileListPath: this.state.fileListPath,
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

  async handleExport() {
    const { fileListPath, selectedFile } = this.state;
    if (!selectedFile) {
      await dialog.message('Please select a file to export.', { type: 'error', title: 'Aseprite Multiple Export - No file selected!' });
      return;
    }

    try {
      const command = new Command('Aseprite', ['-b', selectedFile.path, '--save-as', `${this.getAsepriteOutputName()}`], {
        cwd: fileListPath,
      });
      command.stdout.on('data', (data) => console.log(data));
      command.on('close', () => {
        dialog.message('Exported successfully!', { type: 'info', title: 'Aseprite Multiple Export - Exported!' });
        this.setState({ loading: false });
      });

      this.setState({ loading: true });
      await command.spawn();
    } catch (error) {
      console.error(error);
    }
  }

  getAsepriteOutputName() {
    const { selectedFile } = this.state;
    if (!selectedFile) return 'ERR';

    if (selectedFile.name) {
      const ext = selectedFile.name.split('.').pop();
      return selectedFile.name.replace(`.${ext}`, '.png');
    }

    const fileName = selectedFile.path.split('\\').pop();
    if (fileName && fileName.length) {
      const ext = fileName.split('.').pop();
      return fileName.replace(`.${ext}`, '.png');
    }

    return 'ERR';
  }

  render() {
    const { keepConfig, fileListPath, selectedFile, loading } = this.state;

    return (
      <div className='overflow-hidden'>
        {loading && (
          <div className='absolute flex gap-1 items-center justify-center z-10 w-[100vw] h-[100vh] bg-[rgba(0,0,0,0.5)]'>
            <label className='text-2xl label'>Exporting</label>
            <div className='mt-4 loading loading-dots'></div>
          </div>
        )}

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
        {this.state.fileList?.length && (
          <div className='max-w-[800px] max-h-[300px] bg-base-300 rounded-md mt-2 ml-4 overflow-y-auto'>
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
        )}

        {/* Aseprite Button Export */}
        <button className='absolute btn btn-error bottom-4 right-4' onClick={this.handleExport}>
          {loading && <span className='loading loading-spinner'></span>}
          Export!
        </button>
      </div>
    );
  }
}
