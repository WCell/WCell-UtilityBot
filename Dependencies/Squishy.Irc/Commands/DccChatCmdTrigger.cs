using Squishy.Irc.Dcc;
using StringStream = WCell.Util.Strings.StringStream;

namespace Squishy.Irc.Commands
{
	/// <summary>
	/// Triggers through DCC-chat
	/// </summary>
	public class DccChatCmdTrigger : IrcCmdTrigger
	{
		public DccChatCmdTrigger(StringStream args, IrcUser user)
			: base(args, user, null)
		{
		}

		public override void Reply(string text)
		{
			DccChatClient client = Args.IrcClient.Dcc.GetChatClient(Args.User);
			//if (client != null) {
			client.Send(text);
			//}
		}
	}
}