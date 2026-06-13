
using Microsoft.VisualBasic.Devices;
using ResourceMiningGame.Input;

namespace ResourceMiningGame.UI
{
    public class ScrollList : UIContainer
    {
        public ScrollBar? ScrollBar { get; set; } //対応するスクロールバー
        public List<Action>? Callbacks { get; set; } //各要素が押されたときの処理のまとまり
        public int ItemSpacing { get; set; }

        private int contentHeight; //表示できるコンテンツ全体の高さ
        private int scrollOffset; //どの高さまでスクロールしたか

        private RasterizerState scissorRaster;
        private RasterizerState defaultRaster;

        public ScrollList(int Spacing) : base()
        {
            scissorRaster = new RasterizerState() { ScissorTestEnable = true };
            defaultRaster = new RasterizerState() { ScissorTestEnable = false};
            this.ItemSpacing = Spacing;
        }

        public void SetScrollBar(ScrollBar scroll)
        {
            this.ScrollBar = scroll;
        }

        public void Add(UIElement child, Action callback)
        {
            child.Parent = this;
            Children.Add(child);

            if (Callbacks == null) Callbacks = new List<Action>();
            Callbacks.Add(callback);
        }

        public override bool Update(MouseInput mouse)
        {
            if (!Visible) return false;
            //スクロールリスト内の座標に補正
            var localMouse = new MouseInput();

            LayoutChildrenVertically(); //縦方向の配置
            UpdateContentHeight(); //スクロールバーのContentHeightを更新
            UpdateScrollOffset(mouse); //スクロールoffsetの更新
            SyncScrollBar(); //スクロールバーと状態を同期
            ApplyScrollOffset(); //子UIの位置にScrollOffsetを適用
            ClickCheck(mouse);//子要素のクリック判定をスクロール分補正

            return false;
        }

        public override void Draw(SpriteBatch sb)
        {
            BeginClip(sb); //クリップ描画開始
            
            foreach(var child in Children)
            {
                var oldRect = child.Rect;
                child.X += rect.X;
                child.Y += rect.Y;

                child.Draw(sb);

                child.X = oldRect.X;
                child.Y = oldRect.Y;
            }
            EndClip(sb);  //クリップ描画終了
        }

        private void LayoutChildrenVertically()
        {
            int x = 0; //ScrollListのローカル座標
            int y = 0;

            foreach(var child in Children) //各要素のY座標をループで積み上げるようなイメージ
            {
                child.X = x;
                child.Y = y;
                y += child.Height + ItemSpacing;
            }

            contentHeight = y;
        }

        private void UpdateContentHeight()
        {
            //ScrollBarがある場合に更新
            if (ScrollBar != null) ScrollBar.SetContentHeight(contentHeight, rect.Height);
        }

        private void UpdateScrollOffset(MouseInput mouse)
        {
            //マウスホイールの変化量
            int wheelDelta = mouse.ScrollDelta();

            //スクロール量を更新（ホイールは逆方向）
            scrollOffset -= (int)(wheelDelta * 0.1f);

            //範囲外にいかないように制限
            scrollOffset = Math.Clamp(scrollOffset, 0, Math.Max(0, contentHeight - rect.Height));
        }

        private void SyncScrollBar() //ScrollBarと同期
        {
            if (ScrollBar != null) ScrollBar.Update(scrollOffset, rect.Height);
        }

        private void ApplyScrollOffset() //子UIの位置にOffsetを適用
        {
            int y = - scrollOffset; //下スクロールで上に移動するためscrollOffsetはマイナス

            foreach(var child in Children)
            {
                child.Y = y;
                y += child.Height + ItemSpacing;
            }
        }

        private void ClickCheck(MouseInput mouse) //クリックがどのボタンについて画面上でされたか
        {
            for (int i = 0; i < Children.Count; i++)
            {
                var child = Children[i];
                bool clicked = child.UpdateWithOffset(rect.X, rect.Y, mouse); //ScrollListのX,Yだけ補正

                if (clicked && Callbacks != null && i < Callbacks.Count)
                {
                    Callbacks[i]?.Invoke();
                }

            }
        }
        
        private void BeginClip(SpriteBatch sb)
        {
            sb.End(); //一度設定を変えるために終了
            sb.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                DepthStencilState.None,
                scissorRaster,
                null,
                Matrix.Identity
                );

            sb.GraphicsDevice.ScissorRectangle = rect; //描画範囲をUIElementの大きさに指定
        }

        private void EndClip(SpriteBatch sb)
        {
            sb.End();

            sb.Begin( //改めてもとの設定に変更
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                DepthStencilState.None,
                defaultRaster,
                null,
                Matrix.Identity
                );
        }
    }
}
