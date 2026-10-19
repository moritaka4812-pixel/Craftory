# Craftory

## ゲームの内容

Craftoryは、タイルベースの2Dゲームで地形にあるタイルの鉱石を採掘して様々な設備を建設して鉱石をアイテムに加工するゲーム。
流通設備を建設してユーザーが自由にマップ上を工場へと作りかえることができる、工業系のゲーム。
設計やゲームの参考としてMindustryを参考にして設計や開発を勧めている。

## プレイヤー体験

- アイテムの加工ラインの設計
- 加工ラインの最適化
- 物流ネットワークの拡張
- 新アイテムや設備の開発

## ゲームの設計要素（予定含む）

- タイルにコンベアを設置して流通網を作る
- アイテムを加工する
- 加工アイテムを使って新しい設備を作る
- 豊富なアイテムの加工レシピを用意する
- 技術ツリーを用意して段階的に新しい設備やアイテムを解放する
- 自動生成のマップを用意する
- 電力管理の概念を導入する
- 熱源管理の概念を導入する
- 任意で敵を出現させて工場を守る要素を導入する

## 設計方針

- オブジェクト配置型
- タイルベースの2Dゲーム
- 拡張しやすい構造を重視


次の実装案としては生産系設備の実装を考えているのだけれども、以下のBuildingInstanceを元に銅鉱石５つから圧縮機のクラスをつくってくれる？


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
回答は簡潔にしてください。
原因候補と根拠を優先してください。
不明な場合は仮説として提示してください。