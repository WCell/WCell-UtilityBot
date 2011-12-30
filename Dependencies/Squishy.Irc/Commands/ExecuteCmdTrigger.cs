using System;
using StringStream = WCell.Util.Strings.StringStream;

namespace Squishy.Irc.Commands
{
	/// <summary>
	/// Uses the console for output.
	/// </summary>
	public class ExecuteCmdTrigger : IrcCmdTrigger
	{
		public ExecuteCmdTrigger(StringStream args, IrcUser user, IrcChannel chan)
			: base(args, user, chan)
		{
		}

		public override void Reply(string text)
		{
			Console.WriteLine(text);
		}
	}
}