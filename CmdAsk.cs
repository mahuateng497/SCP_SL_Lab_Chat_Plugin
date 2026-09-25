using System;
using CommandSystem;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;

namespace CxChat;

[CommandHandler(typeof(ClientCommandHandler))]
[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class CmdAsk : ICommand
{
	public string Command => "ac";

	public string[] Aliases => Array.Empty<string>();

	public string Description => ".ac 内容 → 管理员求助";

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
			CxChatPlugin.Instance?.SendAdminNotify("求助: " + val.Nickname + ": " + text);
			Logger.Info((object)("[聊天插件] .ac " + val.Nickname + ": " + text));
			response = "已发送管理员求助!";
			return true;
		}
		catch (Exception ex)
		{
			response = "执行失败: " + ex.Message;
			return false;
		}
	}
}
