using System;
using CommandSystem;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;

namespace CxChat;

[CommandHandler(typeof(ClientCommandHandler))]
[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class CmdStaff : ICommand
{
	public string Command => "cc";

	public string[] Aliases => Array.Empty<string>();

	public string Description => ".cc 内容 → 管理员频道（仅RA）";

	public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
	{
		response = string.Empty;
		try
		{
			Player val = ChatHelper.TryGetPlayer(sender);
			if (val == null || val.ReferenceHub == (ReferenceHub)null)
			{
				response = "未经过中心服务器验证";
				return false;
			}
			if (!val.ReferenceHub.serverRoles.RemoteAdmin)
			{
				response = "你没有权限使用.cc频道（需要RA面板权限）";
				return false;
			}
			if (args.Count == 0)
			{
				response = "您没有输入聊天内容!";
				return false;
			}
			string text = ChatHelper.Sanitize(string.Join(" ", args));
			if (text.Length > 120)
			{
				response = "聊天内容过长!(最多120字)";
				return false;
			}
			string text2 = CxChatPlugin.Instance?.GetIdentityName(val.ReferenceHub) ?? "unknown";
			string content = val.Nickname + "<color=#EC2121><b>[管理频道|" + text2 + "]</b></color>: " + text;
			string consoleText = ".cc " + val.Nickname + ": " + text;
			CxChatPlugin.Instance?.Enqueue(content, consoleText, null, adminOnly: true);
			Logger.Info((object)("[CxChat] .cc " + val.Nickname + "(" + text2 + "): " + text));
			response = "已发送管理员频道!";
			return true;
		}
		catch (Exception ex)
		{
			response = "执行失败: " + ex.Message;
			return false;
		}
	}
}
