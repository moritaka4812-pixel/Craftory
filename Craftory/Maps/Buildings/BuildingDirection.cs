using Point = Microsoft.Xna.Framework.Point;

namespace Craftory.Maps.Buildings
{
    public enum BuildingDirection
    {
        None,
        Right,
        Left,
        Up,
        Down
    }

    public static class BuildingDirectionExtensions
    {
        public static BuildingDirection GetOpposite(this BuildingDirection dir)
        {
            return dir switch
            {
                BuildingDirection.Right => BuildingDirection.Left,
                BuildingDirection.Left => BuildingDirection.Right,
                BuildingDirection.Up => BuildingDirection.Down,
                BuildingDirection.Down => BuildingDirection.Up,
                _ => BuildingDirection.None
            };
        }

        public static BuildingDirection RotateClockWise(this BuildingDirection dir)
        {
            return dir switch
            {
                BuildingDirection.Right => BuildingDirection.Down,
                BuildingDirection.Down => BuildingDirection.Left,
                BuildingDirection.Left => BuildingDirection.Up,
                BuildingDirection.Up => BuildingDirection.Right,
                _ => BuildingDirection.None
            };
        }

        public static BuildingDirection RotateCounterClockWise(this BuildingDirection dir)
        {
            return dir switch
            {
                BuildingDirection.Right => BuildingDirection.Up,
                BuildingDirection.Up => BuildingDirection.Left,
                BuildingDirection.Left => BuildingDirection.Down,
                BuildingDirection.Down => BuildingDirection.Right,
                _ => BuildingDirection.None
            };
        }

        public static Point GetPoint(this BuildingDirection dir)
        {
            switch (dir)
            {
                case BuildingDirection.Right:
                    return new Point(1, 0);

                case BuildingDirection.Left:
                    return new Point(-1, 0);

                case BuildingDirection.Up:
                    return new Point(0, -1);

                case BuildingDirection.Down:
                    return new Point(0, 1);

                default:
                    return new Point(0, 0);
            }
        }
    }
}
