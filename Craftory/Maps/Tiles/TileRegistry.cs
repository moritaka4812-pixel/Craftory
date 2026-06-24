
using Craftory.Maps.Tiles;

namespace Craftory.Maps.Tiles
{
    public static class TileRegistry
    {
        public static Dictionary<TileType, TileAnimationInfo> Terrain =
            new Dictionary<TileType, TileAnimationInfo>()
            {
            {
                TileType.Ground,
                new TileAnimationInfo
                {
                    TexturePath = "TileUI/Ground",
                    FrameCount = 1,
                    FrameTime = 0f
                }
            },
            {
                TileType.Road,
                new TileAnimationInfo
                {
                    TexturePath = "TileUI/Road",
                    FrameCount = 1,
                    FrameTime = 0f
                }
            },
            {
                TileType.stone,
                new TileAnimationInfo
                {
                    TexturePath = "TileUI/Stone",
                    FrameCount = 1,
                    FrameTime = 0f
                }
            },
            {
                    TileType.blockedStone,
                    new TileAnimationInfo()
                    {
                        TexturePath = "TileUI/Stone",
                        FrameCount = 1,
                        FrameTime = 0f
                    }
            }
            };
    }

}
