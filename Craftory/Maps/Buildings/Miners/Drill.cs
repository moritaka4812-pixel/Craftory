using Craftory.Conversion;
using Craftory.Core;
using Craftory.Maps.Buildings.Conveyors;
using Point = Microsoft.Xna.Framework.Point;

namespace Craftory.Maps.Buildings.Miners
{
    public class Drill : BuildingInstance
    {
        public Drill(BuildType type, Point pos) :
            base(type, pos, BuildingDirection.None)
        {
            OutDirections[pos] = new List<BuildingDirection>
            {
                BuildingDirection.Right,
                BuildingDirection.Left,
                BuildingDirection.Up,
                BuildingDirection.Down
            };
        }

        public override void UpdateLogic(GameTime gameTime)
        {
            if(!IsActive) return;

            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(timer >= 1f / WorkSpeed)
            {
                timer -= 1f / WorkSpeed;

                var tile = GameCore.Instance.MapManager.Map.GetTile(TilePosition.X, TilePosition.Y);

                if(tile.Resource != Resource.TileResourceType.None)
                {
                    TryOutputToConveyor(tile.Resource);
                }
            }
        }

        private void TryOutputToConveyor(Resource.TileResourceType type)
        {
            var dirs = new (int x, int y, BuildingDirection dir)[]
            {
                (1,0, BuildingDirection.Right),
                (-1,0, BuildingDirection.Left),
                (0,1, BuildingDirection.Down), 
                (0,-1, BuildingDirection.Up)
            };

            var itemType = ResourceToItemConvertor.Convert(type);


            foreach (var tilePos in GetOccupiedTiles())
            {
                foreach (var dir in dirs)
                {
                    var nextPos = new Point(TilePosition.X + dir.x, TilePosition.Y + dir.y);
                    var tile = GameCore.Instance.MapManager.Map.GetTile(nextPos.X, nextPos.Y);

                    if (tile?.Occupant is IItemAcceptor acceptor)
                    {
                        var item = new ConveyorItem { Type = itemType };

                        if (acceptor.TryAccept(item, dir.dir.GetOpposite()))
                            return; // 最初に受け取ってくれたAcceptorに渡して終了
                    }
                }
            }
        }
    }
}
