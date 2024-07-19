#pragma warning disable IDE1006 // Estilos de Nomenclatura
#pragma warning disable CA1050 // Declarar tipos em namespaces

public enum ExportType
{
    EveryFrame,
    SpriteSheet,
}

public enum SheetExportType
{
    Horizontal,
    Vertical,
    Rows,
    Columns,
    Packed,
}

public class AsepriteJsonFile
{
    public required List<AsepriteFrame> frames { get; set; }

    public required AsepriteMeta meta { get; set; }
}

public class AsepriteFrame
{
    public required string filename { get; set; }

    public required AsepriteBound frame { get; set; }

    public required bool rotated { get; set; }

    public required bool trimmed { get; set; }

    public required AsepriteBound spriteSourceSize { get; set; }

    public required AsepriteSize sourceSize { get; set; }

    public required int duration { get; set; }
}

public class AsepriteBound
{
    public required int x { get; set; }

    public required int y { get; set; }

    public required int w { get; set; }

    public required int h { get; set; }
}

public class AsepriteSize
{
    public required int w { get; set; }

    public required int h { get; set; }
}

public class AsepriteMeta
{
    public required string app { get; set; }

    public required string version { get; set; }

    public required string image { get; set; }

    public required string format { get; set; }

    public required AsepriteSize size { get; set; }

    public required string scale { get; set; }

    public required List<AsepriteFrameTag> frameTags { get; set; }

    public required List<AsepriteLayer> layers { get; set; }
}

public class AsepriteFrameTag
{
    public required string name { get; set; }

    public required int from { get; set; }

    public required int to { get; set; }

    public required string direction { get; set; }
}

public class AsepriteLayer
{
    public required string name { get; set; }

    public required string? group { get; set; }
}

public class LayerNode
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public required string Name { get; set; }

    public required string FullPath { get; set; }

    public required List<LayerNode> Children { get; set; }
}