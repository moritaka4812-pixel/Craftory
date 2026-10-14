using Craftory.Item;

namespace Craftory.Maps.Buildings.Logistics.Conveyors
{
    public class ConveyorItem
    {
        public ItemType Type;
        public float LocalPosition;
        public BuildingDirection currentDirection;
        public ConveyorTile CurrentTile;
        public ConveyorTile pastTile;
        public BuildingDirection pastOutDir;
        public float? arrivalTime;

        public static ConveyorItem Create(ItemType type)
        {
            return new ConveyorItem {
                Type = type,
                LocalPosition = 1f,
                currentDirection = BuildingDirection.None,
                CurrentTile = null,
                pastTile = null,
                pastOutDir = BuildingDirection.None,
                arrivalTime = null
            };
        }
    }
}
