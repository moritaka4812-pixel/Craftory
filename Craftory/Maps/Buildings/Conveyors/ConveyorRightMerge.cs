using Color = Microsoft.Xna.Framework.Color; 
using Point = Microsoft.Xna.Framework.Point;

namespace Craftory.Maps.Buildings.Conveyors
{
    public class ConveyorRightMerge : Conveyor
    {
        public ConveyorRightMerge(BuildType type, Point pos, BuildingDirection outDir)
            : base(type, pos, outDir)
        {
        }

        public override void UpdateLogic(GameTime gameTime)
        {
            TileLogic.UpdateMerge(gameTime);
        }

        protected override void InitDirections(List<BuildingDirection> outDir)
        {
            OutDirections[TilePosition] = new List<BuildingDirection> { outDir[0] };
            InDirections[TilePosition] = GetInDirectionFromOut(outDir[0]);
        }

        protected new List<BuildingDirection> GetInDirectionFromOut(BuildingDirection outDir)
        {
            return outDir switch
            {
                BuildingDirection.Right => new List<BuildingDirection> { BuildingDirection.Left, BuildingDirection.Down },
                BuildingDirection.Left => new List<BuildingDirection> { BuildingDirection.Right, BuildingDirection.Up },
                BuildingDirection.Up => new List<BuildingDirection> { BuildingDirection.Down, BuildingDirection.Right },
                BuildingDirection.Down => new List<BuildingDirection> { BuildingDirection.Up, BuildingDirection.Left },
                _ => new List<BuildingDirection>()
            };
        }

        public new IEnumerable<Point> GetBackPosition()
        {
            foreach (var Indir in InDirections[TilePosition])
            {
                yield return Indir switch
                {
                    BuildingDirection.Right => new Point(TilePosition.X - 1, TilePosition.Y),
                    BuildingDirection.Left => new Point(TilePosition.X + 1, TilePosition.Y),
                    BuildingDirection.Up => new Point(TilePosition.X, TilePosition.Y + 1),
                    BuildingDirection.Down => new Point(TilePosition.X, TilePosition.Y - 1),
                    _ => TilePosition
                };
            }
        }

        public override Vector2 GetItemPosition(Vector2 worldPos, float local, ConveyorItem item)
        {
            const float tileSize = 32f;
            const float itemSize = 24f;

            if(item.pastOutDir == OutDirections[TilePosition][0]) //直線描画
            {
                return DefaultCalculate(worldPos, local, item.pastOutDir);
            }

            Vector2 center = InDirections[TilePosition][1] switch
            {
                BuildingDirection.Down => worldPos + new Vector2(tileSize, tileSize),
                BuildingDirection.Right => worldPos + new Vector2(tileSize, 0),
                BuildingDirection.Up => worldPos + new Vector2(0, 0),
                BuildingDirection.Left => worldPos + new Vector2(0, tileSize),
            };

            //アイテムが通る円の半径
            float radius = tileSize / 2f;

            float arcAngle = MathF.PI / 2f;

            float arcLength = radius * arcAngle;
            float traveled = arcLength * local;

            //角度を計算
            float angleOffset = traveled / radius;

            float startAngle = InDirections[TilePosition][1] switch
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
    }
}
