using Craftory.Maps.Tiles;
using Point = Microsoft.Xna.Framework.Point;
using Color = Microsoft.Xna.Framework.Color;

namespace Craftory.Maps.Buildings
{
    public class BuildingInfo
    {
        public Dictionary<BuildingDirection, string> TexturePaths;
        public Dictionary<BuildingDirection, Texture2D> CachedTextures = new();
        public int FrameCount;
        public float FrameTime;
        public Point SizeInTiles;
        public float WorkSpeed;

        public BuildType Type;
        public int Width;  //タイル準拠の幅
        public int Height; //タイル準拠の高さ

        public Func<Point, BuildingDirection, BuildingInstance> Create;

        public TileAnimation CreateTileAnimation(BuildingDirection dir)
        {
            Texture2D tex;
            if (CachedTextures.TryGetValue(dir, out var t))
                tex = t;
            else
                tex = CachedTextures[BuildingDirection.None];

            return new TileAnimation(tex, FrameCount, tex.Width / FrameCount, tex.Height, FrameTime);
        }

        public IEnumerable<Point> GetArea(Point origin)
        {
            for(int x = 0; x < SizeInTiles.X; x++)
                for(int y = 0; y < SizeInTiles.Y; y++)
                    yield return new Point(origin.X + x, origin.Y + y);
        }

        public void DrawPreview(SpriteBatch sb, Point tilePos, BuildingDirection dir, Color color)
        {
            var anim = CreateTileAnimation(dir);
            var frame = anim.GetCurrentFrameRect();
            var tex = anim.Texture;

            float rotation = dir switch
            {
                BuildingDirection.Right => 0f,
                BuildingDirection.Down => MathF.PI / 2,
                BuildingDirection.Left => MathF.PI,
                BuildingDirection.Up => -MathF.PI / 2,
                _ => 0f
            };

            Vector2 origin = new(tex.Width / anim.FrameCount / 2f, tex.Height / 2f);
            Vector2 pos = tilePos.ToVector2() * 32 + origin;

            sb.Draw(tex, pos, frame, color, rotation, origin, 1f, SpriteEffects.None, 0f);
        }

    }
}
