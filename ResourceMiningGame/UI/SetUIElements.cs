
namespace ResourceMiningGame.UI
{
    public class SetUIElements //UIをウィンドウサイズで再配置するクラス
    {
        private List<UIElement> elements = new(); //UIエレメントを格納する

        public void Add(UIElement element) //画面のUIを追加するメソッド
        {
            elements.Add(element);
        }

        public void UpdateLayout(int screenW, int screenH) //各エレメントをアンカーに合わせて再配置
        {
            foreach(var e in elements)
            {
                var pos = UILayoutManager.GetPositionForAnchor(
                    e.Anchor,
                    screenW, screenH,
                    e.Width, e.Height,
                    paddingX: e.PaddingX,
                    paddingY: e.PaddingY
                    );

                e.X = (int)pos.X;
                e.Y = (int)pos.Y;
            }
        }
    }
}
