﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Pes7BotCrator.Type;
using Telegram.Bot.Types;

namespace Pes7BotCrator.Commands
{
    public class Help : SynkCommand
    {
        public Help() : base(Act, new List<string>() { "/help" }, commandName: "хелп", descr:"Список команд.") { }
        public static void Act(Message re, IBot Parent, List<ArgC> args)
        {
            Parent.Client.SendTextMessageAsync(re.Chat.Id,$"This bot[{Parent.Name}] was created with pes7's Bot Creator.");
            string coms = "";
            if (args == null || args.Count < 2)
            {
                foreach (SynkCommand sn in Parent.SynkCommands.Where(fn => fn.Type == TypeOfCommand.Standart && fn.TypeOfAccess == TypeOfAccess.Public && fn.CommandLine.First() != "Default"))
                {
                    if (sn.Description != null)
                    {
                        if (sn.CommandName != null)
                            coms += $"\n{sn.CommandLine.First()}[{sn.CommandName}] - {sn.Description}";
                        else
                            coms += $"\n{sn.CommandLine.First()} - {sn.Description}";
                    }
                    else
                        coms += $"\n{sn.CommandLine.First()}";
                }
                Parent.Client.SendTextMessageAsync(re.Chat.Id, $"/Команда[Строковое имя]-[Описание]: {coms}\nВводить команды двумя способами:\n/Команда -параметер:значение\nгачи Строковое имя ПАРАМЕТЕР 1 или/и ПАРАМЕТЕР 2\nКоманды могут быть вызваны без параметров.");
            }
            else
            {
                var d = args.Find(fn => fn.Name == "0");
                var arg = d == null ? args.Find(fn => fn.Name == "admin") : d;
                if(arg != null || arg?.Arg == "админ")
                {
                    Thread th = new Thread(async () =>
                    {
                        bool canwe = false;
                        if (re.Chat.Type != Telegram.Bot.Types.Enums.ChatType.Private)
                            canwe = await WebHook.IsAdminAsync(Parent, re.Chat.Id, re.From.Id);
                        else
                            canwe = true;
                        if (canwe)
                        {
                            foreach (SynkCommand sn in Parent.SynkCommands.Where(fn => fn.Type == TypeOfCommand.Standart && fn.TypeOfAccess == TypeOfAccess.Admin && fn.CommandLine.First() != "Default"))
                            {
                                if (sn.Description != null)
                                    coms += $"\n{sn.CommandLine.First()} - {sn.Description}";
                                else
                                    coms += $"\n{sn.CommandLine.First()}";
                            }
                            await Parent.Client.SendTextMessageAsync(re.Chat.Id, $"Commands: {coms}");
                        }
                    });
                    th.Start();
                }
            }
            
        }
    }
}
