using Craftory.Core;
using Craftory.GameUI;
using Craftory.Input;
using Craftory.Maps;
using Craftory.Maps.Buildings;
using Craftory.Screens;
using Craftory.Controller.BuildMode;
using Craftory.UI.Elements;

namespace Craftory.Controller
{
    public class ToolControllerManager
    {
        public IToolController CurrentController { get; private set; }

        public EmptyController Empty { get; }
        public BuildModeController Build { get; }
        public DeleteModeController Delete { get; }
        public ToolControllerManager(MapManager map, ToolPanel panel, Game1 game, Camera camera, GamePlayScreen screen)
        {
            Empty = new EmptyController();
            Build = new BuildModeController(map, panel, game, camera, screen);
            Delete = new DeleteModeController(map, panel, game, camera, screen);

            CurrentController = Empty;
        }
        public void SetMode(GameMode mode)
        {
            switch (mode)
            {
                case GameMode.None: CurrentController = Empty; break;
                case GameMode.Build: CurrentController = Build; break;
                case GameMode.Delete: CurrentController = Delete; break;
            }
        }

        public bool Update(MouseInput mouse, bool uiConsumed)
        {
            bool consumed = (bool)CurrentController?.Update(mouse, uiConsumed);

            if (!CurrentController.IsActive)
            {
                Reset();
            }

            return consumed;
        }

        public void DrawUI(SpriteBatch sb)
        {
            CurrentController?.DrawUI(sb);
        }

        public void DrawWorld(SpriteBatch sb)
        {
            CurrentController?.DrawWorld(sb);
        }

        public void Reset()
        {
            SetMode(GameMode.None);
        }

        public void StartBuild(BuildType type)
        {
            SetMode(GameMode.Build);
            Build.Start(type);
        }

        public void StartRemove()
        {
            SetMode(GameMode.Delete);
            Delete.Start();
        }
    }
}
