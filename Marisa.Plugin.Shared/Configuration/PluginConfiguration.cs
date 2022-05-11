﻿#pragma warning disable CS8618

using YamlDotNet.Serialization;

namespace Marisa.Plugin.Shared.Configuration;

public class PluginConfiguration
{
    public string[] Chi { get; set; }

    public long[] Commander { get; set; }

    public string[] Dirty { get; set; }
    
    public TodayFortune Fortune { get; set; }
    
    [YamlMember(Alias = "maimai", ApplyNamingConventions = false)]
    public MaiMaiConfiguration MaiMai { get; set; }
    
    public ArcaeaConfiguration Arcaea { get; set; }
    
    public string ImageDatabasePath { get; set; }

    public string ImageDatabaseKanKanPath { get; set; }

    public string HelpPath { get; set; }

    public string FfmpegPath { get; set; }
}