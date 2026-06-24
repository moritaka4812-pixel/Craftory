
using Craftory.Item;

namespace Craftory.Core
{
    public class ItemManager
    {
        private readonly Dictionary<ItemType, int> _items;

        public ItemManager()
        {
            _items = new Dictionary<ItemType, int>();

            foreach (ItemType type in Enum.GetValues(typeof(ItemType)))
            {
                _items[type] = 0;
            }
        }

        public int Get(ItemType type)
        {
            return _items[type];
        }
        public void Add(ItemType type, int amount)
        {
            _items[type] += amount;
        }

        public void Set(ItemType type, int amount)
        {
            _items[type] = amount;
        }

        public IReadOnlyDictionary<ItemType, int> GetAll()
        {
            return _items;
        }
    }
}
