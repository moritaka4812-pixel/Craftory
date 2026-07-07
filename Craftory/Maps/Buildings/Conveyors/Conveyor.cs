using Craftory.Core;
using Point = Microsoft.Xna.Framework.Point;
using Color = Microsoft.Xna.Framework.Color;

namespace Craftory.Maps.Buildings.Conveyors
{
    public class Conveyor : BuildingInstance, IItemAcceptor
    {
        public ConveyorTile TileLogic { get; private set; }

        public Conveyor(BuildType type, Point pos, BuildingDirection dir) : 
            base(type, pos, dir)
        {
            InitDirections(new List<BuildingDirection> { dir });

            TileLogic = new ConveyorTile(WorkSpeed, this);

            TileLogic.InitializeTileStart();

            InitializeConnections();
        }

        protected virtual void InitDirections(List<BuildingDirection> outdir)
        {
            OutDirections[TilePosition] = new List<BuildingDirection> { outdir[0] };
            InDirections[TilePosition] = new List<BuildingDirection> { GetInDirectionFromOut(outdir[0]) };
        }

        public virtual void InitializeConnections()
        {
            var tile = GameCore.Instance.MapManager.Map.GetTile(GetNextPosition().X, GetNextPosition().Y);
            if(tile?.Occupant is  Conveyor nextConveyor)
                TileLogic.SetNextTile(nextConveyor.TileLogic);
        }

        protected virtual BuildingDirection GetInDirectionFromOut(BuildingDirection outDir)
        {
            return outDir switch
            {
                BuildingDirection.Up => BuildingDirection.Down,
                BuildingDirection.Right => BuildingDirection.Left,
                BuildingDirection.Down => BuildingDirection.Up,
                BuildingDirection.Left => BuildingDirection.Right,
                _ => BuildingDirection.None
            };
        }

        public virtual BuildingDirection GetDirectionForItem(ConveyorItem item)
        {
            return OutDirections[TilePosition][0];
        }

        public override void UpdateLogic(GameTime gameTime)
        {
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

            float rotation = OutDirections[tilePos][0] switch
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

        public void RefreshNextTile()
        {
            InitializeConnections();
        }

        public virtual IEnumerable<Point> GetNextPositions()
        {
            foreach (var dir in OutDirections[TilePosition])
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
            foreach (var dir in InDirections[TilePosition])
            {
                yield return dir switch
                {
                    BuildingDirection.Right => new Point(TilePosition.X - 1, TilePosition.Y),
                    BuildingDirection.Left => new Point(TilePosition.X + 1, TilePosition.Y),
                    BuildingDirection.Up => new Point(TilePosition.X, TilePosition.Y + 1),
                    BuildingDirection.Down => new Point(TilePosition.X, TilePosition.Y - 1),
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
            item.pastOutDir = this.OutDirections[TilePosition][0];
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
        public bool CanAccept(ConveyorItem item, BuildingDirection fromDir)
        {
            foreach(var dir in InDirections[TilePosition])
            {
                if (dir == fromDir)
                {
                    return TileLogic.CanAcceptItem();
                }
            }
            return false;
        }

        public bool TryAccept(ConveyorItem item, BuildingDirection fromDir)
        {
            if(!CanAccept(item, fromDir))
            {
                return false;
            }

            return TileLogic.TryAccept(item);
        }

        public IEnumerable<BuildingDirection> GetInputDirections()
        {
            foreach(var dir in InDirections[TilePosition])
            {
                yield return dir;
            }
        }

        public IEnumerable<BuildingDirection> GetOutputDirections()
        {
            foreach (var dir in OutDirections[TilePosition])
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

        public bool CanPreviewAccept(ConveyorItem item, BuildingDirection fromDir)
        {
            foreach(var dir in InDirections[TilePosition])
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
