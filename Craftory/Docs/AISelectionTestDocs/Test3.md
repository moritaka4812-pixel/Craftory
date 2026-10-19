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
                以下コード略

以上のBuildingRegistryの情報を元にCSVファイルを使ったデータ読みだしの設計にした場合、どのような設計にするべきですか？
回答は簡潔にしてください。
原因候補と根拠を優先してください。
不明な場合は仮説として提示してください。