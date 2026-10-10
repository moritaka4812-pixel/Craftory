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
                        PortDefinitions = new List<PortDefinitions>()
                        {
                            new PortDefinitions
                            {
                                Name = "OutputUp",
                                Offset = new Point(0,0),
                                Direction = BuildingDirection.Up,
                                Pattern = PortPattern.Output
                            },
                            new PortDefinitions
                            {
                                Name = "OutputDown",
                                Offset = new Point(0, 0),
                                Direction = BuildingDirection.Down,
                                Pattern = PortPattern.Output
                            },

                            new PortDefinitions
                            {
                                Name = "OutputLeft",
                                Offset = new Point(0, 0),
                                Direction = BuildingDirection.Left,
                                Pattern = PortPattern.Output
                            },

                            new PortDefinitions
                            {
                                Name = "OutputRight",
                                Offset = new Point(0, 0),
                                Direction = BuildingDirection.Right,
                                Pattern = PortPattern.Output
                            }
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
                        PortDefinitions = new List<PortDefinitions>()
                        {
                            new PortDefinitions
                            {
                                Name = "Output",
                                Offset = new Point(0,0),
                                Direction = BuildingDirection.Right,
                                Pattern = PortPattern.Output
                            },
                            new PortDefinitions
                            {
                                Name = "Input",
                                Offset = new Point(0, 0),
                                Direction = BuildingDirection.Left,
                                Pattern = PortPattern.Input
                            }
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
                        PortDefinitions = new List<PortDefinitions>()
                        {
                            new PortDefinitions
                            {
                                Name = "Output",
                                Offset = new Point(0,0),
                                Direction = BuildingDirection.Down,
                                Pattern = PortPattern.Output
                            },
                            new PortDefinitions
                            {
                                Name = "Input",
                                Offset = new Point(0, 0),
                                Direction = BuildingDirection.Left,
                                Pattern = PortPattern.Input
                            }
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
                        PortDefinitions = new List<PortDefinitions>()
                        {
                            new PortDefinitions
                            {
                                Name = "Output",
                                Offset = new Point(0,0),
                                Direction = BuildingDirection.Up,
                                Pattern = PortPattern.Output
                            },
                            new PortDefinitions
                            {
                                Name = "Input",
                                Offset = new Point(0, 0),
                                Direction = BuildingDirection.Left,
                                Pattern = PortPattern.Input
                            }
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
                        PortDefinitions = new List<PortDefinitions>()
                        {
                            new PortDefinitions
                            {
                                Name = "Output",
                                Offset = new Point(0,0),
                                Direction = BuildingDirection.Right,
                                Pattern = PortPattern.Output
                            },
                            new PortDefinitions
                            {
                                Name = "SubInput",
                                Offset = new Point(0, 0),
                                Direction = BuildingDirection.Down,
                                Pattern = PortPattern.Input

                            },
                            new PortDefinitions
                            {
                                Name = "MainInput",
                                Offset = new Point(0, 0),
                                Direction = BuildingDirection.Left,
                                Pattern = PortPattern.Input
                            }
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
                        PortDefinitions = new List<PortDefinitions>()
                        {
                            new PortDefinitions
                            {
                                Name = "Output",
                                Offset = new Point(0,0),
                                Direction = BuildingDirection.Right,
                                Pattern = PortPattern.Output
                            },
                            new PortDefinitions
                            {
                                Name = "SubInput",
                                Offset = new Point(0, 0),
                                Direction = BuildingDirection.Down,
                                Pattern = PortPattern.Input

                            },
                            new PortDefinitions
                            {
                                Name = "MainInput",
                                Offset = new Point(0, 0),
                                Direction = BuildingDirection.Left,
                                Pattern = PortPattern.Input
                            }
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
                        PortDefinitions = new List<PortDefinitions>()
                        {
                            new PortDefinitions
                            {
                                Name = "Output",
                                Offset = new Point(0,0),
                                Direction = BuildingDirection.Right,
                                Pattern = PortPattern.Output
                            },
                            new PortDefinitions
                            {
                                Name = "RightInput",
                                Offset = new Point(0, 0),
                                Direction = BuildingDirection.Down,
                                Pattern = PortPattern.Input
                            },
                            new PortDefinitions
                            {
                                Name = "LeftInput",
                                Offset = new Point(0, 0),
                                Direction = BuildingDirection.Up,
                                Pattern = PortPattern.Input
                            }
                        },
                        WorkSpeed = 1.0f,
                        BuildTime = 0,
                        AnimationMode = Tiles.TileAnimationMode.Rotate,
                        Create = (pos, outDir) => new ConveyorRightLeftMerge(BuildType.ConveyorRightLeftMerge, pos, outDir)
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
                        Type = BuildType.ConveyorMerge,
                        Width = 1,
                        Height = 1,
                        FrameCount = 5,
                        FrameTime = 0.25f,
                        SizeInTiles = new Point(1,1),
                        OccupiedTilesByDirection = null,
                        PortDefinitions = new List<PortDefinitions>()
                        {
                            new PortDefinitions
                            {
                                Name = "Output",
                                Offset = new Point(0,0),
                                Direction = BuildingDirection.Right,
                                Pattern = PortPattern.Output
                            },
                            new PortDefinitions
                            {
                                Name = "RightInput",
                                Offset = new Point(0, 0),
                                Direction = BuildingDirection.Down,
                                Pattern = PortPattern.Input
                            },
                            new PortDefinitions
                            {
                                Name = "LeftInput",
                                Offset = new Point(0, 0),
                                Direction = BuildingDirection.Up,
                                Pattern = PortPattern.Input
                            },
                            new PortDefinitions
                            {
                                Name = "MainInput",
                                Offset = new Point(0, 0),
                                Direction = BuildingDirection.Left,
                                Pattern = PortPattern.Input
                            }
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
                        PortDefinitions = new List<PortDefinitions>()
                        {
                            new PortDefinitions
                            {
                                Name = "Output",
                                Offset = new Point(0,0),
                                Direction = BuildingDirection.Right,
                                Pattern = PortPattern.Output
                            },
                            new PortDefinitions
                            {
                                Name = "RightOutput",
                                Offset = new Point(0, 0),
                                Direction = BuildingDirection.Down,
                                Pattern = PortPattern.Output
                            },
                            new PortDefinitions
                            {
                                Name = "LeftOutput",
                                Offset = new Point(0, 0),
                                Direction = BuildingDirection.Up,
                                Pattern = PortPattern.Output
                            },
                            new PortDefinitions
                            {
                                Name = "Input",
                                Offset = new Point(0, 0),
                                Direction = BuildingDirection.Left,
                                Pattern = PortPattern.Input
                            }
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
