
using Craftory.Controller;
using Craftory.Input;
using Craftory.Maps.Tiles;
using Craftory.Core;
using Craftory.Maps;
using Craftory.Maps.Buildings;

namespace Craftory.System
{
    public class TileSelectionSystem
    {
        private TileSelectionController controller;
        public Tile? SelectedTile { get; private set; }
        public ITileOccupant? SelectedOccupant { get; private set; }

        public TileSelectionSystem(TileSelectionController controller)
        {
            this.controller = controller;
        }

        public void Update(MouseInput mouse, Camera camera)
        {
            var result = controller.SelectTile(mouse, camera);

            if (result.Type == TileSelectionResultType.Outside || mouse.RightClicked()) //マップ外か右クリックで選択解除
            {
                SelectedTile = null;
                SelectedOccupant = null;
            }

            else if (result.Type == TileSelectionResultType.Selected)
            {
                SelectedTile = result.Tile;
                SelectedOccupant = result.Tile?.Occupant;
            }
        }
    }
}
