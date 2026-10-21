using Craftory.Core;
using Point = Microsoft.Xna.Framework.Point;
using Color = Microsoft.Xna.Framework.Color;
using System.Diagnostics;

namespace Craftory.Maps.Buildings.Logistics.Conveyors
{
    public class Conveyor : BuildingInstance, IItemAcceptor, IPureConveyor
    {
        public ConveyorTile TileLogic { get; private set; }

        public Conveyor(BuildType type, Point pos, BuildingDirection dir) : 
            base(type, pos, dir)
        {
            TileLogic = new ConveyorTile(WorkSpeed, this);
        }

        public virtual BuildingDirection GetDirectionForItem(ConveyorItem item)
        {
            var outputPort = GetPort("Output").Direction;

            return outputPort;
        }

        public override void UpdateLogic(GameTime gameTime)
        {
            if (UpdateConstructingState(gameTime)) 
                return;

            TileLogic.Update(gameTime);
        }

        public override void Draw(SpriteBatch sb, Camera camera)
        {
            //建物描画
            DrawRotated(sb, TilePosition, Color.White);
        }

        public void RefreshConnection()
        {
            ConnectionManager.RefreshConnections(this);
        }

        public virtual Vector2 GetItemPosition(Vector2 worldPos, float local, ConveyorItem item)
        {
            return DefaultCalculate(worldPos, local, item.currentDirection);
        }

        public virtual void SetOutDir(ConveyorItem item)
        {
            item.pastOutDir = GetPort("Output").Direction;
        }

        public Vector2 DefaultCalculate(Vector2 worldPos, float pos, BuildingDirection dir)
        {
            const float tileSize = 32f;
            const float itemSize = 24f;
            const float centerOffset = (tileSize - itemSize) / 2f; //タイル中央への補正

            // アイテム中心を返す
            Vector2 centerPos = dir switch
            {
                BuildingDirection.Right => new Vector2(
                    worldPos.X + pos * tileSize,
                    worldPos.Y + tileSize / 2f
                ),

                BuildingDirection.Left => new Vector2(
                    worldPos.X + (1 - pos) * tileSize,
                    worldPos.Y + tileSize / 2f
                ),

                BuildingDirection.Up => new Vector2(
                    worldPos.X + tileSize / 2f,
                    worldPos.Y + (1 - pos) * tileSize
                ),

                BuildingDirection.Down => new Vector2(
                    worldPos.X + tileSize / 2f,
                    worldPos.Y + pos * tileSize
                ),

                _ => throw new Exception($"undefined direction in Conveyor DefaultCalculat. dir = {dir}")
            };

            var drawPos = centerPos - new Vector2(itemSize / 2f, itemSize / 2f);
            return drawPos;
        }

        //IItemAcceptorの実装
        public virtual bool CanAccept(ConveyorItem item, BuildingPort senderPort)
        {
            return TileLogic.CanAcceptItem();
        }

        public virtual bool TryAccept(ConveyorItem item, BuildingPort senderPort)
        {
            return TileLogic.TryAccept(item, senderPort);
        }

        public bool HasSpace()
        {
            return TileLogic.HasSpace();
        }

        public bool IsFull()
        {
            return TileLogic.IsFull;
        }

        public virtual bool CanPreviewAccept(ConveyorItem item, BuildingPort senderPort)
        {
            return TileLogic.CanPreviewAccept(item, senderPort);
        }
    }
}
