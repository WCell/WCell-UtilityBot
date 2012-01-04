using WCell.Util.Commands;

namespace Squishy.Irc.Commands
{
	public class IrcCmdArgs : ICmdArgs
	{
		public IrcCmdArgs(IrcUser user, IrcChannel channel)
		{
			User = user;
			Channel = channel;
		}
		public IrcClient IrcClient
		{
			get;
			internal set;	
		}

		public IrcChannel Channel
		{
			get;
			internal set;
		}

		public IrcUser User
		{
			get;
			internal set;
		}

		/// <summary>
		/// The ChatTarget which initialized this trigger (<code>Channel</code> or <code>User</code>).
		/// </summary>
		public ChatTarget Target
		{
			get { return (Channel != null ? (ChatTarget)Channel : User); }
		}

	}
}
