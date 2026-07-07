using Point = Microsoft.Xna.Framework.Point;

namespace Craftory.Maps.Buildings.Conveyors
{
    public interface ISplitConveyor
    {
        IEnumerable<Point> GetNextPositions();
    }
}
