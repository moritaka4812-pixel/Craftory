ConveyorSplitterの実装をしてください。Splitterは一方向からの入力を受けてラウンドロビンを使って均等に出力する建物でConveyorを継承して
実装される。一方向が詰まったときにには正しく他の二方向に1対1で等しく配分出力できるように実装されなければいけない。ConveyorはBuildingInstanceを
継承しています。

以下はBuildingInstance.csです。

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

    public class BuildingInstance : ITileOccupant
    {
        protected int outputIndex = 0; //出力方向のラウンドロビンキャッシュ
        public int OutputIndex => outputIndex; //読み取り専用
        public BuildType Type { get; private set; }     // 建物の種類
        public Point TilePosition { get; private set; } //タイル座標
        public Point SizeInTiles { get; private set; }  //タイル単位の大きさ
        public bool IsActive => State == BuildState.Active;      //稼働状況
        public float WorkSpeed { get; private set; }    //採掘速度など、タイプ依存の性能値
        public List<Point> OccupiedTiles { get; private set; }
        public BuildState State { get; private set; }
        public float BuildProgress { get; private set; }
        public float BuildTime { get; private set; }
        public BuildingDirection buildingDirection; //入出力に寄らない建物の向き (画像準拠)
        public List<BuildingPort> Ports { get; private set; }
        public List<BuildingPort> InputPorts { get; } = new List<BuildingPort>();
        public List<BuildingPort> OutputPorts { get; } = new List<BuildingPort>();

        public TileAnimation Anim;
        public BuildingInfo info;

        protected float timer = 0f;

        public BuildingInstance(BuildType type, Point tilePosition, BuildingDirection dir)
        {
            Type = type;
            TilePosition = tilePosition;

            info = BuildingRegistry.Data[type];

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
            
            //稼働中の通常ロジック（継承先で実装）
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
            if(!IsActive) return;
            Anim.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch sb, Camera camera)
        {
            var worldPos = TilePosition.ToVector2() * 32;

            if(State == BuildState.Constructing)
            {
                //建設中の建物の表示アニメーション
                return;
                
            }
            // 建物のスプライト描画
            Anim.Draw(sb, worldPos);
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

        protected bool TryOutputFair(ConveyorItem item)
        {　
            if (OutputPorts.Count == 0)
                return false;

            int count = OutputPorts.Count;

            for(int i=0; i< count; i++)
            {
                var idx = (outputIndex + i) % count;
                var candidatePort = OutputPorts[idx];

                var targetBuilding = candidatePort.ConnectedPort?.Owner;

                if (targetBuilding is not IItemAcceptor acceptor)
                    continue;

                if (!acceptor.CanAccept(item))
                    continue;

                if (acceptor.TryAccept(item, candidatePort.Direction))
                {
                    outputIndex = (idx + 1) % count;
                    return true;
                }
            }

            return false;
        }

        public BuildingPort? GetPort(string name)
        {
            return Ports.FirstOrDefault(p => p.Name == name);
        }
    }
}

---
以下はConveyor.csです。

using Craftory.Core;
using Point = Microsoft.Xna.Framework.Point;
using Color = Microsoft.Xna.Framework.Color;
using System.Diagnostics;

namespace Craftory.Maps.Buildings.Logistics.Conveyors
{
    public class Conveyor : BuildingInstance, IItemAcceptor, IPureConveyor
    {
        public ConveyorTile TileLogic { get; private set; }

        public Conveyor(BuildType type, Point pos, BuildingDirection dir) : 
            base(type, pos, dir)
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
            if (UpdateConstructingState(gameTime)) 
                return;

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

        public virtual void SetOutDir(ConveyorItem item)
        {
            item.pastOutDir = GetPort("Output").Direction;
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
        public virtual bool CanAccept(ConveyorItem item)
        {
            return TileLogic.CanAcceptItem();
        }

        public virtual bool TryAccept(ConveyorItem item, BuildingDirection dir)
        {
            return TileLogic.TryAccept(item, dir);
        }

        public bool HasSpace()
        {
            return TileLogic.HasSpace();
        }

        public bool IsFull()
        {
            return TileLogic.IsFull;
        }

        public virtual bool CanPreviewAccept(ConveyorItem item)
        {
            return TileLogic.CanPreviewAccept(item);
        }
    }
}

回答は簡潔にしてください。
原因候補と根拠を優先してください。
不明な場合は仮説として提示してください。