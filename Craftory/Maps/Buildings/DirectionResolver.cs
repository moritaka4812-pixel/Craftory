using Point = Microsoft.Xna.Framework.Point;

namespace Craftory.Maps.Buildings
{
    public static class DirectionResolver
    {
        //入力方向
        public static IReadOnlyList<BuildingDirection> GetInDirection(BuildingDirection dir)
        {
            return dir switch
            {
                BuildingDirection.Up => new[] { BuildingDirection.Down },
                BuildingDirection.Down => new[] { BuildingDirection.Up },
                BuildingDirection.Left => new[] { BuildingDirection.Right },
                BuildingDirection.Right => new[] { BuildingDirection.Left },
                _ => Array.Empty<BuildingDirection>()
            };
        }

        //出力方向
        public static IReadOnlyList<BuildingDirection> GetOutDirection(BuildingDirection dir)
        {
            return dir switch
            { 
                BuildingDirection.Up => new[] { BuildingDirection.Up },
                BuildingDirection.Down => new[] { BuildingDirection.Down },
                BuildingDirection.Left => new[] { BuildingDirection.Left },
                BuildingDirection.Right => new[] { BuildingDirection.Right },
                _ => Array.Empty<BuildingDirection>()
            };
        }

        public static Point GetNextTile(Point origin, BuildingDirection dir)
        {
            return dir switch
            {
                BuildingDirection.Up => origin + BuildingDirectionExtensions.GetPoint(BuildingDirection.Up),
                BuildingDirection.Down => origin + BuildingDirectionExtensions.GetPoint(BuildingDirection.Down),
                BuildingDirection.Left => origin + BuildingDirectionExtensions.GetPoint(BuildingDirection.Left),
                BuildingDirection.Right => origin + BuildingDirectionExtensions.GetPoint(BuildingDirection.Right),
                _ => origin
            };
        }


    }
}
