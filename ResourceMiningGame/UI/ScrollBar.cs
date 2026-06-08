using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Color = Microsoft.Xna.Framework.Color;
using Microsoft.Xna.Framework.Graphics;

namespace ResourceMiningGame.UI
{
    public class ScrollBar
    {
        public Rectangle BarRect; // the full area of the scrollbar track
        public Rectangle HandleRect; // draggable handle area (thumb)

        public void Update(int scrollY, int contentHeight, int viewHeight) // update the scrollbar position every frame.
        {
            float ratio = (float)scrollY / (contentHeight - viewHeight); // calculate the scroll/content ratio (0.0 to 1.0)
            int handleY = BarRect.Y + (int)(ratio * (BarRect.Height - HandleRect.Height)); // calculate the thumb's Y position within the track. 

            HandleRect = new Rectangle(
                BarRect.X,
                handleY,
                HandleRect.Width,
                HandleRect.Height
                );
        }

        public void Draw(SpriteBatch sb, Texture2D white)
        {
            sb.Draw(white, BarRect, Color.DarkGray);
            sb.Draw(white, HandleRect, Color.White);
        }
    }
}
