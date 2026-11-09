# タイトル

2026-10-28 ~ 2026-10-28

## 追加内容

ConveyorSplitterの実装。Splitterは一方向からの入力を受けてラウンドロビンを使って均等に出力する建物でConveyorを継承して
実装される。一方向が詰まったときにには正しく他の二方向に等しく1対1配分できるように実装されなければいけない。

## 実装方針

BuildingInstanceに標準で実装されているTryOutputFair(item)を利用してSplitterを実装する。TryOutputFairについてboolを返り値に
するのではなくBuildingPortを返すようにし、細かい部分を修正する。

## 進行中メモ

BuildingInstanceの標準で実装されているTryOutputFair()だけでは情報の取得できる関数として不足していたので、PeekOutputPort()を
実装とPeekOutputIndex()を実装した。他にもBuildingInstanceの派生先でOutputIndexを利用した処理ができるように
AdvanceOutputIndexを空実装した。SplitterではAdvanceOutputIndexをOverrideしてOutputIndexを進める実装をした。

## 次回作業用メモ

次回用メモ

# タイトル

2026-11-04 ~ 2026-11-

## 追加内容

ゲームにアイテムを加工する圧縮機を追加する。

## 実装方針

新しく圧縮機クラスをつくって、始めて実装される生産設備のためレシピRegistryを作り、圧縮機クラスは対応するレシピを参照して
実際にアイテムを変換する。

## 進行中メモ

アニメーションを作っている内に今のDrillでは採掘ができなかったり排出が出来ない場合のアニメーションの停止がされていないことに
気付き、また今後の拡張を考えるとそれぞれの建物の状態に応じてアニメーションが必要になることがわかったため、
BuildingInstanceにBuildingVisualStateを作った。UpdateVisualとDrawもそれに対応するように変更した。
VisualStateのためのIsWorkiing,IsBlocked, IsStarting, IsStopping, IsIdleをBuildingInstanceに追加した。
BuildingInstanceの派生先でそれぞれの状態に応じてoverrideする。
描画の点滅を防ぐために、IsReallyBlockedを追加した。
ドリルのアニメーションは正しく停止するようになったが、コンベアの二つアイテムがあり移動しなくった時のアニメーションの停止
がうまく動作しない。
原因はConveyor側でUpdaateBlockedTime()を呼んでいないことでIsReallyBlockedが正しく更新されていないことだった。
継承元のBuildingInstanceでUpdateBlockedTime()を呼ぶようにして、継承先でBaseを呼ぶようにした。

## 次回作業用メモ

BuildingInstanceのUpdateVisuallとDrawの実装の確認から始める。