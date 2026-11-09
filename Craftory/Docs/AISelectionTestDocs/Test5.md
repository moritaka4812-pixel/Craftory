二つのアイテムが描画上移動しなくなった時にConveyor自体の描画も止めるようにしたい。具体的にはConveyorのアイテムの数が二つ
でLocalPositionが先頭から1f,0.5fであればアイテムの移動が止まるため、そのときにコンベアの描画も止めたい。以下のコードだと、
意図通りにならない。Conveyor.IsBlockedをdebug.WriteLineしたところTrueであった。どの箇所が原因か。

BuildingInstance.cs

---

using Craftory.Core;
using Craftory.Maps.Tiles;
using Point = Microsoft.Xna.Framework.Point;
using Color = Microsoft.Xna.Framework.Color;
using Craftory.Maps.Buildings.Logistics.Conveyors;

namespace Craftory.Maps.Buildings
{
    public enum BuildState
    {
        Constructing,
        Active,
        Deconstructing
    }

    public enum  BuildingVisualState
    {
        Constructing,
        Idle,
        Starting,
        Stopping,
        Working,
        Blocked,
        NoPower,
        Deconstructing
    }

    public class BuildingInstance : ITileOccupant
    {
        protected int outputIndex = 0; //出力方向のラウンドロビンキャッシュ
        public BuildType Type { get; private set; }     // 建物の種類
        public Point TilePosition { get; private set; } //タイル座標
        public Point SizeInTiles { get; private set; }  //タイル単位の大きさ
        public bool IsActive => State == BuildState.Active;      //稼働状況
        public float WorkSpeed { get; private set; }    //採掘速度など、タイプ依存の性能値
        public List<Point> OccupiedTiles { get; private set; }
        public BuildState State { get; private set; }
        //継承先でオーバーライドして、建物の状態に応じたビジュアルを返す
        public virtual BuildingVisualState VisualState 
        {
            get
            {
                return State switch { 
                    BuildState.Constructing => BuildingVisualState.Constructing,
                    BuildState.Deconstructing => BuildingVisualState.Deconstructing,
                    _ when IsStarting => BuildingVisualState.Starting,
                    _ when IsStopping => BuildingVisualState.Stopping,
                    _ when IsReallyBlocked => BuildingVisualState.Blocked,
                    _ when IsWorking => BuildingVisualState.Working,
                    _ => BuildingVisualState.Idle
                };
            }
        }
        public virtual bool IsWorking => false;
        public virtual bool IsBlocked => false;
        public virtual bool IsStarting => false;
        public virtual bool IsStopping => false;
        public bool IsReallyBlocked => blockedTime > 0.3f; 
        public bool IsIdle => !IsWorking && !IsBlocked;
        public float BuildProgress { get; private set; }
        public float BuildTime { get; private set; }
        protected float blockedTime = 0f; //Blocked状態の継続時間を計測するタイマー
        public BuildingDirection buildingDirection; //入出力に寄らない建物の向き (画像準拠)
        public List<BuildingPort> Ports { get; private set; }
        public List<BuildingPort> InputPorts { get; } = new List<BuildingPort>();
        public List<BuildingPort> OutputPorts { get; } = new List<BuildingPort>();

        public TileAnimation Anim;
        public BuildingInfo info;

        protected float timer = 0f;

        public BuildingInstance(string id, Point tilePosition, BuildingDirection dir)
        {
            TilePosition = tilePosition;

            info = BuildingRegistry.Data[id];

            Type = info.Type;

            Anim = info.CreateTileAnimation(dir);

            SizeInTiles = info.SizeInTiles;
            WorkSpeed = info.WorkSpeed;

            OccupiedTiles = new List<Point>();
            for (int x = 0; x < info.Width; x++)
            {
                for (int y = 0; y < info.Height; y++)
                {
                    OccupiedTiles.Add(new Point(tilePosition.X + x, tilePosition.Y + y));
                }
            }

            buildingDirection = dir;

            Ports = BuildingPortFactory.CreatePorts(this, info, buildingDirection);
            foreach(var port in Ports)
            {
                if (port.Pattern == PortPattern.Input || port.Pattern == PortPattern.InputOutput)
                    InputPorts.Add(port);

                if (port.Pattern == PortPattern.Output || port.Pattern == PortPattern.InputOutput)
                    OutputPorts.Add(port);
            }

            State = BuildState.Constructing;
            BuildProgress = 0f;
            BuildTime = info.BuildTime;
        }

        public virtual void UpdateLogic(GameTime gameTime)
        {
            if (UpdateConstructingState(gameTime)) return;
        }

        protected void UpdateBlockedTime(bool blocked, float dt)
        {
            if (blocked)
                blockedTime += dt;
            else
                blockedTime = 0f;
        }

        protected bool UpdateConstructingState(GameTime gameTime)
        {
            if (State == BuildState.Constructing)
            {
                BuildProgress += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (BuildProgress >= BuildTime)
                    State = BuildState.Active;

                return true; // Constructing 中
            }

            if (State == BuildState.Deconstructing)
            {
                // 解体処理
                return true; // Deconstructing 中
            }

            return false; // Active
        }


        public virtual void UpdateVisual(GameTime gameTime)
        {
            switch(VisualState)
            {
                case BuildingVisualState.Working:
                    Anim.Update(gameTime);
                    break;

                case BuildingVisualState.Starting:
                    Anim.Update(gameTime);
                    break;

                case BuildingVisualState.Stopping:
                    Anim.Update(gameTime);
                    break;

                default:
                    break;
            }
        }

        public virtual void Draw(SpriteBatch sb, Camera camera)
        {
            var worldPos = TilePosition.ToVector2() * 32;

            switch (VisualState)
            {
                case BuildingVisualState.Idle:
                    Anim.Draw(sb, worldPos);
                    break;

                case BuildingVisualState.Constructing:
                    Anim.Draw(sb, worldPos);
                    break;

                case BuildingVisualState.Working:
                    Anim.Draw(sb, worldPos);
                    break;

                case BuildingVisualState.Blocked:
                    Anim.Draw(sb, worldPos);
                    break;

                case BuildingVisualState.NoPower:
                    Anim.Draw(sb, worldPos);
                    break;

                default:
                    throw new Exception($"BuildingInstance.Draw: VisualState {VisualState} is not handled.");
            }
        }

        public virtual void DrawRotated(SpriteBatch sb, Point tilePos, Color tint) //標準回転描画(buildingDirection基準)
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

        public IEnumerable<Point> GetOccupiedTiles()
        {
            for(int dx = 0; dx < SizeInTiles.X; dx++)
            {
                for (int dy = 0; dy < SizeInTiles.Y; dy++)
                {
                    yield return new Point(TilePosition.X + dx, TilePosition.Y + dy);
                }
            }
        }

        public BuildingPort? TryOutputFair(ConveyorItem item)
        {　
            if (OutputPorts.Count == 0)
                return null;

            int count = OutputPorts.Count;

            for(int i=0; i< count; i++)
            {
                var idx = (outputIndex + i) % count;
                var candidatePort = OutputPorts[idx];

                var targetBuilding = candidatePort.ConnectedPort?.Owner;

                if (targetBuilding is not IItemAcceptor acceptor)
                    continue;

                if (!acceptor.CanAccept(item, candidatePort))
                    continue;

                if (acceptor.TryAccept(item, candidatePort))
                {
                    outputIndex = (idx + 1) % count;
                    return candidatePort;
                }
            }

            return null;
        }

        public BuildingPort? PeekOutputPort(ConveyorItem item = null)
        {
            var idx = FindOutputIndex(item);

            return idx == null ? null : OutputPorts[idx.Value];
        }

        public int? PeekOutputIndex(ConveyorItem item = null)
        {
            return FindOutputIndex(item);
        }

        protected virtual int? FindOutputIndex(ConveyorItem? item)
        {
            item ??= new();

            if (OutputPorts.Count == 0)
                return null;

            int count = OutputPorts.Count;

            for (int i = 0; i < count; i++)
            {
                var idx = (outputIndex + i) % count;
                var port = OutputPorts[idx];

                var target = port.ConnectedPort?.Owner;

                if (target is not IItemAcceptor acceptor)
                    continue;

                if (acceptor.CanAccept(item, port))
                    return idx;
            }

            return null;
        }

        public virtual void AdvanceOutputIndex(int selectedIndex)
        {
        }

        public BuildingPort? GetPort(string name)
        {
            return Ports.FirstOrDefault(p => p.Name == name);
        }
    }
}

---

Conveyor.cs

---

using Craftory.Core;
using Point = Microsoft.Xna.Framework.Point;
using Color = Microsoft.Xna.Framework.Color;
using System.Diagnostics;

namespace Craftory.Maps.Buildings.Logistics.Conveyors
{
    public class Conveyor : BuildingInstance, IItemAcceptor, IPureConveyor
    {
        public ConveyorTile TileLogic { get; private set; }
        public override bool IsWorking => IsActive;
        public override bool IsBlocked => IsActive && TileLogic.IsBlocked;
        public Conveyor(string id, Point pos, BuildingDirection dir) : 
            base(id, pos, dir)
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
            base.UpdateLogic(gameTime);

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

---

ConveyorTile.cs

---

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
        public bool IsBlocked => Items.Count == 2 && Items.First().LocalPosition >= 1f && Items[1].LocalPosition >= 0.5f;
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
            if (Items.Count == 0) return;

            var first = Items[0];

            float tileEnd = 1f;


            if (first.LocalPosition >= tileEnd)
            {
                if (first.arrivalTime == null)
                    first.arrivalTime = GameCore.Instance.Time;

                int? idx = ownerConveyor.PeekOutputIndex();

                if(idx == null)
                {
                    first.LocalPosition = tileEnd;
                    return;
                }

                var NextOut = OutputTiles[(int)idx];

                if (NextOut?.ownerConveyor is IMergeConveyor)
                {
                    var NextAcceptItem = NextOut.SelectNextAcceptItem();
                    if (first != NextAcceptItem) //Null処理を含む(first != Null)
                    {
                        first.LocalPosition = tileEnd;
                        return;
                    }
                }

                var outPort = ownerConveyor.OutputPorts[(int)idx];

                if (NextOut != null && NextOut.TryAccept(first, outPort))
                {
                    first.currentDirection = outPort.Direction;
                    ownerConveyor.AdvanceOutputIndex((int)idx);
                }
                else
                {
                    first.LocalPosition = tileEnd;
                    return;
                }
                
            }
        }

        public bool TryAccept(ConveyorItem item, BuildingPort senderPort)
        {
            if (Items.Count >= 2)
            {
                IsFull = true;
                return false;
            }

            if (Items.Count == 1 && Items[0].LocalPosition < 0.5f) //まだアイテムが半分を過ぎていない
                return false;

            item.InitializeDirections(senderPort.Direction, senderPort.Direction);
            item.currentDirection = ownerConveyor.GetDirectionForItem(item);
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

            foreach (var tile in InputTiles)
            {
                if (tile == null) continue;

                if (tile.Items.Count == 0) continue;

                var first = tile.Items[0];

                if (first.LocalPosition >= 1f)
                {
                    candidates.Add(first);
                }
            }
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

        public bool CanPreviewAccept(ConveyorItem item, BuildingPort senderPort)
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
