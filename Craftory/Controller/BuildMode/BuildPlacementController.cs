using Craftory.Maps;
using Craftory.Maps.Buildings;
using Craftory.Maps.Tiles;
using Point = Microsoft.Xna.Framework.Point;

namespace Craftory.Controller.BuildMode
{
    public class BuildPlacementController
    {
        private MapManager mapManager;
        private BuildPreviewManager preview;

        private Point? lastDragOrigin = null;
        private BuildingDirection? dragConveyorDirection = null;

        public BuildPlacementController(MapManager mapManager, BuildPreviewManager preview)
        {
            this.mapManager = mapManager;
            this.preview = preview;
        }

        public void Reset()
        {
            lastDragOrigin = null;
            dragConveyorDirection = null;
        }

        public void OnClick(Point p, Tile tile, Vector2 worldPos, string BuildingId, BuildingDirection direction)
        {
            var info = BuildingRegistry.Data[BuildingId];
            var candidate = new BuildCandidate { Origin = p, BuildingId = BuildingId, Direction = direction };

            preview.AddPreview(p, candidate, info);

            lastDragOrigin = p;
            dragConveyorDirection = null;
        }

        public void OnDrag(Point p, Tile tile, Vector2 worldPos, string BuildingId, BuildingDirection direction)
        {
            if (BuildingId == "Conveyor_Straight")
            {
                HandleConveyorDrag(p);
            }
            else
            {
                HandleNormalDrag(p, BuildingId, direction);
            }
        }

        public void OnDragEnd()
        {
            lastDragOrigin = null;
            dragConveyorDirection = null;
        }

        private void HandleConveyorDrag(Point current)
        {
            if (!lastDragOrigin.HasValue) return;

            var last = lastDragOrigin.Value;

            int dx = current.X - last.X;
            int dy = current.Y - last.Y;

            if (dragConveyorDirection == null)
            {
                if(dx == 0 && dy == 0) return;

                dragConveyorDirection =
                    Math.Abs(dx) > Math.Abs(dy)
                    ? (dx > 0 ? BuildingDirection.Right : BuildingDirection.Left)
                    : (dy > 0 ? BuildingDirection.Down : BuildingDirection.Up);

                preview.ReplacePreview(last, dragConveyorDirection.Value);
            }

            var dir = dragConveyorDirection.Value;

            int stepX = Math.Sign(dx);
            int stepY = Math.Sign(dy);
            int length = Math.Max(Math.Abs(dx), Math.Abs(dy));

            for (int i = 1; i <= length; i++)
            {
                var p = new Point(last.X + stepX * i, last.Y + stepY * i);

                var candidate = new BuildCandidate
                {
                    Origin = p,
                    BuildingId = "Conveyor_Straight",
                    Direction = dir
                };

                var info = BuildingRegistry.Data["Conveyor_Straight"];
                preview.AddPreview(p, candidate, info);
            }

            lastDragOrigin = current;
        }

        private void HandleNormalDrag(Point p, string BuildingId, BuildingDirection direction)
        {
            if (lastDragOrigin == p) return;

            var info = BuildingRegistry.Data[BuildingId];
            var candidate = new BuildCandidate { Origin = p, BuildingId = BuildingId, Direction = direction };

            preview.AddPreview(p, candidate, info);

            lastDragOrigin = p;
        }
    }
}
