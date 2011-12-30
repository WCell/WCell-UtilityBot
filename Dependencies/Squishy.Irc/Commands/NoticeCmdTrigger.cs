using StringStream = WCell.Util.Strings.StringStream;

namespace Squishy.Irc.Commands
{
	/// <summary>
	/// Triggers through NOTICE
	/// </summary>
	public class NoticeCmdTrigger : IrcCmdTrigger
	{
		public NoticeCmdTrigger(string args, IrcUser user, IrcChannel chan = null)
			: this(new StringStream(args), user, chan)
		{
		}

		public NoticeCmdTrigger(StringStream args, IrcUser user, IrcChannel chan = null)
			: base(args, user, chan)
		{
		}

		public override void Reply(string text)
		{
			Args.Target.Notice(text);
		}
	}
}