using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;
using Craftory.Maps.Buildings;
using Craftory.GameUI;
using Craftory.Maps;
using Craftory.Input;
using Craftory.Core;
using Craftory.Maps.Tiles;
using Craftory.Screens;

public struct BuildCandidate
{
    public Point Origin;
    public BuildType Type;
    public BuildingDirection Direction;
}

namespace Craftory.Controller
{
    public class BuildModeController : IToolController
    {
        public bool IsActive { get; private set; }

        private WorldUIFactory worldui;

        private WorldButton rotateButton;

        private Camera camera;
        private ToolPanel toolPanel;
        private MapManager mapManager;
        private BuildType currentBuildType;
        private List<BuildCandidate> buildTargets = new(); //建設する位置の一時リスト
        private List<BuildCandidate> invalidTargets = new(); //建設不可の一時リスト
        private bool[,] previewOccupied;
        private List<BuildCandidate>[,] previewOwner;
        private Vector2 confirmButtonWorldPos;
        private BuildPlacementValidator validator;
        private Point? lastDragOrigin = null;
        private BuildingDirection direction;

        public WorldPanel confirmPanel;

        public BuildModeController(MapManager mapManager, ToolPanel toolPanel, Game1 game, Camera camera, GamePlayScreen screen)
        {
            this.camera = camera;
            this.mapManager = mapManager;
            this.toolPanel = toolPanel;

            this.worldui = new WorldUIFactory(game, camera);

            confirmPanel = worldui.CreateWorldPanel(120, 40);

            var okButton = worldui.CreateWorldTextButton("o", 0, 0, 40, 40);
            var cancelButton = worldui.CreateWorldTextButton("x", 40, 0, 40, 40);
            rotateButton = worldui.CreateWorldTextButton("↑", 80, 0, 40, 40);

            okButton.LeftClicked += () => screen.toolControllerManager.Build.Confirm();
            cancelButton.LeftClicked += () => screen.toolControllerManager.Build.Cancel();
            rotateButton.LeftClicked += RotateDirection;

            confirmPanel.AddChild(okButton);
            confirmPanel.AddChild(cancelButton);
            confirmPanel.AddChild(rotateButton);
        }

        public void Start(BuildType type)
        {
            IsActive = true;
            currentBuildType = type;
            buildTargets.Clear();
            invalidTargets.Clear();

            confirmPanel.Visible = true;
            confirmButtonWorldPos.X = camera.Position.X;
            confirmButtonWorldPos.Y = camera.Position.Y;

            direction = BuildingDirection.Up;
            rotateButton.SetText("↑");
            previewOccupied = new bool[mapManager.Map.MapSizeX, mapManager.Map.MapSizeY];
            previewOwner = new List<BuildCandidate>[mapManager.Map.MapSizeX, mapManager.Map.MapSizeY];
            for (int x = 0; x < mapManager.Map.MapSizeX; x++)
                for (int y = 0; y < mapManager.Map.MapSizeY; y++)
                    previewOwner[x, y] = new List<BuildCandidate>();

            validator = new BuildPlacementValidator(mapManager.Map, previewOccupied);
        }

        public void SetCurrentType(BuildType type)
        {
            currentBuildType = type;
        }

        public void Cancel()
        {
            confirmPanel.Visible = false;
            IsActive = false;
            buildTargets.Clear();
            invalidTargets.Clear();
            toolPanel.ClearActiveButton();
        }

        public void Confirm()
        {
            if (buildTargets.Count == 0) return;
            foreach (var c in buildTargets)
                mapManager.AddBuilding(c.Type, c.Origin, c.Direction);

            confirmPanel.Visible = false;
            IsActive = false;
            buildTargets.Clear();
            invalidTargets.Clear();
            toolPanel.ClearActiveButton();
            mapManager.shadowGenerator.MarkDirty();
        }

        public bool Update(MouseInput mouse, bool uiConsumed)
        {
            if (uiConsumed) return true;

            //confirmPanelが入力を消費
            if (confirmPanel.UpdateWorld(mouse)) return true;

            var worldPos = camera.ScreenToWorld(mouse.Current.Position.ToVector2());

            var tilePos = mapManager.Map.WorldToTile(worldPos);
            if (tilePos == null) return false;

            var p = tilePos.Value;
            var tile = mapManager.Map.GetTile(p.X, p.Y);

            //左クリック
            if (mouse.LeftClicked())
            {
                HandleBuildTarget(p, tile, worldPos);
            }

            //左ドラッグ
            if (mouse.LeftDragging())
            {
                var candidate = new BuildCandidate
                {
                    Origin = p,
                    Type = currentBuildType,
                    Direction = direction
                };

                if (lastDragOrigin != p && //前のタイルと別のタイルをドラッグしている
                    !buildTargets.Contains(candidate) && //BuildTargetに含まれない
                    !invalidTargets.Contains(candidate)) //InvalidTargetにも含まれない
                {
                    bool canPlace = validator.CanPlace(BuildingRegistry.Data[currentBuildType], p); //配置が可能か

                    if (canPlace)
                    {
                        HandleBuildTarget(p, tile, worldPos);
                        lastDragOrigin = p; //ドラッグの前タイルを格納
                    }
                    else
                    {
                        lastDragOrigin = p; //追加するタイルではなかったが前タイルは格納
                    }
                }
            }
            else
            {
                lastDragOrigin = null; //マウス右ドラッグされていないならドラッグの前タイルはなし
            }

            confirmPanel.X = (int)confirmButtonWorldPos.X;
            confirmPanel.Y = (int)confirmButtonWorldPos.Y;

            return false;
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (var c in buildTargets)
            {
                var info = BuildingRegistry.Data[c.Type];
                info.DrawPreview(sb, c.Origin, c.Direction, Color.White * 0.5f);
            }
            foreach (var c in invalidTargets)
            {
                var info = BuildingRegistry.Data[c.Type];
                info.DrawPreview(sb, c.Origin, c.Direction, Color.Red * 0.5f);
            }
            confirmPanel.DrawWorld(sb);
        }

        private void HandleBuildTarget(Point p, Tile tile, Vector2 worldPos)
        {
            confirmPanel.Visible = true;
            var info = BuildingRegistry.Data[currentBuildType];

            var candidate = new BuildCandidate
            {
                Origin = p,
                Type = currentBuildType,
                Direction = direction
            };

            bool canPlace = validator.CanPlace(info, p); //タイルが設置可能かを判定


            // pが既存のプレビュー建物の占有タイルか
            var owners = previewOwner[p.X, p.Y];
            if (owners.Count > 0)
            {
                //最後に追加された建物を優先して消去
                var origin = owners.Last();
                RemovePreviewBuilding(origin, info);
                return;
            }

            if (canPlace) //建設可能
                buildTargets.Add(candidate);
            else //建設不可
                invalidTargets.Add(candidate);

            //仮想マップに追加
            foreach (var pos in info.GetArea(p))
            {
                previewOccupied[pos.X, pos.Y] = true;
                previewOwner[pos.X, pos.Y].Add(candidate);
            }

            confirmButtonWorldPos = worldPos + new Vector2(10, 10);
        }

        private void RemovePreviewBuilding(BuildCandidate origin, BuildingInfo info) 
        {
            buildTargets.Remove(origin);
            invalidTargets.Remove(origin);

            for (int x = 0; x < info.SizeInTiles.X; x++)
            {
                for (int y = 0; y < info.SizeInTiles.Y; y++)
                {
                    var pos = new Point(origin.Origin.X + x, origin.Origin.Y + y);

                    previewOwner[pos.X, pos.Y].Remove(origin);

                    //他の建物が残っていればoccupiedのまま
                    previewOccupied[pos.X, pos.Y] = previewOwner[pos.X, pos.Y].Count > 0;
                }
            }
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
