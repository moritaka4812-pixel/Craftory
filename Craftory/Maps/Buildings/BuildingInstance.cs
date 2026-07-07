using Craftory.Core;
using Craftory.Maps.Tiles;
using Point = Microsoft.Xna.Framework.Point;
using Color = Microsoft.Xna.Framework.Color;

namespace Craftory.Maps.Buildings
{
    public class BuildingInstance : ITileOccupant
    {
        public BuildType Type { get; private set; }     // 建物の種類
        public Point TilePosition { get; private set; } //タイル座標
        public Point SizeInTiles { get; private set; }  //タイル単位の大きさ
        public bool IsActive { get; private set; }      //稼働状況
        public float WorkSpeed { get; private set; }    //採掘速度など、タイプ依存の性能値
        public List<Point> OccupiedTiles { get; private set; }

        // タイルごとの入口・出口情報
        public Dictionary<Point, List<BuildingDirection>> InDirections { get; private set; }
        public Dictionary<Point, List<BuildingDirection>> OutDirections { get; private set; }

        public TileAnimation Anim;
        public BuildingInfo info;

        protected float timer = 0f;

        public BuildingInstance(BuildType type, Point tilePosition, BuildingDirection dir)
        {
            Type = type;
            TilePosition = tilePosition;

            info = BuildingRegistry.Data[type];

            Anim = info.CreateTileAnimation(dir);

            SizeInTiles = info.SizeInTiles;
            WorkSpeed = info.WorkSpeed;
            IsActive = true;

            InDirections = new();
            OutDirections = new();

            OccupiedTiles = new List<Point>();
            for (int x = 0; x < info.Width; x++)
            {
                for (int y = 0; y < info.Height; y++)
                {
                    OccupiedTiles.Add(new Point(tilePosition.X + x, tilePosition.Y + y));
                }
            }
        }

        public virtual void UpdateLogic(GameTime gameTime)
        {
            if(!IsActive) return;
            
        }

        public virtual void UpdateVisual(GameTime gameTime)
        {
            if(!IsActive) return;
            Anim.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch sb, Camera camera)
        {
            var worldPos = TilePosition.ToVector2() * 32;
            // 建物のスプライト描画
            Anim.Draw(sb, worldPos);
        }

        public virtual void DrawRotated(SpriteBatch sb, Point tilePos, Color tint) //標準回転描画(Out基準)
        {
            var tex = Anim.Texture;
            var frame = Anim.GetCurrentFrameRect();

            float rotation = OutDirections[tilePos][0] switch
            {
                BuildingDirection.Right => 0f,
                BuildingDirection.Down => MathF.PI / 2,
                BuildingDirection.Left => MathF.PI,
                BuildingDirection.Up => -MathF.PI / 2,
                _ => 0f
            };

            Vector2 origin = new(tex.Width / Anim.FrameCount / 2f, tex.Height / 2f);
            Vector2 pos = tilePos.ToVector2() * 32 + origin;

            sb.Draw(
                tex,
                pos,
                frame,
                tint,
                rotation,
                origin,
                1f,
                SpriteEffects.None,
                0f
            );
        }

        public IEnumerable<Point> GetOccupiedTiles()
        {
            for(int dx = 0; dx < SizeInTiles.X; dx++)
            {
                for (int dy = 0; dy < SizeInTiles.Y; dy++)
                {
                    yield return new Point(TilePosition.X + dx, TilePosition.Y + dy);
                }
            }
        }
    }
}
