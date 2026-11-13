using Craftory.Conversion;
using Craftory.Core;
using Craftory.Maps.Buildings.Logistics.Conveyors;
using Point = Microsoft.Xna.Framework.Point;

namespace Craftory.Maps.Buildings.Miners
{
    public class Drill : BuildingInstance
    {

        private ConveyorItem bufferItem = null;
        public override bool IsWorking => IsActive && tile.Resource != Resource.TileResourceType.None;
        public override bool IsBlocked => IsActive && bufferItem != null;

        public Tiles.Tile tile;
        public Drill(string id, Point pos) :
            base(id, pos, BuildingDirection.None)
        {
            tile = GameCore.Instance.MapManager.Map.GetTile(TilePosition.X, TilePosition.Y);
        }

        public override void UpdateLogic(GameTime gameTime)
        {
            base.UpdateLogic(gameTime);

            if (!IsActive) return;

            TryFlushBuffer();

            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer >= 1f / WorkSpeed)
            {
                timer -= 1f / WorkSpeed;

                if (tile.Resource != Resource.TileResourceType.None)
                {
                    TryOutputToAcceptor(tile.Resource);
                }
            }

        }

        private void TryOutputToAcceptor(Resource.TileResourceType type)
        {

            var itemType = ResourceToItemConvertor.Convert(type);

            if(bufferItem == null)
                bufferItem = ConveyorItem.Create(itemType);


            foreach (var Port in OutputPorts)
            {
                var nextPos = new Point(TilePosition.X + Port.Offset.X + Port.Direction.GetPoint().X, TilePosition.Y + Port.Offset.Y + Port.Direction.GetPoint().Y);
                var tile = GameCore.Instance.MapManager.Map.GetTile(nextPos.X, nextPos.Y);

                if (tile?.Occupant is IItemAcceptor acceptor)
                {
                    if(bufferItem != null)
                    {
                        bufferItem.InitializeDirections(Port.Direction, Port.Direction);

                        if (TryOutputFair(bufferItem) != null)
                        {
                                bufferItem = null;
                        }
                    }
                }
            }
        }

        private void TryFlushBuffer()
        {
            if (bufferItem == null)
                return;

            
            if (TryOutputFair(bufferItem) != null)
                bufferItem = null;
        }
    }
}
