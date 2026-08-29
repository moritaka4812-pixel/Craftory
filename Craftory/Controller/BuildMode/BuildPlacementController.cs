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

        public void OnClick(Point p, Tile tile, Vector2 worldPos, BuildType type, BuildingDirection direction)
        {
            var info = BuildingRegistry.Data[type];
            var candidate = new BuildCandidate { Origin = p, Type = type, Direction = direction };

            preview.AddPreview(p, candidate, info);

            lastDragOrigin = p;
            dragConveyorDirection = null;
        }

        public void OnDrag(Point p, Tile tile, Vector2 worldPos, BuildType type, BuildingDirection direction)
        {
            if (type == BuildType.Conveyor)
            {
                HandleConveyorDrag(p);
            }
            else
            {
                HandleNormalDrag(p, type, direction);
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
                    Type = BuildType.Conveyor,
                    Direction = dir
                };

                var info = BuildingRegistry.Data[BuildType.Conveyor];
                preview.AddPreview(p, candidate, info);
            }

            lastDragOrigin = current;
        }

        private void HandleNormalDrag(Point p, BuildType type, BuildingDirection direction)
        {
            if (lastDragOrigin == p) return;

            var info = BuildingRegistry.Data[type];
            var candidate = new BuildCandidate { Origin = p, Type = type, Direction = direction };

            preview.AddPreview(p, candidate, info);

            lastDragOrigin = p;
        }
    }
}
