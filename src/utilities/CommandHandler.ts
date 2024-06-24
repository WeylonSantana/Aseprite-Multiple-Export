import { Command } from '@tauri-apps/api/shell';

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
}
