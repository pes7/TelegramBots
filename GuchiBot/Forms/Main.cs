﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pes7BotCrator;
using Pes7BotCrator.Modules;
using Pes7BotCrator.Type;
using System.Threading;
using Pes7BotCrator.Commands;
using LuaAble;
using GuchiBot.Interface;
using GuchiBot.Commands;
using Pes7BotCrator.Modules.FunFunc;
using Pes7BotCrator.Modules.LikeDislikeModule;
using GuchiBot.Modules;
using System.Configuration;

namespace GuchiBot
{
    public partial class Main : Form
    {
        /*
         Долепить вывод инфы в ListView, туда также передать картинки юзверей что чатяться.
         Дописать сохранение сообщений и ошибков в отделые папки и файлы
        */
        
        public Bot Bot;
        private int Ms = 30000;
        private int CurTime = 0;

        public OLua lua; // Ради фана

        public static string PostToId = "@guchithread";

        public Main()
        {
            InitializeComponent();

            BotLogic bt = new BotLogic();

            /*
             * Нужно написать модуль автопоста, при том что туда будет попадать кастомная функция, а настройка будет производиться в интерфейсе проги. 
             * В опрос надо добавить параметрический ввод своего текста кнопок. Так же сделать не анонимное голосование.
             */
            Bot = new Bot(
                /*key - please put ur public bot key, u can put it directly here or on app.config*/
                key: ConfigurationManager.ConnectionStrings["botKey"].ConnectionString,
                name: "guchimuchibot",
                nameString: "Гачи",
                usernameofcreator:"nazarpes7",
                webmdir: "G:/WebServers/home/apirrrsseer.ru/www/List_down/video",
                gachiimage: "C:/Users/user/Desktop/GachiArch",
                modules: new List<IModule> {
                    new _2chModule(),
                    new SaveLoadModule(60,30*60),
                    new LikeDislikeModule("./like.bot"),
                    new VoteModule("./votes.bot","./voteslike.bot"),
                    new TransitFileModule("./Downloads"),
                    new Statistic(),
                    new TRM(),
                    new FunFunc("FunFunc/FaceYouZerro","FunFunc/Who/WhoTitles","FunFunc/Who/WhoAnswers","FunFunc/Trigger"),
                    new GuchiVoice()
                }
            );

            // Команды с строки UI
            lua = new OLua(Bot);

            Bot.SynkCommands.Add(Bot.GetModule<LikeDislikeModule>().Command);
            Bot.SynkCommands.Add(new Pes7BotCrator.Commands.Help());
            Bot.SynkCommands.Add(Bot.GetModule<Statistic>().CommandHelp);
            Bot.SynkCommands.Add(Bot.GetModule<Statistic>().CommandRuntime);
            Bot.SynkCommands.Add(new LogUlog(Bot,LogUlog.TypeOf.MessageWithNameAndChannelName,"Приветсвую тебя @{0} в нашем {1}!","Увы, наши пути расходятся..."));
            Bot.SynkCommands.Add(Bot.GetModule<VoteModule>().QueryCommand);
            Bot.SynkCommands.Add(Bot.GetModule<VoteModule>().CreateCommand);
            Bot.SynkCommands.Add(Bot.GetModule<TransitFileModule>().DownloadCommandSynk);
            Bot.SynkCommands.Add(new FindImageStick());
            Bot.SynkCommands.Add(Bot.GetModule<FunFunc>()._CommandElse);
            Bot.SynkCommands.Add(Bot.GetModule<FunFunc>()._CommandInf);
            Bot.SynkCommands.Add(Bot.GetModule<FunFunc>()._CommandGuchi);
            Bot.SynkCommands.Add(Bot.GetModule<FunFunc>()._CommandWhoAreU);
            Bot.SynkCommands.Add(Bot.GetModule<FunFunc>()._Triggered);
            Bot.SynkCommands.Add(Bot.GetModule<FunFunc>()._TrueFalse);
            Bot.SynkCommands.Add(Bot.GetModule<FunFunc>()._DvachRoll);
            Bot.SynkCommands.Add(Bot.GetModule<FunFunc>()._ChtoEto);
            Bot.SynkCommands.Add(Bot.GetModule<FunFunc>()._Otvetka);
            Bot.SynkCommands.Add(Bot.GetModule<TRM>()._SayAfterMe);
            Bot.SynkCommands.Add(Bot.GetModule<GuchiVoice>()._GVoice);
            Bot.SynkCommands.Add(Bot.GetModule<FunFunc>()._BossOfTheGym);
            Bot.SynkCommands.Add(Bot.GetModule<FunFunc>()._BossOfTheGymSide);
            Bot.SynkCommands.Add(Bot.GetModule<FunFunc>()._GiznIliMut);
            //Bot.SynkCommands.Add(Bot.GetModule<FunFunc>()._ActiveUsersMosaic);
            Bot.SynkCommands.Add(new SynkCommand(new WebmModule().WebmFuncForBot, new List<string>()
            {
                "/sendrandwebm"
            }, commandName:"картика", descr:"Webm с личной колекции."));
            Bot.SynkCommands.Add(new SynkCommand(bt.GachiAttakSynk, new List<string>()
            {
                "/gachiattak"
            },TypeOfAccess.Admin, commandName: "атака", descr:"Секретное оружие."));
            Bot.SynkCommands.Add(new SynkCommand(bt.GetGachiImageLogic, new List<string>()
            {
                "/sendrandimg"
            },descr:"Пикча с личной колекции"));
            Bot.SynkCommands.Add(new SynkCommand(Bot.GetModule<_2chModule>().get2chSmartRandWebm, new List<string>()
            {
                "/2ch"
            }, commandName: "двач", descr:"Пост webm в тред, Argc: `-a` если хотите аниме. `-c:` количество"));
            Bot.SynkCommands.Add(new SynkCommand(Bot.GetModule<_2chModule>().Ragenerated, new List<string>()
            {
                "/regenerate"
            },TypeOfAccess.Named,"парс",descr:"Перепарсить двач."));
            /*
            Bot.SynkCommands.Add(new SynkCommand(bt.GetArgkSynk, new List<string>()
            {
                "/testmemory"
            }, commandName: "парсдвача", descr: "Бот повторит за вами."));
            */
            Bot.SynkCommands.Add(new SynkCommand(bt.ArgMessage, new List<string>()
            {
                "/testparam"
            }, descr:"Выведет сообщение ботом с праметрами `-id` `-text`"));
            //Inline
            Bot.SynkCommands.Add(new SynkCommand(bt.InlineMenu,new List<string>() {"_noon"}));
            Bot.SynkCommands.Add(new SynkCommand(bt.AutoDelMessage, new List<string>()
            {
                "/adp"
            }, descr:"Auto deliting post `text` `time` - time of life."));
            Bot.SynkCommands.Add(new SynkCommand(bt.DefaultSynk, new List<string>()
            {
                "Default"
            }));
            Bot.SynkCommands.Add(new SynkCommand((Telegram.Bot.Types.Message ms, IBot parent, List<ArgC> args)=> {
                parent.Client.SendTextMessageAsync(ms.Chat.Id,"Слушаюсь, уже сплю...");
                Bot.GetModule<SaveLoadModule>().saveIt();
                Bot.Dispose();
                Application.Exit();
            }, new List<string>()
            {
                "_"
            },commandName:"спать", access:TypeOfAccess.Named, descr: "Бот ложиться спать."));
            Bot.SynkCommands.Add(new SynkCommand((Telegram.Bot.Types.Message ms, IBot parent, List<ArgC> args) => {
                parent.Client.SendTextMessageAsync(ms.Chat.Id, $"Это мой господин, {ms.From.FirstName} {ms.From.LastName}, мой создатель и повелитель анусов в Fate/KPI");
            }, new List<string>()
            {
                "/me"
            }, commandName: "я", access: TypeOfAccess.Named, descr: "СОЗДАТЕЛЬ БОТА"));
            //Example of TimeReley Photo Message. Ps: special for Mordvinov B.
            /*
            Bot.SynkCommands.Add(new SynkCommand(async (Telegram.Bot.Types.Message ms, IBot parent, List<ArgC> args) =>
            {
                try
                {
                    Stream st = System.IO.File.Open("./previews/14637724531700.webm.jpg", FileMode.Open);
                    await parent.GetModule<TRM>().SendTimeRelayPhotoAsynkAsync(parent.MessagesLast.First().Chat.Id, new FileToSend("fl.jpg", st), 10, "KEKUS");
                }
                catch { await parent.GetModule<TRM>().SendTimeRelayMessageAsynkAsync(ms.Chat.Id,"Error to send simple .jpg",10); }
            }, new List<string>() { "/kek" }));
            label1.Text = $"{Ms} ms";
            */

            Bot.GetModule<SaveLoadModule>().SaveActions.Add(Bot.GetModule<LikeDislikeModule>().Save);
            Bot.GetModule<SaveLoadModule>().SaveActions.Add(Bot.GetModule<VoteModule>().Save);
            Bot.GetModule<SaveLoadModule>().SaveActions.Add(Bot.GetModule<FunFunc>().SaveBosses);
            Bot.Start();
        }

        private async Task GetInf()
        {
            listBox1.Items.Clear();
            listBox1.Items.AddRange(Bot.getInfForList());
        }

        private bool Triger_Alife = true;
        private void Main_Load(object sender, EventArgs e)
        {
            textBox1.Text = Bot.WebmDir;
            textBox2.Text = Bot.GachiImage;
            textBox3.Text = Bot.PreViewDir;

            Thread timeTh = new Thread(() =>
            {
                while (Triger_Alife)
                {
                    InvokeUI(() => { GetInf(); });
                    Thread.Sleep(5000);
                }
            });
            timeTh.Start();
            label3.Text = $"Trafic to: {PostToId}";
        }


        private bool TimerTrigger = false;
        private void TimeMinus_Click(object sender, EventArgs e)
        {
            if (!TimerTrigger && Ms > 0)
            {
                Ms -= 1000;
                label1.Text = $"{Ms} ms";
            }
        }

        private void TimePlus_Click(object sender, EventArgs e)
        {
            if (!TimerTrigger)
            {
                Ms += 1000;
                label1.Text = $"{Ms} ms";
            }
        }

        private void TimeStart_Click(object sender, EventArgs e)
        {
            CurTime = 0;
            timer1.Start();
            pictureBox1.BackColor = Color.Green;
            label1.Text = $"{CurTime} sec";
            TimerTrigger = true;
        }

        private void TimePause_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            pictureBox1.BackColor = Color.Gray;
            label1.Text = $"{CurTime} sec";
        }

        private void TimeStop_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            pictureBox1.BackColor = Color.Red;
            label1.Text = $"{Ms} ms";
            TimerTrigger = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            CurTime++;
            TimerSynk();
            label1.Text = $"{CurTime} sec";
        }

        private int DurPreSet = 0;
        private void TimerSynk()
        {
            if (Bot != null)
            {
                if (CurTime >= Ms/1000)
                {
                    if (DurPreSet == 0)
                    {
                        if (_2chModule.WebmCountA > 0 && _2chModule.WebmCountW > 0)
                        {
                            _2chModule Ch = Bot.GetModule<_2chModule>();
                            if (!checkBox1.Checked)
                            {
                                int rd = Bot.Rand.Next(0, _2chModule.WebmCountW);
                                Ch.SendWebm(Bot, Ch.WebmsW[rd], PostToId);
                                Ch.WebmsW.RemoveAt(rd);
                                _2chModule.WebmCountW = Ch.WebmsW.Count;
                            }
                            else
                            {
                                int rd = Bot.Rand.Next(0, _2chModule.WebmCountA);
                                Ch.SendWebm(Bot, Ch.WebmsA[rd], PostToId);
                                Ch.WebmsA.RemoveAt(rd);
                                _2chModule.WebmCountA = Ch.WebmsA.Count;
                            }
                        }
                    }
                    else
                    {
                        if (_2chModule.WebmCountA > 0 && _2chModule.WebmCountW > 0)
                        {
                            _2chModule Ch = Bot.GetModule<_2chModule>();
                            if (!checkBox1.Checked)
                            {
                                var webms = Ch.WebmsW.Where(sn => sn.DurationSec < DurPreSet);
                                int rd = Bot.Rand.Next(0, webms.Count());
                                Ch.SendWebm(Bot, webms.ElementAt(rd), PostToId);
                                Ch.WebmsW.Remove(webms.ElementAt(rd));
                                _2chModule.WebmCountW = Ch.WebmsW.Count;
                            }
                            else
                            {
                                var webms = Ch.WebmsA.Where(sn => sn.DurationSec < DurPreSet);
                                int rd = Bot.Rand.Next(0, _2chModule.WebmCountA);
                                Ch.SendWebm(Bot, webms.ElementAt(rd), PostToId);
                                Ch.WebmsA.Remove(webms.ElementAt(rd));
                                _2chModule.WebmCountA = Ch.WebmsA.Count;
                            }
                        }
                    }
                    CurTime = 0;
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Bot.WebmDir = textBox1.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bot.GetModule<_2chModule>().ParseWebmsFromDvach(Bot);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            Triger_Alife = false;
            Bot.GetModule<SaveLoadModule>().saveIt();
            Bot.Dispose();
            base.OnFormClosed(e);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            List<dynamic> files = new List<dynamic>();
            List<Thread> downloadTh = new List<Thread>();

            Thread th = new Thread(async () =>
            {
                foreach (UserM us in Bot.ActiveUsers)
                {
                    await us.DownloadImageToDirectory(Bot);
                    Image mg = Image.FromFile($"./UserPhotoes/{us.Id}.jpg");
                    files.Add(new { id = us.Id, Image = mg });
                }

                foreach (Telegram.Bot.Types.Message ms in Bot.MessagesLast)
                {
                    var afs = await Bot.getFileFrom(ms.Photo.ToList().First().FileId);
                    InvokeUI(() =>
                    {
                        if (ms.Type == Telegram.Bot.Types.Enums.MessageType.Text)
                        {
                            MessageUI mu = new MessageUI(files.Find(fs => ms.From.Id == fs.id).Image, ms.Text)
                            {
                                Width = flowLayoutPanel1.Width - 25
                            };
                            flowLayoutPanel1.Controls.Add(mu);
                        }
                        
                        else if (ms.Type == Telegram.Bot.Types.Enums.MessageType.Photo)
                        {
                            MessageUIPhoto mu = new MessageUIPhoto(Image.FromStream(afs));
                            mu.Width = flowLayoutPanel1.Width - 25;
                            flowLayoutPanel1.Controls.Add(mu);
                        }
                        
                    });
                }
            });
            th.Start();
        }

        private void InvokeUI(Action a)
        {
            BeginInvoke(new MethodInvoker(a));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(Bot != null)
                Bot.Exceptions.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                lua.Lua.DoString(textBox4.Text);
            }
            catch { }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DurPreSet = int.Parse(textBox5.Text);
            }
            catch { DurPreSet = 0; }
        }
    }
}
