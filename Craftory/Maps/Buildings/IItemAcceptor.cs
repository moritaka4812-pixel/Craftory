using Craftory.Maps.Buildings.Logistics.Conveyors;

namespace Craftory.Maps.Buildings
{
    public interface IItemAcceptor
    {
        //受け取れるか
        bool CanAccept(ConveyorItem item);
        //実際に受け取る
        bool TryAccept(ConveyorItem item);
        //内部キューの状態
        bool HasSpace();
        bool IsFull();

        //受け取った場合の仮判定(実際には入れない)
        bool CanPreviewAccept(ConveyorItem item);
    }
}
