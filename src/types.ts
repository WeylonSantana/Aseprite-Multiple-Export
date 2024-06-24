export enum ExportTypes {
  EveryFrame,
  SheetExport,
}

export enum SheetTypes {
  Horizontal,
  Vertical,
  Rows,
  Columns,
  Packed,
}

export interface Layer {
  name: string;
  opacity?: number;
  blendMode?: string;
  group?: string;
  color?: string;
}

export interface LayerNode {
  name: string;
  fullPath: string;
  children: LayerNode[];
}
