
using Craftory.Core;
using Craftory.GameUI;
using Craftory.Input;
using Craftory.Maps;
using Craftory.Screens;

namespace Craftory.Controller
{
    public class DeleteModeController : IToolController
    {
        public DeleteModeController(MapManager map, ToolPanel panel, Game1 game, Camera camera, GamePlayScreen screen) 
        {

        }
        public bool Update(MouseInput mouse, bool uiConsumed)
        {
            return false;
        }

        public void Draw(SpriteBatch sb)
        {

        }
    }
}
