using Microsoft.Xna.Framework;
using ResourceMiningGame.Screens;

namespace ResourceMiningGame
{
    public class Game1 : Game // inherit from Game class.
    {
        private GraphicsDeviceManager _graphics; //manage graphics settings
        private SpriteBatch _spriteBatch; //draw text and images
        private ScreenBase currentScreen; //showing screen;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this); //create graphics object (this is created only once in project)
            Content.RootDirectory = "Content"; // set the root directory of the content
            IsMouseVisible = true; // able to see the mouse cursor
        }

        protected override void Initialize() // called for initialization of the game objects
        {
            // TODO: Add your initialization logic here
            base.Initialize(); // use base class(親クラス) method to Initialize the game objects.
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice); // create a new SpriteBatch which is used to draw and textures in this project
            currentScreen = new TitleScreen (this); // start with the TitleScreen.

            // TODO: use this.Content to load your game content here
            

        }

        public void ChangeScreen(ScreenBase next)// change the screen. All of the screen will be changed by this same instance method.
        {
            currentScreen = next;
        }

        // GameTime : ゲーム世界の時間情報（前フレームからの経過時間および累計時間）
        // Updateロジックをフレームレートに依存させないために使う。
        protected override void Update(GameTime gameTime) // called every frame to update the internal state of the game.
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == Microsoft.Xna.Framework.Input.ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            currentScreen.Update(gameTime);

            base.Update(gameTime); // use base class method Update().
        }

        protected override void Draw(GameTime gameTime) // called every frame after Update to draw the UI and game objects on the screen.
        {
            GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Black);

            // TODO: Add your drawing code here
            currentScreen.Draw(_spriteBatch);

            base.Draw(gameTime); // use base class method Draw().
        }
    }
}
