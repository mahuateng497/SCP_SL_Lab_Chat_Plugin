using System;
using PlayerRoles;

namespace CxChat;

public class ChatMessage
{
	public string Content { get; }

	public DateTime TimeSent { get; }

	public Team? TargetTeam { get; }

	public bool AdminOnly { get; }

	public float Duration { get; }

	public ChatMessage(string content, DateTime timeSent, Team? targetTeam, bool adminOnly, float duration)
	{
		Content = content;
		TimeSent = timeSent;
		TargetTeam = targetTeam;
		AdminOnly = adminOnly;
		Duration = duration;
	}

	public bool IsVisible(Team team, bool ra)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		if (AdminOnly && !ra)
		{
			return false;
		}
		if (TargetTeam.HasValue && TargetTeam != (Team?)team)
		{
			return false;
		}
		return true;
	}
}
