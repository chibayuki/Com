/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2013-2018 chibayuki@foxmail.com

Com.WinForm.RecommendColors
Version 18.5.23.0000

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

        private ColorX _FormBackground = new ColorX(); // 窗体背景颜色。
        private ColorX _FormTitleBar = new ColorX(); // 窗体标题栏颜色。
        private ColorX _FormCaption = new ColorX(); // 窗体标题颜色。

        private ColorX __ControlButton = new ColorX(); // 控制按钮颜色。
        private ColorX __ControlButton_DEC = new ColorX(); // 控制按钮颜色，降低对比度。
        private ColorX __ControlButton_INC = new ColorX(); // 控制按钮颜色，提高对比度。

        private ColorX __ExitButton = new ColorX(); // 退出按钮颜色。
        private ColorX __ExitButton_DEC = new ColorX(); // 退出按钮颜色，降低对比度。
        private ColorX __ExitButton_INC = new ColorX(); // 退出按钮颜色，提高对比度。

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

        internal ColorX _ControlButton // 控制按钮颜色。
        {
            get
            {
                return __ControlButton;
            }
        }

        internal ColorX _ControlButton_DEC // 控制按钮颜色，降低对比度。
        {
            get
            {
                return __ControlButton_DEC;
            }
        }

        internal ColorX _ControlButton_INC // 控制按钮颜色，提高对比度。
        {
            get
            {
                return __ControlButton_INC;
            }
        }

        //

        internal ColorX _ExitButton // 退出按钮颜色。
        {
            get
            {
                return __ExitButton;
            }
        }

        internal ColorX _ExitButton_DEC // 退出按钮颜色，降低对比度。
        {
            get
            {
                return __ExitButton_DEC;
            }
        }

        internal ColorX _ExitButton_INC // 退出按钮颜色，提高对比度。
        {
            get
            {
                return __ExitButton_INC;
            }
        }

        //

        internal static bool _BackColorFitLightText(ColorX color) // 返回表示指定的背景色是否与浅色文本相符的布尔值。
        {
            return (color.Lightness_LAB < 70);
        }

        internal static bool _BackColorFitDarkText(ColorX color) // 返回表示指定的背景色是否与深色文本相符的布尔值。
        {
            return (color.Lightness_LAB > 30);
        }

        //

        internal void _Ctor(Theme theme, ColorX themeColor, bool showFormTitleBarColor) // 为以主题、主题色与表示是否在窗体标题栏上显示主题色的布尔值为参数的构造函数提供实现。
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

                        if (showFormTitleBarColor)
                        {
                            _FormTitleBar = _Main;
                            _FormCaption = new ColorX(!_BackColorFitLightText(_FormTitleBar) ? Color.Black : Color.White);
                        }
                        else
                        {
                            _FormTitleBar = _FormBackground;
                            _FormCaption = themeColor.AtLightness_HSL(24);
                        }

                        __ControlButton = ColorX.Transparent;
                        __ControlButton_INC = (!_BackColorFitLightText(_FormTitleBar) ? ColorManipulation.ShiftLightnessByHSL(_FormTitleBar, -0.2) : ColorManipulation.ShiftLightnessByHSL(_FormTitleBar, 0.3)).AtOpacity(70);
                        __ControlButton_DEC = __ControlButton_INC.AtOpacity(50);

                        __ExitButton = ColorX.Transparent;
                        __ExitButton_DEC = ColorX.FromRGB(232, 17, 35);
                        __ExitButton_INC = __ExitButton_DEC.AtOpacity(70);

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

                        if (showFormTitleBarColor)
                        {
                            _FormTitleBar = _Main;
                            _FormCaption = new ColorX(!_BackColorFitLightText(_FormTitleBar) ? Color.Black : Color.White);
                        }
                        else
                        {
                            _FormTitleBar = _FormBackground;
                            _FormCaption = themeColor.AtLightness_HSL(24).GrayscaleColor;
                        }

                        __ControlButton = ColorX.Transparent;
                        __ControlButton_INC = (!_BackColorFitLightText(_FormTitleBar) ? ColorManipulation.ShiftLightnessByHSL(_FormTitleBar, -0.2) : ColorManipulation.ShiftLightnessByHSL(_FormTitleBar, 0.3)).AtOpacity(70);
                        __ControlButton_DEC = __ControlButton_INC.AtOpacity(50);

                        __ExitButton = ColorX.Transparent;
                        __ExitButton_DEC = ColorX.FromRGB(232, 17, 35);
                        __ExitButton_INC = __ExitButton_DEC.AtOpacity(70);

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

                        if (showFormTitleBarColor)
                        {
                            _FormTitleBar = _Main;
                            _FormCaption = new ColorX(!_BackColorFitLightText(_FormTitleBar) ? Color.Black : Color.White);
                        }
                        else
                        {
                            _FormTitleBar = _FormBackground;
                            _FormCaption = themeColor.AtLightness_HSL(24).GrayscaleColor;
                        }

                        __ControlButton = ColorX.Transparent;
                        __ControlButton_INC = (!_BackColorFitLightText(_FormTitleBar) ? ColorManipulation.ShiftLightnessByHSL(_FormTitleBar, -0.2) : ColorManipulation.ShiftLightnessByHSL(_FormTitleBar, 0.3)).AtOpacity(70);
                        __ControlButton_DEC = __ControlButton_INC.AtOpacity(50);

                        __ExitButton = ColorX.Transparent;
                        __ExitButton_DEC = ColorX.FromRGB(232, 17, 35);
                        __ExitButton_INC = __ExitButton_DEC.AtOpacity(70);

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

                        if (showFormTitleBarColor)
                        {
                            _FormTitleBar = _Main;
                            _FormCaption = new ColorX(!_BackColorFitLightText(_FormTitleBar) ? Color.Black : Color.White);
                        }
                        else
                        {
                            _FormTitleBar = _FormBackground;
                            _FormCaption = themeColor.AtLightness_HSL(76).GrayscaleColor;
                        }

                        __ControlButton = ColorX.Transparent;
                        __ControlButton_INC = (!_BackColorFitLightText(_FormTitleBar) ? ColorManipulation.ShiftLightnessByHSL(_FormTitleBar, -0.2) : ColorManipulation.ShiftLightnessByHSL(_FormTitleBar, 0.3)).AtOpacity(70);
                        __ControlButton_DEC = __ControlButton_INC.AtOpacity(50);

                        __ExitButton = ColorX.Transparent;
                        __ExitButton_DEC = ColorX.FromRGB(232, 17, 35);
                        __ExitButton_INC = __ExitButton_DEC.AtOpacity(70);

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

                        if (showFormTitleBarColor)
                        {
                            _FormTitleBar = _Main;
                            _FormCaption = new ColorX(!_BackColorFitLightText(_FormTitleBar) ? Color.Black : Color.White);
                        }
                        else
                        {
                            _FormTitleBar = _FormBackground;
                            _FormCaption = themeColor.AtLightness_HSL(76).GrayscaleColor;
                        }

                        __ControlButton = ColorX.Transparent;
                        __ControlButton_INC = (!_BackColorFitLightText(_FormTitleBar) ? ColorManipulation.ShiftLightnessByHSL(_FormTitleBar, -0.2) : ColorManipulation.ShiftLightnessByHSL(_FormTitleBar, 0.3)).AtOpacity(70);
                        __ControlButton_DEC = __ControlButton_INC.AtOpacity(50);

                        __ExitButton = ColorX.Transparent;
                        __ExitButton_DEC = ColorX.FromRGB(232, 17, 35);
                        __ExitButton_INC = __ExitButton_DEC.AtOpacity(70);

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

        #region 构造与析构函数

        internal RecommendColors(Theme theme, ColorX themeColorX, bool showFormTitleColorX) // 使用主题、主题色与表示是否在窗体标题栏上显示主题色的布尔值初始化 RecommendColors 的新实例。
        {
            _Ctor(theme, themeColorX, showFormTitleColorX);
        }

        #endregion

        #region 属性

        /// <summary>
        /// 窗体背景颜色。
        /// </summary>
        public ColorX FormBackground
        {
            get
            {
                return _FormBackground;
            }
        }

        /// <summary>
        /// 窗体标题栏颜色。
        /// </summary>
        public ColorX FormTitleBar
        {
            get
            {
                return _FormTitleBar;
            }
        }

        /// <summary>
        /// 窗体标题颜色。
        /// </summary>
        public ColorX FormCaption
        {
            get
            {
                return _FormCaption;
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
    }
}