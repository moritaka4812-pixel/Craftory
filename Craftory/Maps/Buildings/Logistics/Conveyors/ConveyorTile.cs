using Craftory.Core;
using Craftory.Item;
using Craftory.Maps.Shadow;
using System.Diagnostics;
using System.DirectoryServices;
using Color = Microsoft.Xna.Framework.Color;

namespace Craftory.Maps.Buildings.Logistics.Conveyors
{
    public class ConveyorTile
    {
        public List<ConveyorItem> Items { get; private set; } = new();
        public List<ConveyorTile> InputTiles { get; private set; } = new List<ConveyorTile>(); 
        public List<ConveyorTile> OutputTiles { get; private set; } = new List<ConveyorTile>(); 
        public bool IsFull { get; private set; } = false;
        public float speed;

        float minDistance = 0.5f;

        public Conveyor ownerConveyor;

        public ConveyorTile(float speed, Conveyor ownerConveyor)
        {
            this.speed = speed;
            this.ownerConveyor = ownerConveyor;
        }

        public void SetOutputTiles(List<ConveyorTile> nexts)
        {
            OutputTiles = nexts;
        }

        public void SetInputTiles(List<ConveyorTile> backs)
        {
            InputTiles = backs;
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
                item.LocalPosition += speed * delta;
            }
        }

        public void CheckDistance()
        {
            //前のアイテムが先になる逆順ソート
            Items.Sort((a, b) => b.LocalPosition.CompareTo(a.LocalPosition));

            for(int i = 0; i< Items.Count - 1; i++)
            {
                var current = Items[i]; //先に運ばれている
                var next = Items[i+1]; //後から運ばれている

                float distance = current.LocalPosition - next.LocalPosition;

                if(distance < minDistance)
                {
                    next.LocalPosition = current.LocalPosition - minDistance;
                }
            }
        }

        public void TryMoveToNextTile()
        {
            //global::System.Diagnostics.Debug.WriteLine("A");
            if (Items.Count == 0) return;

            //global::System.Diagnostics.Debug.WriteLine("B");
            var first = Items[0];

            //global::System.Diagnostics.Debug.WriteLine($"C:{first.LocalPosition}");
            float tileEnd = 1f;


            if (first.LocalPosition >= tileEnd)
            {
                //global::System.Diagnostics.Debug.WriteLine("D");
                if (first.arrivalTime == null)
                    first.arrivalTime = GameCore.Instance.Time;

                //global::System.Diagnostics.Debug.WriteLine($"OutputTiles.Count={OutputTiles.Count}");
                //global::System.Diagnostics.Debug.WriteLine($"OutputIndex={ownerConveyor.OutputIndex}");

                var NextOut = OutputTiles[ownerConveyor.OutputIndex];

                //global::System.Diagnostics.Debug.WriteLine($"NextOut null = {NextOut == null}");

                //global::System.Diagnostics.Debug.WriteLine(NextOut?.ownerConveyor.GetType().FullName);

                if (NextOut?.ownerConveyor is IMergeConveyor)
                {
                    //global::System.Diagnostics.Debug.WriteLine("aaa");
                        var NextAcceptItem = NextOut.SelectNextAcceptItem();
                    if (first != NextAcceptItem) //Null処理を含む(first != Null)
                    {
                        first.LocalPosition = tileEnd;
                        return;
                    }
                }

                if (NextOut != null && NextOut.TryAccept(first))
                {
                    ownerConveyor.SetOutDir(first);
                }
                else
                {
                    first.LocalPosition = tileEnd;
                    return;
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

            if (Items.Count == 1 && Items[0].LocalPosition < 0.5f) //まだアイテムが半分を過ぎていない
                return false;

            item.pastTile = item.CurrentTile;
            item.pastTile?.Items.Remove(item);
            item.CurrentTile = this;
            item.arrivalTime = null;

            IsFull = false;

            item.LocalPosition -= 1f;
            Items.Add(item);
            return true;
        }

        public ConveyorItem? SelectNextAcceptItem()
        {
            var candidates = new List<ConveyorItem>();

            //Debug.WriteLine($"InputTiles={InputTiles.Count}");

            foreach (var tile in InputTiles)
            {
                if (tile == null) continue;

                //Debug.WriteLine($"TileItems={tile.Items.Count}");

                if (tile.Items.Count == 0) continue;

                var first = tile.Items[0];

                //Debug.WriteLine($"LP={first.LocalPosition}");

                if (first.LocalPosition >= 1f)
                {
                    candidates.Add(first);
                    //Debug.WriteLine("ADD");
                }
            }
            // Debug.WriteLine($"{candidates.Count}");
            if (candidates.Count == 0) return null;

            candidates.Sort((a, b) => Nullable.Compare(a.arrivalTime, b.arrivalTime));

            return candidates[0];
        }

        public bool CanAcceptItem()
        {
            if (Items.Count >= 2)
                return false;

            if (Items.Count == 1 && Items[0].LocalPosition < 0.5f)
                return false;

            return true;
        }

        public bool HasSpace()
        {
            if (Items.Count >= 2)
                return false;

            if (Items.Count == 1 && Items[0].LocalPosition < 0.5f)
                return false;

            return true;
        }

        public bool CanPreviewAccept(ConveyorItem item)
        {
            return HasSpace();
        }


        public void Draw(SpriteBatch sb, Vector2 worldPos)
        {
            // 後ろから前へ描画
            for (int i = Items.Count - 1; i >= 0; i--)
            {
                var item = Items[i];
                Vector2 pos = ownerConveyor.GetItemPosition(worldPos, item.LocalPosition, item);

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
