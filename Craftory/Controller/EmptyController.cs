
using Craftory.Input;

namespace Craftory.Controller
{
    public class EmptyController : IToolController
    {
        public bool IsActive => false;
        public bool Update(MouseInput mouse, bool uiConsumed) { return false; }
        public void DrawUI(SpriteBatch sb) { }
        public void DrawWorld(SpriteBatch sb) { }
    }
}
