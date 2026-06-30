using Craftory.Core;
using Point = Microsoft.Xna.Framework.Point;
using Color = Microsoft.Xna.Framework.Color;

namespace Craftory.Maps.Buildings.Conveyors
{
    public class Conveyor : BuildingInstance
    {
        public ConveyorTile TileLogic { get; private set; }

        public Conveyor(BuildType type, Point pos, BuildingDirection dir) : 
            base(type, pos, dir)
        {
            InitDirections(dir);

            TileLogic = new ConveyorTile(WorkSpeed, this);

            SetNextTile();

            TileLogic.InitializeTileStart();
        }

        protected virtual void InitDirections(BuildingDirection dir)
        {
            OutDirections[TilePosition] = new List<BuildingDirection> { dir };
            InDirections[TilePosition] = new List<BuildingDirection> { GetInDirectionFromOut(dir) };
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

        protected virtual void SetNextTile()
        {
            var nextPos = OutDirections[TilePosition][0] switch
            {
                BuildingDirection.Right => new Point(TilePosition.X + 1, TilePosition.Y),
                BuildingDirection.Left => new Point(TilePosition.X - 1, TilePosition.Y),
                BuildingDirection.Up => new Point(TilePosition.X, TilePosition.Y - 1),
                BuildingDirection.Down => new Point(TilePosition.X, TilePosition.Y + 1),
                _ => TilePosition
            };

            var tile = GameCore.Instance.MapManager.Map.GetTile(nextPos.X, nextPos.Y);
            if (tile?.Occupant is Conveyor nextConveyor)
            {
                TileLogic.SetNextTile(nextConveyor.TileLogic);

                TileLogic.InitializeTileStart();
            }
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
            SetNextTile();
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
            return TileLogic.DefaultCalculate(worldPos, local, item.currentDirection);
        }
    }
}
