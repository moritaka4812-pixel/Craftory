using Craftory.Input;
using Craftory.UI.Core;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = Microsoft.Xna.Framework.Color;
using Rect = Microsoft.Xna.Framework.Rectangle;

namespace Craftory.GameUI
{
    public class ConfirmPanel : UIElement
    {
        private List<UIElement> children = new List<UIElement>();
        private Color background = Color.Gray;

        public ConfirmPanel(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        public void AddChild(UIElement child)
        {
            child.Parent = this;
            children.Add(child);
        }

        public override bool Update(MouseInput mouse)
        {
            bool consumed = false;

            // パネル背景の判定
            if (HitTest(mouse.Current.Position) && mouse.LeftClicked())
                consumed = true;

            // 子要素の更新
            foreach (var child in children)
                consumed |= child.Update(mouse);

            return consumed;
        }

        public override void Draw(SpriteBatch sb)
        {
            var abs = GetAbsolutePosition();
            sb.Draw(whiteTex, new Rect(abs.X, abs.Y, Width, Height), background);

            foreach (var child in children)
                child.Draw(sb);
        }
    }

}
