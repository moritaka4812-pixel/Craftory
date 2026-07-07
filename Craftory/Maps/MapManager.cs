
using Craftory.Core;
using Craftory.Maps.Buildings;
using Craftory.Maps.Buildings.Conveyors;
using Craftory.Maps.Shadow;
using Craftory.Maps.Tiles;
using Point = Microsoft.Xna.Framework.Point;

namespace Craftory.Maps
{
    public class MapManager
    {
        public IMap Map { get; set; }
        public List<BuildingInstance> Buildings { get; private set; }
        private TileAnimator tileAnimator;
        public MapShadowGenerator shadowGenerator {  get; private set; }

        public void MapSet(IMap map, GraphicsDevice graphics)
        {
            Map = map;
            Buildings = new();
            tileAnimator = new TileAnimator(map);
            shadowGenerator = new MapShadowGenerator(map, graphics);

        }

        public void AddBuilding(BuildType type, Point tilePos, BuildingDirection dir)
        {
            var building = BuildingRegistry.Data[type].Create(tilePos, dir);
            
            foreach(var pos in building.OccupiedTiles)
            {
                var tile = Map.GetTile(pos.X, pos.Y);
                tile.Occupant = building;
                tile.ShadowSources.Add(new ShadowSource
                {
                    Type = ShadowSourceType.Building,
                    Strength = 2
                });
            }

            Buildings.Add(building);
            NotifyNeighborsOfChange(tilePos);
        }

        public void Update(GameTime gameTime, Camera camera, GraphicsDevice device)
        {
            tileAnimator.UpdateVisibleTiles(gameTime, camera, device);
            Map.Update(gameTime);

            var range = Map.GetVisibleRange(camera, device);

            foreach(var b in Buildings)
            {
                b.UpdateLogic(gameTime);

                if (range.Contains(b.TilePosition))
                    b.UpdateVisual(gameTime);
            }
        }

        public void Draw(SpriteBatch sb, Camera camera)
        {
            var range = Map.GetVisibleRange(camera, sb.GraphicsDevice);
            //地形
            Map.Draw(sb, range);
            shadowGenerator.Draw(sb);

            //コンベア
            foreach (var b in Buildings)
                if (b is Conveyor)
                    b.Draw(sb, camera);

            //アイテム
            foreach(var b in Buildings)
                if(b is Conveyor conveyor)
                    conveyor.TileLogic.Draw(sb, new Vector2(conveyor.TilePosition.X * 32, conveyor.TilePosition.Y * 32));

            //その他の建物
            foreach (var b in Buildings)
                if (b is not Conveyor)
                    b.Draw(sb, camera);
        }

        private void NotifyNeighborsOfChange(Point pos)
        {
            var dirs = new (int x, int y)[]
            {
                (1,0), (-1,0), (0,1), (0,-1)
            };

            foreach (var d in dirs)
            {
                var npos = new Point(pos.X + d.x, pos.Y + d.y);
                var tile = Map.GetTile(npos.X, npos.Y);

                if (tile?.Occupant is Conveyor c)
                {
                    bool changed = false;

                    // 出力側の更新
                    if (c is ISplitConveyor split)
                    {
                        foreach (var nextPos in split.GetNextPositions())
                        {
                            if (nextPos == pos)
                            {
                                c.RefreshConnection();
                                changed = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        // 通常の1出力
                        if (c.GetNextPosition() == pos)
                        {
                            c.RefreshConnection();
                            changed = true;
                        }
                    }


                    // ★ 入力側の更新（Merge は複数の入力を持つのでループで判定）
                    foreach (var backPos in c.GetBackPositions())  // ← IEnumerable<Point> をループ
                    {
                        if (backPos == pos)
                        {
                            c.RefreshConnection();
                            changed = true;
                            break;
                        }
                    }

                    // ★ 接続が変わったなら TileStart を再計算
                    if (changed)
                    {
                        c.TileLogic.InitializeTileStart();

                        // Merge の場合は inputTiles を使って TileStart を再計算
                        if (c is IMergeConveyor merge)
                        {
                            merge.InitializeMergeTileStart();
                        }
                    }
                }
            }
        }

    }
}

