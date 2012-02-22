using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Squishy.Irc;

namespace WCellUtilityBot.ChannelManagment
{
    public class AutoVoiceUser
    {
        private readonly Timer  _voiceTimer = new Timer(TimeSpan.FromSeconds(30).TotalMilliseconds);
        public IrcUser User;
        public IrcChannel Channel;
        public AutoVoiceUser(IrcUser user,IrcChannel channel)
        {
            User = user;
            Channel = channel;
            _voiceTimer.Elapsed += delegate
                                      {
                                          if(!user.Modes.Contains("v"))
                                          user.IrcClient.CommandHandler.Mode(Channel, "+v", User);
                                      };
            _voiceTimer.Start();
        }
    }
}
