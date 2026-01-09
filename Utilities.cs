namespace Aseprite_Multiple_Export;

public static class Utilities
{
    public static List<LayerNode> BuildLayerTree(List<AsepriteLayer> layers)
    {
        Dictionary<string, LayerNode> layerMap = [];
        List<LayerNode> rootNodes = [];

        foreach (AsepriteLayer layer in layers)
        {
            string? parentPath = layer.group;
            if (!string.IsNullOrEmpty(parentPath))
                parentPath = parentPath.Replace("\\", "/");

            string fullPath = parentPath != default
                ? $"{parentPath}/{layer.name}"
                : layer.name;

            LayerNode node = new() { Name = layer.name, FullPath = fullPath, Children = [] };
            layerMap[fullPath] = node;

            if (layer.group != default)
            {
                string lookup = parentPath ?? layer.group!;
                if (layerMap.TryGetValue(lookup, out LayerNode? parent))
                    parent.Children.Add(node);
                else
                    rootNodes.Add(node);
            }
            else
            {
                rootNodes.Add(node);
            }
        }

        return rootNodes;
    }

    public static List<string> FormatLayerTree(List<LayerNode> nodes)
    {
        List<string> lines = [];

        foreach (LayerNode node in nodes)
        {
            TransverseLayerTree(ref lines, node, string.Empty);
        }

        return lines;
    }

    private static void TransverseLayerTree(ref List<string> lines, LayerNode node, string parentPath)
    {
        string currentPath = parentPath.Length > 0 ? $"{parentPath}/{node.Name}" : node.Name;

        if (node.Children.Count == 0)
        {
            lines.Add(currentPath);
        }
        else
        {
            lines.Add(currentPath);

            foreach (LayerNode child in node.Children)
            {
                TransverseLayerTree(ref lines, child, currentPath);
            }
        }
    }
}
