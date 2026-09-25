using System;
using CommandSystem;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;

namespace CxChat;

[CommandHandler(typeof(ClientCommandHandler))]
[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class CmdPub : ICommand
{
	public string Command => "bc";

	public string[] Aliases => Array.Empty<string>();

	public string Description => ".bc 内容 → 全体广播";

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
			string content = "<color=#FFFF7C><b>[全体]</b></color>" + val.Nickname + ": " + text;
			string consoleText = ".bc " + val.Nickname + ": " + text;
			CxChatPlugin.Instance?.Enqueue(content, consoleText);
			Logger.Info((object)("[俩天插件] .bc " + val.Nickname + ": " + text));
			response = "发送成功!";
			return true;
		}
		catch (Exception ex)
		{
			response = "执行失败: " + ex.Message;
			return false;
		}
	}
}
