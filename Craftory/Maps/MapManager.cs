
using Craftory.Core;
using Craftory.Maps.Buildings;
using Craftory.Maps.Buildings.Logistics.Conveyors;
using Craftory.Maps.Buildings.Logistics.Splitters;
using Craftory.Maps.Shadow;
using Craftory.Maps.Tiles;
using System.Diagnostics;
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

        public void AddBuilding(string Id, Point tilePos, BuildingDirection dir)
        {
            var building = BuildingRegistry.Data[Id].Create(tilePos, dir);
            
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

            ConnectionManager.RefreshConnections(building);
            ConnectionManager.RefreshNeighborConnections(building);
        }

        public void RemoveBuildingAt(Point tilePos)
        {
            var building = GetBuildingAt(tilePos);
            if (building == null) return;

            foreach (var pos in building.OccupiedTiles)
            {
                var tile = Map.GetTile(pos.X, pos.Y);
                tile.Occupant = null;
                tile.ShadowSources.RemoveAll(s => s.Type == ShadowSourceType.Building);
            }

            Buildings.Remove(building);

            ConnectionManager.RefreshNeighborConnections(building);
        }

        public BuildingInstance? GetBuildingAt(Point tilePos)
        {
            return Buildings.FirstOrDefault(b => b.TilePosition == tilePos);
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
                if (b is IPureConveyor)
                    b.Draw(sb, camera);

            //アイテム
            foreach (var b in Buildings)
                if (b is Conveyor conveyor)
                    conveyor.TileLogic.Draw(sb, new Vector2(conveyor.TilePosition.X * 32, conveyor.TilePosition.Y * 32));

            // 特殊系コンベア（Splitter / Sorter）
            foreach (var b in Buildings)
                if (b is ISpecialConveyor)
                    b.Draw(sb, camera);

            //その他の建物
            foreach (var b in Buildings)
                if (b is not Conveyor)
                    b.Draw(sb, camera);
        }
    }
}

