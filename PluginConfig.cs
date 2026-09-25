using System.Collections.Generic;
using System.ComponentModel;

namespace CxChat;

public class PluginConfig
{
	[Description("插件开关")]
	public bool IsEnabled { get; set; } = true;


	[Description("调试日志")]
	public bool Debug { get; set; }

	[Description("聊天起始Y坐标（距离顶部）")]
	public float StartYCoordinate { get; set; } = 60f;


	[Description("每行消息的高度间距")]
	public float LineHeight { get; set; } = 30f;


	[Description("聊天字体大小")]
	public int ChatFontSize { get; set; } = 26;


	[Description("最大聊天行数")]
	public int MaxMessageCount { get; set; } = 6;


	[Description("消息持续秒数（倒计时）")]
	public float HintDuration { get; set; } = 12f;


	[Description("刷新间隔（秒）")]
	public float RefreshInterval { get; set; } = 1f;


	[Description("权限组→中文身份名映射（用于.staff频道）")]
	public Dictionary<string, string> AdminGroupNames { get; set; } = new Dictionary<string, string>
	{
		{ "owner", "服主" },
		{ "founder", "创始者" },
		{ "admin", "管理员" },
		{ "admin00", "实习管理员" },
		{ "administrator", "管理员" },
		{ "high", "高级管理员" },
		{ "headadmin", "首席管理" },
		{ "head_admin", "首席管理" },
		{ "yladmin", "娱乐管理员" },
		{ "moderator", "监督员" },
		{ "mod", "监督员" },
		{ "junior", "初级管理" },
		{ "helper", "帮助员" },
		{ "support", "客服" },
		{ "trial", "试用管理" },
		{ "vip", "VIP" },
		{ "svip", "SVIP" },
		{ "mvp", "MVP" },
		{ "donator", "赞助者" },
		{ "donor", "赞助者" },
		{ "youtuber", "主播" },
		{ "streamer", "主播" },
		{ "content_creator", "内容创作者" },
		{ "developer", "开发者" },
		{ "dev", "开发者" },
		{ "tech", "技术" },
		{ "builder", "建筑师" },
		{ "event", "活动管理" },
		{ "event_manager", "活动管理" },
		{ "staff", "工作人员" },
		{ "trusted", "信任玩家" },
		{ "user", "玩家" },
		{ "default", "玩家" },
		{ "guest", "访客" },
		{ "banned", "已封禁" }
	};

}
