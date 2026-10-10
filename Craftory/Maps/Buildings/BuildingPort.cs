using Point = Microsoft.Xna.Framework.Point;

namespace Craftory.Maps.Buildings
{
    public enum PortPattern
    {
        Input,
        Output,
        InputOutput
    }


    public class BuildingPort
    {
        public string Name { get; set; } //Portの名前
        public Point Offset { get; set; } //建物の左上からのオフセット座標

        public BuildingDirection Direction { get; set; } //Portの向き

        public PortPattern Pattern { get; set; }

        public BuildingPort? ConnectedPort { get; set; } //接続されているPort

    }

    public class PortDefinitions //Portの静的情報を保持するクラス
    {
        public string Name { get; set; }
        public Point Offset { get; set; }
        public BuildingDirection Direction { get; set; }
        public PortPattern Pattern { get; set; }
    }
}
