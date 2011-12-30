using System;

namespace Squishy.Irc.Auth
{
	public class AuthenticationMgr
	{
		public event Action<IrcUser> AuthResolved;

		public AuthenticationMgr()
		{
		}

		public IIrcAuthenticator Authenticator
		{
			get;
			set;
		}

		public bool CanResolve
		{
			get { return Authenticator != null; }
		}

		public bool ResolvesInstantly
		{
			get { return Authenticator != null && Authenticator.ResolvesInstantly; }
		}

		public bool IsResolving(IrcUser user)
		{
			return Authenticator != null && Authenticator.IsResolving(user);
		}

		internal bool ResolveAuth(IrcUser user, IrcUserAuthResolvedHandler callback)
		{
			if (Authenticator == null)
			{
				// No Authenticator => Cannot resolve
				return false;
			}
			else
			{
				Authenticator.ResolveAuth(user, userArg =>
				{
					OnAuthResolved(userArg);

					if (callback != null)
					{
						callback(user);
					}
				});
				return true;
			}
		}

		internal void OnAuthResolved(IrcUser user)
		{
			if (user.IrcClient.NotifyAuthedUsers)
			{
				user.Msg("You have been authenticated as: " + user.AuthName);
			}
			var evt = AuthResolved;
			if (evt != null)
			{
				evt(user);
			}
		}

		internal void OnNewUser(IrcUser user)
		{
			if (user.IrcClient.AutoResolveAuth && Authenticator != null)
			{
				ResolveAuth(user, null);
			}
		}

		internal void Cleanup()
		{
			if (Authenticator != null)
			{
				Authenticator.Dispose();
				Authenticator = null;
			}
		}
	}
}
