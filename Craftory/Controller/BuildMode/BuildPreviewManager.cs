using Craftory.Maps;
using Craftory.Maps.Buildings;
using Microsoft.Xna.Framework.Graphics;
using Point = Microsoft.Xna.Framework.Point;
using Color = Microsoft.Xna.Framework.Color;

namespace Craftory.Controller.BuildMode
{
    public class BuildPreviewManager
    {
        private MapManager mapManager;

        private List<BuildCandidate> buildTargets = new();
        private List<BuildCandidate> invalidTargets = new();

        private bool[,] previewOccupied;
        private List<BuildCandidate>[,] previewOwner;
        private BuildPlacementValidator validator;

        public BuildPreviewManager(MapManager mapManager)
        {
            this.mapManager = mapManager;

            ResetAll();

            validator = new BuildPlacementValidator(mapManager.Map, previewOccupied);
        }

        public void ResetAll()
        {
            buildTargets.Clear();
            invalidTargets.Clear();

            previewOccupied = new bool[mapManager.Map.MapSizeX, mapManager.Map.MapSizeY];

            previewOwner = new List<BuildCandidate>[mapManager.Map.MapSizeX, mapManager.Map.MapSizeY];

            for (int x = 0; x < mapManager.Map.MapSizeX; x++)
                for (int y = 0; y < mapManager.Map.MapSizeY; y++)
                    previewOwner[x, y] = new List<BuildCandidate>();

            // ★ これが必要
            validator = new BuildPlacementValidator(mapManager.Map, previewOccupied);
        }


        public bool[,] GetOccupiedMap() => previewOccupied;

        public void AddPreview(Point p, BuildCandidate candidate, BuildingInfo info)
        {
            var owners = previewOwner[p.X, p.Y];
            if (owners.Count > 0)
            {
                var origin = owners.Last();
                RemovePreview(origin, info);
                return;
            }

            bool canPlace = validator.CanPlace(info, p);

            if (canPlace)
                buildTargets.Add(candidate);
            else
                invalidTargets.Add(candidate);

            foreach (var pos in info.GetArea(p))
            {
                previewOccupied[pos.X, pos.Y] = true;
                previewOwner[pos.X, pos.Y].Add(candidate);
            }
        }

        public void RemovePreview(BuildCandidate origin, BuildingInfo info)
        {
            buildTargets.Remove(origin);
            invalidTargets.Remove(origin);

            for (int x = 0; x < info.SizeInTiles.X; x++)
            {
                for (int y = 0; y < info.SizeInTiles.Y; y++)
                {
                    var pos = new Point(origin.Origin.X + x, origin.Origin.Y + y);

                    previewOwner[pos.X, pos.Y].Remove(origin);
                    previewOccupied[pos.X, pos.Y] = previewOwner[pos.X, pos.Y].Count > 0;
                }
            }
        }

        public void ReplacePreview(Point origin, BuildingDirection newDir)
        {
            var info = BuildingRegistry.Data[BuildType.Conveyor];

            var oldCandidate = new BuildCandidate
            {
                Origin = origin,
                Type = BuildType.Conveyor,
                Direction = BuildingDirection.Up // 旧方向は不要、RemovePreview が処理する
            };

            RemovePreview(oldCandidate, info);

            var newCandidate = new BuildCandidate
            {
                Origin = origin,
                Type = BuildType.Conveyor,
                Direction = newDir
            };

            AddPreview(origin, newCandidate, info);
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (var c in buildTargets)
                BuildingRegistry.Data[c.Type].DrawPreview(sb, c.Origin, c.Direction, Color.White * 0.5f);

            foreach (var c in invalidTargets)
                BuildingRegistry.Data[c.Type].DrawPreview(sb, c.Origin, c.Direction, Color.Red * 0.5f);
        }

        public void ApplyToMap(MapManager mapManager)
        {
            foreach (var c in buildTargets)
                mapManager.AddBuilding(c.Type, c.Origin, c.Direction);
        }
    }
}
