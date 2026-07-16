
using Craftory.Input;

namespace Craftory.Controller
{
    public class EmptyController : IToolController
    {
        public bool Update(MouseInput mouse, bool uiConsumed) { return false; }
        public void Draw(SpriteBatch sb) { }
    }
}
