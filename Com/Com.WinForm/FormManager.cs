/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2013-2018 chibayuki@foxmail.com

Com.WinForm.FormManager
Version 18.5.26.0000

This file is part of Com

Com is released under the GPLv3 license
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Com.WinForm
{
    /// <summary>
    /// 窗口管理器。
    /// </summary>
    public sealed class FormManager
    {
        #region 私有与内部成员

        private bool _Initialized = false; // 表示此 FormManager 对象是否已完成初始化的布尔值。

        private bool _LoadingNow = false; // 表示此 FormManager 对象是否正在执行 Loading 事件或 Loaded 事件的布尔值。
        private bool _ClosingNow = false; // 表示此 FormManager 对象是否正在执行 Closing 事件或 Closed 事件的布尔值。

        //

        private static List<FormManager> _FormManagerList = new List<FormManager>(1); // 窗口管理器列表。

        private bool _IsMainForm = false; // 表示此窗口是否为主窗口的布尔值。

        //

        private Form _Client = null; // 表示工作区的 Form 对象。

        internal Form Client // 获取表示工作区的 Form 对象。
        {
            get
            {
                return _Client;
            }
        }

        private CaptionBar _CaptionBar = null; // 表示标题栏的 CaptionBar 对象。

        internal CaptionBar CaptionBar // 获取表示标题栏的 CaptionBar 对象。
        {
            get
            {
                return _CaptionBar;
            }
        }

        private Resizer _Resizer = null; // 表示窗口大小调节器的 Resizer 对象。

        internal Resizer Resizer // 获取表示窗口大小调节器的 Resizer 对象。
        {
            get
            {
                return _Resizer;
            }
        }

        private SplashScreen _SplashScreen = null; // 表示启动屏幕的 SplashScreen 对象。

        internal SplashScreen SplashScreen // 获取表示启动屏幕的 SplashScreen 对象。
        {
            get
            {
                return _SplashScreen;
            }
        }

        //

        private FormManager _Owner = null; // 拥有此窗口的窗口的窗口管理器。
        private List<FormManager> _Owned = new List<FormManager>(0); // 表示此窗口拥有的所有窗口的窗口管理器列表。

        private void _AddOwned(FormManager formManager) // 向表示此窗口拥有的所有窗口的窗口管理器列表添加一个窗口管理器。
        {
            if (formManager != null && !_Owned.Contains(formManager))
            {
                _Owned.Add(formManager);

                if (Enabled == true && _Owned.Count > 0)
                {
                    Enabled = false;
                }
            }
        }

        private void _RemoveOwned(FormManager formManager) // 从表示此窗口拥有的所有窗口的窗口管理器列表删除一个窗口管理器。
        {
            if (formManager != null && _Owned.Contains(formManager))
            {
                _Owned.Remove(formManager);

                if (Enabled == false && _Owned.Count <= 0)
                {
                    Enabled = true;
                }
            }
        }

        //

        private BackgroundWorker _FormLoadingAsyncWorker = null; // 用于异步执行 Loading 事件的 BackgroundWorker。
        private BackgroundWorker _FormClosingAsyncWorker = null; // 用于异步执行 Closing 事件的 BackgroundWorker。

        //

        internal static Point CursorPosition // 鼠标指针在桌面的位置。
        {
            get
            {
                return Cursor.Position;
            }
        }

        internal static Rectangle PrimaryScreenClient // 主屏幕的工作区。
        {
            get
            {
                return Screen.PrimaryScreen.WorkingArea;
            }
        }

        internal static Rectangle PrimaryScreenBounds // 主屏幕的边界。
        {
            get
            {
                return Screen.PrimaryScreen.Bounds;
            }
        }

        //

        private const int _ControlBoxButtonWidth = 46; // 控制按钮的宽度。
        private const int _ControlBoxButtonHeight = 32; // 控制按钮的高度。

        internal int ControlBoxButtonWidth // 获取控制按钮的宽度。
        {
            get
            {
                return _ControlBoxButtonWidth;
            }
        }

        internal int ControlBoxButtonHeight // 获取控制按钮的高度。
        {
            get
            {
                return _ControlBoxButtonHeight;
            }
        }

        internal Size ControlBoxButtonSize // 获取控制按钮的大小。
        {
            get
            {
                return new Size(_ControlBoxButtonWidth, _ControlBoxButtonHeight);
            }
        }

        //

        private const int _ResizerSize = 8; // 窗口大小调节器的大小。

        internal int ResizerSize // 获取窗口大小调节器的大小。
        {
            get
            {
                return _ResizerSize;
            }
        }

        //

        private FormWindowState _LastFormWindowState = FormWindowState.Normal; // _Client.WindowState 的最终值。

        //

        private FormStyle _FormStyle = FormStyle.Sizable; // 窗口的样式。

        private bool _EnableFullScreen = true; // 表示是否允许窗口以全屏幕模式运行的布尔值。
        private bool _TopMost = false; // 表示是否允许窗口置于顶层的布尔值。
        private bool _ShowInTaskbar = true; // 表示窗口是否在任务栏显示的布尔值。

        //

        private int _CaptionBarHeight = _ControlBoxButtonHeight; // 标题栏的高度。

        //

        private int _MinimumWidth = 0; // 窗口的最小宽度。
        private int _MinimumHeight = 0; // 窗口的最小高度。

        internal int MinimumWidth // 获取或设置窗口的最小宽度。
        {
            get
            {
                return _MinimumWidth;
            }

            set
            {
                _MinimumWidth = Math.Max(0, value);
            }
        }

        internal int MinimumHeight // 获取或设置窗口的最小高度。
        {
            get
            {
                return _MinimumHeight;
            }

            set
            {
                _MinimumHeight = Math.Max(0, value);
            }
        }

        internal int MinimumBoundsWidth // 获取窗口的最小宽度。
        {
            get
            {
                return Math.Max(Math.Min(_ControlBoxButtonHeight, _CaptionBarHeight) + _ControlBoxButtonWidth, Math.Min(PrimaryScreenBounds.Width, _MinimumWidth));
            }
        }

        internal int MinimumBoundsHeight // 获取窗口的最小高度。
        {
            get
            {
                return Math.Max(_CaptionBarHeight + 2, Math.Min(PrimaryScreenBounds.Height, _MinimumHeight));
            }
        }

        internal Size MinimumBoundsSize // 获取窗口的最小大小。
        {
            get
            {
                return new Size(MinimumBoundsWidth, MinimumBoundsHeight);
            }
        }

        private int _MaximumWidth = 0; // 窗口的最大宽度。
        private int _MaximumHeight = 0; // 窗口的最大高度。

        internal int MaximumWidth // 获取或设置窗口的最大宽度。
        {
            get
            {
                return _MaximumWidth;
            }

            set
            {
                _MaximumWidth = Math.Max(0, value);
            }
        }

        internal int MaximumHeight // 获取或设置窗口的最大高度。
        {
            get
            {
                return _MaximumHeight;
            }

            set
            {
                _MaximumHeight = Math.Max(0, value);
            }
        }

        internal int MaximumBoundsWidth // 获取窗口的最大宽度。
        {
            get
            {
                return Math.Max(1, (_MaximumWidth > 0 ? Math.Min(PrimaryScreenBounds.Width, _MaximumWidth) : PrimaryScreenBounds.Width));
            }
        }

        internal int MaximumBoundsHeight // 获取窗口的最大高度。
        {
            get
            {
                return Math.Max(1, (_MaximumHeight > 0 ? Math.Min(PrimaryScreenBounds.Height, _MaximumHeight) : PrimaryScreenBounds.Height));
            }
        }

        internal Size MaximumBoundsSize // 获取窗口的最大大小。
        {
            get
            {
                return new Size(MaximumBoundsWidth, MaximumBoundsHeight);
            }
        }

        //

        private FormState _FormState = FormState.Normal; // 窗口的状态。
        private FormState _FormState_BeforeFullScreen = FormState.Normal; // 进入全屏幕状态之前窗口的状态。

        //

        private bool _Enabled = true; // 表示窗口是否对用户交互作出响应的布尔值。
        private double _Opacity = 1.0; // 窗口的不透明度。
        private string _Caption = string.Empty; // 窗口的标题。
        private Font _CaptionFont = new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, 134); // 窗口标题的字体。
        private ContentAlignment _CaptionAlign = ContentAlignment.TopCenter; // 窗口标题的文本对齐方式。
        private Bitmap _CaptionBarBackgroundImage = null; // 标题栏的背景图像。
        private Theme _Theme = Theme.Colorful; // 窗口的主题。
        private ColorX _ThemeColor = ColorX.FromRGB(128, 128, 128); // 窗口的主题色。
        private bool _ShowCaptionBarColor = true; // 表示是否在标题栏上显示主题色的布尔值。
        private bool _EnableCaptionBarTransparent = true; // 表示是否允许以半透明方式显示标题栏的布尔值。
        private RecommendColors _RecommendColors = null; // 当前主题建议的颜色。

        //

        private int _Bounds_Current_X = int.MinValue; // 当前状态窗口在桌面的左边距。
        private int _Bounds_Current_Y = int.MinValue; // 当前状态窗口在桌面的上边距。
        private int _Bounds_Current_Width = 300; // 当前状态窗口的宽度。
        private int _Bounds_Current_Height = 300; // 当前状态窗口的高度。

        internal int Bounds_Current_X // 获取或设置当前状态窗口在桌面的左边距。
        {
            get
            {
                return _Bounds_Current_X;
            }

            set
            {
                _Bounds_Current_X = value;
            }
        }

        internal int Bounds_Current_Y // 获取或设置当前状态窗口在桌面的上边距。
        {
            get
            {
                return _Bounds_Current_Y;
            }

            set
            {
                _Bounds_Current_Y = value;
            }
        }

        internal int Bounds_Current_Right // 获取当前状态窗口在桌面的右边距。
        {
            get
            {
                return (_Bounds_Current_X + _Bounds_Current_Width);
            }
        }

        internal int Bounds_Current_Bottom // 获取当前状态窗口在桌面的下边距。
        {
            get
            {
                return (_Bounds_Current_Y + _Bounds_Current_Height);
            }
        }

        internal int Bounds_Current_Width // 获取或设置当前状态窗口的宽度。
        {
            get
            {
                return _Bounds_Current_Width;
            }

            set
            {
                _Bounds_Current_Width = Math.Max(MinimumBoundsWidth, Math.Min(value, MaximumBoundsWidth));
            }
        }

        internal int Bounds_Current_Height // 获取或设置当前状态窗口的高度。
        {
            get
            {
                return _Bounds_Current_Height;
            }

            set
            {
                _Bounds_Current_Height = Math.Max(MinimumBoundsHeight, Math.Min(value, MaximumBoundsHeight));
            }
        }

        internal Point Bounds_Current_Location // 获取或设置当前状态窗口在桌面的位置。
        {
            get
            {
                return new Point(_Bounds_Current_X, _Bounds_Current_Y);
            }

            set
            {
                _Bounds_Current_X = value.X;
                _Bounds_Current_Y = value.Y;
            }
        }

        internal Size Bounds_Current_Size // 获取或设置当前状态窗口的大小。
        {
            get
            {
                return new Size(_Bounds_Current_Width, _Bounds_Current_Height);
            }

            set
            {
                _Bounds_Current_Width = Math.Max(MinimumBoundsWidth, Math.Min(value.Width, MaximumBoundsWidth));
                _Bounds_Current_Height = Math.Max(MinimumBoundsHeight, Math.Min(value.Height, MaximumBoundsHeight));
            }
        }

        internal Rectangle Bounds_Current // 获取或设置当前状态窗口的位置与大小。
        {
            get
            {
                return new Rectangle(_Bounds_Current_X, _Bounds_Current_Y, _Bounds_Current_Width, _Bounds_Current_Height);
            }

            set
            {
                _Bounds_Current_X = value.X;
                _Bounds_Current_Y = value.Y;
                _Bounds_Current_Width = Math.Max(MinimumBoundsWidth, Math.Min(value.Width, MaximumBoundsWidth));
                _Bounds_Current_Height = Math.Max(MinimumBoundsHeight, Math.Min(value.Height, MaximumBoundsHeight));
            }
        }

        private int _Bounds_Normal_X = int.MinValue; // 普通状态窗口在桌面的左边距。
        private int _Bounds_Normal_Y = int.MinValue; // 普通状态窗口在桌面的上边距。
        private int _Bounds_Normal_Width = 300; // 普通状态窗口的宽度。
        private int _Bounds_Normal_Height = 300; // 普通状态窗口的高度。

        internal int Bounds_Normal_X // 获取或设置普通状态窗口在桌面的左边距。
        {
            get
            {
                return _Bounds_Normal_X;
            }

            set
            {
                _Bounds_Normal_X = value;
            }
        }

        internal int Bounds_Normal_Y // 获取或设置普通状态窗口在桌面的上边距。
        {
            get
            {
                return _Bounds_Normal_Y;
            }

            set
            {
                _Bounds_Normal_Y = value;
            }
        }

        internal int Bounds_Normal_Right // 获取普通状态窗口在桌面的右边距。
        {
            get
            {
                return (_Bounds_Normal_X + _Bounds_Normal_Width);
            }
        }

        internal int Bounds_Normal_Bottom // 获取普通状态窗口在桌面的下边距。
        {
            get
            {
                return (_Bounds_Normal_Y + _Bounds_Normal_Height);
            }
        }

        internal int Bounds_Normal_Width // 获取或设置普通状态窗口的宽度。
        {
            get
            {
                return _Bounds_Normal_Width;
            }

            set
            {
                _Bounds_Normal_Width = Math.Max(MinimumBoundsWidth, Math.Min(value, MaximumBoundsWidth));
            }
        }

        internal int Bounds_Normal_Height // 获取或设置普通状态窗口的高度。
        {
            get
            {
                return _Bounds_Normal_Height;
            }

            set
            {
                _Bounds_Normal_Height = Math.Max(MinimumBoundsHeight, Math.Min(value, MaximumBoundsHeight));
            }
        }

        internal Point Bounds_Normal_Location // 获取或设置普通状态窗口在桌面的位置。
        {
            get
            {
                return new Point(_Bounds_Normal_X, _Bounds_Normal_Y);
            }

            set
            {
                _Bounds_Normal_X = value.X;
                _Bounds_Normal_Y = value.Y;
            }
        }

        internal Size Bounds_Normal_Size // 获取或设置普通状态窗口的大小。
        {
            get
            {
                return new Size(_Bounds_Normal_Width, _Bounds_Normal_Height);
            }

            set
            {
                _Bounds_Normal_Width = Math.Max(MinimumBoundsWidth, Math.Min(value.Width, MaximumBoundsWidth));
                _Bounds_Normal_Height = Math.Max(MinimumBoundsHeight, Math.Min(value.Height, MaximumBoundsHeight));
            }
        }

        internal Rectangle Bounds_Normal // 获取或设置普通状态窗口的位置与大小。
        {
            get
            {
                return new Rectangle(_Bounds_Normal_X, _Bounds_Normal_Y, _Bounds_Normal_Width, _Bounds_Normal_Height);
            }

            set
            {
                _Bounds_Normal_X = value.X;
                _Bounds_Normal_Y = value.Y;
                _Bounds_Normal_Width = Math.Max(MinimumBoundsWidth, Math.Min(value.Width, MaximumBoundsWidth));
                _Bounds_Normal_Height = Math.Max(MinimumBoundsHeight, Math.Min(value.Height, MaximumBoundsHeight));
            }
        }

        private int _Bounds_QuarterScreen_X = 0; // 占据桌面四分之一状态窗口在桌面的左边距。
        private int _Bounds_QuarterScreen_Y = 0; // 占据桌面四分之一状态窗口在桌面的上边距。
        private int _Bounds_QuarterScreen_Width = 1; // 占据桌面四分之一状态窗口的宽度。
        private int _Bounds_QuarterScreen_Height = 1; // 占据桌面四分之一状态窗口的高度。

        internal int Bounds_QuarterScreen_X // 获取或设置占据桌面四分之一状态窗口在桌面的左边距。
        {
            get
            {
                return _Bounds_QuarterScreen_X;
            }

            set
            {
                _Bounds_QuarterScreen_X = value;
            }
        }

        internal int Bounds_QuarterScreen_Y // 获取或设置占据桌面四分之一状态窗口在桌面的上边距。
        {
            get
            {
                return _Bounds_QuarterScreen_Y;
            }

            set
            {
                _Bounds_QuarterScreen_Y = value;
            }
        }

        internal int Bounds_QuarterScreen_Right // 获取占据桌面四分之一状态窗口在桌面的右边距。
        {
            get
            {
                return (_Bounds_QuarterScreen_X + _Bounds_QuarterScreen_Width);
            }
        }

        internal int Bounds_QuarterScreen_Bottom // 获取占据桌面四分之一状态窗口在桌面的下边距。
        {
            get
            {
                return (_Bounds_QuarterScreen_Y + _Bounds_QuarterScreen_Height);
            }
        }

        internal int Bounds_QuarterScreen_Width // 获取或设置占据桌面四分之一状态窗口的宽度。
        {
            get
            {
                return _Bounds_QuarterScreen_Width;
            }

            set
            {
                _Bounds_QuarterScreen_Width = Math.Max(MinimumBoundsWidth, Math.Min(value, MaximumBoundsWidth));
            }
        }

        internal int Bounds_QuarterScreen_Height // 获取或设置占据桌面四分之一状态窗口的高度。
        {
            get
            {
                return _Bounds_QuarterScreen_Height;
            }

            set
            {
                _Bounds_QuarterScreen_Height = Math.Max(MinimumBoundsHeight, Math.Min(value, MaximumBoundsHeight));
            }
        }

        internal Point Bounds_QuarterScreen_Location // 获取或设置占据桌面四分之一状态窗口在桌面的位置。
        {
            get
            {
                return new Point(_Bounds_QuarterScreen_X, _Bounds_QuarterScreen_Y);
            }

            set
            {
                _Bounds_QuarterScreen_X = value.X;
                _Bounds_QuarterScreen_Y = value.Y;
            }
        }

        internal Size Bounds_QuarterScreen_Size // 获取或设置占据桌面四分之一状态窗口的大小。
        {
            get
            {
                return new Size(_Bounds_QuarterScreen_Width, _Bounds_QuarterScreen_Height);
            }

            set
            {
                _Bounds_QuarterScreen_Width = Math.Max(MinimumBoundsWidth, Math.Min(value.Width, MaximumBoundsWidth));
                _Bounds_QuarterScreen_Height = Math.Max(MinimumBoundsHeight, Math.Min(value.Height, MaximumBoundsHeight));
            }
        }

        internal Rectangle Bounds_QuarterScreen // 获取或设置占据桌面四分之一状态窗口的位置与大小。
        {
            get
            {
                return new Rectangle(_Bounds_QuarterScreen_X, _Bounds_QuarterScreen_Y, _Bounds_QuarterScreen_Width, _Bounds_QuarterScreen_Height);
            }

            set
            {
                _Bounds_QuarterScreen_X = value.X;
                _Bounds_QuarterScreen_Y = value.Y;
                _Bounds_QuarterScreen_Width = Math.Max(MinimumBoundsWidth, Math.Min(value.Width, MaximumBoundsWidth));
                _Bounds_QuarterScreen_Height = Math.Max(MinimumBoundsHeight, Math.Min(value.Height, MaximumBoundsHeight));
            }
        }

        private int _Bounds_BeforeFullScreen_X = 0; // 进入全屏幕状态前窗口在桌面的左边距。
        private int _Bounds_BeforeFullScreen_Y = 0; // 进入全屏幕状态前窗口在桌面的上边距。
        private int _Bounds_BeforeFullScreen_Width = 1; // 进入全屏幕状态前窗口的宽度。
        private int _Bounds_BeforeFullScreen_Height = 1; // 进入全屏幕状态前窗口的高度。

        internal int Bounds_BeforeFullScreen_X // 获取或设置进入全屏幕状态前窗口在桌面的左边距。
        {
            get
            {
                return _Bounds_BeforeFullScreen_X;
            }

            set
            {
                _Bounds_BeforeFullScreen_X = value;
            }
        }

        internal int Bounds_BeforeFullScreen_Y // 获取或设置进入全屏幕状态前窗口在桌面的上边距。
        {
            get
            {
                return _Bounds_BeforeFullScreen_Y;
            }

            set
            {
                _Bounds_BeforeFullScreen_Y = value;
            }
        }

        internal int Bounds_BeforeFullScreen_Right // 获取进入全屏幕状态前窗口在桌面的右边距。
        {
            get
            {
                return (_Bounds_BeforeFullScreen_X + _Bounds_BeforeFullScreen_Width);
            }
        }

        internal int Bounds_BeforeFullScreen_Bottom // 获取进入全屏幕状态前窗口在桌面的下边距。
        {
            get
            {
                return (_Bounds_BeforeFullScreen_Y + _Bounds_BeforeFullScreen_Height);
            }
        }

        internal int Bounds_BeforeFullScreen_Width // 获取或设置进入全屏幕状态前窗口的宽度。
        {
            get
            {
                return _Bounds_BeforeFullScreen_Width;
            }

            set
            {
                _Bounds_BeforeFullScreen_Width = Math.Max(MinimumBoundsWidth, Math.Min(value, MaximumBoundsWidth));
            }
        }

        internal int Bounds_BeforeFullScreen_Height // 获取或设置进入全屏幕状态前窗口的高度。
        {
            get
            {
                return _Bounds_BeforeFullScreen_Height;
            }

            set
            {
                _Bounds_BeforeFullScreen_Height = Math.Max(MinimumBoundsHeight, Math.Min(value, MaximumBoundsHeight));
            }
        }

        internal Point Bounds_BeforeFullScreen_Location // 获取或设置进入全屏幕状态前窗口在桌面的位置。
        {
            get
            {
                return new Point(_Bounds_BeforeFullScreen_X, _Bounds_BeforeFullScreen_Y);
            }

            set
            {
                _Bounds_BeforeFullScreen_X = value.X;
                _Bounds_BeforeFullScreen_Y = value.Y;
            }
        }

        internal Size Bounds_BeforeFullScreen_Size // 获取或设置进入全屏幕状态前窗口的大小。
        {
            get
            {
                return new Size(_Bounds_BeforeFullScreen_Width, _Bounds_BeforeFullScreen_Height);
            }

            set
            {
                _Bounds_BeforeFullScreen_Width = Math.Max(MinimumBoundsWidth, Math.Min(value.Width, MaximumBoundsWidth));
                _Bounds_BeforeFullScreen_Height = Math.Max(MinimumBoundsHeight, Math.Min(value.Height, MaximumBoundsHeight));
            }
        }

        internal Rectangle Bounds_BeforeFullScreen // 获取或设置进入全屏幕状态前窗口的位置与大小。
        {
            get
            {
                return new Rectangle(_Bounds_BeforeFullScreen_X, _Bounds_BeforeFullScreen_Y, _Bounds_BeforeFullScreen_Width, _Bounds_BeforeFullScreen_Height);
            }

            set
            {
                _Bounds_BeforeFullScreen_X = value.X;
                _Bounds_BeforeFullScreen_Y = value.Y;
                _Bounds_BeforeFullScreen_Width = Math.Max(MinimumBoundsWidth, Math.Min(value.Width, MaximumBoundsWidth));
                _Bounds_BeforeFullScreen_Height = Math.Max(MinimumBoundsHeight, Math.Min(value.Height, MaximumBoundsHeight));
            }
        }

        //

        private void _UpdateLayout(UpdateLayoutEventType updateLayoutEventType) // 更新窗口布局。
        {
            _Client.Bounds = _SplashScreen.Bounds = new Rectangle(new Point(Bounds_Current_X, Bounds_Current_Y + _CaptionBarHeight), new Size(Bounds_Current_Width, Bounds_Current_Height - _CaptionBarHeight));

            if (_FormState != FormState.FullScreen)
            {
                _CaptionBar.Bounds = new Rectangle(Bounds_Current_Location, new Size(Bounds_Current_Width, _CaptionBarHeight));
            }

            _Resizer.Bounds = new Rectangle(new Point(Bounds_Current_X - _ResizerSize, Bounds_Current_Y - _ResizerSize), new Size(Bounds_Current_Width + 2 * _ResizerSize, Bounds_Current_Height + 2 * _ResizerSize));

            if (updateLayoutEventType == UpdateLayoutEventType.Manual)
            {
                _OnMove();
                _OnResize();
            }
            else if (updateLayoutEventType == UpdateLayoutEventType.All)
            {
                _OnLocationChanged();
                _OnSizeChanged();
            }
        }

        internal void UpdateLayout(UpdateLayoutEventType updateLayoutEventType) // 更新窗口布局。
        {
            _UpdateLayout(updateLayoutEventType);
        }

        private void _SetBoundsAndUpdateLayout(Rectangle bounds, UpdateLayoutBehavior updateLayoutBehavior, UpdateLayoutEventType updateLayoutEventType) // 设置窗口的位置与大小，并更新窗口布局。
        {
            if (updateLayoutBehavior == UpdateLayoutBehavior.None)
            {
                Bounds_Current = bounds;
            }
            else if (updateLayoutBehavior == UpdateLayoutBehavior.Static)
            {
                Bounds_Current = bounds;

                _UpdateLayout(updateLayoutEventType);
            }
            else if (updateLayoutBehavior == UpdateLayoutBehavior.Animate)
            {
                int MaxLTRB = Math.Max(1, Math.Max(Math.Abs(bounds.Left - Bounds_Current.Left), Math.Max(Math.Abs(bounds.Top - Bounds_Current.Top), Math.Max(Math.Abs(bounds.Right - Bounds_Current.Right), Math.Abs(bounds.Bottom - Bounds_Current.Bottom)))));
                int Delta = Math.Min(MaxLTRB, (int)(new PointD(PrimaryScreenBounds.Size).VectorModule / 40.96));

                Rectangle oldBounds = Rectangle.FromLTRB(bounds.Left - (bounds.Left - Bounds_Current.Left) * Delta / MaxLTRB, bounds.Top - (bounds.Top - Bounds_Current.Top) * Delta / MaxLTRB, bounds.Right - (bounds.Right - Bounds_Current.Right) * Delta / MaxLTRB, bounds.Bottom - (bounds.Bottom - Bounds_Current.Bottom) * Delta / MaxLTRB);

                Animation.Frame Frame = (frameId, frameCount, msPerFrame) =>
                {
                    double Pct_F = (frameId == frameCount ? 1 : 1 - Math.Pow(1 - (double)frameId / frameCount, 2));

                    Bounds_Current = new Rectangle((new PointD(oldBounds.Location) * (1 - Pct_F) + new PointD(bounds.Location) * Pct_F).ToPoint(), (new PointD(oldBounds.Size) * (1 - Pct_F) + new PointD(bounds.Size) * Pct_F).ToSize());

                    if (frameId == frameCount)
                    {
                        _UpdateLayout(updateLayoutEventType);
                    }
                    else
                    {
                        _UpdateLayout(UpdateLayoutEventType.None);
                    }
                };

                Animation.Show(Frame, 6, 15);
            }
        }

        //

        private Predicate<EventArgs> _ReturnVerification = null; // 用于验证是否允许窗口还原的方法。
        private Predicate<EventArgs> _MaximizeVerification = null; // 设置用于验证是否允许窗口最大化的方法。
        private Predicate<EventArgs> _EnterFullScreenVerification = null; // 用于验证是否允许窗口进入全屏幕模式的方法。
        private Predicate<EventArgs> _ExitFullScreenVerification = null; // 用于验证是否允许窗口退出全屏幕模式的方法。
        private Predicate<EventArgs> _CloseVerification = null; // 设置用于验证是否允许窗口关闭的方法。

        private bool _CanReturn() // 判断是否允许窗口还原。
        {
            return (_Initialized && (_FormState != FormState.FullScreen && _FormState != FormState.Normal) && (_ReturnVerification == null || (_ReturnVerification != null && _ReturnVerification(EventArgs.Empty))));
        }

        internal bool CanReturn() // 判断是否允许窗口还原。
        {
            return _CanReturn();
        }

        private bool _CanMinimize() // 判断是否允许窗口最小化。
        {
            return (_Initialized && _FormStyle != FormStyle.Dialog && _Client.WindowState != FormWindowState.Minimized);
        }

        private bool _CanMaximize() // 判断是否允许窗口最大化。
        {
            return (_Initialized && _FormStyle == FormStyle.Sizable && _Client.WindowState != FormWindowState.Minimized && (_FormState != FormState.FullScreen && _FormState != FormState.Maximized) && (_MaximizeVerification == null || (_MaximizeVerification != null && _MaximizeVerification(EventArgs.Empty))));
        }

        private bool _CanEnterFullScreen() // 判断是否允许窗口进入全屏幕模式。
        {
            return (_Initialized && _EnableFullScreen && _FormState != FormState.FullScreen && _Client.WindowState != FormWindowState.Minimized && (_EnterFullScreenVerification == null || (_EnterFullScreenVerification != null && _EnterFullScreenVerification(EventArgs.Empty))));
        }

        private bool _CanExitFullScreen() // 判断是否允许窗口退出全屏幕模式。
        {
            return (_Initialized && _Client.WindowState != FormWindowState.Minimized && _FormState == FormState.FullScreen && (_ExitFullScreenVerification == null || (_ExitFullScreenVerification != null && _ExitFullScreenVerification(EventArgs.Empty))));
        }

        private bool _CanHighAsScreen() // 判断是否允许窗口与桌面的高度相同。
        {
            return (_Initialized && _FormStyle == FormStyle.Sizable && _Client.WindowState != FormWindowState.Minimized && _FormState != FormState.FullScreen);
        }

        private bool _CanQuarterScreen() // 判断是否允许窗口占据桌面的四分之一区域。
        {
            return (_Initialized && _FormStyle == FormStyle.Sizable && _Client.WindowState != FormWindowState.Minimized && _FormState != FormState.FullScreen);
        }

        private bool _CanClose() // 判断是否允许窗口关闭。
        {
            return (_Initialized && (_CloseVerification == null || (_CloseVerification != null && _CloseVerification(EventArgs.Empty))));
        }

        //

        private EventHandlerList _Events = new EventHandlerList(); // 窗口事件委托列表。

        private void _TrigEvent(object eventKey) // 引发窗口事件。
        {
            EventHandler Action = _Events[eventKey] as EventHandler;

            if (Action != null)
            {
                Action(this, EventArgs.Empty);
            }
        }

        private void _OnLoading() // 引发 Loading 事件。
        {
            _TrigEvent(EventKey.Loading);
        }

        private void _OnLoaded() // 引发 Loaded 事件。
        {
            _TrigEvent(EventKey.Loaded);
        }

        private void _OnClosing() // 引发 Closing 事件。
        {
            _TrigEvent(EventKey.Closing);
        }

        private void _OnClosed() // 引发 Closed 事件。
        {
            _TrigEvent(EventKey.Closed);
        }

        private void _OnMove() // 引发 Move 事件。
        {
            _TrigEvent(EventKey.Move);
        }

        private void _OnLocationChanged() // 引发 LocationChanged 事件。
        {
            _OnMove();

            _TrigEvent(EventKey.LocationChanged);
        }

        private void _OnResize() // 引发 Resize 事件。
        {
            _TrigEvent(EventKey.Resize);
        }

        private void _OnSizeChanged() // 引发 SizeChanged 事件。
        {
            _OnResize();

            _TrigEvent(EventKey.SizeChanged);
        }

        private void _OnFormStyleChanged() // 引发 FormStyleChanged 事件。
        {
            _TrigEvent(EventKey.FormStyleChanged);
        }

        private void _OnFormStateChanged() // 引发 FormStateChanged 事件。
        {
            _TrigEvent(EventKey.FormStateChanged);
        }

        private void _OnEnabledChanged() // 引发 EnabledChanged 事件。
        {
            _TrigEvent(EventKey.EnabledChanged);
        }

        private void _OnOpacityChanged() // 引发 OpacityChanged 事件。
        {
            _TrigEvent(EventKey.OpacityChanged);
        }

        private void _OnCaptionChanged() // 引发 CaptionChanged 事件。
        {
            _TrigEvent(EventKey.CaptionChanged);
        }

        private void _OnThemeChanged() // 引发 ThemeChanged 事件。
        {
            _TrigEvent(EventKey.ThemeChanged);
        }

        private void _OnThemeColorChanged() // 引发 ThemeColorChanged 事件。
        {
            _TrigEvent(EventKey.ThemeColorChanged);
        }

        //

        private void _Return(UpdateLayoutBehavior updateLayoutBehavior, UpdateLayoutEventType updateLayoutEventType) // 使窗口还原至普通大小。
        {
            _FormState = FormState.Normal;

            Rectangle OldBounds = Bounds_Current;
            Bounds_Current = Bounds_Normal;

            _CaptionBar.OnFormStateChanged();
            _Resizer.OnFormStateChanged();

            Rectangle NewBounds = Bounds_Current;
            Bounds_Current = OldBounds;

            _SetBoundsAndUpdateLayout(NewBounds, updateLayoutBehavior, updateLayoutEventType);

            if (updateLayoutEventType != UpdateLayoutEventType.None)
            {
                _OnFormStateChanged();
            }
        }

        internal void ReturnByMoveForm() // 通过移动最大化、与桌面高度相同或占据桌面四分之一的窗口，使窗口还原至普通大小。
        {
            _FormState = FormState.Normal;

            Rectangle OldBounds = Bounds_Current;
            Bounds_Current = Bounds_Normal;

            _CaptionBar.OnFormStateChanged();
            _Resizer.OnFormStateChanged();

            Rectangle NewBounds = Bounds_Current;
            Bounds_Current = OldBounds;

            _SetBoundsAndUpdateLayout(NewBounds, UpdateLayoutBehavior.None, UpdateLayoutEventType.None);

            _OnFormStateChanged();
        }

        internal void ReturnFromHighAsScreen() // 使窗口由与桌面高度相同状态还原至普通大小。
        {
            _FormState = FormState.Normal;

            Rectangle OldBounds = Bounds_Current;
            Bounds_Current_Y = Bounds_Normal_Y;
            Bounds_Current_Height = Bounds_Normal_Height;

            _CaptionBar.OnFormStateChanged();
            _Resizer.OnFormStateChanged();

            Rectangle NewBounds = Bounds_Current;
            Bounds_Current = OldBounds;

            _SetBoundsAndUpdateLayout(NewBounds, UpdateLayoutBehavior.Static, UpdateLayoutEventType.All);

            _OnFormStateChanged();
        }

        private void _Minimize() // 使窗口最小化至任务栏。
        {
            _Client.WindowState = FormWindowState.Minimized;
        }

        private void _Maximize(UpdateLayoutBehavior updateLayoutBehavior, UpdateLayoutEventType updateLayoutEventType) // 使窗口最大化。
        {
            _FormState = FormState.Maximized;

            Rectangle OldBounds = Bounds_Current;
            Bounds_Current = PrimaryScreenClient;

            _CaptionBar.OnFormStateChanged();
            _Resizer.OnFormStateChanged();

            Rectangle NewBounds = Bounds_Current;
            Bounds_Current = OldBounds;

            _SetBoundsAndUpdateLayout(NewBounds, updateLayoutBehavior, updateLayoutEventType);

            if (updateLayoutEventType != UpdateLayoutEventType.None)
            {
                _OnFormStateChanged();
            }
        }

        private void _EnterFullScreen(UpdateLayoutBehavior updateLayoutBehavior, UpdateLayoutEventType updateLayoutEventType) // 使窗口进入全屏幕模式。
        {
            _FormState_BeforeFullScreen = _FormState;
            Bounds_BeforeFullScreen = Bounds_Current;

            _FormState = FormState.FullScreen;

            Rectangle OldBounds = Bounds_Current;
            Bounds_Current = PrimaryScreenBounds;

            _CaptionBar.OnFormStateChanged();
            _Resizer.OnFormStateChanged();

            Rectangle NewBounds = Bounds_Current;
            Bounds_Current = OldBounds;

            _SetBoundsAndUpdateLayout(NewBounds, updateLayoutBehavior, updateLayoutEventType);

            _Client.WindowState = _SplashScreen.WindowState = FormWindowState.Maximized;

            if (updateLayoutEventType != UpdateLayoutEventType.None)
            {
                _OnFormStateChanged();
            }
        }

        private void _ExitFullScreen(UpdateLayoutBehavior updateLayoutBehavior, UpdateLayoutEventType updateLayoutEventType) // 使窗口退出全屏幕模式。
        {
            if (_FormStyle != FormStyle.Sizable)
            {
                _FormState_BeforeFullScreen = FormState.Normal;
                Bounds_BeforeFullScreen = Bounds_Normal;
            }

            _FormState = _FormState_BeforeFullScreen;

            _Client.WindowState = _SplashScreen.WindowState = FormWindowState.Normal;

            Rectangle OldBounds = Bounds_Current;
            Bounds_Current = Bounds_BeforeFullScreen;

            _CaptionBar.OnFormStateChanged();
            _Resizer.OnFormStateChanged();

            Rectangle NewBounds = Bounds_Current;
            Bounds_Current = OldBounds;

            if (_Client.WindowState == FormWindowState.Maximized)
            {
                _Client.WindowState = FormWindowState.Minimized;
            }
            else
            {
                _SetBoundsAndUpdateLayout(NewBounds, updateLayoutBehavior, updateLayoutEventType);
            }

            if (updateLayoutEventType != UpdateLayoutEventType.None)
            {
                _OnFormStateChanged();
            }
        }

        private void _LeftHalfScreen(UpdateLayoutBehavior updateLayoutBehavior, UpdateLayoutEventType updateLayoutEventType) // 使窗口占据桌面的左半区域。
        {
            _FormState = FormState.HighAsScreen;

            Rectangle OldBounds = Bounds_Current;
            Bounds_Current_Size = new Size(Math.Max(MinimumBoundsWidth, Math.Min(MaximumBoundsWidth, PrimaryScreenClient.Width / 2)), Math.Max(MinimumBoundsHeight, Math.Min(MaximumBoundsHeight, PrimaryScreenClient.Height)));
            Bounds_Current_Location = PrimaryScreenClient.Location;

            _CaptionBar.OnFormStateChanged();
            _Resizer.OnFormStateChanged();

            Rectangle NewBounds = Bounds_Current;
            Bounds_Current = OldBounds;

            _SetBoundsAndUpdateLayout(NewBounds, updateLayoutBehavior, updateLayoutEventType);

            if (updateLayoutEventType != UpdateLayoutEventType.None)
            {
                _OnFormStateChanged();
            }
        }

        private void _RightHalfScreen(UpdateLayoutBehavior updateLayoutBehavior, UpdateLayoutEventType updateLayoutEventType) // 使窗口占据桌面的右半区域。
        {
            _FormState = FormState.HighAsScreen;

            Rectangle OldBounds = Bounds_Current;
            Bounds_Current_Size = new Size(Math.Max(MinimumBoundsWidth, Math.Min(MaximumBoundsWidth, PrimaryScreenClient.Width / 2)), Math.Max(MinimumBoundsHeight, Math.Min(MaximumBoundsHeight, PrimaryScreenClient.Height)));
            Bounds_Current_Location = new Point(PrimaryScreenClient.Right - Bounds_Current_Width, PrimaryScreenClient.Y);

            _CaptionBar.OnFormStateChanged();
            _Resizer.OnFormStateChanged();

            Rectangle NewBounds = Bounds_Current;
            Bounds_Current = OldBounds;

            _SetBoundsAndUpdateLayout(NewBounds, updateLayoutBehavior, updateLayoutEventType);

            if (updateLayoutEventType != UpdateLayoutEventType.None)
            {
                _OnFormStateChanged();
            }
        }

        private void _HighAsScreen(UpdateLayoutBehavior updateLayoutBehavior, UpdateLayoutEventType updateLayoutEventType) // 使窗口与桌面的高度相同。
        {
            _FormState = FormState.HighAsScreen;

            Rectangle OldBounds = Bounds_Current;
            Bounds_Current_Height = Math.Max(MinimumBoundsHeight, Math.Min(MaximumBoundsHeight, PrimaryScreenClient.Height));
            Bounds_Current_Y = PrimaryScreenClient.Y;

            _CaptionBar.OnFormStateChanged();
            _Resizer.OnFormStateChanged();

            Rectangle NewBounds = Bounds_Current;
            Bounds_Current = OldBounds;

            _SetBoundsAndUpdateLayout(NewBounds, updateLayoutBehavior, updateLayoutEventType);

            if (updateLayoutEventType != UpdateLayoutEventType.None)
            {
                _OnFormStateChanged();
            }
        }

        private void _TopLeftQuarterScreen(UpdateLayoutBehavior updateLayoutBehavior, UpdateLayoutEventType updateLayoutEventType) // 使窗口占据桌面的左上四分之一区域。
        {
            _FormState = FormState.QuarterScreen;

            Rectangle OldBounds = Bounds_Current;
            Bounds_Current_Size = Bounds_QuarterScreen_Size = new Size(Math.Max(MinimumBoundsWidth, Math.Min(MaximumBoundsWidth, PrimaryScreenClient.Width / 2)), Math.Max(MinimumBoundsHeight, Math.Min(MaximumBoundsHeight, PrimaryScreenClient.Height / 2)));
            Bounds_Current_Location = Bounds_QuarterScreen_Location = PrimaryScreenClient.Location;

            _CaptionBar.OnFormStateChanged();
            _Resizer.OnFormStateChanged();

            Rectangle NewBounds = Bounds_Current;
            Bounds_Current = OldBounds;

            _SetBoundsAndUpdateLayout(NewBounds, updateLayoutBehavior, updateLayoutEventType);

            if (updateLayoutEventType != UpdateLayoutEventType.None)
            {
                _OnFormStateChanged();
            }
        }

        private void _TopRightQuarterScreen(UpdateLayoutBehavior updateLayoutBehavior, UpdateLayoutEventType updateLayoutEventType) // 使窗口占据桌面的右上四分之一区域。
        {
            _FormState = FormState.QuarterScreen;

            Rectangle OldBounds = Bounds_Current;
            Bounds_Current_Size = Bounds_QuarterScreen_Size = new Size(Math.Max(MinimumBoundsWidth, Math.Min(MaximumBoundsWidth, PrimaryScreenClient.Width / 2)), Math.Max(MinimumBoundsHeight, Math.Min(MaximumBoundsHeight, PrimaryScreenClient.Height / 2)));
            Bounds_Current_Location = Bounds_QuarterScreen_Location = new Point(PrimaryScreenClient.Right - Bounds_Current_Width, PrimaryScreenClient.Y);

            _CaptionBar.OnFormStateChanged();
            _Resizer.OnFormStateChanged();

            Rectangle NewBounds = Bounds_Current;
            Bounds_Current = OldBounds;

            _SetBoundsAndUpdateLayout(NewBounds, updateLayoutBehavior, updateLayoutEventType);

            if (updateLayoutEventType != UpdateLayoutEventType.None)
            {
                _OnFormStateChanged();
            }
        }

        private void _BottomLeftQuarterScreen(UpdateLayoutBehavior updateLayoutBehavior, UpdateLayoutEventType updateLayoutEventType) // 使窗口占据桌面的左下四分之一区域。
        {
            _FormState = FormState.QuarterScreen;

            Rectangle OldBounds = Bounds_Current;
            Bounds_Current_Size = Bounds_QuarterScreen_Size = new Size(Math.Max(MinimumBoundsWidth, Math.Min(MaximumBoundsWidth, PrimaryScreenClient.Width / 2)), Math.Max(MinimumBoundsHeight, Math.Min(MaximumBoundsHeight, PrimaryScreenClient.Height / 2)));
            Bounds_Current_Location = Bounds_QuarterScreen_Location = new Point(PrimaryScreenClient.X, PrimaryScreenClient.Bottom - Bounds_Current_Height);

            _CaptionBar.OnFormStateChanged();
            _Resizer.OnFormStateChanged();

            Rectangle NewBounds = Bounds_Current;
            Bounds_Current = OldBounds;

            _SetBoundsAndUpdateLayout(NewBounds, updateLayoutBehavior, updateLayoutEventType);

            if (updateLayoutEventType != UpdateLayoutEventType.None)
            {
                _OnFormStateChanged();
            }
        }

        private void _BottomRightQuarterScreen(UpdateLayoutBehavior updateLayoutBehavior, UpdateLayoutEventType updateLayoutEventType) // 使窗口占据桌面的右下四分之一区域。
        {
            _FormState = FormState.QuarterScreen;

            Rectangle OldBounds = Bounds_Current;
            Bounds_Current_Size = Bounds_QuarterScreen_Size = new Size(Math.Max(MinimumBoundsWidth, Math.Min(MaximumBoundsWidth, PrimaryScreenClient.Width / 2)), Math.Max(MinimumBoundsHeight, Math.Min(MaximumBoundsHeight, PrimaryScreenClient.Height / 2)));
            Bounds_Current_Location = Bounds_QuarterScreen_Location = new Point(PrimaryScreenClient.Right - Bounds_Current_Width, PrimaryScreenClient.Bottom - Bounds_Current_Height);

            _CaptionBar.OnFormStateChanged();
            _Resizer.OnFormStateChanged();

            Rectangle NewBounds = Bounds_Current;
            Bounds_Current = OldBounds;

            _SetBoundsAndUpdateLayout(NewBounds, updateLayoutBehavior, updateLayoutEventType);

            if (updateLayoutEventType != UpdateLayoutEventType.None)
            {
                _OnFormStateChanged();
            }
        }

        private void _Close() // 关闭窗口。
        {
            _Client.Closed -= Client_Closed;
            _CaptionBar.Closed -= CaptionBar_Closed;
            _Resizer.Closed -= Resizer_Closed;
            _SplashScreen.Closed -= SplashScreen_Closed;

            //

            if (!_LoadingNow && !_ClosingNow)
            {
                _ClosingNow = true;

                //

                _CaptionBar.OnClosing();

                //

                _FormClosingAsyncWorker.RunWorkerAsync();
            }
            else
            {
                _SplashScreen.OnClosing();
                _SplashScreen.Visible = true;

                _Client.Visible = false;

                //

                _Client.ShowInTaskbar = false;

                //

                double Opacity = _Opacity;
                int Bounds_Current_Y = this.Bounds_Current_Y;

                Animation.Frame Frame = (frameId, frameCount, msPerFrame) =>
                {
                    double Pct_F = (frameId == frameCount ? 1 : 1 - Math.Pow(1 - (double)frameId / frameCount, 2));

                    _Opacity = Opacity * (1 - Pct_F);

                    _SplashScreen.Opacity = _Opacity;
                    _CaptionBar.Opacity = (_EnableCaptionBarTransparent ? _Opacity * 0.9 : _Opacity);
                    _Resizer.OnOpacityChanged();

                    this.Bounds_Current_Y = Bounds_Current_Y - (int)(16 * Pct_F);

                    _UpdateLayout(UpdateLayoutEventType.None);
                };

                Animation.Show(Frame, 9, 15);

                //

                _Client.Close();
            }
        }

        private void _AltF4(object sender, EventArgs e) // 通过 Alt + F4 或其他非正常方式关闭窗口。
        {
            _Client.Closed -= Client_Closed;
            _CaptionBar.Closed -= CaptionBar_Closed;
            _Resizer.Closed -= Resizer_Closed;
            _SplashScreen.Closed -= SplashScreen_Closed;

            //

            if (!_LoadingNow && !_ClosingNow)
            {
                _ClosingNow = true;

                //

                _OnClosing();
                _OnClosed();
            }

            //

            if (!object.ReferenceEquals(sender, _Client))
            {
                _Client.Close();
            }
        }

        //

        private void _Ctor(Form client, FormManager owner) // 为以 Form 对象与 FormManager 对象为参数的构造函数提供实现。
        {
            if (client == null)
            {
                try
                {
                    throw new NullReferenceException();
                }
                finally
                {
                    Application.Exit();
                }
            }

            //

            if (_FormManagerList.Count <= 0)
            {
                _IsMainForm = true;
            }

            _FormManagerList.Add(this);

            //

            _Client = client;

            _CaptionBar = new CaptionBar(this);
            _Resizer = new Resizer(this);
            _SplashScreen = new SplashScreen(this);

            _CaptionBar.Owner = _Resizer.Owner = _SplashScreen.Owner = _Client;

            _CaptionBar.Icon = _Resizer.Icon = _SplashScreen.Icon = _Client.Icon;

            //

            if (owner != null && !object.ReferenceEquals(_Owner, owner))
            {
                _Owner = owner;
            }
            else
            {
                _Owner = null;
            }

            if (_Owner != null)
            {
                _Owner._AddOwned(this);

                //

                _Opacity = _Owner._Opacity;
                _CaptionFont = _Owner._CaptionFont;
                _CaptionAlign = _Owner._CaptionAlign;
                _Theme = _Owner._Theme;
                _ThemeColor = _Owner._ThemeColor;
                _ShowCaptionBarColor = _Owner._ShowCaptionBarColor;
                _EnableCaptionBarTransparent = _Owner._EnableCaptionBarTransparent;
            }

            //

            _Client.FormBorderStyle = FormBorderStyle.None;
            _Client.StartPosition = FormStartPosition.Manual;
            _Client.WindowState = FormWindowState.Normal;

            //

            _Client.Opacity = _CaptionBar.Opacity = _Resizer.Opacity = _SplashScreen.Opacity = 0;

            //

            _RecommendColors = new RecommendColors(_Theme, _ThemeColor, _ShowCaptionBarColor);

            //

            _FormLoadingAsyncWorker = new BackgroundWorker();
            _FormLoadingAsyncWorker.DoWork += FormLoadingAsyncWorker_DoWork;
            _FormLoadingAsyncWorker.RunWorkerCompleted += FormLoadingAsyncWorker_RunWorkerCompleted;

            _FormClosingAsyncWorker = new BackgroundWorker();
            _FormClosingAsyncWorker.DoWork += FormClosingAsyncWorker_DoWork;
            _FormClosingAsyncWorker.RunWorkerCompleted += FormClosingAsyncWorker_RunWorkerCompleted;

            //

            _Client.Load += Client_Load;
            _Client.Closed += Client_Closed;
            _Client.SizeChanged += Client_SizeChanged;

            _CaptionBar.Closed += CaptionBar_Closed;

            _Resizer.Closed += Resizer_Closed;

            _SplashScreen.Closed += SplashScreen_Closed;

            //

            _LoadingNow = true;
        }

        private void _Dtor() // 为析构函数提供实现。
        {
            _FormManagerList.Remove(this);

            //

            if (_Owner != null)
            {
                _Owner._RemoveOwned(this);
            }
        }

        #endregion

        #region 回调函数

        private void Client_Load(object sender, EventArgs e) // Client 的 Load 事件的回调函数。
        {
            _Client.Visible = false;

            //

            _CaptionBar.Show();
            _Resizer.Show();
            _SplashScreen.Show();

            //

            Size = Bounds_Current_Size;

            if (X == int.MinValue || Y == int.MinValue)
            {
                Rectangle Rect = (_Owner != null ? _Owner.Bounds : PrimaryScreenClient);

                if (X == int.MinValue && Y == int.MinValue)
                {
                    Location = new Point(Rect.X + (Rect.Width - Width) / 2, Rect.Y + (Rect.Height - Height) / 2);
                }
                else if (X == int.MinValue)
                {
                    X = Rect.X + (Rect.Width - Width) / 2;
                }
                else if (Y == int.MinValue)
                {
                    Y = Rect.Y + (Rect.Height - Height) / 2;
                }
            }

            Location = Bounds_Current_Location;

            _UpdateLayout(UpdateLayoutEventType.None);

            //

            if (string.IsNullOrEmpty(_Caption))
            {
                if (!string.IsNullOrEmpty(_Client.Text))
                {
                    _Caption = _Client.Text;
                }
                else if (_IsMainForm)
                {
                    _Caption = Application.ProductName;
                }
            }

            if (_Client.Text != _Caption)
            {
                _Client.Text = _Caption;
            }

            //

            _CaptionBar.OnFormStyleChanged();
            _Resizer.OnFormStyleChanged();

            _CaptionBar.OnCaptionChanged();
            _CaptionBar.OnThemeChanged();

            _SplashScreen.OnThemeChanged();

            //

            _Client.TopMost = _CaptionBar.TopMost = _Resizer.TopMost = _SplashScreen.TopMost = _TopMost;

            _Client.ShowInTaskbar = _ShowInTaskbar;

            //

            _Client.Enabled = _CaptionBar.Enabled = _Resizer.Enabled = _SplashScreen.Enabled = _Enabled;

            //

            _Client.BackColor = RecommendColors.FormBackground.ToColor();

            //

            _CaptionBar.BringToFront();
            _Client.BringToFront();
            _SplashScreen.BringToFront();
            _SplashScreen.Focus();

            //

            if (_FormStyle == FormStyle.Sizable && _FormState == FormState.Maximized)
            {
                _Maximize(UpdateLayoutBehavior.Static, UpdateLayoutEventType.None);
            }
            else if (_EnableFullScreen && _FormState == FormState.FullScreen)
            {
                _EnterFullScreen(UpdateLayoutBehavior.Static, UpdateLayoutEventType.None);
            }
            else if (_FormStyle != FormStyle.Sizable && (_FormState != FormState.Normal && _FormState != FormState.FullScreen))
            {
                _Return(UpdateLayoutBehavior.Static, UpdateLayoutEventType.None);
            }

            //

            _Resizer.Opacity = 1;
            _Resizer.OnOpacityChanged();

            //

            double Opacity = _Opacity;
            int Bounds_Current_Y = this.Bounds_Current_Y;

            Animation.Frame Frame = (frameId, frameCount, msPerFrame) =>
            {
                double Pct_F = (frameId == frameCount ? 1 : 1 - Math.Pow(1 - (double)frameId / frameCount, 2));

                _Opacity = Opacity * Pct_F;

                _SplashScreen.Opacity = _Opacity;
                _CaptionBar.Opacity = (_EnableCaptionBarTransparent ? _Opacity * 0.9 : _Opacity);
                _Resizer.OnOpacityChanged();

                this.Bounds_Current_Y = Bounds_Current_Y - (int)(16 * (1 - Pct_F));

                _UpdateLayout(UpdateLayoutEventType.None);
            };

            Animation.Show(Frame, 9, 15);

            //

            _CaptionBar.OnLoading();
            _SplashScreen.OnLoading();

            //

            _Initialized = true;

            //

            _FormLoadingAsyncWorker.RunWorkerAsync();
        }

        private void Client_Closed(object sender, EventArgs e) // Client 的 Closed 事件的回调函数。
        {
            _AltF4(sender, e);
        }

        private void Client_SizeChanged(object sender, EventArgs e) // Client 的 SizeChanged 事件的回调函数。
        {
            if (_FormStyle == FormStyle.Dialog && _Client.WindowState == FormWindowState.Minimized)
            {
                _Client.WindowState = FormWindowState.Normal;
            }

            //

            if (_FormState != FormState.FullScreen && _Client.WindowState == FormWindowState.Maximized)
            {
                _Client.WindowState = FormWindowState.Normal;
            }
            else if (_FormState == FormState.FullScreen && _Client.WindowState == FormWindowState.Normal)
            {
                _Client.WindowState = FormWindowState.Maximized;
            }

            //

            if (_LastFormWindowState != _Client.WindowState)
            {
                FormWindowState _FormWindowState = _Client.WindowState;

                if ((_LastFormWindowState != FormWindowState.Minimized && _FormWindowState == FormWindowState.Minimized) || (_LastFormWindowState == FormWindowState.Minimized && _FormWindowState != FormWindowState.Minimized))
                {
                    _OnFormStateChanged();
                }

                _LastFormWindowState = _FormWindowState;
            }
        }

        private void CaptionBar_Closed(object sender, EventArgs e) // CaptionBar 的 Closed 事件的回调函数。
        {
            _AltF4(sender, e);
        }

        private void Resizer_Closed(object sender, EventArgs e) // Resizer 的 Closed 事件的回调函数。
        {
            _AltF4(sender, e);
        }

        private void SplashScreen_Closed(object sender, EventArgs e) // SplashScreen 的 Closed 事件的回调函数。
        {
            _AltF4(sender, e);
        }

        //

        private void FormLoadingAsyncWorker_DoWork(object sender, DoWorkEventArgs e) // FormLoadingAsyncWorker 的 DoWork 事件的回调函数。
        {
            _OnLoading();
        }

        private void FormLoadingAsyncWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) // FormLoadingAsyncWorker 的 RunWorkerCompleted 事件的回调函数。
        {
            _Client.Visible = true;
            _Client.Opacity = _Opacity;

            _SplashScreen.Visible = false;

            //

            _OnLoaded();

            //

            _LoadingNow = false;
        }

        private void FormClosingAsyncWorker_DoWork(object sender, DoWorkEventArgs e) // FormClosingAsyncWorker 的 DoWork 事件的回调函数。
        {
            if (!_FormLoadingAsyncWorker.IsBusy)
            {
                _OnClosing();
            }
        }

        private void FormClosingAsyncWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) // FormClosingAsyncWorker 的 RunWorkerCompleted 事件的回调函数。
        {
            _SplashScreen.OnClosing();
            _SplashScreen.Visible = true;

            _Client.Visible = false;

            //

            _Client.ShowInTaskbar = false;

            //

            double Opacity = _Opacity;
            int Bounds_Current_Y = this.Bounds_Current_Y;

            Animation.Frame Frame = (frameId, frameCount, msPerFrame) =>
            {
                double Pct_F = (frameId == frameCount ? 1 : 1 - Math.Pow(1 - (double)frameId / frameCount, 2));

                _Opacity = Opacity * (1 - Pct_F);

                _SplashScreen.Opacity = _Opacity;
                _CaptionBar.Opacity = (_EnableCaptionBarTransparent ? _Opacity * 0.9 : _Opacity);
                _Resizer.OnOpacityChanged();

                this.Bounds_Current_Y = Bounds_Current_Y - (int)(16 * Pct_F);

                _UpdateLayout(UpdateLayoutEventType.None);
            };

            Animation.Show(Frame, 9, 15);

            //

            _OnClosed();

            //

            _Client.Close();
        }

        #endregion

        #region 构造与析构函数

        /// <summary>
        /// 使用 Form 对象初始化 FormManager 的新实例。
        /// </summary>
        /// <param name="client">Form 对象，表示工作区。</param>
        public FormManager(Form client)
        {
            _Ctor(client, null);
        }

        /// <summary>
        /// 使用 Form 对象与 FormManager 对象初始化 FormManager 的新实例。
        /// </summary>
        /// <param name="client">Form 对象，表示工作区。</param>
        /// <param name="owner">FormManager 对象，表示拥有此窗口的窗口的窗口管理器。</param>
        public FormManager(Form client, FormManager owner)
        {
            _Ctor(client, owner);
        }

        /// <summary>
        /// 在垃圾回收将此 FormManager 对象回收前尝试释放资源并执行其他清理操作。
        /// </summary>
        ~FormManager()
        {
            _Dtor();
        }

        #endregion

        #region 属性

        /// <summary>
        /// 获取表示此窗口是否为主窗口的布尔值。
        /// </summary>
        public bool IsMainForm
        {
            get
            {
                return _IsMainForm;
            }
        }

        //

        /// <summary>
        /// 获取拥有此窗口的窗口的窗口管理器。
        /// </summary>
        public FormManager Owner
        {
            get
            {
                return _Owner;
            }
        }

        /// <summary>
        /// 获取表示此窗口拥有的所有窗口的窗口管理器列表。
        /// </summary>
        public List<FormManager> Owned
        {
            get
            {
                return _Owned;
            }
        }

        //

        /// <summary>
        /// 获取或设置窗口的样式。
        /// </summary>
        public FormStyle FormStyle
        {
            get
            {
                return _FormStyle;
            }

            set
            {
                _FormStyle = value;

                if (_Initialized)
                {
                    Action InvokeMethod = () =>
                    {
                        _CaptionBar.OnFormStyleChanged();
                        _Resizer.OnFormStyleChanged();

                        if (_FormStyle == FormStyle.Dialog && _Client.WindowState == FormWindowState.Minimized)
                        {
                            if (_FormState == FormState.FullScreen)
                            {
                                _Client.WindowState = FormWindowState.Maximized;
                            }
                            else
                            {
                                _Client.WindowState = FormWindowState.Normal;
                            }
                        }
                        else if (_FormStyle != FormStyle.Sizable && (_FormState != FormState.Normal && _FormState != FormState.FullScreen))
                        {
                            _Return(UpdateLayoutBehavior.Animate, UpdateLayoutEventType.All);
                        }

                        _OnFormStyleChanged();
                    };

                    _Client.Invoke(InvokeMethod);
                }
            }
        }

        /// <summary>
        /// 获取或设置表示是否允许窗口以全屏幕模式运行的布尔值。
        /// </summary>
        public bool EnableFullScreen
        {
            get
            {
                return _EnableFullScreen;
            }

            set
            {
                _EnableFullScreen = value;

                if (_Initialized)
                {
                    Action InvokeMethod = () =>
                    {
                        _CaptionBar.OnFormStyleChanged();

                        if (!_EnableFullScreen && _FormState == FormState.FullScreen)
                        {
                            _ExitFullScreen(UpdateLayoutBehavior.Animate, UpdateLayoutEventType.All);
                        }

                        _OnFormStyleChanged();
                    };

                    _Client.Invoke(InvokeMethod);
                }
            }
        }

        /// <summary>
        /// 获取或设置表示是否允许窗口置于顶层的布尔值。
        /// </summary>
        public bool TopMost
        {
            get
            {
                return _TopMost;
            }

            set
            {
                _TopMost = value;

                if (_Initialized)
                {
                    Action InvokeMethod = () =>
                    {
                        _Client.TopMost = _TopMost;

                        _OnFormStyleChanged();
                    };

                    _Client.Invoke(InvokeMethod);
                }
            }
        }

        /// <summary>
        /// 获取或设置表示窗口是否在任务栏显示的布尔值。
        /// </summary>
        public bool ShowInTaskbar
        {
            get
            {
                return _ShowInTaskbar;
            }

            set
            {
                _ShowInTaskbar = value;

                if (_Initialized)
                {
                    Action InvokeMethod = () =>
                    {
                        _Client.ShowInTaskbar = _ShowInTaskbar;

                        _Resizer.BringToFront();
                        _CaptionBar.BringToFront();
                        _Client.BringToFront();
                        _Client.Focus();

                        _OnFormStyleChanged();
                    };

                    _Client.Invoke(InvokeMethod);
                }
            }
        }

        //

        /// <summary>
        /// 获取或设置标题栏的高度。
        /// </summary>
        public int CaptionBarHeight
        {
            get
            {
                return _CaptionBarHeight;
            }

            set
            {
                _CaptionBarHeight = Math.Max(24, Math.Min(PrimaryScreenBounds.Height, value));

                if (_Initialized)
                {
                    Action InvokeMethod = () =>
                    {
                        _UpdateLayout(UpdateLayoutEventType.All);
                    };

                    _Client.Invoke(InvokeMethod);
                }
            }
        }

        //

        /// <summary>
        /// 获取或设置窗口的最小大小。
        /// </summary>
        public Size MinimumSize
        {
            get
            {
                return new Size(_MinimumWidth, _MinimumHeight);
            }

            set
            {
                if (!_Initialized)
                {
                    _MinimumWidth = Math.Max(0, value.Width);
                    _MinimumHeight = Math.Max(0, value.Height);
                }
            }
        }

        /// <summary>
        /// 获取或设置窗口的最大大小。
        /// </summary>
        public Size MaximumSize
        {
            get
            {
                return new Size(_MaximumWidth, _MaximumHeight);
            }

            set
            {
                if (!_Initialized)
                {
                    _MaximumWidth = Math.Max(0, value.Width);
                    _MaximumHeight = Math.Max(0, value.Height);
                }
            }
        }

        //

        /// <summary>
        /// 获取或设置窗口的状态。
        /// </summary>
        public FormState FormState
        {
            get
            {
                if (_Client.WindowState == FormWindowState.Minimized)
                {
                    return FormState.Minimized;
                }

                return _FormState;
            }

            set
            {
                Action InvokeMethod = () =>
                {
                    if (_Client.WindowState == FormWindowState.Minimized)
                    {
                        if (value == FormState.Normal || value == _FormState)
                        {
                            if (_Initialized)
                            {
                                Return();
                            }
                        }
                    }
                    else
                    {
                        if (_FormState == FormState.FullScreen && (value == FormState.Normal || value == _FormState_BeforeFullScreen))
                        {
                            if (_Initialized)
                            {
                                ExitFullScreen();
                            }
                        }
                        else if (_FormState != FormState.Normal && value == FormState.Normal)
                        {
                            if (_Initialized)
                            {
                                Return();
                            }
                            else
                            {
                                _FormState = FormState.Normal;
                            }
                        }
                        else if (value == FormState.Minimized)
                        {
                            if (_Initialized)
                            {
                                Minimize();
                            }
                        }
                        else if (_FormState != FormState.Maximized && value == FormState.Maximized)
                        {
                            if (_Initialized)
                            {
                                Maximize();
                            }
                            else if (_FormStyle == FormStyle.Sizable)
                            {
                                _FormState = FormState.Maximized;
                            }
                        }
                        else if (_FormState != FormState.FullScreen && value == FormState.FullScreen)
                        {
                            if (_Initialized)
                            {
                                EnterFullScreen();
                            }
                            else if (_EnableFullScreen)
                            {
                                _FormState = FormState.FullScreen;
                            }
                        }
                    }
                };

                _Client.Invoke(InvokeMethod);
            }
        }

        //

        /// <summary>
        /// 获取或设置表示窗口是否对用户交互作出响应的布尔值。
        /// </summary>
        public bool Enabled
        {
            get
            {
                return _Enabled;
            }

            set
            {
                _Enabled = value;

                if (_Initialized)
                {
                    Action InvokeMethod = () =>
                    {
                        _Client.Enabled = _CaptionBar.Enabled = _Resizer.Enabled = _SplashScreen.Enabled = _Enabled;

                        _OnEnabledChanged();
                    };

                    _Client.Invoke(InvokeMethod);
                }
            }
        }

        /// <summary>
        /// 获取或设置窗口的不透明度，取值范围为 [0, 1] 或 (1, 100]。
        /// </summary>
        public double Opacity
        {
            get
            {
                return _Opacity;
            }

            set
            {
                if (double.IsNaN(value) || double.IsInfinity(value))
                {
                    _Opacity = 1;
                }
                else
                {
                    _Opacity = Math.Max(0, Math.Min(value, 100));

                    if (_Opacity > 1)
                    {
                        _Opacity /= 100;
                    }
                }

                if (_Initialized)
                {
                    Action InvokeMethod = () =>
                    {
                        _Client.Opacity = _SplashScreen.Opacity = _Opacity;
                        _CaptionBar.Opacity = (_EnableCaptionBarTransparent ? _Opacity * 0.9 : _Opacity);

                        _Resizer.OnOpacityChanged();

                        _OnOpacityChanged();
                    };

                    _Client.Invoke(InvokeMethod);
                }
            }
        }

        /// <summary>
        /// 获取或设置窗口的标题。
        /// </summary>
        public string Caption
        {
            get
            {
                return _Caption;
            }

            set
            {
                _Caption = value;

                if (_Initialized)
                {
                    Action InvokeMethod = () =>
                    {
                        _Client.Text = _Caption;

                        _CaptionBar.OnCaptionChanged();

                        _OnCaptionChanged();
                    };

                    _Client.Invoke(InvokeMethod);
                }
            }
        }

        /// <summary>
        /// 获取或设置窗口标题的字体。
        /// </summary>
        public Font CaptionFont
        {
            get
            {
                return _CaptionFont;
            }

            set
            {
                _CaptionFont = value;

                if (_Initialized)
                {
                    Action InvokeMethod = () =>
                    {
                        _CaptionBar.OnCaptionChanged();
                    };

                    _Client.Invoke(InvokeMethod);
                }
            }
        }

        /// <summary>
        /// 获取或设置窗口标题的文本对齐方式。
        /// </summary>
        public ContentAlignment CaptionAlign
        {
            get
            {
                return _CaptionAlign;
            }

            set
            {
                _CaptionAlign = value;

                if (_Initialized)
                {
                    Action InvokeMethod = () =>
                    {
                        _CaptionBar.OnCaptionChanged();
                    };

                    _Client.Invoke(InvokeMethod);
                }
            }
        }

        /// <summary>
        /// 获取或设置标题栏的背景图像。
        /// </summary>
        public Bitmap CaptionBarBackgroundImage
        {
            get
            {
                return _CaptionBarBackgroundImage;
            }

            set
            {
                _CaptionBarBackgroundImage = value;

                if (_Initialized)
                {
                    Action InvokeMethod = () =>
                    {
                        _CaptionBar.OnCaptionChanged();
                    };

                    _Client.Invoke(InvokeMethod);
                }
            }
        }

        /// <summary>
        /// 获取或设置窗口的主题。
        /// </summary>
        public Theme Theme
        {
            get
            {
                return _Theme;
            }

            set
            {
                _Theme = value;

                _RecommendColors = new RecommendColors(_Theme, _ThemeColor, _ShowCaptionBarColor);

                if (_Initialized)
                {
                    Action InvokeMethod = () =>
                    {
                        _Client.BackColor = RecommendColors.FormBackground.ToColor();

                        _CaptionBar.OnThemeChanged();
                        _SplashScreen.OnThemeChanged();

                        _OnThemeChanged();
                    };

                    _Client.Invoke(InvokeMethod);
                }
            }
        }

        /// <summary>
        /// 获取或设置窗口的主题色。
        /// </summary>
        public ColorX ThemeColor
        {
            get
            {
                return _ThemeColor;
            }

            set
            {
                _ThemeColor = value;

                _RecommendColors = new RecommendColors(_Theme, _ThemeColor, _ShowCaptionBarColor);

                if (_Initialized)
                {
                    Action InvokeMethod = () =>
                    {
                        _Client.BackColor = RecommendColors.FormBackground.ToColor();

                        _CaptionBar.OnThemeColorChanged();
                        _SplashScreen.OnThemeColorChanged();

                        _OnThemeColorChanged();
                    };

                    _Client.Invoke(InvokeMethod);
                }
            }
        }

        /// <summary>
        /// 获取或设置表示是否在标题栏上显示主题色的布尔值。
        /// </summary>
        public bool ShowCaptionBarColor
        {
            get
            {
                return _ShowCaptionBarColor;
            }

            set
            {
                _ShowCaptionBarColor = value;

                _RecommendColors = new RecommendColors(_Theme, _ThemeColor, _ShowCaptionBarColor);

                if (_Initialized)
                {
                    Action InvokeMethod = () =>
                    {
                        _CaptionBar.OnThemeChanged();
                        _SplashScreen.OnThemeChanged();
                    };

                    _Client.Invoke(InvokeMethod);
                }
            }
        }

        /// <summary>
        /// 获取或设置表示是否允许以半透明方式显示标题栏的布尔值。
        /// </summary>
        public bool EnableCaptionBarTransparent
        {
            get
            {
                return _EnableCaptionBarTransparent;
            }

            set
            {
                _EnableCaptionBarTransparent = value;

                if (_Initialized)
                {
                    Action InvokeMethod = () =>
                    {
                        _CaptionBar.Opacity = (_EnableCaptionBarTransparent ? _Opacity * 0.9 : _Opacity);
                    };

                    _Client.Invoke(InvokeMethod);
                }
            }
        }

        /// <summary>
        /// 获取当前主题建议的颜色。
        /// </summary>
        public RecommendColors RecommendColors
        {
            get
            {
                return _RecommendColors;
            }
        }

        //

        /// <summary>
        /// 获取或设置窗口在桌面的左边距。
        /// </summary>
        public int X
        {
            get
            {
                return Bounds_Current_X;
            }

            set
            {
                int _X = Math.Max(PrimaryScreenBounds.X - Width + 1, Math.Min(value, PrimaryScreenBounds.Right - 1));

                switch (_FormState)
                {
                    case FormState.Normal:
                        Bounds_Current_X = Bounds_Normal_X = _X;
                        break;

                    case FormState.QuarterScreen:
                        Bounds_Current_X = Bounds_QuarterScreen_X = _X;
                        break;

                    case FormState.HighAsScreen:
                        Bounds_Current_X = Bounds_Normal_X = _X;
                        break;
                }

                if (_Initialized)
                {
                    Action InvokeMethod = () =>
                    {
                        _UpdateLayout(UpdateLayoutEventType.All);
                    };

                    _Client.Invoke(InvokeMethod);
                }
            }
        }

        /// <summary>
        /// 获取或设置窗口在桌面的上边距。
        /// </summary>
        public int Y
        {
            get
            {
                return Bounds_Current_Y;
            }

            set
            {
                int _Y = Math.Max(PrimaryScreenBounds.Y - Height + 1, Math.Min(value, PrimaryScreenBounds.Bottom - 1));

                switch (_FormState)
                {
                    case FormState.Normal:
                        Bounds_Current_Y = Bounds_Normal_Y = _Y;
                        break;

                    case FormState.QuarterScreen:
                        Bounds_Current_Y = Bounds_QuarterScreen_Y = _Y;
                        break;
                }

                if (_Initialized)
                {
                    Action InvokeMethod = () =>
                    {
                        _UpdateLayout(UpdateLayoutEventType.All);
                    };

                    _Client.Invoke(InvokeMethod);
                }
            }
        }

        /// <summary>
        /// 获取或设置窗口在桌面的左边距。
        /// </summary>
        public int Left
        {
            get
            {
                return X;
            }

            set
            {
                X = value;
            }
        }

        /// <summary>
        /// 获取窗口在桌面的右边距。
        /// </summary>
        public int Right
        {
            get
            {
                return (X + Width);
            }
        }

        /// <summary>
        /// 获取或设置窗口在桌面的上边距。
        /// </summary>
        public int Top
        {
            get
            {
                return Y;
            }

            set
            {
                Y = value;
            }
        }

        /// <summary>
        /// 获取窗口在桌面的下边距。
        /// </summary>
        public int Bottom
        {
            get
            {
                return (Y + Height);
            }
        }

        /// <summary>
        /// 获取或设置窗口在桌面的位置。
        /// </summary>
        public Point Location
        {
            get
            {
                return Bounds_Current_Location;
            }

            set
            {
                Point _Location = new Point(Math.Max(PrimaryScreenBounds.X - Width + 1, Math.Min(value.X, PrimaryScreenBounds.Right - 1)), Math.Max(PrimaryScreenBounds.Y - Height + 1, Math.Min(value.Y, PrimaryScreenBounds.Bottom - 1)));

                switch (_FormState)
                {
                    case FormState.Normal:
                        Bounds_Current_Location = Bounds_Normal_Location = _Location;
                        break;

                    case FormState.QuarterScreen:
                        Bounds_Current_Location = Bounds_QuarterScreen_Location = _Location;
                        break;

                    case FormState.HighAsScreen:
                        Bounds_Current_X = Bounds_Normal_X = _Location.X;
                        break;
                }

                if (_Initialized)
                {
                    Action InvokeMethod = () =>
                    {
                        _UpdateLayout(UpdateLayoutEventType.All);
                    };

                    _Client.Invoke(InvokeMethod);
                }
            }
        }

        /// <summary>
        /// 获取或设置窗口的宽度。
        /// </summary>
        public int Width
        {
            get
            {
                return Bounds_Current_Width;
            }

            set
            {
                switch (_FormState)
                {
                    case FormState.Normal:
                        Bounds_Current_Width = Bounds_Normal_Width = value;
                        break;

                    case FormState.QuarterScreen:
                        Bounds_Current_Width = Bounds_QuarterScreen_Width = value;
                        break;

                    case FormState.HighAsScreen:
                        Bounds_Current_Width = Bounds_Normal_Width = value;
                        break;
                }

                if (_Initialized)
                {
                    Action InvokeMethod = () =>
                    {
                        _UpdateLayout(UpdateLayoutEventType.All);
                    };

                    _Client.Invoke(InvokeMethod);
                }
            }
        }

        /// <summary>
        /// 获取或设置窗口的高度。
        /// </summary>
        public int Height
        {
            get
            {
                return Bounds_Current_Height;
            }

            set
            {
                switch (_FormState)
                {
                    case FormState.Normal:
                        Bounds_Current_Height = Bounds_Normal_Height = value;
                        break;

                    case FormState.QuarterScreen:
                        Bounds_Current_Height = Bounds_QuarterScreen_Height = value;
                        break;
                }

                if (_Initialized)
                {
                    Action InvokeMethod = () =>
                    {
                        _UpdateLayout(UpdateLayoutEventType.All);
                    };

                    _Client.Invoke(InvokeMethod);
                }
            }
        }

        /// <summary>
        /// 获取或设置窗口的大小。
        /// </summary>
        public Size Size
        {
            get
            {
                return Bounds_Current_Size;
            }

            set
            {
                switch (_FormState)
                {
                    case FormState.Normal:
                        Bounds_Current_Size = Bounds_Normal_Size = value;
                        break;

                    case FormState.QuarterScreen:
                        Bounds_Current_Size = Bounds_QuarterScreen_Size = value;
                        break;

                    case FormState.HighAsScreen:
                        Bounds_Current_Width = Bounds_Normal_Width = value.Width;
                        break;
                }

                if (_Initialized)
                {
                    Action InvokeMethod = () =>
                    {
                        _UpdateLayout(UpdateLayoutEventType.All);
                    };

                    _Client.Invoke(InvokeMethod);
                }
            }
        }

        /// <summary>
        /// 获取或设置窗口的位置与大小。
        /// </summary>
        public Rectangle Bounds
        {
            get
            {
                return Bounds_Current;
            }

            set
            {
                Rectangle _Bounds = new Rectangle();
                _Bounds.Size = new Size(Math.Max(MinimumBoundsWidth, Math.Min(value.Width, MaximumBoundsWidth)), Math.Max(MinimumBoundsHeight, Math.Min(value.Height, MaximumBoundsHeight)));
                _Bounds.Location = new Point(Math.Max(PrimaryScreenBounds.X - _Bounds.Width + 1, Math.Min(value.X, PrimaryScreenBounds.Right - 1)), Math.Max(PrimaryScreenBounds.Y - _Bounds.Height + 1, Math.Min(value.Y, PrimaryScreenBounds.Bottom - 1)));

                switch (_FormState)
                {
                    case FormState.Normal:
                        Bounds_Current = Bounds_Normal = _Bounds;
                        break;

                    case FormState.QuarterScreen:
                        Bounds_Current = Bounds_QuarterScreen = _Bounds;
                        break;

                    case FormState.HighAsScreen:
                        Bounds_Current_X = Bounds_Normal_X = _Bounds.X;
                        Bounds_Current_Width = Bounds_Normal_Width = _Bounds.Width;
                        break;
                }

                if (_Initialized)
                {
                    Action InvokeMethod = () =>
                    {
                        _UpdateLayout(UpdateLayoutEventType.All);
                    };

                    _Client.Invoke(InvokeMethod);
                }
            }
        }

        //

        /// <summary>
        /// 获取或设置窗口的工作区的位置。
        /// </summary>
        public Point ClientLocation
        {
            get
            {
                return new Point(Bounds_Current_X, Bounds_Current_Y + _CaptionBarHeight);
            }

            set
            {
                Location = new Point(value.X, value.Y - _CaptionBarHeight);
            }
        }

        /// <summary>
        /// 获取或设置窗口的工作区的大小。
        /// </summary>
        public Size ClientSize
        {
            get
            {
                return new Size(Bounds_Current_Width, Bounds_Current_Height - _CaptionBarHeight);
            }

            set
            {
                Size = new Size(value.Width, value.Height + _CaptionBarHeight);
            }
        }

        /// <summary>
        /// 获取或设置窗口的工作区的位置与大小。
        /// </summary>
        public Rectangle ClientBounds
        {
            get
            {
                return new Rectangle(new Point(Bounds_Current_X, Bounds_Current_Y + _CaptionBarHeight), new Size(Bounds_Current_Width, Bounds_Current_Height - _CaptionBarHeight));
            }

            set
            {
                Bounds = new Rectangle(new Point(value.X, value.Y - _CaptionBarHeight), new Size(value.Width, value.Height + _CaptionBarHeight));
            }
        }

        //

        /// <summary>
        /// 设置用于验证是否允许窗口还原的方法。
        /// </summary>
        public Predicate<EventArgs> ReturnVerification
        {
            set
            {
                _ReturnVerification = value;
            }
        }

        /// <summary>
        /// 设置用于验证是否允许窗口最大化的方法。
        /// </summary>
        public Predicate<EventArgs> MaximizeVerification
        {
            set
            {
                _MaximizeVerification = value;
            }
        }

        /// <summary>
        /// 设置用于验证是否允许窗口进入全屏的方法。
        /// </summary>
        public Predicate<EventArgs> EnterFullScreenVerification
        {
            set
            {
                _EnterFullScreenVerification = value;
            }
        }

        /// <summary>
        /// 设置用于验证是否允许窗口退出全屏的方法。
        /// </summary>
        public Predicate<EventArgs> ExitFullScreenVerification
        {
            set
            {
                _ExitFullScreenVerification = value;
            }
        }

        /// <summary>
        /// 设置用于验证是否允许窗口关闭的方法。
        /// </summary>
        public Predicate<EventArgs> CloseVerification
        {
            set
            {
                _CloseVerification = value;
            }
        }

        #endregion

        #region 事件

        /// <summary>
        /// 在窗口加载时发生。
        /// </summary>
        public event EventHandler Loading
        {
            add
            {
                if (value != null)
                {
                    _Events.AddHandler(EventKey.Loading, value);
                }
            }

            remove
            {
                if (value != null)
                {
                    _Events.RemoveHandler(EventKey.Loading, value);
                }
            }
        }

        /// <summary>
        /// 在窗口加载后发生。
        /// </summary>
        public event EventHandler Loaded
        {
            add
            {
                if (value != null)
                {
                    _Events.AddHandler(EventKey.Loaded, value);
                }
            }

            remove
            {
                if (value != null)
                {
                    _Events.RemoveHandler(EventKey.Loaded, value);
                }
            }
        }

        /// <summary>
        /// 在窗口关闭时发生。
        /// </summary>
        public event EventHandler Closing
        {
            add
            {
                if (value != null)
                {
                    _Events.AddHandler(EventKey.Closing, value);
                }
            }

            remove
            {
                if (value != null)
                {
                    _Events.RemoveHandler(EventKey.Closing, value);
                }
            }
        }

        /// <summary>
        /// 在窗口关闭后发生。
        /// </summary>
        public event EventHandler Closed
        {
            add
            {
                if (value != null)
                {
                    _Events.AddHandler(EventKey.Closed, value);
                }
            }

            remove
            {
                if (value != null)
                {
                    _Events.RemoveHandler(EventKey.Closed, value);
                }
            }
        }

        /// <summary>
        /// 在窗口移动时发生。
        /// </summary>
        public event EventHandler Move
        {
            add
            {
                if (value != null)
                {
                    _Events.AddHandler(EventKey.Move, value);
                }
            }

            remove
            {
                if (value != null)
                {
                    _Events.RemoveHandler(EventKey.Move, value);
                }
            }
        }

        /// <summary>
        /// 在窗口的位置更改时发生。
        /// </summary>
        public event EventHandler LocationChanged
        {
            add
            {
                if (value != null)
                {
                    _Events.AddHandler(EventKey.LocationChanged, value);
                }
            }

            remove
            {
                if (value != null)
                {
                    _Events.RemoveHandler(EventKey.LocationChanged, value);
                }
            }
        }

        /// <summary>
        /// 在窗口的大小调整时发生。
        /// </summary>
        public event EventHandler Resize
        {
            add
            {
                if (value != null)
                {
                    _Events.AddHandler(EventKey.Resize, value);
                }
            }

            remove
            {
                if (value != null)
                {
                    _Events.RemoveHandler(EventKey.Resize, value);
                }
            }
        }

        /// <summary>
        /// 在窗口的大小更改时发生。
        /// </summary>
        public event EventHandler SizeChanged
        {
            add
            {
                if (value != null)
                {
                    _Events.AddHandler(EventKey.SizeChanged, value);
                }
            }

            remove
            {
                if (value != null)
                {
                    _Events.RemoveHandler(EventKey.SizeChanged, value);
                }
            }
        }

        /// <summary>
        /// 在窗口的样式更改时发生。
        /// </summary>
        public event EventHandler FormStyleChanged
        {
            add
            {
                if (value != null)
                {
                    _Events.AddHandler(EventKey.FormStyleChanged, value);
                }
            }

            remove
            {
                if (value != null)
                {
                    _Events.RemoveHandler(EventKey.FormStyleChanged, value);
                }
            }
        }

        /// <summary>
        /// 在窗口的状态更改时发生。
        /// </summary>
        public event EventHandler FormStateChanged
        {
            add
            {
                if (value != null)
                {
                    _Events.AddHandler(EventKey.FormStateChanged, value);
                }
            }

            remove
            {
                if (value != null)
                {
                    _Events.RemoveHandler(EventKey.FormStateChanged, value);
                }
            }
        }

        /// <summary>
        /// 在表示窗口是否对用户交互作出响应的布尔值更改时发生。
        /// </summary>
        public event EventHandler EnabledChanged
        {
            add
            {
                if (value != null)
                {
                    _Events.AddHandler(EventKey.EnabledChanged, value);
                }
            }

            remove
            {
                if (value != null)
                {
                    _Events.RemoveHandler(EventKey.EnabledChanged, value);
                }
            }
        }

        /// <summary>
        /// 在窗口的不透明度更改时发生。
        /// </summary>
        public event EventHandler OpacityChanged
        {
            add
            {
                if (value != null)
                {
                    _Events.AddHandler(EventKey.OpacityChanged, value);
                }
            }

            remove
            {
                if (value != null)
                {
                    _Events.RemoveHandler(EventKey.OpacityChanged, value);
                }
            }
        }

        /// <summary>
        /// 在窗口的标题更改时发生。
        /// </summary>
        public event EventHandler CaptionChanged
        {
            add
            {
                if (value != null)
                {
                    _Events.AddHandler(EventKey.CaptionChanged, value);
                }
            }

            remove
            {
                if (value != null)
                {
                    _Events.RemoveHandler(EventKey.CaptionChanged, value);
                }
            }
        }

        /// <summary>
        /// 在窗口的主题更改时发生。
        /// </summary>
        public event EventHandler ThemeChanged
        {
            add
            {
                if (value != null)
                {
                    _Events.AddHandler(EventKey.ThemeChanged, value);
                }
            }

            remove
            {
                if (value != null)
                {
                    _Events.RemoveHandler(EventKey.ThemeChanged, value);
                }
            }
        }

        /// <summary>
        /// 在窗口的主题色更改时发生。
        /// </summary>
        public event EventHandler ThemeColorChanged
        {
            add
            {
                if (value != null)
                {
                    _Events.AddHandler(EventKey.ThemeColorChanged, value);
                }
            }

            remove
            {
                if (value != null)
                {
                    _Events.RemoveHandler(EventKey.ThemeColorChanged, value);
                }
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 使窗口还原至普通大小。
        /// </summary>
        public bool Return()
        {
            Func<bool> InvokeMethod = () =>
            {
                if (_Client.WindowState == FormWindowState.Minimized)
                {
                    if (_FormState == FormState.FullScreen)
                    {
                        _Client.WindowState = FormWindowState.Maximized;
                    }
                    else
                    {
                        _Client.WindowState = FormWindowState.Normal;
                    }

                    return true;
                }
                else
                {
                    if (_CanExitFullScreen())
                    {
                        _ExitFullScreen(UpdateLayoutBehavior.Animate, UpdateLayoutEventType.All);

                        return true;
                    }
                    else if (_CanReturn())
                    {
                        _Return(UpdateLayoutBehavior.Animate, UpdateLayoutEventType.All);

                        return true;
                    }

                    return false;
                }
            };

            return (bool)_Client.Invoke(InvokeMethod);
        }

        /// <summary>
        /// 使窗口最小化至任务栏。
        /// </summary>
        public bool Minimize()
        {
            Func<bool> InvokeMethod = () =>
            {
                if (_CanMinimize())
                {
                    _Minimize();

                    return true;
                }

                return false;
            };

            return (bool)_Client.Invoke(InvokeMethod);
        }

        /// <summary>
        /// 使窗口最大化。
        /// </summary>
        public bool Maximize()
        {
            Func<bool> InvokeMethod = () =>
            {
                if (_CanMaximize())
                {
                    _Maximize(UpdateLayoutBehavior.Animate, UpdateLayoutEventType.All);

                    return true;
                }

                return false;
            };

            return (bool)_Client.Invoke(InvokeMethod);
        }

        /// <summary>
        /// 使窗口进入全屏幕模式。
        /// </summary>
        public bool EnterFullScreen()
        {
            Func<bool> InvokeMethod = () =>
            {
                if (_CanEnterFullScreen())
                {
                    _EnterFullScreen(UpdateLayoutBehavior.Animate, UpdateLayoutEventType.All);

                    return true;
                }

                return false;
            };

            return (bool)_Client.Invoke(InvokeMethod);
        }

        /// <summary>
        /// 使窗口退出全屏幕模式。
        /// </summary>
        public bool ExitFullScreen()
        {
            Func<bool> InvokeMethod = () =>
            {
                if (_CanExitFullScreen())
                {
                    _ExitFullScreen(UpdateLayoutBehavior.Animate, UpdateLayoutEventType.All);

                    return true;
                }

                return false;
            };

            return (bool)_Client.Invoke(InvokeMethod);
        }

        /// <summary>
        /// 使窗口占据桌面的左半区域。
        /// </summary>
        public bool LeftHalfScreen()
        {
            Func<bool> InvokeMethod = () =>
            {
                if (_CanHighAsScreen())
                {
                    _LeftHalfScreen(UpdateLayoutBehavior.Animate, UpdateLayoutEventType.All);

                    return true;
                }

                return false;
            };

            return (bool)_Client.Invoke(InvokeMethod);
        }

        /// <summary>
        /// 使窗口占据桌面的右半区域。
        /// </summary>
        public bool RightHalfScreen()
        {
            Func<bool> InvokeMethod = () =>
            {
                if (_CanHighAsScreen())
                {
                    _RightHalfScreen(UpdateLayoutBehavior.Animate, UpdateLayoutEventType.All);

                    return true;
                }

                return false;
            };

            return (bool)_Client.Invoke(InvokeMethod);
        }

        /// <summary>
        /// 使窗口与桌面的高度相同。
        /// </summary>
        public bool HighAsScreen()
        {
            Func<bool> InvokeMethod = () =>
            {
                if (_CanHighAsScreen())
                {
                    _HighAsScreen(UpdateLayoutBehavior.Animate, UpdateLayoutEventType.All);

                    return true;
                }

                return false;
            };

            return (bool)_Client.Invoke(InvokeMethod);
        }

        /// <summary>
        /// 使窗口占据桌面的左上四分之一区域。
        /// </summary>
        public bool TopLeftQuarterScreen()
        {
            Func<bool> InvokeMethod = () =>
            {
                if (_CanQuarterScreen())
                {
                    _TopLeftQuarterScreen(UpdateLayoutBehavior.Animate, UpdateLayoutEventType.All);

                    return true;
                }

                return false;
            };

            return (bool)_Client.Invoke(InvokeMethod);
        }

        /// <summary>
        /// 使窗口占据桌面的右上四分之一区域。
        /// </summary>
        public bool TopRightQuarterScreen()
        {
            Func<bool> InvokeMethod = () =>
            {
                if (_CanQuarterScreen())
                {
                    _TopRightQuarterScreen(UpdateLayoutBehavior.Animate, UpdateLayoutEventType.All);

                    return true;
                }

                return false;
            };

            return (bool)_Client.Invoke(InvokeMethod);
        }

        /// <summary>
        /// 使窗口占据桌面的左下四分之一区域。
        /// </summary>
        public bool BottomLeftQuarterScreen()
        {
            Func<bool> InvokeMethod = () =>
            {
                if (_CanQuarterScreen())
                {
                    _BottomLeftQuarterScreen(UpdateLayoutBehavior.Animate, UpdateLayoutEventType.All);

                    return true;
                }

                return false;
            };

            return (bool)_Client.Invoke(InvokeMethod);
        }

        /// <summary>
        /// 使窗口占据桌面的右下四分之一区域。
        /// </summary>
        public bool BottomRightQuarterScreen()
        {
            Func<bool> InvokeMethod = () =>
            {
                if (_CanQuarterScreen())
                {
                    _BottomRightQuarterScreen(UpdateLayoutBehavior.Animate, UpdateLayoutEventType.All);

                    return true;
                }

                return false;
            };

            return (bool)_Client.Invoke(InvokeMethod);
        }

        /// <summary>
        /// 关闭窗口。
        /// </summary>
        public bool Close()
        {
            Func<bool> InvokeMethod = () =>
            {
                if (_CanClose())
                {
                    _Close();

                    return true;
                }

                return false;
            };

            return (bool)_Client.Invoke(InvokeMethod);
        }

        #endregion

        #region 基类方法

        /// <summary>
        /// 判断此 FormManager 对象是否与指定的对象相等。
        /// </summary>
        /// <param name="obj">用于比较的对象。</param>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is FormManager))
            {
                return false;
            }

            return Equals((FormManager)obj);
        }

        /// <summary>
        /// 判断此 FormManager 对象是否与指定的 FormManager 对象相等。
        /// </summary>
        /// <param name="formManager">用于比较的 FormManager 对象。</param>
        public bool Equals(FormManager formManager)
        {
            if (formManager == null)
            {
                return false;
            }

            return base.Equals(formManager);
        }

        /// <summary>
        /// 返回此 FormManager 对象的哈希代码。
        /// </summary>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// 将此 FormManager 对象转换为字符串。
        /// </summary>
        public override string ToString()
        {
            string Str = base.ToString();

            if (!string.IsNullOrEmpty(_Caption))
            {
                return string.Concat(Str, ", Caption: ", _Caption);
            }

            return Str;
        }

        #endregion
    }
}