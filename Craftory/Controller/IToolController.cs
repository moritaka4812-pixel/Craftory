
using Craftory.Input;

namespace Craftory.Controller
{
    public interface IToolController
    {
        bool IsActive { get; }
        bool Update(MouseInput mouse, bool uiConsumed);

        void DrawWorld(SpriteBatch sb); // ← ワールド描画専用
        void DrawUI(SpriteBatch sb);    // ← UI描画専用
    }

}
