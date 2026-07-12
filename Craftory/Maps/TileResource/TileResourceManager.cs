
namespace Craftory.Maps.Resource
{
    public class TileResourceManager
    {
        private Dictionary<TileResourceType, int> _resources;

        public TileResourceManager() 
        {
            _resources = new Dictionary<TileResourceType, int>();

            foreach (TileResourceType type in Enum.GetValues(typeof(TileResourceType)))
            {
                _resources[type] = 0;
            }
        }

        public int Get(TileResourceType type)
        {
            return _resources[type];
        }

        public void Add(TileResourceType type, int amount)
        {
            _resources[type] += amount;
        }

        public void Set(TileResourceType type, int amount)
        {
            _resources[type] = amount;
        }

        public IReadOnlyDictionary<TileResourceType, int> GetAll()
        {
            return _resources;
        }
    }
}
