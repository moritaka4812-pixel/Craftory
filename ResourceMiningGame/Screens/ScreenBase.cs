
namespace ResourceMiningGame.Screens
{
    public abstract class ScreenBase // all screens inherit from this class.
    {
        protected Game1 game; // reference to the main game class.
        protected SpriteFont font; // for drawing text on the screen.
        public ScreenBase(Game1 game) // passing game instance.
        {
            this.game = game;
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
