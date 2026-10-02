using Point = Microsoft.Xna.Framework.Point;
using Color = Microsoft.Xna.Framework.Color;

namespace Craftory.Maps.Buildings.Logistics.Conveyors
{
    public class ConveyorLeftCurve : Conveyor, IItemAcceptor, IPureConveyor
    {
        public ConveyorLeftCurve(BuildType type, Point pos, BuildingDirection OppositeInDir)
            : base (type, pos, OppositeInDir)
        {
        }

        protected override void InitDirections(List<BuildingDirection> OppositeInDir)
        {
            InDirections[TilePosition] = new List<BuildingDirection> { OppositeInDir[0].GetOpposite() };
            OutDirections[TilePosition] = new List<BuildingDirection> { GetOutDirectionFromIn(OppositeInDir[0].GetOpposite()) };
        }

        protected BuildingDirection GetOutDirectionFromIn(BuildingDirection inDir)
        {
            return inDir switch
            {
                BuildingDirection.Up => BuildingDirection.Right,
                BuildingDirection.Left => BuildingDirection.Up,
                BuildingDirection.Down => BuildingDirection.Left,
                BuildingDirection.Right => BuildingDirection.Down,
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
                BuildingDirection.Right => new Point(TilePosition.X + 1, TilePosition.Y),
                BuildingDirection.Left => new Point(TilePosition.X - 1, TilePosition.Y),
                BuildingDirection.Up => new Point(TilePosition.X, TilePosition.Y - 1),
                BuildingDirection.Down => new Point(TilePosition.X, TilePosition.Y + 1),
                _ => TilePosition
            };
        }

        public override Vector2 GetItemPosition(Vector2 worldPos, float local, ConveyorItem item)
        {
            const float tileSize = 32f;
            const float itemSize = 24f;

            Vector2 center = InDirections[TilePosition][0].GetOpposite() switch
            {
                BuildingDirection.Down => worldPos + new Vector2(tileSize, 0),
                BuildingDirection.Right => worldPos + new Vector2(0, 0),
                BuildingDirection.Up => worldPos + new Vector2(0, tileSize),
                BuildingDirection.Left => worldPos + new Vector2(tileSize, tileSize),
            };

            //アイテムが通る円の半径
            float radius = tileSize / 2f;

            float arcAngle = MathF.PI / 2f;

            float arcLength = radius * arcAngle;
            float traveled = arcLength * local;

            //角度を計算
            float angleOffset = traveled / radius;

            float startAngle = InDirections[TilePosition][0].GetOpposite() switch
            {
                BuildingDirection.Down => MathF.PI,
                BuildingDirection.Left => MathF.PI * 1.5f,
                BuildingDirection.Up => 0f,
                BuildingDirection.Right => MathF.PI * 0.5f,
            };

            float angle = startAngle - angleOffset;

            Vector2 arcCenterPos = new Vector2(
                center.X + MathF.Cos(angle) * radius,
                center.Y + MathF.Sin(angle) * radius
                );

            return arcCenterPos - new Vector2(itemSize / 2, itemSize / 2);
        }

        public override void DrawRotated(SpriteBatch sb, Point tilePos, Color tint)
        {
            var tex = Anim.Texture;
            var frame = Anim.GetCurrentFrameRect();

            float rotation = InDirections[tilePos][0].GetOpposite() switch
            {
                BuildingDirection.Right => 0,
                BuildingDirection.Down => MathF.PI * 0.5f,
                BuildingDirection.Left => MathF.PI,
                BuildingDirection.Up => MathF.PI * 1.5f,
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
