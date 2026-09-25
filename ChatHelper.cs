using System.Linq;
using System.Reflection;
using CommandSystem;
using LabApi.Features.Wrappers;
using PlayerRoles;

namespace CxChat;

public static class ChatHelper
{
	public static string Sanitize(string s)
	{
		if (string.IsNullOrEmpty(s))
		{
			return s;
		}
		return s.Replace("<", "＜").Replace(">", "＞");
	}

	public static string BuildLine(Player player, string msg, string channelName, string channelColor, bool showIdentity)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		if (showIdentity)
		{
			string roleColor = GetRoleColor(player.Role);
			string roleDisplayName = GetRoleDisplayName(player.Role);
			return "[" + player.Nickname + "][<color=" + roleColor + ">" + roleDisplayName + "</color>]<color=" + channelColor + "><b>[" + channelName + "]</b></color>: " + msg;
		}
		return "<color=" + channelColor + "><b>[" + channelName + "]</b></color>: " + msg;
	}

	public static Player TryGetPlayer(ICommandSender sender)
	{
		if (sender == null)
		{
			return null;
		}
		Player val = (Player)(object)((sender is Player) ? sender : null);
		if (val != null)
		{
			return val;
		}
		try
		{
			PropertyInfo property = ((object)sender).GetType().GetProperty("Player", BindingFlags.Instance | BindingFlags.Public);
			if (property != null && property.PropertyType == typeof(Player))
			{
				object value = property.GetValue(sender);
				Player val2 = (Player)((value is Player) ? value : null);
				if (val2 != null)
				{
					return val2;
				}
			}
			FieldInfo field = ((object)sender).GetType().GetField("Player", BindingFlags.Instance | BindingFlags.Public);
			if (field != null && field.FieldType == typeof(Player))
			{
				object value2 = field.GetValue(sender);
				Player val3 = (Player)((value2 is Player) ? value2 : null);
				if (val3 != null)
				{
					return val3;
				}
			}
		}
		catch
		{
		}
		try
		{
			string id = null;
			PropertyInfo propertyInfo = ((object)sender).GetType().GetProperty("UserId") ?? ((object)sender).GetType().GetProperty("SenderId") ?? ((object)sender).GetType().GetProperty("Id");
			if (propertyInfo != null)
			{
				id = propertyInfo.GetValue(sender)?.ToString();
			}
			if (!string.IsNullOrEmpty(id))
			{
				Player val4 = Player.List.FirstOrDefault((Player x) => x != null && x.UserId == id);
				if (val4 != null)
				{
					return val4;
				}
			}
			string nick = null;
			PropertyInfo propertyInfo2 = ((object)sender).GetType().GetProperty("Nickname") ?? ((object)sender).GetType().GetProperty("LogName") ?? ((object)sender).GetType().GetProperty("Name");
			if (propertyInfo2 != null)
			{
				nick = propertyInfo2.GetValue(sender)?.ToString();
			}
			if (!string.IsNullOrEmpty(nick))
			{
				Player val5 = Player.List.FirstOrDefault((Player x) => x != null && x.Nickname == nick);
				if (val5 != null)
				{
					return val5;
				}
			}
		}
		catch
		{
		}
		return null;
	}

	public static string GetRoleColor(RoleTypeId role)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected I4, but got Unknown
		switch ((int)role)
		{
		case 1:
			return "#FFCC00";
		case 6:
			return "#6FC3FF";
		case 15:
			return "#B0C4DE";
		case 13:
			return "#0096FF";
		case 11:
			return "#6FC3FF";
		case 4:
			return "#00E5FF";
		case 12:
			return "#003ECA";
		case 8:
			return "#7FFFAA";
		case 18:
			return "#32CD32";
		case 20:
			return "#98FB98";
		case 19:
			return "#006400";
		case 0:
		case 3:
		case 5:
		case 7:
		case 9:
		case 10:
		case 16:
		case 23:
			return "#EC2121";
		case 14:
			return "#999999";
		default:
			return "#999999";
		}
	}

	public static string GetRoleDisplayName(RoleTypeId role)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected I4, but got Unknown
		return ((int)role - -1) switch
		{
			0 => "观察者", 
			2 => "D级人员", 
			7 => "科学家", 
			16 => "设施警卫", 
			14 => "机动特遣队-列兵", 
			12 => "机动特遣队-中士", 
			5 => "机动特遣队-收容专家", 
			13 => "机动特遣队-指挥官", 
			9 => "混沌分裂者-征召兵", 
			19 => "混沌分裂者-步枪兵", 
			21 => "混沌分裂者-压制者", 
			20 => "混沌分裂者-掠夺者", 
			6 => "SCP-049", 
			11 => "SCP-049-2", 
			8 => "SCP-079", 
			10 => "SCP-096", 
			4 => "SCP-106", 
			1 => "SCP-173", 
			17 => "SCP-939", 
			24 => "SCP-3114", 
			15 => "教程人员", 
			3 => "观察者", 
			22 => "监督者", 
			23 => "导演模式", 
			_ => role.ToString(), 
		};
	}
}
