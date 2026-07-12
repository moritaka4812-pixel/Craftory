using Craftory.Maps.Buildings.Conveyors;

namespace Craftory.Maps.Buildings
{
    public interface IItemAcceptor
    {
        //受け取れるか
        bool CanAccept(ConveyorItem item, BuildingDirection fromDir);
        //実際に受け取る
        bool TryAccept(ConveyorItem item, BuildingDirection fromDir);
        //入力方向一覧
        IEnumerable<BuildingDirection> GetInputDirections();
        //出力方向一覧
        IEnumerable<BuildingDirection> GetOutputDirections();

        //内部キューの状態
        bool HasSpace();
        bool IsFull();

        //受け取った場合の仮判定(実際には入れない)
        bool CanPreviewAccept(ConveyorItem item, BuildingDirection fromDir);
    }
}
