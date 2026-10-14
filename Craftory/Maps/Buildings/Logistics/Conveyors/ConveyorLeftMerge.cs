using Craftory.Core;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Point = Microsoft.Xna.Framework.Point;

namespace Craftory.Maps.Buildings.Logistics.Conveyors
{
    public class ConveyorLeftMerge : Conveyor, IItemAcceptor, IMergeConveyor, IPureConveyor
    {
        public ConveyorLeftMerge(BuildType type, Point pos, BuildingDirection dir)
            : base (type, pos, dir)
        {
        }

        public override Vector2 GetItemPosition(Vector2 worldPos, float local, ConveyorItem item)
        {
            const float tileSize = 32f;
            const float itemSize = 24f;

            if(item.pastOutDir == GetPort("MainInput").Direction.GetOpposite())
            {
                return DefaultCalculate(worldPos, local, item.pastOutDir);
            }

            Vector2 center = GetPort("SubInput").Direction switch
            {
                BuildingDirection.Down => worldPos + new Vector2(0, tileSize),
                BuildingDirection.Right => worldPos + new Vector2(tileSize, tileSize),
                BuildingDirection.Up => worldPos + new Vector2(tileSize, 0),
                BuildingDirection.Left => worldPos + new Vector2(0, 0)
            };

            float radius = tileSize / 2f;

            float arcAngle = MathF.PI / 2f;

            float arcLength = radius * arcAngle;
            float traveled = arcLength * local;

            float angleOffset = traveled / radius;

            float startAngle = GetPort("SubInput").Direction switch
            {
                BuildingDirection.Down => 0,
                BuildingDirection.Left => MathF.PI * 0.5f,
                BuildingDirection.Up => MathF.PI * 1f,
                BuildingDirection.Right => MathF.PI * 1.5f
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
