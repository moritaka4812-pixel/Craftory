using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;
using Button = Craftory.UI.Elements.Button;
using Panel = Craftory.UI.Elements.Panel;
using Craftory.Maps.Buildings;
using Craftory.GameUI;
using Craftory.Maps;
using Craftory.Input;
using Craftory.Core;
using Craftory.Maps.Tiles;
using Craftory.Screens;
using Craftory.UI.Elements;
using Craftory.UI.Core;

public struct BuildCandidate
{
    public Point Origin;
    public BuildType Type;
    public BuildingDirection Direction;
}

namespace Craftory.Controller.BuildMode
{
    public class BuildModeController : IToolController
    {
        public bool IsActive { get; private set; }

        private UIFactory ui;
        private Button rotateButton;

        private Camera camera;
        private ToolPanel toolPanel;
        private MapManager mapManager;

        private BuildPreviewManager preview;
        private BuildPlacementController placement;

        private BuildType currentBuildType;
        private BuildingDirection direction;

        private Vector2 confirmButtonWorldPos;
        public ConfirmPanel confirmPanel;

        public BuildModeController(MapManager mapManager, ToolPanel toolPanel, Game1 game, Camera camera, GamePlayScreen screen)
        {
            this.camera = camera;
            this.mapManager = mapManager;
            this.toolPanel = toolPanel;

            preview = new BuildPreviewManager(mapManager);
            placement = new BuildPlacementController(mapManager, preview);

            ui = new UIFactory(game);

            confirmPanel = new ConfirmPanel(120, 40);

            var okButton = ui.CreateTextButton("o", 0, 0, 40, 40);
            var cancelButton = ui.CreateTextButton("x", 40, 0, 40, 40);
            rotateButton = ui.CreateTextButton("↑", 80, 0, 40, 40);

            okButton.LeftClicked += () => screen.toolControllerManager.Build.Confirm();
            cancelButton.LeftClicked += () => screen.toolControllerManager.Build.Cancel();
            rotateButton.LeftClicked += RotateDirection;

            
            okButton.IgnoreLayoutX = true;
            okButton.IgnoreLayoutY = true;
            cancelButton.IgnoreLayoutX = true;
            cancelButton.IgnoreLayoutY = true;
            rotateButton.IgnoreLayoutX = true;
            rotateButton.IgnoreLayoutY = true;

            confirmPanel.AddChild(okButton);
            confirmPanel.AddChild(cancelButton);
            confirmPanel.AddChild(rotateButton);
        }

        public void SetCurrentType(BuildType type)
        {
            currentBuildType = type;
            placement.Reset();
        }

        public void Start(BuildType type)
        {
            IsActive = true;
            currentBuildType = type;

            preview.ResetAll();
            placement.Reset();

            confirmPanel.Visible = true;
            confirmButtonWorldPos = camera.Position;

            direction = BuildingDirection.Up;
            rotateButton.SetText("↑");
        }

        public void Cancel()
        {
            IsActive = false;
            preview.ResetAll();
            placement.Reset();
            confirmPanel.Visible = false;
            toolPanel.ClearActiveButton();
        }

        public void Confirm()
        {
            preview.ApplyToMap(mapManager);
            Cancel();
            mapManager.shadowGenerator.MarkDirty();
        }

        public bool Update(MouseInput mouse, bool uiConsumed)
        {
            if (uiConsumed) return true;

            if (confirmPanel.Update(mouse)) return true;

            var worldPos = camera.ScreenToWorld(mouse.Current.Position.ToVector2());
            var tilePos = mapManager.Map.WorldToTile(worldPos);
            if (tilePos == null) return false;

            var p = tilePos.Value;
            var tile = mapManager.Map.GetTile(p.X, p.Y);

            if (mouse.LeftClicked())
            {
                placement.OnClick(p, tile, worldPos, currentBuildType, direction);
                confirmButtonWorldPos = worldPos + new Vector2(10, 10);
            }

            if (mouse.LeftDragging())
            {
                placement.OnDrag(p, tile, worldPos, currentBuildType, direction);
                confirmButtonWorldPos = worldPos + new Vector2(10, 10);
            }
            
            if(mouse.LeftReleased())
            {
                placement.OnDragEnd();
            }

            var screenPos = camera.WorldToScreen(confirmButtonWorldPos);
            confirmPanel.X = (int)screenPos.X;
            confirmPanel.Y = (int)screenPos.Y;


            return false;
        }

        public void DrawWorld(SpriteBatch sb)
        {
            preview.Draw(sb);
        }

        public void DrawUI(SpriteBatch sb)
        {
            confirmPanel.Draw(sb);
        }



        private void RotateDirection()
        {
            switch (direction)
            {
                case BuildingDirection.Up:
                    direction = BuildingDirection.Right;
                    rotateButton.SetText("→");
                    break;
                case BuildingDirection.Right:
                    direction = BuildingDirection.Down;
                    rotateButton.SetText("↓");
                    break;
                case BuildingDirection.Down:
                    direction = BuildingDirection.Left;
                    rotateButton.SetText("←");
                    break;
                case BuildingDirection.Left:
                    direction = BuildingDirection.Up;
                    rotateButton.SetText("↑");
                    break;
            }
        }
    }
}


