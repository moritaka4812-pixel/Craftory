using ResourceMiningGame.Core;
using ResourceMiningGame.Input;
using SharpDX.XInput;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace ResourceMiningGame.Controller
{
    public class CameraController //入力をカメラ操作用の値に変換するクラス
    {
        public Vector2 MoveDirection { get; private set; }
        public float ZoomDelta { get; private set; }
        public Vector2 DragDelta { get; private set; }

        public void Update(InputManager input) //カメラ操作の意図[移動・ズーム・ドラッグ]を更新
        {
            // Zoom
            int scroll = input.Mouse.ScrollDelta();
            ZoomDelta = scroll > 0 ? 0.1f : scroll < 0 ? -0.1f : 0f;

            //WASD
            Vector2 move = Vector2.Zero;
            if (input.keyboard.IsDown(Keys.W)) move.Y -= 1;
            if (input.keyboard.IsDown(Keys.S)) move.Y += 1;
            if (input.keyboard.IsDown(Keys.A)) move.X -= 1;
            if (input.keyboard.IsDown(Keys.D)) move.X += 1;
            MoveDirection = move;

            // Drag
            if (input.Mouse.Current.MiddleButton == ButtonState.Pressed)
                DragDelta = input.Mouse.PointDelta().ToVector2();
            else
                DragDelta = Vector2.Zero;
        }

        public void ApplyToCamera(Camera camera, float dt)　//カメラにUpdateで更新したカメラ操作の意図をcameraに適用する
        {
            camera.ZoomBy(ZoomDelta); //Zoom量だけZoom

            if (MoveDirection != Vector2.Zero) //キーが押されていたら
                camera.Move(MoveDirection * 500f * dt / camera.Zoom); //その方向に移動(500fはワールド座標での補正量 1f = 1px)

            if (DragDelta != Vector2.Zero) //マウスがミドルキーを押しながらドラッグされていたら
                camera.Drag(DragDelta); //カメラ自体がZoomを補正して移動
        }
    }
}
