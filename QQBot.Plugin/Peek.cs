﻿using System.Drawing;
using QQBot.MiraiHttp.DI;
using QQBot.MiraiHttp.Entity;
using QQBot.MiraiHttp.Plugin;
using QQBot.Plugin.Shared.Util;

namespace QQBot.Plugin;

[MiraiPluginCommand(":peek")]
[MiraiPluginTrigger(typeof(MiraiPluginTrigger), nameof(MiraiPluginTrigger.PlainTextTrigger))]
public class Peek : MiraiPluginBase
{
    private bool _peekEnabled = false;

    private static string CaptureScreen(bool hide = true)
    {
        const int w = 1440;
        const int h = 810;

        var bitmap = new Bitmap(2560, 1440);

        using (var g = Graphics.FromImage(bitmap))
        {
            g.CopyFromScreen(0, 0, 0, 0, bitmap.Size, CopyPixelOperation.SourceCopy);
        }

        return hide
            ? bitmap.Resize(w, h).Blur(new Rectangle(0, 0, w, h), 4).ToB64()
            : bitmap.ToB64();
    }

    [MiraiPluginCommand]
    private MiraiPluginTaskState Handler(Message message, MessageSenderProvider ms)
    {
        const long authorId = 642191352L;
        var        senderId = message.Sender!.Id;

        MessageChain chain;
        switch (message.Command)
        {
            // disable peek
            case "0" when senderId == authorId:
                _peekEnabled = false;
                chain        = MessageChain.FromPlainText("peek disabled");
                break;
            case "1" when senderId == authorId:
                _peekEnabled = true;
                chain        = MessageChain.FromPlainText("peek enabled");
                break;
            case "" when _peekEnabled:
                chain = MessageChain.FromImageB64(CaptureScreen(senderId != authorId));
                break;
            default:
                chain = MessageChain.FromPlainText("Denied");
                break;
        }
            
        ms.Reply(chain, message);

        return MiraiPluginTaskState.CompletedTask;
    }
}