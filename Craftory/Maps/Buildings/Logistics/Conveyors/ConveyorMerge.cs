using Point = Microsoft.Xna.Framework.Point;
using Color = Microsoft.Xna.Framework.Color;
using Craftory.Core;
using System.Diagnostics;

namespace Craftory.Maps.Buildings.Logistics.Conveyors
{
    public class ConveyorMerge : Conveyor, IItemAcceptor, IMergeConveyor, IPureConveyor
    {
        public ConveyorMerge(string id, Point pos, BuildingDirection dir)
            : base(id, pos, dir)
        {
        }
        public override Vector2 GetItemPosition(Vector2 worldPos, float local, ConveyorItem item)
        {
            const float tileSize = 32f;
            const float itemSize = 24f;

            float radius = tileSize / 2f;

            float arcAngle = MathF.PI / 2f;

            float arcLength = radius * arcAngle;
            float traveled = arcLength * local;

            float angleOffset = traveled / radius;

            if (item.pastOutDir == GetPort("MainInput").Direction.GetOpposite())
            {
                return DefaultCalculate(worldPos, local, item.pastOutDir);
            }

            if (item.pastOutDir == GetPort("RightInput").Direction.GetOpposite()) //右回り
            {
                Vector2 center = GetPort("RightInput").Direction switch
                {
                    BuildingDirection.Down => worldPos + new Vector2(tileSize, tileSize),
                    BuildingDirection.Right => worldPos + new Vector2(tileSize, 0),
                    BuildingDirection.Up => worldPos + new Vector2(0,0),
                    BuildingDirection.Left => worldPos + new Vector2(0, tileSize),
                };

                float startAngle = GetPort("RightInput").Direction switch
                {
                    BuildingDirection.Down => MathF.PI * 1f,
                    BuildingDirection.Left => MathF.PI * 1.5f,
                    BuildingDirection.Up => 0,
                    BuildingDirection.Right => MathF.PI * 0.5f,
                };

                float angle = startAngle + angleOffset;

                Vector2 arcCenterPos = new Vector2(
                    center.X + MathF.Cos(angle) * radius,
                    center.Y + MathF.Sin(angle) * radius
                    );

                return arcCenterPos - new Vector2(itemSize / 2, itemSize / 2);
            }

            if (item.pastOutDir == GetPort("LeftInput").Direction.GetOpposite()) //左回り
            {
                Vector2 center = GetPort("LeftInput").Direction switch
                { 
                    BuildingDirection.Down => worldPos + new Vector2(0, tileSize),
                    BuildingDirection.Right => worldPos + new Vector2(tileSize, tileSize),
                    BuildingDirection.Up => worldPos + new Vector2(tileSize, 0),
                    BuildingDirection.Left => worldPos + new Vector2(0, 0)
                };

                float startAngle = GetPort("LeftInput").Direction switch
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

            throw new Exception("No draw merge candidate in ConveyorMerge GetItemPosition()");
        }
    }
}
