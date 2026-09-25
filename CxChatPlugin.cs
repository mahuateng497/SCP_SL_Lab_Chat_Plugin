using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Handlers;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;
using LabApi.Loader.Features.Plugins;
using MEC;
using PlayerRoles;
using RemoteAdmin;

namespace CxChat;

public class CxChatPlugin : Plugin<PluginConfig>
{
	[CompilerGenerated]
	private sealed class _003CRenderLoop_003Ed__28 : IEnumerator<float>, IDisposable, IEnumerator
	{
		private int _003C_003E1__state;

		private float _003C_003E2__current;

		public CxChatPlugin _003C_003E4__this;

		float IEnumerator<float>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CRenderLoop_003Ed__28(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			CxChatPlugin cxChatPlugin = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			if (!cxChatPlugin._disabled)
			{
				try
				{
					MessagesList.TrimExpired();
					DisplayManager.UpdateAll();
				}
				catch (Exception ex)
				{
					Logger.Error((object)("[CxChat] 渲染异常: " + ex.Message));
				}
				_003C_003E2__current = Timing.WaitForSeconds(1f);
				_003C_003E1__state = 1;
				return true;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	private CoroutineHandle _loop;

	private bool _disabled;

	public static CxChatPlugin Instance { get; private set; }

	public override string Name { get; } = "赛尔号纯净插件（服务器已关闭）";


	public override string Description { get; } = "";


	public override string Author { get; } = "真奈美 qq 2444646142 倒卖死冯";


	public override Version RequiredApiVersion { get; } = new Version(1, 1, 7);
	public override bool IsTransparent => true;


	public override void Enable()
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		_disabled = false;
		Instance = this;
		PlayerEvents.Joined += OnPlayerJoined;
		PlayerEvents.Left += OnPlayerLeft;
		_loop = Timing.RunCoroutine(RenderLoop());
		Logger.Info((object)"========== CxChat v4 启用） ==========");
		Logger.Info((object)"命令：.bc 全体 / .c 团队 / .ac 求助 / .cc 管理频道");
	}

	public override void Disable()
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		_disabled = true;
		PlayerEvents.Joined -= OnPlayerJoined;
		PlayerEvents.Left -= OnPlayerLeft;
		try
		{
			if (_loop.IsRunning)
			{
				Timing.KillCoroutines((CoroutineHandle[])(object)new CoroutineHandle[1] { _loop });
			}
		}
		catch
		{
		}
		try
		{
			MessagesList.Clear();
			DisplayManager.ClearAll();
		}
		catch
		{
		}
		Instance = null;
		Logger.Info((object)"========== CxChat v4 已禁用 ==========");
	}

	private void OnPlayerJoined(PlayerJoinedEventArgs ev)
	{
		try
		{
			if (!_disabled && ev.Player != null)
			{
				new DisplayManager(ev.Player);
			}
		}
		catch
		{
		}
	}

	private void OnPlayerLeft(PlayerLeftEventArgs ev)
	{
		try
		{
			if (ev.Player != null)
			{
				DisplayManager.Remove(ev.Player);
			}
		}
		catch
		{
		}
	}

	public void Enqueue(string content, string consoleText, Team? targetTeam = null, bool adminOnly = false)
	{
		try
		{
			if (!_disabled && Instance != null)
			{
				MessagesList.Add(new ChatMessage(content, DateTime.Now, targetTeam, adminOnly, base.Config.HintDuration));
				DisplayManager.UpdateAll();
				SendConsoleToEligible(consoleText, targetTeam, adminOnly);
			}
		}
		catch
		{
		}
	}

	private void SendConsoleToEligible(string text, Team? targetTeam, bool adminOnly)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			foreach (Player item in Player.List.ToList())
			{
				try
				{
					if (item != null && !(item.ReferenceHub == (ReferenceHub)null))
					{
						Team team = item.ReferenceHub.roleManager.CurrentRole.Team;
						bool remoteAdmin = item.ReferenceHub.serverRoles.RemoteAdmin;
						if ((!adminOnly || remoteAdmin) && (!targetTeam.HasValue || targetTeam == (Team?)team || remoteAdmin))
						{
							SendConsoleToPlayer(item, text);
						}
					}
				}
				catch
				{
				}
			}
		}
		catch
		{
		}
	}

	private static void SendConsoleToPlayer(Player player, string text, string color = "white")
	{
		try
		{
			QueryProcessor queryProcessor = player.ReferenceHub.queryProcessor;
			FieldInfo[] fields = ((object)queryProcessor).GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			for (int i = 0; i < fields.Length; i++)
			{
				object value = fields[i].GetValue(queryProcessor);
				if (value == null || !value.GetType().Name.Contains("CommandSender"))
				{
					continue;
				}
				MethodInfo method = value.GetType().GetMethod("Print", BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
				if (method != null)
				{
					if (method.GetParameters().Length >= 2)
					{
						method.Invoke(value, new object[2] { text, color });
					}
					else
					{
						method.Invoke(value, new object[1] { text });
					}
				}
				break;
			}
		}
		catch
		{
		}
	}

	public void ClearAll()
	{
		try
		{
			MessagesList.Clear();
		}
		catch
		{
		}
		DisplayManager.ClearAll();
		Logger.Info((object)"[CxChat] 聊天已清空");
	}

	public void SendAdminNotify(string fullMsg)
	{
		int num = DisplayManager.SetAdminNotify(fullMsg);
		Logger.Info((object)$"[CxChat] 求助通知已发送给 {num} 个RA玩家: {fullMsg}");
	}

	public string GetIdentityName(ReferenceHub hub)
	{
		try
		{
			string text = hub.serverRoles.Group?.Name ?? "unknown";
			if (base.Config.AdminGroupNames != null && base.Config.AdminGroupNames.TryGetValue(text.ToLower(), out var value))
			{
				return value;
			}
			return text;
		}
		catch
		{
			return "unknown";
		}
	}

	[IteratorStateMachine(typeof(_003CRenderLoop_003Ed__28))]
	private IEnumerator<float> RenderLoop()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CRenderLoop_003Ed__28(0)
		{
			_003C_003E4__this = this
		};
	}
}
