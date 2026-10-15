using Craftory.Maps.Tiles;
using Point = Microsoft.Xna.Framework.Point;
using Color = Microsoft.Xna.Framework.Color;

namespace Craftory.Maps.Buildings
{
    public class BuildingInfo
    {
        public Dictionary<BuildingDirection, string> TexturePaths;
        public Dictionary<BuildingDirection, string> ActiveTexturePaths;
        public Dictionary<BuildingDirection, Texture2D> CachedTextures = new();
        public Dictionary<BuildingDirection, Texture2D> CachedActiveTextures = new();
        public int FrameCount;
        public float FrameTime;
        public Point SizeInTiles;
        public Dictionary<BuildingDirection, List<Point>> OccupiedTilesByDirection; //建物の方向別の占有タイル座標

        public List<PortDefinitions> PortDefinitions;

        public float WorkSpeed;
        public float BuildTime;

        public BuildType Type;
        public int Width;  //タイル準拠の幅
        public int Height; //タイル準拠の高さ

        public Func<Point, BuildingDirection, BuildingInstance> Create;

        public TileAnimationMode AnimationMode; // プレビュー用のアニメーションモード


        public TileAnimation CreateTileAnimation(BuildingDirection dir)
        {
            Texture2D tex;
            bool useRotation;
            bool useFlip;
            if (CachedTextures.TryGetValue(dir, out var t))
            {
                tex = t;
            }

            else
            {
                tex = CachedTextures[BuildingDirection.None];
            }

            switch (AnimationMode)
            {
                case TileAnimationMode.None:
                    useRotation = false;
                    useFlip = false;
                    break;

                case TileAnimationMode.Rotate:
                    useRotation = true;
                    useFlip = false;
                    break;

                case TileAnimationMode.Flip:
                    useRotation = false;
                    useFlip = true;
                    break;

                default:
                    useRotation = false;
                    useFlip = false;
                    break;
            }
            return new TileAnimation(tex, FrameCount, tex.Width / FrameCount, tex.Height, FrameTime, useRotation, useFlip);
        }

        public IEnumerable<Point> GetArea(Point origin, BuildingDirection dir = BuildingDirection.None)
        {
            if(dir != BuildingDirection.None &&
                OccupiedTilesByDirection != null &&
                OccupiedTilesByDirection.TryGetValue(dir, out var list))
            {
                foreach (var p in list)
                    yield return origin + p;
            }
            else
            {
                for(int x = 0; x < SizeInTiles.X; x++)
                    for(int y = 0; y < SizeInTiles.Y; y++)
                        yield return new Point(origin.X + x, origin.Y + y);
            }

        }

        public void DrawPreview(SpriteBatch sb, Point tilePos, BuildingDirection dir, Color color)
        {
            var anim = CreateTileAnimation(dir);
            var frame = anim.GetCurrentFrameRect();
            var tex = anim.Texture;
            
            Vector2 origin = new(tex.Width / anim.FrameCount / 2f, tex.Height / 2f);
            Vector2 pos = tilePos.ToVector2() * 32 + origin;

            if (anim.UseRotation)
            {
                float rotation = dir switch
                {
                    BuildingDirection.Right => 0f,
                    BuildingDirection.Down => MathF.PI / 2,
                    BuildingDirection.Left => MathF.PI,
                    BuildingDirection.Up => -MathF.PI / 2,
                    _ => 0f
                };
                sb.Draw(tex, pos, frame, color, rotation, origin, 1f, SpriteEffects.None, 0f);
                return;
            }

            else if (anim.UseFlip)
            {
                sb.Draw(tex, pos, frame, color, MathF.PI, origin, 1f, SpriteEffects.None, 0f);
                return;
            }

            else
            {
                // ★方向別テクスチャはそのまま描画
                sb.Draw(tex, pos, frame, color, 0, origin, 1f, SpriteEffects.None, 0f);
                return;
            }
        }
    }
}
