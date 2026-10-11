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
    }
}
