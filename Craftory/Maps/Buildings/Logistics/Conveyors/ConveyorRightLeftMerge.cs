
using Craftory.Core;
using Point = Microsoft.Xna.Framework.Point;

namespace Craftory.Maps.Buildings.Logistics.Conveyors
{
    public class ConveyorRightLeftMerge : Conveyor, IItemAcceptor, IMergeConveyor, IPureConveyor
    {
        public ConveyorRightLeftMerge(BuildType type, Point pos, BuildingDirection outDir)
            : base(type, pos, outDir)
        {
        }

        public override void UpdateLogic(GameTime gameTime)
        {
            if (UpdateConstructingState(gameTime))
                return;

            TileLogic.UpdateMerge(gameTime);
        }

        public void InitializeMergeTileStart()
        {
            TileLogic.InitializeMergeTileStart();
        }

        public override void InitializeConnections()
        {
            base.InitializeConnections();

            var backs = new List<ConveyorTile>();

            foreach (var pos in GetBackPosition())
            {
                var tile = GameCore.Instance.MapManager.Map.GetTile(pos.X, pos.Y);
                if (tile?.Occupant is Conveyor c)
                {
                    backs.Add(c.TileLogic);
                    c.TileLogic.InitializeTileStart();
                }
            }

            TileLogic.SetBackTiles(backs);
            TileLogic.InitializeMergeTileStart();


        }

        public new IEnumerable<Point> GetBackPosition()
        {
            foreach (var Indir in info.ReceivedDirections[buildingDirection])
            {
                yield return Indir switch
                {
                    BuildingDirection.Right => new Point(TilePosition.X + 1, TilePosition.Y),
                    BuildingDirection.Left => new Point(TilePosition.X - 1, TilePosition.Y),
                    BuildingDirection.Up => new Point(TilePosition.X, TilePosition.Y - 1),
                    BuildingDirection.Down => new Point(TilePosition.X, TilePosition.Y + 1),
                    _ => TilePosition
                };
            }
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

            if (item.pastOutDir == info.ReceivedDirections[buildingDirection][1].GetOpposite()) //右回り
            {
                Vector2 center = info.ReceivedDirections[buildingDirection][1] switch
                {
                    BuildingDirection.Down => worldPos + new Vector2(tileSize, tileSize),
                    BuildingDirection.Right => worldPos + new Vector2(tileSize, 0),
                    BuildingDirection.Up => worldPos + new Vector2(0, 0),
                    BuildingDirection.Left => worldPos + new Vector2(0, tileSize),
                };

                float startAngle = info.ReceivedDirections[buildingDirection][1] switch
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

            if (item.pastOutDir == info.ReceivedDirections[buildingDirection][0].GetOpposite()) //左回り
            {
                Vector2 center = info.ReceivedDirections[buildingDirection][0] switch
                {
                    BuildingDirection.Down => worldPos + new Vector2(0, tileSize),
                    BuildingDirection.Right => worldPos + new Vector2(tileSize, tileSize),
                    BuildingDirection.Up => worldPos + new Vector2(tileSize, 0),
                    BuildingDirection.Left => worldPos + new Vector2(0, 0)
                };

                float startAngle = info.ReceivedDirections[buildingDirection][0] switch
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

            return Vector2.Zero;
        }
    }
}

