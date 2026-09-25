using System;
using System.Collections.Generic;
using LabApi.Loader.Features.Plugins;

namespace CxChat;

public static class MessagesList
{
	public static readonly LinkedList<ChatMessage> Messages = new LinkedList<ChatMessage>();

	public static void Add(ChatMessage m)
	{
		Messages.AddFirst(m);
		TrimExpired();
		while (Messages.Count > (((Plugin<PluginConfig>)CxChatPlugin.Instance)?.Config.MaxMessageCount ?? 6))
		{
			Messages.RemoveLast();
		}
	}

	public static void TrimExpired()
	{
		try
		{
			while (Messages.Last != null && (DateTime.Now - Messages.Last.Value.TimeSent).TotalSeconds > (double)Messages.Last.Value.Duration)
			{
				Messages.RemoveLast();
			}
		}
		catch
		{
		}
	}

	public static void Clear()
	{
		Messages.Clear();
	}
}
