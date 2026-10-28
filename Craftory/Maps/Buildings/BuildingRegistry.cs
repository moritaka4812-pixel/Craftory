using Point = Microsoft.Xna.Framework.Point;
using Splitter = Craftory.Maps.Buildings.Logistics.Splitters.Splitter;
using Craftory.Maps.Buildings.Miners;
using Craftory.Maps.Buildings.Logistics.Conveyors;
using Craftory.Maps.Buildings.Logistics.Splitters;

namespace Craftory.Maps.Buildings
{
    public static class BuildingRegistry
    {

        public static Dictionary<string, BuildingInfo> Data =
            new()
            {
                //Miners
                {
                    "Drill",
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
                        Create = (pos, dir) => new Drill("Drill", pos)
                    }
                },
                //Logistics.Conveyors
                {
                    "Conveyor_Straight",
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
                        Create = (pos, dir) => new Conveyor("Conveyor_Straight", pos, dir)
                    }
                },
                {
                    "Conveyor_RightCurve",
                    new BuildingInfo()
                    {
                        TexturePaths = new()
                        {
                            { BuildingDirection.None, "Buildings/Logistics/Conveyors/ConveyorRightCurve" }
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
                        Create = (pos, inDir) => new ConveyorRightCurve("Conveyor_RightCurve", pos, inDir)
                    }
                },
                {
                    "Conveyor_LeftCurve",
                    new BuildingInfo()
                    {
                        TexturePaths = new()
                        {
                            {BuildingDirection.None, "Buildings/Logistics/Conveyors/ConveyorLeftCurve" }
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
                        Create = (pos, inDir) => new ConveyorLeftCurve("Conveyor_LeftCurve", pos, inDir)
                    }
                },
                {
                    "Conveyor_RightMerge",
                    new BuildingInfo()
                    {
                        TexturePaths = new ()
                        {
                            { BuildingDirection.None, "Buildings/Logistics/Conveyors/ConveyorRightMerge" }
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
                        Create = (pos, outDir) => new ConveyorRightMerge("Conveyor_RightMerge", pos, outDir)
                    }
                },
                {
                    "Conveyor_LeftMerge",
                    new BuildingInfo()
                    {
                        TexturePaths = new ()
                        {
                            { BuildingDirection.None, "Buildings/Logistics/Conveyors/ConveyorLeftMerge" }
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
                                Name = "SubInput",
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
                        Create = (pos, outDir) => new ConveyorLeftMerge("Conveyor_LeftMerge", pos, outDir)
                    }
                },

                {
                    "Conveyor_RightLeftMerge",
                    new BuildingInfo()
                    {
                        TexturePaths = new Dictionary<BuildingDirection, string>()
                        {
                            { BuildingDirection.None, "Buildings/Logistics/Conveyors/ConveyorRightLeftMerge" }
                        },
                        ActiveTexturePaths = new()
                        {

                        },
                        Type =BuildType.Conveyor,
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
                        Create = (pos, outDir) => new ConveyorRightLeftMerge("Conveyor_RightLeftMerge", pos, outDir)
                    }
                },

                {
                    "Conveyor_Merge",
                    new BuildingInfo()
                    {
                        TexturePaths = new Dictionary<BuildingDirection, string>()
                        {
                            { BuildingDirection.None, "Buildings/Logistics/Conveyors/ConveyorMerge" }
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
                        Create = (pos, outDir) => new ConveyorMerge("Conveyor_Merge", pos, outDir)
                    }
                },

                //Logistics.Splitters
                {
                    "Splitter",
                    new BuildingInfo()
                    {
                        TexturePaths= new Dictionary<BuildingDirection, string>()
                        {
                            { BuildingDirection.None, "Buildings/Logistics/Splitters/Splitter_Idle_Right"}
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
                        AnimationMode = Tiles.TileAnimationMode.Rotate,
                        Create = (pos, inDir) => new Splitter("Splitter", pos, inDir)
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
