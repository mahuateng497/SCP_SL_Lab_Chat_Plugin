using System;
using CommandSystem;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;
using PlayerRoles;

namespace CxChat;

[CommandHandler(typeof(ClientCommandHandler))]
[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class CmdTalk : ICommand
{
	public string Command => "c";

	public string[] Aliases => Array.Empty<string>();

	public string Description => ".c 内容 → 团队聊天（同阵营）";

	public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
	{
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
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
			Team team = val.ReferenceHub.roleManager.CurrentRole.Team;
			string roleColor = ChatHelper.GetRoleColor(val.Role);
			string content = "<color=" + roleColor + ">" + val.Nickname + "</color><color=#FFFF7C><b>[团队]</b></color>: " + text;
			string consoleText = ".c " + val.Nickname + ": " + text;
			CxChatPlugin.Instance?.Enqueue(content, consoleText, team);
			Logger.Info((object)$"[CxChat] .c {val.Nickname} 阵营:{team}: {text}");
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
