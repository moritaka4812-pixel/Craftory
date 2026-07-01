
using Craftory.Core;
using Craftory.Item;
using Craftory.Maps.Shadow;
using System.DirectoryServices;
using Color = Microsoft.Xna.Framework.Color;

namespace Craftory.Maps.Buildings.Conveyors
{
    public class ConveyorTile
    {
        public List<ConveyorItem> Items { get; private set; } = new();
        public ConveyorTile nextTile { get; private set; } = null;
        public ConveyorTile backTile { get; private set; } = null;
        public bool IsFull { get; private set; } = false;
        public float speed;
        public float TileStart; //このタイルの開始距離

        float minDistance = 0.5f;

        public Conveyor ownerConveyor;

        public ConveyorTile(float speed, Conveyor ownerConveyor)
        {
            this.speed = speed;
            this.ownerConveyor = ownerConveyor;
        }
        public void SetNextTile(ConveyorTile next)
        {
            this.nextTile = next;
            if (next != null)
            {
                next.backTile = this;

                InitializeTileStart();

                next.InitializeTileStart();
            }
        }

        public void InitializeTileStart()
        {
            if (backTile == null)
                TileStart = 0f;
            else
                TileStart = backTile.TileStart + 1f;
        }

        public void Update(GameTime time)
        {
            foreach (var item in Items)
                UpdateDirection(item);

            UpdatePosition(time);
            CheckDistance();
            TryMoveToNextTile();
        }

        public void UpdateDirection(ConveyorItem item)
        {
            item.currentDirection = ownerConveyor.GetDirectionForItem(item);
        }

        public void UpdatePosition(GameTime time)
        {
            float delta = (float)time.ElapsedGameTime.TotalSeconds;
            foreach (var item in Items)
            {
                item.GlobalPosition += speed * delta;
            }
        }

        public void CheckDistance()
        {
            //前のアイテムが先になる逆順ソート
            Items.Sort((a, b) => b.GlobalPosition.CompareTo(a.GlobalPosition));

            for(int i = 0; i< Items.Count - 1; i++)
            {
                var current = Items[i]; //先に運ばれている
                var next = Items[i+1]; //後から運ばれている

                float distance = current.GlobalPosition - next.GlobalPosition;

                if(distance < minDistance)
                {
                    next.GlobalPosition = current.GlobalPosition - minDistance;
                }
            }
        }

        public void TryMoveToNextTile()
        {
            if (Items.Count == 0) return;

            var first = Items[0];

            float tileEnd = TileStart + 1f;

            if (first.GlobalPosition >= tileEnd)
            {

                if (nextTile != null && nextTile.TryAccept(first))
                {
                    Items.RemoveAt(0);
                }
                else
                {
                    first.GlobalPosition = tileEnd;
                }
                
            }
        }

        public bool TryAccept(ConveyorItem item)
        {
            if (Items.Count >= 2)
            {
                IsFull = true;
                return false;
            }
            item.pastTile = this;
            item.pastOutDir = item.currentDirection;
            IsFull = false;

            item.GlobalPosition = TileStart;
            Items.Add(item);
            return true;
        }

        public void Draw(SpriteBatch sb, Vector2 worldPos)
        {
            // 後ろから前へ描画
            for (int i = Items.Count - 1; i >= 0; i--)
            {
                var item = Items[i];
                float local = item.GlobalPosition - TileStart; // 0 ~ 1
                Vector2 pos = ownerConveyor.GetItemPosition(worldPos, local, item);

                sb.Draw(
                    ItemRegistry.Data[item.Type].Texture,
                    pos,
                    null,
                    Color.White,
                    0f,
                    Vector2.Zero,
                    24f / 32f,
                    SpriteEffects.None,
                    0f
                );
            }
        }


        public Vector2 DefaultCalculate(Vector2 worldPos, float pos, BuildingDirection dir)
        {
            const float tileSize = 32f;
            const float itemSize = 24f;
            const float centerOffset = (tileSize - itemSize) / 2f; //タイル中央への補正
            float visualOffset = 0.97f;

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
    }
}
