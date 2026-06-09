using Rect = Microsoft.Xna.Framework.Rectangle;
using Color = Microsoft.Xna.Framework.Color;
using Input = Microsoft.Xna.Framework.Input;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;

namespace ResourceMiningGame.UI
{
    public class Button
    {
        public Rect Rect;
        public string Text;
        public Color TextColor;
        public Color FillColor;
        public Color BorderColor;
        public Color NormalFillColor;
        public Color HoverFillColor;
        public Texture2D Icon; //アイコン画像
        public bool IsImageButton = false; //テキストボタンか画像ボタンか
        private ButtonState prevState = ButtonState.Released;

        Texture2D whiteTex; //１ドットの白テクスチャ
        SpriteFont font; //フォントデータ

        public Button(GraphicsDevice device, SpriteFont font, Rect rect, string text) //テキスト付きボタン
        {
            this.Rect = rect;　//引数受け渡しとテクスチャの初期化
            this.Text = text;
            this.font = font;
            this.TextColor = Color.White;
            this.FillColor = Color.DarkSlateGray;
            this.BorderColor = Color.White;
            this.NormalFillColor = Color.DarkSlateGray;
            this.HoverFillColor = Color.Gray;
            whiteTex = new Texture2D(device, 1, 1);
            whiteTex.SetData(new[] { Color.White });
        }

        public Button(GraphicsDevice device, Texture2D icon, Rect rect) //イメージ付きボタン
        {
            this.Icon = icon;
            this.Rect = rect;
            this.FillColor = Color.DarkSlateGray;
            this.BorderColor= Color.White;
            this.NormalFillColor = Color.DarkSlateGray;
            this.HoverFillColor= Color.Gray;
            IsImageButton = true;
            whiteTex = new Texture2D(device, 1, 1);
            whiteTex.SetData(new[] { Color.White });
        }

        public bool Update(MouseState current, MouseState previous)
        {
            bool hover = Rect.Contains(current.Position); //マウスがRect上にあるか

            if (hover) //Rect上にある
            {
                FillColor = HoverFillColor;
                BorderColor = Color.Yellow;
            }
            else //Rect上にない
            {
                FillColor = NormalFillColor;
                BorderColor = Color.White;
            }

            // クリック瞬間判定（押した瞬間だけtrue）
            bool clicked = hover &&
                           current.LeftButton == ButtonState.Pressed &&
                           previous.LeftButton == ButtonState.Released;

            return clicked;
        }

        public bool UpdateWithOffset(int offsetX, int offsetY, MouseState current, MouseState previous) //画面が移動した時のUpdate処理
        {
            var pos = new Microsoft.Xna.Framework.Point(current.X - offsetX, current.Y - offsetY); //内部の相対座標を計算（ミシンの縫物のイメージ）描画と内部のズレがoffset
            bool hover = Rect.Contains(pos);

            if (hover)
            {
                FillColor = HoverFillColor;
                BorderColor = Color.Yellow;
            }
            else
            {
                FillColor = NormalFillColor;
                BorderColor = Color.White;
            }

            // クリック瞬間判定（押した瞬間だけtrue）
            bool clicked = hover &&
                           current.LeftButton == ButtonState.Pressed &&
                           previous.LeftButton == ButtonState.Released;

            return clicked;
        }

        public void Draw(SpriteBatch sb)
        {
            //背景
            sb.Draw(whiteTex, Rect, FillColor);
            //ボタンの枠
            DrawRectangle(sb, Rect, 3, BorderColor);

            if(IsImageButton && Icon != null)
            {
                var iconPos = new Vector2(
                    Rect.X + (Rect.Width - Icon.Width) / 2,
                    Rect.Y + (Rect.Height - Icon.Height) / 2
                    );
                sb.Draw(Icon, iconPos, Color.White);
            }
            else
            {
                // ボタンテキストサイズを取得
                var size = font.MeasureString(Text);
                // テキストの位置計算
                var pos = new Vector2(
                    Rect.X + (Rect.Width - size.X) / 2,
                    Rect.Y + (Rect.Height - size.Y) / 2
                    );

                sb.DrawString(font, Text, pos, TextColor);
            }
        }

        public void SetBackgroundColor(Color color) //背景色変更メソッド
        {
            NormalFillColor = color;
            FillColor = color;
        }

        public void SetHoverColor(Color color) //ホバー時の色変更メソッド
        {
            HoverFillColor = color;
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
