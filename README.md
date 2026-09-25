# CxChat 聊天插件（v4）by 真奈美 

SCP:SL 服务器的聊天系统插件，基于 LabAPI 与 HintServiceMeow 实现。采用「预创建提示槽位 + 只改属性」的渲染架构（TextChatMeow 风格），稳定且对性能开销小。

## 功能特性

- 四个聊天频道：全体广播 / 团队聊天 / 管理员求助 / 管理频道
- 消息以 HUD 提示形式显示在屏幕左侧，每条带**剩余秒数倒计时**（默认 12 秒）
- 同一条消息同时推送到玩家**左下角控制台**（通过反射调用）
- 内容防注入：消息中的 `<` `>` 自动替换为全角 `＜` `＞`
- 单条消息长度限制 120 字
- 角色按阵营自动配色（D 级、SCP、MTF、混沌等），团队消息昵称带角色颜色
- 管理频道显示权限组中文身份名（服主 / 管理员 / 监督员等，可自定义映射）
- 玩家进出自动创建 / 清理显示槽位，无残留

## 命令用法

所有命令在游戏内聊天框输入（客户端命令），均支持 RA 面板使用。

| 命令 | 权限 | 作用 | 可见范围 |
|---|---|---|---|
| `.bc 内容` | 全部玩家 | 全体广播 | 所有玩家 |
| `.c 内容` | 全部玩家 | 团队聊天 | 仅同阵营玩家（RA 可见全部） |
| `.ac 内容` | 全部玩家 | 管理员求助 | 所有在线 RA（屏幕顶部红色 ⚠ 通知 10 秒 + 服务器日志可以查询） |
| `.cc 内容` | 仅 RA | 管理频道 | 仅 RA 玩家，频道标签显示中文身份名 |

使用示例：

```
.bc 服务器将于 20:00 重启，请做好准备
.c 有人看到 096 吗
.ac 设施内发现作弊玩家
.cc 注意 914 房间有人刷道具
```

## 配置说明

首次加载自动生成 `CxChat` 的 JSON 配置文件，编辑后重启服务器生效。

| 配置项 | 默认值 | 说明 |
|---|---|---|
| `IsEnabled` | `true` | 插件总开关 |
| `Debug` | `false` | 调试日志开关 |
| `StartYCoordinate` | `60` | 聊天区域起始 Y 坐标（距屏幕顶部） |
| `LineHeight` | `30` | 每行消息的高度间距 |
| `ChatFontSize` | `26` | 聊天字体大小 |
| `MaxMessageCount` | `6` | 屏幕同时显示的最大消息行数 |
| `HintDuration` | `12` | 单条消息持续秒数（显示倒计时） |
| `RefreshInterval` | `1` | 渲染刷新间隔（秒） |
| `AdminGroupNames` | 见下方 | 权限组英文名 → 中文身份名映射，用于 `.cc` 频道标签 |

`AdminGroupNames` 默认映射（可在配置中增删改）：

```
owner → 服主          founder → 创始者      admin → 管理员
administrator → 管理员 high → 高级管理员     headadmin → 首席管理
moderator → 监督员     junior → 初级管理     helper → 帮助员
support → 客服         trial → 试用管理      vip → VIP
svip → SVIP           donator → 赞助者      youtuber → 主播
developer → 开发者     builder → 建筑师      event → 活动管理
staff → 工作人员       trusted → 信任玩家    user/default → 玩家
guest → 访客          banned → 已封禁
```

## 安装

1. 将 `CxChat.dll` 放入服务器插件目录：
   - Windows：`%APPDATA%\SCP Secret Laboratory\PluginAPI\plugins\<端口>\`
   - Linux 专服：`~/.config/SCP Secret Laboratory/PluginAPI/plugins/<端口>/`
2. 重启服务器，控制台出现 `========== CxChat v4 启用 ==========` 即加载成功。
3. 如需卸载，直接删除 DLL 并重启即可，不会残留任何数据。

## 依赖

- LabAPI ≥ 1.1.7（`RequiredApiVersion`）
- HintServiceMeow（聊天消息 HUD 渲染）
- MEC 协程库（渲染循环）

## 从源码编译

```bash
dotnet build -c Release src/CxChat.csproj
```

编译依赖的游戏程序集（`LabApi.dll`、`Assembly-CSharp.dll`、`HintServiceMeow.dll`、`Mirror.dll`、`UnityEngine.CoreModule.dll` 等）需从你的服务器 / 游戏安装目录获取，放入 `lib/` 目录。
