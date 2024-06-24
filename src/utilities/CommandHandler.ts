import { path } from '@tauri-apps/api';
import { FileEntry } from '@tauri-apps/api/fs';
import { Command } from '@tauri-apps/api/shell';
import { GetAsepriteOutputName } from './Utilities';
import { ExportTypes, SheetTypes } from '../types';
import { AppState } from '../App';

export default class CommandHandler {
  async executeCommand(command: string, args: string[], cwd?: string, closeHandler?: () => void) {
    console.log(`Executing command: ${command} ${args.join(' ')}`);
    const cmd = new Command(command, args, { cwd });

    return new Promise<void>((resolve, reject) => {
      if (closeHandler) {
        cmd.on('close', () => {
          closeHandler();
          resolve();
        });
      } else {
        cmd.on('close', resolve);
      }

      cmd.on('error', reject);
      cmd.execute().catch(reject);
    });
  }

  async exportSingleFile(file: FileEntry, data: AppState) {
    const { scale, exportType, selectedLayers, options } = data;
    const { sheetType, sheetRows, sheetColumns, exportJson, splitLayers, allLayers } = options;
    const outputName = GetAsepriteOutputName(file, scale);

    const exportTypesArgs: string[][] = [
      // export every frame as a separate file
      ['-b', file.path, '--save-as', `${outputName}`],
      // export sheet
      ['-b', file.path, '--sheet', `${outputName}`],
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

    if (splitLayers) args.splice(1, 0, '--split-layers');
    if (allLayers) args.splice(1, 0, '--all-layers');
    args.push('--scale', `${scale}`);

    return await this.executeCommand('Aseprite', args, `${file.path.split(path.sep).slice(0, -1).join(path.sep)}`);
  }
}
