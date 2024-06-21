import { Component } from 'react';
import { open } from '@tauri-apps/api/dialog';
import { exists, readTextFile, writeTextFile, BaseDirectory, createDir } from '@tauri-apps/api/fs';

interface AppState {
  keepConfig: boolean;
  asepritePath: string;
}

export default class App extends Component<any, AppState> {
  constructor(props: any) {
    super(props);
    this.state = {
      keepConfig: false,
      asepritePath: '',
    };

    this.updateConfig = this.updateConfig.bind(this);
    this.searchAsepritePath = this.searchAsepritePath.bind(this);
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
      });
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

  render() {
    const { keepConfig, asepritePath } = this.state;

    return (
      <div>
        <div className='flex gap-2 mt-6 ml-4 items-center absolute right-4 top-0'>
          <input id='keep-config' type='checkbox' className='checkbox' checked={keepConfig} onChange={() => this.updateConfig('keepConfig', !keepConfig)} />
          <label htmlFor='keep-config'>Manter Configurações?</label>
        </div>

        {/* Aseprite Path Search */}
        <div className='flex gap-2 mt-4 ml-4 items-center'>
          <label className='label'>Aseprite Path:</label>
          <input className='input input-sm w-[700px]' type='text' value={asepritePath} disabled />
          <button className='btn btn-sm btn-success' onClick={this.searchAsepritePath}>
            Procurar
          </button>
        </div>
      </div>
    );
  }
}
