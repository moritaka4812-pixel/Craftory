# Conveyor Architecture

## 主要コンポーネント

### Conveyor系統
- アイテム搬送を担当

### ~~DirectionResolver~~
- ~~方向管理を担当~~ Registryに方向情報を持たせることで責務重複のため削除

### BuildingPort
- 建物の入力出力を管理する

### BuildingRegistry
- 建物の方向の静的情報を管理する

### BuildingPortFactory
- staticでBuildingInstanceの生成時にBuildingRegistryの情報からインスタンスのBuildingPortを生成する

### ConnectionManager
- 接続管理を担当

## 方針
コンベア系の~~コードから方向管理をDirectionResolverに~~、接続管理を、BuildingPort、ConnectionManagerに分離する。