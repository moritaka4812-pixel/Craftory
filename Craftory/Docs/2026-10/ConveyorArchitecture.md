# Conveyor Architecture

## 主要コンポーネント

### Conveyor系統
- アイテム搬送を担当

### ~~DirectionResolver~~
- ~~方向管理を担当~~ Registryに方向情報を持たせることで責務重複のため削除
 
### ConnectionManager
- 接続管理を担当

## 方針
コンベア系の~~コードから方向管理をDirectionResolverに~~、接続管理をConnectionManagerに分離する。