﻿using System.Collections.Generic;
using Pes7BotCrator.Type;
using Telegram.Bot.Types;

namespace Pic_finder
{
    public class micro_logic:Module
    {
        public micro_logic():base("Robot micro logic", typeof(micro_logic)) { }

        public async void SayHello(Message msg, IBot serving, List<ArgC> args)
        {
            await serving.Client.SendTextMessageAsync(msg.Chat.Id, @"Hi, I'm a Telegram bot which get's an image's from service's ""yande.re"", ""Danbooru"" and ""Gelbooru"".
To adjust a criteria of a search, you can use a parametric system, which consist's on ""Argument:Value"" combination.
For example you can type: ""/getyandere -limit:100 -tag:loli -tag:feet -page:2""(without qoutation marks) to enjoy with an another 100 pairs of lolie's feet's. ;-)
If you want to get picture as file, just type with other parameter's ""-file"".
If you a bad guy or a girl, type a ""-show_any"", to see as photo's an explicit pic's.
Also you can use a ""named"" commands.
For Example you can just type ""anipic yandere"", in a chat with bot, to avoid of using a slash symbol.
If you need more about commands, you can call /help.");
        }

        public async void Help(Message msg, IBot serving, List<ArgC> args)
        {
            await serving.Client.SendTextMessageAsync(msg.Chat.Id, "Some help.\n" +
                "Bot can run default slash (via '/') commands, and \"named\".\n" +
                "If with first it's anything is clear, with second you need to do next.\n" +
                "To make make Bot execute a command, first of all you need to type \"anipic\"(without qoutation marks) in message, before an actual command.\n" +
                "Then you need to type an actual phrase of it.\n" +
                "It should look like \"anipic <command>\"\n" +
                "Avalible named and usual commands are:\n" +
                " yandere, or /getyandere — gather pics from yande.re;\n" +
                " danbooru, or /getdanbooru — gather pics from Danbooru;\n" +
                " gelbooru, or /getgelbooru — gather pics from Gelbooru;\n" +
                " yandere_tags, or /getyandere_tags — gather names of avalible tags on yande.re;\n" +
                " danbooru_tags, or /getdanbooru_tags — gather names of avalible tags on Danbooru;\n" +
                " gelbooru_tags, or /getgelbooru_tags — gather names of avalible tags on Gelbooru;\n" +
                " sauce — (experimental function) gets links to the pic, which was sent to the bot, with caption \"AniPic sauce\" (without qoutation marks) (requires SauceNAO account);\n" +
                " (Actually, you can just send a photo as private message to the bot, even forward someone\'s photo-message, and it will be do a search of original with default parameters.)\n" +
                " putkey or /putkey — puts your API-key of your SauceNAO account, which is in parameter \"key\";\n" +
                " deletekey or /deletekey — deletes API-key of your SauceNAO account from bot\'s database;\n" +
                " stats or /getstats — gets count of avalieble searches via SauceNAO.\n" +
                "To use parameters, to adjust search with commands, you need type them with an actual command.\n" +
                "With default commands you need to type parameter in this way: \"-<key>:<value>\"(without qoutation marks), if parameter doesn't have a value — -<key>.\n" +
                "If you use named command, just type \"<key>=<value>\"(without qoutation marks), without value – just a \"<key>\".\n" +
                "Avalible keys are:" +
                "limit — apply a count of results, which bot shoud returned;\n" +
                "tag – apply tag for a search, many tags can be combined with \'plus\' sign, like \"tag1+tag2\";\n" +
                "page – usually resluts are limited, so if you want more results from the query, you need to type it again, then apply this keys with value more than 1;\n" +
                "id – if you know an id number of the illustration, on certain service, you can apply this key;\n" +
                "show_any – bot doesn\'t send you pics, whic marked as \'explicit\', so you need to apply this key (wihtout a value), to make it do it;\n" +
                "file – if you need a pic saved in a file, to avoid a compression of Telegram, use this key (without a value).\n" +
                "unsimilar – parameter for sauce-searching, which shows not very similar images as results.\n" +
                "For a tags, most of this most of this keys are appliable, just use a \'name\' key, to ensure that neccesary tag is avalible.\n\n" +
                "How to get a SauceNAO API-key?\n" +
                "First, please go to the https://saucenao.com/user.php \n" +
                "Second, visit a https://saucenao.com/user.php?page=search-api and you will see the \"api key\" section with letters and numbers string.\n" +
                "That\'s what we need – copy it to the buffer.\n" +
                "Third, type a message to the bot \"AniPic putkey key=<your api-key>\"(without qoutation and less-more marks, just a key,̶ ̶a̶n̶d̶ ̶I̶ ̶t̶i̶r̶e̶d̶ ̶a̶ ̶l̶i̶t̶t̶l̶e̶ ̶t̶o̶ ̶r̶e̶p̶e̶a̶t̶ ̶t̶h̶a̶t̶) and send it.\n" +
                "Your ready to use this bot.\n\n" +
                "P.S. This bot doesn\'t refers to the SauceNAO officially, it's just made by a few enthusiasts.\nSo it would be great if you may support the main resource.\n" +
                "If you found an issue, please report it to the GitHub page in issues section, thank you.\n" +
                "https://github.com/pes7/Telegram-Bots-Library/tree/tedechan" /*+
                "P.P.S. This bot is writes messages, chats and users which addresed to him, to it\'s own database.\n" +
                "His developer swears by it\'s Hatsune Miku figure, that he wouldn\'t use it in evil aims,̶ ̶e̶x̶c̶e̶p̶t̶ ̶i̶n̶ ̶r̶o̶f̶l̶s̶ ̶;̶)̶ or send it to a third faces."*/);
        }

        public async void DeleteMyMessage(Message msg, IBot serving, List<ArgC> args)
        {
            if (msg.ReplyToMessage.From.Username == serving.Name) await serving.Client.DeleteMessageAsync(msg.Chat.Id, msg.MessageId);
            else await serving.Client.SendTextMessageAsync(msg.Chat.Id, "Sorry, but it\'s not my bussines.\n" +
                "I can delete only my own message.");
        }

        public async void EmExit(Message msg, IBot serving, List<ArgC> args)
        {
            //await serving.Client.DeleteMessageAsync(msg.Chat.Id, msg.MessageId);
            await serving.Client.SendTextMessageAsync(msg.Chat.Id, "Bot will got shut down.");
            System.Environment.Exit(0);
        }
    }
}