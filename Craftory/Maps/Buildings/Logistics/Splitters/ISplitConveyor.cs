using Point = Microsoft.Xna.Framework.Point;

namespace Craftory.Maps.Buildings.Logistics.Splitters
{
    public interface ISplitConveyor
    {
        IEnumerable<Point> GetNextPositions();
    }
}
