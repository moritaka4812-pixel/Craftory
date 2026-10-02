using Point = Microsoft.Xna.Framework.Point;
using Splitter = Craftory.Maps.Buildings.Logistics.Splitters.Splitter;
using Craftory.Maps.Buildings.Miners;
using Craftory.Maps.Buildings.Logistics.Conveyors;
using Craftory.Maps.Buildings.Logistics.Splitters;

namespace Craftory.Maps.Buildings
{
    public static class BuildingRegistry
    {

        public static Dictionary<BuildType, BuildingInfo> Data =
            new()
            {
                //Miners
                {
                    BuildType.Drill,
                    new BuildingInfo()
                    {
                        TexturePaths = new()
                        {
                            { BuildingDirection.None, "Buildings/Miners/Drill" }
                        },
                        ActiveTexturePaths = new()
                        {

                        },
                        Type = BuildType.Drill,
                        Width = 1,
                        Height = 1,
                        FrameCount = 9,
                        FrameTime = 0.2f,
                        SizeInTiles = new Point(1,1),
                        OccupiedTilesByDirection = null,
                        ReceivedDirections = new()
                        {
                            { BuildingDirection.None, new List<BuildingDirection> () }
                        },
                        OutputDirections = new()
                        {
                            { BuildingDirection.None, new List<BuildingDirection> { 
                                BuildingDirection.Up,
                                BuildingDirection.Down,
                                BuildingDirection.Left,
                                BuildingDirection.Right }
                            }
                        },
                        ReceivedTileOffsetsByDirection = new()
                        {
                            { BuildingDirection.None, new List<Point> () }
                        },
                        OutputTileOffsetsByDirection = new()
                        {
                            { BuildingDirection.None, new List<Point> { new Point(0, 0) } }
                        },
                        WorkSpeed = 0.25f,
                        BuildTime = 0,
                        AnimationMode = Tiles.TileAnimationMode.None,
                        Create = (pos, dir) => new Drill(BuildType.Drill, pos)
                    }
                },
                //Logistics.Conveyors
                {
                    BuildType.Conveyor,
                    new BuildingInfo
                    {
                        TexturePaths = new()
                        {
                            { BuildingDirection.None, "Buildings/Logistics/Conveyors/ConveyorStraight"}
                        },
                        ActiveTexturePaths = new()
                        {

                        },
                        Type = BuildType.Conveyor,
                        Width = 1,
                        Height = 1,
                        FrameCount = 5,
                        FrameTime = 0.25f,
                        SizeInTiles = new Point(1,1),
                        OccupiedTilesByDirection = null,
                        ReceivedDirections = new()
                        {
                            { BuildingDirection.Up, new List<BuildingDirection> {BuildingDirection.Down} },
                            { BuildingDirection.Down, new List<BuildingDirection> {BuildingDirection.Up} },
                            { BuildingDirection.Left, new List<BuildingDirection> {BuildingDirection.Right} },
                            { BuildingDirection.Right, new List<BuildingDirection> {BuildingDirection.Left} }
                        },
                        OutputDirections = new()
                        {
                            { BuildingDirection.Up, new List<BuildingDirection> { BuildingDirection.Up } },
                            { BuildingDirection.Down, new List<BuildingDirection> { BuildingDirection.Down } },
                            { BuildingDirection.Left, new List<BuildingDirection> { BuildingDirection.Left } },
                            { BuildingDirection.Right, new List<BuildingDirection> { BuildingDirection.Right } }
                        },
                        ReceivedTileOffsetsByDirection = new()
                        {
                            { BuildingDirection.Up, new List<Point> { new Point(0, 0) } },
                            { BuildingDirection.Down, new List<Point> { new Point(0, 0) } },
                            { BuildingDirection.Left, new List<Point> { new Point(0, 0) } },
                            { BuildingDirection.Right, new List<Point> { new Point(0, 0) } }
                        },
                        OutputTileOffsetsByDirection = new()
                        {
                            { BuildingDirection.Up, new List<Point> { new Point(0, 0) } },
                            { BuildingDirection.Down, new List<Point> { new Point(0, 0) } },
                            { BuildingDirection.Left, new List<Point> { new Point(0, 0) } },
                            { BuildingDirection.Right, new List<Point> { new Point(0, 0) } }
                        },
                        WorkSpeed = 1.0f,
                        BuildTime = 0,
                        AnimationMode = Tiles.TileAnimationMode.Rotate,
                        Create = (pos, dir) => new Conveyor(BuildType.Conveyor, pos, dir)
                    }
                },
                {
                    BuildType.ConveyorRightCurve,
                    new BuildingInfo()
                    {
                        TexturePaths = new()
                        {
                            { BuildingDirection.None, "Buildings/Logistics/Conveyors/ConveyorRightCurve" }
                        },
                        ActiveTexturePaths = new()
                        {

                        },
                        Type = BuildType.ConveyorRightCurve,
                        Width = 1,
                        Height = 1,
                        FrameCount = 5,
                        FrameTime = 0.25f,
                        SizeInTiles = new Point(1,1),
                        OccupiedTilesByDirection = null,
                        ReceivedDirections = new()
                        {
                            { BuildingDirection.Up, new List<BuildingDirection> {BuildingDirection.Down} },
                            { BuildingDirection.Down, new List<BuildingDirection> {BuildingDirection.Up} },
                            { BuildingDirection.Left, new List<BuildingDirection> {BuildingDirection.Right} },
                            { BuildingDirection.Right, new List<BuildingDirection> {BuildingDirection.Left} }
                        },
                        OutputDirections = new()
                        {
                            { BuildingDirection.Up, new List<BuildingDirection> { BuildingDirection.Right } },
                            { BuildingDirection.Down, new List<BuildingDirection> { BuildingDirection.Left } },
                            { BuildingDirection.Left, new List<BuildingDirection> { BuildingDirection.Up } },
                            { BuildingDirection.Right, new List<BuildingDirection> { BuildingDirection.Down } }
                        },
                        ReceivedTileOffsetsByDirection = new()
                        {
                            { BuildingDirection.Up, new List<Point> { new Point(0, 0) } },
                            { BuildingDirection.Down, new List<Point> { new Point(0, 0) } },
                            { BuildingDirection.Left, new List<Point> { new Point(0, 0) } },
                            { BuildingDirection.Right, new List<Point> { new Point(0, 0) } }
                        },
                        OutputTileOffsetsByDirection = new()
                        {
                            { BuildingDirection.Up, new List<Point> { new Point(0, 0) } },
                            { BuildingDirection.Down, new List<Point> { new Point(0, 0) } },
                            { BuildingDirection.Left, new List<Point> { new Point(0, 0) } },
                            { BuildingDirection.Right, new List<Point> { new Point(0, 0) } }
                        },
                        WorkSpeed = 1.0f,
                        BuildTime = 0,
                        AnimationMode = Tiles.TileAnimationMode.Rotate,
                        Create = (pos, inDir) => new ConveyorRightCurve(BuildType.ConveyorRightCurve, pos, inDir)
                    }
                },
                {
                    BuildType.ConveyorLeftCurve,
                    new BuildingInfo()
                    {
                        TexturePaths = new()
                        {
                            {BuildingDirection.None, "Buildings/Logistics/Conveyors/ConveyorLeftCurve" }
                        },
                        ActiveTexturePaths = new()
                        {

                        },
                        Type = BuildType.ConveyorLeftCurve,
                        Width = 1,
                        Height = 1,
                        FrameCount = 5,
                        FrameTime = 0.25f,
                        SizeInTiles = new Point(1,1),
                        OccupiedTilesByDirection = null,
                        ReceivedDirections = new()
                        {
                            { BuildingDirection.Up, new List<BuildingDirection> {BuildingDirection.Down} },
                            { BuildingDirection.Down, new List<BuildingDirection> {BuildingDirection.Up} },
                            { BuildingDirection.Left, new List<BuildingDirection> {BuildingDirection.Right} },
                            { BuildingDirection.Right, new List<BuildingDirection> {BuildingDirection.Left} }
                        },
                        OutputDirections = new()
                        {
                            { BuildingDirection.Up, new List<BuildingDirection> { BuildingDirection.Left } },
                            { BuildingDirection.Down, new List<BuildingDirection> { BuildingDirection.Right } },
                            { BuildingDirection.Left, new List<BuildingDirection> { BuildingDirection.Down } },
                            { BuildingDirection.Right, new List<BuildingDirection> { BuildingDirection.Up } }
                        },
                        ReceivedTileOffsetsByDirection = new()
                        {
                            { BuildingDirection.Up, new List<Point> { new Point(0, 0) } },
                            { BuildingDirection.Down, new List<Point> { new Point(0, 0) } },
                            { BuildingDirection.Left, new List<Point> { new Point(0, 0) } },
                            { BuildingDirection.Right, new List<Point> { new Point(0, 0) } }
                        },
                        OutputTileOffsetsByDirection = new()
                        {
                            { BuildingDirection.Up, new List<Point> { new Point(0, 0) } },
                            { BuildingDirection.Down, new List<Point> { new Point(0, 0) } },
                            { BuildingDirection.Left, new List<Point> { new Point(0, 0) } },
                            { BuildingDirection.Right, new List<Point> { new Point(0, 0) } }
                        },
                        WorkSpeed = 1.0f,
                        BuildTime = 0,
                        AnimationMode = Tiles.TileAnimationMode.Rotate,
                        Create = (pos, inDir) => new ConveyorLeftCurve(BuildType.ConveyorLeftCurve, pos, inDir)
                    }
                },
                {
                    BuildType.ConveyorRightMerge,
                    new BuildingInfo()
                    {
                        TexturePaths = new ()
                        {
                            { BuildingDirection.None, "Buildings/Logistics/Conveyors/ConveyorRightMerge" }
                        },
                        ActiveTexturePaths = new()
                        {

                        },
                        Type = BuildType.ConveyorRightMerge,
                        Width = 1,
                        Height = 1,
                        FrameCount = 5,
                        FrameTime = 0.25f,
                        SizeInTiles = new Point(1,1),
                        OccupiedTilesByDirection = null,
                        ReceivedDirections = new()
                        {
                            { BuildingDirection.Up, new List<BuildingDirection> {BuildingDirection.Down, BuildingDirection.Right} },
                            { BuildingDirection.Down, new List<BuildingDirection> {BuildingDirection.Up, BuildingDirection.Left} },
                            { BuildingDirection.Left, new List<BuildingDirection> {BuildingDirection.Right, BuildingDirection.Up} },
                            { BuildingDirection.Right, new List<BuildingDirection> {BuildingDirection.Left, BuildingDirection.Down} }
                        },
                        OutputDirections = new()
                        {
                            { BuildingDirection.Up, new List<BuildingDirection> { BuildingDirection.Up } },
                            { BuildingDirection.Down, new List<BuildingDirection> { BuildingDirection.Down } },
                            { BuildingDirection.Left, new List<BuildingDirection> { BuildingDirection.Left } },
                            { BuildingDirection.Right, new List<BuildingDirection> { BuildingDirection.Right } }
                        },
                        ReceivedTileOffsetsByDirection = new()
                        {
                            { BuildingDirection.Up, new List<Point> { new Point(0, 0) } },
                            { BuildingDirection.Down, new List<Point> { new Point(0, 0) } },
                            { BuildingDirection.Left, new List<Point> { new Point(0, 0) } },
                            { BuildingDirection.Right, new List<Point> { new Point(0, 0) } }
                        },
                        OutputTileOffsetsByDirection = new()
                        {
                            { BuildingDirection.Up, new List<Point> { new Point(0, 0) } },
                            { BuildingDirection.Down, new List<Point> { new Point(0, 0) } },
                            { BuildingDirection.Left, new List<Point> { new Point(0, 0) } },
                            { BuildingDirection.Right, new List<Point> { new Point(0, 0) } }
                        },
                        WorkSpeed = 1.0f,
                        BuildTime = 0,
                        AnimationMode = Tiles.TileAnimationMode.Rotate,
                        Create = (pos, outDir) => new ConveyorRightMerge(BuildType.ConveyorRightMerge, pos, outDir)
                    }
                },
                {
                    BuildType.ConveyorLeftMerge,
                    new BuildingInfo()
                    {
                        TexturePaths = new ()
                        {
                            { BuildingDirection.None, "Buildings/Logistics/Conveyors/ConveyorLeftMerge" }
                        },
                        ActiveTexturePaths = new()
                        {

                        },
                        Type = BuildType.ConveyorLeftMerge,
                        Width = 1,
                        Height = 1,
                        FrameCount = 5,
                        FrameTime = 0.25f,
                        SizeInTiles = new Point(1,1),
                        OccupiedTilesByDirection = null,
                        ReceivedDirections = new()
                        {
                            { BuildingDirection.Up, new List<BuildingDirection> {BuildingDirection.Down, BuildingDirection.Left} },
                            { BuildingDirection.Down, new List<BuildingDirection> {BuildingDirection.Up, BuildingDirection.Right} },
                            { BuildingDirection.Left, new List<BuildingDirection> {BuildingDirection.Right, BuildingDirection.Down} },
                            { BuildingDirection.Right, new List<BuildingDirection> {BuildingDirection.Left, BuildingDirection.Up} }
                        },
                        OutputDirections = new()
                        {
                            { BuildingDirection.Up, new List<BuildingDirection> { BuildingDirection.Up } },
                            { BuildingDirection.Down, new List<BuildingDirection> { BuildingDirection.Down } },
                            { BuildingDirection.Left, new List<BuildingDirection> { BuildingDirection.Left } },
                            { BuildingDirection.Right, new List<BuildingDirection> { BuildingDirection.Right } }
                        },
                        ReceivedTileOffsetsByDirection = new()
                        {
                            { BuildingDirection.Up, new List<Point> { new Point(0, 0) } },
                            { BuildingDirection.Down, new List<Point> { new Point(0, 0) } },
                            { BuildingDirection.Left, new List<Point> { new Point(0, 0) } },
                            { BuildingDirection.Right, new List<Point> { new Point(0, 0) } }
                        },
                        OutputTileOffsetsByDirection = new()
                        {
                            { BuildingDirection.Up, new List<Point> { new Point(0, 0) } },
                            { BuildingDirection.Down, new List<Point> { new Point(0, 0) } },
                            { BuildingDirection.Left, new List<Point> { new Point(0, 0) } },
                            { BuildingDirection.Right, new List<Point> { new Point(0, 0) } }
                        },
                        WorkSpeed = 1.0f,
                        BuildTime= 0,
                        AnimationMode = Tiles.TileAnimationMode.Rotate,
                        Create = (pos, outDir) => new ConveyorLeftMerge(BuildType.ConveyorLeftMerge, pos, outDir)
                    }
                },

                {
                    BuildType.ConveyorRightLeftMerge,
                    new BuildingInfo()
                    {
                        TexturePaths = new Dictionary<BuildingDirection, string>()
                        {
                            { BuildingDirection.None, "Buildings/Logistics/Conveyors/ConveyorRightLeftMerge" }
                        },
                        ActiveTexturePaths = new()
                        {

                        },
                        Type =BuildType.ConveyorRightLeftMerge,
                        Width = 1,
                        Height = 1,
                        FrameCount = 5,
                        FrameTime = 0.25f,
                        SizeInTiles = new Point(1,1),
                        OccupiedTilesByDirection = null,
                        ReceivedDirections = new()
                        {
                            { BuildingDirection.Up, new List<BuildingDirection> {BuildingDirection.Left, BuildingDirection.Right} },
                            { BuildingDirection.Down, new List<BuildingDirection> {BuildingDirection.Right, BuildingDirection.Left} },
                            { BuildingDirection.Left, new List<BuildingDirection> {BuildingDirection.Down, BuildingDirection.Up} },
                            { BuildingDirection.Right, new List<BuildingDirection> {BuildingDirection.Up, BuildingDirection.Down} }
                        },
                        OutputDirections = new()
                        {
                            { BuildingDirection.Up, new List<BuildingDirection> { BuildingDirection.Up } },
                            { BuildingDirection.Down, new List<BuildingDirection> { BuildingDirection.Down } },
                            { BuildingDirection.Left, new List<BuildingDirection> { BuildingDirection.Left } },
                            { BuildingDirection.Right, new List<BuildingDirection> { BuildingDirection.Right } }
                        },
                        ReceivedTileOffsetsByDirection = new()
                        {
                            { BuildingDirection.Up, new List<Point> { new Point(0, 0) } },
                            { BuildingDirection.Down, new List<Point> { new Point(0, 0) } },
                            { BuildingDirection.Left, new List<Point> { new Point(0, 0) } },
                            { BuildingDirection.Right, new List<Point> { new Point(0, 0) } }
                        },
                        OutputTileOffsetsByDirection = new()
                        {
                            { BuildingDirection.Up, new List<Point> { new Point(0, 0) } },
                            { BuildingDirection.Down, new List<Point> { new Point(0, 0) } },
                            { BuildingDirection.Left, new List<Point> { new Point(0, 0) } },
                            { BuildingDirection.Right, new List<Point> { new Point(0, 0) } }
                        },
                        WorkSpeed = 1.0f,
                        BuildTime = 0,
                        AnimationMode = Tiles.TileAnimationMode.Rotate,
                        Create = (pos, outDir) => new ConveyorRightLeftMerge(BuildType.ConveyorMerge, pos, outDir)
                    }
                },

                {
                    BuildType.ConveyorMerge,
                    new BuildingInfo()
                    {
                        TexturePaths = new Dictionary<BuildingDirection, string>()
                        {
                            { BuildingDirection.None, "Buildings/Logistics/Conveyors/ConveyorMerge" }
                        },
                        ActiveTexturePaths = new()
                        {

                        },
                        Type = BuildType.ConveyorLeftMerge,
                        Width = 1,
                        Height = 1,
                        FrameCount = 5,
                        FrameTime = 0.25f,
                        SizeInTiles = new Point(1,1),
                        OccupiedTilesByDirection = null,
                        ReceivedDirections = new()
                        {
                            { BuildingDirection.Up, new List<BuildingDirection> {BuildingDirection.Left, BuildingDirection.Down, BuildingDirection.Right} },
                            { BuildingDirection.Down, new List<BuildingDirection> {BuildingDirection.Right, BuildingDirection.Up, BuildingDirection.Left} },
                            { BuildingDirection.Left, new List<BuildingDirection> {BuildingDirection.Down, BuildingDirection.Right, BuildingDirection.Up} },
                            { BuildingDirection.Right, new List<BuildingDirection> {BuildingDirection.Up, BuildingDirection.Left, BuildingDirection.Down} }
                        },
                        OutputDirections = new()
                        {
                            { BuildingDirection.Up, new List<BuildingDirection> { BuildingDirection.Up } },
                            { BuildingDirection.Down, new List<BuildingDirection> { BuildingDirection.Down } },
                            { BuildingDirection.Left, new List<BuildingDirection> { BuildingDirection.Left } },
                            { BuildingDirection.Right, new List<BuildingDirection> { BuildingDirection.Right } }
                        },
                        ReceivedTileOffsetsByDirection = new()
                        {
                            { BuildingDirection.Up, new List<Point> { new Point(0, 0) } },
                            { BuildingDirection.Down, new List<Point> { new Point(0, 0) } },
                            { BuildingDirection.Left, new List<Point> { new Point(0, 0) } },
                            { BuildingDirection.Right, new List<Point> { new Point(0, 0) } }
                        },
                        OutputTileOffsetsByDirection = new()
                        {
                            { BuildingDirection.Up, new List<Point> { new Point(0, 0) } },
                            { BuildingDirection.Down, new List<Point> { new Point(0, 0) } },
                            { BuildingDirection.Left, new List<Point> { new Point(0, 0) } },
                            { BuildingDirection.Right, new List<Point> { new Point(0, 0) } }
                        },
                        WorkSpeed = 1.0f,
                        BuildTime= 0,
                        AnimationMode = Tiles.TileAnimationMode.Rotate,
                        Create = (pos, outDir) => new ConveyorMerge(BuildType.ConveyorMerge, pos, outDir)
                    }
                },

                //Logistics.Splitters
                {
                    BuildType.Splitter,
                    new BuildingInfo()
                    {
                        TexturePaths= new Dictionary<BuildingDirection, string>()
                        {
                            { BuildingDirection.Up, "Buildings/Logistics/Splitters/Splitter_Idle_Up" },
                            { BuildingDirection.Left, "Buildings/Logistics/Splitters/Splitter_Idle_Left" },
                            { BuildingDirection.Down, "Buildings/Logistics/Splitters/Splitter_Idle_Down" },
                            { BuildingDirection.Right, "Buildings/Logistics/Splitters/Splitter_Idle_Right" }
                        },
                        ActiveTexturePaths = new Dictionary<BuildingDirection, string>()
                        {
                            { BuildingDirection.Up, "Buildings/Logistics/Splitters/Splitter_Active_Up" },
                            { BuildingDirection.Left, "Buildings/Logistics/Splitters/Splitter_Active_Left" },
                            { BuildingDirection.Down, "Buildings/Logistics/Splitters/Splitter_Active_Down" },
                            { BuildingDirection.Right, "Buildings/Logistics/Splitters/Splitter_Active_Right" }
                        },
                        Type = BuildType.Splitter,
                        Width = 1,
                        Height = 1,
                        FrameCount = 1,
                        FrameTime = 0.25f,
                        SizeInTiles= new Point(1,1),
                        OccupiedTilesByDirection = null,
                        ReceivedDirections = new()
                        {
                            { BuildingDirection.Up, new List<BuildingDirection> { BuildingDirection.Down } },
                            { BuildingDirection.Down, new List<BuildingDirection> { BuildingDirection.Up } },
                            { BuildingDirection.Left, new List<BuildingDirection> { BuildingDirection.Right } },
                            { BuildingDirection.Right, new List<BuildingDirection> { BuildingDirection.Left } }
                        },
                        OutputDirections = new()
                        {
                            { BuildingDirection.Up, new List<BuildingDirection> { BuildingDirection.Left, BuildingDirection.Down, BuildingDirection.Right } },
                            { BuildingDirection.Down, new List<BuildingDirection> { BuildingDirection.Right, BuildingDirection.Up, BuildingDirection.Left } },
                            { BuildingDirection.Left, new List<BuildingDirection> { BuildingDirection.Down, BuildingDirection.Right, BuildingDirection.Up } },
                            { BuildingDirection.Right, new List<BuildingDirection> { BuildingDirection.Up, BuildingDirection.Left, BuildingDirection.Down } }
                        },
                        ReceivedTileOffsetsByDirection = new()
                        {
                            { BuildingDirection.Up, new List<Point> { new Point(0, 0) } },
                            { BuildingDirection.Down, new List<Point> { new Point(0, 0) } },
                            { BuildingDirection.Left, new List<Point> { new Point(0, 0) } },
                            { BuildingDirection.Right, new List<Point> { new Point(0, 0) } }
                        },
                        OutputTileOffsetsByDirection = new()
                        {
                            { BuildingDirection.Up, new List<Point> { new Point(0, 0) } },
                            { BuildingDirection.Down, new List<Point> { new Point(0, 0) } },
                            { BuildingDirection.Left, new List<Point> { new Point(0, 0) } },
                            { BuildingDirection.Right, new List<Point> { new Point(0, 0) } }
                        },
                        WorkSpeed= 1.0f,
                        BuildTime = 0,
                        AnimationMode = Tiles.TileAnimationMode.Flip,
                        Create = (pos, inDir) => new Splitter(BuildType.Splitter, pos, inDir)
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

                foreach (var kv in info.ActiveTexturePaths)
                {
                    var dir = kv.Key;
                    var path = kv.Value;

                    info.CachedActiveTextures[dir] = ContentLoader.LoadTexture(path);
                }
            }
        }
    }
}
