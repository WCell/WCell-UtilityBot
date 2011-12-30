using StringStream = WCell.Util.Strings.StringStream;

namespace Squishy.Irc.Commands
{
	/// <summary>
	/// Triggers through PRIVMSG (normal chatting in channels and queries)
	/// </summary>
	internal class MsgCmdTrigger : IrcCmdTrigger
	{
		public MsgCmdTrigger(string args, IrcUser user, IrcChannel chan = null)
			: this(new StringStream(args), user, chan)
		{
		}

		public MsgCmdTrigger(StringStream args, IrcUser user, IrcChannel chan = null)
			: base(args, user, chan)
		{
		}

		public override void Reply(string text)
		{
			Args.Target.Msg(text);
		}
	}
}