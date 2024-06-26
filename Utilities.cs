namespace Aseprite_Multiple_Export
{
    public static class Utilities
    {
        public static List<LayerNode> BuildLayerTree(List<AsepriteLayer> layers)
        {
            var layerMap = new Dictionary<Guid, LayerNode>();
            var rootNodes = new List<LayerNode>();

            foreach ( var layer in layers )
            {
                var node = new LayerNode() { Name = layer.name, FullPath = layer.name, Children = new List<LayerNode>() };
                layerMap.Add(node.Id, node);

                if ( layer.group != default )
                {
                    var parent = layerMap.Values.FirstOrDefault(x => x.Name == layer.group);
                    if (parent != default) parent.Children.Add(node);
                    else rootNodes.Add(node);
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
            var lines = new List<string>();

            foreach ( var node in nodes )
            {
                TransverseLayerTree(ref lines, node, string.Empty);
            }

            return lines;
        }

        private static void TransverseLayerTree(ref List<string> lines, LayerNode node, string parentPath)
        {
            var currentPath = parentPath.Length > 0 ? Path.Combine(parentPath, node.Name) : node.Name;

            if (node.Children.Count == 0 )
            {
                lines.Add(currentPath);
            }
            else
            {
                lines.Add(currentPath);

                foreach ( var child in node.Children )
                {
                    TransverseLayerTree(ref lines, child, currentPath);
                }
            }
        }
    }
}
