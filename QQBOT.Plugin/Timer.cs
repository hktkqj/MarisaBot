﻿using QQBot.EntityFrameworkCore;
using QQBot.MiraiHttp;
using QQBot.MiraiHttp.Entity;
using QQBot.MiraiHttp.Plugin;
using QQBOT.Plugin.Shared.Util;

namespace QQBot.Plugin
{
    public class Timer : MiraiPluginBase
    {
        protected override async Task<MiraiPluginTaskState> FriendMessageHandler(MiraiHttpSession session, Message message)
        {
            var mc = message.MessageChain!.PlainText;

            await using var dbContext = new BotDbContext();
            const string    command1  = ":ts";
            const string    command2  = ":te";
            var             uid       = message.Sender!.Id;

            switch (mc)
            {
                case command1:
                {
                    var last = mc.TrimStart(command1)!.Trim();

                    if (dbContext.Timers.Any(t => t.Uid == uid && t.Name == last && t.TimeEnd == null))
                    {
                        await session.SendFriendMessage(
                            new Message(MessageChain.FromPlainText($"Timer `{last}` already started")), uid);
                    }
                    else
                    {
                        var time = DateTime.Now;

                        dbContext.Timers.Add(new EntityFrameworkCore.Entity.Plugin.Timer
                        {
                            TimeBegin = time,
                            TimeEnd   = null,
                            Uid       = uid,
                            Name      = last
                        });

                        await dbContext.SaveChangesAsync();
                        await session.SendFriendMessage(
                            new Message(
                                MessageChain.FromPlainText($"Timer `{last}` started: {time:yyyy-MM-dd hh:mm:ss fff}")),
                            uid);
                    }

                    return MiraiPluginTaskState.CompletedTask;
                }
                case command2:
                {
                    var last = mc.TrimStart(command2)!.Trim();

                    var res = dbContext.Timers.Where(t => t.Uid == uid && t.TimeEnd == null && t.Name == last);

                    if (res.Any())
                    {
                        var update = res.First();
                        var time   = DateTime.Now;
                        update.TimeEnd = time;
                        dbContext.Update(update);

                        await dbContext.SaveChangesAsync();
                        await session.SendFriendMessage(
                            new Message(
                                MessageChain.FromPlainText(
                                    $"Timer `{last}` ended, duration: {time - update.TimeBegin:dd\\.hh\\:mm\\:ss}")),
                            uid);
                    }
                    else
                    {
                        await session.SendFriendMessage(
                            new Message(MessageChain.FromPlainText($"Timer `{last}` already ended")), uid);
                    }

                    return MiraiPluginTaskState.CompletedTask;
                }
                default:
                    return MiraiPluginTaskState.NoResponse;
            }
        }
    }
}