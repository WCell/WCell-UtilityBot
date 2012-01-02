/*using System;
using System.Collections.Generic;
using System.Text;
using Squishy.Irc;
//using Squishy.Irc.Aliasing;
using Squishy.Irc.Commands;
using WCell.Util.Commands;

namespace Squishy.Irc.ACL {
    public class CommandAccessController {
        IrcClient irc;
        /// <summary>
        /// If no rule applies, allow or disallow?
        /// </summary>
        public bool DefaultAccess = false;
        /// <summary>
        /// Command -> Accessor-Id -> List of rules
        /// </summary>
        readonly IDictionary<Command<IrcCmdArgs>, IDictionary<CommandAccessor, List<CommandAccessRule>>> ruleMap;
        public CommandAccessController(IrcClient irc) {
            this.irc = irc;
            ruleMap = new Dictionary<Command<IrcCmdArgs>, IDictionary<CommandAccessor, List<CommandAccessRule>>>();
        }

        public void AddRule(CommandAccessRule rule) {
            foreach (Command<IrcCmdArgs> cmd in rule.Commands)
            {
                IDictionary<CommandAccessor, List<CommandAccessRule>> rules;
                if (!ruleMap.TryGetValue(cmd, out rules)) {
                    ruleMap.Add(cmd, rules = new Dictionary<CommandAccessor, List<CommandAccessRule>>());
                }
                foreach (KeyValuePair<string, CommandAccessor> accessor in rule.Accessors) {
                    List<CommandAccessRule> list;
                    if (!rules.TryGetValue(accessor.Value, out list)) {
                        rules.Add(accessor.Value, list = new List<CommandAccessRule>());
                    }
                    list.Add(rule);
                }
            }
        }

        public bool Check(string id) {
            return Check(id, null);
        }

        public bool Check(string id, UserPrivSet privs) {
            IDictionary<CommandAccessor, List<CommandAccessRule>> rules;
            if (ruleMap.TryGetValue(cmd, out rules)) {
                List<CommandAccessRule> list;
                if (rules.TryGetValue(id, out list)) {
                    foreach (CommandAccessRule rule in list) {
                        return rule.Allow &&(privs == null || (privs.PrivSet & rule.GetPrivs(id)));
                    }
                }
            }
        }

        /// <summary>
        /// Checks wether there are contradictionary rules.
        /// </summary>
        public void CheckList() {
            foreach (IDictionary<string, List<CommandAccessRule>> rules in ruleMap.Values)
            {
                foreach (List<CommandAccessRule> list in rules.Values) {
                    foreach (CommandAccessRule rule in list) {
                        return rule.Allow &&
                                (privs == null || (privs.Set & rule.GetPrivs(id)));
                    }
                }
            }
        }

        /// <summary>
        /// Returns wether or not Command cmd may be triggered by the given trigger.
        /// </summary>
        public bool MayExecute(CmdTrigger<IrcCmdArgs> trigger, Command<IrcCmdArgs> cmd)
        {

            return DefaultAccess;
        }
    }

    public class CommandAccessRule {
        public bool Allow;
        List<Command<IrcCmdArgs>> commands;
        IDictionary<string,CommandAccessor> accessors;

        public CommandAccessRule(bool Allow)
            : this(allow, new List<Command<IrcCmdArgs>>(),
                                                        new Dictionary<string, CommandAccessor>()) {
        }

        public CommandAccessRule(bool allow, List<Command<IrcCmdArgs>> commands,
                                    IDictionary<string, CommandAccessor> accessors) {
            this.Allow = allow;
            this.commands = commands;
            this.accessors = accessors;
        }

        public List<Command<IrcCmdArgs>> Commands
        {
            get {
                return commands;
            }
        }

        public IDictionary<string,CommandAccessor> Accessors {
            get {
                return accessors;
            }
        }

        public Set<Privilege> GetPrivs(string id) {
            CommandAccessor accessor;
            if (accessors.TryGetValue(id, out accessor)) {
                return accessor.Privs;
            }
            return null;
        }

        public CommandAccessor GetAccessor(string id) {
            CommandAccessor accessor;
            accessors.TryGetValue(id, out accessor);
            return accessor;
        }
    }

    // TODO: Change
    public class CommandAccessor {
        private Set<Privilege> Voiced = new Set<Privilege>().Add(Privilege.Voice | Privilege.HalfOp | Privilege.Op | Privilege.Admin | Privilege.Owner);

        private Set<Privilege> Opped = new Set<Privilege>().Add(Privilege.HalfOp | Privilege.Op | Privilege.Admin | Privilege.Owner);

        ChatTarget accessor;
        Set<Privilege> privs;

        public CommandAccessor(IrcUser user) {
            accessor = user;
        }

        public CommandAccessor(IrcChannel channel, Set<Privilege> privs) {
            accessor = channel;
            this.privs = privs;
        }

        public bool IsChannel() {
            return accessor is IrcChannel;
        }

        public ChatTarget Accessor {
            get {
                return accessor;
            }
        }

        public Set<Privilege> Privs {
            get {
                return privs;
            }
        }
    }
}
*/