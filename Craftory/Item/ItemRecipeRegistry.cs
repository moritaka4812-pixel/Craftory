
namespace Craftory.Item
{
    public static class ItemRecipeRegistry
    {
        public static readonly List<ItemRecipe> Recipes =
            new()
            {
                new ItemRecipe()
                {
                    Input = new (){ (ItemType.CopperOre, 1) },
                    Output = new (){(ItemType.CopperIngot, 1) },
                    Time = 1.5f
                }
            };
    }


}
