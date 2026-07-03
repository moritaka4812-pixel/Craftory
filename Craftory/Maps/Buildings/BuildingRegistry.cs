using Point = Microsoft.Xna.Framework.Point;
using Craftory.Maps.Buildings.Miners;
using Craftory.Maps.Buildings.Conveyors;
using SharpDX.Direct3D11;

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
                        TexturePaths = new()
                        {
                            { BuildingDirection.None, "Buildings/Miner/Drill" }
                        },
                        Type = BuildType.Drill,
                        Width = 1,
                        Height = 1,
                        FrameCount = 9,
                        FrameTime = 0.2f,
                        SizeInTiles = new Point(1,1),
                        WorkSpeed = 0.25f,
                        Create = (pos, dir) => new Drill(BuildType.Drill, pos)
                    }
                },
                {
                    BuildType.Conveyor,
                    new BuildingInfo
                    {
                        TexturePaths = new()
                        {
                            { BuildingDirection.None, "Buildings/Conveyor/ConveyorStraight"}
                        },
                        Type = BuildType.Conveyor,
                        Width = 1,
                        Height = 1,
                        FrameCount = 5,
                        FrameTime = 0.25f,
                        SizeInTiles = new Point(1,1),
                        WorkSpeed = 1.0f,
                        Create = (pos, dir) => new Conveyor(BuildType.Conveyor, pos, dir)
                    }
                },
                {
                    BuildType.ConveyorRightCurve,
                    new BuildingInfo()
                    {
                        TexturePaths = new()
                        {
                            { BuildingDirection.None, "Buildings/Conveyor/ConveyorRightCurve" }
                        },
                        Type = BuildType.ConveyorRightCurve,
                        Width = 1,
                        Height = 1,
                        FrameCount = 5,
                        FrameTime = 0.25f,
                        SizeInTiles = new Point(1,1),
                        WorkSpeed = 1.0f,
                        Create = (pos, inDir) => new ConveyorRightCurve(BuildType.ConveyorRightCurve, pos, inDir)
                    }
                },
                {
                    BuildType.ConveyorLeftCurve,
                    new BuildingInfo()
                    {
                        TexturePaths = new()
                        {
                            {BuildingDirection.None, "Buildings/Conveyor/ConveyorLeftCurve" }
                        },
                        Type = BuildType.ConveyorLeftCurve,
                        Width = 1,
                        Height = 1,
                        FrameCount = 5,
                        FrameTime = 0.25f,
                        SizeInTiles = new Point(1,1),
                        WorkSpeed = 1.0f,
                        Create = (pos, inDir) => new ConveyorLeftCurve(BuildType.ConveyorLeftCurve, pos, inDir)
                    }
                },
                {
                    BuildType.ConveyorRightMerge,
                    new BuildingInfo()
                    {
                        TexturePaths = new ()
                        {
                            { BuildingDirection.None, "Buildings/Conveyor/ConveyorRightMerge" }
                        },
                        Type = BuildType.ConveyorRightMerge,
                        Width = 1,
                        Height = 1,
                        FrameCount = 5,
                        FrameTime = 0.25f,
                        SizeInTiles = new Point(1,1),
                        WorkSpeed = 1.0f,
                        Create = (pos, outDir) => new ConveyorRightMerge(BuildType.ConveyorRightMerge, pos, outDir)
                    }
                }

            };

        public static void LoadTextures()
        {
            foreach( var info in Data.Values)
            {
                foreach( var kv in info.TexturePaths)
                {
                    var dir = kv.Key;
                    var path = kv.Value;

                    info.CachedTextures[dir] = ContentLoader.LoadTexture(path);
                }
            }
        }
    }
}
