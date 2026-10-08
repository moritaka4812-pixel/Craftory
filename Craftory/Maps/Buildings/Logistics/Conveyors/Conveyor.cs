using Craftory.Core;
using Point = Microsoft.Xna.Framework.Point;
using Color = Microsoft.Xna.Framework.Color;

namespace Craftory.Maps.Buildings.Logistics.Conveyors
{
    public class Conveyor : BuildingInstance, IItemAcceptor, IPureConveyor
    {
        public ConveyorTile TileLogic { get; private set; }

        public Conveyor(BuildType type, Point pos, BuildingDirection dir) : 
            base(type, pos, dir)
        {

            TileLogic = new ConveyorTile(WorkSpeed, this);

            TileLogic.InitializeTileStart();

            InitializeConnections();
        }

        public virtual void InitializeConnections()
        {
            var map = GameCore.Instance.MapManager.Map;
            var nextPos = GetNextPosition();
            var tile = GameCore.Instance.MapManager.Map.GetTile(GetNextPosition().X, GetNextPosition().Y);
            if(tile?.Occupant is  Conveyor nextConveyor)
            {
                var outDir = info.OutputDirections[buildingDirection][0];

                var nextInputDirs = nextConveyor.GetInputDirections();

                if (nextInputDirs.Contains(outDir.GetOpposite()))
                {
                    TileLogic.SetNextTile(nextConveyor.TileLogic);
                }
                else
                {
                    TileLogic.SetNextTile(null);
                }
            }
               
            else
                TileLogic.SetNextTile(null);

            // BackTile 設定
            var backPos = GetBackPosition();
            var backTile = GameCore.Instance.MapManager.Map.GetTile(backPos.X, backPos.Y);
            if (backTile?.Occupant is Conveyor backConveyor)
                TileLogic.SetBackTiles(new List<ConveyorTile> { backConveyor.TileLogic });
            else
                TileLogic.SetBackTiles(new List<ConveyorTile>());

        }

        public virtual BuildingDirection GetDirectionForItem(ConveyorItem item)
        {
            return info.OutputDirections[buildingDirection][0];
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


        public override void DrawRotated(SpriteBatch sb, Point tilePos, Color tint)
        {
            var tex = Anim.Texture;
            var frame = Anim.GetCurrentFrameRect();

            float rotation = buildingDirection switch
            {
                BuildingDirection.Right => 0f,
                BuildingDirection.Down => MathF.PI / 2,
                BuildingDirection.Left => MathF.PI,
                BuildingDirection.Up => -MathF.PI / 2,
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

        public void RefreshConnection()
        {
            InitializeConnections();
        }

        public virtual IEnumerable<Point> GetNextPositions()
        {
            foreach (var dir in info.OutputDirections[buildingDirection])
            {
                yield return dir switch
                {
                    BuildingDirection.Right => new Point(TilePosition.X + 1, TilePosition.Y),
                    BuildingDirection.Left => new Point(TilePosition.X - 1, TilePosition.Y),
                    BuildingDirection.Up => new Point(TilePosition.X, TilePosition.Y - 1),
                    BuildingDirection.Down => new Point(TilePosition.X, TilePosition.Y + 1),
                    _ => TilePosition
                };
            }
        }

        public virtual Point GetNextPosition()
        {
            return GetNextPositions().First();
        }

        public virtual IEnumerable<Point> GetBackPositions()
        {
            foreach (var dir in info.ReceivedDirections[buildingDirection])
            {
                yield return dir switch
                {
                    BuildingDirection.Right => new Point(TilePosition.X + 1, TilePosition.Y),
                    BuildingDirection.Left => new Point(TilePosition.X - 1, TilePosition.Y),
                    BuildingDirection.Up => new Point(TilePosition.X, TilePosition.Y - 1),
                    BuildingDirection.Down => new Point(TilePosition.X, TilePosition.Y + 1),
                    _ => TilePosition
                };
            }
        }

        public virtual Point GetBackPosition()
        {
            return GetBackPositions().First();
        }

        public virtual Vector2 GetItemPosition(Vector2 worldPos, float local, ConveyorItem item)
        {
            return DefaultCalculate(worldPos, local, item.currentDirection);
        }

        public virtual void SetOutDir(ConveyorItem item)
        {
            item.pastOutDir = this.info.OutputDirections[buildingDirection][0];
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

                _ => worldPos + new Vector2(tileSize / 2f, tileSize / 2f)
            };

            var drawPos = centerPos - new Vector2(itemSize / 2f, itemSize / 2f);
            return drawPos;
        }

        //IItemAcceptorの実装
        public virtual bool CanAccept(ConveyorItem item, BuildingDirection fromDir)
        {
            foreach(var dir in info.ReceivedDirections[buildingDirection])
            {
                if (dir == fromDir)
                {
                    return TileLogic.CanAcceptItem();
                }
            }
            return false;
        }

        public virtual bool TryAccept(ConveyorItem item, BuildingDirection fromDir)
        {
            if(!CanAccept(item, fromDir))
            {
                return false;
            }

            return TileLogic.TryAccept(item);
        }

        public IEnumerable<BuildingDirection> GetInputDirections()
        {
            foreach(var dir in info.ReceivedDirections[buildingDirection])
            {
                yield return dir;
            }
        }

        public IEnumerable<BuildingDirection> GetOutputDirections()
        {
            foreach (var dir in info.OutputDirections[buildingDirection])
            {
                yield return dir;
            }
        }

        public bool HasSpace()
        {
            return TileLogic.HasSpace();
        }

        public bool IsFull()
        {
            return TileLogic.IsFull;
        }

        public virtual bool CanPreviewAccept(ConveyorItem item, BuildingDirection fromDir)
        {
            foreach(var dir in info.ReceivedDirections[buildingDirection])
            {
                if (dir == fromDir)
                {
                    return TileLogic.CanPreviewAccept(item);
                }
            }
            return false;
        }
    }
}
