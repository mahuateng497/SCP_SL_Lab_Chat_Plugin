using System;
using System.Collections.Generic;
using System.Linq;
using HintServiceMeow.Core.Enum;
using HintServiceMeow.Core.Models.Hints;
using HintServiceMeow.Core.Utilities;
using LabApi.Features.Wrappers;
using LabApi.Loader.Features.Plugins;

namespace CxChat;

public class DisplayManager
{
	private static readonly List<DisplayManager> Managers = new List<DisplayManager>();

	private static readonly object Sync = new object();

	private readonly Player _player;

	private readonly Hint[] _slots;

	private readonly Hint _adminNotify;

	private string _adminNotifyText;

	private DateTime _adminNotifyUntil;

	public DisplayManager(Player player)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Expected O, but got Unknown
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		_player = player;
		PluginConfig pluginConfig = ((Plugin<PluginConfig>)CxChatPlugin.Instance)?.Config;
		int num = pluginConfig?.MaxMessageCount ?? 6;
		PlayerDisplay val = PlayerDisplay.Get(player);
		_slots = (Hint[])(object)new Hint[num];
		for (int i = 0; i < num; i++)
		{
			_slots[i] = new Hint
			{
				Id = $"CxChatLine_{i}",
				Text = string.Empty,
				FontSize = (pluginConfig?.ChatFontSize ?? 26),
				Alignment = (HintAlignment)0,
				YCoordinateAlign = (HintVerticalAlign)0,
				YCoordinate = (pluginConfig?.StartYCoordinate ?? 60f) + (float)i * (pluginConfig?.LineHeight ?? 30f),
				SyncSpeed = (HintSyncSpeed)160,
				Hide = true
			};
			val.AddHint((AbstractHint)(object)_slots[i]);
		}
		_adminNotify = new Hint
		{
			Id = "CxAdminNotify",
			Text = string.Empty,
			FontSize = 40,
			Alignment = (HintAlignment)2,
			YCoordinateAlign = (HintVerticalAlign)1,
			YCoordinate = 150f,
			SyncSpeed = (HintSyncSpeed)160,
			Hide = true
		};
		val.AddHint((AbstractHint)(object)_adminNotify);
		lock (Sync)
		{
			Managers.Add(this);
		}
	}

	public static void UpdateAll()
	{
		List<DisplayManager> list;
		lock (Sync)
		{
			list = Managers.ToList();
		}
		foreach (DisplayManager item in list)
		{
			try
			{
				item.Update();
			}
			catch
			{
			}
		}
	}

	public static void ClearAll()
	{
		List<DisplayManager> list;
		lock (Sync)
		{
			list = Managers.ToList();
		}
		foreach (DisplayManager item in list)
		{
			try
			{
				Hint[] slots = item._slots;
				for (int i = 0; i < slots.Length; i++)
				{
					((AbstractHint)slots[i]).Hide = true;
				}
				((AbstractHint)item._adminNotify).Hide = true;
			}
			catch
			{
			}
		}
	}

	public static int SetAdminNotify(string text)
	{
		int num = 0;
		DateTime now = DateTime.Now;
		List<DisplayManager> list;
		lock (Sync)
		{
			list = Managers.ToList();
		}
		foreach (DisplayManager item in list)
		{
			try
			{
				if (item._player != null && !(item._player.ReferenceHub == (ReferenceHub)null) && item._player.ReferenceHub.serverRoles.RemoteAdmin)
				{
					item._adminNotifyText = text;
					item._adminNotifyUntil = now.AddSeconds(10.0);
					item.Update();
					num++;
				}
			}
			catch
			{
			}
		}
		return num;
	}

	public static void Remove(Player player)
	{
		List<DisplayManager> list;
		lock (Sync)
		{
			list = Managers.Where((DisplayManager x) => x._player == player).ToList();
			foreach (DisplayManager item in list)
			{
				Managers.Remove(item);
			}
		}
		foreach (DisplayManager item2 in list)
		{
			try
			{
				PlayerDisplay val = PlayerDisplay.Get(player);
				if (val != null)
				{
					Hint[] slots = item2._slots;
					foreach (Hint val2 in slots)
					{
						val.RemoveHint((AbstractHint)(object)val2);
					}
					val.RemoveHint((AbstractHint)(object)item2._adminNotify);
				}
			}
			catch
			{
			}
		}
	}

	public void Update()
	{
		if (_player == null || _player.ReferenceHub == (ReferenceHub)null)
		{
			return;
		}
		DateTime now = DateTime.Now;
		List<ChatMessage> list = MessagesList.Messages.Where((ChatMessage m) => m.IsVisible(_player.ReferenceHub.roleManager.CurrentRole.Team, _player.ReferenceHub.serverRoles.RemoteAdmin)).Take(_slots.Length).ToList();
		bool flag = _adminNotifyText != null && now < _adminNotifyUntil;
		for (int i = 0; i < _slots.Length; i++)
		{
			if (i < list.Count)
			{
				ChatMessage chatMessage = list[i];
				int num = Math.Max(0, (int)((double)chatMessage.Duration - (now - chatMessage.TimeSent).TotalSeconds));
				((AbstractHint)_slots[i]).Text = $"<color=#D2D4C1>[{num}s]</color> {chatMessage.Content}";
				((AbstractHint)_slots[i]).Hide = false;
			}
			else
			{
				((AbstractHint)_slots[i]).Hide = true;
			}
		}
		if (flag)
		{
			((AbstractHint)_adminNotify).Text = "<size=40><color=red><b>⚠ " + _adminNotifyText + " ⚠</b></color></size>";
			((AbstractHint)_adminNotify).Hide = false;
		}
		else
		{
			((AbstractHint)_adminNotify).Hide = true;
			_adminNotifyText = null;
		}
	}
}
