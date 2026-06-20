using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Color = Microsoft.Xna.Framework.Color;

namespace ResourceMiningGame.Maps.Tiles
{
    public class Tile
    {
        public TileType Type;
        public ResourceType Resource;
        public bool IsBuildable; //建設可能かどうか
        public Vector2 Position; //タイルの位置（ワールド座標）

        public TileAnimation TerrainAnim;
        public TileAnimation ResourceAnim;


        public Tile(TileType Type,
                    ResourceType resource,
                    Vector2 Position)
        {
            this.Type = Type;
            this.Resource = resource;
            this.Position = Position;
            this.IsBuildable = TileRules.Buildable[Type];

            TerrainAnim = TileRegistry.Terrain[Type].CreateTileAnimation();
            ResourceAnim = ResourceRegistry.Resources[resource]?.CreateTileAnimation();
        }

        public void Update(GameTime gameTime)
        {
            TerrainAnim?.Update(gameTime);
            ResourceAnim?.Update(gameTime);
        }

        public void Draw(SpriteBatch sb)
        {
            TerrainAnim?.Draw(sb, Position);
            ResourceAnim?.Draw(sb, Position);

        }

    }
}
