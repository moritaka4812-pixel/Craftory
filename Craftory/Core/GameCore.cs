using Craftory.Maps;
using Craftory.Maps.Resource;

namespace Craftory.Core
{
    public class GameCore
    {
        public static GameCore Instance { get; private set; }
        public MapManager MapManager;
        public ItemManager ItemManager;
        public float Time { get; private set; } = 0f;

        public GameCore()
        {
            Instance = this;

            MapManager = new MapManager();
            ItemManager = new ItemManager();
        }

        public void Update(GameTime gameTime)
        {
            Time += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
