関係ブランチ-> AI-Selection-Problem2
CraftoryのDrillクラスです。

症状:
Drillに隣接したConveyorへItemを直接搬出した場合、
アイテムの描画が不安定になります。

Conveyor→Conveyorでは問題ありません。
Drill→Conveyorのときのみ発生します。

原因として考えられる箇所を調査してください。
必要であれば仮説を複数提示してください。

[Drillコード]
using Craftory.Conversion;
using Craftory.Core;
using Craftory.Maps.Buildings.Logistics.Conveyors;
using Point = Microsoft.Xna.Framework.Point;

namespace Craftory.Maps.Buildings.Miners
{
    public class Drill : BuildingInstance
    {
        private ConveyorItem bufferItem = null;

        public Drill(BuildType type, Point pos) :
            base(type, pos, BuildingDirection.None)
        {
        }

        public override void UpdateLogic(GameTime gameTime)
        {
            if (UpdateConstructingState(gameTime))
                return;

            if(!IsActive) return;

            TryFlushBuffer();

            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer >= 1f / WorkSpeed)
            {
                timer -= 1f / WorkSpeed;

                var tile = GameCore.Instance.MapManager.Map.GetTile(TilePosition.X, TilePosition.Y);

                if (tile.Resource != Resource.TileResourceType.None)
                {
                    TryOutputToAcceptor(tile.Resource);
                }
            }

        }

        private void TryOutputToAcceptor(Resource.TileResourceType type)
        {
            var dirs = new (int x, int y, BuildingDirection dir)[]
            {
                (1,0, BuildingDirection.Right),
                (-1,0, BuildingDirection.Left),
                (0,1, BuildingDirection.Down), 
                (0,-1, BuildingDirection.Up)
            };

            var itemType = ResourceToItemConvertor.Convert(type);

            if(bufferItem == null)
                bufferItem = new ConveyorItem { Type = itemType };


            foreach (var tilePos in GetOccupiedTiles())
            {
                foreach (var dir in dirs)
                {
                    var nextPos = new Point(TilePosition.X + dir.x, TilePosition.Y + dir.y);
                    var tile = GameCore.Instance.MapManager.Map.GetTile(nextPos.X, nextPos.Y);

                    if (tile?.Occupant is IItemAcceptor acceptor)
                    {
                        if(bufferItem != null)
                        {
                            if (TryOutputFair(bufferItem))
                            {
                                bufferItem.pastOutDir = dir.dir;
                                bufferItem = null;
                            }
                        }
                    }
                }
            }
        }

        private void TryFlushBuffer()
        {
            if (bufferItem == null)
                return;

            if (TryOutputFair(bufferItem))
                bufferItem = null;
        }
    }
}

回答は簡潔にしてください。
原因候補と根拠を優先してください。
不明な場合は仮説として提示してください。