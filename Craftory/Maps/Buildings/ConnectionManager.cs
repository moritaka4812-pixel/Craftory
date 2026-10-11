
using Craftory.Core;
using Craftory.Maps.Buildings.Logistics.Conveyors;
using Point = Microsoft.Xna.Framework.Point;

namespace Craftory.Maps.Buildings
{
    public class ConnectionManager
    {
        public static void RefreshConnections(BuildingInstance building)
        {
            foreach(var port in building.Ports) //初期化
            {
                port.ConnectedPort = null;
            }

            foreach(var port in building.Ports)
            {
                ConnectPort(port);
            }

            if(building is Conveyor conveyor)
            {
                BuildConveyorConnections(conveyor);
            }
        }

        public static void RefreshNeighborConnections(BuildingInstance changeBuilding)
        {
            HashSet<BuildingInstance> neighbors = new(); //重複Refreshをなくすため

            foreach(var port in changeBuilding.Ports)
            {
                var targetPos = changeBuilding.TilePosition + port.Offset + port.Direction.GetPoint();
                var tile = GameCore.Instance.MapManager.Map.GetTile(targetPos.X, targetPos.Y);

                if(tile?.Occupant is BuildingInstance neighbor)
                {
                    neighbors.Add(neighbor);
                }
            }

            foreach (var neighbor in neighbors)
                RefreshConnections(neighbor);
        }

        private static void ConnectPort(BuildingPort port)
        {
            var worldPos = port.Owner.TilePosition + port.Offset;

            var targetPos = worldPos + port.Direction.GetPoint();

            var tile = 
                GameCore.Instance.MapManager.Map.GetTile(targetPos.X, targetPos.Y);

            if(tile?.Occupant is not BuildingInstance target)
            {
                port.ConnectedPort = null;
                return;
            }

            foreach(var targetPort in target.Ports)
            {
                if(CanConnect(port, targetPort))
                {
                    port.ConnectedPort = targetPort;
                    targetPort.ConnectedPort = port;
                }
            }
        }

        private static bool CanConnect(BuildingPort source, BuildingPort target)
        {
            if (source.Pattern != PortPattern.Output)
                return false;

            if (target.Pattern != PortPattern.Input &&
                target.Pattern != PortPattern.InputOutput)
                return false;

            return source.Direction.GetOpposite() == target.Direction;
        }

        private static void BuildConveyorConnections(Conveyor conveyor)
        {
            var outputs = new List<ConveyorTile>();

            foreach(var outputPort in conveyor.OutputPorts)
            {
                var connected = outputPort.ConnectedPort?.Owner;

                if (connected is Conveyor next)
                    outputs.Add(next.TileLogic);
            }

            conveyor.TileLogic.SetOutputTiles(outputs);

            var inputs = new List<ConveyorTile>();

            foreach(var inputPort in conveyor.InputPorts)
            {
                var connected = inputPort.ConnectedPort?.Owner;

                if (connected is Conveyor previous)
                    inputs.Add(previous.TileLogic);
            }

            conveyor.TileLogic.SetInputTiles(inputs);
        }
    }
}
