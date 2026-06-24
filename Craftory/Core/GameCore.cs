using Craftory.Maps;
using Craftory.Maps.Resource;

namespace Craftory.Core
{
    public class GameCore
    {
        public static GameCore Instance { get; private set; }
        public MapManager MapManager;
        public ResourceManager ResourceManager;

        public GameCore()
        {
            Instance = this;

            MapManager = new MapManager();
            ResourceManager = new ResourceManager();
        }
    }
}
