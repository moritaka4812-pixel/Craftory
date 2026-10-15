
using Craftory.Core;
using Craftory.Maps.Buildings.Logistics.Conveyors;
using System.Diagnostics;
using Point = Microsoft.Xna.Framework.Point;

namespace Craftory.Maps.Buildings
{
    public class ConnectionManager
    {
        public static void RefreshConnections(BuildingInstance building)
        {
            //Debug.WriteLine($"Refresh {building.GetType()}");
            foreach (var port in building.Ports) //初期化
            {
                port.ConnectedPort = null;
            }

            foreach(var port in building.Ports)
            {
                ConnectPort(port);
            }

            if (building is Conveyor conveyor)
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
                    //Debug.WriteLine($"{port.Direction} Change owner = {changeBuilding.GetType()}");
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

            //Debug.WriteLine($"TARGETPOS={targetPos}");
            
            /*
            Debug.WriteLine(
            $"{port.Owner.Type} " +
            $"{port.Name} " +
            $"Offset={port.Offset} " +
            $"Dir={port.Direction} " +
            $"Target={targetPos}");
            */

            var tile = 
                GameCore.Instance.MapManager.Map.GetTile(targetPos.X, targetPos.Y);

            //Debug.WriteLine($"OCCUPANT={tile?.Occupant?.GetType().Name}");

            if (tile?.Occupant is not BuildingInstance target)
            {
                //Debug.WriteLine($"NO TARGET : {targetPos}");
                //Debug.WriteLine($"TARGET = {tile.Occupant?.GetType()}");
                port.ConnectedPort = null;
                return;
            }

            foreach(var targetPort in target.Ports)
            {
                if(CanConnect(port, targetPort))
                {
                    port.ConnectedPort = targetPort;
                    targetPort.ConnectedPort = port;
                    //global::System.Diagnostics.Debug.WriteLine($"Connected: {port.Name} - {targetPort.Name}");
                    //Debug.WriteLine($"source: {port.Name} Hash={port.GetHashCode()}");
                    //Debug.WriteLine($"target: {targetPort.Name} Hash={targetPort.GetHashCode()}");
                }
            }
        }

        private static bool CanConnect(BuildingPort source, BuildingPort target)
        {
            bool outputToInput =
                source.Pattern == PortPattern.Output &&
                (target.Pattern == PortPattern.Input ||
                 target.Pattern == PortPattern.InputOutput) &&
                source.Direction.GetOpposite() == target.Direction;

            bool inputToOutput =
                (source.Pattern == PortPattern.Input ||
                 source.Pattern == PortPattern.InputOutput) &&
                target.Pattern == PortPattern.Output &&
                source.Direction.GetOpposite() == target.Direction;

            return outputToInput || inputToOutput;
        }

        private static void BuildConveyorConnections(Conveyor conveyor)
        {
            var outputs = new List<ConveyorTile>();

            foreach(var outputPort in conveyor.OutputPorts)
            {
                var connected = outputPort.ConnectedPort?.Owner;

                outputs.Add(connected is Conveyor next ? next.TileLogic : null);
            }

            conveyor.TileLogic.SetOutputTiles(outputs);

            var inputs = new List<ConveyorTile>();

            foreach(var inputPort in conveyor.InputPorts)
            {
                var connected = inputPort.ConnectedPort?.Owner;

                inputs.Add(connected is Conveyor previous ? previous.TileLogic : null);
            }

            conveyor.TileLogic.SetInputTiles(inputs);
        }
    }
}
