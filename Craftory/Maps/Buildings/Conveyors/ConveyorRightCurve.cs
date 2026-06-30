using Craftory.Maps.Tiles;
using System.Linq.Expressions;
using Point = Microsoft.Xna.Framework.Point;
using Color = Microsoft.Xna.Framework.Color;

namespace Craftory.Maps.Buildings.Conveyors
{
    public class ConveyorRightCurve : Conveyor
    {
        public ConveyorRightCurve(BuildType type, Point pos, BuildingDirection inDir)
            :base(type, pos, inDir)
        {
        }

        protected override void InitDirections(BuildingDirection inDir)
        {
            InDirections[TilePosition] = new List<BuildingDirection> { inDir };
            OutDirections[TilePosition] = new List<BuildingDirection> { GetOutDirectionFromIn(inDir) };
        }

        protected BuildingDirection GetOutDirectionFromIn(BuildingDirection inDir)
        {
            return inDir switch
            { 
                BuildingDirection.Up => BuildingDirection.Right,
                BuildingDirection.Right => BuildingDirection.Down,
                BuildingDirection.Down => BuildingDirection.Left,
                BuildingDirection.Left => BuildingDirection.Up,
                _ => BuildingDirection.None
            };
        }

        public override BuildingDirection GetDirectionForItem(ConveyorItem item)
        {
            float local = item.GlobalPosition - TileLogic.TileStart;

            if (local < 0.5f)
                return InDirections[TilePosition][0];
            else
                return OutDirections[TilePosition][0];
        }

        

        public override Point GetBackPosition()
        {
            return InDirections[TilePosition][0] switch
            {
                BuildingDirection.Right => new Point(TilePosition.X - 1, TilePosition.Y),
                BuildingDirection.Left => new Point(TilePosition.X + 1, TilePosition.Y),
                BuildingDirection.Up => new Point(TilePosition.X, TilePosition.Y + 1),
                BuildingDirection.Down => new Point(TilePosition.X, TilePosition.Y - 1),
                _ => TilePosition
            };
        }

        public override Vector2 GetItemPosition(Vector2 worldPos, float local, ConveyorItem item)
        {
            var dir = GetDirectionForItem(item); // ← カーブの方向を使う
            return TileLogic.DefaultCalculate(worldPos, local, dir);
        }

        public override void DrawRotated(SpriteBatch sb, Point tilePos, Color tint)
        {
            var tex = Anim.Texture;
            var frame = Anim.GetCurrentFrameRect();

            float rotation = InDirections[tilePos][0] switch
            {
                BuildingDirection.Right => 0f,
                BuildingDirection.Down => MathF.PI / 2,
                BuildingDirection.Left => MathF.PI,
                BuildingDirection.Up => - MathF.PI / 2,
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

    }
}
