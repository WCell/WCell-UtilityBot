using System;
using Squishy.Irc.Protocol;

namespace Squishy.Irc.Commands
{
	public partial class IrcCommandHandler
	{


		public void Join(string target)
		{
			IrcClient.Send("JOIN " + target);
		}

		public void Join(string target, string key)
		{
			IrcClient.Send("JOIN {0} :{1}", target, key);
		}

		public void Nick(string newNick)
		{
			IrcClient.Send("NICK " + newNick);
		}

		public void Whois(string nick)
		{
			IrcClient.Send("WHOIS " + nick + " " + nick);
		}

		public void WhoisSimple(string nick)
		{
			IrcClient.Send("WHOIS " + nick);
		}

		public void Who(string channel)
		{
			IrcClient.Send("WHO " + channel);
		}

		/// <summary>
		/// Extended Whois resolved auth names of users
		/// </summary>
		public void WhoX(string channel)
		{
			IrcClient.Send("WHO " + channel + " %na");
		}

		public void Part(string chan, string reason)
		{
			IrcClient.Send("PART " + chan + " :" + reason);
		}

		public void Part(string chan, string reason, params object[] args)
		{
			IrcClient.Send("PART " + chan + " :" + String.Format(reason, args));
		}

		public void Part(IrcChannel chan, string reason)
		{
			IrcClient.Send("PART " + chan.Name + " :" + reason);
		}

		public void Part(IrcChannel chan, string reason, params object[] args)
		{
			IrcClient.Send("PART " + chan.Name + " :" + String.Format(reason, args));
		}

		void SendMsg(string cmdPrefix, string splittableArg)
		{
			var lines = splittableArg.Replace("\r\n", "\n").Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
			foreach (string line in lines)
			{
				var len = line.Length;
				var maxLen = IrcProtocol.MaxLineLength - cmdPrefix.Length;
				var offset = 0;
				while (len > maxLen)
				{
					var l = line.Substring(offset, maxLen);
					IrcClient.Send(cmdPrefix + l);
					offset += maxLen;
					len -= maxLen;
				}
				if (len > 0)
				{
					IrcClient.Send(cmdPrefix + line.Substring(offset));
				}
			}
		}

		public void Msg(string target, string str)
		{
			SendMsg("PRIVMSG " + target + " :", str);
		}

		public void Msg(ChatTarget target, object format, params object[] args)
		{
			Msg(target.Identifier, format, args);
		}

		public void Msg(string target, object format, params object[] args)
		{
			Msg(target, String.Format(format.ToString(), args));
		}

		public void Notice(string target, string str)
		{
			SendMsg("NOTICE " + target + " :", str);
		}

		public void Notice(ChatTarget target, string format)
		{
			Notice(target.Identifier, format);
		}

		public void Notice(ChatTarget target, string format, params object[] args)
		{
			Notice(target.Identifier, format, args);
		}

		public void Notice(string target, string format, params object[] args)
		{
			Notice(target, String.Format(format, args));
		}

		public void Describe(ChatTarget Target, string format, params object[] args)
		{
			Describe(Target.Identifier, format, args);
		}

		public void Describe(string target, string str)
		{
			string[] lines = str.Replace("\r\n", "\n").Split('\r', '\n');
			foreach (string line in lines)
				CtcpRequest(target, "ACTION", line);
		}

		public void Describe(string target, string format, params object[] args)
		{
			Describe(target, string.Format(format, args));
		}

		public void SetTopic(string chan, string topic)
		{
			IrcClient.Send("TOPIC " + chan + " :" + topic);
		}

		public void CtcpRequest(string target, string request, string str)
		{
			Msg(target, "{0} {1}", request.ToUpper(), str);
		}

		public void CtcpRequest(string target, string request, string argFormat, params object[] args)
		{
			Msg(target, "{0} {1}", request.ToUpper(), string.Format(argFormat, args));
		}

		public void CtcpReply(string target, string request, string str)
		{
			Notice(target, "{0} {1}", request.ToUpper(), str);
		}

		public void CtcpReply(string target, string request, string argFormat, params object[] args)
		{
			Notice(target, "{0} {1}", request.ToUpper(), string.Format(argFormat, args));
		}

		public void DccRequest(string target, string requestFormat)
		{
			CtcpRequest(target, "DCC", requestFormat);
		}

		public void DccRequest(string target, string requestFormat, params object[] args)
		{
			CtcpRequest(target, "DCC", requestFormat, args);
		}

		public void Mode(string flags)
		{
			IrcClient.Send("MODE " + flags);
		}

		public void Mode(string flags, string Targets)
		{
			IrcClient.Send("MODE " + flags + " " + Targets);
		}

		public void Mode(string Channel, string flags, string Targets)
		{
			IrcClient.Send("MODE " + Channel + " " + flags + " " + Targets);
		}

		public void Mode(IrcChannel Channel, string flags, string Targets)
		{
			IrcClient.Send("MODE " + Channel.Name + " " + flags + " " + Targets);
		}

		public void Mode(string flags, params object[] Targets)
		{
			IrcClient.Send("MODE " + flags + " " + Util.GetWords(Targets, 0));
		}

		public void Mode(string Channel, string flags, params object[] Targets)
		{
			IrcClient.Send("MODE " + Channel + " " + flags + " " + Util.GetWords(Targets, 0));
		}

		public void Mode(IrcChannel Channel, string flags, params object[] Targets)
		{
			IrcClient.Send("MODE " + Channel.Name + " " + flags + " " + Util.GetWords(Targets, 0));
		}

		public void Kick(IrcChannel channel, IrcUser user)
		{
			Kick(channel.Name, user.Nick);
		}

		public void Kick(string channel, string user)
		{
			IrcClient.Send("KICK " + channel + " " + user);
		}

		public void Kick(IrcChannel Channel, IrcUser User, string reason)
		{
			Kick(Channel.Name, User.Nick, reason);
		}

		public void Kick(IrcChannel Channel, IrcUser User, string reasonFormat, params object[] args)
		{
			Kick(Channel.Name, User.Nick, reasonFormat, args);
		}

		public void Kick(string Channel, string User, string reason)
		{
			IrcClient.Send("KICK " + Channel + " " + User + " :" + reason);
		}

		public void Kick(string Channel, string User, string reasonFormat, params object[] args)
		{
			IrcClient.Send("KICK " + Channel + " " + User + " :" + string.Format(reasonFormat, args));
		}

		public void Ban(IrcChannel Channel, params object[] Masks)
		{
			Ban(Channel.Name, Masks);
		}

		public void Ban(string Channel, params object[] Masks)
		{
			if (Masks.Length == 0)
				return;
			string flag = "+";
			for (int i = 0; i < Masks.Length; i++)
				flag += "b";
			IrcClient.Send("MODE {0} {1} {2}", Channel, flag, Util.GetWords(Masks, 0));
		}

		public void Ban(string Channel, TimeSpan Time, params object[] Masks)
		{
			Ban(IrcClient.GetChannel(Channel), Time, Masks);
		}

		public void Ban(IrcChannel Channel, TimeSpan Time, params object[] Masks)
		{
			if (Masks.Length == 0)
				return;
			string flag = "+";
			foreach (string mask in Masks)
			{
				new UnbanTimer(Channel, mask, Time);
				flag += "b";
			}
			IrcClient.Send("MODE {0} {1} {2}", Channel, flag, Util.GetWords(Masks, 0));
		}

		public void KickBan(string channel, string reason, params object[] masks)
		{
			KickBan(IrcClient.GetChannel(channel), reason, masks);
		}

		public void KickBan(IrcChannel channel, string reason, params object[] masks)
		{
			Ban(channel, masks);
			foreach (string mask in masks)
			{
				foreach (IrcUser u in channel)
					if (u.Matches(mask))
						Kick(channel, u, reason);
			}
		}

		public void KickBan(string channel, params object[] masks)
		{
			KickBan(IrcClient.GetChannel(channel), masks);
		}

		public void KickBan(IrcChannel channel, params object[] masks)
		{
			Ban(channel, masks);
			foreach (string mask in masks)
			{
				foreach (IrcUser u in channel)
					if (u.Matches(mask))
						Kick(channel, u);
			}
		}

		public void KickBan(string channel, TimeSpan time, string reason, params object[] masks)
		{
			KickBan(IrcClient.GetChannel(channel), time, reason, masks);
		}

		public void KickBan(IrcChannel channel, TimeSpan time, string reason, params object[] masks)
		{
			Ban(channel, time, masks);
			foreach (string mask in masks)
			{
				foreach (IrcUser u in channel)
					if (u.Matches(mask))
						Kick(channel, u, reason);
			}
		}

		public void KickBan(string channel, TimeSpan time, params object[] masks)
		{
			KickBan(IrcClient.GetChannel(channel), time, masks);
		}

		public void KickBan(IrcChannel channel, TimeSpan time, params object[] masks)
		{
			Ban(channel, time, masks);
			foreach (string mask in masks)
			{
				foreach (IrcUser u in channel)
					if (u.Matches(mask))
						Kick(channel, u);
			}
		}

		public void Unban(IrcChannel Channel, string Masks)
		{
			Unban(Channel, Masks.Split(' '));
		}

		public void Unban(IrcChannel Channel, params string[] Masks)
		{
			Unban(Channel.Name, Masks);
		}

		public void Unban(string Channel, string Masks)
		{
			Unban(Channel, Masks.Split(' '));
		}

		public void Unban(string Channel, params string[] Masks)
		{
			if (Masks.Length == 0)
				return;
			string flag = "-";
			for (int i = 0; i < Masks.Length; i++)
				flag += "b";
			IrcClient.Send("MODE {0} {1} {2}", Channel, flag, Util.GetWords(Masks, 0));
		}

		public void RetrieveBanList(string Channel)
		{
			IrcClient.Send("MODE " + Channel + " +b");
		}

		public void Invite(string Nick, string Channel)
		{
			IrcClient.Send("INVITE " + Nick + " " + Channel);
		}

		public void Invite(string Nick, IrcChannel Channel)
		{
			IrcClient.Send("INVITE " + Nick + " " + Channel);
		}

		public void Invite(IrcUser User, string Channel)
		{
			IrcClient.Send("INVITE " + User + " " + Channel);
		}

		public void Invite(IrcUser User, IrcChannel Channel)
		{
			IrcClient.Send("INVITE " + User + " " + Channel);
		}

	}
}
