
using Craftory.Core;
using Point = Microsoft.Xna.Framework.Point;

namespace Craftory.Maps.Buildings
{
    public static class BuildingPortFactory
    {
        public static BuildingPort Create(
            PortDefinitions definition,
            BuildingInfo info,
            BuildingDirection buildingDirection
            )
        {
            return new BuildingPort
            {
                Name = definition.Name,
                Offset = RotateOffset(
                    definition.Offset,
                    info,
                    buildingDirection
                    ),

                Direction = RotateDirection(
                    definition.Direction,
                    buildingDirection
                    ),

                Pattern = definition.Pattern,
            };
        }

        public static List<BuildingPort> CreatePorts(
            BuildingInfo info, 
            BuildingDirection buildingDirection
            )
        {
            return info.PortDefinitions
                .Select(p => Create(
                    p,
                    info,
                    buildingDirection))
                .ToList();
        }

        private static Point RotateOffset(Point offset, BuildingInfo info, BuildingDirection buildingDirection)
        {
            switch (buildingDirection)
            {
                case BuildingDirection.None:
                    return offset;

                case BuildingDirection.Right:
                    return offset;
                case BuildingDirection.Down:
                    return new Point(info.Height - offset.Y - 1, offset.X);
                case BuildingDirection.Left:
                    return new Point(info.Width - offset.X - 1, info.Height - offset.Y - 1);
                case BuildingDirection.Up:
                    return new Point(offset.Y, info.Width - offset.X - 1);
                default:
                    throw new ArgumentOutOfRangeException(nameof(buildingDirection), buildingDirection, null);
            }
        }

        private static BuildingDirection RotateDirection(BuildingDirection portDirection, BuildingDirection buildingDirection)
        {
            int rotateCount = buildingDirection switch
            {
                BuildingDirection.Right => 0,
                BuildingDirection.Down => 1,
                BuildingDirection.Left => 2,
                BuildingDirection.Up => 3,
                _ => 0
            };

            var result = portDirection;

            for (int i = 0; i < rotateCount; i++)
            {
                result = result.RotateClockWise();
            }

            return result;
        }
    }
}
