using Craftory.UI.Elements;
using Panel = Craftory.UI.Elements.Panel;
using Button = Craftory.UI.Elements.Button;
using Rect = Microsoft.Xna.Framework.Rectangle;
using Color = Microsoft.Xna.Framework.Color;
using Craftory.UI.Core;
using Craftory.Input;
using System.DirectoryServices;
using Craftory.Maps.Buildings;

namespace Craftory.GameUI
{
    public class ToolPanel : UIElement
    {
        public Panel panel; //UI.Elements.Panel
        public event Action<BuildType> OnBuildRequested;
        public event Action OnResetRequested;
        public event Action OnRemoveRequested;
        private Button handleButton; //取って
        private bool isOpen;
        private float targetX;
        private Button? activeButton = null;
        float openX;
        float closedX;

        public ToolPanel(UIFactory ui)
        {
            panel = new Panel(200, 300);
            panel.RelativeHeight = 1f;
            panel.RelativeWidth = 0.35f;
            panel.RelativeX = -panel.RelativeWidth; //初期の閉じている状態
            panel.RelativeY = 0f;
            panel.OnLeftClickHandler = (mouse) => { return true; };

            handleButton = ui.CreateTextButton("T", 0, 0, 40, 40);
            handleButton.OnClicked += Toggle;

            var list = new ScrollMultiList();
            list.RelativeY = 0.15f;
            list.RelativeHeight = 0.7f;
            list.RelativeWidth = 1f;
            list.BackgroundColor = new Color(30, 30, 30, 200);

            AddBuildButton(ui, list, "Buildings/Miners/Drill", BuildType.Drill, 32);
            AddBuildButton(ui, list, "Buildings/Logistics/Conveyors/ConveyorStraight", BuildType.Conveyor, 32);
            AddBuildButton(ui, list, "Buildings/Logistics/Conveyors/ConveyorRightCurve", BuildType.ConveyorRightCurve, 32);
            AddBuildButton(ui, list, "Buildings/Logistics/Conveyors/ConveyorLeftCurve", BuildType.ConveyorLeftCurve, 32);
            AddBuildButton(ui, list, "Buildings/Logistics/Conveyors/ConveyorRightMerge", BuildType.ConveyorRightMerge, 32);
            AddBuildButton(ui, list, "Buildings/Logistics/Conveyors/ConveyorLeftMerge", BuildType.ConveyorLeftMerge, 32);
            AddBuildButton(ui, list, "Buildings/Logistics/Conveyors/ConveyorRightLeftMerge", BuildType.ConveyorRightLeftMerge, 32);
            AddBuildButton(ui, list, "Buildings/Logistics/Conveyors/ConveyorMerge", BuildType.ConveyorMerge, 32);
            AddBuildButton(ui, list, "Buildings/Logistics/Splitters/Splitter_Idle_Down", BuildType.Splitter, 32);

            panel.AddChild(list);


            var removeButton = ui.CreateTextButton("Remove", 0, 0, 40, 40);
            removeButton.RelativeHeight = 0.075f;
            removeButton.RelativeWidth = 0.5f;
            removeButton.RelativeX = 0f;
            removeButton.RelativeY = 0.85f;

            removeButton.OnClicked += () =>
            {
                if(activeButton != null)
                {
                    activeButton.IsToggle = false;
                    activeButton = null;
                }

                OnRemoveRequested?.Invoke();
            };

            panel.AddChild(removeButton);

            panel.RecalculateLayout();
            handleButton.RecalculateLayout();

            isOpen = false;
            openX = 0f;
            closedX = (float) - panel.RelativeWidth;
        }

        private void AddBuildButton(UIFactory ui, ScrollMultiList list, string label, BuildType type, int size)
        {
            var btn = ui.CreateImageButtonFrame(label, new Rect(0, 0, size, size));
            btn.OnClicked += () =>
            {
                if(activeButton == btn)
                {
                    btn.IsToggle = false;
                    activeButton = null;
                    OnResetRequested?.Invoke();
                    return;
                }

                if(activeButton != null)
                    activeButton.IsToggle = false;

                btn.IsToggle = true;
                activeButton = btn;

                OnBuildRequested?.Invoke(type);
            };
            list.Add(btn);
        }

        private void Toggle()
        {
            isOpen = !isOpen;
            targetX = isOpen ? 0 : (float) -panel.RelativeWidth;
        }

        public override bool Update(MouseInput mouse)
        {
            base.Update(mouse);

            panel.RelativeX = MathHelper.Lerp((float)panel.RelativeX, isOpen ? openX : closedX, 0.2f);
            panel.RecalculateLayout();

            var rect = panel.Rect;
            handleButton.X = rect.X + rect.Width; // パネル右側に絶対座標で配置
            handleButton.Y = rect.Y + (rect.Height - handleButton.Height) / 2;

            bool consumed = false;
            consumed |= handleButton.Update(mouse);
            consumed |= panel.Update(mouse);
            return consumed;
        }

        public override void Draw(SpriteBatch sb)
        {
            panel.Draw(sb);
            handleButton.Draw(sb);
        }

        public void ClearActiveButton()
        {
            if(activeButton != null)
            {
                activeButton.IsToggle = false;
                activeButton = null;
            }
        }
    }
}
