import { FileEntry } from '@tauri-apps/api/fs';
import { Layer, LayerNode } from '../types';

export function GetAsepriteOutputName(files: FileEntry[], selectedIndex: number, scale: number) {
  if (!files?.length) return 'ERR';

  const file = files[selectedIndex];
  if (file.name) {
    const ext = file.name.split('.').pop();
    return `${scale}x/${file.name.replace(`.${ext}`, '.png')}`;
  }

  const fileName = file.path.split('\\').pop();
  if (fileName && fileName.length) {
    const ext = fileName.split('.').pop();
    return `${scale}x/${fileName.replace(`.${ext}`, '.png')}`;
  }

  return 'ERR';
}

export function BuildLayerTree(layers: Layer[]) {
  const layerMap: { [key: string]: LayerNode } = {};
  const rootNodes: LayerNode[] = [];

  layers.forEach((layer) => {
    const node: LayerNode = {
      name: layer.name,
      fullPath: layer.name,
      children: [],
    };

    layerMap[layer.name] = node;

    if (layer.group) {
      const parent = layerMap[layer.group];
      if (parent) {
        parent.children.push(node);
      } else {
        rootNodes.push(node);
      }
    } else {
      rootNodes.push(node);
    }
  });

  return rootNodes;
}

export function FormatLayerTree(nodes: LayerNode[]) {
  const lines: string[] = [];

  // recursive function to format the layer tree, to show in the UI all the paths like 'parent/group/layer'
  const traverse = (node: LayerNode, parentPath = '') => {
    const currentPath = parentPath ? `${parentPath}/${node.name}` : node.name;

    if (node.children.length === 0) {
      // Only add to lines if it's not a group (i.e., it has no children)
      lines.push(currentPath);
    } else {
      // Traverse children
      node.children.forEach((child) => traverse(child, currentPath));
    }
  };

  nodes.forEach((node) => traverse(node));
  return lines.reverse();
}
