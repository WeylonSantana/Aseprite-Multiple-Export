import { Component } from 'react';
import { FileEntry } from '@tauri-apps/api/fs';
import { AppState } from '../App';

interface FilelistProps {
  data: AppState;

  loadLayerList: (file?: FileEntry) => void;
  selectFiles: (files: FileEntry[]) => void;
  selectLayer: (layer: string, file?: FileEntry) => void;
}

export default class Filelist extends Component<FilelistProps> {
  render() {
    const { data, loadLayerList, selectFiles, selectLayer } = this.props;
    const { fileList, layerList, layersLoading, selectedFiles, selectedLayerFile, selectedLayers } = data;

    return (
      <>
        {fileList?.length && (
          // File List
          <div className='flex gap-2'>
            <div className='min-w-[600px] max-h-[400px] bg-base-300 rounded-md mt-2 ml-4 overflow-y-auto'>
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
                        onClick={(event) => {
                          if (!event.ctrlKey && (layersLoading || !selectedFiles || selectedLayers?.length)) return;
                          if (event.ctrlKey) return loadLayerList(file);

                          const newFiles = selectedFiles;
                          if (!newFiles) return;

                          // we are trying to add?
                          if (!newFiles.includes(file)) {
                            newFiles.push(file);
                          } else {
                            newFiles.splice(newFiles.indexOf(file), 1);
                          }

                          selectFiles(newFiles);
                        }}>
                        {file.name}
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>

            {/* Layers List */}
            <div className='min-w-[500px] max-h-[400px] bg-base-300 rounded-md mt-2 ml-4 overflow-y-auto'>
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
                                  <td className='text-base font-black text-gray-300'>{layer}</td>
                                </tr>
                              );

                            return (
                              <tr key={index} className={`${selectedLayers?.includes(layer) ? 'bg-base-200' : ''}`}>
                                <td className='cursor-pointer select-none' onClick={() => selectLayer(layer, selectedLayerFile)}>
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
        <span className='ml-6 text-sm'>CTRL + Click in one file to show the Layer List</span>
      </>
    );
  }
}
