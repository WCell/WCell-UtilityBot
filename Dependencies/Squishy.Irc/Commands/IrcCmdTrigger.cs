using WCell.Util.Commands;
using WCell.Util.Strings;

namespace Squishy.Irc.Commands
{
	public abstract class IrcCmdTrigger : CmdTrigger<IrcCmdArgs>
	{
		public IrcCmdTrigger(string args, IrcUser user = null, IrcChannel chan = null)
			: this(new StringStream(args), user, chan)
		{
		}

        public IrcCmdTrigger(StringStream args, IrcUser user = null, IrcChannel chan = null)
            : base(args, new IrcCmdArgs(user, chan))
        {
        }

	    public override void ReplyFormat(string text)
		{
			Reply(text);
		}
        public void Notice(string text)
        {
            Args.User.Notice(text);
        }
	}
}
