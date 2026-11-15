using Point = Microsoft.Xna.Framework.Point;
using Craftory.Core;

namespace Craftory.Maps.Buildings.Processor
{
    public class Compressor : BuildingInstance // IItemAcceptor
    {
        public Compressor(string id, Point pos, BuildingDirection dir) :
            base(id, pos, dir)
        {
        }

        public override void UpdateLogic(GameTime gameTime)
        {
            base.UpdateLogic(gameTime);

            if(State == BuildState.Active)
            {
                ProcessCompression(gameTime);
            }
        }

        protected void ProcessCompression(GameTime gameTime)
        {

        }
    }
}
