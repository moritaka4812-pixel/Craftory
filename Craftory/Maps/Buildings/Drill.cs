using Craftory.Core;
using Point = Microsoft.Xna.Framework.Point;

namespace Craftory.Maps.Buildings
{
    public class Drill : BuildingInstance
    {
        public Drill(Point pos) :
            base(BuildType.Drill, pos)
        {

        }

        public override void UpdateLogic(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(timer >= 1f / WorkSpeed)
            {
                timer -= 1f / WorkSpeed;

                var tile = GameCore.Instance.MapManager.Map.GetTile(TilePosition.X, TilePosition.Y);

                if(tile.Resource != Resource.ResourceType.None)
                {
                    GameCore.Instance.ResourceManager.Add(tile.Resource, 1);
                }
            }
        }
    }
}
