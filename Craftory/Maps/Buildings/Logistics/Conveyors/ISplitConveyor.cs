using Point = Microsoft.Xna.Framework.Point;

namespace Craftory.Maps.Buildings.Logistics.Conveyors
{
    public interface ISplitConveyor
    {
        IEnumerable<Point> GetNextPositions();
    }
}
