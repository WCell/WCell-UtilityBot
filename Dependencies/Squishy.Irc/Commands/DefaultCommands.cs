using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using Squishy.Irc.Dcc;
using Squishy.Irc.Protocol;
using WCell.Util;
using WCell.Util.Commands;

namespace Squishy.Irc.Commands
{
	/*public class VersionCommand : IrcCommand
	{
		protected override void Initialize()
		{
			Init("Version");
			EnglishDescription = "Shows the version of this client.";
		    Enabled = false;
		}

		public override void Process(CmdTrigger<IrcCmdArgs> trigger)
		{
			//trigger.Reply(IrcClient.Version);
			AssemblyName asmName = Assembly.GetAssembly(GetType()).GetName();
			trigger.Reply(asmName.Name + ", v" + asmName.Version);
		}
	}
    */
	public class JoinCommand : IrcCommand
	{
		protected override void Initialize()
		{
			Init("Join", "J");
			EnglishParamInfo = "<Channel>";
			EnglishDescription = "Joins a channel";
		    RequiredAccountLevel = AccountLevel.SuperAdmin;
		}

		public override void Process(CmdTrigger<IrcCmdArgs> trigger)
		{
			trigger.Args.IrcClient.CommandHandler.Join(trigger.Text.NextWord());
		}

		//public override string[] ExpectedServResponses {
		//    get {
		//        return new string[] {
		//            "JOIN",
		//            "405",				// Too many channels
		//            "471",				// Cannot join channel (+l)
		//            "473",				// Cannot join channel (+i)
		//            "474",				// Cannot join channel (+b)
		//            "475",				// Cannot join channel (+k)
		//            "477",				// need to register
		//            "485"				// Cannot join channel
		//        };
		//    }
		//}
	}

	/*public class AuthCommand : IrcCommand
	{
		protected override void Initialize()
		{
			Init("Auth");
			EnglishDescription = "Will query the Authentication data from the server if not already present.";
		}

		public override void Process(CmdTrigger<IrcCmdArgs> trigger)
		{
			var user = trigger.Args.User;
			if (user == null)
			{
				trigger.Reply("AuthCommand requires User-argument.");
			}
			else
			{
				if (user.IsAuthenticated)
				{
					trigger.Reply("User {0} is authed as: {1}", user.Nick, user.AuthName);
				}
				else
				{
					var authMgr = trigger.Args.IrcClient.AuthMgr;
					if (authMgr.IsResolving(user))
					{
						trigger.Reply("User {0} is being resolved - Please wait...", user.Nick, user.AuthName);
					}
					else if (!authMgr.CanResolve)
					{
						trigger.Reply("Authentication is not supported on this Network.");
					}
					else
					{
						trigger.Reply("Resolving User...".Colorize(IrcColorCode.Red));
						if (!user.ResolveAuthentication())
						{
							trigger.Reply("Failed to resolve user - This network's authentication method is probably not supported.".Colorize(IrcColorCode.Red));
						}
					}
				}
			}
		}

		//public override string[] ExpectedServResponses {
		//    get {
		//        return new string[] {
		//            "JOIN",
		//            "405",				// Too many channels
		//            "471",				// Cannot join channel (+l)
		//            "473",				// Cannot join channel (+i)
		//            "474",				// Cannot join channel (+b)
		//            "475",				// Cannot join channel (+k)
		//            "477",				// need to register
		//            "485"				// Cannot join channel
		//        };
		//    }
		//}
	}*/

	public class NickCommand : IrcCommand
	{
		protected override void Initialize()
		{
			Init("Nick");
			EnglishParamInfo = "<NewNnick>";
			EnglishDescription = "Changes your current nickname.";
		    RequiredAccountLevel = AccountLevel.SuperAdmin;
		}

		public override void Process(CmdTrigger<IrcCmdArgs> trigger)
		{
			trigger.Args.IrcClient.CommandHandler.Nick(trigger.Text.NextWord());
		}
	}

	public class TopicCommand : IrcCommand
	{
		protected override void Initialize()
		{
			Init("Topic");
			EnglishParamInfo = "[<Channel>] <Topic>";
			EnglishDescription = "Changes the Topic in the given Channel (if possible). The channel parameter will only be accepted if not used in a Channel.";
		    RequiredAccountLevel = AccountLevel.Admin;
		}

		public override void Process(CmdTrigger<IrcCmdArgs> trigger)
		{
			var chan = trigger.Args.Channel;
			if (chan == null)
			{
				chan = trigger.Args.IrcClient.GetChannel(trigger.Text.NextWord());
				if (chan == null)
				{
					trigger.Reply("Invalid Channel.");
					return;
				}
			}
			chan.Topic = trigger.Text.Remainder;
		}
	}

	public class PartCommand : IrcCommand
	{
		protected override void Initialize()
		{
			Init("Part");
			EnglishParamInfo = "[<Channel> [<Reason>]]";
			EnglishDescription = "Parts a given channel (or the channel of origin if no argument given) with an optional reason";
		    RequiredAccountLevel = AccountLevel.SuperAdmin;
		}

		public override void Process(CmdTrigger<IrcCmdArgs> trigger)
		{
			var target = trigger.Text.NextWord();
			if (target.Length == 0)
			{
				target = trigger.Args.Channel.Name;
			}
			trigger.Args.IrcClient.CommandHandler.Part(target,
									  trigger.Text.Remainder.Trim());
		}
	}

	public class PartThisCommand : IrcCommand
	{
		protected override void Initialize()
		{
			Init("PartThis");
			EnglishParamInfo = "[<Reason>]";
			EnglishDescription = "Parts the channel from where the trigger originated";
            RequiredAccountLevel = AccountLevel.SuperAdmin;
		}

		public override void Process(CmdTrigger<IrcCmdArgs> trigger)
		{
			trigger.Args.IrcClient.CommandHandler.Part(trigger.Args.Channel,
									  trigger.Text.Remainder.Trim());
		}
	}

	public class MsgCommand : IrcCommand
	{
		protected override void Initialize()
		{
			Init("Msg", "Message", "Privmsg");
			EnglishParamInfo = "<Target> <Text>";
			EnglishDescription = "Sends a privmsg to the specified target";
		    RequiredAccountLevel = AccountLevel.SuperAdmin;
		}

		public override void Process(CmdTrigger<IrcCmdArgs> trigger)
		{
			trigger.Args.IrcClient.CommandHandler.Msg(trigger.Text.NextWord(), trigger.Text.Remainder.Trim());
		}
	}

	public class NoticeCommand : IrcCommand
	{
	    protected override void Initialize()
		{
			Init("Notice");
			EnglishParamInfo = "<Target> <Text>";
			EnglishDescription = "Sends a notice to the specified target";
		    RequiredAccountLevel = AccountLevel.SuperAdmin;
		}

		public override void Process(CmdTrigger<IrcCmdArgs> trigger)
		{
			trigger.Args.IrcClient.CommandHandler.Notice(trigger.Text.NextWord(), trigger.Text.Remainder.Trim());
		}
	}

	public class CtcpRequestCommand : IrcCommand
	{
		protected override void Initialize()
		{
			Init("Ctcp");
			EnglishParamInfo = "<Target> <Request> [<arguments>]";
			EnglishDescription = "Sends a ctcp - request to a target";
            RequiredAccountLevel = AccountLevel.SuperAdmin;
		}

		public override void Process(CmdTrigger<IrcCmdArgs> trigger)
		{
			trigger.Args.IrcClient.CommandHandler.CtcpRequest(trigger.Text.NextWord(), trigger.Text.NextWord(),
											 trigger.Text.Remainder.Trim());
		}
	}

	public class KickCommand : IrcCommand
	{
		protected override void Initialize()
		{
			Init("Kick");
			EnglishParamInfo = "<Channel> <User> [<Reason>]";
			EnglishDescription = "Kicks a user from a channel with an optional reason";
            RequiredAccountLevel = AccountLevel.Admin;
		}

		public override void Process(CmdTrigger<IrcCmdArgs> trigger)
		{
			trigger.Args.IrcClient.CommandHandler.Kick(trigger.Text.NextWord(), trigger.Text.NextWord(),
									  trigger.Text.Remainder.Trim());
		}
	}


	public class KickMaskCommand : IrcCommand
	{
		protected override void Initialize()
		{
			Init("KickMask", "KickM");
			EnglishParamInfo = "<Channel> <Mask> [<Reason>]";
			EnglishDescription = "Kicks all users with a specified mask for an optional reason";
            RequiredAccountLevel = AccountLevel.SuperAdmin;
		}

		public override void Process(CmdTrigger<IrcCmdArgs> trigger)
		{
			string chanName = trigger.Text.NextWord();
			IrcChannel chan = trigger.Args.IrcClient.GetChannel(chanName);

			if (chan != null)
			{
				string mask = trigger.Text.NextWord();

				foreach (IrcUser user in chan)
				{
					if (user.Matches(mask))
						trigger.Args.IrcClient.CommandHandler.Kick(chanName, user.Nick, trigger.Text.Remainder.Trim());
				}
			}
		}
	}

	public class ModeCommand : IrcCommand
	{
		protected override void Initialize()
		{
			Init("Mode");
			EnglishParamInfo = "<flags> <targets>";
			EnglishDescription = "Sets the specified mode";
            RequiredAccountLevel = AccountLevel.SuperAdmin;
		}

		public override void Process(CmdTrigger<IrcCmdArgs> trigger)
		{
			trigger.Args.IrcClient.CommandHandler.Mode(trigger.Text.Remainder.Trim());
		}
	}

	public class BigBanCommand : IrcCommand
	{
		protected override void Initialize()
		{
			Init("BigBan");
			EnglishParamInfo = "[-u <seconds>] <Channel> <Mask1> <Mask2> ...";
			EnglishDescription =
				"Bans masks from a channel. " +
				"If the -u switch is specified, the following argument must be the number of seconds before the masks are automatically unbanned again.";
		    RequiredAccountLevel = AccountLevel.Admin;
		}

		public override void Process(CmdTrigger<IrcCmdArgs> trigger)
		{
			string word = trigger.Text.NextWord();
			string target = trigger.Text.NextWord();
			if (word.Equals("-u"))
			{
				TimeSpan time = TimeSpan.FromSeconds(trigger.Text.NextInt());
				trigger.Args.IrcClient.CommandHandler.Ban(target, time, trigger.Text.RemainingWords());
			}
			else
			{
				trigger.Args.IrcClient.CommandHandler.Ban(target, trigger.Text.RemainingWords());
			}
		}
	}

	public class UnbanCommand : IrcCommand
	{
		protected override void Initialize()
		{
			Init("Unban");
			EnglishParamInfo = "<Channel> <Mask1> <Mask2> ...";
			EnglishDescription = "Unbans given masks from a channel";
		    RequiredAccountLevel = AccountLevel.Admin;
		}

		public override void Process(CmdTrigger<IrcCmdArgs> trigger)
		{
			trigger.Args.IrcClient.CommandHandler.Unban(trigger.Text.NextWord(), trigger.Text.RemainingWords());
		}
	}

	public class InviteCommand : IrcCommand
	{
		protected override void Initialize()
		{
			Init("Invite");
			EnglishParamInfo = "<Nick> [<Channel>]";
			EnglishDescription = "Invites a person into a channel. Invites into the channel of origin if channel is left out.";
		    RequiredAccountLevel = AccountLevel.Staff;
		}

		public override void Process(CmdTrigger<IrcCmdArgs> trigger)
		{
			string nick = trigger.Text.NextWord();
			string target = trigger.Text.NextWord();
			if (target.Length == 0)
			{
				target = trigger.Args.Channel.Name;
			}
			trigger.Args.IrcClient.CommandHandler.Invite(nick, target);
		}
	}

	public class InviteMeCommand : IrcCommand
	{
		protected override void Initialize()
		{
			Init("InviteMe");
			EnglishParamInfo = "<Channel>";
			EnglishDescription = "Invites the triggering user into a channel.";
		    RequiredAccountLevel = AccountLevel.Staff;
		}

		public override void Process(CmdTrigger<IrcCmdArgs> trigger)
		{
			trigger.Args.IrcClient.CommandHandler.Invite(trigger.Args.User, trigger.Text.NextWord());
		}
	}

	public class BanListCommand : IrcCommand
	{
		protected override void Initialize()
		{
			Init("BanList", "ListBans");
			EnglishParamInfo = "<channel>";
			EnglishDescription = "Retrieves the active banmasks from a channel";
		    RequiredAccountLevel = AccountLevel.SuperAdmin;
		}

		public override void Process(CmdTrigger<IrcCmdArgs> trigger)
		{
			trigger.Args.IrcClient.CommandHandler.RetrieveBanList(trigger.Text.NextWord());
		}
	}

	public class SetInfoCommand : IrcCommand
	{
		protected override void Initialize()
		{
			Init("SetInfo", "ChangeInfo");
			EnglishParamInfo = "<userinfo>";
			EnglishDescription = "Changes your user-info (will have effect after reconnect)";
            RequiredAccountLevel = AccountLevel.SuperAdmin;
		}

		public override void Process(CmdTrigger<IrcCmdArgs> trigger)
		{
			trigger.Args.IrcClient.Info = trigger.Text.NextWord();
		}
	}

	public class SetPwCommand : IrcCommand
	{
		protected override void Initialize()
		{
			Init("SetPw", "SetPass", "ChangePass");
			EnglishParamInfo = "<newPassword>";
			EnglishDescription = "Changes your password (will have effect after reconnect)";
            RequiredAccountLevel = AccountLevel.SuperAdmin;
		}

		public override void Process(CmdTrigger<IrcCmdArgs> trigger)
		{
			trigger.Args.IrcClient.ServerPassword = trigger.Text.NextWord();
		}
	}

	public class SetUsernameCommand : IrcCommand
	{
		protected override void Initialize()
		{
			Init("SetUser", "SetUsername");
			EnglishParamInfo = "<username>";
			EnglishDescription = "Changes your username (will have effect after reconnect)";
            RequiredAccountLevel = AccountLevel.SuperAdmin;
		}

		public override void Process(CmdTrigger<IrcCmdArgs> trigger)
		{
			trigger.Args.IrcClient.UserName = trigger.Text.NextWord();
		}
	}

	public class SetNicksCommand : IrcCommand
	{
		protected override void Initialize()
		{
			Init("SetNicks");
			EnglishParamInfo = "<nick> [<nick2> [<nick3>...]]";
			EnglishDescription = "Changes your default nicknames (seperated by space).";
		}

		public override void Process(CmdTrigger<IrcCmdArgs> trigger)
		{
			trigger.Args.IrcClient.Nicks = trigger.Text.RemainingWords();
            trigger.Args.IrcClient.CommandHandler.Nick(trigger.Args.IrcClient.Nicks[0]);
		}
	}

	public class ConnectCommand : IrcCommand
	{
		protected override void Initialize()
		{
			Init("Connect", "Con");
			EnglishParamInfo = "[<address> [<port>]]";
			EnglishDescription = "(Re)connects to the given server.";
            RequiredAccountLevel = AccountLevel.SuperAdmin;
		}

		public override void Process(CmdTrigger<IrcCmdArgs> trigger)
		{
			string addr = trigger.Text.NextWord();
			int port = trigger.Args.IrcClient.Client.RemotePort;
			if (addr.Length == 0)
			{
				addr = trigger.Args.IrcClient.Client.RemoteAddress;
			}
			else
			{
				port = trigger.Text.NextInt(port);
			}
			trigger.Args.IrcClient.Client.Disconnect();

			trigger.Args.IrcClient.BeginConnect(addr, port);
		}
	}

	public class DisconnectCommand : IrcCommand
	{
		protected override void Initialize()
		{
			Init("Disconnect", "Discon");
			EnglishParamInfo = "";
			EnglishDescription = "Disconnects the current connection";
            RequiredAccountLevel = AccountLevel.SuperAdmin;
		}

		public override void Process(CmdTrigger<IrcCmdArgs> trigger)
		{
			trigger.Args.IrcClient.Client.DisconnectNow();
		}
	}

	public class SetExternalIPCommand : IrcCommand
	{
		protected override void Initialize()
		{
			Init("SetExternalIP");
			EnglishParamInfo = "<Ip>";
			EnglishDescription = "Changes your Util.ExternalAddres. This is used for DCC sessions and to bind local sockets to, if possible.";
            RequiredAccountLevel = AccountLevel.SuperAdmin;
		}

		public override void Process(CmdTrigger<IrcCmdArgs> trigger)
		{
			Util.ExternalAddress = IPAddress.Parse(trigger.Text.NextWord());
		}
	}
    
	public class DccSendCommand : IrcCommand
	{
		protected override void Initialize()
		{
			Init("DccSend");
			EnglishParamInfo = "<filename> [<port> [<target>]]";
			EnglishDescription = "Tries to send a file to a user.";
            RequiredAccountLevel = AccountLevel.SuperAdmin;
		}

		public override void Process(CmdTrigger<IrcCmdArgs> trigger)
		{
			string filename = trigger.Text.NextWord();
			var port = trigger.Text.NextInt(0);
			string target = trigger.Text.NextWord();
			if (target.Length == 0)
			{
				target = trigger.Args.User.Nick;
			}
			trigger.Args.IrcClient.Dcc.Send(target, filename, port);
		}
	}

	public class DccChatCommand : IrcCommand
	{
		protected override void Initialize()
		{
			Init("DccChat");
			EnglishParamInfo = "<target> [<port> [<text>]]";
			EnglishDescription =
				"Tries to establish a direct Chat session with the specified target or sends the given text if the connection is already established.";
		}

		public override void Process(CmdTrigger<IrcCmdArgs> trigger)
		{
			var nick = trigger.Text.NextWord();
			var port = trigger.Text.NextInt(0);
			var client = trigger.Args.IrcClient.Dcc.GetChatClient(nick);
			if (client == null)
			{
				trigger.Args.IrcClient.Dcc.Chat(nick, port);
			}
			else
			{
				client.Send(trigger.Text.Remainder.Trim());
			}
		}
	}

	//public class StatsCommand : IrcCommand {
	//    protected override void Initialize()
	//        {
	//		  Init("Stats");
	//        EnglishParamInfo ="<options>";
	//        EnglishDescription = "Queries server stats.";
	//    }

	//    public override void Process(CmdTrigger<IrcCmdArgs> trigger) {
	//        trigger.Args.IrcClient.Send("STATS " + trigger.Text.Remainder.Trim());
	//    }
	//}
    
	public class SendCommand : IrcCommand
	{
		protected override void Initialize()
		{
			Init("Send");
			EnglishParamInfo = "<args>";
			EnglishDescription = "Sends the given args as-is to the server (raw).";
            RequiredAccountLevel = AccountLevel.SuperAdmin;
		}

		public override void Process(CmdTrigger<IrcCmdArgs> trigger)
		{
			trigger.Args.IrcClient.Send(trigger.Text.Remainder.Trim());
		}
	}
	/*
	public class EchoCommand : IrcCommand {
		protected override void Initialize() {
Init("echo","e","ech") {
			EnglishParamInfo ="Echo <text>";
			EnglishDescription = "Echo'str some evluated text in the active window";
		}

		public override void Process(CmdTrigger<IrcCmdArgs> trigger) {
			Match match = (new Regex(@"\$([^ ]+)")).Match(args);
			Type type = typeof(IrcConnection);
			while (match.Success) {
				string var = match.Groups[1].Value;
				PropertyInfo prop = type.GetProperty(var, BindingFlags.IgnoreCase);
				if (prop != null) {
					args = args.Replace(var, prop.GetValue(trigger.Args.IrcClient, new object[0]).ToString());
				}
				match = match.NextMatch();
			}
			Window.Active.WriteLine(args.Replace("$active", Window.Active.Text));
		}
	}*/
}