using Point = Microsoft.Xna.Framework.Point;
using Color = Microsoft.Xna.Framework.Color;

namespace Craftory.Maps.Buildings.Logistics.Conveyors
{
    public class ConveyorLeftCurve : Conveyor, IItemAcceptor, IPureConveyor
    {
        public ConveyorLeftCurve(BuildType type, Point pos, BuildingDirection dir)
            : base (type, pos, dir)
        {
        }

        public override Vector2 GetItemPosition(Vector2 worldPos, float local, ConveyorItem item)
        {
            const float tileSize = 32f;
            const float itemSize = 24f;

            Vector2 center = GetPort("Input").Direction.GetOpposite() switch
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

            float startAngle = GetPort("Input").Direction.GetOpposite() switch
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
    }
}
