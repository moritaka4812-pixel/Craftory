using Point = Microsoft.Xna.Framework.Point;

namespace Craftory.Maps.Buildings
{
    public static class BuildingRegistry
    {

        public static Dictionary<BuildType, BuildingInfo> Data =
            new()
            {
                {
                    BuildType.Drill,
                    new BuildingInfo()
                    {
                        TexturePath = "Buildings/Miner/Drill",
                        Type = BuildType.Drill,
                        Width = 1,
                        Height = 1,
                        FrameCount = 9,
                        FrameTime = 0.2f,
                        SizeInTiles = new Point(1,1),
                        WorkSpeed = 0.25f,
                        Create = pos => new Drill(pos)
                    }
                }
            };
    }
}
