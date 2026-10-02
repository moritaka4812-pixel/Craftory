# Conveyor Architecture

## 主要コンポーネント

### Conveyor系統
- アイテム搬送を担当

### DirectionResolver
- 方向管理を担当
 
### ConnectionManager
- 接続管理を担当

## 方針
コンベア系のコードから方向管理をDirectionResolverに、接続管理をConnectionManagerに分離する。