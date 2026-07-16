
using Craftory.Input;

namespace Craftory.Controller
{
    public interface IToolController
    {
        public bool Update(MouseInput mouse, bool uiConsumed);
        public void Draw(SpriteBatch sb);
    }
}
