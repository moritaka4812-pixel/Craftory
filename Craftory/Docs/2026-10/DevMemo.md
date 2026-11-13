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