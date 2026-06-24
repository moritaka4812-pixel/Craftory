using Craftory.Maps;
using Craftory.Maps.Resource;

namespace Craftory.Core
{
    public class GameCore
    {
        public static GameCore Instance { get; private set; }
        public MapManager MapManager;
        public ItemManager ItemManager;

        public GameCore()
        {
            Instance = this;

            MapManager = new MapManager();
            ItemManager = new ItemManager();
        }
    }
}
