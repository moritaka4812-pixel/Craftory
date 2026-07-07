
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
    }
}
