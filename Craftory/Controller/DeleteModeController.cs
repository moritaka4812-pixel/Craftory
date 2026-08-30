using Craftory.Core;
using Craftory.GameUI;
using Craftory.Input;
using Craftory.Maps;
using Craftory.Screens;
using Craftory.UI.Core;
using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;

namespace Craftory.Controller
{
    public class DeleteModeController : IToolController
    {
        public bool IsActive { get; private set; }
        private MapManager mapManager;
        private ToolPanel toolPanel;
        private Camera camera;

        private List<Point> deleteTargets = new();
        private bool[,] previewOccupied;
        private List<Point>[,] previewOwner;

        private ConfirmPanel confirmPanel;   // ★ BuildMode と同じ UIConfirmPanel
        private Vector2 confirmPanelPos;

        private Point? dragStart = null;

        public DeleteModeController(MapManager map, ToolPanel panel, Game1 game, Camera camera, GamePlayScreen screen)
        {
            this.mapManager = map;
            this.toolPanel = panel;
            this.camera = camera;

            var ui = new UIFactory(game);

            // ★ BuildMode と同じ ConfirmPanel を使う
            confirmPanel = new ConfirmPanel(80, 40);

            var okButton = ui.CreateTextButton("o", 0, 0, 40, 40);
            var cancelButton = ui.CreateTextButton("x", 40, 0, 40, 40);

            okButton.LeftClicked += ConfirmDelete;
            cancelButton.LeftClicked += CancelDelete;

            okButton.IgnoreLayoutX = true;
            okButton.IgnoreLayoutY = true;
            cancelButton.IgnoreLayoutX = true;
            cancelButton.IgnoreLayoutY = true;

            confirmPanel.AddChild(okButton);
            confirmPanel.AddChild(cancelButton);
        }

        public void Start()
        {
            IsActive = true;
            deleteTargets.Clear();

            previewOccupied = new bool[mapManager.Map.MapSizeX, mapManager.Map.MapSizeY];
            previewOwner = new List<Point>[mapManager.Map.MapSizeX, mapManager.Map.MapSizeY];

            for (int x = 0; x < mapManager.Map.MapSizeX; x++)
                for (int y = 0; y < mapManager.Map.MapSizeY; y++)
                    previewOwner[x, y] = new List<Point>();

            confirmPanel.Visible = true;
            confirmPanelPos = camera.Position;
        }

        public bool Update(MouseInput mouse, bool uiConsumed)
        {
            if (uiConsumed) return true;

            // ★ BuildMode と同じ Update
            if (confirmPanel.Update(mouse)) return true;

            var worldPos = camera.ScreenToWorld(mouse.Current.Position.ToVector2());
            var tilePos = mapManager.Map.WorldToTile(worldPos);
            if (tilePos == null) return false;

            var p = tilePos.Value;

            if (mouse.LeftClicked())
            {
                dragStart = p;
                HandleDeleteTarget(p);

                // ConfirmPanel の位置更新
                confirmPanelPos = worldPos + new Vector2(10, 10);
            }

            if (mouse.LeftDragging() && dragStart != null)
            {
                var start = dragStart.Value;
                var end = p;

                int minX = Math.Min(start.X, end.X);
                int maxX = Math.Max(start.X, end.X);
                int minY = Math.Min(start.Y, end.Y);
                int maxY = Math.Max(start.Y, end.Y);

                ApplyPreviewRectangle(minX, maxX, minY, maxY);

                confirmPanelPos = worldPos + new Vector2(10, 10);
            }

            // ★ World → Screen 座標変換
            var screenPos = camera.WorldToScreen(confirmPanelPos);
            confirmPanel.X = (int)screenPos.X;
            confirmPanel.Y = (int)screenPos.Y;

            return false;
        }

        private void HandleDeleteTarget(Point p)
        {
            var building = mapManager.GetBuildingAt(p);
            if (building == null)
                return;

            if (previewOccupied[p.X, p.Y])
            {
                RemovePreview(p);
                return;
            }

            deleteTargets.Add(p);
            previewOccupied[p.X, p.Y] = true;
            previewOwner[p.X, p.Y].Add(p);
        }

        private void ApplyPreviewRectangle(int minX, int maxX, int minY, int maxY)
        {
            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    var building = mapManager.GetBuildingAt(new Point(x, y));
                    if (building == null) continue;

                    if (!previewOccupied[x, y])
                    {
                        previewOccupied[x, y] = true;
                        deleteTargets.Add(new Point(x, y));
                    }
                }
            }
        }

        private void RemovePreview(Point p)
        {
            deleteTargets.Remove(p);
            previewOwner[p.X, p.Y].Clear();
            previewOccupied[p.X, p.Y] = false;
        }

        private void ConfirmDelete()
        {
            foreach (var p in deleteTargets)
                mapManager.RemoveBuildingAt(p);

            mapManager.shadowGenerator.MarkDirty();
            EndMode();
        }

        private void CancelDelete()
        {
            EndMode();
        }

        private void EndMode()
        {
            IsActive = false;
            deleteTargets.Clear();
            confirmPanel.Visible = false;
        }

        public void DrawWorld(SpriteBatch sb)
        {
            foreach (var p in deleteTargets)
            {
                var building = mapManager.GetBuildingAt(p);
                building?.info.DrawPreview(sb, building.TilePosition, building.buildingDirection, Color.Red * 0.5f);
            }
        }

        public void DrawUI(SpriteBatch sb)
        {
            confirmPanel.Draw(sb);
        }
    }
}
