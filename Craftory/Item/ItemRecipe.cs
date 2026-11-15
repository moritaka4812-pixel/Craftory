
namespace Craftory.Item
{
    public class ItemRecipe
    {
        public string Id { get; set; }
        public List<(ItemType type, int amount)> Input { get; set; }
        public List<(ItemType type, int amount)> Output { get; set; }
        public float Time { get; set; }
    }
}
