/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2018 chibayuki@foxmail.com

Com.WinForm.RecommendColors
Version 18.7.6.2250

This file is part of Com

Com is released under the GPLv3 license
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace Com.WinForm
{
    /// <summary>
    /// 建议的颜色。
    /// </summary>
    public sealed class RecommendColors
    {
        #region 私有与内部成员

        private ColorX _FormBackground = new ColorX(); // 窗口背景颜色。
        private ColorX _CaptionBar = new ColorX(); // 标题栏颜色。
        private ColorX _Caption = new ColorX(); // 窗口标题颜色。

        private ColorX _ControlButton = new ColorX(); // 控制按钮颜色。
        private ColorX _ControlButton_DEC = new ColorX(); // 控制按钮颜色，降低对比度。
        private ColorX _ControlButton_INC = new ColorX(); // 控制按钮颜色，提高对比度。

        private ColorX _ExitButton = new ColorX(); // 退出按钮颜色。
        private ColorX _ExitButton_DEC = new ColorX(); // 退出按钮颜色，降低对比度。
        private ColorX _ExitButton_INC = new ColorX(); // 退出按钮颜色，提高对比度。

        private ColorX _MenuItemBackground = new ColorX(); // 菜单项背景颜色。
        private ColorX _MenuItemText = new ColorX(); // 菜单项文字颜色。

        private ColorX _Main = new ColorX(); // 主要颜色。
        private ColorX _Main_DEC = new ColorX(); // 主要颜色，降低对比度。
        private ColorX _Main_INC = new ColorX(); // 主要颜色，提高对比度。

        private ColorX _Text = new ColorX(); // 文本颜色。
        private ColorX _Text_DEC = new ColorX(); // 文本颜色，降低对比度。
        private ColorX _Text_INC = new ColorX(); // 文本颜色，提高对比度。

        private ColorX _Background = new ColorX(); // 背景颜色。
        private ColorX _Background_DEC = new ColorX(); // 背景颜色，降低对比度。
        private ColorX _Background_INC = new ColorX(); // 背景颜色，提高对比度。

        private ColorX _Border = new ColorX(); // 边框颜色。
        private ColorX _Border_DEC = new ColorX(); // 边框颜色，降低对比度。
        private ColorX _Border_INC = new ColorX(); // 边框颜色，提高对比度。

        private ColorX _Button = new ColorX(); // 按钮颜色。
        private ColorX _Button_DEC = new ColorX(); // 按钮颜色，降低对比度。
        private ColorX _Button_INC = new ColorX(); // 按钮颜色，提高对比度。

        private ColorX _Slider = new ColorX(); // 滑块颜色。
        private ColorX _Slider_DEC = new ColorX(); // 滑块颜色，降低对比度。
        private ColorX _Slider_INC = new ColorX(); // 滑块颜色，提高对比度。

        private ColorX _ScrollBar = new ColorX(); // 滚动条颜色。
        private ColorX _ScrollBar_DEC = new ColorX(); // 滚动条颜色，降低对比度。
        private ColorX _ScrollBar_INC = new ColorX(); // 滚动条颜色，提高对比度。

        //

        internal ColorX ControlButton // 控制按钮颜色。
        {
            get
            {
                return _ControlButton;
            }
        }

        internal ColorX ControlButton_DEC // 控制按钮颜色，降低对比度。
        {
            get
            {
                return _ControlButton_DEC;
            }
        }

        internal ColorX ControlButton_INC // 控制按钮颜色，提高对比度。
        {
            get
            {
                return _ControlButton_INC;
            }
        }

        //

        internal ColorX ExitButton // 退出按钮颜色。
        {
            get
            {
                return _ExitButton;
            }
        }

        internal ColorX ExitButton_DEC // 退出按钮颜色，降低对比度。
        {
            get
            {
                return _ExitButton_DEC;
            }
        }

        internal ColorX ExitButton_INC // 退出按钮颜色，提高对比度。
        {
            get
            {
                return _ExitButton_INC;
            }
        }

        //

        internal static bool BackColorFitLightText(ColorX color) // 返回表示指定的背景色是否与浅色文本相符的布尔值。
        {
            return (color.Lightness_LAB < 70);
        }

        internal static bool BackColorFitDarkText(ColorX color) // 返回表示指定的背景色是否与深色文本相符的布尔值。
        {
            return (color.Lightness_LAB > 30);
        }

        //

        private void _Ctor(Theme theme, ColorX themeColor, bool showCaptionBarColor) // 为以主题、主题色与表示是否在标题栏上显示主题色的布尔值为参数的构造函数提供实现。
        {
            switch (theme)
            {
                case Theme.Colorful:
                    {
                        _Main = ColorManipulation.BlendByLAB(themeColor.AtLightness_HSL(56), themeColor, 0.75);
                        _Main = ColorManipulation.ShiftLightnessByLAB(_Main, (1 - _Main.Lightness_LAB / 50) / 4);
                        _Main_DEC = ColorManipulation.ShiftLightnessByHSL(_Main, 0.2);
                        _Main_INC = ColorManipulation.ShiftLightnessByHSL(_Main, -0.15);

                        _FormBackground = themeColor.AtLightness_HSL(98);

                        if (showCaptionBarColor)
                        {
                            _CaptionBar = _Main;
                            _Caption = new ColorX(!BackColorFitLightText(_CaptionBar) ? Color.Black : Color.White);
                        }
                        else
                        {
                            _CaptionBar = _FormBackground;
                            _Caption = themeColor.AtLightness_HSL(24);
                        }

                        _ControlButton = ColorX.Transparent;
                        _ControlButton_INC = (!BackColorFitLightText(_CaptionBar) ? ColorManipulation.ShiftLightnessByHSL(_CaptionBar, -0.2) : ColorManipulation.ShiftLightnessByHSL(_CaptionBar, 0.3)).AtOpacity(70);
                        _ControlButton_DEC = _ControlButton_INC.AtOpacity(50);

                        _ExitButton = ColorX.Transparent;
                        _ExitButton_DEC = ColorX.FromRGB(232, 17, 35);
                        _ExitButton_INC = _ExitButton_DEC.AtOpacity(70);

                        _MenuItemBackground = themeColor.AtLightness_HSL(98);
                        _MenuItemText = themeColor.AtLightness_HSL(24);

                        _Text = themeColor.AtLightness_HSL(40);
                        _Text_DEC = themeColor.AtLightness_HSL(56);
                        _Text_INC = themeColor.AtLightness_HSL(24);

                        _Background = themeColor.AtLightness_HSL(92);
                        _Background_DEC = themeColor.AtLightness_HSL(96);
                        _Background_INC = themeColor.AtLightness_HSL(88);

                        _Border = themeColor.AtLightness_HSL(68);
                        _Border_DEC = themeColor.AtLightness_HSL(84);
                        _Border_INC = themeColor.AtLightness_HSL(52);

                        _Button = themeColor.AtLightness_HSL(76);
                        _Button_DEC = themeColor.AtLightness_HSL(82);
                        _Button_INC = themeColor.AtLightness_HSL(70);

                        _Slider = themeColor.AtLightness_HSL(68);
                        _Slider_DEC = themeColor.AtLightness_HSL(76);
                        _Slider_INC = themeColor.AtLightness_HSL(60);

                        _ScrollBar = themeColor.AtLightness_HSL(84);
                        _ScrollBar_DEC = themeColor.AtLightness_HSL(86);
                        _ScrollBar_INC = themeColor.AtLightness_HSL(82);
                    }
                    break;

                case Theme.White:
                    {
                        _Main = ColorManipulation.BlendByLAB(themeColor.AtLightness_HSL(56), themeColor, 0.75);
                        _Main = ColorManipulation.ShiftLightnessByLAB(_Main, (1 - _Main.Lightness_LAB / 50) / 4);
                        _Main_DEC = ColorManipulation.ShiftLightnessByHSL(_Main, 0.2);
                        _Main_INC = ColorManipulation.ShiftLightnessByHSL(_Main, -0.15);

                        _FormBackground = themeColor.AtLightness_HSL(98).GrayscaleColor;

                        if (showCaptionBarColor)
                        {
                            _CaptionBar = _Main;
                            _Caption = new ColorX(!BackColorFitLightText(_CaptionBar) ? Color.Black : Color.White);
                        }
                        else
                        {
                            _CaptionBar = _FormBackground;
                            _Caption = themeColor.AtLightness_HSL(24).GrayscaleColor;
                        }

                        _ControlButton = ColorX.Transparent;
                        _ControlButton_INC = (!BackColorFitLightText(_CaptionBar) ? ColorManipulation.ShiftLightnessByHSL(_CaptionBar, -0.2) : ColorManipulation.ShiftLightnessByHSL(_CaptionBar, 0.3)).AtOpacity(70);
                        _ControlButton_DEC = _ControlButton_INC.AtOpacity(50);

                        _ExitButton = ColorX.Transparent;
                        _ExitButton_DEC = ColorX.FromRGB(232, 17, 35);
                        _ExitButton_INC = _ExitButton_DEC.AtOpacity(70);

                        _MenuItemBackground = themeColor.AtLightness_HSL(98).GrayscaleColor;
                        _MenuItemText = themeColor.AtLightness_HSL(24).GrayscaleColor;

                        _Text = themeColor.AtLightness_HSL(40).GrayscaleColor;
                        _Text_DEC = themeColor.AtLightness_HSL(56).GrayscaleColor;
                        _Text_INC = themeColor.AtLightness_HSL(24).GrayscaleColor;

                        _Background = themeColor.AtLightness_HSL(92).GrayscaleColor;
                        _Background_DEC = themeColor.AtLightness_HSL(96).GrayscaleColor;
                        _Background_INC = themeColor.AtLightness_HSL(88).GrayscaleColor;

                        _Border = themeColor.AtLightness_HSL(68).GrayscaleColor;
                        _Border_DEC = themeColor.AtLightness_HSL(84).GrayscaleColor;
                        _Border_INC = themeColor.AtLightness_HSL(52).GrayscaleColor;

                        _Button = themeColor.AtLightness_HSL(76).GrayscaleColor;
                        _Button_DEC = themeColor.AtLightness_HSL(82).GrayscaleColor;
                        _Button_INC = themeColor.AtLightness_HSL(70).GrayscaleColor;

                        _Slider = themeColor.AtLightness_HSL(68).GrayscaleColor;
                        _Slider_DEC = themeColor.AtLightness_HSL(76).GrayscaleColor;
                        _Slider_INC = themeColor.AtLightness_HSL(60).GrayscaleColor;

                        _ScrollBar = themeColor.AtLightness_HSL(84).GrayscaleColor;
                        _ScrollBar_DEC = themeColor.AtLightness_HSL(86).GrayscaleColor;
                        _ScrollBar_INC = themeColor.AtLightness_HSL(82).GrayscaleColor;
                    }
                    break;

                case Theme.LightGray:
                    {
                        _Main = ColorManipulation.BlendByLAB(themeColor.AtLightness_HSL(56), themeColor, 0.75);
                        _Main = ColorManipulation.ShiftLightnessByLAB(_Main, (1 - _Main.Lightness_LAB / 50) / 4).GrayscaleColor;
                        _Main_DEC = ColorManipulation.ShiftLightnessByHSL(_Main, 0.2);
                        _Main_INC = ColorManipulation.ShiftLightnessByHSL(_Main, -0.15);

                        _FormBackground = themeColor.AtLightness_HSL(98).GrayscaleColor;

                        if (showCaptionBarColor)
                        {
                            _CaptionBar = _Main;
                            _Caption = new ColorX(!BackColorFitLightText(_CaptionBar) ? Color.Black : Color.White);
                        }
                        else
                        {
                            _CaptionBar = _FormBackground;
                            _Caption = themeColor.AtLightness_HSL(24).GrayscaleColor;
                        }

                        _ControlButton = ColorX.Transparent;
                        _ControlButton_INC = (!BackColorFitLightText(_CaptionBar) ? ColorManipulation.ShiftLightnessByHSL(_CaptionBar, -0.2) : ColorManipulation.ShiftLightnessByHSL(_CaptionBar, 0.3)).AtOpacity(70);
                        _ControlButton_DEC = _ControlButton_INC.AtOpacity(50);

                        _ExitButton = ColorX.Transparent;
                        _ExitButton_DEC = ColorX.FromRGB(232, 17, 35);
                        _ExitButton_INC = _ExitButton_DEC.AtOpacity(70);

                        _MenuItemBackground = themeColor.AtLightness_HSL(98).GrayscaleColor;
                        _MenuItemText = themeColor.AtLightness_HSL(24).GrayscaleColor;

                        _Text = themeColor.AtLightness_HSL(40).GrayscaleColor;
                        _Text_DEC = themeColor.AtLightness_HSL(56);
                        _Text_INC = themeColor.AtLightness_HSL(24);

                        _Background = themeColor.AtLightness_HSL(92).GrayscaleColor;
                        _Background_DEC = themeColor.AtLightness_HSL(96);
                        _Background_INC = themeColor.AtLightness_HSL(88);

                        _Border = themeColor.AtLightness_HSL(68).GrayscaleColor;
                        _Border_DEC = themeColor.AtLightness_HSL(84);
                        _Border_INC = themeColor.AtLightness_HSL(52);

                        _Button = themeColor.AtLightness_HSL(76).GrayscaleColor;
                        _Button_DEC = themeColor.AtLightness_HSL(82);
                        _Button_INC = themeColor.AtLightness_HSL(70);

                        _Slider = themeColor.AtLightness_HSL(68).GrayscaleColor;
                        _Slider_DEC = themeColor.AtLightness_HSL(76);
                        _Slider_INC = themeColor.AtLightness_HSL(60);

                        _ScrollBar = themeColor.AtLightness_HSL(84).GrayscaleColor;
                        _ScrollBar_DEC = themeColor.AtLightness_HSL(86);
                        _ScrollBar_INC = themeColor.AtLightness_HSL(82);
                    }
                    break;

                case Theme.DarkGray:
                    {
                        _Main = ColorManipulation.BlendByLAB(themeColor.AtLightness_HSL(44), themeColor, 0.75);
                        _Main = ColorManipulation.ShiftLightnessByLAB(_Main, (1 - _Main.Lightness_LAB / 50) / 4).GrayscaleColor;
                        _Main_DEC = ColorManipulation.ShiftLightnessByHSL(_Main, -0.2);
                        _Main_INC = ColorManipulation.ShiftLightnessByHSL(_Main, 0.15);

                        _FormBackground = themeColor.AtLightness_HSL(2).GrayscaleColor;

                        if (showCaptionBarColor)
                        {
                            _CaptionBar = _Main;
                            _Caption = new ColorX(!BackColorFitLightText(_CaptionBar) ? Color.Black : Color.White);
                        }
                        else
                        {
                            _CaptionBar = _FormBackground;
                            _Caption = themeColor.AtLightness_HSL(76).GrayscaleColor;
                        }

                        _ControlButton = ColorX.Transparent;
                        _ControlButton_INC = (!BackColorFitLightText(_CaptionBar) ? ColorManipulation.ShiftLightnessByHSL(_CaptionBar, -0.2) : ColorManipulation.ShiftLightnessByHSL(_CaptionBar, 0.3)).AtOpacity(70);
                        _ControlButton_DEC = _ControlButton_INC.AtOpacity(50);

                        _ExitButton = ColorX.Transparent;
                        _ExitButton_DEC = ColorX.FromRGB(232, 17, 35);
                        _ExitButton_INC = _ExitButton_DEC.AtOpacity(70);

                        _MenuItemBackground = themeColor.AtLightness_HSL(98).GrayscaleColor;
                        _MenuItemText = themeColor.AtLightness_HSL(24).GrayscaleColor;

                        _Text = themeColor.AtLightness_HSL(60).GrayscaleColor;
                        _Text_DEC = themeColor.AtLightness_HSL(44);
                        _Text_INC = themeColor.AtLightness_HSL(76);

                        _Background = themeColor.AtLightness_HSL(8).GrayscaleColor;
                        _Background_DEC = themeColor.AtLightness_HSL(4);
                        _Background_INC = themeColor.AtLightness_HSL(12);

                        _Border = themeColor.AtLightness_HSL(32).GrayscaleColor;
                        _Border_DEC = themeColor.AtLightness_HSL(16);
                        _Border_INC = themeColor.AtLightness_HSL(48);

                        _Button = themeColor.AtLightness_HSL(24).GrayscaleColor;
                        _Button_DEC = themeColor.AtLightness_HSL(18);
                        _Button_INC = themeColor.AtLightness_HSL(30);

                        _Slider = themeColor.AtLightness_HSL(32).GrayscaleColor;
                        _Slider_DEC = themeColor.AtLightness_HSL(24);
                        _Slider_INC = themeColor.AtLightness_HSL(40);

                        _ScrollBar = themeColor.AtLightness_HSL(16).GrayscaleColor;
                        _ScrollBar_DEC = themeColor.AtLightness_HSL(14);
                        _ScrollBar_INC = themeColor.AtLightness_HSL(18);
                    }
                    break;

                case Theme.Black:
                    {
                        _Main = ColorManipulation.BlendByLAB(themeColor.AtLightness_HSL(44), themeColor, 0.75);
                        _Main = ColorManipulation.ShiftLightnessByLAB(_Main, (1 - _Main.Lightness_LAB / 50) / 4);
                        _Main_DEC = ColorManipulation.ShiftLightnessByHSL(_Main, -0.2);
                        _Main_INC = ColorManipulation.ShiftLightnessByHSL(_Main, 0.15);

                        _FormBackground = themeColor.AtLightness_HSL(2).GrayscaleColor;

                        if (showCaptionBarColor)
                        {
                            _CaptionBar = _Main;
                            _Caption = new ColorX(!BackColorFitLightText(_CaptionBar) ? Color.Black : Color.White);
                        }
                        else
                        {
                            _CaptionBar = _FormBackground;
                            _Caption = themeColor.AtLightness_HSL(76).GrayscaleColor;
                        }

                        _ControlButton = ColorX.Transparent;
                        _ControlButton_INC = (!BackColorFitLightText(_CaptionBar) ? ColorManipulation.ShiftLightnessByHSL(_CaptionBar, -0.2) : ColorManipulation.ShiftLightnessByHSL(_CaptionBar, 0.3)).AtOpacity(70);
                        _ControlButton_DEC = _ControlButton_INC.AtOpacity(50);

                        _ExitButton = ColorX.Transparent;
                        _ExitButton_DEC = ColorX.FromRGB(232, 17, 35);
                        _ExitButton_INC = _ExitButton_DEC.AtOpacity(70);

                        _MenuItemBackground = themeColor.AtLightness_HSL(98).GrayscaleColor;
                        _MenuItemText = themeColor.AtLightness_HSL(24).GrayscaleColor;

                        _Text = themeColor.AtLightness_HSL(60).GrayscaleColor;
                        _Text_DEC = themeColor.AtLightness_HSL(44).GrayscaleColor;
                        _Text_INC = themeColor.AtLightness_HSL(76).GrayscaleColor;

                        _Background = themeColor.AtLightness_HSL(8).GrayscaleColor;
                        _Background_DEC = themeColor.AtLightness_HSL(4).GrayscaleColor;
                        _Background_INC = themeColor.AtLightness_HSL(12).GrayscaleColor;

                        _Border = themeColor.AtLightness_HSL(32).GrayscaleColor;
                        _Border_DEC = themeColor.AtLightness_HSL(16).GrayscaleColor;
                        _Border_INC = themeColor.AtLightness_HSL(48).GrayscaleColor;

                        _Button = themeColor.AtLightness_HSL(24).GrayscaleColor;
                        _Button_DEC = themeColor.AtLightness_HSL(18).GrayscaleColor;
                        _Button_INC = themeColor.AtLightness_HSL(30).GrayscaleColor;

                        _Slider = themeColor.AtLightness_HSL(32).GrayscaleColor;
                        _Slider_DEC = themeColor.AtLightness_HSL(24).GrayscaleColor;
                        _Slider_INC = themeColor.AtLightness_HSL(40).GrayscaleColor;

                        _ScrollBar = themeColor.AtLightness_HSL(16).GrayscaleColor;
                        _ScrollBar_DEC = themeColor.AtLightness_HSL(14).GrayscaleColor;
                        _ScrollBar_INC = themeColor.AtLightness_HSL(18).GrayscaleColor;
                    }
                    break;
            }
        }

        #endregion

        #region 构造函数

        internal RecommendColors(Theme theme, ColorX themeColor, bool showCaptionBarColor) // 使用主题、主题色与表示是否在标题栏上显示主题色的布尔值初始化 RecommendColors 的新实例。
        {
            _Ctor(theme, themeColor, showCaptionBarColor);
        }

        #endregion

        #region 属性

        /// <summary>
        /// 窗口背景颜色。
        /// </summary>
        public ColorX FormBackground
        {
            get
            {
                return _FormBackground;
            }
        }

        /// <summary>
        /// 标题栏颜色。
        /// </summary>
        public ColorX CaptionBar
        {
            get
            {
                return _CaptionBar;
            }
        }

        /// <summary>
        /// 窗口标题颜色。
        /// </summary>
        public ColorX Caption
        {
            get
            {
                return _Caption;
            }
        }

        //

        /// <summary>
        /// 菜单项背景颜色。
        /// </summary>
        public ColorX MenuItemBackground
        {
            get
            {
                return _MenuItemBackground;
            }
        }

        /// <summary>
        /// 菜单项文字颜色。
        /// </summary>
        public ColorX MenuItemText
        {
            get
            {
                return _MenuItemText;
            }
        }

        //

        /// <summary>
        /// 主要颜色。
        /// </summary>
        public ColorX Main
        {
            get
            {
                return _Main;
            }
        }

        /// <summary>
        /// 主要颜色，降低对比度。
        /// </summary>
        public ColorX Main_DEC
        {
            get
            {
                return _Main_DEC;
            }
        }

        /// <summary>
        /// 主要颜色，提高对比度。
        /// </summary>
        public ColorX Main_INC
        {
            get
            {
                return _Main_INC;
            }
        }

        //

        /// <summary>
        /// 文本颜色。
        /// </summary>
        public ColorX Text
        {
            get
            {
                return _Text;
            }
        }

        /// <summary>
        /// 文本颜色，降低对比度。
        /// </summary>
        public ColorX Text_DEC
        {
            get
            {
                return _Text_DEC;
            }
        }

        /// <summary>
        /// 文本颜色，提高对比度。
        /// </summary>
        public ColorX Text_INC
        {
            get
            {
                return _Text_INC;
            }
        }

        //

        /// <summary>
        /// 背景颜色。
        /// </summary>
        public ColorX Background
        {
            get
            {
                return _Background;
            }
        }

        /// <summary>
        /// 背景颜色，降低对比度。
        /// </summary>
        public ColorX Background_DEC
        {
            get
            {
                return _Background_DEC;
            }
        }

        /// <summary>
        /// 背景颜色，提高对比度。
        /// </summary>
        public ColorX Background_INC
        {
            get
            {
                return _Background_INC;
            }
        }

        //

        /// <summary>
        /// 边框颜色。
        /// </summary>
        public ColorX Border
        {
            get
            {
                return _Border;
            }
        }

        /// <summary>
        /// 边框颜色，降低对比度。
        /// </summary>
        public ColorX Border_DEC
        {
            get
            {
                return _Border_DEC;
            }
        }

        /// <summary>
        /// 边框颜色，提高对比度。
        /// </summary>
        public ColorX Border_INC
        {
            get
            {
                return _Border_INC;
            }
        }

        //

        /// <summary>
        /// 按钮颜色。
        /// </summary>
        public ColorX Button
        {
            get
            {
                return _Button;
            }
        }

        /// <summary>
        /// 按钮颜色，降低对比度。
        /// </summary>
        public ColorX Button_DEC
        {
            get
            {
                return _Button_DEC;
            }
        }

        /// <summary>
        /// 按钮颜色，提高对比度。
        /// </summary>
        public ColorX Button_INC
        {
            get
            {
                return _Button_INC;
            }
        }

        //

        /// <summary>
        /// 滑块颜色。
        /// </summary>
        public ColorX Slider
        {
            get
            {
                return _Slider;
            }
        }

        /// <summary>
        /// 滑块颜色，降低对比度。
        /// </summary>
        public ColorX Slider_DEC
        {
            get
            {
                return _Slider_DEC;
            }
        }

        /// <summary>
        /// 滑块颜色，提高对比度。
        /// </summary>
        public ColorX Slider_INC
        {
            get
            {
                return _Slider_INC;
            }
        }

        //

        /// <summary>
        /// 滚动条颜色。
        /// </summary>
        public ColorX ScrollBar
        {
            get
            {
                return _ScrollBar;
            }
        }

        /// <summary>
        /// 滚动条颜色，降低对比度。
        /// </summary>
        public ColorX ScrollBar_DEC
        {
            get
            {
                return _ScrollBar_DEC;
            }
        }

        /// <summary>
        /// 滚动条颜色，提高对比度。
        /// </summary>
        public ColorX ScrollBar_INC
        {
            get
            {
                return _ScrollBar_INC;
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 判断此 RecommendColors 对象是否与指定的 RecommendColors 对象相等。
        /// </summary>
        /// <param name="recommendColors">用于比较的 RecommendColors 对象。</param>
        public bool Equals(RecommendColors recommendColors)
        {
            if ((object)recommendColors == null)
            {
                return false;
            }

            return (_FormBackground.Equals(recommendColors._FormBackground) && _CaptionBar.Equals(recommendColors._CaptionBar) && _Caption.Equals(recommendColors._Caption) && _ControlButton.Equals(recommendColors._ControlButton) && _ControlButton_DEC.Equals(recommendColors._ControlButton_DEC) && _ControlButton_INC.Equals(recommendColors._ControlButton_INC) && _ExitButton.Equals(recommendColors._ExitButton) && _ExitButton_DEC.Equals(recommendColors._ExitButton_DEC) && _ExitButton_INC.Equals(recommendColors._ExitButton_INC) && _MenuItemBackground.Equals(recommendColors._MenuItemBackground) && _MenuItemText.Equals(recommendColors._MenuItemText) && _Main.Equals(recommendColors._Main) && _Main_DEC.Equals(recommendColors._Main_DEC) && _Main_INC.Equals(recommendColors._Main_INC) && _Text.Equals(recommendColors._Text) && _Text_DEC.Equals(recommendColors._Text_DEC) && _Text_INC.Equals(recommendColors._Text_INC) && _Background.Equals(recommendColors._Background) && _Background_DEC.Equals(recommendColors._Background_DEC) && _Background_INC.Equals(recommendColors._Background_INC) && _Border.Equals(recommendColors._Border) && _Border_DEC.Equals(recommendColors._Border_DEC) && _Border_INC.Equals(recommendColors._Border_INC) && _Button.Equals(recommendColors._Button) && _Button_DEC.Equals(recommendColors._Button_DEC) && _Button_INC.Equals(recommendColors._Button_INC) && _Slider.Equals(recommendColors._Slider) && _Slider_DEC.Equals(recommendColors._Slider_DEC) && _Slider_INC.Equals(recommendColors._Slider_INC) && _ScrollBar.Equals(recommendColors._ScrollBar) && _ScrollBar_DEC.Equals(recommendColors._ScrollBar_DEC) && _ScrollBar_INC.Equals(recommendColors._ScrollBar_INC));
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 判断两个 RecommendColors 对象是否相等。
        /// </summary>
        /// <param name="left">用于比较的第一个 RecommendColors 对象。</param>
        /// <param name="right">用于比较的第二个 RecommendColors 对象。</param>
        public static bool Equals(RecommendColors left, RecommendColors right)
        {
            if ((object)left == null && (object)right == null)
            {
                return true;
            }
            else if (object.ReferenceEquals(left, right))
            {
                return true;
            }
            else if ((object)left == null || (object)right == null)
            {
                return false;
            }

            return left.Equals(right);
        }

        #endregion

        #region 基类方法

        /// <summary>
        /// 判断此 RecommendColors 对象是否与指定的对象相等。
        /// </summary>
        /// <param name="obj">用于比较的对象。</param>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is RecommendColors))
            {
                return false;
            }

            return Equals((RecommendColors)obj);
        }

        /// <summary>
        /// 返回此 RecommendColors 对象的哈希代码。
        /// </summary>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// 将此 RecommendColors 对象转换为字符串。
        /// </summary>
        public override string ToString()
        {
            return base.GetType().Name;
        }

        #endregion

        #region 运算符

        /// <summary>
        /// 判断两个 RecommendColors 对象是否相等。
        /// </summary>
        /// <param name="left">运算符左侧比较的 RecommendColors 对象。</param>
        /// <param name="right">运算符右侧比较的 RecommendColors 对象。</param>
        public static bool operator ==(RecommendColors left, RecommendColors right)
        {
            if ((object)left == null && (object)right == null)
            {
                return true;
            }
            else if (object.ReferenceEquals(left, right))
            {
                return true;
            }
            else if ((object)left == null || (object)right == null)
            {
                return false;
            }

            return (left._FormBackground == right._FormBackground && left._CaptionBar == right._CaptionBar && left._Caption == right._Caption && left._ControlButton == right._ControlButton && left._ControlButton_DEC == right._ControlButton_DEC && left._ControlButton_INC == right._ControlButton_INC && left._ExitButton == right._ExitButton && left._ExitButton_DEC == right._ExitButton_DEC && left._ExitButton_INC == right._ExitButton_INC && left._MenuItemBackground == right._MenuItemBackground && left._MenuItemText == right._MenuItemText && left._Main == right._Main && left._Main_DEC == right._Main_DEC && left._Main_INC == right._Main_INC && left._Text == right._Text && left._Text_DEC == right._Text_DEC && left._Text_INC == right._Text_INC && left._Background == right._Background && left._Background_DEC == right._Background_DEC && left._Background_INC == right._Background_INC && left._Border == right._Border && left._Border_DEC == right._Border_DEC && left._Border_INC == right._Border_INC && left._Button == right._Button && left._Button_DEC == right._Button_DEC && left._Button_INC == right._Button_INC && left._Slider == right._Slider && left._Slider_DEC == right._Slider_DEC && left._Slider_INC == right._Slider_INC && left._ScrollBar == right._ScrollBar && left._ScrollBar_DEC == right._ScrollBar_DEC && left._ScrollBar_INC == right._ScrollBar_INC);
        }

        /// <summary>
        /// 判断两个 RecommendColors 对象是否不相等。
        /// </summary>
        /// <param name="left">运算符左侧比较的 RecommendColors 对象。</param>
        /// <param name="right">运算符右侧比较的 RecommendColors 对象。</param>
        public static bool operator !=(RecommendColors left, RecommendColors right)
        {
            if ((object)left == null && (object)right == null)
            {
                return false;
            }
            else if (object.ReferenceEquals(left, right))
            {
                return false;
            }
            else if ((object)left == null || (object)right == null)
            {
                return true;
            }

            return (left._FormBackground != right._FormBackground || left._CaptionBar != right._CaptionBar || left._Caption != right._Caption || left._ControlButton != right._ControlButton || left._ControlButton_DEC != right._ControlButton_DEC || left._ControlButton_INC != right._ControlButton_INC || left._ExitButton != right._ExitButton || left._ExitButton_DEC != right._ExitButton_DEC || left._ExitButton_INC != right._ExitButton_INC || left._MenuItemBackground != right._MenuItemBackground || left._MenuItemText != right._MenuItemText || left._Main != right._Main || left._Main_DEC != right._Main_DEC || left._Main_INC != right._Main_INC || left._Text != right._Text || left._Text_DEC != right._Text_DEC || left._Text_INC != right._Text_INC || left._Background != right._Background || left._Background_DEC != right._Background_DEC || left._Background_INC != right._Background_INC || left._Border != right._Border || left._Border_DEC != right._Border_DEC || left._Border_INC != right._Border_INC || left._Button != right._Button || left._Button_DEC != right._Button_DEC || left._Button_INC != right._Button_INC || left._Slider != right._Slider || left._Slider_DEC != right._Slider_DEC || left._Slider_INC != right._Slider_INC || left._ScrollBar != right._ScrollBar || left._ScrollBar_DEC != right._ScrollBar_DEC || left._ScrollBar_INC != right._ScrollBar_INC);
        }

        #endregion
    }
}