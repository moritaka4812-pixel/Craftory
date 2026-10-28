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
        private BuildingDirection blinkDir;

        public Splitter(string id, Point pos, BuildingDirection dir)
            : base(id, pos, dir)
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
            return item.currentDirection;
        }


        // 出力方向を進める + 点滅開始
        public override void AdvanceOutputIndex(int selectedIndex)
        {
            blinkTimer = 0.15f;
            isBlinking = true;
            blinkDir = OutputPorts[selectedIndex].Direction;

            outputIndex = (selectedIndex + 1) % OutputPorts.Count;
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

            base.Draw(sb, camera);

            // Active（黄色部分）
            if (isBlinking)
            {
                sb.Draw(
                    info.CachedActiveTextures[blinkDir],
                    worldPos,
                    Color.White
                );
            }
        }

        public override Vector2 GetItemPosition(Vector2 worldPos, float local, ConveyorItem item)
        {
            return SplitCalculate(worldPos, local, item.currentDirection);
        }

        public Vector2 SplitCalculate(Vector2 worldPos, float pos, BuildingDirection dir)
        {
            const float tileSize = 32f;
            const float itemSize = 24f;
            const float centerOffset = (tileSize - itemSize) / 2f; //タイル中央への補正

            pos = MathF.Min(pos, 0.5f);
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

                _ => throw new Exception($"undefined")
            };

            var drawPos = centerPos - new Vector2(itemSize / 2f, itemSize / 2f);
            return drawPos;
        }
    }
}
