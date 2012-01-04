using System;
using System.Reflection;
using WCell.Util.Commands;


namespace Squishy.Irc.Commands
{
	public partial class IrcCommandHandler : CommandMgr<IrcCmdArgs>
	{
		public string[] RemoteCommandPrefixes = new [] {"!"};

        public IrcCommandHandler(IrcClient client)
		{
			Remove(this["Help"]);		// remove default help command (use our custom one instead)

			IrcClient = client;
			TriggerValidator =
				delegate(CmdTrigger<IrcCmdArgs> trigger, BaseCommand<IrcCmdArgs> cmd, bool silent)
				{
					var rootCmd = cmd.RootCmd;
					if (rootCmd is HelpCommand)
					{
						return true;
					}
					return client.MayTriggerCommand(trigger, (IrcCommand) rootCmd);
				};


			AddCmdsOfAsm(typeof(IrcCommand).Assembly);
			var callAsm = Assembly.GetEntryAssembly();
			if (callAsm != null && callAsm != typeof(IrcCommand).Assembly)
			{
				AddCmdsOfAsm(callAsm);
			}
		}

		public IrcClient IrcClient
		{
			get;
			private set;
		}

		public override string ExecFileDir
		{
			get { throw new NotImplementedException(); }
		}

		public override bool Execute(CmdTrigger<IrcCmdArgs> trigger)
		{
			trigger.Args.IrcClient = IrcClient;
			return base.Execute(trigger);
		}

		public override bool Execute(CmdTrigger<IrcCmdArgs> trigger, BaseCommand<IrcCmdArgs> cmd, bool silentFail)
		{
			trigger.Args.IrcClient = IrcClient;
			return base.Execute(trigger, cmd, silentFail);
		}
	}
}
