/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2019 chibayuki@foxmail.com

Com.ColorX
Version 19.6.17.0000

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
using System.Text.RegularExpressions;

namespace Com
{
    internal static class ColorConverter
    {
        private static Hashtable _ColorNameTable = null;
        private static Hashtable _ColorTable = null;

        private static void _EnsureColorNameTable()
        {
            if (_ColorNameTable == null)
            {
                _ColorNameTable = new Hashtable();

                _ColorNameTable.Add(Color.Transparent.ToArgb(), "Transparent");
                _ColorNameTable.Add(Color.AliceBlue.ToArgb(), "AliceBlue");
                _ColorNameTable.Add(Color.AntiqueWhite.ToArgb(), "AntiqueWhite");
                // Aqua 与 Cyan 异名同色，使用 Cyan
                // _ColorNameTable.Add(Color.Aqua.ToArgb(), "Aqua");
                _ColorNameTable.Add(Color.Aquamarine.ToArgb(), "Aquamarine");
                _ColorNameTable.Add(Color.Azure.ToArgb(), "Azure");
                _ColorNameTable.Add(Color.Beige.ToArgb(), "Beige");
                _ColorNameTable.Add(Color.Bisque.ToArgb(), "Bisque");
                _ColorNameTable.Add(Color.Black.ToArgb(), "Black");
                _ColorNameTable.Add(Color.BlanchedAlmond.ToArgb(), "BlanchedAlmond");
                _ColorNameTable.Add(Color.Blue.ToArgb(), "Blue");
                _ColorNameTable.Add(Color.BlueViolet.ToArgb(), "BlueViolet");
                _ColorNameTable.Add(Color.Brown.ToArgb(), "Brown");
                _ColorNameTable.Add(Color.BurlyWood.ToArgb(), "BurlyWood");
                _ColorNameTable.Add(Color.CadetBlue.ToArgb(), "CadetBlue");
                _ColorNameTable.Add(Color.Chartreuse.ToArgb(), "Chartreuse");
                _ColorNameTable.Add(Color.Chocolate.ToArgb(), "Chocolate");
                _ColorNameTable.Add(Color.Coral.ToArgb(), "Coral");
                _ColorNameTable.Add(Color.CornflowerBlue.ToArgb(), "CornflowerBlue");
                _ColorNameTable.Add(Color.Cornsilk.ToArgb(), "Cornsilk");
                _ColorNameTable.Add(Color.Crimson.ToArgb(), "Crimson");
                _ColorNameTable.Add(Color.Cyan.ToArgb(), "Cyan");
                _ColorNameTable.Add(Color.DarkBlue.ToArgb(), "DarkBlue");
                _ColorNameTable.Add(Color.DarkCyan.ToArgb(), "DarkCyan");
                _ColorNameTable.Add(Color.DarkGoldenrod.ToArgb(), "DarkGoldenrod");
                _ColorNameTable.Add(Color.DarkGray.ToArgb(), "DarkGray");
                _ColorNameTable.Add(Color.DarkGreen.ToArgb(), "DarkGreen");
                _ColorNameTable.Add(Color.DarkKhaki.ToArgb(), "DarkKhaki");
                _ColorNameTable.Add(Color.DarkMagenta.ToArgb(), "DarkMagenta");
                _ColorNameTable.Add(Color.DarkOliveGreen.ToArgb(), "DarkOliveGreen");
                _ColorNameTable.Add(Color.DarkOrange.ToArgb(), "DarkOrange");
                _ColorNameTable.Add(Color.DarkOrchid.ToArgb(), "DarkOrchid");
                _ColorNameTable.Add(Color.DarkRed.ToArgb(), "DarkRed");
                _ColorNameTable.Add(Color.DarkSalmon.ToArgb(), "DarkSalmon");
                _ColorNameTable.Add(Color.DarkSeaGreen.ToArgb(), "DarkSeaGreen");
                _ColorNameTable.Add(Color.DarkSlateBlue.ToArgb(), "DarkSlateBlue");
                _ColorNameTable.Add(Color.DarkSlateGray.ToArgb(), "DarkSlateGray");
                _ColorNameTable.Add(Color.DarkTurquoise.ToArgb(), "DarkTurquoise");
                _ColorNameTable.Add(Color.DarkViolet.ToArgb(), "DarkViolet");
                _ColorNameTable.Add(Color.DeepPink.ToArgb(), "DeepPink");
                _ColorNameTable.Add(Color.DeepSkyBlue.ToArgb(), "DeepSkyBlue");
                _ColorNameTable.Add(Color.DimGray.ToArgb(), "DimGray");
                _ColorNameTable.Add(Color.DodgerBlue.ToArgb(), "DodgerBlue");
                _ColorNameTable.Add(Color.Firebrick.ToArgb(), "Firebrick");
                _ColorNameTable.Add(Color.FloralWhite.ToArgb(), "FloralWhite");
                _ColorNameTable.Add(Color.ForestGreen.ToArgb(), "ForestGreen");
                // Fuchsia 与 Magenta 异名同色，使用 Magenta
                // _ColorNameTable.Add(Color.Fuchsia.ToArgb(), "Fuchsia");
                _ColorNameTable.Add(Color.Gainsboro.ToArgb(), "Gainsboro");
                _ColorNameTable.Add(Color.GhostWhite.ToArgb(), "GhostWhite");
                _ColorNameTable.Add(Color.Gold.ToArgb(), "Gold");
                _ColorNameTable.Add(Color.Goldenrod.ToArgb(), "Goldenrod");
                _ColorNameTable.Add(Color.Gray.ToArgb(), "Gray");
                _ColorNameTable.Add(Color.Green.ToArgb(), "Green");
                _ColorNameTable.Add(Color.GreenYellow.ToArgb(), "GreenYellow");
                _ColorNameTable.Add(Color.Honeydew.ToArgb(), "Honeydew");
                _ColorNameTable.Add(Color.HotPink.ToArgb(), "HotPink");
                _ColorNameTable.Add(Color.IndianRed.ToArgb(), "IndianRed");
                _ColorNameTable.Add(Color.Indigo.ToArgb(), "Indigo");
                _ColorNameTable.Add(Color.Ivory.ToArgb(), "Ivory");
                _ColorNameTable.Add(Color.Khaki.ToArgb(), "Khaki");
                _ColorNameTable.Add(Color.Lavender.ToArgb(), "Lavender");
                _ColorNameTable.Add(Color.LavenderBlush.ToArgb(), "LavenderBlush");
                _ColorNameTable.Add(Color.LawnGreen.ToArgb(), "LawnGreen");
                _ColorNameTable.Add(Color.LemonChiffon.ToArgb(), "LemonChiffon");
                _ColorNameTable.Add(Color.LightBlue.ToArgb(), "LightBlue");
                _ColorNameTable.Add(Color.LightCoral.ToArgb(), "LightCoral");
                _ColorNameTable.Add(Color.LightCyan.ToArgb(), "LightCyan");
                _ColorNameTable.Add(Color.LightGoldenrodYellow.ToArgb(), "LightGoldenrodYellow");
                _ColorNameTable.Add(Color.LightGray.ToArgb(), "LightGray");
                _ColorNameTable.Add(Color.LightGreen.ToArgb(), "LightGreen");
                _ColorNameTable.Add(Color.LightPink.ToArgb(), "LightPink");
                _ColorNameTable.Add(Color.LightSalmon.ToArgb(), "LightSalmon");
                _ColorNameTable.Add(Color.LightSeaGreen.ToArgb(), "LightSeaGreen");
                _ColorNameTable.Add(Color.LightSkyBlue.ToArgb(), "LightSkyBlue");
                _ColorNameTable.Add(Color.LightSlateGray.ToArgb(), "LightSlateGray");
                _ColorNameTable.Add(Color.LightSteelBlue.ToArgb(), "LightSteelBlue");
                _ColorNameTable.Add(Color.LightYellow.ToArgb(), "LightYellow");
                _ColorNameTable.Add(Color.Lime.ToArgb(), "Lime");
                _ColorNameTable.Add(Color.LimeGreen.ToArgb(), "LimeGreen");
                _ColorNameTable.Add(Color.Linen.ToArgb(), "Linen");
                _ColorNameTable.Add(Color.Magenta.ToArgb(), "Magenta");
                _ColorNameTable.Add(Color.Maroon.ToArgb(), "Maroon");
                _ColorNameTable.Add(Color.MediumAquamarine.ToArgb(), "MediumAquamarine");
                _ColorNameTable.Add(Color.MediumBlue.ToArgb(), "MediumBlue");
                _ColorNameTable.Add(Color.MediumOrchid.ToArgb(), "MediumOrchid");
                _ColorNameTable.Add(Color.MediumPurple.ToArgb(), "MediumPurple");
                _ColorNameTable.Add(Color.MediumSeaGreen.ToArgb(), "MediumSeaGreen");
                _ColorNameTable.Add(Color.MediumSlateBlue.ToArgb(), "MediumSlateBlue");
                _ColorNameTable.Add(Color.MediumSpringGreen.ToArgb(), "MediumSpringGreen");
                _ColorNameTable.Add(Color.MediumTurquoise.ToArgb(), "MediumTurquoise");
                _ColorNameTable.Add(Color.MediumVioletRed.ToArgb(), "MediumVioletRed");
                _ColorNameTable.Add(Color.MidnightBlue.ToArgb(), "MidnightBlue");
                _ColorNameTable.Add(Color.MintCream.ToArgb(), "MintCream");
                _ColorNameTable.Add(Color.MistyRose.ToArgb(), "MistyRose");
                _ColorNameTable.Add(Color.Moccasin.ToArgb(), "Moccasin");
                _ColorNameTable.Add(Color.NavajoWhite.ToArgb(), "NavajoWhite");
                _ColorNameTable.Add(Color.Navy.ToArgb(), "Navy");
                _ColorNameTable.Add(Color.OldLace.ToArgb(), "OldLace");
                _ColorNameTable.Add(Color.Olive.ToArgb(), "Olive");
                _ColorNameTable.Add(Color.OliveDrab.ToArgb(), "OliveDrab");
                _ColorNameTable.Add(Color.Orange.ToArgb(), "Orange");
                _ColorNameTable.Add(Color.OrangeRed.ToArgb(), "OrangeRed");
                _ColorNameTable.Add(Color.Orchid.ToArgb(), "Orchid");
                _ColorNameTable.Add(Color.PaleGoldenrod.ToArgb(), "PaleGoldenrod");
                _ColorNameTable.Add(Color.PaleGreen.ToArgb(), "PaleGreen");
                _ColorNameTable.Add(Color.PaleTurquoise.ToArgb(), "PaleTurquoise");
                _ColorNameTable.Add(Color.PaleVioletRed.ToArgb(), "PaleVioletRed");
                _ColorNameTable.Add(Color.PapayaWhip.ToArgb(), "PapayaWhip");
                _ColorNameTable.Add(Color.PeachPuff.ToArgb(), "PeachPuff");
                _ColorNameTable.Add(Color.Peru.ToArgb(), "Peru");
                _ColorNameTable.Add(Color.Pink.ToArgb(), "Pink");
                _ColorNameTable.Add(Color.Plum.ToArgb(), "Plum");
                _ColorNameTable.Add(Color.PowderBlue.ToArgb(), "PowderBlue");
                _ColorNameTable.Add(Color.Purple.ToArgb(), "Purple");
                _ColorNameTable.Add(Color.Red.ToArgb(), "Red");
                _ColorNameTable.Add(Color.RosyBrown.ToArgb(), "RosyBrown");
                _ColorNameTable.Add(Color.RoyalBlue.ToArgb(), "RoyalBlue");
                _ColorNameTable.Add(Color.SaddleBrown.ToArgb(), "SaddleBrown");
                _ColorNameTable.Add(Color.Salmon.ToArgb(), "Salmon");
                _ColorNameTable.Add(Color.SandyBrown.ToArgb(), "SandyBrown");
                _ColorNameTable.Add(Color.SeaGreen.ToArgb(), "SeaGreen");
                _ColorNameTable.Add(Color.SeaShell.ToArgb(), "SeaShell");
                _ColorNameTable.Add(Color.Sienna.ToArgb(), "Sienna");
                _ColorNameTable.Add(Color.Silver.ToArgb(), "Silver");
                _ColorNameTable.Add(Color.SkyBlue.ToArgb(), "SkyBlue");
                _ColorNameTable.Add(Color.SlateBlue.ToArgb(), "SlateBlue");
                _ColorNameTable.Add(Color.SlateGray.ToArgb(), "SlateGray");
                _ColorNameTable.Add(Color.Snow.ToArgb(), "Snow");
                _ColorNameTable.Add(Color.SpringGreen.ToArgb(), "SpringGreen");
                _ColorNameTable.Add(Color.SteelBlue.ToArgb(), "SteelBlue");
                _ColorNameTable.Add(Color.Tan.ToArgb(), "Tan");
                _ColorNameTable.Add(Color.Teal.ToArgb(), "Teal");
                _ColorNameTable.Add(Color.Thistle.ToArgb(), "Thistle");
                _ColorNameTable.Add(Color.Tomato.ToArgb(), "Tomato");
                _ColorNameTable.Add(Color.Turquoise.ToArgb(), "Turquoise");
                _ColorNameTable.Add(Color.Violet.ToArgb(), "Violet");
                _ColorNameTable.Add(Color.Wheat.ToArgb(), "Wheat");
                _ColorNameTable.Add(Color.White.ToArgb(), "White");
                _ColorNameTable.Add(Color.WhiteSmoke.ToArgb(), "WhiteSmoke");
                _ColorNameTable.Add(Color.Yellow.ToArgb(), "Yellow");
                _ColorNameTable.Add(Color.YellowGreen.ToArgb(), "YellowGreen");
            }
        }

        private static void _EnsureColorTable()
        {
            if (_ColorTable == null)
            {
                _ColorTable = new Hashtable();

                _ColorTable.Add("transparent", Color.Transparent);
                _ColorTable.Add("aliceblue", Color.AliceBlue);
                _ColorTable.Add("antiquewhite", Color.AntiqueWhite);
                _ColorTable.Add("aqua", Color.Aqua);
                _ColorTable.Add("aquamarine", Color.Aquamarine);
                _ColorTable.Add("azure", Color.Azure);
                _ColorTable.Add("beige", Color.Beige);
                _ColorTable.Add("bisque", Color.Bisque);
                _ColorTable.Add("black", Color.Black);
                _ColorTable.Add("blanchedalmond", Color.BlanchedAlmond);
                _ColorTable.Add("blue", Color.Blue);
                _ColorTable.Add("blueviolet", Color.BlueViolet);
                _ColorTable.Add("brown", Color.Brown);
                _ColorTable.Add("burlywood", Color.BurlyWood);
                _ColorTable.Add("cadetblue", Color.CadetBlue);
                _ColorTable.Add("chartreuse", Color.Chartreuse);
                _ColorTable.Add("chocolate", Color.Chocolate);
                _ColorTable.Add("coral", Color.Coral);
                _ColorTable.Add("cornflowerblue", Color.CornflowerBlue);
                _ColorTable.Add("cornsilk", Color.Cornsilk);
                _ColorTable.Add("crimson", Color.Crimson);
                _ColorTable.Add("cyan", Color.Cyan);
                _ColorTable.Add("darkblue", Color.DarkBlue);
                _ColorTable.Add("darkcyan", Color.DarkCyan);
                _ColorTable.Add("darkgoldenrod", Color.DarkGoldenrod);
                _ColorTable.Add("darkgray", Color.DarkGray);
                _ColorTable.Add("darkgreen", Color.DarkGreen);
                _ColorTable.Add("darkkhaki", Color.DarkKhaki);
                _ColorTable.Add("darkmagenta", Color.DarkMagenta);
                _ColorTable.Add("darkolivegreen", Color.DarkOliveGreen);
                _ColorTable.Add("darkorange", Color.DarkOrange);
                _ColorTable.Add("darkorchid", Color.DarkOrchid);
                _ColorTable.Add("darkred", Color.DarkRed);
                _ColorTable.Add("darksalmon", Color.DarkSalmon);
                _ColorTable.Add("darkseagreen", Color.DarkSeaGreen);
                _ColorTable.Add("darkslateblue", Color.DarkSlateBlue);
                _ColorTable.Add("darkslategray", Color.DarkSlateGray);
                _ColorTable.Add("darkturquoise", Color.DarkTurquoise);
                _ColorTable.Add("darkviolet", Color.DarkViolet);
                _ColorTable.Add("deeppink", Color.DeepPink);
                _ColorTable.Add("deepskyblue", Color.DeepSkyBlue);
                _ColorTable.Add("dimgray", Color.DimGray);
                _ColorTable.Add("dodgerblue", Color.DodgerBlue);
                _ColorTable.Add("firebrick", Color.Firebrick);
                _ColorTable.Add("floralwhite", Color.FloralWhite);
                _ColorTable.Add("forestgreen", Color.ForestGreen);
                _ColorTable.Add("fuchsia", Color.Fuchsia);
                _ColorTable.Add("gainsboro", Color.Gainsboro);
                _ColorTable.Add("ghostwhite", Color.GhostWhite);
                _ColorTable.Add("gold", Color.Gold);
                _ColorTable.Add("goldenrod", Color.Goldenrod);
                _ColorTable.Add("gray", Color.Gray);
                _ColorTable.Add("green", Color.Green);
                _ColorTable.Add("greenyellow", Color.GreenYellow);
                _ColorTable.Add("honeydew", Color.Honeydew);
                _ColorTable.Add("hotpink", Color.HotPink);
                _ColorTable.Add("indianred", Color.IndianRed);
                _ColorTable.Add("indigo", Color.Indigo);
                _ColorTable.Add("ivory", Color.Ivory);
                _ColorTable.Add("khaki", Color.Khaki);
                _ColorTable.Add("lavender", Color.Lavender);
                _ColorTable.Add("lavenderblush", Color.LavenderBlush);
                _ColorTable.Add("lawngreen", Color.LawnGreen);
                _ColorTable.Add("lemonchiffon", Color.LemonChiffon);
                _ColorTable.Add("lightblue", Color.LightBlue);
                _ColorTable.Add("lightcoral", Color.LightCoral);
                _ColorTable.Add("lightcyan", Color.LightCyan);
                _ColorTable.Add("lightgoldenrodyellow", Color.LightGoldenrodYellow);
                _ColorTable.Add("lightgray", Color.LightGray);
                _ColorTable.Add("lightgreen", Color.LightGreen);
                _ColorTable.Add("lightpink", Color.LightPink);
                _ColorTable.Add("lightsalmon", Color.LightSalmon);
                _ColorTable.Add("lightseagreen", Color.LightSeaGreen);
                _ColorTable.Add("lightskyblue", Color.LightSkyBlue);
                _ColorTable.Add("lightslategray", Color.LightSlateGray);
                _ColorTable.Add("lightsteelblue", Color.LightSteelBlue);
                _ColorTable.Add("lightyellow", Color.LightYellow);
                _ColorTable.Add("lime", Color.Lime);
                _ColorTable.Add("limegreen", Color.LimeGreen);
                _ColorTable.Add("linen", Color.Linen);
                _ColorTable.Add("magenta", Color.Magenta);
                _ColorTable.Add("maroon", Color.Maroon);
                _ColorTable.Add("mediumaquamarine", Color.MediumAquamarine);
                _ColorTable.Add("mediumblue", Color.MediumBlue);
                _ColorTable.Add("mediumorchid", Color.MediumOrchid);
                _ColorTable.Add("mediumpurple", Color.MediumPurple);
                _ColorTable.Add("mediumseagreen", Color.MediumSeaGreen);
                _ColorTable.Add("mediumslateblue", Color.MediumSlateBlue);
                _ColorTable.Add("mediumspringgreen", Color.MediumSpringGreen);
                _ColorTable.Add("mediumturquoise", Color.MediumTurquoise);
                _ColorTable.Add("mediumvioletred", Color.MediumVioletRed);
                _ColorTable.Add("midnightblue", Color.MidnightBlue);
                _ColorTable.Add("mintcream", Color.MintCream);
                _ColorTable.Add("mistyrose", Color.MistyRose);
                _ColorTable.Add("moccasin", Color.Moccasin);
                _ColorTable.Add("navajowhite", Color.NavajoWhite);
                _ColorTable.Add("navy", Color.Navy);
                _ColorTable.Add("oldlace", Color.OldLace);
                _ColorTable.Add("olive", Color.Olive);
                _ColorTable.Add("olivedrab", Color.OliveDrab);
                _ColorTable.Add("orange", Color.Orange);
                _ColorTable.Add("orangered", Color.OrangeRed);
                _ColorTable.Add("orchid", Color.Orchid);
                _ColorTable.Add("palegoldenrod", Color.PaleGoldenrod);
                _ColorTable.Add("palegreen", Color.PaleGreen);
                _ColorTable.Add("paleturquoise", Color.PaleTurquoise);
                _ColorTable.Add("palevioletred", Color.PaleVioletRed);
                _ColorTable.Add("papayawhip", Color.PapayaWhip);
                _ColorTable.Add("peachpuff", Color.PeachPuff);
                _ColorTable.Add("peru", Color.Peru);
                _ColorTable.Add("pink", Color.Pink);
                _ColorTable.Add("plum", Color.Plum);
                _ColorTable.Add("powderblue", Color.PowderBlue);
                _ColorTable.Add("purple", Color.Purple);
                _ColorTable.Add("red", Color.Red);
                _ColorTable.Add("rosybrown", Color.RosyBrown);
                _ColorTable.Add("royalblue", Color.RoyalBlue);
                _ColorTable.Add("saddlebrown", Color.SaddleBrown);
                _ColorTable.Add("salmon", Color.Salmon);
                _ColorTable.Add("sandybrown", Color.SandyBrown);
                _ColorTable.Add("seagreen", Color.SeaGreen);
                _ColorTable.Add("seashell", Color.SeaShell);
                _ColorTable.Add("sienna", Color.Sienna);
                _ColorTable.Add("silver", Color.Silver);
                _ColorTable.Add("skyblue", Color.SkyBlue);
                _ColorTable.Add("slateblue", Color.SlateBlue);
                _ColorTable.Add("slategray", Color.SlateGray);
                _ColorTable.Add("snow", Color.Snow);
                _ColorTable.Add("springgreen", Color.SpringGreen);
                _ColorTable.Add("steelblue", Color.SteelBlue);
                _ColorTable.Add("tan", Color.Tan);
                _ColorTable.Add("teal", Color.Teal);
                _ColorTable.Add("thistle", Color.Thistle);
                _ColorTable.Add("tomato", Color.Tomato);
                _ColorTable.Add("turquoise", Color.Turquoise);
                _ColorTable.Add("violet", Color.Violet);
                _ColorTable.Add("wheat", Color.Wheat);
                _ColorTable.Add("white", Color.White);
                _ColorTable.Add("whitesmoke", Color.WhiteSmoke);
                _ColorTable.Add("yellow", Color.Yellow);
                _ColorTable.Add("yellowgreen", Color.YellowGreen);
            }
        }

        public static string GetColorNameByArgb(int argb)
        {
            _EnsureColorNameTable();

            if (_ColorNameTable.Contains(argb))
            {
                return (string)_ColorNameTable[argb];
            }

            return string.Empty;
        }

        public static ColorX GetColorByName(string name)
        {
            _EnsureColorTable();

            if (!string.IsNullOrWhiteSpace(name))
            {
                string Name = new Regex(@"[^A-Za-z]").Replace(name, string.Empty).ToLower();

                if (!string.IsNullOrEmpty(Name))
                {
                    if (_ColorTable.Contains(Name))
                    {
                        return (Color)_ColorTable[Name];
                    }
                }
            }

            return Color.Empty;
        }
    }
}