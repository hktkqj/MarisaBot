﻿using System.Drawing;
using QQBot.Plugin.Shared.Util;

namespace QQBot.Plugin.Shared.MaiMaiDx
{
    public class DxRating
    {
        public readonly long AdditionalRating;
        public readonly List<SongScore> DxScores = new();
        public readonly List<SongScore> SdScores = new();
        public readonly string Nickname;
        // public long Rating;
        // public string Username;


        public DxRating(dynamic data)
        {
            // Rating           = data.rating;
            AdditionalRating = data.additional_rating;
            Nickname         = data.nickname;
            foreach (var d in data.charts.dx) DxScores.Add(new SongScore(d));

            foreach (var d in data.charts.sd) SdScores.Add(new SongScore(d));

            DxScores = DxScores.OrderByDescending(s => s.Rating).ToList();
            SdScores = SdScores.OrderByDescending(s => s.Rating).ToList();
        }

        #region Drawer

        private static Bitmap GetScoreCard(SongScore score)
        {
            var color = new[]
            {
                Color.FromArgb(82, 231, 43),
                Color.FromArgb(255, 168, 1),
                Color.FromArgb(255, 90, 102),
                Color.FromArgb(198, 79, 228),
                Color.FromArgb(219, 170, 255)
            };

            var (coverBackground, coverBackgroundAvgColor) = ResourceManager.GetCoverBackground(score.Id);

            var card = new Bitmap(210, 40);

            using (var g = Graphics.FromImage(card))
            {
                // 歌曲类别：DX 和 标准
                g.DrawImage(ResourceManager.GetImage(score.Type == "DX" ? "type_deluxe.png" : "type_standard.png"), 0,
                    0);

                // FC 标志
                var fcImg = ResourceManager.GetImage(string.IsNullOrEmpty(score.Fc)
                    ? "icon_blank.png"
                    : $"icon_{score.Fc.ToLower()}.png", 32, 32);
                g.DrawImage(fcImg, 130, 0);


                // FS 标志
                var fsImg = ResourceManager.GetImage(string.IsNullOrEmpty(score.Fs)
                    ? "icon_blank.png"
                    : $"icon_{score.Fs.ToLower()}.png", 32, 32);
                g.DrawImage(fsImg, 170, 0);
            }

            var levelBar = new
            {
                PaddingLeft = 12,
                PaddingTop  = 17,
                Width       = 7,
                Height      = 167
            };

            using (var g = Graphics.FromImage(coverBackground))
            {
                var fontColor = new SolidBrush(coverBackgroundAvgColor.SelectFontColor());

                // 难度指示
                const int borderWidth = 1;
                g.FillRectangle(new SolidBrush(Color.White), levelBar.PaddingLeft - borderWidth,
                    levelBar.PaddingTop - borderWidth,
                    levelBar.Width + borderWidth * 2, levelBar.Height + borderWidth * 2);
                g.FillRectangle(new SolidBrush(color[score.LevelIdx]), levelBar.PaddingLeft, levelBar.PaddingTop,
                    levelBar.Width, levelBar.Height);

                // 歌曲标题
                using (var font = new Font("MotoyaLMaru", 27, FontStyle.Bold))
                {
                    var title                                                   = score.Title;
                    while (g.MeasureString(title, font).Width > 400 - 25) title = title[..^4] + "...";
                    g.DrawString(title, font, fontColor, 25, 15);
                }

                var achievement = score.Achievement.ToString("F4").Split('.');

                // 达成率整数部分
                using (var font = new Font("Consolas", 36))
                {
                    g.DrawString((score.Achievement < 100 ? "0" : "") + achievement[0], font, fontColor, 20, 52);
                }

                // 达成率小数部分
                using (var font = new Font("Consolas", 27))
                {
                    g.DrawString("." + achievement[1], font, fontColor, 105, 62);
                }

                var rank = ResourceManager.GetImage($"rank_{score.Rank.ToLower()}.png");

                // rank 标志
                g.DrawImage(rank.Resize(0.8), 25, 110);

                // 定数
                using (var font = new Font("Consolas", 12))
                {
                    g.DrawString("BASE", font, fontColor, 97, 110);
                    g.DrawString(score.Constant.ToString("F1"), font, fontColor, 97, 125);
                }

                // Rating
                using (var font = new Font("Consolas", 20))
                {
                    g.DrawString(">", font, fontColor, 140, 110);
                    g.DrawString(score.Rating.ToString(), font, fontColor, 162, 110);
                }

                // card
                g.DrawImage(card, 25, 155);
            }

            return coverBackground;
        }

        private Bitmap GetB40Card()
        {
            const int column = 5;
            const int row    = 8;

            const int cardWidth  = 400;
            const int cardHeight = 200;

            const int paddingH = 30;
            const int paddingV = 30;

            const int bgWidth  = cardWidth  * column + paddingH * (column + 1);
            const int bgHeight = cardHeight * row    + paddingV * (row    + 4);


            var background = new Bitmap(bgWidth, bgHeight);

            using (var g = Graphics.FromImage(background))
            {
                var pxInit = paddingH;
                var pyInit = paddingV;

                var px = pxInit;
                var py = pyInit;

                for (var i = 0; i < SdScores.Count; i++)
                {
                    g.DrawImage(GetScoreCard(SdScores[i]), px, py);

                    if ((i + 1) % 5 == 0)
                    {
                        px =  pxInit;
                        py += cardHeight + paddingV;
                    }
                    else
                    {
                        px += cardWidth + paddingH;
                    }
                }

                pxInit = paddingH;
                pyInit = cardHeight * 5 + paddingV * (6 + 3);

                g.FillRectangle(new SolidBrush(Color.FromArgb(120, 136, 136)),
                    new Rectangle(paddingH, pyInit - 2 * paddingV, bgWidth - 2 * paddingH, paddingV / 2));

                px = pxInit;
                py = pyInit;

                for (var i = 0; i < DxScores.Count; i++)
                {
                    g.DrawImage(GetScoreCard(DxScores[i]), px, py);

                    if ((i + 1) % 5 == 0)
                    {
                        px =  pxInit;
                        py += cardHeight + paddingV;
                    }
                    else
                    {
                        px += cardWidth + paddingH;
                    }
                }
            }

            return background;
        }

        private Bitmap GetRatingCard()
        {
            var rating    = DxScores.Sum(s => s.Rating) + SdScores.Sum(s => s.Rating);
            var addRating = AdditionalRating;
            var name      = Nickname;

            var r = rating + addRating;

            var num = r switch
            {
                < 8000 => r / 1000 + 1,
                < 8500 => 9,
                _      => 10L
            };

            // rating 部分
            var ratingCard = ResourceManager.GetImage($"rating_{num}.png");
            ratingCard = ratingCard.Resize(2);
            using (var g = Graphics.FromImage(ratingCard))
            {
                var ra = r.ToString().PadLeft(5, ' ');

                for (var i = ra.Length - 1; i >= 0; i--)
                {
                    if (ra[i] == ' ') break;
                    g.DrawImage(ResourceManager.GetImage($"num_{ra[i]}.png"), 170 + 29 * i, 20);
                }
            }

            ratingCard = ratingCard.Resize(1.4);

            // 名字
            var nameCard = new Bitmap(690, 140);
            using (var g = Graphics.FromImage(nameCard))
            {
                g.Clear(Color.White);

                var fontSize = 57;
                var font     = new Font("Consolas", fontSize, FontStyle.Bold);

                while (true)
                {
                    var w = g.MeasureString(name, font);

                    if (w.Width < 480)
                    {
                        g.DrawString(name, font, new SolidBrush(Color.Black), 20, (nameCard.Height - w.Height) / 2);
                        break;
                    }

                    fontSize -= 2;
                    font     =  new Font("Consolas", fontSize, FontStyle.Bold);
                }

                var dx = ResourceManager.GetImage("icon_dx.png");
                g.DrawImage(dx.Resize(3.2), 500, 10);
            }

            nameCard = nameCard.RoundCorners(20);

            // 称号（显示底分和段位）
            var rainbowCard = ResourceManager.GetImage("rainbow.png");
            rainbowCard = rainbowCard.Resize((double)nameCard.Width / rainbowCard.Width + 0.05);
            using (var g = Graphics.FromImage(rainbowCard))
            {
                using (var font = new Font("MotoyaLMaru", 30, FontStyle.Bold))
                {
                    g.DrawString($"底分 {rating} + 段位 {addRating}", font, new SolidBrush(Color.Black), 140, 12);
                }
            }

            var userInfoCard = new Bitmap(nameCard.Width + 6,
                ratingCard.Height                        + nameCard.Height + rainbowCard.Height + 20);

            using (var g = Graphics.FromImage(userInfoCard))
            {
                g.DrawImage(ratingCard, 0, 0);
                g.DrawImage(nameCard, 3, ratingCard.Height    + 10);
                g.DrawImage(rainbowCard, 0, ratingCard.Height + nameCard.Height + 20);
            }

            // 添加一个生草头像
            var background = new Bitmap(2180, userInfoCard.Height + 50);
            var dlx        = ResourceManager.GetImage("dlx.png");
            dlx = dlx.Resize(userInfoCard.Height, userInfoCard.Height);
            using (var g = Graphics.FromImage(background))
            {
                g.DrawImage(dlx, 0, 20);
                g.DrawImage(userInfoCard, userInfoCard.Height + 10, 20);
            }

            return background;
        }

        public string GetImage()
        {
            var ratCard = GetRatingCard();
            var b40     = GetB40Card();

            var background = new Bitmap(b40.Width, ratCard.Height + b40.Height);

            using (var g = Graphics.FromImage(background))
            {
                g.Clear(Color.FromArgb(75, 181, 181));
                g.DrawImage(ratCard, 0, 0);
                g.DrawImage(b40, 0, ratCard.Height);
            }

            return background.ToB64();
        }

        #endregion
    }
}