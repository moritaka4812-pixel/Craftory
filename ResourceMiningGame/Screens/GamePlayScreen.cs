
using ResourceMiningGame.Core;
using ResourceMiningGame.Maps;
using ResourceMiningGame.Maps.Tiles;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace ResourceMiningGame.Screens
{
    public class GamePlayScreen : ScreenBase
    {
        private int prevScrollValue = 0;
        private Texture2D pixel;
        public Tile selectedTile = null;
        Camera camera;
        IMap map;
        public GamePlayScreen(Game1 game) : base (game) 
        {
            camera = new Camera(new Vector2(0f,0f));
            map = new Map1();
            this.LoadContent();
        }

        public void LoadContent()
        {
            map.LoadContent(game.Content);

            pixel = new Texture2D(game.GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
        }
        public override void Update(GameTime gameTime)
        {
            camera.Update(gameTime); //カメラ系のUpdate

            var mouse = Mouse.GetState();

            //タイル更新(アニメーション)
            var range = map.GetVisibleRange(camera, game.GraphicsDevice);

            for (int y = range.StartY; y < range.EndY; y++)
            {
                for (int x = range.StartX; x < range.EndX; x++)
                {
                    map.MapTiles[x, y].Update(gameTime);
                }
            }

            //左クリックでタイル選択
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                Vector2 screenPos = mouse.Position.ToVector2(); //スクリーン座標を取得

                Matrix inverse = Matrix.Invert(camera.GetViewMatrix()); //カメラ行列の逆行列を取得
                Vector2 worldPos = Vector2.Transform(screenPos, inverse);　//逆行列でワールド座標に変換

                //タイル座標に変換
                int tileX = (int)(worldPos.X / 16);
                int tileY = (int)(worldPos.Y / 16);

                //範囲チェック
                if(tileX >= 0 && tileX < map.MapSizeX && tileY >= 0 && tileY < map.MapSizeY)
                {
                    selectedTile = map.GetTile(tileX, tileY);
                }
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Begin(transformMatrix : camera.GetViewMatrix());

            var range = map.GetVisibleRange(camera, game.GraphicsDevice);
            map.Draw(sb, range);

            // 選択タイルのハイライト
            if (selectedTile != null)
            {
                var pos = selectedTile.Position;
                int size = 16;

                sb.Draw(pixel, new Rectangle((int)pos.X, (int)pos.Y, size, 1), Color.Yellow); //上
                sb.Draw(pixel, new Rectangle((int)pos.X, (int)pos.Y + size - 1, size, 1), Color.Yellow); //下
                sb.Draw(pixel, new Rectangle((int)pos.X, (int)pos.Y, 1, size), Color.Yellow); //左
                sb.Draw(pixel, new Rectangle((int)pos.X + size - 1, (int)pos.Y, 1, size), Color.Yellow); //右

            }
            sb.End();
        }
    }
}
