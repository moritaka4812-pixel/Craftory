
using Craftory.Input;
using Craftory.Maps.Buildings;
using Craftory.UI.Core;
using Craftory.Maps.Tiles;
using Panel = Craftory.UI.Elements.Panel;
using Craftory.System;
using System.Configuration;


namespace Craftory.GameUI
{
    public class InspectorPanel : UIElement
    {
        private Panel panel;
        private readonly TileSelectionSystem selectionSystem;
        private readonly UIFactory ui;

        public InspectorPanel(TileSelectionSystem selectionSystem, UIFactory ui)
        {
            this.selectionSystem = selectionSystem;
            this.ui = ui;

            panel = new Panel(300, 400);

            panel.RelativeWidth = 0.6f;
            panel.RelativeHeight = 0.25f;

            panel.RelativeX = 0.4f;
            panel.RelativeY = 0.75f;

            var inspectorLabel = ui.CreateRelativeSmallTextLabel("Inspector", 0, 0, 100, 30);
            panel.AddChild(inspectorLabel);
        }

        public override bool Update(MouseInput mouse)
        {
            if (selectionSystem.SelectedTile == null)
                return false;

            return panel.Update(mouse);
        }

        public override void Draw(SpriteBatch sb)
        {
            Tile tile = selectionSystem.SelectedTile;
            ITileOccupant? occupant = selectionSystem.SelectedOccupant;

            if (tile == null)
                return;

            panel.Draw(sb);

        }
    }
}
