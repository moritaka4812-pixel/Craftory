using Craftory.Core;
using Craftory.Maps.Buildings.Logistics.Conveyors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Point = Microsoft.Xna.Framework.Point;
using Color = Microsoft.Xna.Framework.Color;

namespace Craftory.Maps.Buildings.Logistics.Splitters
{
    public class Splitter : Conveyor, IItemAcceptor, ISplitConveyor, ISpecialConveyor
    {
        private float blinkTimer = 0f;
        private bool isBlinking = false;

        public Splitter(BuildType type, Point pos, BuildingDirection dir)
            : base(type, pos, dir)
        {
        }


        public override void UpdateLogic(GameTime gameTime)
        {
            if (UpdateConstructingState(gameTime))
                return;

            TileLogic.Update(gameTime);

            // 点滅処理
            if (isBlinking)
            {
                blinkTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (blinkTimer <= 0f)
                    isBlinking = false;
            }
        }

        // アイテムの方向決定
        public override BuildingDirection GetDirectionForItem(ConveyorItem item)
        {
            if (item.LocalPosition < 0.5f)
                return item.currentDirection;

            // TryOutputFair が内部で outputIndex を進めるのでここでは参照だけ
            return OutputPorts[outputIndex % OutputPorts.Count].Direction;
        }


        // 出力方向を進める + 点滅開始
        public override void SetOutDir(ConveyorItem item)
        {
            isBlinking = true;
            blinkTimer = 0.15f;

            // 公平な出力処理を呼ぶ
            TryOutputFair(item);
        }

        // ConveyorTile の nextTile は使わないのでそのまま受け入れ
        public override bool TryAccept(ConveyorItem item, BuildingPort senderPort)
        {
            return TileLogic.TryAccept(item, senderPort);
        }


        // Splitter の描画（回転ではなく方向別スプライト）
        public override void Draw(SpriteBatch sb, Camera camera)
        {
            var worldPos = TilePosition.ToVector2() * 32f;

            // Idle（黒い本体）
            sb.Draw(
                info.CachedTextures[buildingDirection],
                worldPos,
                Color.White
            );

            // Active（黄色部分）
            if (isBlinking)
            {
                var outDir = buildingDirection; // 仮代入
                sb.Draw(
                    info.CachedActiveTextures[outDir],
                    worldPos,
                    Color.White
                );
            }
        }

        // Splitter の排出口は直線だと不自然なのでカーブ補正を入れる
        public override Vector2 GetItemPosition(Vector2 worldPos, float local, ConveyorItem item)
        {
            return GetCurvedPosition(worldPos, local, item.currentDirection);
        }

        private Vector2 GetCurvedPosition(Vector2 worldPos, float local, BuildingDirection dir)
        {
            const float tileSize = 32f;
            const float itemSize = 24f;

            Vector2 center = worldPos + new Vector2(tileSize / 2f, tileSize / 2f);

            float radius = tileSize / 2f;
            float angle = local * (MathF.PI / 2f);

            float baseAngle = dir switch
            {
                BuildingDirection.Up => MathF.PI,
                BuildingDirection.Right => MathF.PI * 1.5f,
                BuildingDirection.Down => 0f,
                BuildingDirection.Left => MathF.PI * 0.5f,
                _ => 0f
            };

            float finalAngle = baseAngle + angle;

            Vector2 pos = new Vector2(
                center.X + MathF.Cos(finalAngle) * radius,
                center.Y + MathF.Sin(finalAngle) * radius
            );

            return pos - new Vector2(itemSize / 2f, itemSize / 2f);
        }
    }
}
