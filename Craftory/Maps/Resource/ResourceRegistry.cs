using Craftory.Maps.Tiles;

namespace Craftory.Maps.Resource
{
    public static class ResourceRegistry
    {
        public static Dictionary<ResourceType, TileAnimationInfo> Resources =
            new Dictionary<ResourceType, TileAnimationInfo>()
            {
            {
                ResourceType.None,
                null
            },
            {
                ResourceType.Copper,
                new TileAnimationInfo
                {
                    TexturePath = "Resource/Tile/Copper",
                    FrameCount = 1,
                    FrameTime = 0f
                }
            }
            };
    }

}
