/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2019 chibayuki@foxmail.com

Com.ColorX
Version 19.4.28.0000

This file is part of Com

Com is released under the GPLv3 license
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Com
{
    /// <summary>
    /// 以双精度浮点数在 RGB、HSV、HSL、CMYK、LAB、YUV 等色彩空间表示的颜色。
    /// </summary>
    public struct ColorX : IEquatable<ColorX>
    {
        #region 私有成员与内部成员

        private static Hashtable _NameTable = null; // 表示从 32 位 ARGB 值到颜色名称的映射。
        private static Hashtable _ArgbTable = null; // 表示从颜色名称到 32 位 ARGB 值的映射。

        private static void _EnsureNameAndArgbTable() // 初始化 32 位 ARGB 值与颜色名称的映射表。
        {
            if (_NameTable == null || _ArgbTable == null)
            {
                int transparent = Color.Transparent.ToArgb();
                int aliceblue = Color.AliceBlue.ToArgb();
                int antiquewhite = Color.AntiqueWhite.ToArgb();
                int aqua = Color.Aqua.ToArgb();
                int aquamarine = Color.Aquamarine.ToArgb();
                int azure = Color.Azure.ToArgb();
                int beige = Color.Beige.ToArgb();
                int bisque = Color.Bisque.ToArgb();
                int black = Color.Black.ToArgb();
                int blanchedalmond = Color.BlanchedAlmond.ToArgb();
                int blue = Color.Blue.ToArgb();
                int blueviolet = Color.BlueViolet.ToArgb();
                int brown = Color.Brown.ToArgb();
                int burlywood = Color.BurlyWood.ToArgb();
                int cadetblue = Color.CadetBlue.ToArgb();
                int chartreuse = Color.Chartreuse.ToArgb();
                int chocolate = Color.Chocolate.ToArgb();
                int coral = Color.Coral.ToArgb();
                int cornflowerblue = Color.CornflowerBlue.ToArgb();
                int cornsilk = Color.Cornsilk.ToArgb();
                int crimson = Color.Crimson.ToArgb();
                int cyan = Color.Cyan.ToArgb();
                int darkblue = Color.DarkBlue.ToArgb();
                int darkcyan = Color.DarkCyan.ToArgb();
                int darkgoldenrod = Color.DarkGoldenrod.ToArgb();
                int darkgray = Color.DarkGray.ToArgb();
                int darkgreen = Color.DarkGreen.ToArgb();
                int darkkhaki = Color.DarkKhaki.ToArgb();
                int darkmagenta = Color.DarkMagenta.ToArgb();
                int darkolivegreen = Color.DarkOliveGreen.ToArgb();
                int darkorange = Color.DarkOrange.ToArgb();
                int darkorchid = Color.DarkOrchid.ToArgb();
                int darkred = Color.DarkRed.ToArgb();
                int darksalmon = Color.DarkSalmon.ToArgb();
                int darkseagreen = Color.DarkSeaGreen.ToArgb();
                int darkslateblue = Color.DarkSlateBlue.ToArgb();
                int darkslategray = Color.DarkSlateGray.ToArgb();
                int darkturquoise = Color.DarkTurquoise.ToArgb();
                int darkviolet = Color.DarkViolet.ToArgb();
                int deeppink = Color.DeepPink.ToArgb();
                int deepskyblue = Color.DeepSkyBlue.ToArgb();
                int dimgray = Color.DimGray.ToArgb();
                int dodgerblue = Color.DodgerBlue.ToArgb();
                int firebrick = Color.Firebrick.ToArgb();
                int floralwhite = Color.FloralWhite.ToArgb();
                int forestgreen = Color.ForestGreen.ToArgb();
                int fuchsia = Color.Fuchsia.ToArgb();
                int gainsboro = Color.Gainsboro.ToArgb();
                int ghostwhite = Color.GhostWhite.ToArgb();
                int gold = Color.Gold.ToArgb();
                int goldenrod = Color.Goldenrod.ToArgb();
                int gray = Color.Gray.ToArgb();
                int green = Color.Green.ToArgb();
                int greenyellow = Color.GreenYellow.ToArgb();
                int honeydew = Color.Honeydew.ToArgb();
                int hotpink = Color.HotPink.ToArgb();
                int indianred = Color.IndianRed.ToArgb();
                int indigo = Color.Indigo.ToArgb();
                int ivory = Color.Ivory.ToArgb();
                int khaki = Color.Khaki.ToArgb();
                int lavender = Color.Lavender.ToArgb();
                int lavenderblush = Color.LavenderBlush.ToArgb();
                int lawngreen = Color.LawnGreen.ToArgb();
                int lemonchiffon = Color.LemonChiffon.ToArgb();
                int lightblue = Color.LightBlue.ToArgb();
                int lightcoral = Color.LightCoral.ToArgb();
                int lightcyan = Color.LightCyan.ToArgb();
                int lightgoldenrodyellow = Color.LightGoldenrodYellow.ToArgb();
                int lightgray = Color.LightGray.ToArgb();
                int lightgreen = Color.LightGreen.ToArgb();
                int lightpink = Color.LightPink.ToArgb();
                int lightsalmon = Color.LightSalmon.ToArgb();
                int lightseagreen = Color.LightSeaGreen.ToArgb();
                int lightskyblue = Color.LightSkyBlue.ToArgb();
                int lightslategray = Color.LightSlateGray.ToArgb();
                int lightsteelblue = Color.LightSteelBlue.ToArgb();
                int lightyellow = Color.LightYellow.ToArgb();
                int lime = Color.Lime.ToArgb();
                int limegreen = Color.LimeGreen.ToArgb();
                int linen = Color.Linen.ToArgb();
                int magenta = Color.Magenta.ToArgb();
                int maroon = Color.Maroon.ToArgb();
                int mediumaquamarine = Color.MediumAquamarine.ToArgb();
                int mediumblue = Color.MediumBlue.ToArgb();
                int mediumorchid = Color.MediumOrchid.ToArgb();
                int mediumpurple = Color.MediumPurple.ToArgb();
                int mediumseagreen = Color.MediumSeaGreen.ToArgb();
                int mediumslateblue = Color.MediumSlateBlue.ToArgb();
                int mediumspringgreen = Color.MediumSpringGreen.ToArgb();
                int mediumturquoise = Color.MediumTurquoise.ToArgb();
                int mediumvioletred = Color.MediumVioletRed.ToArgb();
                int midnightblue = Color.MidnightBlue.ToArgb();
                int mintcream = Color.MintCream.ToArgb();
                int mistyrose = Color.MistyRose.ToArgb();
                int moccasin = Color.Moccasin.ToArgb();
                int navajowhite = Color.NavajoWhite.ToArgb();
                int navy = Color.Navy.ToArgb();
                int oldlace = Color.OldLace.ToArgb();
                int olive = Color.Olive.ToArgb();
                int olivedrab = Color.OliveDrab.ToArgb();
                int orange = Color.Orange.ToArgb();
                int orangered = Color.OrangeRed.ToArgb();
                int orchid = Color.Orchid.ToArgb();
                int palegoldenrod = Color.PaleGoldenrod.ToArgb();
                int palegreen = Color.PaleGreen.ToArgb();
                int paleturquoise = Color.PaleTurquoise.ToArgb();
                int palevioletred = Color.PaleVioletRed.ToArgb();
                int papayawhip = Color.PapayaWhip.ToArgb();
                int peachpuff = Color.PeachPuff.ToArgb();
                int peru = Color.Peru.ToArgb();
                int pink = Color.Pink.ToArgb();
                int plum = Color.Plum.ToArgb();
                int powderblue = Color.PowderBlue.ToArgb();
                int purple = Color.Purple.ToArgb();
                int red = Color.Red.ToArgb();
                int rosybrown = Color.RosyBrown.ToArgb();
                int royalblue = Color.RoyalBlue.ToArgb();
                int saddlebrown = Color.SaddleBrown.ToArgb();
                int salmon = Color.Salmon.ToArgb();
                int sandybrown = Color.SandyBrown.ToArgb();
                int seagreen = Color.SeaGreen.ToArgb();
                int seashell = Color.SeaShell.ToArgb();
                int sienna = Color.Sienna.ToArgb();
                int silver = Color.Silver.ToArgb();
                int skyblue = Color.SkyBlue.ToArgb();
                int slateblue = Color.SlateBlue.ToArgb();
                int slategray = Color.SlateGray.ToArgb();
                int snow = Color.Snow.ToArgb();
                int springgreen = Color.SpringGreen.ToArgb();
                int steelblue = Color.SteelBlue.ToArgb();
                int tan = Color.Tan.ToArgb();
                int teal = Color.Teal.ToArgb();
                int thistle = Color.Thistle.ToArgb();
                int tomato = Color.Tomato.ToArgb();
                int turquoise = Color.Turquoise.ToArgb();
                int violet = Color.Violet.ToArgb();
                int wheat = Color.Wheat.ToArgb();
                int white = Color.White.ToArgb();
                int whitesmoke = Color.WhiteSmoke.ToArgb();
                int yellow = Color.Yellow.ToArgb();
                int yellowgreen = Color.YellowGreen.ToArgb();

                if (_NameTable == null)
                {
                    _NameTable = new Hashtable();

                    _NameTable.Add(transparent, "Transparent");
                    _NameTable.Add(aliceblue, "AliceBlue");
                    _NameTable.Add(antiquewhite, "AntiqueWhite");
                    // Aqua 与 Cyan 异名同色，使用 Cyan
                    // _NameTable.Add(aqua, "Aqua");
                    _NameTable.Add(aquamarine, "Aquamarine");
                    _NameTable.Add(azure, "Azure");
                    _NameTable.Add(beige, "Beige");
                    _NameTable.Add(bisque, "Bisque");
                    _NameTable.Add(black, "Black");
                    _NameTable.Add(blanchedalmond, "BlanchedAlmond");
                    _NameTable.Add(blue, "Blue");
                    _NameTable.Add(blueviolet, "BlueViolet");
                    _NameTable.Add(brown, "Brown");
                    _NameTable.Add(burlywood, "BurlyWood");
                    _NameTable.Add(cadetblue, "CadetBlue");
                    _NameTable.Add(chartreuse, "Chartreuse");
                    _NameTable.Add(chocolate, "Chocolate");
                    _NameTable.Add(coral, "Coral");
                    _NameTable.Add(cornflowerblue, "CornflowerBlue");
                    _NameTable.Add(cornsilk, "Cornsilk");
                    _NameTable.Add(crimson, "Crimson");
                    _NameTable.Add(cyan, "Cyan");
                    _NameTable.Add(darkblue, "DarkBlue");
                    _NameTable.Add(darkcyan, "DarkCyan");
                    _NameTable.Add(darkgoldenrod, "DarkGoldenrod");
                    _NameTable.Add(darkgray, "DarkGray");
                    _NameTable.Add(darkgreen, "DarkGreen");
                    _NameTable.Add(darkkhaki, "DarkKhaki");
                    _NameTable.Add(darkmagenta, "DarkMagenta");
                    _NameTable.Add(darkolivegreen, "DarkOliveGreen");
                    _NameTable.Add(darkorange, "DarkOrange");
                    _NameTable.Add(darkorchid, "DarkOrchid");
                    _NameTable.Add(darkred, "DarkRed");
                    _NameTable.Add(darksalmon, "DarkSalmon");
                    _NameTable.Add(darkseagreen, "DarkSeaGreen");
                    _NameTable.Add(darkslateblue, "DarkSlateBlue");
                    _NameTable.Add(darkslategray, "DarkSlateGray");
                    _NameTable.Add(darkturquoise, "DarkTurquoise");
                    _NameTable.Add(darkviolet, "DarkViolet");
                    _NameTable.Add(deeppink, "DeepPink");
                    _NameTable.Add(deepskyblue, "DeepSkyBlue");
                    _NameTable.Add(dimgray, "DimGray");
                    _NameTable.Add(dodgerblue, "DodgerBlue");
                    _NameTable.Add(firebrick, "Firebrick");
                    _NameTable.Add(floralwhite, "FloralWhite");
                    _NameTable.Add(forestgreen, "ForestGreen");
                    // Fuchsia 与 Magenta 异名同色，使用 Magenta
                    // _NameTable.Add(fuchsia, "Fuchsia");
                    _NameTable.Add(gainsboro, "Gainsboro");
                    _NameTable.Add(ghostwhite, "GhostWhite");
                    _NameTable.Add(gold, "Gold");
                    _NameTable.Add(goldenrod, "Goldenrod");
                    _NameTable.Add(gray, "Gray");
                    _NameTable.Add(green, "Green");
                    _NameTable.Add(greenyellow, "GreenYellow");
                    _NameTable.Add(honeydew, "Honeydew");
                    _NameTable.Add(hotpink, "HotPink");
                    _NameTable.Add(indianred, "IndianRed");
                    _NameTable.Add(indigo, "Indigo");
                    _NameTable.Add(ivory, "Ivory");
                    _NameTable.Add(khaki, "Khaki");
                    _NameTable.Add(lavender, "Lavender");
                    _NameTable.Add(lavenderblush, "LavenderBlush");
                    _NameTable.Add(lawngreen, "LawnGreen");
                    _NameTable.Add(lemonchiffon, "LemonChiffon");
                    _NameTable.Add(lightblue, "LightBlue");
                    _NameTable.Add(lightcoral, "LightCoral");
                    _NameTable.Add(lightcyan, "LightCyan");
                    _NameTable.Add(lightgoldenrodyellow, "LightGoldenrodYellow");
                    _NameTable.Add(lightgray, "LightGray");
                    _NameTable.Add(lightgreen, "LightGreen");
                    _NameTable.Add(lightpink, "LightPink");
                    _NameTable.Add(lightsalmon, "LightSalmon");
                    _NameTable.Add(lightseagreen, "LightSeaGreen");
                    _NameTable.Add(lightskyblue, "LightSkyBlue");
                    _NameTable.Add(lightslategray, "LightSlateGray");
                    _NameTable.Add(lightsteelblue, "LightSteelBlue");
                    _NameTable.Add(lightyellow, "LightYellow");
                    _NameTable.Add(lime, "Lime");
                    _NameTable.Add(limegreen, "LimeGreen");
                    _NameTable.Add(linen, "Linen");
                    _NameTable.Add(magenta, "Magenta");
                    _NameTable.Add(maroon, "Maroon");
                    _NameTable.Add(mediumaquamarine, "MediumAquamarine");
                    _NameTable.Add(mediumblue, "MediumBlue");
                    _NameTable.Add(mediumorchid, "MediumOrchid");
                    _NameTable.Add(mediumpurple, "MediumPurple");
                    _NameTable.Add(mediumseagreen, "MediumSeaGreen");
                    _NameTable.Add(mediumslateblue, "MediumSlateBlue");
                    _NameTable.Add(mediumspringgreen, "MediumSpringGreen");
                    _NameTable.Add(mediumturquoise, "MediumTurquoise");
                    _NameTable.Add(mediumvioletred, "MediumVioletRed");
                    _NameTable.Add(midnightblue, "MidnightBlue");
                    _NameTable.Add(mintcream, "MintCream");
                    _NameTable.Add(mistyrose, "MistyRose");
                    _NameTable.Add(moccasin, "Moccasin");
                    _NameTable.Add(navajowhite, "NavajoWhite");
                    _NameTable.Add(navy, "Navy");
                    _NameTable.Add(oldlace, "OldLace");
                    _NameTable.Add(olive, "Olive");
                    _NameTable.Add(olivedrab, "OliveDrab");
                    _NameTable.Add(orange, "Orange");
                    _NameTable.Add(orangered, "OrangeRed");
                    _NameTable.Add(orchid, "Orchid");
                    _NameTable.Add(palegoldenrod, "PaleGoldenrod");
                    _NameTable.Add(palegreen, "PaleGreen");
                    _NameTable.Add(paleturquoise, "PaleTurquoise");
                    _NameTable.Add(palevioletred, "PaleVioletRed");
                    _NameTable.Add(papayawhip, "PapayaWhip");
                    _NameTable.Add(peachpuff, "PeachPuff");
                    _NameTable.Add(peru, "Peru");
                    _NameTable.Add(pink, "Pink");
                    _NameTable.Add(plum, "Plum");
                    _NameTable.Add(powderblue, "PowderBlue");
                    _NameTable.Add(purple, "Purple");
                    _NameTable.Add(red, "Red");
                    _NameTable.Add(rosybrown, "RosyBrown");
                    _NameTable.Add(royalblue, "RoyalBlue");
                    _NameTable.Add(saddlebrown, "SaddleBrown");
                    _NameTable.Add(salmon, "Salmon");
                    _NameTable.Add(sandybrown, "SandyBrown");
                    _NameTable.Add(seagreen, "SeaGreen");
                    _NameTable.Add(seashell, "SeaShell");
                    _NameTable.Add(sienna, "Sienna");
                    _NameTable.Add(silver, "Silver");
                    _NameTable.Add(skyblue, "SkyBlue");
                    _NameTable.Add(slateblue, "SlateBlue");
                    _NameTable.Add(slategray, "SlateGray");
                    _NameTable.Add(snow, "Snow");
                    _NameTable.Add(springgreen, "SpringGreen");
                    _NameTable.Add(steelblue, "SteelBlue");
                    _NameTable.Add(tan, "Tan");
                    _NameTable.Add(teal, "Teal");
                    _NameTable.Add(thistle, "Thistle");
                    _NameTable.Add(tomato, "Tomato");
                    _NameTable.Add(turquoise, "Turquoise");
                    _NameTable.Add(violet, "Violet");
                    _NameTable.Add(wheat, "Wheat");
                    _NameTable.Add(white, "White");
                    _NameTable.Add(whitesmoke, "WhiteSmoke");
                    _NameTable.Add(yellow, "Yellow");
                    _NameTable.Add(yellowgreen, "YellowGreen");
                }

                if (_ArgbTable == null)
                {
                    _ArgbTable = new Hashtable();

                    _ArgbTable.Add("transparent", transparent);
                    _ArgbTable.Add("aliceblue", aliceblue);
                    _ArgbTable.Add("antiquewhite", antiquewhite);
                    _ArgbTable.Add("aqua", aqua);
                    _ArgbTable.Add("aquamarine", aquamarine);
                    _ArgbTable.Add("azure", azure);
                    _ArgbTable.Add("beige", beige);
                    _ArgbTable.Add("bisque", bisque);
                    _ArgbTable.Add("black", black);
                    _ArgbTable.Add("blanchedalmond", blanchedalmond);
                    _ArgbTable.Add("blue", blue);
                    _ArgbTable.Add("blueviolet", blueviolet);
                    _ArgbTable.Add("brown", brown);
                    _ArgbTable.Add("burlywood", burlywood);
                    _ArgbTable.Add("cadetblue", cadetblue);
                    _ArgbTable.Add("chartreuse", chartreuse);
                    _ArgbTable.Add("chocolate", chocolate);
                    _ArgbTable.Add("coral", coral);
                    _ArgbTable.Add("cornflowerblue", cornflowerblue);
                    _ArgbTable.Add("cornsilk", cornsilk);
                    _ArgbTable.Add("crimson", crimson);
                    _ArgbTable.Add("cyan", cyan);
                    _ArgbTable.Add("darkblue", darkblue);
                    _ArgbTable.Add("darkcyan", darkcyan);
                    _ArgbTable.Add("darkgoldenrod", darkgoldenrod);
                    _ArgbTable.Add("darkgray", darkgray);
                    _ArgbTable.Add("darkgreen", darkgreen);
                    _ArgbTable.Add("darkkhaki", darkkhaki);
                    _ArgbTable.Add("darkmagenta", darkmagenta);
                    _ArgbTable.Add("darkolivegreen", darkolivegreen);
                    _ArgbTable.Add("darkorange", darkorange);
                    _ArgbTable.Add("darkorchid", darkorchid);
                    _ArgbTable.Add("darkred", darkred);
                    _ArgbTable.Add("darksalmon", darksalmon);
                    _ArgbTable.Add("darkseagreen", darkseagreen);
                    _ArgbTable.Add("darkslateblue", darkslateblue);
                    _ArgbTable.Add("darkslategray", darkslategray);
                    _ArgbTable.Add("darkturquoise", darkturquoise);
                    _ArgbTable.Add("darkviolet", darkviolet);
                    _ArgbTable.Add("deeppink", deeppink);
                    _ArgbTable.Add("deepskyblue", deepskyblue);
                    _ArgbTable.Add("dimgray", dimgray);
                    _ArgbTable.Add("dodgerblue", dodgerblue);
                    _ArgbTable.Add("firebrick", firebrick);
                    _ArgbTable.Add("floralwhite", floralwhite);
                    _ArgbTable.Add("forestgreen", forestgreen);
                    _ArgbTable.Add("fuchsia", fuchsia);
                    _ArgbTable.Add("gainsboro", gainsboro);
                    _ArgbTable.Add("ghostwhite", ghostwhite);
                    _ArgbTable.Add("gold", gold);
                    _ArgbTable.Add("goldenrod", goldenrod);
                    _ArgbTable.Add("gray", gray);
                    _ArgbTable.Add("green", green);
                    _ArgbTable.Add("greenyellow", greenyellow);
                    _ArgbTable.Add("honeydew", honeydew);
                    _ArgbTable.Add("hotpink", hotpink);
                    _ArgbTable.Add("indianred", indianred);
                    _ArgbTable.Add("indigo", indigo);
                    _ArgbTable.Add("ivory", ivory);
                    _ArgbTable.Add("khaki", khaki);
                    _ArgbTable.Add("lavender", lavender);
                    _ArgbTable.Add("lavenderblush", lavenderblush);
                    _ArgbTable.Add("lawngreen", lawngreen);
                    _ArgbTable.Add("lemonchiffon", lemonchiffon);
                    _ArgbTable.Add("lightblue", lightblue);
                    _ArgbTable.Add("lightcoral", lightcoral);
                    _ArgbTable.Add("lightcyan", lightcyan);
                    _ArgbTable.Add("lightgoldenrodyellow", lightgoldenrodyellow);
                    _ArgbTable.Add("lightgray", lightgray);
                    _ArgbTable.Add("lightgreen", lightgreen);
                    _ArgbTable.Add("lightpink", lightpink);
                    _ArgbTable.Add("lightsalmon", lightsalmon);
                    _ArgbTable.Add("lightseagreen", lightseagreen);
                    _ArgbTable.Add("lightskyblue", lightskyblue);
                    _ArgbTable.Add("lightslategray", lightslategray);
                    _ArgbTable.Add("lightsteelblue", lightsteelblue);
                    _ArgbTable.Add("lightyellow", lightyellow);
                    _ArgbTable.Add("lime", lime);
                    _ArgbTable.Add("limegreen", limegreen);
                    _ArgbTable.Add("linen", linen);
                    _ArgbTable.Add("magenta", magenta);
                    _ArgbTable.Add("maroon", maroon);
                    _ArgbTable.Add("mediumaquamarine", mediumaquamarine);
                    _ArgbTable.Add("mediumblue", mediumblue);
                    _ArgbTable.Add("mediumorchid", mediumorchid);
                    _ArgbTable.Add("mediumpurple", mediumpurple);
                    _ArgbTable.Add("mediumseagreen", mediumseagreen);
                    _ArgbTable.Add("mediumslateblue", mediumslateblue);
                    _ArgbTable.Add("mediumspringgreen", mediumspringgreen);
                    _ArgbTable.Add("mediumturquoise", mediumturquoise);
                    _ArgbTable.Add("mediumvioletred", mediumvioletred);
                    _ArgbTable.Add("midnightblue", midnightblue);
                    _ArgbTable.Add("mintcream", mintcream);
                    _ArgbTable.Add("mistyrose", mistyrose);
                    _ArgbTable.Add("moccasin", moccasin);
                    _ArgbTable.Add("navajowhite", navajowhite);
                    _ArgbTable.Add("navy", navy);
                    _ArgbTable.Add("oldlace", oldlace);
                    _ArgbTable.Add("olive", olive);
                    _ArgbTable.Add("olivedrab", olivedrab);
                    _ArgbTable.Add("orange", orange);
                    _ArgbTable.Add("orangered", orangered);
                    _ArgbTable.Add("orchid", orchid);
                    _ArgbTable.Add("palegoldenrod", palegoldenrod);
                    _ArgbTable.Add("palegreen", palegreen);
                    _ArgbTable.Add("paleturquoise", paleturquoise);
                    _ArgbTable.Add("palevioletred", palevioletred);
                    _ArgbTable.Add("papayawhip", papayawhip);
                    _ArgbTable.Add("peachpuff", peachpuff);
                    _ArgbTable.Add("peru", peru);
                    _ArgbTable.Add("pink", pink);
                    _ArgbTable.Add("plum", plum);
                    _ArgbTable.Add("powderblue", powderblue);
                    _ArgbTable.Add("purple", purple);
                    _ArgbTable.Add("red", red);
                    _ArgbTable.Add("rosybrown", rosybrown);
                    _ArgbTable.Add("royalblue", royalblue);
                    _ArgbTable.Add("saddlebrown", saddlebrown);
                    _ArgbTable.Add("salmon", salmon);
                    _ArgbTable.Add("sandybrown", sandybrown);
                    _ArgbTable.Add("seagreen", seagreen);
                    _ArgbTable.Add("seashell", seashell);
                    _ArgbTable.Add("sienna", sienna);
                    _ArgbTable.Add("silver", silver);
                    _ArgbTable.Add("skyblue", skyblue);
                    _ArgbTable.Add("slateblue", slateblue);
                    _ArgbTable.Add("slategray", slategray);
                    _ArgbTable.Add("snow", snow);
                    _ArgbTable.Add("springgreen", springgreen);
                    _ArgbTable.Add("steelblue", steelblue);
                    _ArgbTable.Add("tan", tan);
                    _ArgbTable.Add("teal", teal);
                    _ArgbTable.Add("thistle", thistle);
                    _ArgbTable.Add("tomato", tomato);
                    _ArgbTable.Add("turquoise", turquoise);
                    _ArgbTable.Add("violet", violet);
                    _ArgbTable.Add("wheat", wheat);
                    _ArgbTable.Add("white", white);
                    _ArgbTable.Add("whitesmoke", whitesmoke);
                    _ArgbTable.Add("yellow", yellow);
                    _ArgbTable.Add("yellowgreen", yellowgreen);
                }
            }
        }

        private static string _GetNameByArgb(int argb) // 根据 32 位 ARGB 值获取颜色名称。
        {
            _EnsureNameAndArgbTable();

            if (_NameTable.Contains(argb))
            {
                return (string)_NameTable[argb];
            }

            return null;
        }

        private static int? _GetArgbByName(string name) // 根据颜色名称获取 32 位 ARGB 值。
        {
            _EnsureNameAndArgbTable();

            if (!string.IsNullOrWhiteSpace(name))
            {
                string Name = new Regex(@"[^A-Za-z]").Replace(name, string.Empty).ToLower();

                if (!string.IsNullOrEmpty(Name))
                {
                    if (_ArgbTable.Contains(Name))
                    {
                        return (int)_ArgbTable[Name];
                    }
                }
            }

            return null;
        }

        //

        private const int _32BitArgbAlphaShift = 24; // 32 位 ARGB 颜色的 Alpha 分量（A）的位偏移量。
        private const int _32BitArgbRedShift = 16; // 32 位 ARGB 颜色的红色分量（R）的位偏移量。
        private const int _32BitArgbGreenShift = 8; // 32 位 ARGB 颜色的绿色分量（G）的位偏移量。
        private const int _32BitArgbBlueShift = 0; // 32 位 ARGB 颜色的蓝色分量（B）的位偏移量。

        //

        private const int _ChannelToSpaceDivisor = 0x00010000; // 将色彩通道转换为色彩空间的转换因子。
        private const int _ChannelCount = 4; // 每个色彩空间的最大色彩通道数量。

        private enum _ColorSpace // 色彩空间。
        {
            None = 0, // 不表示任何色彩空间。

            RGB = 0x00010000, // RGB 色彩空间。
            HSV = 0x00020000, // HSV 色彩空间。
            HSL = 0x00030000, // HSL 色彩空间。
            CMYK = 0x00040000, // CMYK 色彩空间。
            LAB = 0x00050000, // LAB 色彩空间。
            YUV = 0x00060000 // YUV 色彩空间。
        }

        private enum _ColorChannel // 色彩通道。
        {
            None = 0, // 不表示任何色彩通道。

            Red = (int)_ColorSpace.RGB, // RGB 色彩空间的红色通道（R）。
            Green, // RGB 色彩空间的绿色通道（G）。
            Blue, // RGB 色彩空间的蓝色通道（B）。

            Hue_HSV = (int)_ColorSpace.HSV, // HSV 色彩空间的色相（H）。
            Saturation_HSV, // HSV 色彩空间的饱和度（S）。
            Brightness, // HSV 色彩空间的亮度（V）。

            Hue_HSL = (int)_ColorSpace.HSL, // HSL 色彩空间的色相（H）。
            Saturation_HSL, // HSL 色彩空间的饱和度（S）。
            Lightness_HSL, // HSL 色彩空间的明度（L）。

            Cyan = (int)_ColorSpace.CMYK, // CMYK 色彩空间的青色通道（C）。
            Magenta, // CMYK 色彩空间的洋红色通道（M）。
            Yellow, // CMYK 色彩空间的黄色通道（Y）。
            Black, // CMYK 色彩空间的黑色通道（K）。

            Lightness_LAB = (int)_ColorSpace.LAB, // LAB 色彩空间的明度（L）。
            GreenRed, // LAB 色彩空间的绿色-红色通道（A）。
            BlueYellow, // LAB 色彩空间的蓝色-黄色通道（B）。

            Luminance = (int)_ColorSpace.YUV, // YUV 色彩空间的亮度（Y）。
            ChrominanceBlue, // YUV 色彩空间的蓝色色度（U）。
            ChrominanceRed, // YUV 色彩空间的红色色度（V）。
        }

        //

        private const double _MinOpacity = 0, _MaxOpacity = 100, _DefaultOpacity = 100; // 不透明度的最小值、最大值与默认值。
        private const double _MinAlpha = 0, _MaxAlpha = 255, _DefaultAlpha = 255; // Alpha 通道（A）的最小值、最大值与默认值。

        private const double _MinRed = 0, _MaxRed = 255, _DefaultRed = 0; // RGB 色彩空间的红色通道（R）的最小值、最大值与默认值。
        private const double _MinGreen = 0, _MaxGreen = 255, _DefaultGreen = 0; // RGB 色彩空间的绿色通道（G）的最小值、最大值与默认值。
        private const double _MinBlue = 0, _MaxBlue = 255, _DefaultBlue = 0; // RGB 色彩空间的蓝色通道（B）的最小值、最大值与默认值。

        private const double _MinHue_HSV = 0, _MaxHue_HSV = 360, _DefaultHue_HSV = 0; // HSV 色彩空间的色相（H）的最小值、最大值与默认值。
        private const double _MinSaturation_HSV = 0, _MaxSaturation_HSV = 100, _DefaultSaturation_HSV = 0; // HSV 色彩空间的饱和度（S）的最小值、最大值与默认值。
        private const double _MinBrightness = 0, _MaxBrightness = 100, _DefaultBrightness = 0; // HSV 色彩空间的亮度（V）的最小值、最大值与默认值。

        private const double _MinHue_HSL = 0, _MaxHue_HSL = 360, _DefaultHue_HSL = 0; // HSL 色彩空间的色相（H）的最小值、最大值与默认值。
        private const double _MinSaturation_HSL = 0, _MaxSaturation_HSL = 100, _DefaultSaturation_HSL = 0; // HSL 色彩空间的饱和度（S）的最小值、最大值与默认值。
        private const double _MinLightness_HSL = 0, _MaxLightness_HSL = 100, _DefaultLightness_HSL = 0; // HSL 色彩空间的明度（L）的最小值、最大值与默认值。

        private const double _MinCyan = 0, _MaxCyan = 100, _DefaultCyan = 0; // CMYK 色彩空间的青色通道（C）的最小值、最大值与默认值。
        private const double _MinMagenta = 0, _MaxMagenta = 100, _DefaultMagenta = 0; // CMYK 色彩空间的洋红色通道（M）的最小值、最大值与默认值。
        private const double _MinYellow = 0, _MaxYellow = 100, _DefaultYellow = 0; // CMYK 色彩空间的黄色通道（Y）的最小值、最大值与默认值。
        private const double _MinBlack = 0, _MaxBlack = 100, _DefaultBlack = 100; // CMYK 色彩空间的黑色通道（K）的最小值、最大值与默认值。

        private const double _MinLightness_LAB = 0, _MaxLightness_LAB = 100, _DefaultLightness_LAB = 0; // LAB 色彩空间的明度（L）的最小值、最大值与默认值。
        private const double _MinGreenRed = -128, _MaxGreenRed = 128, _DefaultGreenRed = 0; // LAB 色彩空间的绿色-红色通道（A）的最小值、最大值与默认值。
        private const double _MinBlueYellow = -128, _MaxBlueYellow = 128, _DefaultBlueYellow = 0; // LAB 色彩空间的蓝色-黄色通道（B）的最小值、最大值与默认值。

        private const double _MinLuminance = 0, _MaxLuminance = 1, _DefaultLuminance = 0; // YUV 色彩空间的亮度（Y）的最小值、最大值与默认值。
        private const double _MinChrominanceBlue = -0.5, _MaxChrominanceBlue = 0.5, _DefaultChrominanceBlue = 0; // YUV 色彩空间的蓝色色度（U）的最小值、最大值与默认值。
        private const double _MinChrominanceRed = -0.5, _MaxChrominanceRed = 0.5, _DefaultChrominanceRed = 0; // YUV 色彩空间的红色色度（V）的最小值、最大值与默认值。

        //

        private const double _MinOpacity_FloDev = _MinOpacity - 5E-13, _MaxOpacity_FloDev = _MaxOpacity + 5E-11; // 不透明度的最小值与最大值，包含浮点偏差。
        private const double _MinAlpha_FloDev = _MinAlpha - 5E-13, _MaxAlpha_FloDev = _MaxAlpha + 5E-11; // Alpha 通道（A）的最小值与最大值，包含浮点偏差。

        private const double _MinRed_FloDev = _MinRed - 5E-13, _MaxRed_FloDev = _MaxRed + 5E-11; // RGB 色彩空间的红色通道（R）的最小值与最大值，包含浮点偏差。
        private const double _MinGreen_FloDev = _MinGreen - 5E-13, _MaxGreen_FloDev = _MaxGreen + 5E-11; // RGB 色彩空间的绿色通道（G）的最小值与最大值，包含浮点偏差。
        private const double _MinBlue_FloDev = _MinBlue - 5E-13, _MaxBlue_FloDev = _MaxBlue + 5E-11; // RGB 色彩空间的蓝色通道（B）的最小值与最大值，包含浮点偏差。

        private const double _MinHue_HSV_FloDev = _MinHue_HSV - 5E-13, _MaxHue_HSV_FloDev = _MaxHue_HSV + 5E-11; // HSV 色彩空间的色相（H）的最小值与最大值，包含浮点偏差。
        private const double _MinSaturation_HSV_FloDev = _MinSaturation_HSV - 5E-13, _MaxSaturation_HSV_FloDev = _MaxSaturation_HSV + 5E-11; // HSV 色彩空间的饱和度（S）的最小值与最大值，包含浮点偏差。
        private const double _MinBrightness_FloDev = _MinBrightness - 5E-13, _MaxBrightness_FloDev = _MaxBrightness + 5E-11; // HSV 色彩空间的亮度（V）的最小值与最大值，包含浮点偏差。

        private const double _MinHue_HSL_FloDev = _MinHue_HSL - 5E-13, _MaxHue_HSL_FloDev = _MaxHue_HSL + 5E-11; // HSL 色彩空间的色相（H）的最小值与最大值，包含浮点偏差。
        private const double _MinSaturation_HSL_FloDev = _MinSaturation_HSL - 5E-13, _MaxSaturation_HSL_FloDev = _MaxSaturation_HSL + 5E-11; // HSL 色彩空间的饱和度（S）的最小值与最大值，包含浮点偏差。
        private const double _MinLightness_HSL_FloDev = _MinLightness_HSL - 5E-13, _MaxLightness_HSL_FloDev = _MaxLightness_HSL + 5E-11; // HSL 色彩空间的明度（L）的最小值与最大值，包含浮点偏差。

        private const double _MinCyan_FloDev = _MinCyan - 5E-13, _MaxCyan_FloDev = _MaxCyan + 5E-11; // CMYK 色彩空间的青色通道（C）的最小值与最大值，包含浮点偏差。
        private const double _MinMagenta_FloDev = _MinMagenta - 5E-13, _MaxMagenta_FloDev = _MaxMagenta + 5E-11; // CMYK 色彩空间的洋红色通道（M）的最小值与最大值，包含浮点偏差。
        private const double _MinYellow_FloDev = _MinYellow - 5E-13, _MaxYellow_FloDev = _MaxYellow + 5E-11; // CMYK 色彩空间的黄色通道（Y）的最小值与最大值，包含浮点偏差。
        private const double _MinBlack_FloDev = _MinBlack - 5E-13, _MaxBlack_FloDev = _MaxBlack + 5E-11; // CMYK 色彩空间的黑色通道（K）的最小值与最大值，包含浮点偏差。

        private const double _MinLightness_LAB_FloDev = _MinLightness_LAB - 5E-13, _MaxLightness_LAB_FloDev = _MaxLightness_LAB + 5E-11; // LAB 色彩空间的明度（L）的最小值与最大值，包含浮点偏差。
        private const double _MinGreenRed_FloDev = _MinGreenRed - 5E-11, _MaxGreenRed_FloDev = _MaxGreenRed + 5E-11; // LAB 色彩空间的绿色-红色通道（A）的最小值与最大值，包含浮点偏差。
        private const double _MinBlueYellow_FloDev = _MinBlueYellow - 5E-11, _MaxBlueYellow_FloDev = _MaxBlueYellow + 5E-11; // LAB 色彩空间的蓝色-黄色通道（B）的最小值与最大值，包含浮点偏差。

        private const double _MinLuminance_FloDev = _MinLuminance - 5E-13, _MaxLuminance_FloDev = _MaxLuminance + 5E-13; // YUV 色彩空间的亮度（Y）的最小值与最大值，包含浮点偏差。
        private const double _MinChrominanceBlue_FloDev = _MinChrominanceBlue - 5E-14, _MaxChrominanceBlue_FloDev = _MaxChrominanceBlue + 5E-14; // YUV 色彩空间的蓝色色度（U）的最小值与最大值，包含浮点偏差。
        private const double _MinChrominanceRed_FloDev = _MinChrominanceRed - 5E-14, _MaxChrominanceRed_FloDev = _MaxChrominanceRed + 5E-14; // YUV 色彩空间的红色色度（V）的最小值与最大值，包含浮点偏差。

        //

        private static double _CheckOpacity(double opacity) // 对颜色的不透明度的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(opacity))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            if (opacity < _MinOpacity)
            {
                if (opacity <= _MinOpacity_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MinOpacity;
            }
            else if (opacity > _MaxOpacity)
            {
                if (opacity >= _MaxOpacity_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MaxOpacity;
            }
            else
            {
                return opacity;
            }
        }

        private static double _CheckAlpha(double alpha) // 对颜色的 Alpha 通道（A）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(alpha))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            if (alpha < _MinAlpha)
            {
                if (alpha <= _MinAlpha_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MinAlpha;
            }
            else if (alpha > _MaxAlpha)
            {
                if (alpha >= _MaxAlpha_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MaxAlpha;
            }
            else
            {
                return alpha;
            }
        }

        private static double _CheckRed(double red) // 对颜色在 RGB 色彩空间的红色通道（R）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(red))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            if (red < _MinRed)
            {
                if (red <= _MinRed_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MinRed;
            }
            else if (red > _MaxRed)
            {
                if (red >= _MaxRed_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MaxRed;
            }
            else
            {
                return red;
            }
        }

        private static double _CheckGreen(double green) // 对颜色在 RGB 色彩空间的绿色通道（G）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(green))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            if (green < _MinGreen)
            {
                if (green <= _MinGreen_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MinGreen;
            }
            else if (green > _MaxGreen)
            {
                if (green >= _MaxGreen_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MaxGreen;
            }
            else
            {
                return green;
            }
        }

        private static double _CheckBlue(double blue) // 对颜色在 RGB 色彩空间的蓝色通道（B）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(blue))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            if (blue < _MinBlue)
            {
                if (blue <= _MinBlue_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MinBlue;
            }
            else if (blue > _MaxBlue)
            {
                if (blue >= _MaxBlue_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MaxBlue;
            }
            else
            {
                return blue;
            }
        }

        private static double _CheckHue_HSV(double hue) // 对颜色在 HSV 色彩空间的色相（H）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(hue))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            if (hue < _MinHue_HSV)
            {
                if (hue <= _MinHue_HSV_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MinHue_HSV;
            }
            else if (hue > _MaxHue_HSV)
            {
                if (hue >= _MaxHue_HSV_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MaxHue_HSV;
            }
            else
            {
                return hue;
            }
        }

        private static double _CheckSaturation_HSV(double saturation) // 对颜色在 HSV 色彩空间的饱和度（S）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(saturation))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            if (saturation < _MinSaturation_HSV)
            {
                if (saturation <= _MinSaturation_HSV_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MinSaturation_HSV;
            }
            else if (saturation > _MaxSaturation_HSV)
            {
                if (saturation >= _MaxSaturation_HSV_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MaxSaturation_HSV;
            }
            else
            {
                return saturation;
            }
        }

        private static double _CheckBrightness(double brightness) // 对颜色在 HSV 色彩空间的亮度（V）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(brightness))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            if (brightness < _MinBrightness)
            {
                if (brightness <= _MinBrightness_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MinBrightness;
            }
            else if (brightness > _MaxBrightness)
            {
                if (brightness >= _MaxBrightness_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MaxBrightness;
            }
            else
            {
                return brightness;
            }
        }

        private static double _CheckHue_HSL(double hue) // 对颜色在 HSL 色彩空间的色相（H）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(hue))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            if (hue < _MinHue_HSL)
            {
                if (hue <= _MinHue_HSL_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MinHue_HSL;
            }
            else if (hue > _MaxHue_HSL)
            {
                if (hue >= _MaxHue_HSL_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MaxHue_HSL;
            }
            else
            {
                return hue;
            }
        }

        private static double _CheckSaturation_HSL(double saturation) // 对颜色在 HSL 色彩空间的饱和度（S）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(saturation))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            if (saturation < _MinSaturation_HSL)
            {
                if (saturation <= _MinSaturation_HSL_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MinSaturation_HSL;
            }
            else if (saturation > _MaxSaturation_HSL)
            {
                if (saturation >= _MaxSaturation_HSL_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MaxSaturation_HSL;
            }
            else
            {
                return saturation;
            }
        }

        private static double _CheckLightness_HSL(double lightness) // 对颜色在 HSL 色彩空间的明度（L）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(lightness))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            if (lightness < _MinLightness_HSL)
            {
                if (lightness <= _MinLightness_HSL_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MinLightness_HSL;
            }
            else if (lightness > _MaxLightness_HSL)
            {
                if (lightness >= _MaxLightness_HSL_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MaxLightness_HSL;
            }
            else
            {
                return lightness;
            }
        }

        private static double _CheckCyan(double cyan) // 对颜色在 CMYK 色彩空间的青色通道（C）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(cyan))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            if (cyan < _MinCyan)
            {
                if (cyan <= _MinCyan_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MinCyan;
            }
            else if (cyan > _MaxCyan)
            {
                if (cyan >= _MaxCyan_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MaxCyan;
            }
            else
            {
                return cyan;
            }
        }

        private static double _CheckMagenta(double magenta) // 对颜色在 CMYK 色彩空间的洋红色通道（M）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(magenta))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            if (magenta < _MinMagenta)
            {
                if (magenta <= _MinMagenta_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MinMagenta;
            }
            else if (magenta > _MaxMagenta)
            {
                if (magenta >= _MaxMagenta_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MaxMagenta;
            }
            else
            {
                return magenta;
            }
        }

        private static double _CheckYellow(double yellow) // 对颜色在 CMYK 色彩空间的黄色通道（Y）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(yellow))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            if (yellow < _MinYellow)
            {
                if (yellow <= _MinYellow_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MinYellow;
            }
            else if (yellow > _MaxYellow)
            {
                if (yellow >= _MaxYellow_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MaxYellow;
            }
            else
            {
                return yellow;
            }
        }

        private static double _CheckBlack(double black) // 对颜色在 CMYK 色彩空间的黑色通道（K）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(black))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            if (black < _MinBlack)
            {
                if (black <= _MinBlack_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MinBlack;
            }
            else if (black > _MaxBlack)
            {
                if (black >= _MaxBlack_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MaxBlack;
            }
            else
            {
                return black;
            }
        }

        private static double _CheckLightness_LAB(double lightness) // 对颜色在 LAB 色彩空间的明度（L）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(lightness))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            if (lightness < _MinLightness_LAB)
            {
                if (lightness <= _MinLightness_LAB_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MinLightness_LAB;
            }
            else if (lightness > _MaxLightness_LAB)
            {
                if (lightness >= _MaxLightness_LAB_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MaxLightness_LAB;
            }
            else
            {
                return lightness;
            }
        }

        private static double _CheckGreenRed(double greenRed) // 对颜色在 LAB 色彩空间的绿色-红色通道（A）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(greenRed))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            if (greenRed < _MinGreenRed)
            {
                if (greenRed <= _MinGreenRed_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MinGreenRed;
            }
            else if (greenRed > _MaxGreenRed)
            {
                if (greenRed >= _MaxGreenRed_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MaxGreenRed;
            }
            else
            {
                return greenRed;
            }
        }

        private static double _CheckBlueYellow(double blueYellow) // 对颜色在 LAB 色彩空间的蓝色-黄色通道（B）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(blueYellow))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            if (blueYellow < _MinBlueYellow)
            {
                if (blueYellow <= _MinBlueYellow_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MinBlueYellow;
            }
            else if (blueYellow > _MaxBlueYellow)
            {
                if (blueYellow >= _MaxBlueYellow_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MaxBlueYellow;
            }
            else
            {
                return blueYellow;
            }
        }

        private static double _CheckLuminance(double luminance) // 对颜色在 YUV 色彩空间的亮度（Y）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(luminance))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            if (luminance < _MinLuminance)
            {
                if (luminance <= _MinLuminance_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MinLuminance;
            }
            else if (luminance > _MaxLuminance)
            {
                if (luminance >= _MaxLuminance_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MaxLuminance;
            }
            else
            {
                return luminance;
            }
        }

        private static double _CheckChrominanceBlue(double chrominanceBlue) // 对颜色在 YUV 色彩空间的蓝色色度（U）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(chrominanceBlue))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            if (chrominanceBlue < _MinChrominanceBlue)
            {
                if (chrominanceBlue <= _MinChrominanceBlue_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MinChrominanceBlue;
            }
            else if (chrominanceBlue > _MaxChrominanceBlue)
            {
                if (chrominanceBlue >= _MaxChrominanceBlue_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MaxChrominanceBlue;
            }
            else
            {
                return chrominanceBlue;
            }
        }

        private static double _CheckChrominanceRed(double chrominanceRed) // 对颜色在 YUV 色彩空间的红色色度（V）的值进行合法性检查，返回合法的值。
        {
            if (InternalMethod.IsNaNOrInfinity(chrominanceRed))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            if (chrominanceRed < _MinChrominanceRed)
            {
                if (chrominanceRed <= _MinChrominanceRed_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MinChrominanceRed;
            }
            else if (chrominanceRed > _MaxChrominanceRed)
            {
                if (chrominanceRed >= _MaxChrominanceRed_FloDev)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _MaxChrominanceRed;
            }
            else
            {
                return chrominanceRed;
            }
        }

        //

        private static void _OpacityToAlpha(double opacity, out double alpha) // 将颜色的不透明度转换为 Alpha 通道（A）的值。此函数不检查输入参数的合法性，但保证输出参数的合法性。
        {
            alpha = opacity / _MaxOpacity * _MaxAlpha;

            if (alpha < _MinAlpha)
            {
                alpha = _MinAlpha;
            }
            else if (alpha > _MaxAlpha)
            {
                alpha = _MaxAlpha;
            }
        }

        private static void _AlphaToOpacity(double alpha, out double opacity) // 将颜色的 Alpha 通道（A）的值转换为不透明度。此函数不检查输入参数的合法性，但保证输出参数的合法性。
        {
            opacity = alpha / _MaxAlpha * _MaxOpacity;

            if (opacity < _MinOpacity)
            {
                opacity = _MinOpacity;
            }
            else if (opacity > _MaxOpacity)
            {
                opacity = _MaxOpacity;
            }
        }

        private static void _RGBToHSV(double red, double green, double blue, out double hue, out double saturation, out double brightness) // 将颜色在 RGB 色彩空间的各分量转换为在 HSV 色彩空间的各分量。此函数不检查输入参数的合法性，但保证输出参数的合法性。
        {
            red /= _MaxRed;
            green /= _MaxGreen;
            blue /= _MaxBlue;

            double Max = red, Min = red;

            if (Max < green)
            {
                Max = green;
            }

            if (Max < blue)
            {
                Max = blue;
            }

            if (Min > green)
            {
                Min = green;
            }

            if (Min > blue)
            {
                Min = blue;
            }

            if (Max == Min)
            {
                hue = 0;
            }
            else
            {
                double dH = 1.0 / 6 / (Max - Min);

                if (Max == red)
                {
                    hue = dH * (green - blue);

                    if (hue < 0)
                    {
                        hue += 1;
                    }
                }
                else if (Max == green)
                {
                    hue = dH * (blue - red) + 1.0 / 3;
                }
                else
                {
                    hue = dH * (red - green) + 2.0 / 3;
                }
            }

            if (Max == 0)
            {
                saturation = 0;
            }
            else
            {
                saturation = 1 - Min / Max;
            }

            brightness = Max;

            hue *= _MaxHue_HSV;
            saturation *= _MaxSaturation_HSV;
            brightness *= _MaxBrightness;

            if (hue < _MinHue_HSV)
            {
                hue = _MinHue_HSV;
            }
            else if (hue > _MaxHue_HSV)
            {
                hue = _MaxHue_HSV;
            }

            if (saturation < _MinSaturation_HSV)
            {
                saturation = _MinSaturation_HSV;
            }
            else if (saturation > _MaxSaturation_HSV)
            {
                saturation = _MaxSaturation_HSV;
            }

            if (brightness < _MinBrightness)
            {
                brightness = _MinBrightness;
            }
            else if (brightness > _MaxBrightness)
            {
                brightness = _MaxBrightness;
            }
        }

        private static void _HSVToRGB(double hue, double saturation, double brightness, out double red, out double green, out double blue) // 将颜色在 HSV 色彩空间的各分量转换为在 RGB 色彩空间的各分量。此函数不检查输入参数的合法性，但保证输出参数的合法性。
        {
            hue /= _MaxHue_HSV;
            saturation /= _MaxSaturation_HSV;
            brightness /= _MaxBrightness;

            int Hi = (int)Math.Floor(hue * 6);

            if (Hi == 6)
            {
                hue = 0;
            }

            Hi %= 6;

            double Hf = hue * 6 - Hi;

            double C = brightness * saturation;

            double X = C * (1 - Math.Abs(Hi % 2 + Hf - 1));

            switch (Hi)
            {
                case 0:
                    {
                        red = C;
                        green = X;
                        blue = 0;
                    }
                    break;

                case 1:
                    {
                        red = X;
                        green = C;
                        blue = 0;
                    }
                    break;

                case 2:
                    {
                        red = 0;
                        green = C;
                        blue = X;
                    }
                    break;

                case 3:
                    {
                        red = 0;
                        green = X;
                        blue = C;
                    }
                    break;

                case 4:
                    {
                        red = X;
                        green = 0;
                        blue = C;
                    }
                    break;

                case 5:
                    {
                        red = C;
                        green = 0;
                        blue = X;
                    }
                    break;

                default:
                    {
                        red = 0;
                        green = 0;
                        blue = 0;
                    }
                    break;
            }

            double M = brightness - C;

            red += M;
            green += M;
            blue += M;

            red *= _MaxRed;
            green *= _MaxGreen;
            blue *= _MaxBlue;

            if (red < _MinRed)
            {
                red = _MinRed;
            }
            else if (red > _MaxRed)
            {
                red = _MaxRed;
            }

            if (green < _MinGreen)
            {
                green = _MinGreen;
            }
            else if (green > _MaxGreen)
            {
                green = _MaxGreen;
            }

            if (blue < _MinBlue)
            {
                blue = _MinBlue;
            }
            else if (blue > _MaxBlue)
            {
                blue = _MaxBlue;
            }
        }

        private static void _RGBToHSL(double red, double green, double blue, out double hue, out double saturation, out double lightness) // 将颜色在 RGB 色彩空间的各分量转换为在 HSL 色彩空间的各分量。此函数不检查输入参数的合法性，但保证输出参数的合法性。
        {
            red /= _MaxRed;
            green /= _MaxGreen;
            blue /= _MaxBlue;

            double Max = red, Min = red;

            if (Max < green)
            {
                Max = green;
            }

            if (Max < blue)
            {
                Max = blue;
            }

            if (Min > green)
            {
                Min = green;
            }

            if (Min > blue)
            {
                Min = blue;
            }

            if (Max == Min)
            {
                hue = 0;
            }
            else
            {
                double dH = 1.0 / 6 / (Max - Min);

                if (Max == red)
                {
                    hue = dH * (green - blue);

                    if (hue < 0)
                    {
                        hue += 1;
                    }
                }
                else if (Max == green)
                {
                    hue = dH * (blue - red) + 1.0 / 3;
                }
                else
                {
                    hue = dH * (red - green) + 2.0 / 3;
                }
            }

            lightness = (Max + Min) / 2;

            if (lightness == 0 || Max == Min)
            {
                saturation = 0;
            }
            else
            {
                saturation = (Max - Min) / (1 - Math.Abs(2 * lightness - 1));
            }

            hue *= _MaxHue_HSL;
            saturation *= _MaxSaturation_HSL;
            lightness *= _MaxLightness_HSL;

            if (hue < _MinHue_HSL)
            {
                hue = _MinHue_HSL;
            }
            else if (hue > _MaxHue_HSL)
            {
                hue = _MaxHue_HSL;
            }

            if (saturation < _MinSaturation_HSL)
            {
                saturation = _MinSaturation_HSL;
            }
            else if (saturation > _MaxSaturation_HSL)
            {
                saturation = _MaxSaturation_HSL;
            }

            if (lightness < _MinLightness_HSL)
            {
                lightness = _MinLightness_HSL;
            }
            else if (lightness > _MaxLightness_HSL)
            {
                lightness = _MaxLightness_HSL;
            }
        }

        private static void _HSLToRGB(double hue, double saturation, double lightness, out double red, out double green, out double blue) // 将颜色在 HSL 色彩空间的各分量转换为在 RGB 色彩空间的各分量。此函数不检查输入参数的合法性，但保证输出参数的合法性。
        {
            hue /= _MaxHue_HSL;
            saturation /= _MaxSaturation_HSL;
            lightness /= _MaxLightness_HSL;

            int Hi = (int)Math.Floor(hue * 6);

            if (Hi == 6)
            {
                hue = 0;
            }

            Hi %= 6;

            double Hf = hue * 6 - Hi;

            double C = (1 - Math.Abs(2 * lightness - 1)) * saturation;

            double X = C * (1 - Math.Abs(Hi % 2 + Hf - 1));

            switch (Hi)
            {
                case 0:
                    {
                        red = C;
                        green = X;
                        blue = 0;
                    }
                    break;

                case 1:
                    {
                        red = X;
                        green = C;
                        blue = 0;
                    }
                    break;

                case 2:
                    {
                        red = 0;
                        green = C;
                        blue = X;
                    }
                    break;

                case 3:
                    {
                        red = 0;
                        green = X;
                        blue = C;
                    }
                    break;

                case 4:
                    {
                        red = X;
                        green = 0;
                        blue = C;
                    }
                    break;

                case 5:
                    {
                        red = C;
                        green = 0;
                        blue = X;
                    }
                    break;

                default:
                    {
                        red = 0;
                        green = 0;
                        blue = 0;
                    }
                    break;
            }

            double M = lightness - C / 2;

            red += M;
            green += M;
            blue += M;

            red *= _MaxRed;
            green *= _MaxGreen;
            blue *= _MaxBlue;

            if (red < _MinRed)
            {
                red = _MinRed;
            }
            else if (red > _MaxRed)
            {
                red = _MaxRed;
            }

            if (green < _MinGreen)
            {
                green = _MinGreen;
            }
            else if (green > _MaxGreen)
            {
                green = _MaxGreen;
            }

            if (blue < _MinBlue)
            {
                blue = _MinBlue;
            }
            else if (blue > _MaxBlue)
            {
                blue = _MaxBlue;
            }
        }

        private static void _RGBToCMYK(double red, double green, double blue, out double cyan, out double magenta, out double yellow, out double black) // 将颜色在 RGB 色彩空间的各分量转换为在 CMYK 色彩空间的各分量。此函数不检查输入参数的合法性，但保证输出参数的合法性。
        {
            red /= _MaxRed;
            green /= _MaxGreen;
            blue /= _MaxBlue;

            double RgbMax = red;

            if (RgbMax < green)
            {
                RgbMax = green;
            }

            if (RgbMax < blue)
            {
                RgbMax = blue;
            }

            if (RgbMax == 0)
            {
                cyan = 0;
                magenta = 0;
                yellow = 0;
                black = 1;
            }
            else
            {
                cyan = (RgbMax - red) / RgbMax;
                magenta = (RgbMax - green) / RgbMax;
                yellow = (RgbMax - blue) / RgbMax;
                black = 1 - RgbMax;
            }

            cyan *= _MaxCyan;
            magenta *= _MaxMagenta;
            yellow *= _MaxYellow;
            black *= _MaxBlack;

            if (cyan < _MinCyan)
            {
                cyan = _MinCyan;
            }
            else if (cyan > _MaxCyan)
            {
                cyan = _MaxCyan;
            }

            if (magenta < _MinMagenta)
            {
                magenta = _MinMagenta;
            }
            else if (magenta > _MaxMagenta)
            {
                magenta = _MaxMagenta;
            }

            if (yellow < _MinYellow)
            {
                yellow = _MinYellow;
            }
            else if (yellow > _MaxYellow)
            {
                yellow = _MaxYellow;
            }

            if (black < _MinBlack)
            {
                black = _MinBlack;
            }
            else if (black > _MaxBlack)
            {
                black = _MaxBlack;
            }
        }

        private static void _CMYKToRGB(double cyan, double magenta, double yellow, double black, out double red, out double green, out double blue) // 将颜色在 CMYK 色彩空间的各分量转换为在 RGB 色彩空间的各分量。此函数不检查输入参数的合法性，但保证输出参数的合法性。
        {
            cyan /= _MaxCyan;
            magenta /= _MaxMagenta;
            yellow /= _MaxYellow;
            black /= _MaxBlack;

            if (black == 1)
            {
                red = 0;
                green = 0;
                blue = 0;
            }
            else
            {
                red = (1 - cyan) * (1 - black);
                green = (1 - magenta) * (1 - black);
                blue = (1 - yellow) * (1 - black);
            }

            red *= _MaxRed;
            green *= _MaxGreen;
            blue *= _MaxBlue;

            if (red < _MinRed)
            {
                red = _MinRed;
            }
            else if (red > _MaxRed)
            {
                red = _MaxRed;
            }

            if (green < _MinGreen)
            {
                green = _MinGreen;
            }
            else if (green > _MaxGreen)
            {
                green = _MaxGreen;
            }

            if (blue < _MinBlue)
            {
                blue = _MinBlue;
            }
            else if (blue > _MaxBlue)
            {
                blue = _MaxBlue;
            }
        }

        private static void _RGBToLAB(double red, double green, double blue, out double lightness, out double greenRed, out double blueYellow) // 将颜色在 RGB 色彩空间的各分量转换为在 LAB 色彩空间的各分量。此函数不检查输入参数的合法性，但保证输出参数的合法性。
        {
            red /= _MaxRed;
            green /= _MaxGreen;
            blue /= _MaxBlue;

            Func<double, double> Frgb = (t) =>
            {
                if (t > 0.04045)
                {
                    return Math.Pow((t + 0.055) / 1.055, 2.4);
                }
                else
                {
                    return (t / 12.92);
                }
            };

            double R = Frgb(red);
            double G = Frgb(green);
            double B = Frgb(blue);

            double X = 0.433953 * R + 0.376219 * G + 0.189828 * B;
            double Y = 0.212671 * R + 0.715160 * G + 0.072169 * B;
            double Z = 0.017758 * R + 0.109477 * G + 0.872765 * B;

            Func<double, double> Fxyz = (t) =>
            {
                const double Delta = 6.0 / 29;

                if (t > Delta * Delta * Delta)
                {
                    return Math.Pow(t, 1.0 / 3);
                }
                else
                {
                    return (t / (3 * Delta * Delta) + 4.0 / 29);
                }
            };

            double Fx = Fxyz(X);
            double Fy = Fxyz(Y);
            double Fz = Fxyz(Z);

            lightness = 116 * Fy - 16;
            greenRed = 500 * (Fx - Fy);
            blueYellow = 200 * (Fy - Fz);

            if (lightness < _MinLightness_LAB)
            {
                lightness = _MinLightness_LAB;
            }
            else if (lightness > _MaxLightness_LAB)
            {
                lightness = _MaxLightness_LAB;
            }

            if (greenRed < _MinGreenRed)
            {
                greenRed = _MinGreenRed;
            }
            else if (greenRed > _MaxGreenRed)
            {
                greenRed = _MaxGreenRed;
            }

            if (blueYellow < _MinBlueYellow)
            {
                blueYellow = _MinBlueYellow;
            }
            else if (blueYellow > _MaxBlueYellow)
            {
                blueYellow = _MaxBlueYellow;
            }
        }

        private static void _LABToRGB(double lightness, double greenRed, double blueYellow, out double red, out double green, out double blue) // 将颜色在 LAB 色彩空间的各分量转换为在 RGB 色彩空间的各分量。此函数不检查输入参数的合法性，但保证输出参数的合法性。
        {
            double L = (lightness + 16) / 116;
            double a = greenRed / 500;
            double b = blueYellow / 200;

            Func<double, double> Fxyz = (t) =>
            {
                const double Delta = 6.0 / 29;

                if (t > Delta)
                {
                    return (t * t * t);
                }
                else
                {
                    return ((t - 4.0 / 29) * (3 * Delta * Delta));
                }
            };

            double X = Fxyz(L + a);
            double Y = Fxyz(L);
            double Z = Fxyz(L - b);

            double R = 3.079932708424 * X - 1.537150 * Y - 0.54278197539 * Z;
            double G = -0.921235180736 * X + 1.875991 * Y + 0.045244261224 * Z;
            double B = 0.052890975488 * X - 0.204043 * Y + 1.151151580494 * Z;

            Func<double, double> Frgb = (t) =>
            {
                if (t > 0.04045 / 12.92)
                {
                    return (Math.Pow(t, 1 / 2.4) * 1.055 - 0.055);
                }
                else
                {
                    return (t * 12.92);
                }
            };

            red = Frgb(R);
            green = Frgb(G);
            blue = Frgb(B);

            red *= _MaxRed;
            green *= _MaxGreen;
            blue *= _MaxBlue;

            if (red < _MinRed)
            {
                red = _MinRed;
            }
            else if (red > _MaxRed)
            {
                red = _MaxRed;
            }

            if (green < _MinGreen)
            {
                green = _MinGreen;
            }
            else if (green > _MaxGreen)
            {
                green = _MaxGreen;
            }

            if (blue < _MinBlue)
            {
                blue = _MinBlue;
            }
            else if (blue > _MaxBlue)
            {
                blue = _MaxBlue;
            }
        }

        private static void _RGBToYUV(double red, double green, double blue, out double luminance, out double chrominanceBlue, out double chrominanceRed) // 将颜色在 RGB 色彩空间的各分量转换为在 YUV 色彩空间的各分量。此函数不检查输入参数的合法性，但保证输出参数的合法性。
        {
            red /= _MaxRed;
            green /= _MaxGreen;
            blue /= _MaxBlue;

            luminance = 0.299 * red + 0.587 * green + 0.114 * blue;
            chrominanceBlue = (blue - luminance) / 1.772;
            chrominanceRed = (red - luminance) / 1.402;

            if (luminance < _MinLuminance)
            {
                luminance = _MinLuminance;
            }
            else if (luminance > _MaxLuminance)
            {
                luminance = _MaxLuminance;
            }

            if (chrominanceBlue < _MinChrominanceBlue)
            {
                chrominanceBlue = _MinChrominanceBlue;
            }
            else if (chrominanceBlue > _MaxChrominanceBlue)
            {
                chrominanceBlue = _MaxChrominanceBlue;
            }

            if (chrominanceRed < _MinChrominanceRed)
            {
                chrominanceRed = _MinChrominanceRed;
            }
            else if (chrominanceRed > _MaxChrominanceRed)
            {
                chrominanceRed = _MaxChrominanceRed;
            }
        }

        private static void _YUVToRGB(double luminance, double chrominanceBlue, double chrominanceRed, out double red, out double green, out double blue) // 将颜色在 YUV 色彩空间的各分量转换为在 RGB 色彩空间的各分量。此函数不检查输入参数的合法性，但保证输出参数的合法性。
        {
            red = luminance + 1.402 * chrominanceRed;
            green = luminance - (0.202008 * chrominanceBlue + 0.419198 * chrominanceRed) / 0.587;
            blue = luminance + 1.772 * chrominanceBlue;

            red *= _MaxRed;
            green *= _MaxGreen;
            blue *= _MaxBlue;

            if (red < _MinRed)
            {
                red = _MinRed;
            }
            else if (red > _MaxRed)
            {
                red = _MaxRed;
            }

            if (green < _MinGreen)
            {
                green = _MinGreen;
            }
            else if (green > _MaxGreen)
            {
                green = _MaxGreen;
            }

            if (blue < _MinBlue)
            {
                blue = _MinBlue;
            }
            else if (blue > _MaxBlue)
            {
                blue = _MaxBlue;
            }
        }

        //

        private _ColorSpace _CurrentColorSpace; // 表示当前使用的色彩空间。

        private double _Opacity; // 颜色的不透明度。

        private double _Channel1; // 颜色在当前色彩空间的第 1 个分量。
        private double _Channel2; // 颜色在当前色彩空间的第 2 个分量。
        private double _Channel3; // 颜色在当前色彩空间的第 3 个分量。
        private double _Channel4; // 颜色在当前色彩空间的第 4 个分量。

        private Hashtable _CachedChannels; // 用于缓存颜色在其他色彩空间的分量。

        //

        private double[] _GetChannels(_ColorSpace colorSpace) // 获取颜色在指定色彩空间的所有分量。
        {
            if (colorSpace == _ColorSpace.None)
            {
                throw new ArgumentException();
            }

            //

            if (_CurrentColorSpace == _ColorSpace.None)
            {
                switch (colorSpace)
                {
                    case _ColorSpace.RGB: return new double[_ChannelCount] { _DefaultRed, _DefaultGreen, _DefaultBlue, 0 };
                    case _ColorSpace.HSV: return new double[_ChannelCount] { _DefaultHue_HSV, _DefaultSaturation_HSV, _DefaultBrightness, 0 };
                    case _ColorSpace.HSL: return new double[_ChannelCount] { _DefaultHue_HSL, _DefaultSaturation_HSL, _DefaultLightness_HSL, 0 };
                    case _ColorSpace.CMYK: return new double[_ChannelCount] { _DefaultCyan, _DefaultMagenta, _DefaultYellow, _DefaultBlack };
                    case _ColorSpace.LAB: return new double[_ChannelCount] { _DefaultLightness_LAB, _DefaultGreenRed, _DefaultBlueYellow, 0 };
                    case _ColorSpace.YUV: return new double[_ChannelCount] { _DefaultLuminance, _DefaultChrominanceBlue, _DefaultChrominanceRed, 0 };
                    default: return new double[_ChannelCount];
                }
            }
            else if (colorSpace == _CurrentColorSpace)
            {
                return new double[_ChannelCount] { _Channel1, _Channel2, _Channel3, _Channel4 };
            }
            else if (_CachedChannels != null && _CachedChannels.Contains(colorSpace))
            {
                return (_CachedChannels[colorSpace] as double[]);
            }
            else
            {
                double[] channels = new double[_ChannelCount];

                if (colorSpace == _ColorSpace.RGB)
                {
                    switch (_CurrentColorSpace)
                    {
                        case _ColorSpace.HSV:
                            _HSVToRGB(_Channel1, _Channel2, _Channel3, out channels[0], out channels[1], out channels[2]);
                            break;

                        case _ColorSpace.HSL:
                            _HSLToRGB(_Channel1, _Channel2, _Channel3, out channels[0], out channels[1], out channels[2]);
                            break;

                        case _ColorSpace.CMYK:
                            _CMYKToRGB(_Channel1, _Channel2, _Channel3, _Channel4, out channels[0], out channels[1], out channels[2]);
                            break;

                        case _ColorSpace.LAB:
                            _LABToRGB(_Channel1, _Channel2, _Channel3, out channels[0], out channels[1], out channels[2]);
                            break;

                        case _ColorSpace.YUV:
                            _YUVToRGB(_Channel1, _Channel2, _Channel3, out channels[0], out channels[1], out channels[2]);
                            break;
                    }
                }
                else
                {
                    double R = _DefaultRed;
                    double G = _DefaultGreen;
                    double B = _DefaultBlue;

                    if (_CurrentColorSpace == _ColorSpace.RGB)
                    {
                        R = _Channel1;
                        G = _Channel2;
                        B = _Channel3;
                    }
                    else if (_CachedChannels != null && _CachedChannels.Contains(_ColorSpace.RGB))
                    {
                        double[] rgb = _CachedChannels[_ColorSpace.RGB] as double[];

                        R = rgb[0];
                        G = rgb[1];
                        B = rgb[2];
                    }
                    else
                    {
                        switch (_CurrentColorSpace)
                        {
                            case _ColorSpace.HSV:
                                _HSVToRGB(_Channel1, _Channel2, _Channel3, out R, out G, out B);
                                break;

                            case _ColorSpace.HSL:
                                _HSLToRGB(_Channel1, _Channel2, _Channel3, out R, out G, out B);
                                break;

                            case _ColorSpace.CMYK:
                                _CMYKToRGB(_Channel1, _Channel2, _Channel3, _Channel4, out R, out G, out B);
                                break;

                            case _ColorSpace.LAB:
                                _LABToRGB(_Channel1, _Channel2, _Channel3, out R, out G, out B);
                                break;

                            case _ColorSpace.YUV:
                                _YUVToRGB(_Channel1, _Channel2, _Channel3, out R, out G, out B);
                                break;
                        }

                        if (_CachedChannels == null)
                        {
                            _CachedChannels = new Hashtable();
                        }

                        _CachedChannels.Add(_ColorSpace.RGB, new double[_ChannelCount] { R, G, B, 0 });
                    }

                    switch (colorSpace)
                    {
                        case _ColorSpace.HSV:
                            _RGBToHSV(R, G, B, out channels[0], out channels[1], out channels[2]);
                            break;

                        case _ColorSpace.HSL:
                            _RGBToHSL(R, G, B, out channels[0], out channels[1], out channels[2]);
                            break;

                        case _ColorSpace.CMYK:
                            _RGBToCMYK(R, G, B, out channels[0], out channels[1], out channels[2], out channels[3]);
                            break;

                        case _ColorSpace.LAB:
                            _RGBToLAB(R, G, B, out channels[0], out channels[1], out channels[2]);
                            break;

                        case _ColorSpace.YUV:
                            _RGBToYUV(R, G, B, out channels[0], out channels[1], out channels[2]);
                            break;
                    }
                }

                if (_CachedChannels == null)
                {
                    _CachedChannels = new Hashtable();
                }

                _CachedChannels.Add(colorSpace, channels);

                return channels;
            }
        }

        private double _GetChannel(_ColorChannel colorChannel) // 获取颜色在指定色彩通道的分量。
        {
            if (colorChannel == _ColorChannel.None)
            {
                throw new ArgumentException();
            }

            //

            int channelIndex = (int)colorChannel % _ChannelToSpaceDivisor;
            _ColorSpace colorSpace = (_ColorSpace)((int)colorChannel - channelIndex);

            if (_CurrentColorSpace == _ColorSpace.None)
            {
                switch (colorChannel)
                {
                    case _ColorChannel.Red: return _DefaultRed;
                    case _ColorChannel.Green: return _DefaultGreen;
                    case _ColorChannel.Blue: return _DefaultBlue;

                    case _ColorChannel.Hue_HSV: return _DefaultHue_HSV;
                    case _ColorChannel.Saturation_HSV: return _DefaultSaturation_HSV;
                    case _ColorChannel.Brightness: return _DefaultBrightness;

                    case _ColorChannel.Hue_HSL: return _DefaultHue_HSL;
                    case _ColorChannel.Saturation_HSL: return _DefaultSaturation_HSL;
                    case _ColorChannel.Lightness_HSL: return _DefaultLightness_HSL;

                    case _ColorChannel.Cyan: return _DefaultCyan;
                    case _ColorChannel.Magenta: return _DefaultMagenta;
                    case _ColorChannel.Yellow: return _DefaultYellow;
                    case _ColorChannel.Black: return _DefaultBlack;

                    case _ColorChannel.Lightness_LAB: return _DefaultLightness_LAB;
                    case _ColorChannel.GreenRed: return _DefaultGreenRed;
                    case _ColorChannel.BlueYellow: return _DefaultBlueYellow;

                    case _ColorChannel.Luminance: return _DefaultLuminance;
                    case _ColorChannel.ChrominanceBlue: return _DefaultChrominanceBlue;
                    case _ColorChannel.ChrominanceRed: return _DefaultChrominanceRed;

                    default: return 0;
                }
            }
            else if (colorSpace == _CurrentColorSpace)
            {
                switch (channelIndex)
                {
                    case 0: return _Channel1;
                    case 1: return _Channel2;
                    case 2: return _Channel3;
                    case 3: return _Channel4;
                    default: return 0;
                }
            }
            else if (_CachedChannels != null && _CachedChannels.Contains(colorSpace))
            {
                return (_CachedChannels[colorSpace] as double[])[channelIndex];
            }
            else
            {
                double[] channels = new double[_ChannelCount];

                if (colorSpace == _ColorSpace.RGB)
                {
                    switch (_CurrentColorSpace)
                    {
                        case _ColorSpace.HSV:
                            _HSVToRGB(_Channel1, _Channel2, _Channel3, out channels[0], out channels[1], out channels[2]);
                            break;

                        case _ColorSpace.HSL:
                            _HSLToRGB(_Channel1, _Channel2, _Channel3, out channels[0], out channels[1], out channels[2]);
                            break;

                        case _ColorSpace.CMYK:
                            _CMYKToRGB(_Channel1, _Channel2, _Channel3, _Channel4, out channels[0], out channels[1], out channels[2]);
                            break;

                        case _ColorSpace.LAB:
                            _LABToRGB(_Channel1, _Channel2, _Channel3, out channels[0], out channels[1], out channels[2]);
                            break;

                        case _ColorSpace.YUV:
                            _YUVToRGB(_Channel1, _Channel2, _Channel3, out channels[0], out channels[1], out channels[2]);
                            break;
                    }
                }
                else
                {
                    double R = _DefaultRed;
                    double G = _DefaultGreen;
                    double B = _DefaultBlue;

                    if (_CurrentColorSpace == _ColorSpace.RGB)
                    {
                        R = _Channel1;
                        G = _Channel2;
                        B = _Channel3;
                    }
                    else if (_CachedChannels != null && _CachedChannels.Contains(_ColorSpace.RGB))
                    {
                        double[] rgb = _CachedChannels[_ColorSpace.RGB] as double[];

                        R = rgb[0];
                        G = rgb[1];
                        B = rgb[2];
                    }
                    else
                    {
                        switch (_CurrentColorSpace)
                        {
                            case _ColorSpace.HSV:
                                _HSVToRGB(_Channel1, _Channel2, _Channel3, out R, out G, out B);
                                break;

                            case _ColorSpace.HSL:
                                _HSLToRGB(_Channel1, _Channel2, _Channel3, out R, out G, out B);
                                break;

                            case _ColorSpace.CMYK:
                                _CMYKToRGB(_Channel1, _Channel2, _Channel3, _Channel4, out R, out G, out B);
                                break;

                            case _ColorSpace.LAB:
                                _LABToRGB(_Channel1, _Channel2, _Channel3, out R, out G, out B);
                                break;

                            case _ColorSpace.YUV:
                                _YUVToRGB(_Channel1, _Channel2, _Channel3, out R, out G, out B);
                                break;
                        }

                        if (_CachedChannels == null)
                        {
                            _CachedChannels = new Hashtable();
                        }

                        _CachedChannels.Add(_ColorSpace.RGB, new double[_ChannelCount] { R, G, B, 0 });
                    }

                    switch (colorSpace)
                    {
                        case _ColorSpace.HSV:
                            _RGBToHSV(R, G, B, out channels[0], out channels[1], out channels[2]);
                            break;

                        case _ColorSpace.HSL:
                            _RGBToHSL(R, G, B, out channels[0], out channels[1], out channels[2]);
                            break;

                        case _ColorSpace.CMYK:
                            _RGBToCMYK(R, G, B, out channels[0], out channels[1], out channels[2], out channels[3]);
                            break;

                        case _ColorSpace.LAB:
                            _RGBToLAB(R, G, B, out channels[0], out channels[1], out channels[2]);
                            break;

                        case _ColorSpace.YUV:
                            _RGBToYUV(R, G, B, out channels[0], out channels[1], out channels[2]);
                            break;
                    }
                }

                if (_CachedChannels == null)
                {
                    _CachedChannels = new Hashtable();
                }

                _CachedChannels.Add(colorSpace, channels);

                return channels[channelIndex];
            }
        }

        private void _SetChannels(_ColorSpace colorSpace, params double[] channels) // 设置颜色在指定色彩空间的所有分量。
        {
            if (colorSpace == _ColorSpace.None)
            {
                throw new ArgumentException();
            }

            if (InternalMethod.IsNullOrEmpty(channels))
            {
                throw new ArgumentNullException();
            }

            //

            switch (colorSpace)
            {
                case _ColorSpace.RGB:
                    {
                        _Channel1 = _CheckRed(channels[0]);
                        _Channel2 = _CheckGreen(channels[1]);
                        _Channel3 = _CheckBlue(channels[2]);
                        _Channel4 = 0;
                    }
                    break;

                case _ColorSpace.HSV:
                    {
                        _Channel1 = _CheckHue_HSV(channels[0]);
                        _Channel2 = _CheckSaturation_HSV(channels[1]);
                        _Channel3 = _CheckBrightness(channels[2]);
                        _Channel4 = 0;
                    }
                    break;

                case _ColorSpace.HSL:
                    {
                        _Channel1 = _CheckHue_HSL(channels[0]);
                        _Channel2 = _CheckSaturation_HSL(channels[1]);
                        _Channel3 = _CheckLightness_HSL(channels[2]);
                        _Channel4 = 0;
                    }
                    break;

                case _ColorSpace.CMYK:
                    {
                        _Channel1 = _CheckCyan(channels[0]);
                        _Channel2 = _CheckMagenta(channels[1]);
                        _Channel3 = _CheckYellow(channels[2]);
                        _Channel4 = _CheckBlack(channels[3]);
                    }
                    break;

                case _ColorSpace.LAB:
                    {
                        _Channel1 = _CheckLightness_LAB(channels[0]);
                        _Channel2 = _CheckGreenRed(channels[1]);
                        _Channel3 = _CheckBlueYellow(channels[2]);
                        _Channel4 = 0;
                    }
                    break;

                case _ColorSpace.YUV:
                    {
                        _Channel1 = _CheckLuminance(channels[0]);
                        _Channel2 = _CheckChrominanceBlue(channels[1]);
                        _Channel3 = _CheckChrominanceRed(channels[2]);
                        _Channel4 = 0;
                    }
                    break;
            }

            _CurrentColorSpace = colorSpace;

            _CachedChannels = null;
        }

        private void _SetChannel(_ColorChannel colorChannel, double channel) // 设置颜色在指定色彩通道的分量。
        {
            if (colorChannel == _ColorChannel.None)
            {
                throw new ArgumentException();
            }

            //

            switch (colorChannel)
            {
                case _ColorChannel.Red: channel = _CheckRed(channel); break;
                case _ColorChannel.Green: channel = _CheckGreen(channel); break;
                case _ColorChannel.Blue: channel = _CheckBlue(channel); break;

                case _ColorChannel.Hue_HSV: channel = _CheckHue_HSV(channel); break;
                case _ColorChannel.Saturation_HSV: channel = _CheckSaturation_HSV(channel); break;
                case _ColorChannel.Brightness: channel = _CheckBrightness(channel); break;

                case _ColorChannel.Hue_HSL: channel = _CheckHue_HSL(channel); break;
                case _ColorChannel.Saturation_HSL: channel = _CheckSaturation_HSL(channel); break;
                case _ColorChannel.Lightness_HSL: channel = _CheckLightness_HSL(channel); break;

                case _ColorChannel.Cyan: channel = _CheckCyan(channel); break;
                case _ColorChannel.Magenta: channel = _CheckMagenta(channel); break;
                case _ColorChannel.Yellow: channel = _CheckYellow(channel); break;
                case _ColorChannel.Black: channel = _CheckBlack(channel); break;

                case _ColorChannel.Lightness_LAB: channel = _CheckLightness_LAB(channel); break;
                case _ColorChannel.GreenRed: channel = _CheckGreenRed(channel); break;
                case _ColorChannel.BlueYellow: channel = _CheckBlueYellow(channel); break;
            }

            //

            int channelIndex = (int)colorChannel % _ChannelToSpaceDivisor;
            _ColorSpace colorSpace = (_ColorSpace)((int)colorChannel - channelIndex);

            if (_CurrentColorSpace == _ColorSpace.None)
            {
                switch (colorSpace)
                {
                    case _ColorSpace.RGB:
                        {
                            _Channel1 = _DefaultRed;
                            _Channel2 = _DefaultGreen;
                            _Channel3 = _DefaultBlue;
                            _Channel4 = 0;
                        }
                        break;

                    case _ColorSpace.HSV:
                        {
                            _Channel1 = _DefaultHue_HSV;
                            _Channel2 = _DefaultSaturation_HSV;
                            _Channel3 = _DefaultBrightness;
                            _Channel4 = 0;
                        }
                        break;

                    case _ColorSpace.HSL:
                        {
                            _Channel1 = _DefaultHue_HSL;
                            _Channel2 = _DefaultSaturation_HSL;
                            _Channel3 = _DefaultLightness_HSL;
                            _Channel4 = 0;
                        }
                        break;

                    case _ColorSpace.CMYK:
                        {
                            _Channel1 = _DefaultCyan;
                            _Channel2 = _DefaultMagenta;
                            _Channel3 = _DefaultYellow;
                            _Channel4 = _DefaultBlack;
                        }
                        break;

                    case _ColorSpace.LAB:
                        {
                            _Channel1 = _DefaultLightness_LAB;
                            _Channel2 = _DefaultGreenRed;
                            _Channel3 = _DefaultBlueYellow;
                            _Channel4 = 0;
                        }
                        break;

                    case _ColorSpace.YUV:
                        {
                            _Channel1 = _DefaultLuminance;
                            _Channel2 = _DefaultChrominanceBlue;
                            _Channel3 = _DefaultChrominanceRed;
                            _Channel4 = 0;
                        }
                        break;
                }
            }
            else if (colorSpace == _CurrentColorSpace)
            {
                goto SET_CHANNEL;
            }
            else if (_CachedChannels != null && _CachedChannels.Contains(colorSpace))
            {
                double[] channels = _CachedChannels[colorSpace] as double[];

                _Channel1 = channels[0];
                _Channel2 = channels[1];
                _Channel3 = channels[2];
                _Channel4 = channels[3];
            }
            else
            {
                if (colorSpace == _ColorSpace.RGB)
                {
                    switch (_CurrentColorSpace)
                    {
                        case _ColorSpace.HSV:
                            _HSVToRGB(_Channel1, _Channel2, _Channel3, out _Channel1, out _Channel2, out _Channel3);
                            break;

                        case _ColorSpace.HSL:
                            _HSLToRGB(_Channel1, _Channel2, _Channel3, out _Channel1, out _Channel2, out _Channel3);
                            break;

                        case _ColorSpace.CMYK:
                            _CMYKToRGB(_Channel1, _Channel2, _Channel3, _Channel4, out _Channel1, out _Channel2, out _Channel3);
                            break;

                        case _ColorSpace.LAB:
                            _LABToRGB(_Channel1, _Channel2, _Channel3, out _Channel1, out _Channel2, out _Channel3);
                            break;

                        case _ColorSpace.YUV:
                            _YUVToRGB(_Channel1, _Channel2, _Channel3, out _Channel1, out _Channel2, out _Channel3);
                            break;
                    }
                }
                else
                {
                    double R = _DefaultRed;
                    double G = _DefaultGreen;
                    double B = _DefaultBlue;

                    if (_CurrentColorSpace == _ColorSpace.RGB)
                    {
                        R = _Channel1;
                        G = _Channel2;
                        B = _Channel3;
                    }
                    else if (_CachedChannels != null && _CachedChannels.Contains(_ColorSpace.RGB))
                    {
                        double[] rgb = _CachedChannels[_ColorSpace.RGB] as double[];

                        R = rgb[0];
                        G = rgb[1];
                        B = rgb[2];
                    }
                    else
                    {
                        switch (_CurrentColorSpace)
                        {
                            case _ColorSpace.HSV:
                                _HSVToRGB(_Channel1, _Channel2, _Channel3, out R, out G, out B);
                                break;

                            case _ColorSpace.HSL:
                                _HSLToRGB(_Channel1, _Channel2, _Channel3, out R, out G, out B);
                                break;

                            case _ColorSpace.CMYK:
                                _CMYKToRGB(_Channel1, _Channel2, _Channel3, _Channel4, out R, out G, out B);
                                break;

                            case _ColorSpace.LAB:
                                _LABToRGB(_Channel1, _Channel2, _Channel3, out R, out G, out B);
                                break;

                            case _ColorSpace.YUV:
                                _YUVToRGB(_Channel1, _Channel2, _Channel3, out R, out G, out B);
                                break;
                        }

                        if (_CachedChannels == null)
                        {
                            _CachedChannels = new Hashtable();
                        }

                        _CachedChannels.Add(_ColorSpace.RGB, new double[_ChannelCount] { R, G, B, 0 });
                    }

                    switch (colorSpace)
                    {
                        case _ColorSpace.HSV:
                            _RGBToHSV(R, G, B, out _Channel1, out _Channel2, out _Channel3);
                            break;

                        case _ColorSpace.HSL:
                            _RGBToHSL(R, G, B, out _Channel1, out _Channel2, out _Channel3);
                            break;

                        case _ColorSpace.CMYK:
                            _RGBToCMYK(R, G, B, out _Channel1, out _Channel2, out _Channel3, out _Channel4);
                            break;

                        case _ColorSpace.LAB:
                            _RGBToLAB(R, G, B, out _Channel1, out _Channel2, out _Channel3);
                            break;

                        case _ColorSpace.YUV:
                            _RGBToYUV(R, G, B, out _Channel1, out _Channel2, out _Channel3);
                            break;
                    }
                }
            }

        SET_CHANNEL:
            switch (channelIndex)
            {
                case 0: _Channel1 = channel; break;
                case 1: _Channel2 = channel; break;
                case 2: _Channel3 = channel; break;
                case 3: _Channel4 = channel; break;
            }

            _CurrentColorSpace = colorSpace;

            _CachedChannels = null;
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 使用 Color 结构初始化 ColorX 结构的新实例。
        /// </summary>
        /// <param name="color">Color 结构。</param>
        public ColorX(Color color) : this()
        {
            if (!color.IsEmpty)
            {
                _SetChannels(_ColorSpace.RGB, color.R, color.G, color.B);
                Alpha = color.A;
            }
        }

        /// <summary>
        /// 使用颜色的 32 位 ARGB 值初始化 ColorX 结构的新实例。
        /// </summary>
        /// <param name="argb">颜色在 RGB 色彩空间的 32 位 ARGB 值。</param>
        public ColorX(int argb) : this()
        {
            _SetChannels(_ColorSpace.RGB, ((uint)argb >> _32BitArgbRedShift) & 0xFFU, ((uint)argb >> _32BitArgbGreenShift) & 0xFFU, ((uint)argb >> _32BitArgbBlueShift) & 0xFFU);
            Alpha = ((uint)argb >> _32BitArgbAlphaShift) & 0xFFU;
        }

        /// <summary>
        /// 使用颜色的 16 进制 ARGB 码或 RGB 码初始化 ColorX 结构的新实例。
        /// </summary>
        /// <param name="hexCode">颜色的 16 进制 ARGB 码或 RGB 码。</param>
        public ColorX(string hexCode) : this()
        {
            if (string.IsNullOrEmpty(hexCode))
            {
                throw new ArgumentNullException();
            }

            string HexCode = new Regex(@"[^A-Za-z\d]").Replace(hexCode, string.Empty);

            if (string.IsNullOrEmpty(HexCode))
            {
                throw new ArgumentNullException();
            }

            if (HexCode.Length > 8)
            {
                throw new OverflowException();
            }

            //

            int Argb = int.Parse(HexCode, NumberStyles.HexNumber);

            _SetChannels(_ColorSpace.RGB, ((uint)Argb >> _32BitArgbRedShift) & 0xFFU, ((uint)Argb >> _32BitArgbGreenShift) & 0xFFU, ((uint)Argb >> _32BitArgbBlueShift) & 0xFFU);
            Alpha = ((uint)Argb >> _32BitArgbAlphaShift) & 0xFFU;
        }

        #endregion

        #region 字段

        /// <summary>
        /// 表示所有属性为其数据类型的默认值的 ColorX 结构的实例。
        /// </summary>
        public static readonly ColorX Empty = new ColorX();

        /// <summary>
        /// 表示透明色的 ColorX 结构的实例。
        /// </summary>
        public static readonly ColorX Transparent = FromRGB(_MinAlpha, _MaxRed, _MaxGreen, _MaxBlue);

        #endregion

        #region 属性

        /// <summary>
        /// 获取表示此 ColorX 结构是否未初始化的布尔值。
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return (_CurrentColorSpace == _ColorSpace.None);
            }
        }

        /// <summary>
        /// 获取表示此 ColorX 结构是否为透明色的布尔值。
        /// </summary>
        public bool IsTransparent
        {
            get
            {
                return (_Opacity == _MinOpacity);
            }
        }

        //

        /// <summary>
        /// 获取或设置此 ColorX 结构的不透明度。
        /// </summary>
        public double Opacity
        {
            get
            {
                return _Opacity;
            }

            set
            {
                _Opacity = _CheckOpacity(value);

                //

                if (_CurrentColorSpace == _ColorSpace.None)
                {
                    _CurrentColorSpace = _ColorSpace.RGB;
                }
            }
        }

        /// <summary>
        /// 获取或设置此 ColorX 结构的 Alpha 通道（A）的值。
        /// </summary>
        public double Alpha
        {
            get
            {
                double alpha;

                _OpacityToAlpha(_Opacity, out alpha);

                return alpha;
            }

            set
            {
                _AlphaToOpacity(_CheckAlpha(value), out _Opacity);

                //

                if (_CurrentColorSpace == _ColorSpace.None)
                {
                    _CurrentColorSpace = _ColorSpace.RGB;
                }
            }
        }

        //

        /// <summary>
        /// 获取或设置此 ColorX 结构在 RGB 色彩空间的红色通道（R）的值。
        /// </summary>
        public double Red
        {
            get
            {
                return _GetChannel(_ColorChannel.Red);
            }

            set
            {
                _SetChannel(_ColorChannel.Red, value);
            }
        }

        /// <summary>
        /// 获取或设置此 ColorX 结构在 RGB 色彩空间的绿色通道（G）的值。
        /// </summary>
        public double Green
        {
            get
            {
                return _GetChannel(_ColorChannel.Green);
            }

            set
            {
                _SetChannel(_ColorChannel.Green, value);
            }
        }

        /// <summary>
        /// 获取或设置此 ColorX 结构在 RGB 色彩空间的蓝色通道（B）的值。
        /// </summary>
        public double Blue
        {
            get
            {
                return _GetChannel(_ColorChannel.Blue);
            }

            set
            {
                _SetChannel(_ColorChannel.Blue, value);
            }
        }

        //

        /// <summary>
        /// 获取或设置此 ColorX 结构在 HSV 色彩空间的色相（H）。
        /// </summary>
        public double Hue_HSV
        {
            get
            {
                return _GetChannel(_ColorChannel.Hue_HSV);
            }

            set
            {
                _SetChannel(_ColorChannel.Hue_HSV, value);
            }
        }

        /// <summary>
        /// 获取或设置此 ColorX 结构在 HSV 色彩空间的饱和度（S）。
        /// </summary>
        public double Saturation_HSV
        {
            get
            {
                return _GetChannel(_ColorChannel.Saturation_HSV);
            }

            set
            {
                _SetChannel(_ColorChannel.Saturation_HSV, value);
            }
        }

        /// <summary>
        /// 获取或设置此 ColorX 结构在 HSV 色彩空间的亮度（V）。
        /// </summary>
        public double Brightness
        {
            get
            {
                return _GetChannel(_ColorChannel.Brightness);
            }

            set
            {
                _SetChannel(_ColorChannel.Brightness, value);
            }
        }

        //

        /// <summary>
        /// 获取或设置此 ColorX 结构在 HSL 色彩空间的色相（H）。
        /// </summary>
        public double Hue_HSL
        {
            get
            {
                return _GetChannel(_ColorChannel.Hue_HSL);
            }

            set
            {
                _SetChannel(_ColorChannel.Hue_HSL, value);
            }
        }

        /// <summary>
        /// 获取或设置此 ColorX 结构在 HSL 色彩空间的饱和度（S）。
        /// </summary>
        public double Saturation_HSL
        {
            get
            {
                return _GetChannel(_ColorChannel.Saturation_HSL);
            }

            set
            {
                _SetChannel(_ColorChannel.Saturation_HSL, value);
            }
        }

        /// <summary>
        /// 获取或设置此 ColorX 结构在 HSL 色彩空间的明度（L）。
        /// </summary>
        public double Lightness_HSL
        {
            get
            {
                return _GetChannel(_ColorChannel.Lightness_HSL);
            }

            set
            {
                _SetChannel(_ColorChannel.Lightness_HSL, value);
            }
        }

        //

        /// <summary>
        /// 获取或设置此 ColorX 结构在 CMYK 色彩空间的青色通道（C）的值。
        /// </summary>
        public double Cyan
        {
            get
            {
                return _GetChannel(_ColorChannel.Cyan);
            }

            set
            {
                _SetChannel(_ColorChannel.Cyan, value);
            }
        }

        /// <summary>
        /// 获取或设置此 ColorX 结构在 CMYK 色彩空间的洋红色通道（M）的值。
        /// </summary>
        public double Magenta
        {
            get
            {
                return _GetChannel(_ColorChannel.Magenta);
            }

            set
            {
                _SetChannel(_ColorChannel.Magenta, value);
            }
        }

        /// <summary>
        /// 获取或设置此 ColorX 结构在 CMYK 色彩空间的黄色通道（Y）的值。
        /// </summary>
        public double Yellow
        {
            get
            {
                return _GetChannel(_ColorChannel.Yellow);
            }

            set
            {
                _SetChannel(_ColorChannel.Yellow, value);
            }
        }

        /// <summary>
        /// 获取或设置此 ColorX 结构在 CMYK 色彩空间的黑色通道（K）的值。
        /// </summary>
        public double Black
        {
            get
            {
                return _GetChannel(_ColorChannel.Black);
            }

            set
            {
                _SetChannel(_ColorChannel.Black, value);
            }
        }

        //

        /// <summary>
        /// 获取或设置此 ColorX 结构在 LAB 色彩空间的明度（L）。
        /// </summary>
        public double Lightness_LAB
        {
            get
            {
                return _GetChannel(_ColorChannel.Lightness_LAB);
            }

            set
            {
                _SetChannel(_ColorChannel.Lightness_LAB, value);
            }
        }

        /// <summary>
        /// 获取或设置此 ColorX 结构在 LAB 色彩空间的绿色-红色通道（A）的值。
        /// </summary>
        public double GreenRed
        {
            get
            {
                return _GetChannel(_ColorChannel.GreenRed);
            }

            set
            {
                _SetChannel(_ColorChannel.GreenRed, value);
            }
        }

        /// <summary>
        /// 获取或设置此 ColorX 结构在 LAB 色彩空间的蓝色-黄色通道（B）的值。
        /// </summary>
        public double BlueYellow
        {
            get
            {
                return _GetChannel(_ColorChannel.BlueYellow);
            }

            set
            {
                _SetChannel(_ColorChannel.BlueYellow, value);
            }
        }

        //

        /// <summary>
        /// 获取或设置此 ColorX 结构在 YUV 色彩空间的亮度（Y）。
        /// </summary>
        public double Luminance
        {
            get
            {
                return _GetChannel(_ColorChannel.Luminance);
            }

            set
            {
                _SetChannel(_ColorChannel.Luminance, value);
            }
        }

        /// <summary>
        /// 获取或设置此 ColorX 结构在 YUV 色彩空间的蓝色色度（U）。
        /// </summary>
        public double ChrominanceBlue
        {
            get
            {
                return _GetChannel(_ColorChannel.ChrominanceBlue);
            }

            set
            {
                _SetChannel(_ColorChannel.ChrominanceBlue, value);
            }
        }

        /// <summary>
        /// 获取或设置此 ColorX 结构在 YUV 色彩空间的红色色度（V）。
        /// </summary>
        public double ChrominanceRed
        {
            get
            {
                return _GetChannel(_ColorChannel.ChrominanceRed);
            }

            set
            {
                _SetChannel(_ColorChannel.ChrominanceRed, value);
            }
        }

        //

        /// <summary>
        /// 获取或设置表示此 ColorX 结构在 RGB 色彩空间的各分量的 PointD3D 结构。
        /// </summary>
        public PointD3D RGB
        {
            get
            {
                double[] channels = _GetChannels(_ColorSpace.RGB);

                return new PointD3D(channels[0], channels[1], channels[2]);
            }

            set
            {
                _SetChannels(_ColorSpace.RGB, value.ToArray());
            }
        }

        /// <summary>
        /// 获取或设置表示此 ColorX 结构在 HSV 色彩空间的各分量的 PointD3D 结构。
        /// </summary>
        public PointD3D HSV
        {
            get
            {
                double[] channels = _GetChannels(_ColorSpace.HSV);

                return new PointD3D(channels[0], channels[1], channels[2]);
            }

            set
            {
                _SetChannels(_ColorSpace.HSV, value.ToArray());
            }
        }

        /// <summary>
        /// 获取或设置表示此 ColorX 结构在 HSL 色彩空间的各分量的 PointD3D 结构。
        /// </summary>
        public PointD3D HSL
        {
            get
            {
                double[] channels = _GetChannels(_ColorSpace.HSL);

                return new PointD3D(channels[0], channels[1], channels[2]);
            }

            set
            {
                _SetChannels(_ColorSpace.HSL, value.ToArray());
            }
        }

        /// <summary>
        /// 获取或设置表示此 ColorX 结构在 CMYK 色彩空间的各分量的 PointD4D 结构。
        /// </summary>
        public PointD4D CMYK
        {
            get
            {
                double[] channels = _GetChannels(_ColorSpace.CMYK);

                return new PointD4D(channels[0], channels[1], channels[2], channels[3]);
            }

            set
            {
                _SetChannels(_ColorSpace.CMYK, value.ToArray());
            }
        }

        /// <summary>
        /// 获取或设置表示此 ColorX 结构在 LAB 色彩空间的各分量的 PointD3D 结构。
        /// </summary>
        public PointD3D LAB
        {
            get
            {
                double[] channels = _GetChannels(_ColorSpace.LAB);

                return new PointD3D(channels[0], channels[1], channels[2]);
            }

            set
            {
                _SetChannels(_ColorSpace.LAB, value.ToArray());
            }
        }

        /// <summary>
        /// 获取或设置表示此 ColorX 结构在 YUV 色彩空间的各分量的 PointD3D 结构。
        /// </summary>
        public PointD3D YUV
        {
            get
            {
                double[] channels = _GetChannels(_ColorSpace.YUV);

                return new PointD3D(channels[0], channels[1], channels[2]);
            }

            set
            {
                _SetChannels(_ColorSpace.YUV, value.ToArray());
            }
        }

        //

        /// <summary>
        /// 获取此 ColorX 结构的互补色。
        /// </summary>
        public ColorX ComplementaryColor
        {
            get
            {
                ColorX color = new ColorX();

                double[] rgb = _GetChannels(_ColorSpace.RGB);

                color._SetChannels(_ColorSpace.RGB, _MaxRed - rgb[0], _MaxGreen - rgb[1], _MaxBlue - rgb[2]);
                color.Alpha = Alpha;

                return color;
            }
        }

        /// <summary>
        /// 获取此 ColorX 结构的灰度颜色。
        /// </summary>
        public ColorX GrayscaleColor
        {
            get
            {
                ColorX color = new ColorX();

                double[] rgb = _GetChannels(_ColorSpace.RGB);

                double Y = 0.299 * rgb[0] + 0.587 * rgb[1] + 0.114 * rgb[2];

                color._SetChannels(_ColorSpace.RGB, Y, Y, Y);
                color.Alpha = Alpha;

                return color;
            }
        }

        //

        /// <summary>
        /// 获取此 ColorX 结构的 16 进制 ARGB 码。
        /// </summary>
        public string ARGBHexCode
        {
            get
            {
                double[] rgb = _GetChannels(_ColorSpace.RGB);

                int Argb = (int)(((uint)Math.Round(Alpha) << _32BitArgbAlphaShift) | ((uint)Math.Round(rgb[0]) << _32BitArgbRedShift) | ((uint)Math.Round(rgb[1]) << _32BitArgbGreenShift) | ((uint)Math.Round(rgb[2]) << _32BitArgbBlueShift));

                string HexCode = Convert.ToString(Argb, 16).ToUpper();

                int Len = HexCode.Length;

                if (Len < 8)
                {
                    return ("#" + HexCode.PadLeft(8, '0'));
                }
                else
                {
                    return ("#" + HexCode);
                }
            }
        }

        /// <summary>
        /// 获取此 ColorX 结构的 16 进制 RGB 码。
        /// </summary>
        public string RGBHexCode
        {
            get
            {
                double[] rgb = _GetChannels(_ColorSpace.RGB);

                int Rgb = (int)(((uint)Math.Round(rgb[0]) << _32BitArgbRedShift) | ((uint)Math.Round(rgb[1]) << _32BitArgbGreenShift) | ((uint)Math.Round(rgb[2]) << _32BitArgbBlueShift));

                string HexCode = Convert.ToString(Rgb, 16).ToUpper();

                int Len = HexCode.Length;

                if (Len < 6)
                {
                    return ("#" + HexCode.PadLeft(6, '0'));
                }
                else
                {
                    return ("#" + HexCode);
                }
            }
        }

        //

        /// <summary>
        /// 获取此 ColorX 结构的名称。
        /// </summary>
        public string Name
        {
            get
            {
                double[] rgb = _GetChannels(_ColorSpace.RGB);

                int Argb = (int)(((uint)Math.Round(Alpha) << _32BitArgbAlphaShift) | ((uint)Math.Round(rgb[0]) << _32BitArgbRedShift) | ((uint)Math.Round(rgb[1]) << _32BitArgbGreenShift) | ((uint)Math.Round(rgb[2]) << _32BitArgbBlueShift));

                string name = _GetNameByArgb(Argb);

                if (string.IsNullOrWhiteSpace(name))
                {
                    string HexCode = Convert.ToString(Argb, 16).ToUpper();

                    int Len = HexCode.Length;

                    if (Len < 8)
                    {
                        name = "#" + HexCode.PadLeft(8, '0');
                    }
                    else
                    {
                        name = "#" + HexCode;
                    }
                }

                return name;
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 判断此 ColorX 结构是否与指定的对象相等。
        /// </summary>
        /// <param name="obj">用于比较的对象。</param>
        /// <returns>布尔值，表示此 ColorX 结构是否与指定的对象相等。</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is ColorX))
            {
                return false;
            }
            else if (object.ReferenceEquals(this, obj))
            {
                return true;
            }
            else
            {
                return Equals((ColorX)obj);
            }
        }

        /// <summary>
        /// 返回此 ColorX 结构的哈希代码。
        /// </summary>
        /// <returns>32 位整数，表示此 ColorX 结构的哈希代码。</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// 将此 ColorX 结构转换为字符串。
        /// </summary>
        /// <returns>字符串，表示此 ColorX 结构的字符串形式。</returns>
        public override string ToString()
        {
            string Str = string.Empty;

            switch (_CurrentColorSpace)
            {
                case _ColorSpace.RGB: Str = string.Concat("A=", Alpha, ", R=", _Channel1, ", G=", _Channel2, ", B=", _Channel3); break;
                case _ColorSpace.HSV: Str = string.Concat("H=", _Channel1, ", S=", _Channel2, ", V=", _Channel3, ", Opacity=", Opacity, "%"); break;
                case _ColorSpace.HSL: Str = string.Concat("H=", _Channel1, ", S=", _Channel2, ", L=", _Channel3, ", Opacity=", Opacity, "%"); break;
                case _ColorSpace.CMYK: Str = string.Concat("C=", _Channel1, ", M=", _Channel2, ", Y=", _Channel3, ", K=", _Channel4, ", Opacity=", Opacity, "%"); break;
                case _ColorSpace.LAB: Str = string.Concat("L=", _Channel1, ", a=", _Channel2, ", b=", _Channel3, ", Opacity=", Opacity, "%"); break;
                case _ColorSpace.YUV: Str = string.Concat("Y=", _Channel1, ", U=", _Channel2, ", V=", _Channel3, ", Opacity=", Opacity, "%"); break;
                default: Str = "Empty"; break;
            }

            return string.Concat(base.GetType().Name, " [", Str, "]");
        }

        //

        /// <summary>
        /// 判断此 ColorX 结构是否与指定的 ColorX 结构相等。
        /// </summary>
        /// <param name="color">用于比较的 ColorX 结构。</param>
        /// <returns>布尔值，表示此 ColorX 结构是否与指定的 ColorX 结构相等。</returns>
        public bool Equals(ColorX color)
        {
            return (_CurrentColorSpace.Equals(color._CurrentColorSpace) && _Opacity.Equals(color._Opacity) && (_Channel1.Equals(color._Channel1) && _Channel2.Equals(color._Channel2) && _Channel3.Equals(color._Channel3) && _Channel4.Equals(color._Channel4)) && Hashtable.Equals(_CachedChannels, color._CachedChannels));
        }

        //

        /// <summary>
        /// 返回将此 ColorX 结构转换为 Color 结构的新实例。
        /// </summary>
        /// <returns>Color 结构，表示将此 ColorX 结构转换为 Color 结构得到的结果。</returns>
        public Color ToColor()
        {
            if (_CurrentColorSpace == _ColorSpace.None)
            {
                return Color.Empty;
            }
            else
            {
                double[] rgb = _GetChannels(_ColorSpace.RGB);

                return Color.FromArgb((int)Math.Round(Alpha), (int)Math.Round(rgb[0]), (int)Math.Round(rgb[1]), (int)Math.Round(rgb[2]));
            }
        }

        /// <summary>
        /// 返回将此 ColorX 结构转换为 Color 结构的 32 位 ARGB 值。
        /// </summary>
        /// <returns>32 位整数，表示将此 ColorX 结构转换为 Color 结构的 32 位 ARGB 值。</returns>
        public int ToARGB()
        {
            double[] rgb = _GetChannels(_ColorSpace.RGB);

            return (int)(((uint)Math.Round(Alpha) << _32BitArgbAlphaShift) | ((uint)Math.Round(rgb[0]) << _32BitArgbRedShift) | ((uint)Math.Round(rgb[1]) << _32BitArgbGreenShift) | ((uint)Math.Round(rgb[2]) << _32BitArgbBlueShift));
        }

        //

        /// <summary>
        /// 返回将此 ColorX 结构的不透明度更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="opacity">颜色的不透明度。</param>
        /// <returns>ColorX 结构，表示将此 ColorX 结构的不透明度更改为指定值得到的结果。</returns>
        public ColorX AtOpacity(double opacity)
        {
            ColorX color = this;

            color.Opacity = opacity;

            return color;
        }

        /// <summary>
        /// 返回将此 ColorX 结构的 Alpha 通道（A）的值更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="alpha">颜色的 Alpha 通道（A）的值。</param>
        /// <returns>ColorX 结构，表示将此 ColorX 结构的 Alpha 通道（A）的值更改为指定值得到的结果。</returns>
        public ColorX AtAlpha(double alpha)
        {
            ColorX color = this;

            color.Alpha = alpha;

            return color;
        }

        //

        /// <summary>
        /// 返回将此 ColorX 结构在 RGB 色彩空间的红色通道（R）的值更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="red">颜色在 RGB 色彩空间的红色通道（R）的值。</param>
        /// <returns>ColorX 结构，表示将此 ColorX 结构在 RGB 色彩空间的红色通道（R）的值更改为指定值得到的结果。</returns>
        public ColorX AtRed(double red)
        {
            ColorX color = this;

            color.Red = red;

            return color;
        }

        /// <summary>
        /// 返回将此 ColorX 结构在 RGB 色彩空间的绿色通道（G）的值更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="green">颜色在 RGB 色彩空间的绿色通道（G）的值。</param>
        /// <returns>ColorX 结构，表示将此 ColorX 结构在 RGB 色彩空间的绿色通道（G）的值更改为指定值得到的结果。</returns>
        public ColorX AtGreen(double green)
        {
            ColorX color = this;

            color.Green = green;

            return color;
        }

        /// <summary>
        /// 返回将此 ColorX 结构在 RGB 色彩空间的蓝色通道（B）的值更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="blue">颜色在 RGB 色彩空间的蓝色通道（B）的值。</param>
        /// <returns>ColorX 结构，表示将此 ColorX 结构在 RGB 色彩空间的蓝色通道（B）的值更改为指定值得到的结果。</returns>
        public ColorX AtBlue(double blue)
        {
            ColorX color = this;

            color.Blue = blue;

            return color;
        }

        //

        /// <summary>
        /// 返回将此 ColorX 结构在 HSV 色彩空间的色相（H）更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="hue">颜色在 HSV 色彩空间的色相（H）。</param>
        /// <returns>ColorX 结构，表示将此 ColorX 结构在 HSV 色彩空间的色相（H）更改为指定值得到的结果。</returns>
        public ColorX AtHue_HSV(double hue)
        {
            ColorX color = this;

            color.Hue_HSV = hue;

            return color;
        }

        /// <summary>
        /// 返回将此 ColorX 结构在 HSV 色彩空间的饱和度（S）更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="saturation">颜色在 HSV 色彩空间的饱和度（S）。</param>
        /// <returns>ColorX 结构，表示将此 ColorX 结构在 HSV 色彩空间的饱和度（S）更改为指定值得到的结果。</returns>
        public ColorX AtSaturation_HSV(double saturation)
        {
            ColorX color = this;

            color.Saturation_HSV = saturation;

            return color;
        }

        /// <summary>
        /// 返回将此 ColorX 结构在 HSV 色彩空间的亮度（V）更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="brightness">颜色在 HSV 色彩空间的亮度（V）。</param>
        /// <returns>ColorX 结构，表示将此 ColorX 结构在 HSV 色彩空间的亮度（V）更改为指定值得到的结果。</returns>
        public ColorX AtBrightness(double brightness)
        {
            ColorX color = this;

            color.Brightness = brightness;

            return color;
        }

        //

        /// <summary>
        /// 返回将此 ColorX 结构在 HSL 色彩空间的色相（H）更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="hue">颜色在 HSL 色彩空间的色相（H）。</param>
        /// <returns>ColorX 结构，表示将此 ColorX 结构在 HSL 色彩空间的色相（H）更改为指定值得到的结果。</returns>
        public ColorX AtHue_HSL(double hue)
        {
            ColorX color = this;

            color.Hue_HSL = hue;

            return color;
        }

        /// <summary>
        /// 返回将此 ColorX 结构在 HSL 色彩空间的饱和度（S）更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="saturation">颜色在 HSL 色彩空间的饱和度（S）。</param>
        /// <returns>ColorX 结构，表示将此 ColorX 结构在 HSL 色彩空间的饱和度（S）更改为指定值得到的结果。</returns>
        public ColorX AtSaturation_HSL(double saturation)
        {
            ColorX color = this;

            color.Saturation_HSL = saturation;

            return color;
        }

        /// <summary>
        /// 返回将此 ColorX 结构在 HSL 色彩空间的明度（L）更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="lightness">颜色在 HSL 色彩空间的明度（L）。</param>
        /// <returns>ColorX 结构，表示将此 ColorX 结构在 HSL 色彩空间的明度（L）更改为指定值得到的结果。</returns>
        public ColorX AtLightness_HSL(double lightness)
        {
            ColorX color = this;

            color.Lightness_HSL = lightness;

            return color;
        }

        //

        /// <summary>
        /// 返回将此 ColorX 结构在 CMYK 色彩空间的青色通道（C）的值更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="cyan">颜色在 CMYK 色彩空间的青色通道（C）的值。</param>
        /// <returns>ColorX 结构，表示将此 ColorX 结构在 CMYK 色彩空间的青色通道（C）的值更改为指定值得到的结果。</returns>
        public ColorX AtCyan(double cyan)
        {
            ColorX color = this;

            color.Cyan = cyan;

            return color;
        }

        /// <summary>
        /// 返回将此 ColorX 结构在 CMYK 色彩空间的洋红色通道（M）的值更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="magenta">颜色在 CMYK 色彩空间的洋红色通道（M）的值。</param>
        /// <returns>ColorX 结构，表示将此 ColorX 结构在 CMYK 色彩空间的洋红色通道（M）的值更改为指定值得到的结果。</returns>
        public ColorX AtMagenta(double magenta)
        {
            ColorX color = this;

            color.Magenta = magenta;

            return color;
        }

        /// <summary>
        /// 返回将此 ColorX 结构在 CMYK 色彩空间的黄色通道（Y）的值更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="yellow">颜色在 CMYK 色彩空间的黄色通道（Y）的值。</param>
        /// <returns>ColorX 结构，表示将此 ColorX 结构在 CMYK 色彩空间的黄色通道（Y）的值更改为指定值得到的结果。</returns>
        public ColorX AtYellow(double yellow)
        {
            ColorX color = this;

            color.Yellow = yellow;

            return color;
        }

        /// <summary>
        /// 返回将此 ColorX 结构在 CMYK 色彩空间的黑色通道（K）的值更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="black">颜色在 CMYK 色彩空间的黑色通道（K）的值。</param>
        /// <returns>ColorX 结构，表示将此 ColorX 结构在 CMYK 色彩空间的黑色通道（K）的值更改为指定值得到的结果。</returns>
        public ColorX AtBlack(double black)
        {
            ColorX color = this;

            color.Black = black;

            return color;
        }

        //

        /// <summary>
        /// 返回将此 ColorX 结构在 LAB 色彩空间的明度（L）更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="lightness">颜色在 LAB 色彩空间的明度（L）。</param>
        /// <returns>ColorX 结构，表示将此 ColorX 结构在 LAB 色彩空间的明度（L）更改为指定值得到的结果。</returns>
        public ColorX AtLightness_LAB(double lightness)
        {
            ColorX color = this;

            color.Lightness_LAB = lightness;

            return color;
        }

        /// <summary>
        /// 返回将此 ColorX 结构在 LAB 色彩空间的绿色-红色通道（A）的值更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="greenRed">颜色在 LAB 色彩空间的绿色-红色通道（A）的值。</param>
        /// <returns>ColorX 结构，表示将此 ColorX 结构在 LAB 色彩空间的绿色-红色通道（A）的值更改为指定值得到的结果。</returns>
        public ColorX AtGreenRed(double greenRed)
        {
            ColorX color = this;

            color.GreenRed = greenRed;

            return color;
        }

        /// <summary>
        /// 返回将此 ColorX 结构在 LAB 色彩空间的蓝色-黄色通道（B）的值更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="blueYellow">颜色在 LAB 色彩空间的蓝色-黄色通道（B）的值。</param>
        /// <returns>ColorX 结构，表示将此 ColorX 结构在 LAB 色彩空间的蓝色-黄色通道（B）的值更改为指定值得到的结果。</returns>
        public ColorX AtBlueYellow(double blueYellow)
        {
            ColorX color = this;

            color.BlueYellow = blueYellow;

            return color;
        }

        //

        /// <summary>
        /// 返回将此 ColorX 结构在 YUV 色彩空间的亮度（Y）更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="luminance">颜色在 YUV 色彩空间的亮度（Y）。</param>
        /// <returns>ColorX 结构，表示将此 ColorX 结构在 YUV 色彩空间的亮度（Y）更改为指定值得到的结果。</returns>
        public ColorX AtLuminance(double luminance)
        {
            ColorX color = this;

            color.Luminance = luminance;

            return color;
        }

        /// <summary>
        /// 返回将此 ColorX 结构在 YUV 色彩空间的蓝色色度（U）更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="chrominanceBlue">颜色在 YUV 色彩空间的蓝色色度（U）。</param>
        /// <returns>ColorX 结构，表示将此 ColorX 结构在 YUV 色彩空间的蓝色色度（U）更改为指定值得到的结果。</returns>
        public ColorX AtChrominanceBlue(double chrominanceBlue)
        {
            ColorX color = this;

            color.ChrominanceBlue = chrominanceBlue;

            return color;
        }

        /// <summary>
        /// 返回将此 ColorX 结构在 YUV 色彩空间的红色色度（V）更改为指定值的 ColorX 结构的新实例。
        /// </summary>
        /// <param name="chrominanceRed">颜色在 YUV 色彩空间的红色色度（V）。</param>
        /// <returns>ColorX 结构，表示将此 ColorX 结构在 YUV 色彩空间的红色色度（V）更改为指定值得到的结果。</returns>
        public ColorX AtChrominanceRed(double chrominanceRed)
        {
            ColorX color = this;

            color.ChrominanceRed = chrominanceRed;

            return color;
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 判断两个 ColorX 结构是否相等。
        /// </summary>
        /// <param name="left">用于比较的第一个 ColorX 结构。</param>
        /// <param name="right">用于比较的第二个 ColorX 结构。</param>
        /// <returns>布尔值，表示两个 ColorX 结构是否相等。</returns>
        public static bool Equals(ColorX left, ColorX right)
        {
            if ((object)left == null && (object)right == null)
            {
                return true;
            }
            else if ((object)left == null || (object)right == null)
            {
                return false;
            }
            else if (object.ReferenceEquals(left, right))
            {
                return true;
            }
            else
            {
                return left.Equals(right);
            }
        }

        //

        /// <summary>
        /// 返回将 Color 结构转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="alpha">Alpha 通道（A）的值。</param>
        /// <param name="color">Color 结构。</param>
        /// <returns>ColorX 结构，表示将 Color 结构转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromColor(int alpha, Color color)
        {
            return new ColorX(Color.FromArgb(alpha, color));
        }

        /// <summary>
        /// 返回将 Color 结构转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="color">Color 结构。</param>
        /// <returns>ColorX 结构，表示将 Color 结构转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromColor(Color color)
        {
            return new ColorX(color);
        }

        //

        /// <summary>
        /// 返回将颜色在 RGB 色彩空间的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="alpha">颜色的 Alpha 通道（A）的值。</param>
        /// <param name="red">颜色在 RGB 色彩空间的红色通道（R）的值。</param>
        /// <param name="green">颜色在 RGB 色彩空间的绿色通道（G）的值。</param>
        /// <param name="blue">颜色在 RGB 色彩空间的蓝色通道（B）的值。</param>
        /// <returns>ColorX 结构，表示将颜色在 RGB 色彩空间的各分量转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromRGB(double alpha, double red, double green, double blue)
        {
            ColorX color = new ColorX();

            color._SetChannels(_ColorSpace.RGB, red, green, blue);
            color.Alpha = alpha;

            return color;
        }

        /// <summary>
        /// 返回将颜色在 RGB 色彩空间的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="red">颜色在 RGB 色彩空间的红色通道（R）的值。</param>
        /// <param name="green">颜色在 RGB 色彩空间的绿色通道（G）的值。</param>
        /// <param name="blue">颜色在 RGB 色彩空间的蓝色通道（B）的值。</param>
        /// <returns>ColorX 结构，表示将颜色在 RGB 色彩空间的各分量转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromRGB(double red, double green, double blue)
        {
            ColorX color = new ColorX();

            color._SetChannels(_ColorSpace.RGB, red, green, blue);
            color.Alpha = _DefaultAlpha;

            return color;
        }

        /// <summary>
        /// 返回将颜色在 RGB 色彩空间的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="alpha">颜色的 Alpha 通道（A）的值。</param>
        /// <param name="rgb">表示颜色在 RGB 色彩空间的各分量的 PointD3D 结构。</param>
        /// <returns>ColorX 结构，表示将颜色在 RGB 色彩空间的各分量转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromRGB(double alpha, PointD3D rgb)
        {
            ColorX color = new ColorX();

            color._SetChannels(_ColorSpace.RGB, rgb.ToArray());
            color.Alpha = alpha;

            return color;
        }

        /// <summary>
        /// 返回将颜色在 RGB 色彩空间的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="rgb">表示颜色在 RGB 色彩空间的各分量的 PointD3D 结构。</param>
        /// <returns>ColorX 结构，表示将颜色在 RGB 色彩空间的各分量转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromRGB(PointD3D rgb)
        {
            ColorX color = new ColorX();

            color._SetChannels(_ColorSpace.RGB, rgb.ToArray());
            color.Alpha = _DefaultAlpha;

            return color;
        }

        /// <summary>
        /// 返回将颜色在 RGB 色彩空间的 32 位 ARGB 值转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="argb">颜色在 RGB 色彩空间的 32 位 ARGB 值。</param>
        /// <returns>ColorX 结构，表示将颜色在 RGB 色彩空间的 32 位 ARGB 值转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromRGB(int argb)
        {
            return new ColorX(argb);
        }

        //

        /// <summary>
        /// 返回将颜色在 HSV 色彩空间的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="hue">颜色在 HSV 色彩空间的色相（H）。</param>
        /// <param name="saturation">颜色在 HSV 色彩空间的饱和度（S）。</param>
        /// <param name="brightness">颜色在 HSV 色彩空间的亮度（V）。</param>
        /// <param name="opacity">颜色的不透明度。</param>
        /// <returns>ColorX 结构，表示将颜色在 HSV 色彩空间的各分量转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromHSV(double hue, double saturation, double brightness, double opacity)
        {
            ColorX color = new ColorX();

            color._SetChannels(_ColorSpace.HSV, hue, saturation, brightness);
            color.Opacity = opacity;

            return color;
        }

        /// <summary>
        /// 返回将颜色在 HSV 色彩空间的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="hue">颜色在 HSV 色彩空间的色相（H）。</param>
        /// <param name="saturation">颜色在 HSV 色彩空间的饱和度（S）。</param>
        /// <param name="brightness">颜色在 HSV 色彩空间的亮度（V）。</param>
        /// <returns>ColorX 结构，表示将颜色在 HSV 色彩空间的各分量转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromHSV(double hue, double saturation, double brightness)
        {
            ColorX color = new ColorX();

            color._SetChannels(_ColorSpace.HSV, hue, saturation, brightness);
            color.Opacity = _DefaultOpacity;

            return color;
        }

        /// <summary>
        /// 返回将颜色在 HSV 色彩空间的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="hsv">表示颜色在 HSV 色彩空间的各分量的 PointD3D 结构。</param>
        /// <param name="opacity">颜色的不透明度。</param>
        /// <returns>ColorX 结构，表示将颜色在 HSV 色彩空间的各分量转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromHSV(PointD3D hsv, double opacity)
        {
            ColorX color = new ColorX();

            color._SetChannels(_ColorSpace.HSV, hsv.ToArray());
            color.Opacity = opacity;

            return color;
        }

        /// <summary>
        /// 返回将颜色在 HSV 色彩空间的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="hsv">表示颜色在 HSV 色彩空间的各分量的 PointD3D 结构。</param>
        /// <returns>ColorX 结构，表示将颜色在 HSV 色彩空间的各分量转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromHSV(PointD3D hsv)
        {
            ColorX color = new ColorX();

            color._SetChannels(_ColorSpace.HSV, hsv.ToArray());
            color.Opacity = _DefaultOpacity;

            return color;
        }

        //

        /// <summary>
        /// 返回将颜色在 HSL 色彩空间的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="hue">颜色在 HSL 色彩空间的色相（H）。</param>
        /// <param name="saturation">颜色在 HSL 色彩空间的饱和度（S）。</param>
        /// <param name="lightness">颜色在 HSL 色彩空间的明度（L）。</param>
        /// <param name="opacity">颜色的不透明度。</param>
        /// <returns>ColorX 结构，表示将颜色在 HSL 色彩空间的各分量转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromHSL(double hue, double saturation, double lightness, double opacity)
        {
            ColorX color = new ColorX();

            color._SetChannels(_ColorSpace.HSL, hue, saturation, lightness);
            color.Opacity = opacity;

            return color;
        }

        /// <summary>
        /// 返回将颜色在 HSL 色彩空间的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="hue">颜色在 HSL 色彩空间的色相（H）。</param>
        /// <param name="saturation">颜色在 HSL 色彩空间的饱和度（S）。</param>
        /// <param name="lightness">颜色在 HSL 色彩空间的明度（L）。</param>
        /// <returns>ColorX 结构，表示将颜色在 HSL 色彩空间的各分量转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromHSL(double hue, double saturation, double lightness)
        {
            ColorX color = new ColorX();

            color._SetChannels(_ColorSpace.HSL, hue, saturation, lightness);
            color.Opacity = _DefaultOpacity;

            return color;
        }

        /// <summary>
        /// 返回将颜色在 HSL 色彩空间的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="hsl">表示颜色在 HSL 色彩空间的各分量的 PointD3D 结构。</param>
        /// <param name="opacity">颜色的不透明度。</param>
        /// <returns>ColorX 结构，表示将颜色在 HSL 色彩空间的各分量转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromHSL(PointD3D hsl, double opacity)
        {
            ColorX color = new ColorX();

            color._SetChannels(_ColorSpace.HSL, hsl.ToArray());
            color.Opacity = opacity;

            return color;
        }

        /// <summary>
        /// 返回将颜色在 HSL 色彩空间的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="hsl">表示颜色在 HSL 色彩空间的各分量的 PointD3D 结构。</param>
        /// <returns>ColorX 结构，表示将颜色在 HSL 色彩空间的各分量转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromHSL(PointD3D hsl)
        {
            ColorX color = new ColorX();

            color._SetChannels(_ColorSpace.HSL, hsl.ToArray());
            color.Opacity = _DefaultOpacity;

            return color;
        }

        //

        /// <summary>
        /// 返回将颜色在 CMYK 色彩空间的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="cyan">颜色在 CMYK 色彩空间的青色通道（C）的值。</param>
        /// <param name="magenta">颜色在 CMYK 色彩空间的洋红色通道（M）的值。</param>
        /// <param name="yellow">颜色在 CMYK 色彩空间的黄色通道（Y）的值。</param>
        /// <param name="black">颜色在 CMYK 色彩空间的黑色通道（K）的值。</param>
        /// <param name="opacity">颜色的不透明度。</param>
        /// <returns>ColorX 结构，表示将颜色在 CMYK 色彩空间的各分量转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromCMYK(double cyan, double magenta, double yellow, double black, double opacity)
        {
            ColorX color = new ColorX();

            color._SetChannels(_ColorSpace.CMYK, cyan, magenta, yellow, black);
            color.Opacity = opacity;

            return color;
        }

        /// <summary>
        /// 返回将颜色在 CMYK 色彩空间的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="cyan">颜色在 CMYK 色彩空间的青色通道（C）的值。</param>
        /// <param name="magenta">颜色在 CMYK 色彩空间的洋红色通道（M）的值。</param>
        /// <param name="yellow">颜色在 CMYK 色彩空间的黄色通道（Y）的值。</param>
        /// <param name="black">颜色在 CMYK 色彩空间的黑色通道（K）的值。</param>
        /// <returns>ColorX 结构，表示将颜色在 CMYK 色彩空间的各分量转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromCMYK(double cyan, double magenta, double yellow, double black)
        {
            ColorX color = new ColorX();

            color._SetChannels(_ColorSpace.CMYK, cyan, magenta, yellow, black);
            color.Opacity = _DefaultOpacity;

            return color;
        }

        /// <summary>
        /// 返回将颜色在 CMYK 色彩空间的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="cmyk">表示颜色在 CMYK 色彩空间的各分量的 PointD4D 结构。</param>
        /// <param name="opacity">颜色的不透明度。</param>
        /// <returns>ColorX 结构，表示将颜色在 CMYK 色彩空间的各分量转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromCMYK(PointD4D cmyk, double opacity)
        {
            ColorX color = new ColorX();

            color._SetChannels(_ColorSpace.CMYK, cmyk.ToArray());
            color.Opacity = opacity;

            return color;
        }

        /// <summary>
        /// 返回将颜色在 CMYK 色彩空间的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="cmyk">表示颜色在 CMYK 色彩空间的各分量的 PointD4D 结构。</param>
        /// <returns>ColorX 结构，表示将颜色在 CMYK 色彩空间的各分量转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromCMYK(PointD4D cmyk)
        {
            ColorX color = new ColorX();

            color._SetChannels(_ColorSpace.CMYK, cmyk.ToArray());
            color.Opacity = _DefaultOpacity;

            return color;
        }

        //

        /// <summary>
        /// 返回将颜色在 LAB 色彩空间的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="lightness">颜色在 LAB 色彩空间的明度（L）。</param>
        /// <param name="greenRed">颜色在 LAB 色彩空间的绿色-红色通道（A）的值。</param>
        /// <param name="blueYellow">颜色在 LAB 色彩空间的蓝色-黄色通道（B）的值。</param>
        /// <param name="opacity">颜色的不透明度。</param>
        /// <returns>ColorX 结构，表示将颜色在 LAB 色彩空间的各分量转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromLAB(double lightness, double greenRed, double blueYellow, double opacity)
        {
            ColorX color = new ColorX();

            color._SetChannels(_ColorSpace.LAB, lightness, greenRed, blueYellow);
            color.Opacity = opacity;

            return color;
        }

        /// <summary>
        /// 返回将颜色在 LAB 色彩空间的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="lightness">颜色在 LAB 色彩空间的明度（L）。</param>
        /// <param name="greenRed">颜色在 LAB 色彩空间的绿色-红色通道（A）的值。</param>
        /// <param name="blueYellow">颜色在 LAB 色彩空间的蓝色-黄色通道（B）的值。</param>
        /// <returns>ColorX 结构，表示将颜色在 LAB 色彩空间的各分量转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromLAB(double lightness, double greenRed, double blueYellow)
        {
            ColorX color = new ColorX();

            color._SetChannels(_ColorSpace.LAB, lightness, greenRed, blueYellow);
            color.Opacity = _DefaultOpacity;

            return color;
        }

        /// <summary>
        /// 返回将颜色在 LAB 色彩空间的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="lab">表示颜色在 LAB 色彩空间的各分量的 PointD3D 结构。</param>
        /// <param name="opacity">颜色的不透明度。</param>
        /// <returns>ColorX 结构，表示将颜色在 LAB 色彩空间的各分量转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromLAB(PointD3D lab, double opacity)
        {
            ColorX color = new ColorX();

            color._SetChannels(_ColorSpace.LAB, lab.ToArray());
            color.Opacity = opacity;

            return color;
        }

        /// <summary>
        /// 返回将颜色在 LAB 色彩空间的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="lab">表示颜色在 LAB 色彩空间的各分量的 PointD3D 结构。</param>
        /// <returns>ColorX 结构，表示将颜色在 LAB 色彩空间的各分量转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromLAB(PointD3D lab)
        {
            ColorX color = new ColorX();

            color._SetChannels(_ColorSpace.LAB, lab.ToArray());
            color.Opacity = _DefaultOpacity;

            return color;
        }

        //

        /// <summary>
        /// 返回将颜色在 YUV 色彩空间的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="luminance">颜色在 YUV 色彩空间的亮度（Y）。</param>
        /// <param name="chrominanceBlue">颜色在 YUV 色彩空间的蓝色色度（U）。</param>
        /// <param name="chrominanceRed">颜色在 YUV 色彩空间的红色色度（V）。</param>
        /// <param name="opacity">颜色的不透明度。</param>
        /// <returns>ColorX 结构，表示将颜色在 YUV 色彩空间的各分量转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromYUV(double luminance, double chrominanceBlue, double chrominanceRed, double opacity)
        {
            ColorX color = new ColorX();

            color._SetChannels(_ColorSpace.YUV, luminance, chrominanceBlue, chrominanceRed);
            color.Opacity = opacity;

            return color;
        }

        /// <summary>
        /// 返回将颜色在 YUV 色彩空间的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="luminance">颜色在 YUV 色彩空间的亮度（Y）。</param>
        /// <param name="chrominanceBlue">颜色在 YUV 色彩空间的蓝色色度（U）。</param>
        /// <param name="chrominanceRed">颜色在 YUV 色彩空间的红色色度（V）。</param>
        /// <returns>ColorX 结构，表示将颜色在 YUV 色彩空间的各分量转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromYUV(double luminance, double chrominanceBlue, double chrominanceRed)
        {
            ColorX color = new ColorX();

            color._SetChannels(_ColorSpace.YUV, luminance, chrominanceBlue, chrominanceRed);
            color.Opacity = _DefaultOpacity;

            return color;
        }

        /// <summary>
        /// 返回将颜色在 YUV 色彩空间的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="yuv">表示颜色在 YUV 色彩空间的各分量的 PointD3D 结构。</param>
        /// <param name="opacity">颜色的不透明度。</param>
        /// <returns>ColorX 结构，表示将颜色在 YUV 色彩空间的各分量转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromYUV(PointD3D yuv, double opacity)
        {
            ColorX color = new ColorX();

            color._SetChannels(_ColorSpace.YUV, yuv.ToArray());
            color.Opacity = opacity;

            return color;
        }

        /// <summary>
        /// 返回将颜色在 YUV 色彩空间的各分量转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="yuv">表示颜色在 YUV 色彩空间的各分量的 PointD3D 结构。</param>
        /// <returns>ColorX 结构，表示将颜色在 YUV 色彩空间的各分量转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromYUV(PointD3D yuv)
        {
            ColorX color = new ColorX();

            color._SetChannels(_ColorSpace.YUV, yuv.ToArray());
            color.Opacity = _DefaultOpacity;

            return color;
        }

        //

        /// <summary>
        /// 返回将颜色的 16 进制 ARGB 码或 RGB 码转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="hexCode">颜色的 16 进制 ARGB 码或 RGB 码。</param>
        /// <returns>ColorX 结构，表示将颜色的 16 进制 ARGB 码或 RGB 码转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromHexCode(string hexCode)
        {
            return new ColorX(hexCode);
        }

        //

        /// <summary>
        /// 返回将颜色的名称转换为 ColorX 结构的新实例。
        /// </summary>
        /// <param name="name">颜色的名称。</param>
        /// <returns>ColorX 结构，表示将颜色的名称转换为 ColorX 结构得到的结果。</returns>
        public static ColorX FromName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException();
            }

            int? argb = _GetArgbByName(name);

            if (argb != null)
            {
                return new ColorX(argb.Value);
            }
            else
            {
                return new ColorX(name);
            }
        }

        //

        /// <summary>
        /// 返回一个不透明度为 100%，其他分量为随机数的 ColorX 结构的新实例。
        /// </summary>
        /// <returns>ColorX 结构，表示不透明度为 100%，其他分量为随机数的 ColorX 结构。</returns>
        public static ColorX RandomColor()
        {
            return FromRGB(Statistics.RandomDouble(_MinRed, _MaxRed), Statistics.RandomDouble(_MinGreen, _MaxGreen), Statistics.RandomDouble(_MinBlue, _MaxBlue));
        }

        #endregion

        #region 运算符

        /// <summary>
        /// 判断两个 ColorX 结构是否表示相同的颜色。
        /// </summary>
        /// <param name="left">运算符左侧比较的 ColorX 结构。</param>
        /// <param name="right">运算符右侧比较的 ColorX 结构。</param>
        /// <returns>布尔值，表示两个 ColorX 结构是否表示相同的颜色。</returns>
        public static bool operator ==(ColorX left, ColorX right)
        {
            if (left._CurrentColorSpace == _ColorSpace.None || right._CurrentColorSpace == _ColorSpace.None)
            {
                return false;
            }
            else
            {
                return (left._CurrentColorSpace == right._CurrentColorSpace && left._Opacity == right._Opacity && (left._Channel1 == right._Channel1 && left._Channel2 == right._Channel2 && left._Channel3 == right._Channel3 && left._Channel4 == right._Channel4));
            }
        }

        /// <summary>
        /// 判断两个 ColorX 结构是否表示不同的颜色。
        /// </summary>
        /// <param name="left">运算符左侧比较的 ColorX 结构。</param>
        /// <param name="right">运算符右侧比较的 ColorX 结构。</param>
        /// <returns>布尔值，表示两个 ColorX 结构是否表示不同的颜色。</returns>
        public static bool operator !=(ColorX left, ColorX right)
        {
            if (left._CurrentColorSpace == _ColorSpace.None || right._CurrentColorSpace == _ColorSpace.None)
            {
                return true;
            }
            else
            {
                return (left._CurrentColorSpace != right._CurrentColorSpace || left._Opacity != right._Opacity || (left._Channel1 != right._Channel1 || left._Channel2 != right._Channel2 || left._Channel3 != right._Channel3 || left._Channel4 != right._Channel4));
            }
        }

        //

        /// <summary>
        /// 将指定的 ColorX 结构显式转换为 Color 结构。
        /// </summary>
        /// <param name="color">用于转换的 ColorX 结构。</param>
        /// <returns>Color 结构，表示显式转换的结果。</returns>
        public static explicit operator Color(ColorX color)
        {
            return color.ToColor();
        }

        /// <summary>
        /// 将指定的 Color 结构隐式转换为 ColorX 结构。
        /// </summary>
        /// <param name="color">用于转换的 Color 结构。</param>
        /// <returns>ColorX 结构，表示隐式转换的结果。</returns>
        public static implicit operator ColorX(Color color)
        {
            return new ColorX(color);
        }

        #endregion
    }
}