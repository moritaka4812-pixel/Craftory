using Craftory.Maps.Tiles;

namespace Craftory.Maps.Resource
{
    public static class TileResourceRegistry
    {
        public static Dictionary<TileResourceType, TileAnimationInfo> Resources =
            new Dictionary<TileResourceType, TileAnimationInfo>()
            {
            {
                TileResourceType.None,
                null
            },
            {
                TileResourceType.Copper,
                new TileAnimationInfo
                {
                    TexturePath = "TileUI/Resources/Copper",
                    FrameCount = 1,
                    FrameTime = 0f
                }
            }
            };
    }

}
