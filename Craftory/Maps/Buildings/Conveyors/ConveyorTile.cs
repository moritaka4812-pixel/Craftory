
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
        public List<ConveyorTile> inputTiles { get; private set; } = new List<ConveyorTile>(); //Merge用
        public List<ConveyorTile> outputTiles { get; private set; } = new List<ConveyorTile>(); //Split用
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

        public void SetNextTiles(List<ConveyorTile> nexts)
        {
            outputTiles = nexts;
            nextTile = nexts.Count == 1 ? nexts[0] : null;

            InitializeTileStart();

            foreach (var next in nexts)
            {
                next.backTile = this;
                next.InitializeTileStart();
            }
        }

        public void SetBackTiles(List<ConveyorTile> backs)
        {
            inputTiles = backs;
            backTile = backs.Count == 1 ? backs[0] : null;

            foreach (var back in backs)
            {
                back.nextTile = this;
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

        public void UpdateMerge(GameTime time)
        {
            foreach (var item in Items)
                UpdateDirection(item);

            UpdatePosition(time);
            CheckDistance();

            TryAcceptMerge();

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
                if(first.arrivalTime == null)
                    first.arrivalTime = GameCore.Instance.Time;

                if (nextTile != null && nextTile.TryAccept(first))
                {
                    ownerConveyor.SetOutDir(first);
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

            if (Items.Count == 1 && Items[0].GlobalPosition - TileStart < 0.5f) //まだアイテムが半分を過ぎていない
                return false;

            item.pastTile = this;
            item.arrivalTime = null;

            IsFull = false;

            item.GlobalPosition = TileStart;
            Items.Add(item);
            return true;
        }

        public void TryAcceptMerge()
        {
            if (inputTiles.Count <= 1) return;

            var candidates = new List<ConveyorItem>();

            foreach( var tile in inputTiles)
            {
                if (tile.Items.Count == 0) continue;

                var first = tile.Items[0];
                float tileEnd = tile.TileStart + 1f;

                if (first.GlobalPosition >= tileEnd)
                    candidates.Add(first);
            }

            if (candidates.Count == 0) return;

            candidates.Sort((a, b) => Nullable.Compare(a.arrivalTime, b.arrivalTime));

            var item = candidates[0];

            item.pastTile.Items.Remove(item);

            TryAccept(item);
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
    }
}
