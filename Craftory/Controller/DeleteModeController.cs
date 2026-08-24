
using Craftory.Core;
using Craftory.GameUI;
using Craftory.Input;
using Craftory.Maps;
using Craftory.Screens;
using Point = Microsoft.Xna.Framework.Point;
using Color = Microsoft.Xna.Framework.Color;

namespace Craftory.Controller
{
    public class DeleteModeController : IToolController
    {
        public bool IsActive { get; private set; }
        private MapManager mapManager;
        private ToolPanel toolPanel;
        private Camera camera;
        private WorldUIFactory worldui;
        private List<Point> deleteTargets = new();
        private bool[,] previewOccupied;
        private List<Point>[,] previewOwner;

        private WorldPanel confirmPanel;
        private Vector2 confirmPanelPos;

        public DeleteModeController(MapManager map, ToolPanel panel, Game1 game, Camera camera, GamePlayScreen screen) 
        {
            this.mapManager = map;
            this.toolPanel = panel;
            this.camera = camera;

            this.worldui = new WorldUIFactory(game, camera);

            confirmPanel = worldui.CreateWorldPanel(80, 40);

            var okButton = worldui.CreateWorldTextButton("o", 0, 0, 40, 40);
            var cancelButton = worldui.CreateWorldTextButton("x", 40, 0, 40, 40);

            okButton.LeftClicked += ConfirmDelete;
            cancelButton.LeftClicked += CancelDelete;

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
                for(int y = 0; y < mapManager.Map.MapSizeY; y++)
                    previewOwner[x, y] = new List<Point>();

            confirmPanel.Visible = true;
            confirmPanelPos = camera.Position;
        }

        public bool Update(MouseInput mouse, bool uiConsumed)
        {
            if (uiConsumed) return true;
            if(confirmPanel.UpdateWorld(mouse)) return true;

            var worldPos = camera.ScreenToWorld(mouse.Current.Position.ToVector2());
            var tilePos = mapManager.Map.WorldToTile(worldPos);
            if (tilePos == null) return false;

            var p = tilePos.Value;

            if(mouse.LeftClicked())
                HandleDeleteTarget(p);

            if (mouse.LeftDragging())
                HandleDeleteTarget(p);

            confirmPanel.X = (int)confirmPanelPos.X;
            confirmPanel.Y = (int)confirmPanelPos.Y;

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

        public void Draw(SpriteBatch sb)
        {
            foreach(var p in deleteTargets)
            {
                var building = mapManager.GetBuildingAt(p);
                building?.info.DrawPreview(sb, building.TilePosition, building.buildingDirection, Color.Red * 0.5f);
            }

            confirmPanel.DrawWorld(sb);
        }
    }
}
