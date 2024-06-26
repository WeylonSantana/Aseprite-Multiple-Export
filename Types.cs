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
    public List<AsepriteFrame> frames { get; set; }

    public AsepriteMeta meta { get; set; }
}

public class AsepriteFrame
{
    public string filename { get; set; }

    public AsepriteBound frame { get; set; }

    public bool rotated { get; set; }

    public bool trimmed { get; set; }

    public AsepriteBound spriteSourceSize { get; set; }

    public AsepriteSize sourceSize { get; set; }

    public int duration { get; set; }
}

public class AsepriteBound
{
    public int x { get; set; }

    public int y { get; set; }

    public int w { get; set; }

    public int h { get; set; }
}

public class AsepriteSize
{
    public int w { get; set; }

    public int h { get; set; }
}

public class AsepriteMeta
{
    public string app { get; set; }

    public string version { get; set; }

    public string image { get; set; }

    public string format { get; set; }

    public AsepriteSize size { get; set; }

    public string scale { get; set; }

    public List<AsepriteFrameTag> frameTags { get; set; }

    public List<AsepriteLayer> layers { get; set; }
}

public class AsepriteFrameTag
{
    public string name { get; set; }

    public int from { get; set; }

    public int to { get; set; }

    public string direction { get; set; }
}

public class AsepriteLayer
{
    public string name { get; set; }

    public string? group { get; set; }
}

public class LayerNode
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; }

    public string FullPath { get; set; }

    public List<LayerNode> Children { get; set; }
}