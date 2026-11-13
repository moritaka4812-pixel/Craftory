
namespace Craftory.Item
{
    public static class ItemRecipeRegistry
    {
        public static readonly List<ItemRecipe> Recipes =
            new()
            {
                new ItemRecipe()
                {
                    Input = new (){ (ItemType.CopperOre, 5) },
                    Output = new (){(ItemType.CopperBrick, 1) },
                    Time = 1.5f
                }
            };
    }
}
