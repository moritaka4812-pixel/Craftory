using Rect = Microsoft.Xna.Framework.Rectangle;
using MyUI = ResourceMiningGame.UI;
using Color = Microsoft.Xna.Framework.Color;

namespace ResourceMiningGame.Screens
{
    public class TitleScreen : ScreenBase
    {
        MyUI.Button startButton;

        public TitleScreen(Game1 game) : base(game) // call base class constructor to pass game instance and base class stores the instance.
        {
            font = game.Content.Load<SpriteFont>("Fonts\\MyFont"); // load font data
            startButton = new MyUI.Button( // create a start button from MyUI.Button class
                game.GraphicsDevice,
                font,
                new Rect(300, 400, 200, 80),
                "Start");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(); // start drawing with batch (=setting)
            startButton.Draw(spriteBatch); // draw the start button
            spriteBatch.DrawString(font, "My TD Game", new Vector2(200, 100), Color.AliceBlue); // draw the title text
            spriteBatch.End(); // flush the batch and finish drawing
        }

        public override void Update(GameTime gameTime)
        {
            if (startButton.Update()) // start button was clicked
            {
                game.ChangeScreen(new LevelSelectScreen(game));
            }
        }
    }
}
