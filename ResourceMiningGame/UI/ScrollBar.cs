using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Color = Microsoft.Xna.Framework.Color;
using Microsoft.Xna.Framework.Graphics;
using ResourceMiningGame.Input;

namespace ResourceMiningGame.UI
{
    public class ScrollBar : UIElement
    {
        public Rectangle HandleRect; // 映している範囲を示すスクロールバー（つまみ）
        public int HandleHeight; //つまみの高さ

        public ScrollBar(int x, int y, int width, int height, int handleHeight)
        {
            rect = new Rectangle(x, y, width, height); //BarRect（映せる画面全体の長形）として扱う
            HandleHeight = handleHeight;

            HandleRect = new Rectangle(
                x,
                y,
                width,
                HandleHeight);
        }
        public void Update(int scrollY, int contentHeight, int viewHeight) // スクロールバーの位置を更新
        {
            float ratio = (float)scrollY / (contentHeight - viewHeight); // コンテンツのどの位置にスクロールバーがあるか（0~1.0）
            int handleY = rect.Y + (int)(ratio * (rect.Height - HandleRect.Height)); // スクロールバーがコンテンツ全体のどの高さにあるかを計算

            HandleRect = new Rectangle(
                rect.X,
                handleY,
                HandleRect.Width,
                HandleRect.Height
                );
        }

        public override void  Draw(SpriteBatch sb)
        {
            sb.Draw(whiteTex, rect, Color.DarkGray); // BarRect
            sb.Draw(whiteTex, HandleRect, Color.White); //HandleRect
        }

        public override bool Update(MouseInput mouse)
        {
            //スクロールバーはクリック処理がない
            return false;
        }
    }
}
