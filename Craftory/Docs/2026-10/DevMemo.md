# タイトル

yyyy-mm-dd ~ yyyy-mm-dd

## 追加内容

ConveyorSplitterの実装。Splitterは一方向からの入力を受けてラウンドロビンを使って均等に出力する建物でConveyorを継承して
実装される。一方向が詰まったときにには正しく他の二方向に等しく1対1配分できるように実装されなければいけない。

## 実装方針

BuildingInstanceに標準で実装されているTryOutputFair(item)を利用してSplitterを実装する。TryOutputFairについてboolを返り値に
するのではなくBuildingPortを返すようにし、細かい部分を修正する。

## 進行中メモ



## 次回作業用メモ

次回用メモ