

using ResourceMiningGame.Core;

namespace ResourceMiningGame.Maps
{
    public abstract class MapBase : IMap
    {
        public Tiles.Tile[,] MapTiles { get; set; }
        public int MapSizeX { get; set; }
        public int MapSizeY { get; set; }
        public int TileSize = 16;

        public VisibleTileRange GetVisibleRange(Camera camera, GraphicsDevice graphics)
        {
            int viewportWidth = graphics.Viewport.Width;
            int viewportHeight = graphics.Viewport.Height;

            // ViewMatrix の逆行列を使って画面のワールド座標を求める
            Matrix inverse = Matrix.Invert(camera.GetViewMatrix());

            Vector2 topLeft = Vector2.Transform(Vector2.Zero, inverse);
            Vector2 bottomRight = Vector2.Transform(
                new Vector2(viewportWidth, viewportHeight),
                inverse
            );

            return new VisibleTileRange
            {
                StartX = Math.Max((int)(topLeft.X / TileSize), 0),
                EndX = Math.Min((int)(bottomRight.X / TileSize) + 1, MapSizeX),
                StartY = Math.Max((int)(topLeft.Y / TileSize), 0),
                EndY = Math.Min((int)(bottomRight.Y / TileSize) + 1, MapSizeY)
            };
        }


        public Tiles.Tile GetTile(int x, int y)
        {
            return MapTiles[x, y];
        }

        public abstract void LoadContent(ContentManager content);
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch sb, VisibleTileRange range);
    }
}
