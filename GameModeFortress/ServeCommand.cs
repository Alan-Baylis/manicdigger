using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using GameModeFortress;
using ManicDigger;
using System.Drawing;

namespace ManicDiggerServer
{
    public partial class Server
    {
        public void CommandInterpreter(int sourceClientId, string command, string argument)
        {
            string[] ss;
            int id;

            switch (command)
            {
                case "msg":
                case "pm":
                    ss = argument.Split(new[] { ' ' });
                    if (ss.Length >= 2)
                    {
                        this.PrivateMessage(sourceClientId, ss[0], string.Join(" ", ss, 1, ss.Length - 1));
                        return;
                    }
                    SendMessage(sourceClientId, colorError + "Invalid arguments. Type /help to see command's usage.");
                    return;
                case "op":
                case "chgrp":
                case "cg":
                    ss = argument.Split(new[] { ' ' });
                    if (ss.Length == 2)
                    {
                        this.ChangeGroup(sourceClientId, ss[0], ss[1]);
                        return;
                    }
                    SendMessage(sourceClientId, colorError + "Invalid arguments. Type /help to see command's usage.");
                    return;
                case "login":
                     // enables to change temporary group with a group's password (only if group allows it)
                    ss = argument.Split(new[] { ' ' });
                    if (ss.Length == 2)
                    {
                        this.Login(sourceClientId, ss[0], ss[1]);
                        return;
                    }
                    SendMessage(sourceClientId, colorError + "Invalid arguments. Type /help to see command's usage.");
                    return;
                case "welcome":
                    this.WelcomeMessage(sourceClientId, argument);
                    return;
                case "announcement":
                    this.Announcement(sourceClientId, argument);
                    return;
                case "logging":
                    ss = argument.Split(new[] { ' ' });
                    if (ss.Length == 1)
                    {
                        this.SetLogging(sourceClientId, ss[0], "");
                        return;
                    }
                    if (ss.Length == 2)
                    {
                        this.SetLogging(sourceClientId, ss[0], ss[1]);
                        return;
                    }
                    SendMessage(sourceClientId, colorError + "Invalid arguments. Type /help to see command's usage.");
                    return;
                case "kick_id":
                    ss = argument.Split(new[] { ' ' });
                    if (!Int32.TryParse(ss[0], out id))
                    {
                        SendMessage(sourceClientId, colorError + "Invalid arguments. Type /help to see command's usage.");
                        return;
                    }
                    if (ss.Length >= 2)
                    {
                        this.Kick(sourceClientId, id, string.Join(" ", ss, 1, ss.Length - 1));
                        return;
                    }
                    this.Kick(sourceClientId, id);
                    return;
                case "kick":
                    ss = argument.Split(new[] { ' ' });
                    if (ss.Length >= 2)
                    {
                        this.Kick(sourceClientId, ss[0], string.Join(" ", ss, 1, ss.Length - 1));
                        return;
                    }
                    this.Kick(sourceClientId, argument);
                    return;
                case "banip_id":
                    ss = argument.Split(new[] { ' ' });
                    if (!Int32.TryParse(ss[0], out id))
                    {
                        SendMessage(sourceClientId, colorError + "Invalid arguments. Type /help to see command's usage.");
                        return;
                    }
                    if (ss.Length >= 2)
                    {
                        this.BanIP(sourceClientId, id, string.Join(" ", ss, 1, ss.Length - 1));
                        return;
                    }
                    this.BanIP(sourceClientId, id);
                    return;
                case "banip":
                    ss = argument.Split(new[] { ' ' });
                    if (ss.Length >= 2)
                    {
                        this.BanIP(sourceClientId, ss[0], string.Join(" ", ss, 1, ss.Length - 1));
                        return;
                    }
                    this.BanIP(sourceClientId, argument);
                    return;
                case "ban_id":
                    ss = argument.Split(new[] { ' ' });
                    if (!Int32.TryParse(ss[0], out id))
                    {
                        SendMessage(sourceClientId, colorError + "Invalid arguments. Type /help to see command's usage.");
                        return;
                    }
                    if (ss.Length >= 2)
                    {
                        this.Ban(sourceClientId, id, string.Join(" ", ss, 1, ss.Length - 1));
                        return;
                    }
                    this.Ban(sourceClientId, id);
                    return;
                case "ban":
                    ss = argument.Split(new[] { ' ' });
                    if (ss.Length >= 2)
                    {
                        this.Ban(sourceClientId, ss[0], string.Join(" ", ss, 1, ss.Length - 1));
                        return;
                    }
                    this.Ban(sourceClientId, argument);
                    return;
                case "unban":
                    ss = argument.Split(new[] { ' ' });
                    if (ss.Length == 2)
                    {
                        this.Unban(sourceClientId, ss[0], ss[1]);
                        return;
                    }
                    SendMessage(sourceClientId, colorError + "Invalid arguments. Type /help to see command's usage.");
                    return;
                case "list":
                    this.List(sourceClientId, argument);
                    return;
                case "giveall":
                    this.GiveAll(sourceClientId, argument);
                    return;
                case "give":
                    ss = argument.Split(new[] { ' ' });
                    if (ss.Length == 3)
                    {
                        int amount;
                        if (!Int32.TryParse(ss[2], out amount))
                        {
                            SendMessage(sourceClientId, colorError + "Invalid arguments. Type /help to see command's usage.");
                            return;
                        }
                        else
                        {
                            this.Give(sourceClientId, ss[0], ss[1], amount);
                        }
                        return;
                    }
                    SendMessage(sourceClientId, colorError + "Invalid arguments. Type /help to see command's usage.");
                    return;
                case "monsters":
                    if (!argument.Equals("off") && !argument.Equals("on"))
                    {
                        SendMessage(sourceClientId, colorError + "Invalid arguments. Type /help to see command's usage.");
                        return;
                    }
                    this.Monsters(sourceClientId, argument);
                    return;
            /*
                     TODO: reimpliment
                case "areauser":
                    ss = argument.Split(new[] { ' ' });
                    if (ss.Length == 2)
                    {
                        this.AreaUser(sourceClientId, ss[0], ss[1]);
                        return;
                    }
                    SendMessage(sourceClientId, colorError + "Usage: /areauser [olduser] [newuser].");
                    return;
                case "addarea":
                    ss = argument.Split(new[] { ' ' });
                    if (ss.Length == 2)
                    {
                        this.AddArea(sourceClientId, ss[0], ss[1]);
                        return;
                    }
                    SendMessage(sourceClientId, colorError + "Usage: /addarea username coords.");
                    return;
                    */
                case "help":
                    this.Help(sourceClientId);
                    return;
                case "run":
                case "":
                    // JavaScript
                    // assume script expression or command coming
                    var script = argument;
                    RunInClientSandbox(script, sourceClientId);
                    return;
                case "crash":
                    KillPlayer(sourceClientId);
                    return;
                default:
                    SendMessage(sourceClientId, colorError + "Unknown command /" + command);
                    return;
            }
        }

        public void Help(int sourceClientId)
        {
            SendMessage(sourceClientId, colorHelp + "Available privileges:");
            foreach (ServerClientMisc.Privilege privilege in clients[sourceClientId].privileges)
            {
                SendMessage(sourceClientId,string.Format("{0}{1}: {2}",colorHelp, privilege.ToString(), this.CommandHelp(privilege.ToString())));
            }
        }

        private string CommandHelp(string command)
        {
            switch (command)
            {
                case "msg":
                case "pm":
                    return "/msg [username] [message]";
                case "kick":
                    return "/kick [username] {reason}";
                case "kick_id":
                    return "kick_id [player id] {reason}";
                case "ban":
                    return "/ban [username] {reason}";
                case "ban_id":
                    return "/ban_id [player id] {reason}";
                case "banip":
                    return "/banip [username] {reason}";
                case "banip_id":
                    return "/banip_id [player id] {reason}";
                case "unban":
                    return "/unban [-p playername | -ip ipaddress]";
                case "run":
                    return "/run [JavaScript (max. length 4096 char.)]";
                case "op":
                    return "/op [username] [group]";
                case "chgrp":
                    return "/chgrp [username] [group]";
                case "login":
                    return "/login [group] [password]";
                case "welcome":
                    return "/welcome [login motd message]";
                case "logging":
                    return "/logging [-s | -b | -se | -c] {on | off}";
                case "list_clients":
                    return "/list [-clients]";
                case "list_saved_clients":
                    return "/list [-saved_clients]";
                case "list_groups":
                    return "/list [-groups]";
                case "list_banned_users":
                    return "/list [-bannedusers | -bannedips]";
                case "list_areas":
                    return "/list [-areas]";
                case "give":
                    return "/give [username] blockname amount";
                case "giveall":
                    return "/giveall [username]";
                case "monsters":
                    return "/monsters [on|off]";
                case "announcement":
                    return "/announcement [message]";
                default:
                    return "No description available.";
            }
        }

        public bool PrivateMessage(int sourceClientId, string recipient, string message)
        {
            if (!clients[sourceClientId].privileges.Contains(ServerClientMisc.Privilege.pm))
            {
                SendMessage(sourceClientId, string.Format("{0}Insufficient privileges to access this command.", colorError));
                return false;
            }

            foreach (var k in clients)
            {
                if (k.Value.playername.Equals(recipient, StringComparison.InvariantCultureIgnoreCase))
                {
                    SendMessage(k.Key, string.Format("PM {0}: {1}", clients[sourceClientId].ColoredPlayername(colorNormal), message));
                    SendMessage(sourceClientId, string.Format("PM -> {0}: {1}", k.Value.ColoredPlayername(colorNormal), message));
                    // TODO: move message sound to client
                    //SendSound(k.Key, "message.wav");
                    return true;
                }
            }
            SendMessage(sourceClientId, string.Format("{0}Player {1} not found.", colorError, recipient));
            return false;
        }

        public bool ChangeGroup(int sourceClientId, string target, string newGroupName)
        {
            if (!clients[sourceClientId].privileges.Contains(ServerClientMisc.Privilege.chgrp))
            {
                SendMessage(sourceClientId, string.Format("{0}Insufficient privileges to access this command.", colorError));
                return false;
            }

            // Get related group from config file.
            GameModeFortress.Group newGroup = serverClient.Groups.Find(
                delegate(GameModeFortress.Group grp)
                {
                    return grp.Name.Equals(newGroupName);
                }
            );
            if (newGroup == null)
            {
                SendMessage(sourceClientId, string.Format("{0}Group {1} not found.", colorError, newGroupName));
                return false;
            }

            // Forbid to assign groups with levels higher then the source's client group level.
            if (newGroup.IsSuperior(clients[sourceClientId].clientGroup))
            {
                SendMessage(sourceClientId, string.Format("{0}The target group is superior your group.", colorError));
                return false;
            }

            // Get related client from config file
            GameModeFortress.Client clientConfig = serverClient.Clients.Find(
                delegate(GameModeFortress.Client client)
                {
                    return client.Name.Equals(target, StringComparison.InvariantCultureIgnoreCase);
                }
            );

            // Get related client.
            foreach (var k in clients)
            {
                if (k.Value.playername.Equals(target, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (k.Value.clientGroup.IsSuperior(clients[sourceClientId].clientGroup) || k.Value.clientGroup.EqualLevel(clients[sourceClientId].clientGroup))
                    {
                        SendMessage(sourceClientId, string.Format("{0}Target user is superior or equal.", colorError));
                        return false;
                    }
                    // Add or change group membership in config file.

                    // Client is not yet in config file. Create a new entry.
                    if (clientConfig == null)
                    {
                        clientConfig = new GameModeFortress.Client();
                        clientConfig.Name = k.Value.playername;
                        clientConfig.Group = newGroup.Name;
                        serverClient.Clients.Add(clientConfig);
                    }
                    else
                    {
                        clientConfig.Group = newGroup.Name;
                    }
                    SaveServerClient();
                    SendMessageToAll(string.Format("{0}New group for {1}: {2}", colorSuccess, k.Value.ColoredPlayername(colorSuccess), newGroup.GroupColorString() + newGroupName));
                    ServerEventLog(String.Format("{0} set group of {1} to {2}.", clients[sourceClientId].playername, k.Value.playername, newGroupName));
                    k.Value.AssignGroup(newGroup);
                    return true;
                }
            }

            // Target is at the moment not online. Create or change anyway a entry in ServerClient
            if (clientConfig == null)
            {
                clientConfig = new GameModeFortress.Client();
                clientConfig.Name = target;
                clientConfig.Group = newGroup.Name;
                serverClient.Clients.Add(clientConfig);
            }
            else
            {
                // Check if target's current group is superior.
                GameModeFortress.Group oldGroup = serverClient.Groups.Find(
                    delegate(GameModeFortress.Group grp)
                {
                    return grp.Name.Equals(clientConfig.Group);
                }
                );
                if (oldGroup == null)
                {
                    SendMessage(sourceClientId, string.Format("{0}Invalid group.", colorError));
                    return false;
                }
                if (oldGroup.IsSuperior(clients[sourceClientId].clientGroup) || oldGroup.EqualLevel(clients[sourceClientId].clientGroup))
                {
                    SendMessage(sourceClientId, string.Format("{0}Target user is superior or equal.", colorError));
                    return false;
                }
                clientConfig.Group = newGroup.Name;
            }

            SaveServerClient();
            SendMessageToAll(string.Format("{0}New group for {1}: {2} (offline)", colorSuccess, target, newGroup.GroupColorString() + newGroupName));
            ServerEventLog(String.Format("{0} set group of {1} to {2} (offline).", clients[sourceClientId].playername, target, newGroupName));
            return true;
        }

        public bool Login (int sourceClientId, string targetGroupString, string password)
        {
            if (!clients[sourceClientId].privileges.Contains(ServerClientMisc.Privilege.login))
            {
                SendMessage(sourceClientId, string.Format("{0}Insufficient privileges to access this command.", colorError));
                return false;
            }
            GameModeFortress.Group targetGroup = serverClient.Groups.Find(
                delegate(GameModeFortress.Group grp)
                {
                    return grp.Name.Equals(targetGroupString);
                }
            );
            if (targetGroup == null)
            {
                SendMessage(sourceClientId, string.Format("{0}Group {1} not found.", colorError, targetGroupString));
                return false;
            }
            if (string.IsNullOrEmpty(targetGroup.Password))
            {
                SendMessage(sourceClientId, string.Format("{0}Group {1} doesn't allow password access.", colorError, targetGroupString));
                return false;
            }
            if (targetGroup.Password.Equals(password))
            {
                clients[sourceClientId].AssignGroup(targetGroup);
                SendMessageToAll(string.Format("{0}{1} logs in group {2}.", colorSuccess, clients[sourceClientId].ColoredPlayername(colorSuccess), targetGroupString));
                SendMessage(sourceClientId, "Type /help see your available privileges.");
                ServerEventLog(string.Format("{0} logs in group {1}.", clients[sourceClientId].playername, targetGroupString));
                return true;
            }
            SendMessage(sourceClientId, string.Format("{0}Invalid password.", colorError));
            ServerEventLog(string.Format("{0} fails to log in (invalid password: {1}).", clients[sourceClientId].playername, password));
            return false;
        }

        public bool WelcomeMessage(int sourceClientId, string welcomeMessage)
        {
            if (!clients[sourceClientId].privileges.Contains(ServerClientMisc.Privilege.welcome))
            {
                SendMessage(sourceClientId, string.Format("{0}Insufficient privileges to access this command.", colorError));
                return false;
            }
            config.WelcomeMessage = welcomeMessage;
            SendMessageToAll(string.Format("{0}{1} set new welcome message: {2}", colorSuccess, clients[sourceClientId].ColoredPlayername(colorSuccess), welcomeMessage));
            ServerEventLog(string.Format("{0} changes welcome message to {1}.", clients[sourceClientId].playername, welcomeMessage));
            SaveConfig();
            return true;
        }

        public bool SetLogging(int sourceClientId, string type, string option)
        {
            if (!clients[sourceClientId].privileges.Contains(ServerClientMisc.Privilege.logging))
            {
                SendMessage(sourceClientId, string.Format("{0}Insufficient privileges to access this command.", colorError));
                return false;
            }
            switch (type)
            {
            // all logging state
                case "-s":
                    SendMessage(sourceClientId, "Build: " + config.BuildLogging);
                    SendMessage(sourceClientId, "Server events: " + config.ServerEventLogging);
                    SendMessage(sourceClientId, "Chat: " + config.ChatLogging);
                    return true;
                case "-b":
                    if (option.Equals("on"))
                    {
                        config.BuildLogging = true;
                        SaveConfig();
                        SendMessage(sourceClientId, string.Format("{0}Build logging enabled.", colorSuccess));
                        ServerEventLog(string.Format("{0} enables build logging.", clients[sourceClientId].playername));
                        return true;
                    }
                    if (option.Equals("off"))
                    {
                        config.BuildLogging = false;
                        SaveConfig();
                        SendMessage(sourceClientId, string.Format("{0}Build logging disabled.", colorSuccess));
                        ServerEventLog(string.Format("{0} disables build logging.", clients[sourceClientId].playername));
                        return true;
                    }
                    SendMessage(sourceClientId, string.Format("{0}Build logging: {1}", colorNormal, config.BuildLogging));
                    return true;
                case "-se":
                    if (option.Equals("on"))
                    {
                        config.ServerEventLogging = true;
                        SaveConfig();
                        SendMessage(sourceClientId, string.Format("{0}Server event logging enabled.", colorSuccess));
                        ServerEventLog(string.Format("{0} enables server event logging.", clients[sourceClientId].playername));
                        return true;
                    }
                    if (option.Equals("off"))
                    {
                        ServerEventLog(string.Format("{0} disables server event logging.", clients[sourceClientId].playername));
                        config.ServerEventLogging = false;
                        SaveConfig();
                        SendMessage(sourceClientId, string.Format("{0}Server event logging disabled.", colorSuccess));
                        return true;
                    }
                    SendMessage(sourceClientId, string.Format("{0}Server event logging: {1}", colorNormal, config.ServerEventLogging));
                    return true;
                case "-c":
                    if (option.Equals("on"))
                    {
                        config.ChatLogging = true;
                        SaveConfig();
                        SendMessage(sourceClientId, string.Format("{0}Chat logging enabled.", colorSuccess));
                        ServerEventLog(string.Format("{0} enables chat logging.", clients[sourceClientId].playername));
                        return true;
                    }
                    if (option.Equals("off"))
                    {
                        config.ChatLogging = false;
                        SaveConfig();
                        SendMessage(sourceClientId, string.Format("{0}Chat logging disabled.", colorSuccess));
                        ServerEventLog(string.Format("{0} disables chat logging.", clients[sourceClientId].playername));
                        return true;
                    }
                    SendMessage(sourceClientId, string.Format("{0}Chat logging: {1}", colorNormal, config.ChatLogging));
                    return true;
                default:
                    SendMessage(sourceClientId, string.Format("{0}Invalid type: {1}", colorError, type));
                    return false;
            }
        }

        public bool Kick(int sourceClientId, string target)
        {
            return Kick(sourceClientId, target, "");
        }

        public bool Kick(int sourceClientId, string target, string reason)
        {
            foreach (var k in clients)
            {
                if (k.Value.playername.Equals(target, StringComparison.InvariantCultureIgnoreCase))
                {
                    return this.Kick(sourceClientId, k.Key, reason);
                }
            }
            SendMessage(sourceClientId, string.Format("{0}Player {1} not found.", colorError, target));
            return false;
        }

        public bool Kick(int sourceClientId, int targetClientId)
        {
            return this.Kick(sourceClientId, targetClientId, "");
        }

        public bool Kick(int sourceClientId, int targetClientId, string reason)
        {
            if (!clients[sourceClientId].privileges.Contains(ServerClientMisc.Privilege.kick))
            {
                SendMessage(sourceClientId, string.Format("{0}Insufficient privileges to access this command.", colorError));
                return false;
            }
            if (!reason.Equals(""))
            {
                reason = " Reason: " + reason;
            }
            if (clients.ContainsKey(targetClientId))
            {
                if (clients[targetClientId].clientGroup.IsSuperior(clients[sourceClientId].clientGroup) || clients[targetClientId].clientGroup.EqualLevel(clients[sourceClientId].clientGroup))
                {
                    SendMessage(sourceClientId, string.Format("{0}Target is superior or equal.", colorError));
                    return false;
                }
                string targetName = clients[targetClientId].playername;
                string sourceName = clients[sourceClientId].playername;
                string targetNameColored = clients[targetClientId].ColoredPlayername(colorImportant);
                string sourceNameColored = clients[sourceClientId].ColoredPlayername(colorImportant);
                SendMessageToAll(string.Format("{0}{1} was kicked by {2}", colorImportant, targetNameColored, sourceNameColored));
                ServerEventLog(string.Format("{0} kicks {1}.{2}", sourceName, targetName, reason));
                SendDisconnectPlayer(targetClientId, string.Format("You were kicked by an administrator.{0}", reason));
                KillPlayer(targetClientId);
                return true;
            }
            SendMessage(sourceClientId, string.Format("{0}Player ID {1} does not exist.", colorError, targetClientId));
            return false;
        }

        public bool Ban(int sourceClientId, string target)
        {
            return Ban(sourceClientId, target, "");
        }

        public bool Ban(int sourceClientId, string target, string reason)
        {
            foreach (var k in clients)
            {
                if (k.Value.playername.Equals(target, StringComparison.InvariantCultureIgnoreCase))
                {
                    return this.Ban(sourceClientId, k.Key, reason);
                }
            }
            SendMessage(sourceClientId, string.Format("{0}Player {1} not found.", colorError, target));
            return false;
        }

        public bool Ban(int sourceClientId, int targetClientId)
        {
            return this.Ban(sourceClientId, targetClientId, "");
        }

        public bool Ban(int sourceClientId, int targetClientId, string reason)
        {
            if (!clients[sourceClientId].privileges.Contains(ServerClientMisc.Privilege.ban))
            {
                SendMessage(sourceClientId, string.Format("{0}Insufficient privileges to access this command.", colorError));
                return false;
            }
            if (!reason.Equals(""))
            {
                reason = " Reason: " + reason;
            }
            if (clients.ContainsKey(targetClientId))
            {
                if (clients[targetClientId].clientGroup.IsSuperior(clients[sourceClientId].clientGroup) || clients[targetClientId].clientGroup.EqualLevel(clients[sourceClientId].clientGroup))
                {
                    SendMessage(sourceClientId, string.Format("{0}Target is superior or equal.", colorError));
                    return false;
                }
                string targetName = clients[targetClientId].playername;
                string sourceName = clients[sourceClientId].playername;
                string targetNameColored = clients[targetClientId].ColoredPlayername(colorImportant);
                string sourceNameColored = clients[sourceClientId].ColoredPlayername(colorImportant);
                config.BannedUsers.Add(targetName);
                SaveConfig();
                SendMessageToAll(string.Format("{0}{1} was banned by {2}", colorImportant, targetNameColored, sourceNameColored));
                ServerEventLog(string.Format("{0} bans {1}.{2}", sourceName, targetName, reason));
                SendDisconnectPlayer(targetClientId, string.Format("You were banned by an administrator.{0}", reason));
                KillPlayer(targetClientId);
                return true;
            }
            SendMessage(sourceClientId, string.Format("{0}Player ID {1} does not exist.", colorError, targetClientId));
            return false;
        }

        public bool BanIP(int sourceClientId, string target)
        {
            return BanIP(sourceClientId, target, "");
        }

        public bool BanIP(int sourceClientId, string target, string reason)
        {
            foreach (var k in clients)
            {
                if (k.Value.playername.Equals(target, StringComparison.InvariantCultureIgnoreCase))
                {
                    return this.BanIP(sourceClientId, k.Key, reason);
                }
            }
            SendMessage(sourceClientId, string.Format("{0}Player {1} not found.", colorError, target));
            return false;
        }

        public bool BanIP(int sourceClientId, int targetClientId)
        {
            return this.BanIP(sourceClientId, targetClientId, "");
        }

        public bool BanIP(int sourceClientId, int targetClientId, string reason)
        {
            if (!clients[sourceClientId].privileges.Contains(ServerClientMisc.Privilege.banip))
            {
                SendMessage(sourceClientId, string.Format("{0}Insufficient privileges to access this command.", colorError));
                return false;
            }
            if (!reason.Equals(""))
            {
                reason = " Reason: " + reason;
            }
            if (clients.ContainsKey(targetClientId))
            {
                if (clients[targetClientId].clientGroup.IsSuperior(clients[sourceClientId].clientGroup) || clients[targetClientId].clientGroup.EqualLevel(clients[sourceClientId].clientGroup))
                {
                    SendMessage(sourceClientId, string.Format("{0}Target is superior or equal.", colorError));
                    return false;
                }
                string targetName = clients[targetClientId].playername;
                string sourceName = clients[sourceClientId].playername;
                string targetNameColored = clients[targetClientId].ColoredPlayername(colorImportant);
                string sourceNameColored = clients[sourceClientId].ColoredPlayername(colorImportant);
                config.BannedIPs.Add(((IPEndPoint)clients[targetClientId].socket.RemoteEndPoint).Address.ToString());
                SaveConfig();
                SendMessageToAll(string.Format("{0}{1} was IP banned by {2}", colorImportant, targetNameColored, sourceNameColored));
                ServerEventLog(string.Format("{0} IP bans {1}.{2}", sourceName, targetName, reason));
                SendDisconnectPlayer(targetClientId, string.Format("You were IP banned by an administrator.{0}", reason));
                KillPlayer(targetClientId);
                return true;
            }
            SendMessage(sourceClientId, string.Format("{0}Player ID {1} does not exist.", colorError, targetClientId));
            return false;
        }

        public bool Unban(int sourceClientId, string type, string target)
        {
            if (!clients[sourceClientId].privileges.Contains(ServerClientMisc.Privilege.unban))
            {
                SendMessage(sourceClientId, string.Format("{0}Insufficient privileges to access this command.", colorError));
                return false;
            }
            // unban a playername
            if (type.Equals("-p"))
            {
                // case insensitive
                bool exists = config.UnbanPlayer(target);
                SaveConfig();
                if (!exists)
                {
                    SendMessage(sourceClientId, string.Format("{0}Player {1} not found.", colorError, target));
                }
                else
                {
                    SendMessage(sourceClientId, string.Format("{0}Player {1} unbanned.", colorSuccess, target));
                    ServerEventLog(string.Format("{0} unbans player {1}.", clients[sourceClientId].playername, target));
                }
                return true;
            }
            // unban an IP
            else if (type.Equals("-ip"))
            {
                bool exists = config.BannedIPs.Remove(target);
                SaveConfig();
                if (!exists)
                {
                    SendMessage(sourceClientId, string.Format("{0}IP {1} not found.", colorError, target));
                }
                else
                {
                    SendMessage(sourceClientId, string.Format("{0}IP {1} unbanned.", colorSuccess, target));
                    ServerEventLog(string.Format("{0} unbans IP {1}.", clients[sourceClientId].playername, target));
                }
                return true;
            }
            SendMessage(sourceClientId, string.Format("{0}Invalid type: {1}", colorError, type));
            return false;
        }

        public bool List(int sourceClientId, string type)
        {
            switch (type)
            {
                case "-clients":
                case "-c":
                    if (!clients[sourceClientId].privileges.Contains(ServerClientMisc.Privilege.list_clients))
                    {
                        SendMessage(sourceClientId, string.Format("{0}Insufficient privileges to access this command.", colorError));
                        return false;
                    }
                    SendMessage(sourceClientId, colorImportant + "List of Players:");
                    foreach (var k in clients)
                    {
                        // Format: Key Playername:Group:Privileges IP
                        SendMessage(sourceClientId, string.Format("{0} {1}", k.Key, k.Value.ToString()));
                    }
                    return true;
                case "-areas":
                case "-a":
                    if (!clients[sourceClientId].privileges.Contains(ServerClientMisc.Privilege.list_areas))
                    {
                        SendMessage(sourceClientId, string.Format("{0}Insufficient privileges to access this command.", colorError));
                        return false;
                    }
                    SendMessage(sourceClientId, colorImportant + "List of Areas:");
                    foreach (var area in config.Areas)
                    {
                        SendMessage(sourceClientId, area.ToString());
                    }
                    return true;
                case "-bannedusers":
                case "-bu":
                    if (!clients[sourceClientId].privileges.Contains(ServerClientMisc.Privilege.list_banned_users))
                    {
                        SendMessage(sourceClientId, string.Format("{0}Insufficient privileges to access this command.", colorError));
                        return false;
                    }
                    SendMessage(sourceClientId, colorImportant + "List of Banned Users:");
                    foreach (string currentUser in config.BannedUsers)
                    {
                        SendMessage(sourceClientId, currentUser);
                    }
                    return true;
                case "-bannedips":
                case "-bip":
                    if (!clients[sourceClientId].privileges.Contains(ServerClientMisc.Privilege.list_banned_users))
                    {
                        SendMessage(sourceClientId, string.Format("{0}Insufficient privileges to access this command.", colorError));
                        return false;
                    }
                    SendMessage(sourceClientId, colorImportant + "List of Banned IPs:");
                    foreach (string currentIP in config.BannedIPs)
                    {
                        SendMessage(sourceClientId, currentIP);
                    }
                    return true;
                case "-groups":
                    if (!clients[sourceClientId].privileges.Contains(ServerClientMisc.Privilege.list_groups))
                    {
                        SendMessage(sourceClientId, string.Format("{0}Insufficient privileges to access this command.", colorError));
                        return false;
                    }
                    SendMessage(sourceClientId, colorImportant + "List of groups:");
                    foreach (GameModeFortress.Group currenGroup in serverClient.Groups)
                    {
                        SendMessage(sourceClientId, currenGroup.ToString());
                    }
                    return true;
                case "-saved_clients":
                    if (!clients[sourceClientId].privileges.Contains(ServerClientMisc.Privilege.list_saved_clients))
                    {
                        SendMessage(sourceClientId, string.Format("{0}Insufficient privileges to access this command.", colorError));
                        return false;
                    }
                    SendMessage(sourceClientId, colorImportant + "List of saved clients:");
                    foreach (GameModeFortress.Client currenClient in serverClient.Clients)
                    {

                        SendMessage(sourceClientId, currenClient.ToString());
                    }
                    return true;
                default:
                    SendMessage(sourceClientId, "Invalid parameter.");
                    return false;
            }
        }

        public bool GiveAll(int sourceClientId, string target)
        {
            if (!clients[sourceClientId].privileges.Contains(ServerClientMisc.Privilege.giveall))
            {
                SendMessage(sourceClientId, string.Format("{0}Insufficient privileges to access this command.", colorError));
                return false;
            }
            foreach (var k in clients)
            {
                if (k.Value.playername.Equals(target, StringComparison.InvariantCultureIgnoreCase))
                {
                    string targetName = k.Value.playername;
                    string sourcename = clients[sourceClientId].playername;
                    for (int i = 0; i < d_Data.IsBuildable.Length; i++)
                    {
                        if (!d_Data.IsBuildable[i])
                        {
                            continue;
                        }
                        Inventory inventory = GetPlayerInventory(targetName).Inventory;
                        InventoryUtil util = GetInventoryUtil(inventory);

                        for (int xx = 0; xx < util.CellCount.X; xx++)
                        {
                            for (int yy = 0; yy < util.CellCount.Y; yy++)
                            {
                                if (!inventory.Items.ContainsKey(new ProtoPoint(xx, yy)))
                                {
                                    continue;
                                }
                                Item currentItem = inventory.Items[new ProtoPoint(xx, yy)];
                                if (currentItem != null
                                    && currentItem.ItemClass == ItemClass.Block
                                    && currentItem.BlockId == i)
                                {
                                    currentItem.BlockCount = 999;
                                    goto nextblock;
                                }
                            }
                        }
                        for (int xx = 0; xx < util.CellCount.X; xx++)
                        {
                            for (int yy = 0; yy < util.CellCount.Y; yy++)
                            {
                                Item newItem = new Item();
                                newItem.ItemClass = ItemClass.Block;
                                newItem.BlockId = i;
                                newItem.BlockCount = 999;

                                if (util.ItemAtCell(new Point(xx, yy)) == null)
                                {
                                    inventory.Items[new ProtoPoint(xx, yy)] = newItem;
                                    goto nextblock;
                                }
                            }
                        }
                    nextblock:
                        k.Value.IsInventoryDirty = true;
                    }
                    ServerEventLog(string.Format("{0} gives all to {1}.", sourcename, targetName));
                    return true;
                }
            }
            SendMessage(sourceClientId, string.Format("{0}Player {1} not found.", colorError, target));
            return false;
        }

        public bool Give(int sourceClientId, string target, string blockname, int amount)
        {
            if (!clients[sourceClientId].privileges.Contains(ServerClientMisc.Privilege.give))
            {
                SendMessage(sourceClientId, string.Format("{0}Insufficient privileges to access this command.", colorError));
                return false;
            }

            foreach (var k in clients)
            {

                if (k.Value.playername.Equals(target, StringComparison.InvariantCultureIgnoreCase))
                {
                    string targetName = k.Value.playername;
                    string sourcename = clients[sourceClientId].playername;
                    //int amount;
                    if (amount < 0)
                    {
                        return false;
                    }
                    if (amount > 9999)
                    {
                        amount = 9999;
                    }
                    for (int i = 0; i < d_Data.IsBuildable.Length; i++)
                    {
                        if (!d_Data.IsBuildable[i])
                        {
                            continue;
                        }
                        if (!d_Data.Name[i].Equals(blockname, StringComparison.InvariantCultureIgnoreCase))
                        {
                            continue;
                        }
                        Inventory inventory = GetPlayerInventory(targetName).Inventory;
                        InventoryUtil util = GetInventoryUtil(inventory);
    
                        for (int xx = 0; xx < util.CellCount.X; xx++)
                        {
                            for (int yy = 0; yy < util.CellCount.Y; yy++)
                            {
                                if (!inventory.Items.ContainsKey(new ProtoPoint(xx, yy)))
                                {
                                    continue;
                                }
                                Item currentItem = inventory.Items[new ProtoPoint(xx, yy)];
                                if (currentItem != null
                                     && currentItem.ItemClass == ItemClass.Block
                                     && currentItem.BlockId == i)
                                {
                                    if (amount == 0)
                                    {
                                        inventory.Items[new ProtoPoint(xx, yy)] = null;
                                    }
                                    else
                                    {
                                        currentItem.BlockCount = amount;
                                    }
                                    goto nextblock;
                                }
                            }
                        }
                        for (int xx = 0; xx < util.CellCount.X; xx++)
                        {
                            for (int yy = 0; yy < util.CellCount.Y; yy++)
                            {
                                Item newItem = new Item();
                                newItem.ItemClass = ItemClass.Block;
                                newItem.BlockId = i;
                                newItem.BlockCount = amount;
    
                                if (util.ItemAtCell(new Point(xx, yy)) == null)
                                {
                                    inventory.Items[new ProtoPoint(xx, yy)] = newItem;
                                    goto nextblock;
                                }
                            }
                        }
                     nextblock:
                        k.Value.IsInventoryDirty = true;
                    }
                    ServerEventLog(string.Format("{0} gives {1} {2} to {3}.", sourcename, amount, blockname, targetName));
                    return true;
                }
            }
            SendMessage(sourceClientId, string.Format("{0}Player {1} not found.", colorError, target));
            return false;
        }

        public bool Monsters(int sourceClientId, string option)
        {
            if (!clients[sourceClientId].privileges.Contains(ServerClientMisc.Privilege.monsters))
            {
                SendMessage(sourceClientId, string.Format("{0}Insufficient privileges to access this command.", colorError));
                return false;
            }
            config.Monsters = option.Equals("off") ? false : true;
            SaveConfig();
            if (!config.Monsters)
            {
                foreach (var k in clients)
                {
                    SendPacket(k.Key, Serialize(new PacketServer()
                        {
                         PacketId = ServerPacketId.RemoveMonsters
                        }));
                }
            }
            SendMessage(sourceClientId, colorSuccess + "Monsters turned " + option);
            ServerEventLog(string.Format("{0} turns monsters {1}.", clients[sourceClientId].playername, option));
            return true;
        }

        /*
        TODO: Reimplement
        public bool AreaChangeUser (int sourceClientId, string oldUser, string newUser)
        {
            if (!clients[sourceClientId].privileges.Contains(Privilege.AreaChangeUser))
            {
                SendMessage(sourceClientId, string.Format("{0}Insufficient privileges to access this command.", colorError));
                return false;
            }

            foreach (AreaConfig area in config.Areas)
            {
                if (area.PermittedUsers.Equals(oldUser, StringComparison.InvariantCultureIgnoreCase))
                {
                    area.PermittedUsers = newUser;
                    SaveConfig();
                    SendMessage(sourceClientId, "Area changed.");
                    ServerEventLog(string.Format("{0} changes area from {1} to {2}.", clients[sourceClientId].playername, oldUser, newUser));
                    return true;
                }
            }
            return false;
        }
        public bool AreaChangeGroup (int sourceClientId, string oldGroup, string newGroup)
        {
            if (!clients[sourceClientId].privileges.Contains(Privilege.AreaChangeGroup))
            {
                SendMessage(sourceClientId, string.Format("{0}Insufficient privileges to access this command.", colorError));
                return false;
            }

            foreach (AreaConfig area in config.Areas)
            {
                if (area.PermittedUsers.Equals(oldUser, StringComparison.InvariantCultureIgnoreCase))
                {
                    area.PermittedGroups = newUser;
                    SaveConfig();
                    SendMessage(sourceClientId, "Area changed.");
                    ServerEventLog(string.Format("{0} changes area from {1} to {2}.", clients[sourceClientId].playername, oldUser, newUser));
                    return true;
                }
            }
            return false;
        }
        public bool AreaAdd (int sourceClientId, string permittedUsers, string coords)
        {
            if (!clients[sourceClientId].privileges.Contains(Privilege.AreaAdd))
            {
                SendMessage(sourceClientId, string.Format("{0}Insufficient privileges to access this command.", colorError));
                return false;
            }
            config.Areas.Add(new AreaConfig(){PermittedUsers = permittedUsers, Coords = coords});
            SaveConfig();
            SendMessage(sourceClientId, "Area added.");
            ServerEventLog(string.Format("{0} adds area {2} {1}.", clients[sourceClientId].playername, permittedUsers, coords));
            return true;
        }
        */

        public bool Announcement(int sourceClientId, string message)
        {
            if (!clients[sourceClientId].privileges.Contains(ServerClientMisc.Privilege.announcement))
            {
                SendMessage(sourceClientId, string.Format("{0}Insufficient privileges to access this command.", colorError));
                return false;
            }
            SendMessageToAll(string.Format("{0}Announcenment: {1}", colorError, message));
            return true;
        }

        public bool ClearInterpreter(int sourceClientId)
        {
            if (!clients[sourceClientId].privileges.Contains(ServerClientMisc.Privilege.run))
            {
                SendMessage(sourceClientId, string.Format("{0}Insufficient privileges to access this command.", colorError));
                return false;
            }
            clients[sourceClientId].Interpreter = null;
            SendMessage(sourceClientId, "Interpreter cleared.");
            return true;
        }
    }
}