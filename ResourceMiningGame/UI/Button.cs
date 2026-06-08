using Rect = Microsoft.Xna.Framework.Rectangle;
using Color = Microsoft.Xna.Framework.Color;
using Input = Microsoft.Xna.Framework.Input;

namespace ResourceMiningGame.UI
{
    public class Button
    {
        public Rect Rect;
        public string Text;
        public Color TextColor = Color.White;
        public Color FillColor = Color.DarkSlateGray;
        public Color BorderColor = Color.White;

        Texture2D whiteTex; //１ドットの白テクスチャ
        SpriteFont font; //フォントデータ

        public Button(GraphicsDevice device, SpriteFont font, Rect rect, string text) //引数受け渡しとテクスチャの初期化
        {
            this.Rect = rect;
            this.Text = text;
            this.font = font;
            whiteTex = new Texture2D(device, 1, 1);
            whiteTex.SetData(new[] { Color.White });
        }

        public bool Update()
        {
            var mouse = Mouse.GetState(); //マウス位置取得
            bool hover = Rect.Contains(mouse.Position); //マウスがRect上にあるか

            if (hover) //Rect上にある
            {
                FillColor = Color.Gray;
                BorderColor = Color.Yellow;
            }
            else //Rect上にない
            {
                FillColor = Color.DarkSlateGray;
                BorderColor = Color.White;
            }
            // Rect上にあって左クリックがされたか
            return hover && mouse.LeftButton == Input.ButtonState.Pressed;
        }

        public bool UpdateWithOffset(int offsetX, int offsetY) //画面が移動した時のUpdate処理
        {
            var mouse = Mouse.GetState(); //ウィンドウ上のマウス位置（絶対座標）
            var pos = new Microsoft.Xna.Framework.Point(mouse.X - offsetX, mouse.Y - offsetY); //内部の相対座標を計算（ミシンの縫物のイメージ）描画と内部のズレがoffset
            bool hover = Rect.Contains(pos);

            if (hover)
            {
                FillColor = Color.Gray;
                BorderColor = Color.Yellow;
            }
            else
            {
                FillColor = Color.DarkSlateGray;
                BorderColor = Color.White;
            }
            return hover && mouse.LeftButton == Input.ButtonState.Pressed;
        }

        public void Draw(SpriteBatch sb)
        {
            //背景
            sb.Draw(whiteTex, Rect, FillColor);
            //ボタンの枠
            DrawRectangle(sb, Rect, 3, BorderColor);
            // ボタンテキストサイズを取得
            var size = font.MeasureString(Text);
            // テキストの位置計算
            var pos = new Vector2(
                Rect.X + (Rect.Width - size.X) / 2,
                Rect.Y + (Rect.Height - size.Y) / 2
                );

            sb.DrawString(font, Text, pos, TextColor);
        }
        void DrawRectangle(SpriteBatch sb, Rect rect, int thickness, Color color)
        {
            sb.Draw(whiteTex, new Rect(rect.X, rect.Y, rect.Width, thickness), color); // 上
            sb.Draw(whiteTex, new Rect(rect.X, rect.Y, thickness, rect.Height), color); // 左
            sb.Draw(whiteTex, new Rect(rect.X, rect.Y + rect.Height - thickness, rect.Width, thickness), color); // 下
            sb.Draw(whiteTex, new Rect(rect.X + rect.Width - thickness, rect.Y, thickness, rect.Height), color); // 右
        }
    }
}
