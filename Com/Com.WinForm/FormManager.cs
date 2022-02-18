/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2022 chibayuki@foxmail.com

Com.WinForm.FormManager
Version 20.10.27.1900

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
    public sealed class FormManager : IEquatable<FormManager>
    {
        #region 非公开成员

        // 获取主屏幕的边界。
        internal static Rectangle PrimaryScreenBounds
        {
            get
            {
                return Screen.PrimaryScreen.Bounds;
            }
        }

        // 获取主屏幕的工作区。
        internal static Rectangle PrimaryScreenClient
        {
            get
            {
                return Screen.PrimaryScreen.WorkingArea;
            }
        }

        //

        private bool _Initialized = false; // 表示此 FormManager 是否已完成初始化的布尔值。

        private bool _LoadingNow = false; // 表示此 FormManager 是否正在执行 Loading 事件或 Loaded 事件的布尔值。
        private bool _ClosingNow = false; // 表示此 FormManager 是否正在执行 Closing 事件或 Closed 事件的布尔值。

        //

        private Form _Client = null; // 表示工作区的 Form 对象。

        // 获取表示工作区的 Form 对象。
        internal Form Client
        {
            get
            {
                return _Client;
            }
        }

        private CaptionBar _CaptionBar = null; // 表示窗口标题栏的 CaptionBar 对象。

        // 获取表示窗口标题栏的 CaptionBar 对象。
        internal CaptionBar CaptionBar
        {
            get
            {
                return _CaptionBar;
            }
        }

        private Resizer _Resizer = null; // 表示窗口大小调节器的 Resizer 对象。

        // 获取表示窗口大小调节器的 Resizer 对象。
        internal Resizer Resizer
        {
            get
            {
                return _Resizer;
            }
        }

        private SplashScreen _SplashScreen = null; // 表示启动屏幕的 SplashScreen 对象。

        // 获取表示启动屏幕的 SplashScreen 对象。
        internal SplashScreen SplashScreen
        {
            get
            {
                return _SplashScreen;
            }
        }

        //

        private static List<FormManager> _FormManagerList = new List<FormManager>(1); // 窗口管理器列表。

        private bool _IsMainForm = false; // 表示此窗口是否为主窗口的布尔值。

        // 获取表示此窗口是否为主窗口的布尔值。
        internal bool IsMainForm
        {
            get
            {
                return _IsMainForm;
            }
        }

        //

        private FormManager _Owner = null; // 拥有此窗口的窗口的窗口管理器。
        private List<FormManager> _Owned = new List<FormManager>(1); // 表示此窗口拥有的所有窗口的窗口管理器列表。

        // 向表示此窗口拥有的所有窗口的窗口管理器列表添加一个窗口管理器。
        private void _AddOwned(FormManager formManager)
        {
            if (!(formManager is null) && !_Owned.Contains(formManager))
            {
                _Owned.Add(formManager);

                if (Enabled == true && _Owned.Count > 0)
                {
                    Enabled = false;
                }
            }
        }

        // 从表示此窗口拥有的所有窗口的窗口管理器列表删除一个窗口管理器。
        private void _RemoveOwned(FormManager formManager)
        {
            if (!(formManager is null) && _Owned.Contains(formManager))
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

        private Timer _ResolutionMonitor = null; // 用于监听主屏幕分辨率变更的 Timer。

        private Rectangle _PreviousPrimaryScreenClient = PrimaryScreenClient; // Screen.PrimaryScreen.WorkingArea 的此前值。

        //

        private bool _ClientIsActive = false; // 表示 _Client 是否处于激活状态的布尔值。
        private bool _CaptionBarIsActive = false; // 表示 _CaptionBar 是否处于激活状态的布尔值。
        private bool _ResizerIsActive = false; // 表示 _Resizer 是否处于激活状态的布尔值。
        private bool _SplashScreenIsActive = false; // 表示 _SplashScreen 是否处于激活状态的布尔值。

        private bool _MainMenuIsActive = false; // 表示主菜单是否处于激活状态的布尔值。

        // 设置主菜单是否处于激活状态的布尔值。
        internal bool MainMenuIsActive
        {
            set
            {
                _MainMenuIsActive = value;
            }
        }

        private Timer _ActivationMonitor = null; // 用于监听窗口激活状态变更的 Timer。

        private bool _IsActive = true; // 表示窗口是否处于激活状态的布尔值。

        //

        private const int _ControlBoxButtonWidth = 46; // 控制按钮的宽度。
        private const int _ControlBoxButtonHeight = 32; // 控制按钮的高度。

        // 获取控制按钮的宽度。
        internal int ControlBoxButtonWidth
        {
            get
            {
                return _ControlBoxButtonWidth;
            }
        }

        // 获取控制按钮的高度。
        internal int ControlBoxButtonHeight
        {
            get
            {
                return _ControlBoxButtonHeight;
            }
        }

        // 获取控制按钮的大小。
        internal Size ControlBoxButtonSize
        {
            get
            {
                return new Size(_ControlBoxButtonWidth, _ControlBoxButtonHeight);
            }
        }

        //

        private const int _ResizerSize = 8; // 窗口大小调节器的大小。

        // 获取窗口大小调节器的大小。
        internal int ResizerSize
        {
            get
            {
                return _ResizerSize;
            }
        }

        //

        private FormStyle _FormStyle = FormStyle.Sizable; // 窗口的样式。

        private bool _EnableMinimize = true; // 表示是否允许窗口最小化的布尔值。
        private bool _EnableMaximize = true; // 表示是否允许窗口最大化的布尔值。
        private bool _EnableFullScreen = true; // 表示是否允许窗口以全屏幕模式运行的布尔值。

        private bool _ShowIconOnCaptionBar = true; // 表示是否在窗口标题栏上显示图标的布尔值。
        private bool _ShowInTaskbar = true; // 表示窗口是否在任务栏显示的布尔值。
        private bool _TopMost = false; // 表示是否允许窗口置于顶层的布尔值。

        //

        private Effect _Effect = Effect.All; // 窗口交互过程显示的效果。

        //

        private int _MinimumWidth = 0; // 窗口的最小宽度。
        private int _MinimumHeight = 0; // 窗口的最小高度。

        // 获取或设置窗口的最小宽度。
        internal int MinimumWidth
        {
            get
            {
                return Math.Max(_ControlBoxButtonWidth, Math.Min(_MinimumWidth, PrimaryScreenBounds.Width));
            }

            set
            {
                _MinimumWidth = value;
            }
        }

        // 获取或设置窗口的最小高度。
        internal int MinimumHeight
        {
            get
            {
                return Math.Max(_CaptionBarHeight + 2, Math.Min(_MinimumHeight, PrimaryScreenBounds.Height));
            }

            set
            {
                _MinimumHeight = value;
            }
        }

        private int _MaximumWidth = 0; // 窗口的最大宽度。
        private int _MaximumHeight = 0; // 窗口的最大高度。

        // 获取或设置窗口的最大宽度。
        internal int MaximumWidth
        {
            get
            {
                return Math.Max(1, (_MaximumWidth > 0 ? Math.Min(_MaximumWidth, PrimaryScreenBounds.Width) : PrimaryScreenBounds.Width));
            }

            set
            {
                _MaximumWidth = value;
            }
        }

        // 获取或设置窗口的最大高度。
        internal int MaximumHeight
        {
            get
            {
                return Math.Max(1, (_MaximumHeight > 0 ? Math.Min(_MaximumHeight, PrimaryScreenBounds.Height) : PrimaryScreenBounds.Height));
            }

            set
            {
                _MaximumHeight = value;
            }
        }

        //

        private int _CaptionBarHeight = _ControlBoxButtonHeight; // 窗口标题栏的高度。

        //

        private bool _Enabled = true; // 表示窗口是否对用户交互作出响应的布尔值。
        private bool _Visible = true; // 表示窗口是否可见的布尔值。
        private double _Opacity = 1.0; // 窗口的不透明度。
        private string _Caption = null; // 窗口的标题。
        private bool _ShowCaption = true; // 表示是否在窗口标题栏上显示窗口的标题的布尔值。
        private Font _CaptionFont = new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, 134); // 窗口标题的字体。
        private ContentAlignment _CaptionAlign = ContentAlignment.TopCenter; // 窗口标题的文本对齐方式。
        private Bitmap _CaptionBarBackgroundImage = null; // 窗口标题栏的背景图像。
        private Theme _Theme = Theme.Colorful; // 窗口的主题。
        private ColorX _ThemeColor = ColorX.FromRGB(128, 128, 128); // 窗口的主题色。
        private bool _ShowCaptionBarColor = true; // 表示是否在窗口标题栏上显示主题色的布尔值。
        private bool _EnableCaptionBarTransparent = true; // 表示是否允许以半透明方式显示窗口标题栏的布尔值。
        private bool _ShowShadowColor = true; // 表示是否在窗口阴影显示主题色的布尔值。
        private RecommendColors _RecommendColors = null; // 当前主题建议的颜色。

        //

        // 获取窗口标题栏的不透明度系数。
        internal double CaptionBarOpacityRatio
        {
            get
            {
                return (_EnableCaptionBarTransparent && _IsActive ? 0.9 : 1.0);
            }
        }

        // 获取窗口阴影的不透明度系数。
        internal double ShadowOpacityRatio
        {
            get
            {
                return (_ShowShadowColor ? (_IsActive ? 0.15 : 0.09) : (_IsActive ? 0.1 : 0.06));
            }
        }

        //

        private FormWindowState _PreviousFormWindowState = FormWindowState.Normal; // _Client.WindowState 的此前值。

        //

        private FormState _FormState = FormState.Normal; // 窗口的状态。
        private FormState _FormState_BeforeFullScreen = FormState.Normal; // 进入全屏幕状态之前窗口的状态。

        private FormState _PreviousFormState = FormState.Normal; // FormState 的此前值。

        // 获取 FormState 的实际值（非 FormState.Minimized 值）。
        internal FormState ActualFormState
        {
            get
            {
                return _FormState;
            }
        }

        // 获取 FormState 的此前值。
        internal FormState PreviousFormState
        {
            get
            {
                return _PreviousFormState;
            }
        }

        //

        private int _Bounds_Current_X = int.MinValue; // 当前状态窗口在桌面的左边距。
        private int _Bounds_Current_Y = int.MinValue; // 当前状态窗口在桌面的上边距。
        private int _Bounds_Current_Width = int.MinValue; // 当前状态窗口的宽度。
        private int _Bounds_Current_Height = int.MinValue; // 当前状态窗口的高度。

        // 获取或设置当前状态窗口在桌面的左边距。
        internal int Bounds_Current_X
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

        // 获取或设置当前状态窗口在桌面的上边距。
        internal int Bounds_Current_Y
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

        // 获取当前状态窗口在桌面的右边距。
        internal int Bounds_Current_Right
        {
            get
            {
                return (_Bounds_Current_X + _Bounds_Current_Width);
            }
        }

        // 获取当前状态窗口在桌面的下边距。
        internal int Bounds_Current_Bottom
        {
            get
            {
                return (_Bounds_Current_Y + _Bounds_Current_Height);
            }
        }

        // 获取或设置当前状态窗口的宽度。
        internal int Bounds_Current_Width
        {
            get
            {
                return _Bounds_Current_Width;
            }

            set
            {
                if (_FormState == FormState.FullScreen)
                {
                    _Bounds_Current_Width = value;
                }
                else
                {
                    _Bounds_Current_Width = Math.Max(MinimumWidth, Math.Min(value, MaximumWidth));
                }
            }
        }

        // 获取或设置当前状态窗口的高度。
        internal int Bounds_Current_Height
        {
            get
            {
                return _Bounds_Current_Height;
            }

            set
            {
                if (_FormState == FormState.FullScreen)
                {
                    _Bounds_Current_Height = value;
                }
                else
                {
                    _Bounds_Current_Height = Math.Max(MinimumHeight, Math.Min(value, MaximumHeight));
                }
            }
        }

        // 获取或设置当前状态窗口在桌面的位置。
        internal Point Bounds_Current_Location
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

        // 获取或设置当前状态窗口的大小。
        internal Size Bounds_Current_Size
        {
            get
            {
                return new Size(_Bounds_Current_Width, _Bounds_Current_Height);
            }

            set
            {
                if (_FormState == FormState.FullScreen)
                {
                    _Bounds_Current_Width = value.Width;
                    _Bounds_Current_Height = value.Height;
                }
                else
                {
                    _Bounds_Current_Width = Math.Max(MinimumWidth, Math.Min(value.Width, MaximumWidth));
                    _Bounds_Current_Height = Math.Max(MinimumHeight, Math.Min(value.Height, MaximumHeight));
                }
            }
        }

        // 获取或设置当前状态窗口的位置与大小。
        internal Rectangle Bounds_Current
        {
            get
            {
                return new Rectangle(_Bounds_Current_X, _Bounds_Current_Y, _Bounds_Current_Width, _Bounds_Current_Height);
            }

            set
            {
                _Bounds_Current_X = value.X;
                _Bounds_Current_Y = value.Y;

                if (_FormState == FormState.FullScreen)
                {
                    _Bounds_Current_Width = value.Width;
                    _Bounds_Current_Height = value.Height;
                }
                else
                {
                    _Bounds_Current_Width = Math.Max(MinimumWidth, Math.Min(value.Width, MaximumWidth));
                    _Bounds_Current_Height = Math.Max(MinimumHeight, Math.Min(value.Height, MaximumHeight));
                }
            }
        }

        private int _Bounds_Normal_X = int.MinValue; // 普通状态窗口在桌面的左边距。
        private int _Bounds_Normal_Y = int.MinValue; // 普通状态窗口在桌面的上边距。
        private int _Bounds_Normal_Width = int.MinValue; // 普通状态窗口的宽度。
        private int _Bounds_Normal_Height = int.MinValue; // 普通状态窗口的高度。

        // 获取或设置普通状态窗口在桌面的左边距。
        internal int Bounds_Normal_X
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

        // 获取或设置普通状态窗口在桌面的上边距。
        internal int Bounds_Normal_Y
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

        // 获取普通状态窗口在桌面的右边距。
        internal int Bounds_Normal_Right
        {
            get
            {
                return (_Bounds_Normal_X + _Bounds_Normal_Width);
            }
        }

        // 获取普通状态窗口在桌面的下边距。
        internal int Bounds_Normal_Bottom
        {
            get
            {
                return (_Bounds_Normal_Y + _Bounds_Normal_Height);
            }
        }

        // 获取或设置普通状态窗口的宽度。
        internal int Bounds_Normal_Width
        {
            get
            {
                return _Bounds_Normal_Width;
            }

            set
            {
                _Bounds_Normal_Width = Math.Max(MinimumWidth, Math.Min(value, MaximumWidth));
            }
        }

        // 获取或设置普通状态窗口的高度。
        internal int Bounds_Normal_Height
        {
            get
            {
                return _Bounds_Normal_Height;
            }

            set
            {
                _Bounds_Normal_Height = Math.Max(MinimumHeight, Math.Min(value, MaximumHeight));
            }
        }

        // 获取或设置普通状态窗口在桌面的位置。
        internal Point Bounds_Normal_Location
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

        // 获取或设置普通状态窗口的大小。
        internal Size Bounds_Normal_Size
        {
            get
            {
                return new Size(_Bounds_Normal_Width, _Bounds_Normal_Height);
            }

            set
            {
                _Bounds_Normal_Width = Math.Max(MinimumWidth, Math.Min(value.Width, MaximumWidth));
                _Bounds_Normal_Height = Math.Max(MinimumHeight, Math.Min(value.Height, MaximumHeight));
            }
        }

        // 获取或设置普通状态窗口的位置与大小。
        internal Rectangle Bounds_Normal
        {
            get
            {
                return new Rectangle(_Bounds_Normal_X, _Bounds_Normal_Y, _Bounds_Normal_Width, _Bounds_Normal_Height);
            }

            set
            {
                _Bounds_Normal_X = value.X;
                _Bounds_Normal_Y = value.Y;
                _Bounds_Normal_Width = Math.Max(MinimumWidth, Math.Min(value.Width, MaximumWidth));
                _Bounds_Normal_Height = Math.Max(MinimumHeight, Math.Min(value.Height, MaximumHeight));
            }
        }

        private int _Bounds_QuarterScreen_X = 0; // 占据桌面四分之一状态窗口在桌面的左边距。
        private int _Bounds_QuarterScreen_Y = 0; // 占据桌面四分之一状态窗口在桌面的上边距。
        private int _Bounds_QuarterScreen_Width = 0; // 占据桌面四分之一状态窗口的宽度。
        private int _Bounds_QuarterScreen_Height = 0; // 占据桌面四分之一状态窗口的高度。

        // 获取或设置占据桌面四分之一状态窗口在桌面的左边距。
        internal int Bounds_QuarterScreen_X
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

        // 获取或设置占据桌面四分之一状态窗口在桌面的上边距。
        internal int Bounds_QuarterScreen_Y
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

        // 获取占据桌面四分之一状态窗口在桌面的右边距。
        internal int Bounds_QuarterScreen_Right
        {
            get
            {
                return (_Bounds_QuarterScreen_X + _Bounds_QuarterScreen_Width);
            }
        }

        // 获取占据桌面四分之一状态窗口在桌面的下边距。
        internal int Bounds_QuarterScreen_Bottom
        {
            get
            {
                return (_Bounds_QuarterScreen_Y + _Bounds_QuarterScreen_Height);
            }
        }

        // 获取或设置占据桌面四分之一状态窗口的宽度。
        internal int Bounds_QuarterScreen_Width
        {
            get
            {
                return _Bounds_QuarterScreen_Width;
            }

            set
            {
                _Bounds_QuarterScreen_Width = Math.Max(MinimumWidth, Math.Min(value, MaximumWidth));
            }
        }

        // 获取或设置占据桌面四分之一状态窗口的高度。
        internal int Bounds_QuarterScreen_Height
        {
            get
            {
                return _Bounds_QuarterScreen_Height;
            }

            set
            {
                _Bounds_QuarterScreen_Height = Math.Max(MinimumHeight, Math.Min(value, MaximumHeight));
            }
        }

        // 获取或设置占据桌面四分之一状态窗口在桌面的位置。
        internal Point Bounds_QuarterScreen_Location
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

        // 获取或设置占据桌面四分之一状态窗口的大小。
        internal Size Bounds_QuarterScreen_Size
        {
            get
            {
                return new Size(_Bounds_QuarterScreen_Width, _Bounds_QuarterScreen_Height);
            }

            set
            {
                _Bounds_QuarterScreen_Width = Math.Max(MinimumWidth, Math.Min(value.Width, MaximumWidth));
                _Bounds_QuarterScreen_Height = Math.Max(MinimumHeight, Math.Min(value.Height, MaximumHeight));
            }
        }

        // 获取或设置占据桌面四分之一状态窗口的位置与大小。
        internal Rectangle Bounds_QuarterScreen
        {
            get
            {
                return new Rectangle(_Bounds_QuarterScreen_X, _Bounds_QuarterScreen_Y, _Bounds_QuarterScreen_Width, _Bounds_QuarterScreen_Height);
            }

            set
            {
                _Bounds_QuarterScreen_X = value.X;
                _Bounds_QuarterScreen_Y = value.Y;
                _Bounds_QuarterScreen_Width = Math.Max(MinimumWidth, Math.Min(value.Width, MaximumWidth));
                _Bounds_QuarterScreen_Height = Math.Max(MinimumHeight, Math.Min(value.Height, MaximumHeight));
            }
        }

        private int _Bounds_BeforeFullScreen_X = 0; // 进入全屏幕状态前窗口在桌面的左边距。
        private int _Bounds_BeforeFullScreen_Y = 0; // 进入全屏幕状态前窗口在桌面的上边距。
        private int _Bounds_BeforeFullScreen_Width = 0; // 进入全屏幕状态前窗口的宽度。
        private int _Bounds_BeforeFullScreen_Height = 0; // 进入全屏幕状态前窗口的高度。

        // 获取或设置进入全屏幕状态前窗口在桌面的左边距。
        internal int Bounds_BeforeFullScreen_X
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

        // 获取或设置进入全屏幕状态前窗口在桌面的上边距。
        internal int Bounds_BeforeFullScreen_Y
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

        // 获取进入全屏幕状态前窗口在桌面的右边距。
        internal int Bounds_BeforeFullScreen_Right
        {
            get
            {
                return (_Bounds_BeforeFullScreen_X + _Bounds_BeforeFullScreen_Width);
            }
        }

        // 获取进入全屏幕状态前窗口在桌面的下边距。
        internal int Bounds_BeforeFullScreen_Bottom
        {
            get
            {
                return (_Bounds_BeforeFullScreen_Y + _Bounds_BeforeFullScreen_Height);
            }
        }

        // 获取或设置进入全屏幕状态前窗口的宽度。
        internal int Bounds_BeforeFullScreen_Width
        {
            get
            {
                return _Bounds_BeforeFullScreen_Width;
            }

            set
            {
                _Bounds_BeforeFullScreen_Width = Math.Max(MinimumWidth, Math.Min(value, MaximumWidth));
            }
        }

        // 获取或设置进入全屏幕状态前窗口的高度。
        internal int Bounds_BeforeFullScreen_Height
        {
            get
            {
                return _Bounds_BeforeFullScreen_Height;
            }

            set
            {
                _Bounds_BeforeFullScreen_Height = Math.Max(MinimumHeight, Math.Min(value, MaximumHeight));
            }
        }

        // 获取或设置进入全屏幕状态前窗口在桌面的位置。
        internal Point Bounds_BeforeFullScreen_Location
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

        // 获取或设置进入全屏幕状态前窗口的大小。
        internal Size Bounds_BeforeFullScreen_Size
        {
            get
            {
                return new Size(_Bounds_BeforeFullScreen_Width, _Bounds_BeforeFullScreen_Height);
            }

            set
            {
                _Bounds_BeforeFullScreen_Width = Math.Max(MinimumWidth, Math.Min(value.Width, MaximumWidth));
                _Bounds_BeforeFullScreen_Height = Math.Max(MinimumHeight, Math.Min(value.Height, MaximumHeight));
            }
        }

        // 获取或设置进入全屏幕状态前窗口的位置与大小。
        internal Rectangle Bounds_BeforeFullScreen
        {
            get
            {
                return new Rectangle(_Bounds_BeforeFullScreen_X, _Bounds_BeforeFullScreen_Y, _Bounds_BeforeFullScreen_Width, _Bounds_BeforeFullScreen_Height);
            }

            set
            {
                _Bounds_BeforeFullScreen_X = value.X;
                _Bounds_BeforeFullScreen_Y = value.Y;
                _Bounds_BeforeFullScreen_Width = Math.Max(MinimumWidth, Math.Min(value.Width, MaximumWidth));
                _Bounds_BeforeFullScreen_Height = Math.Max(MinimumHeight, Math.Min(value.Height, MaximumHeight));
            }
        }

        //

        // 更新窗口布局，并触发指定的事件。
        private void _UpdateLayout(UpdateLayoutEventType updateLayoutEventType)
        {
            if (_FormState == FormState.FullScreen)
            {
                _Client.Bounds = _SplashScreen.Bounds = new Rectangle(new Point(Bounds_Current_X, Bounds_Current_Y), new Size(Bounds_Current_Width, Bounds_Current_Height));
            }
            else
            {
                _Client.Bounds = _SplashScreen.Bounds = new Rectangle(new Point(Bounds_Current_X, Bounds_Current_Y + _CaptionBarHeight), new Size(Bounds_Current_Width, Bounds_Current_Height - _CaptionBarHeight));
                _CaptionBar.Bounds = new Rectangle(Bounds_Current_Location, new Size(Bounds_Current_Width, _CaptionBarHeight));
            }

            _Resizer.Bounds = new Rectangle(new Point(Bounds_Current_X - _ResizerSize, Bounds_Current_Y - _ResizerSize), new Size(Bounds_Current_Width + 2 * _ResizerSize, Bounds_Current_Height + 2 * _ResizerSize));

            //

            if (updateLayoutEventType.HasFlag(UpdateLayoutEventType.LocationChanged))
            {
                _OnLocationChanged();
            }
            else if (updateLayoutEventType.HasFlag(UpdateLayoutEventType.Move))
            {
                _OnMove();
            }

            if (updateLayoutEventType.HasFlag(UpdateLayoutEventType.SizeChanged))
            {
                _OnSizeChanged();
            }
            else if (updateLayoutEventType.HasFlag(UpdateLayoutEventType.Resize))
            {
                _OnResize();
            }
        }

        // 更新窗口布局，并触发指定的事件。
        internal void UpdateLayout(UpdateLayoutEventType updateLayoutEventType)
        {
            _UpdateLayout(updateLayoutEventType);
        }

        // 设置窗口的位置与大小，并以指定的行为更新窗口布局，触发指定的事件。
        private void _SetBoundsAndUpdateLayout(Rectangle bounds, UpdateLayoutBehavior updateLayoutBehavior, UpdateLayoutEventType updateLayoutEventType)
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
                if (_Effect.HasFlag(Effect.SmoothShift))
                {
                    int MaxLTRB = Math.Max(1, Math.Max(Math.Abs(bounds.Left - Bounds_Current.Left), Math.Max(Math.Abs(bounds.Top - Bounds_Current.Top), Math.Max(Math.Abs(bounds.Right - Bounds_Current.Right), Math.Abs(bounds.Bottom - Bounds_Current.Bottom)))));
                    int Delta = Math.Min(MaxLTRB, (int)(((PointD)PrimaryScreenBounds.Size).Module / 40.96));

                    Rectangle oldBounds = Rectangle.FromLTRB(bounds.Left - (bounds.Left - Bounds_Current.Left) * Delta / MaxLTRB, bounds.Top - (bounds.Top - Bounds_Current.Top) * Delta / MaxLTRB, bounds.Right - (bounds.Right - Bounds_Current.Right) * Delta / MaxLTRB, bounds.Bottom - (bounds.Bottom - Bounds_Current.Bottom) * Delta / MaxLTRB);

                    Animation.Frame Frame = (frameId, frameCount, msPerFrame) =>
                    {
                        double Progress = (frameId == frameCount ? 1 : 1 - Math.Pow(1 - (double)frameId / frameCount, 2));

                        Bounds_Current = new Rectangle(((PointD)oldBounds.Location * (1 - Progress) + (PointD)bounds.Location * Progress).ToPoint(), ((PointD)oldBounds.Size * (1 - Progress) + (PointD)bounds.Size * Progress).ToSize());

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
                else
                {
                    Bounds_Current = bounds;

                    _UpdateLayout(updateLayoutEventType);
                }
            }
        }

        //

        private Predicate<EventArgs> _ReturnVerification = null; // 用于验证是否允许窗口还原的方法。
        private Predicate<EventArgs> _MaximizeVerification = null; // 设置用于验证是否允许窗口最大化的方法。
        private Predicate<EventArgs> _EnterFullScreenVerification = null; // 用于验证是否允许窗口进入全屏幕模式的方法。
        private Predicate<EventArgs> _ExitFullScreenVerification = null; // 用于验证是否允许窗口退出全屏幕模式的方法。
        private Predicate<EventArgs> _CloseVerification = null; // 设置用于验证是否允许窗口关闭的方法。

        // 判断是否允许窗口还原。
        private bool _CanReturn()
        {
            return (_Initialized && (_FormState != FormState.FullScreen && _FormState != FormState.Normal) && (_ReturnVerification is null || (!(_ReturnVerification is null) && _ReturnVerification(EventArgs.Empty))));
        }

        // 判断是否允许窗口还原。
        internal bool CanReturn()
        {
            return _CanReturn();
        }

        // 判断是否允许窗口最小化。
        private bool _CanMinimize()
        {
            return (_Initialized && _EnableMinimize && _Client.WindowState != FormWindowState.Minimized);
        }

        // 判断是否允许窗口最大化。
        private bool _CanMaximize()
        {
            return (_Initialized && _EnableMaximize && _Client.WindowState != FormWindowState.Minimized && (_FormState != FormState.FullScreen && _FormState != FormState.Maximized) && (_MaximizeVerification is null || (!(_MaximizeVerification is null) && _MaximizeVerification(EventArgs.Empty))));
        }

        // 判断是否允许窗口进入全屏幕模式。
        private bool _CanEnterFullScreen()
        {
            return (_Initialized && _EnableFullScreen && _Client.WindowState != FormWindowState.Minimized && _FormState != FormState.FullScreen && (_EnterFullScreenVerification is null || (!(_EnterFullScreenVerification is null) && _EnterFullScreenVerification(EventArgs.Empty))));
        }

        // 判断是否允许窗口退出全屏幕模式。
        private bool _CanExitFullScreen()
        {
            return (_Initialized && _Client.WindowState != FormWindowState.Minimized && _FormState == FormState.FullScreen && (_ExitFullScreenVerification is null || (!(_ExitFullScreenVerification is null) && _ExitFullScreenVerification(EventArgs.Empty))));
        }

        // 判断是否允许窗口与桌面的高度相同。
        private bool _CanHighAsScreen()
        {
            return (_Initialized && _FormStyle == FormStyle.Sizable && _Client.WindowState != FormWindowState.Minimized && _FormState != FormState.FullScreen);
        }

        // 判断是否允许窗口占据桌面的四分之一区域。
        private bool _CanQuarterScreen()
        {
            return (_Initialized && _FormStyle == FormStyle.Sizable && _Client.WindowState != FormWindowState.Minimized && _FormState != FormState.FullScreen);
        }

        // 判断是否允许窗口关闭。
        private bool _CanClose()
        {
            return (_Initialized && (_CloseVerification is null || (!(_CloseVerification is null) && _CloseVerification(EventArgs.Empty))));
        }

        //

        private EventHandlerList _Events = new EventHandlerList(); // 窗口事件委托列表。

        // 引发窗口事件。
        private void _TrigEvent(object eventKey)
        {
            EventHandler Method = _Events[eventKey] as EventHandler;

            if (!(Method is null))
            {
                Method(this, EventArgs.Empty);
            }
        }

        // 引发 Loading 事件。
        private void _OnLoading()
        {
            _TrigEvent(EventKey.Loading);
        }

        // 引发 Loaded 事件。
        private void _OnLoaded()
        {
            _TrigEvent(EventKey.Loaded);
        }

        // 引发 Closing 事件。
        private void _OnClosing()
        {
            _TrigEvent(EventKey.Closing);
        }

        // 引发 Closed 事件。
        private void _OnClosed()
        {
            _TrigEvent(EventKey.Closed);
        }

        // 引发 Activated 事件。
        private void _OnActivated()
        {
            _TrigEvent(EventKey.Activated);
        }

        // 引发 Deactivate 事件。
        private void _OnDeactivate()
        {
            _TrigEvent(EventKey.Deactivate);
        }

        // 引发 Move 事件。
        private void _OnMove()
        {
            _TrigEvent(EventKey.Move);
        }

        // 引发 LocationChanged 事件。
        private void _OnLocationChanged()
        {
            _OnMove();

            _TrigEvent(EventKey.LocationChanged);
        }

        // 引发 Resize 事件。
        private void _OnResize()
        {
            _TrigEvent(EventKey.Resize);
        }

        // 引发 SizeChanged 事件。
        private void _OnSizeChanged()
        {
            _OnResize();

            _TrigEvent(EventKey.SizeChanged);
        }

        // 引发 FormStateChanged 事件。
        private void _OnFormStateChanged()
        {
            _TrigEvent(EventKey.FormStateChanged);
        }

        // 引发 EnabledChanged 事件。
        private void _OnEnabledChanged()
        {
            _TrigEvent(EventKey.EnabledChanged);
        }

        // 引发 VisibleChanged 事件。
        private void _OnVisibleChanged()
        {
            _TrigEvent(EventKey.VisibleChanged);
        }

        // 引发 OpacityChanged 事件。
        private void _OnOpacityChanged()
        {
            _TrigEvent(EventKey.OpacityChanged);
        }

        // 引发 CaptionChanged 事件。
        private void _OnCaptionChanged()
        {
            _TrigEvent(EventKey.CaptionChanged);
        }

        // 引发 ThemeChanged 事件。
        private void _OnThemeChanged()
        {
            _TrigEvent(EventKey.ThemeChanged);
        }

        // 引发 ThemeColorChanged 事件。
        private void _OnThemeColorChanged()
        {
            _TrigEvent(EventKey.ThemeColorChanged);
        }

        //

        // 使窗口还原至普通大小。
        private void _Return(UpdateLayoutBehavior updateLayoutBehavior, UpdateLayoutEventType updateLayoutEventType)
        {
            if (_Client.WindowState == FormWindowState.Minimized)
            {
                _Client.WindowState = FormWindowState.Normal;
            }
            else
            {
                _PreviousFormState = _FormState;
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
        }

        // 通过移动最大化、与桌面高度相同或占据桌面四分之一的窗口，使窗口还原至普通大小。
        internal void ReturnByMoveForm()
        {
            _PreviousFormState = _FormState;
            _FormState = FormState.Normal;

            Rectangle OldBounds = Bounds_Current;
            Bounds_Current = Bounds_Normal;

            _CaptionBar.OnFormStateChanged();
            _Resizer.OnFormStateChanged();

            Rectangle NewBounds = Bounds_Current;
            Bounds_Current = OldBounds;

            _SetBoundsAndUpdateLayout(NewBounds, UpdateLayoutBehavior.None, UpdateLayoutEventType.None);
        }

        // 使窗口由与桌面高度相同状态还原至普通大小。
        internal void ReturnFromHighAsScreen()
        {
            _PreviousFormState = _FormState;
            _FormState = FormState.Normal;

            Rectangle OldBounds = Bounds_Current;
            Bounds_Current_Y = Bounds_Normal_Y;
            Bounds_Current_Height = Bounds_Normal_Height;

            _CaptionBar.OnFormStateChanged();
            _Resizer.OnFormStateChanged();

            Rectangle NewBounds = Bounds_Current;
            Bounds_Current = OldBounds;

            _SetBoundsAndUpdateLayout(NewBounds, UpdateLayoutBehavior.Static, UpdateLayoutEventType.Result);

            _OnFormStateChanged();
        }

        // 使窗口最小化至任务栏。
        private void _Minimize()
        {
            if (_Client.WindowState != FormWindowState.Minimized)
            {
                _Client.WindowState = FormWindowState.Minimized;
            }
        }

        // 使窗口最大化。
        private void _Maximize(UpdateLayoutBehavior updateLayoutBehavior, UpdateLayoutEventType updateLayoutEventType)
        {
            _PreviousFormState = _FormState;
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

        // 使窗口进入全屏幕模式。
        private void _EnterFullScreen(UpdateLayoutBehavior updateLayoutBehavior, UpdateLayoutEventType updateLayoutEventType)
        {
            _FormState_BeforeFullScreen = _FormState;
            Bounds_BeforeFullScreen = Bounds_Current;

            _PreviousFormState = _FormState;
            _FormState = FormState.FullScreen;

            _Client.TopMost = true;

            Rectangle OldBounds = Bounds_Current;
            Bounds_Current = PrimaryScreenBounds;

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

        // 使窗口退出全屏幕模式。
        private void _ExitFullScreen(UpdateLayoutBehavior updateLayoutBehavior, UpdateLayoutEventType updateLayoutEventType)
        {
            if (_FormStyle != FormStyle.Sizable)
            {
                _FormState_BeforeFullScreen = FormState.Normal;
                Bounds_BeforeFullScreen = Bounds_Normal;
            }

            _PreviousFormState = _FormState;
            _FormState = _FormState_BeforeFullScreen;

            _Client.TopMost = _TopMost;

            Rectangle OldBounds = Bounds_Current;
            Bounds_Current = Bounds_BeforeFullScreen;

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

        // 使窗口占据桌面的左半区域。
        private void _LeftHalfScreen(UpdateLayoutBehavior updateLayoutBehavior, UpdateLayoutEventType updateLayoutEventType)
        {
            _PreviousFormState = _FormState;
            _FormState = FormState.HighAsScreen;

            Rectangle CurScrClient = PrimaryScreenClient;

            Rectangle OldBounds = Bounds_Current;
            Bounds_Current_Size = new Size(CurScrClient.Width / 2, CurScrClient.Height);
            Bounds_Current_Location = CurScrClient.Location;

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

        // 使窗口占据桌面的右半区域。
        private void _RightHalfScreen(UpdateLayoutBehavior updateLayoutBehavior, UpdateLayoutEventType updateLayoutEventType)
        {
            _PreviousFormState = _FormState;
            _FormState = FormState.HighAsScreen;

            Rectangle CurScrClient = PrimaryScreenClient;

            Rectangle OldBounds = Bounds_Current;
            Bounds_Current_Size = new Size(CurScrClient.Width / 2, CurScrClient.Height);
            Bounds_Current_Location = new Point(CurScrClient.Right - Bounds_Current_Width, CurScrClient.Y);

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

        // 使窗口与桌面的高度相同。
        private void _HighAsScreen(UpdateLayoutBehavior updateLayoutBehavior, UpdateLayoutEventType updateLayoutEventType)
        {
            _PreviousFormState = _FormState;
            _FormState = FormState.HighAsScreen;

            Rectangle CurScrClient = PrimaryScreenClient;

            Rectangle OldBounds = Bounds_Current;
            Bounds_Current_Height = CurScrClient.Height;
            Bounds_Current_Y = CurScrClient.Y;

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

        // 使窗口占据桌面的左上四分之一区域。
        private void _TopLeftQuarterScreen(UpdateLayoutBehavior updateLayoutBehavior, UpdateLayoutEventType updateLayoutEventType)
        {
            _PreviousFormState = _FormState;
            _FormState = FormState.QuarterScreen;

            Rectangle CurScrClient = PrimaryScreenClient;

            Rectangle OldBounds = Bounds_Current;
            Bounds_Current_Size = Bounds_QuarterScreen_Size = new Size(CurScrClient.Width / 2, CurScrClient.Height / 2);
            Bounds_Current_Location = Bounds_QuarterScreen_Location = CurScrClient.Location;

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

        // 使窗口占据桌面的右上四分之一区域。
        private void _TopRightQuarterScreen(UpdateLayoutBehavior updateLayoutBehavior, UpdateLayoutEventType updateLayoutEventType)
        {
            _PreviousFormState = _FormState;
            _FormState = FormState.QuarterScreen;

            Rectangle CurScrClient = PrimaryScreenClient;

            Rectangle OldBounds = Bounds_Current;
            Bounds_Current_Size = Bounds_QuarterScreen_Size = new Size(CurScrClient.Width / 2, CurScrClient.Height / 2);
            Bounds_Current_Location = Bounds_QuarterScreen_Location = new Point(CurScrClient.Right - Bounds_Current_Width, CurScrClient.Y);

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

        // 使窗口占据桌面的左下四分之一区域。
        private void _BottomLeftQuarterScreen(UpdateLayoutBehavior updateLayoutBehavior, UpdateLayoutEventType updateLayoutEventType)
        {
            _PreviousFormState = _FormState;
            _FormState = FormState.QuarterScreen;

            Rectangle CurScrClient = PrimaryScreenClient;

            Rectangle OldBounds = Bounds_Current;
            Bounds_Current_Size = Bounds_QuarterScreen_Size = new Size(CurScrClient.Width / 2, CurScrClient.Height / 2);
            Bounds_Current_Location = Bounds_QuarterScreen_Location = new Point(CurScrClient.X, CurScrClient.Bottom - Bounds_Current_Height);

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

        // 使窗口占据桌面的右下四分之一区域。
        private void _BottomRightQuarterScreen(UpdateLayoutBehavior updateLayoutBehavior, UpdateLayoutEventType updateLayoutEventType)
        {
            _PreviousFormState = _FormState;
            _FormState = FormState.QuarterScreen;

            Rectangle CurScrClient = PrimaryScreenClient;

            Rectangle OldBounds = Bounds_Current;
            Bounds_Current_Size = Bounds_QuarterScreen_Size = new Size(CurScrClient.Width / 2, CurScrClient.Height / 2);
            Bounds_Current_Location = Bounds_QuarterScreen_Location = new Point(CurScrClient.Right - Bounds_Current_Width, CurScrClient.Bottom - Bounds_Current_Height);

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

        // 关闭窗口。
        private void _Close()
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

                //

                _SplashScreen.Visible = _Visible;
                _Client.Visible = false;

                //

                _Client.ShowInTaskbar = false;

                //

                if (_Visible)
                {
                    double Opa = _Opacity;

                    if (_Effect.HasFlag(Effect.Fade))
                    {
                        Animation.Frame Frame = (frameId, frameCount, msPerFrame) =>
                        {
                            double Progress = (frameId == frameCount ? 1 : 1 - Math.Pow(1 - (double)frameId / frameCount, 2));

                            _Opacity = Opa * (1 - Progress);

                            _SplashScreen.Opacity = _Opacity;

                            if (_FormState != FormState.FullScreen)
                            {
                                _CaptionBar.Opacity = _Opacity * CaptionBarOpacityRatio;
                            }

                            _Resizer.OnThemeChanged();
                        };

                        Animation.Show(Frame, 9, 15);
                    }
                    else
                    {
                        _Opacity = 0;

                        _SplashScreen.Opacity = _Opacity;

                        if (_FormState != FormState.FullScreen)
                        {
                            _CaptionBar.Opacity = _Opacity * CaptionBarOpacityRatio;
                        }

                        _Resizer.OnThemeChanged();
                    }

                    _Opacity = Opa;
                }

                //

                _FormManagerList.Remove(this);

                //

                if (!(_Owner is null))
                {
                    _Owner._RemoveOwned(this);
                }

                //

                _Client.Close();
            }
        }

        // 通过 Alt + F4 或其他非正常方式关闭窗口。
        private void _AltF4(object sender, EventArgs e)
        {
            _ResolutionMonitor.Enabled = false;
            _ActivationMonitor.Enabled = false;

            //

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

            _FormManagerList.Remove(this);

            //

            if (!(_Owner is null))
            {
                _Owner._RemoveOwned(this);
            }

            //

            if (!object.ReferenceEquals(sender, _Client))
            {
                _Client.Close();
            }
        }

        //

        // 为以 Form 对象与 FormManager 对象为参数的构造函数提供实现。
        private void _Ctor(Form client, FormManager owner)
        {
            if (client is null)
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

            if (!(owner is null) && !object.ReferenceEquals(_Owner, owner))
            {
                _Owner = owner;
            }
            else
            {
                _Owner = null;
            }

            if (!(_Owner is null))
            {
                _Owner._AddOwned(this);

                //

                _Client.Owner = _Owner._Client;

                //

                _Effect = _Owner._Effect;

                _Opacity = _Owner._Opacity;
                _ShowCaption = _Owner._ShowCaption;
                _CaptionFont = _Owner._CaptionFont;
                _CaptionAlign = _Owner._CaptionAlign;
                _Theme = _Owner._Theme;
                _ThemeColor = _Owner._ThemeColor;
                _ShowCaptionBarColor = _Owner._ShowCaptionBarColor;
                _EnableCaptionBarTransparent = _Owner._EnableCaptionBarTransparent;
                _ShowShadowColor = _Owner._ShowShadowColor;
            }

            //

            _Client.FormBorderStyle = FormBorderStyle.None;
            _Client.WindowState = FormWindowState.Normal;

            //

            _Client.Opacity = _CaptionBar.Opacity = _Resizer.Opacity = _SplashScreen.Opacity = 0;

            //

            _RecommendColors = new RecommendColors(_Theme, _ThemeColor, _ShowCaptionBarColor, _ShowShadowColor, _IsActive);

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
            _Client.Activated += Client_Activated;
            _Client.Deactivate += Client_Deactivate;

            _CaptionBar.Closed += CaptionBar_Closed;
            _CaptionBar.Activated += CaptionBar_Activated;
            _CaptionBar.Deactivate += CaptionBar_Deactivate;

            _Resizer.Closed += Resizer_Closed;
            _Resizer.Activated += Resizer_Activated;
            _Resizer.Deactivate += Resizer_Deactivate;

            _SplashScreen.Closed += SplashScreen_Closed;
            _SplashScreen.Activated += SplashScreen_Activated;
            _SplashScreen.Deactivate += SplashScreen_Deactivate;

            //

            _LoadingNow = true;
        }

        #endregion

        #region 回调函数

        // _Client 的 Load 事件的回调函数。
        private void Client_Load(object sender, EventArgs e)
        {
            _Client.Visible = false;

            //

            _CaptionBar.Show();
            _Resizer.Show();
            _SplashScreen.Show();

            if (!_Visible)
            {
                _CaptionBar.Visible = _Resizer.Visible = _SplashScreen.Visible = false;
            }

            //

            if (Width == int.MinValue || Height == int.MinValue)
            {
                if (Width == int.MinValue && Height == int.MinValue)
                {
                    ClientSize = _Client.Size;
                }
                else if (Width == int.MinValue)
                {
                    ClientSize = new Size(_Client.Width, Height);
                }
                else if (Height == int.MinValue)
                {
                    ClientSize = new Size(Width, _Client.Height);
                }
            }

            Rectangle CurScrClient = PrimaryScreenClient;

            Rectangle Bounds_Screen = CurScrClient;
            Rectangle Bounds_Parent = (_Owner is null ? CurScrClient : _Owner.Bounds);

            switch (_Client.StartPosition)
            {
                case FormStartPosition.Manual:
                    if (X == int.MinValue || Y == int.MinValue)
                    {
                        if (X == int.MinValue && Y == int.MinValue)
                        {
                            Location = _Client.Location;
                        }
                        else if (X == int.MinValue)
                        {
                            X = _Client.Left;
                        }
                        else if (Y == int.MinValue)
                        {
                            Y = _Client.Top;
                        }
                    }
                    break;

                case FormStartPosition.CenterScreen:
                    Location = new Point(Bounds_Screen.X + (Bounds_Screen.Width - Width) / 2, Bounds_Screen.Y + (Bounds_Screen.Height - Height) / 2);
                    break;

                case FormStartPosition.WindowsDefaultLocation:
                case FormStartPosition.WindowsDefaultBounds:
                    Location = new Point(Statistics.RandomInteger(Bounds_Screen.X, Bounds_Screen.Right - Width), Statistics.RandomInteger(Bounds_Screen.Y, Bounds_Screen.Bottom - Height));
                    break;

                case FormStartPosition.CenterParent:
                    Location = new Point(Bounds_Parent.X + (Bounds_Parent.Width - Width) / 2, Bounds_Parent.Y + (Bounds_Parent.Height - Height) / 2);
                    break;
            }

            _UpdateLayout(UpdateLayoutEventType.None);

            //

            if (_Caption is null)
            {
                if (!string.IsNullOrEmpty(_Client.Text))
                {
                    _Caption = _Client.Text;
                }
                else if (_IsMainForm)
                {
                    _Caption = Application.ProductName;
                }
                else
                {
                    _Caption = string.Empty;
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

            _CaptionBar.OnLoading();

            //

            _Client.TopMost = _TopMost;
            _Client.ShowInTaskbar = _ShowInTaskbar;

            //

            _Client.Enabled = _CaptionBar.Enabled = _Resizer.Enabled = _SplashScreen.Enabled = _Enabled;

            //

            _Client.BackColor = RecommendColors.FormBackground.ToColor();

            //

            _Resizer.Opacity = 1;
            _Resizer.OnThemeChanged();

            //

            _CaptionBar.BringToFront();
            _Client.BringToFront();
            _SplashScreen.BringToFront();
            _SplashScreen.Focus();

            //

            try
            {
                if (_EnableMinimize)
                {
                    User32.SetWindowLongA(_Client.Handle.ToInt32(), -16, User32.GetWindowLongA(_Client.Handle.ToInt32(), -16) | 131072);
                }
                else
                {
                    User32.SetWindowLongA(_Client.Handle.ToInt32(), -16, User32.GetWindowLongA(_Client.Handle.ToInt32(), -16) & (~131072));
                }
            }
            catch { }

            //

            if (_EnableMaximize && _FormState == FormState.Maximized)
            {
                _Maximize(UpdateLayoutBehavior.Static, UpdateLayoutEventType.None);
            }
            else if (_EnableFullScreen && _FormState == FormState.FullScreen)
            {
                _FormState = FormState.Normal;

                _EnterFullScreen(UpdateLayoutBehavior.Static, UpdateLayoutEventType.None);
            }
            else if (_FormStyle != FormStyle.Sizable && (_FormState != FormState.Normal && _FormState != FormState.Maximized && _FormState != FormState.FullScreen))
            {
                _Return(UpdateLayoutBehavior.Static, UpdateLayoutEventType.None);
            }

            //

            if (_Visible)
            {
                if (_Effect.HasFlag(Effect.Fade))
                {
                    double Opa = _Opacity;

                    Animation.Frame Frame = (frameId, frameCount, msPerFrame) =>
                    {
                        double Progress = (frameId == frameCount ? 1 : 1 - Math.Pow(1 - (double)frameId / frameCount, 2));

                        _Opacity = Opa * Progress;

                        _Client.Opacity = _SplashScreen.Opacity = _Opacity;

                        if (_FormState != FormState.FullScreen)
                        {
                            _CaptionBar.Opacity = _Opacity * CaptionBarOpacityRatio;
                        }

                        _Resizer.OnThemeChanged();
                    };

                    Animation.Show(Frame, 9, 15);
                }
                else
                {
                    _Client.Opacity = _SplashScreen.Opacity = _Opacity;

                    if (_FormState != FormState.FullScreen)
                    {
                        _CaptionBar.Opacity = _Opacity * CaptionBarOpacityRatio;
                    }

                    _Resizer.OnThemeChanged();
                }
            }
            else
            {
                _Client.Opacity = _SplashScreen.Opacity = _Opacity;

                if (_FormState != FormState.FullScreen)
                {
                    _CaptionBar.Opacity = _Opacity * CaptionBarOpacityRatio;
                }

                _Resizer.OnThemeChanged();
            }

            //

            _SplashScreen.OnLoading();

            //

            _Initialized = true;

            //

            _ResolutionMonitor = new Timer();
            _ResolutionMonitor.Interval = 200;
            _ResolutionMonitor.Tick += ResolutionMonitor_Tick;
            _ResolutionMonitor.Enabled = true;

            //

            _ActivationMonitor = new Timer();
            _ActivationMonitor.Interval = 200;
            _ActivationMonitor.Tick += ActivationMonitor_Tick;
            _ActivationMonitor.Enabled = true;

            //

            _FormLoadingAsyncWorker.RunWorkerAsync();
        }

        // _Client 的 Closed 事件的回调函数。
        private void Client_Closed(object sender, EventArgs e)
        {
            _AltF4(sender, e);
        }

        // _Client 的 SizeChanged 事件的回调函数。
        private void Client_SizeChanged(object sender, EventArgs e)
        {
            if (_Client.WindowState == FormWindowState.Maximized || (!_EnableMinimize && _Client.WindowState == FormWindowState.Minimized))
            {
                _Client.WindowState = FormWindowState.Normal;
            }

            //

            if (_PreviousFormWindowState != _Client.WindowState)
            {
                FormWindowState _FormWindowState = _Client.WindowState;

                if ((_PreviousFormWindowState != FormWindowState.Minimized && _FormWindowState == FormWindowState.Minimized) || (_PreviousFormWindowState == FormWindowState.Minimized && _FormWindowState != FormWindowState.Minimized))
                {
                    _OnFormStateChanged();
                }

                _PreviousFormWindowState = _FormWindowState;
            }
        }

        // _CaptionBar 的 Closed 事件的回调函数。
        private void CaptionBar_Closed(object sender, EventArgs e)
        {
            _AltF4(sender, e);
        }

        // _Resizer 的 Closed 事件的回调函数。
        private void Resizer_Closed(object sender, EventArgs e)
        {
            _AltF4(sender, e);
        }

        // _SplashScreen 的 Closed 事件的回调函数。
        private void SplashScreen_Closed(object sender, EventArgs e)
        {
            _AltF4(sender, e);
        }

        //

        // _FormLoadingAsyncWorker 的 DoWork 事件的回调函数。
        private void FormLoadingAsyncWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            _OnLoading();
        }

        // _FormLoadingAsyncWorker 的 RunWorkerCompleted 事件的回调函数。
        private void FormLoadingAsyncWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _Client.Visible = _Visible;
            _SplashScreen.Visible = false;

            //

            _OnLoaded();

            //

            _LoadingNow = false;
        }

        // _FormClosingAsyncWorker 的 DoWork 事件的回调函数。
        private void FormClosingAsyncWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (!_FormLoadingAsyncWorker.IsBusy)
            {
                _OnClosing();
            }
        }

        // _FormClosingAsyncWorker 的 RunWorkerCompleted 事件的回调函数。
        private void FormClosingAsyncWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _ResolutionMonitor.Enabled = false;
            _ActivationMonitor.Enabled = false;

            //

            _SplashScreen.OnClosing();

            //

            _SplashScreen.Visible = _Visible;
            _Client.Visible = false;

            //

            _Client.ShowInTaskbar = false;

            //

            if (_Visible)
            {
                double Opa = _Opacity;

                if (_Effect.HasFlag(Effect.Fade))
                {
                    Animation.Frame Frame = (frameId, frameCount, msPerFrame) =>
                    {
                        double Progress = (frameId == frameCount ? 1 : 1 - Math.Pow(1 - (double)frameId / frameCount, 2));

                        _Opacity = Opa * (1 - Progress);

                        _SplashScreen.Opacity = _Opacity;

                        if (_FormState != FormState.FullScreen)
                        {
                            _CaptionBar.Opacity = _Opacity * CaptionBarOpacityRatio;
                        }

                        _Resizer.OnThemeChanged();
                    };

                    Animation.Show(Frame, 9, 15);
                }
                else
                {
                    _Opacity = 0;

                    _SplashScreen.Opacity = _Opacity;

                    if (_FormState != FormState.FullScreen)
                    {
                        _CaptionBar.Opacity = _Opacity * CaptionBarOpacityRatio;
                    }

                    _Resizer.OnThemeChanged();
                }

                _Opacity = Opa;
            }

            //

            _OnClosed();

            //

            _FormManagerList.Remove(this);

            //

            if (!(_Owner is null))
            {
                _Owner._RemoveOwned(this);
            }

            //

            _Client.Close();
        }

        //

        // _ResolutionMonitor 的 Tick 事件的回调函数。
        private void ResolutionMonitor_Tick(object sender, EventArgs e)
        {
            Rectangle CurScrClient = PrimaryScreenClient;

            if (_PreviousPrimaryScreenClient != CurScrClient)
            {
                UpdateLayoutEventType _UpdateLayoutEventType = UpdateLayoutEventType.None;

                if (!_LoadingNow && !_ClosingNow)
                {
                    _UpdateLayoutEventType = UpdateLayoutEventType.Result;
                }

                switch (_FormState)
                {
                    case FormState.FullScreen:
                        {
                            Rectangle OldBounds = Bounds_Current;
                            Bounds_Current = PrimaryScreenBounds;

                            _CaptionBar.OnFormStateChanged();

                            Rectangle NewBounds = Bounds_Current;
                            Bounds_Current = OldBounds;

                            _SetBoundsAndUpdateLayout(NewBounds, UpdateLayoutBehavior.Static, _UpdateLayoutEventType);

                            //

                            switch (_FormState_BeforeFullScreen)
                            {
                                case FormState.Maximized:
                                    {
                                        Bounds_BeforeFullScreen = CurScrClient;
                                    }
                                    break;

                                case FormState.HighAsScreen:
                                    {
                                        Bounds_BeforeFullScreen_Width = Math.Min(CurScrClient.Width, Bounds_BeforeFullScreen_Width);
                                        Bounds_BeforeFullScreen_Height = CurScrClient.Height;
                                        Bounds_BeforeFullScreen_Y = CurScrClient.Y;
                                    }
                                    break;

                                case FormState.QuarterScreen:
                                    {
                                        Bounds_BeforeFullScreen_Width = Bounds_QuarterScreen_Width = Math.Min(CurScrClient.Width, Bounds_BeforeFullScreen_Width);
                                        Bounds_BeforeFullScreen_Height = Bounds_QuarterScreen_Height = Math.Min(CurScrClient.Width, Bounds_BeforeFullScreen_Height);
                                    }
                                    break;

                                case FormState.Normal:
                                    {
                                        Bounds_BeforeFullScreen_Width = Math.Min(CurScrClient.Width, Bounds_BeforeFullScreen_Width);
                                        Bounds_BeforeFullScreen_Height = Math.Min(CurScrClient.Width, Bounds_BeforeFullScreen_Height);
                                    }
                                    break;
                            }

                            Bounds_BeforeFullScreen_Y = Math.Max(CurScrClient.Y, Bounds_BeforeFullScreen_Y);
                        }
                        break;

                    case FormState.Maximized:
                        {
                            Rectangle OldBounds = Bounds_Current;
                            Bounds_Current = CurScrClient;

                            Rectangle NewBounds = Bounds_Current;
                            Bounds_Current = OldBounds;

                            _SetBoundsAndUpdateLayout(NewBounds, UpdateLayoutBehavior.Static, _UpdateLayoutEventType);
                        }
                        break;

                    case FormState.HighAsScreen:
                        {
                            Rectangle OldBounds = Bounds_Current;
                            Bounds_Current_Width = Math.Min(CurScrClient.Width, Bounds_Current_Width);
                            Bounds_Current_Height = CurScrClient.Height;
                            Bounds_Current_Y = CurScrClient.Y;

                            Rectangle NewBounds = Bounds_Current;
                            Bounds_Current = OldBounds;

                            _SetBoundsAndUpdateLayout(NewBounds, UpdateLayoutBehavior.Static, _UpdateLayoutEventType);
                        }
                        break;

                    case FormState.QuarterScreen:
                        {
                            Rectangle OldBounds = Bounds_Current;
                            Bounds_Current_Width = Bounds_QuarterScreen_Width = Math.Min(CurScrClient.Width, Bounds_Current_Width);
                            Bounds_Current_Height = Bounds_QuarterScreen_Height = Math.Min(CurScrClient.Width, Bounds_Current_Height);
                            Bounds_Current_Y = Bounds_QuarterScreen_Y = Math.Max(CurScrClient.Y, Bounds_Current_Y);

                            Rectangle NewBounds = Bounds_Current;
                            Bounds_Current = OldBounds;

                            _SetBoundsAndUpdateLayout(NewBounds, UpdateLayoutBehavior.Static, _UpdateLayoutEventType);
                        }
                        break;

                    case FormState.Normal:
                        {
                            Rectangle OldBounds = Bounds_Current;
                            Bounds_Current_Width = Math.Min(CurScrClient.Width, Bounds_Current_Width);
                            Bounds_Current_Height = Math.Min(CurScrClient.Width, Bounds_Current_Height);
                            Bounds_Current_Y = Math.Max(CurScrClient.Y, Bounds_Current_Y);

                            Rectangle NewBounds = Bounds_Current;
                            Bounds_Current = OldBounds;

                            _SetBoundsAndUpdateLayout(NewBounds, UpdateLayoutBehavior.Static, _UpdateLayoutEventType);
                        }
                        break;
                }

                Bounds_Normal_Width = Math.Min(CurScrClient.Width, Bounds_Normal_Width);
                Bounds_Normal_Height = Math.Min(CurScrClient.Width, Bounds_Normal_Height);
                Bounds_Normal_Y = Math.Max(CurScrClient.Y, Bounds_Normal_Y);

                _PreviousPrimaryScreenClient = CurScrClient;
            }
        }

        //

        // _Client 的 Activated 事件的回调函数。
        private void Client_Activated(object sender, EventArgs e)
        {
            _ClientIsActive = true;
        }

        // _Client 的 Deactivate 事件的回调函数。
        private void Client_Deactivate(object sender, EventArgs e)
        {
            _ClientIsActive = false;
        }

        // _CaptionBar 的 Activated 事件的回调函数。
        private void CaptionBar_Activated(object sender, EventArgs e)
        {
            _CaptionBarIsActive = true;
        }

        // _CaptionBar 的 Deactivate 事件的回调函数。
        private void CaptionBar_Deactivate(object sender, EventArgs e)
        {
            _CaptionBarIsActive = false;
        }

        // _Resizer 的 Activated 事件的回调函数。
        private void Resizer_Activated(object sender, EventArgs e)
        {
            _ResizerIsActive = true;
        }

        // _Resizer 的 Deactivate 事件的回调函数。
        private void Resizer_Deactivate(object sender, EventArgs e)
        {
            _ResizerIsActive = false;
        }

        // _SplashScreen 的 Activated 事件的回调函数。
        private void SplashScreen_Activated(object sender, EventArgs e)
        {
            _SplashScreenIsActive = true;
        }

        // _SplashScreen 的 Deactivate 事件的回调函数。
        private void SplashScreen_Deactivate(object sender, EventArgs e)
        {
            _SplashScreenIsActive = false;
        }

        // _ActivationMonitor 的 Tick 事件的回调函数。
        private void ActivationMonitor_Tick(object sender, EventArgs e)
        {
            bool Active = (_ClientIsActive || _CaptionBarIsActive || _ResizerIsActive || _SplashScreenIsActive || _MainMenuIsActive);

            if (_IsActive != Active)
            {
                _IsActive = Active;

                if (_FormState != FormState.FullScreen)
                {
                    _CaptionBar.Opacity = _Opacity * CaptionBarOpacityRatio;
                }

                _RecommendColors = new RecommendColors(_Theme, _ThemeColor, _ShowCaptionBarColor, _ShowShadowColor, _IsActive);

                if (_IsActive)
                {
                    _CaptionBar.OnActivated();
                }
                else
                {
                    _CaptionBar.OnDeactivate();
                }

                _CaptionBar.OnThemeChanged();
                _Resizer.OnThemeChanged();

                if (_Initialized)
                {
                    if (_IsActive)
                    {
                        _OnActivated();
                    }
                    else
                    {
                        _OnDeactivate();
                    }
                }
            }
        }

        #endregion

        #region 构造函数

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

        #endregion

        #region 属性

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
        /// 获取表示此窗口拥有的所有窗口的窗口管理器数组。
        /// </summary>
        public FormManager[] Owned
        {
            get
            {
                return _Owned.ToArray();
            }
        }

        //

        /// <summary>
        /// 获取表示窗口是否处于激活状态的布尔值。
        /// </summary>
        public bool IsActive
        {
            get
            {
                return _IsActive;
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
                if (_FormStyle != value)
                {
                    _FormStyle = value;

                    if (_Initialized)
                    {
                        Action Method = () =>
                        {
                            _CaptionBar.OnFormStyleChanged();
                            _Resizer.OnFormStyleChanged();

                            if (_FormStyle != FormStyle.Sizable && (_FormState != FormState.Normal && _FormState != FormState.Maximized && _FormState != FormState.FullScreen))
                            {
                                _Return(UpdateLayoutBehavior.Animate, UpdateLayoutEventType.Result);
                            }
                        };

                        _Client.Invoke(Method);
                    }
                }
            }
        }

        /// <summary>
        /// 获取或设置表示是否允许窗口最小化的布尔值。
        /// </summary>
        public bool EnableMinimize
        {
            get
            {
                return _EnableMinimize;
            }

            set
            {
                if (_EnableMinimize != value)
                {
                    _EnableMinimize = value;

                    if (_Initialized)
                    {
                        Action Method = () =>
                        {
                            try
                            {
                                if (_EnableMinimize)
                                {
                                    User32.SetWindowLongA(_Client.Handle.ToInt32(), -16, User32.GetWindowLongA(_Client.Handle.ToInt32(), -16) | 131072);
                                }
                                else
                                {
                                    User32.SetWindowLongA(_Client.Handle.ToInt32(), -16, User32.GetWindowLongA(_Client.Handle.ToInt32(), -16) & (~131072));
                                }
                            }
                            catch { }

                            _CaptionBar.OnFormStyleChanged();

                            if (!_EnableMinimize && _Client.WindowState == FormWindowState.Minimized)
                            {
                                _Return(UpdateLayoutBehavior.None, UpdateLayoutEventType.None);
                            }
                        };

                        _Client.Invoke(Method);
                    }
                }
            }
        }

        /// <summary>
        /// 获取或设置表示是否允许窗口最大化的布尔值。
        /// </summary>
        public bool EnableMaximize
        {
            get
            {
                return _EnableMaximize;
            }

            set
            {
                if (_EnableMaximize != value)
                {
                    _EnableMaximize = value;

                    if (_Initialized)
                    {
                        Action Method = () =>
                        {
                            _CaptionBar.OnFormStyleChanged();

                            if (!_EnableMaximize && _FormState == FormState.Maximized)
                            {
                                _Return(UpdateLayoutBehavior.Animate, UpdateLayoutEventType.Result);
                            }
                        };

                        _Client.Invoke(Method);
                    }
                    else
                    {
                        if (!_EnableMaximize && _FormState == FormState.Maximized)
                        {
                            _PreviousFormState = _FormState;
                            _FormState = FormState.Normal;
                        }
                    }
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
                if (_EnableFullScreen != value)
                {
                    _EnableFullScreen = value;

                    if (_Initialized)
                    {
                        Action Method = () =>
                        {
                            _CaptionBar.OnFormStyleChanged();

                            if (!_EnableFullScreen && _FormState == FormState.FullScreen)
                            {
                                _ExitFullScreen(UpdateLayoutBehavior.Animate, UpdateLayoutEventType.Result);
                            }
                        };

                        _Client.Invoke(Method);
                    }
                    else
                    {
                        if (!_EnableFullScreen && _FormState == FormState.FullScreen)
                        {
                            _PreviousFormState = _FormState;
                            _FormState = FormState.Normal;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取或设置表示是否在窗口标题栏上显示图标的布尔值。
        /// </summary>
        public bool ShowIconOnCaptionBar
        {
            get
            {
                return _ShowIconOnCaptionBar;
            }

            set
            {
                if (_ShowIconOnCaptionBar != value)
                {
                    _ShowIconOnCaptionBar = value;

                    if (_Initialized)
                    {
                        Action Method = () =>
                        {
                            _CaptionBar.OnFormStyleChanged();
                        };

                        _Client.Invoke(Method);
                    }
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
                if (_ShowInTaskbar != value)
                {
                    _ShowInTaskbar = value;

                    if (_Initialized)
                    {
                        Action Method = () =>
                        {
                            _Client.ShowInTaskbar = _ShowInTaskbar;

                            _CaptionBar.OnFormStyleChanged();

                            _Resizer.BringToFront();
                            _CaptionBar.BringToFront();
                            _Client.BringToFront();
                            _Client.Focus();

                            _UpdateLayout(UpdateLayoutEventType.None);
                        };

                        _Client.Invoke(Method);
                    }
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
                if (_TopMost != value)
                {
                    _TopMost = value;

                    if (_Initialized)
                    {
                        Action Method = () =>
                        {
                            _Client.TopMost = _TopMost;
                        };

                        _Client.Invoke(Method);
                    }
                }
            }
        }

        //

        /// <summary>
        /// 获取或设置窗口交互过程显示的效果。
        /// </summary>
        public Effect Effect
        {
            get
            {
                return _Effect;
            }

            set
            {
                _Effect = value;
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
                return new Size(MinimumWidth, MinimumHeight);
            }

            set
            {
                MinimumWidth = Math.Max(0, value.Width);
                MinimumHeight = Math.Max(0, value.Height);

                if (_Initialized)
                {
                    if (Bounds_Current_Width < MinimumWidth || Bounds_Current_Height < MinimumHeight)
                    {
                        Bounds_Current_Width = Math.Max(MinimumWidth, Bounds_Current_Width);
                        Bounds_Current_Height = Math.Max(MinimumHeight, Bounds_Current_Height);

                        Action Method = () =>
                        {
                            _UpdateLayout(UpdateLayoutEventType.SizeChanged);
                        };

                        _Client.Invoke(Method);
                    }
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
                return new Size(MaximumWidth, MaximumHeight);
            }

            set
            {
                MaximumWidth = Math.Max(0, value.Width);
                MaximumHeight = Math.Max(0, value.Height);

                if (_Initialized)
                {
                    if (Bounds_Current_Width > MaximumWidth || Bounds_Current_Height > MaximumHeight)
                    {
                        Bounds_Current_Width = Math.Min(MaximumWidth, Bounds_Current_Width);
                        Bounds_Current_Height = Math.Min(MaximumHeight, Bounds_Current_Height);

                        Action Method = () =>
                        {
                            _UpdateLayout(UpdateLayoutEventType.SizeChanged);
                        };

                        _Client.Invoke(Method);
                    }
                }
            }
        }

        //

        /// <summary>
        /// 获取或设置窗口标题栏的高度。
        /// </summary>
        public int CaptionBarHeight
        {
            get
            {
                return _CaptionBarHeight;
            }

            set
            {
                _CaptionBarHeight = Math.Max(24, Math.Min(value, PrimaryScreenBounds.Height));

                if (_Initialized)
                {
                    Action Method = () =>
                    {
                        _UpdateLayout(UpdateLayoutEventType.SizeChanged);
                    };

                    _Client.Invoke(Method);
                }
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
                if (_Enabled != value && (_Owned.Count <= 0 || (_Owned.Count > 0 && value == false)))
                {
                    _Enabled = value;

                    if (_Initialized)
                    {
                        Action Method = () =>
                        {
                            _Client.Enabled = _CaptionBar.Enabled = _Resizer.Enabled = _SplashScreen.Enabled = _Enabled;

                            _OnEnabledChanged();
                        };

                        _Client.Invoke(Method);
                    }
                }
            }
        }

        /// <summary>
        /// 获取或设置表示窗口是否可见的布尔值。
        /// </summary>
        public bool Visible
        {
            get
            {
                return _Visible;
            }

            set
            {
                if (_Visible != value)
                {
                    _Visible = value;

                    if (_Initialized)
                    {
                        Action Method = () =>
                        {
                            if (!_LoadingNow && !_ClosingNow)
                            {
                                _Client.Visible = _CaptionBar.Visible = _Resizer.Visible = _Visible;
                            }
                            else
                            {
                                _CaptionBar.Visible = _Resizer.Visible = _SplashScreen.Visible = _Visible;
                            }

                            _OnVisibleChanged();
                        };

                        _Client.Invoke(Method);
                    }
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
                double Opa = 1;

                if (InternalMethod.IsNaNOrInfinity(value))
                {
                    Opa = 1;
                }
                else
                {
                    Opa = Math.Max(0, Math.Min(value, 100));

                    if (Opa > 1)
                    {
                        Opa /= 100;
                    }
                }

                if (_Opacity != Opa)
                {
                    _Opacity = Opa;

                    if (_Initialized)
                    {
                        Action Method = () =>
                        {
                            _Client.Opacity = _SplashScreen.Opacity = _Opacity;

                            if (_FormState != FormState.FullScreen)
                            {
                                _CaptionBar.Opacity = _Opacity * CaptionBarOpacityRatio;
                            }

                            _Resizer.OnThemeChanged();

                            _OnOpacityChanged();
                        };

                        _Client.Invoke(Method);
                    }
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
                return (_Caption is null ? string.Empty : _Caption);
            }

            set
            {
                string Cap = (value is null ? string.Empty : value);

                if (_Caption != Cap)
                {
                    _Caption = Cap;

                    if (_Initialized)
                    {
                        Action Method = () =>
                        {
                            _Client.Text = _Caption;

                            if (_ShowCaption)
                            {
                                _CaptionBar.OnCaptionChanged();
                            }

                            _OnCaptionChanged();
                        };

                        _Client.Invoke(Method);
                    }
                }
            }
        }

        /// <summary>
        /// 获取或设置表示是否在窗口标题栏上显示窗口的标题的布尔值。
        /// </summary>
        public bool ShowCaption
        {
            get
            {
                return _ShowCaption;
            }

            set
            {
                if (_ShowCaption != value)
                {
                    _ShowCaption = value;

                    if (_Initialized)
                    {
                        Action Method = () =>
                        {
                            _CaptionBar.OnCaptionChanged();
                        };

                        _Client.Invoke(Method);
                    }
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
                if (!(value is null) && !_CaptionFont.Equals(value))
                {
                    _CaptionFont = value;

                    if (_Initialized)
                    {
                        if (_ShowCaption)
                        {
                            Action Method = () =>
                            {
                                _CaptionBar.OnCaptionChanged();
                            };

                            _Client.Invoke(Method);
                        }
                    }
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
                if (_CaptionAlign != value)
                {
                    _CaptionAlign = value;

                    if (_Initialized)
                    {
                        if (_ShowCaption)
                        {
                            Action Method = () =>
                            {
                                _CaptionBar.OnCaptionChanged();
                            };

                            _Client.Invoke(Method);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取或设置窗口标题栏的背景图像。
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
                    Action Method = () =>
                    {
                        _CaptionBar.OnCaptionChanged();
                    };

                    _Client.Invoke(Method);
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
                if (_Theme != value)
                {
                    _Theme = value;

                    _RecommendColors = new RecommendColors(_Theme, _ThemeColor, _ShowCaptionBarColor, _ShowShadowColor, _IsActive);

                    if (_Initialized)
                    {
                        Action Method = () =>
                        {
                            _Client.BackColor = RecommendColors.FormBackground.ToColor();

                            _CaptionBar.OnThemeChanged();
                            _Resizer.OnThemeChanged();
                            _SplashScreen.OnThemeChanged();

                            _OnThemeChanged();
                        };

                        _Client.Invoke(Method);
                    }
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
                if (!value.IsEmpty && !value.IsTransparent && !_ThemeColor.Equals(value))
                {
                    _ThemeColor = value;

                    _RecommendColors = new RecommendColors(_Theme, _ThemeColor, _ShowCaptionBarColor, _ShowShadowColor, _IsActive);

                    if (_Initialized)
                    {
                        Action Method = () =>
                        {
                            _Client.BackColor = RecommendColors.FormBackground.ToColor();

                            _CaptionBar.OnThemeChanged();
                            _Resizer.OnThemeChanged();
                            _SplashScreen.OnThemeChanged();

                            _OnThemeColorChanged();
                        };

                        _Client.Invoke(Method);
                    }
                }
            }
        }

        /// <summary>
        /// 获取或设置表示是否在窗口标题栏上显示主题色的布尔值。
        /// </summary>
        public bool ShowCaptionBarColor
        {
            get
            {
                return _ShowCaptionBarColor;
            }

            set
            {
                if (_ShowCaptionBarColor != value)
                {
                    _ShowCaptionBarColor = value;

                    _RecommendColors = new RecommendColors(_Theme, _ThemeColor, _ShowCaptionBarColor, _ShowShadowColor, _IsActive);

                    if (_Initialized)
                    {
                        Action Method = () =>
                        {
                            _CaptionBar.OnThemeChanged();
                        };

                        _Client.Invoke(Method);
                    }
                }
            }
        }

        /// <summary>
        /// 获取或设置表示是否允许以半透明方式显示窗口标题栏的布尔值。
        /// </summary>
        public bool EnableCaptionBarTransparent
        {
            get
            {
                return _EnableCaptionBarTransparent;
            }

            set
            {
                if (_EnableCaptionBarTransparent != value)
                {
                    _EnableCaptionBarTransparent = value;

                    if (_Initialized)
                    {
                        if (_FormState != FormState.FullScreen)
                        {
                            Action Method = () =>
                            {
                                _CaptionBar.Opacity = _Opacity * CaptionBarOpacityRatio;
                            };

                            _Client.Invoke(Method);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取或设置表示是否在窗口阴影显示主题色的布尔值。
        /// </summary>
        public bool ShowShadowColor
        {
            get
            {
                return _ShowShadowColor;
            }

            set
            {
                if (_ShowShadowColor != value)
                {
                    _ShowShadowColor = value;

                    _RecommendColors = new RecommendColors(_Theme, _ThemeColor, _ShowCaptionBarColor, _ShowShadowColor, _IsActive);

                    if (_Initialized)
                    {
                        Action Method = () =>
                        {
                            _Resizer.OnThemeChanged();
                        };

                        _Client.Invoke(Method);
                    }
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
                else
                {
                    return _FormState;
                }
            }

            set
            {
                if (_Initialized)
                {
                    Action Method = () =>
                    {
                        if (_Client.WindowState != FormWindowState.Minimized && value == FormState.Minimized)
                        {
                            Minimize();
                        }
                        else if (_Client.WindowState == FormWindowState.Minimized && (value == FormState.Normal || value == _FormState))
                        {
                            Return();
                        }
                        else if (_FormState != FormState.Normal && value == FormState.Normal)
                        {
                            Return();
                        }
                        else if (_FormState != FormState.Maximized && value == FormState.Maximized)
                        {
                            Maximize();
                        }
                        else if (_FormState != FormState.FullScreen && value == FormState.FullScreen)
                        {
                            EnterFullScreen();
                        }
                        else if (_FormState == FormState.FullScreen && (value == FormState.Normal || value == _FormState_BeforeFullScreen))
                        {
                            ExitFullScreen();
                        }
                    };

                    _Client.Invoke(Method);
                }
                else
                {
                    if (_FormState != FormState.Normal && value == FormState.Normal)
                    {
                        _PreviousFormState = _FormState;
                        _FormState = FormState.Normal;
                    }
                    else if (_FormState != FormState.Maximized && value == FormState.Maximized)
                    {
                        if (_EnableMaximize)
                        {
                            _PreviousFormState = _FormState;
                            _FormState = FormState.Maximized;
                        }
                    }
                    else if (_FormState != FormState.FullScreen && value == FormState.FullScreen)
                    {
                        if (_EnableFullScreen)
                        {
                            _PreviousFormState = _FormState;
                            _FormState = FormState.FullScreen;
                        }
                    }
                }
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
                Rectangle CurScrBounds = PrimaryScreenBounds;

                int _X = Math.Max(CurScrBounds.X - Width + 1, Math.Min(value, CurScrBounds.Right - 1));

                if (_Initialized)
                {
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

                    Action Method = () =>
                    {
                        _UpdateLayout(UpdateLayoutEventType.LocationChanged);
                    };

                    _Client.Invoke(Method);
                }
                else
                {
                    Bounds_Current_X = Bounds_Normal_X = _X;
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
                Rectangle CurScrBounds = PrimaryScreenBounds;

                int _Y = Math.Max(CurScrBounds.Y - Height + 1, Math.Min(value, CurScrBounds.Bottom - 1));

                if (_Initialized)
                {
                    switch (_FormState)
                    {
                        case FormState.Normal:
                            Bounds_Current_Y = Bounds_Normal_Y = _Y;
                            break;

                        case FormState.QuarterScreen:
                            Bounds_Current_Y = Bounds_QuarterScreen_Y = _Y;
                            break;
                    }

                    Action Method = () =>
                    {
                        _UpdateLayout(UpdateLayoutEventType.LocationChanged);
                    };

                    _Client.Invoke(Method);
                }
                else
                {
                    Bounds_Current_Y = Bounds_Normal_Y = _Y;
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
                return Bounds_Current_Right;
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
                return Bounds_Current_Bottom;
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
                Rectangle CurScrBounds = PrimaryScreenBounds;

                Point _Location = new Point(Math.Max(CurScrBounds.X - Width + 1, Math.Min(value.X, CurScrBounds.Right - 1)), Math.Max(CurScrBounds.Y - Height + 1, Math.Min(value.Y, CurScrBounds.Bottom - 1)));

                if (_Initialized)
                {
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

                    Action Method = () =>
                    {
                        _UpdateLayout(UpdateLayoutEventType.LocationChanged);
                    };

                    _Client.Invoke(Method);
                }
                else
                {
                    Bounds_Current_Location = Bounds_Normal_Location = _Location;
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
                if (_Initialized)
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

                    Action Method = () =>
                    {
                        _UpdateLayout(UpdateLayoutEventType.SizeChanged);
                    };

                    _Client.Invoke(Method);
                }
                else
                {
                    Bounds_Current_Width = Bounds_Normal_Width = value;
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
                if (_Initialized)
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

                    Action Method = () =>
                    {
                        _UpdateLayout(UpdateLayoutEventType.SizeChanged);
                    };

                    _Client.Invoke(Method);
                }
                else
                {
                    Bounds_Current_Height = Bounds_Normal_Height = value;
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
                if (_Initialized)
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

                    Action Method = () =>
                    {
                        _UpdateLayout(UpdateLayoutEventType.SizeChanged);
                    };

                    _Client.Invoke(Method);
                }
                else
                {
                    Bounds_Current_Size = Bounds_Normal_Size = value;
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
                Rectangle CurScrBounds = PrimaryScreenBounds;

                Rectangle _Bounds = new Rectangle();
                _Bounds.Size = new Size(Math.Max(MinimumWidth, Math.Min(value.Width, MaximumWidth)), Math.Max(MinimumHeight, Math.Min(value.Height, MaximumHeight)));
                _Bounds.Location = new Point(Math.Max(CurScrBounds.X - _Bounds.Width + 1, Math.Min(value.X, CurScrBounds.Right - 1)), Math.Max(CurScrBounds.Y - _Bounds.Height + 1, Math.Min(value.Y, CurScrBounds.Bottom - 1)));

                if (_Initialized)
                {
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

                    Action Method = () =>
                    {
                        _UpdateLayout(UpdateLayoutEventType.Result);
                    };

                    _Client.Invoke(Method);
                }
                else
                {
                    Bounds_Current = Bounds_Normal = _Bounds;
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
                if (!(value is null))
                {
                    _Events.AddHandler(EventKey.Loading, value);
                }
            }

            remove
            {
                if (!(value is null))
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
                if (!(value is null))
                {
                    _Events.AddHandler(EventKey.Loaded, value);
                }
            }

            remove
            {
                if (!(value is null))
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
                if (!(value is null))
                {
                    _Events.AddHandler(EventKey.Closing, value);
                }
            }

            remove
            {
                if (!(value is null))
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
                if (!(value is null))
                {
                    _Events.AddHandler(EventKey.Closed, value);
                }
            }

            remove
            {
                if (!(value is null))
                {
                    _Events.RemoveHandler(EventKey.Closed, value);
                }
            }
        }

        /// <summary>
        /// 在窗口变为激活状态时发生。
        /// </summary>
        public event EventHandler Activated
        {
            add
            {
                if (!(value is null))
                {
                    _Events.AddHandler(EventKey.Activated, value);
                }
            }

            remove
            {
                if (!(value is null))
                {
                    _Events.RemoveHandler(EventKey.Activated, value);
                }
            }
        }

        /// <summary>
        /// 在窗口变为非激活状态时发生。
        /// </summary>
        public event EventHandler Deactivate
        {
            add
            {
                if (!(value is null))
                {
                    _Events.AddHandler(EventKey.Deactivate, value);
                }
            }

            remove
            {
                if (!(value is null))
                {
                    _Events.RemoveHandler(EventKey.Deactivate, value);
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
                if (!(value is null))
                {
                    _Events.AddHandler(EventKey.Move, value);
                }
            }

            remove
            {
                if (!(value is null))
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
                if (!(value is null))
                {
                    _Events.AddHandler(EventKey.LocationChanged, value);
                }
            }

            remove
            {
                if (!(value is null))
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
                if (!(value is null))
                {
                    _Events.AddHandler(EventKey.Resize, value);
                }
            }

            remove
            {
                if (!(value is null))
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
                if (!(value is null))
                {
                    _Events.AddHandler(EventKey.SizeChanged, value);
                }
            }

            remove
            {
                if (!(value is null))
                {
                    _Events.RemoveHandler(EventKey.SizeChanged, value);
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
                if (!(value is null))
                {
                    _Events.AddHandler(EventKey.FormStateChanged, value);
                }
            }

            remove
            {
                if (!(value is null))
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
                if (!(value is null))
                {
                    _Events.AddHandler(EventKey.EnabledChanged, value);
                }
            }

            remove
            {
                if (!(value is null))
                {
                    _Events.RemoveHandler(EventKey.EnabledChanged, value);
                }
            }
        }

        /// <summary>
        /// 在表示窗口是否可见的布尔值更改时发生。
        /// </summary>
        public event EventHandler VisibleChanged
        {
            add
            {
                if (!(value is null))
                {
                    _Events.AddHandler(EventKey.VisibleChanged, value);
                }
            }

            remove
            {
                if (!(value is null))
                {
                    _Events.RemoveHandler(EventKey.VisibleChanged, value);
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
                if (!(value is null))
                {
                    _Events.AddHandler(EventKey.OpacityChanged, value);
                }
            }

            remove
            {
                if (!(value is null))
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
                if (!(value is null))
                {
                    _Events.AddHandler(EventKey.CaptionChanged, value);
                }
            }

            remove
            {
                if (!(value is null))
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
                if (!(value is null))
                {
                    _Events.AddHandler(EventKey.ThemeChanged, value);
                }
            }

            remove
            {
                if (!(value is null))
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
                if (!(value is null))
                {
                    _Events.AddHandler(EventKey.ThemeColorChanged, value);
                }
            }

            remove
            {
                if (!(value is null))
                {
                    _Events.RemoveHandler(EventKey.ThemeColorChanged, value);
                }
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 判断此 FormManager 是否与指定的对象相等。
        /// </summary>
        /// <param name="obj">用于比较的对象。</param>
        /// <returns>布尔值，表示此 FormManager 是否与指定的对象相等。</returns>
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }
            else if (obj is null || !(obj is FormManager))
            {
                return false;
            }
            else
            {
                return Equals((FormManager)obj);
            }
        }

        /// <summary>
        /// 返回此 FormManager 的哈希代码。
        /// </summary>
        /// <returns>32 位整数，表示此 FormManager 的哈希代码。</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// 将此 FormManager 转换为字符串。
        /// </summary>
        /// <returns>字符串，表示此 FormManager 的字符串形式。</returns>
        public override string ToString()
        {
            string Str = base.ToString();

            if (string.IsNullOrEmpty(_Caption))
            {
                return Str;
            }
            else
            {
                return string.Concat(Str, ", Caption: ", _Caption);
            }
        }

        //

        /// <summary>
        /// 判断此 FormManager 是否与指定的 FormManager 对象相等。
        /// </summary>
        /// <param name="formManager">用于比较的 FormManager 对象。</param>
        /// <returns>布尔值，表示此 FormManager 是否与指定的 FormManager 对象相等。</returns>
        public bool Equals(FormManager formManager)
        {
            if (formManager is null)
            {
                return false;
            }
            else if (object.ReferenceEquals(this, formManager))
            {
                return true;
            }
            else
            {
                return base.Equals(formManager);
            }
        }

        //

        /// <summary>
        /// 使窗口还原至普通大小。
        /// </summary>
        /// <returns>布尔值，表示此操作是否成功执行。</returns>
        public bool Return()
        {
            Func<bool> Method = () =>
            {
                if (_Client.WindowState == FormWindowState.Minimized)
                {
                    _Return(UpdateLayoutBehavior.None, UpdateLayoutEventType.None);

                    return true;
                }
                else
                {
                    if (_CanExitFullScreen())
                    {
                        _ExitFullScreen(UpdateLayoutBehavior.Animate, UpdateLayoutEventType.Result);

                        return true;
                    }
                    else if (_CanReturn())
                    {
                        _Return(UpdateLayoutBehavior.Animate, UpdateLayoutEventType.Result);

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            };

            return (bool)_Client.Invoke(Method);
        }

        /// <summary>
        /// 使窗口最小化至任务栏。
        /// </summary>
        /// <returns>布尔值，表示此操作是否成功执行。</returns>
        public bool Minimize()
        {
            Func<bool> Method = () =>
            {
                if (_CanMinimize())
                {
                    _Minimize();

                    return true;
                }
                else
                {
                    return false;
                }
            };

            return (bool)_Client.Invoke(Method);
        }

        /// <summary>
        /// 使窗口最大化。
        /// </summary>
        /// <returns>布尔值，表示此操作是否成功执行。</returns>
        public bool Maximize()
        {
            Func<bool> Method = () =>
            {
                if (_CanMaximize())
                {
                    _Maximize(UpdateLayoutBehavior.Animate, UpdateLayoutEventType.Result);

                    return true;
                }
                else
                {
                    return false;
                }
            };

            return (bool)_Client.Invoke(Method);
        }

        /// <summary>
        /// 使窗口进入全屏幕模式。
        /// </summary>
        /// <returns>布尔值，表示此操作是否成功执行。</returns>
        public bool EnterFullScreen()
        {
            Func<bool> Method = () =>
            {
                if (_CanEnterFullScreen())
                {
                    _EnterFullScreen(UpdateLayoutBehavior.Animate, UpdateLayoutEventType.Result);

                    return true;
                }
                else
                {
                    return false;
                }
            };

            return (bool)_Client.Invoke(Method);
        }

        /// <summary>
        /// 使窗口退出全屏幕模式。
        /// </summary>
        /// <returns>布尔值，表示此操作是否成功执行。</returns>
        public bool ExitFullScreen()
        {
            Func<bool> Method = () =>
            {
                if (_CanExitFullScreen())
                {
                    _ExitFullScreen(UpdateLayoutBehavior.Animate, UpdateLayoutEventType.Result);

                    return true;
                }
                else
                {
                    return false;
                }
            };

            return (bool)_Client.Invoke(Method);
        }

        /// <summary>
        /// 使窗口占据桌面的左半区域。
        /// </summary>
        /// <returns>布尔值，表示此操作是否成功执行。</returns>
        public bool LeftHalfScreen()
        {
            Func<bool> Method = () =>
            {
                if (_CanHighAsScreen())
                {
                    _LeftHalfScreen(UpdateLayoutBehavior.Animate, UpdateLayoutEventType.Result);

                    return true;
                }
                else
                {
                    return false;
                }
            };

            return (bool)_Client.Invoke(Method);
        }

        /// <summary>
        /// 使窗口占据桌面的右半区域。
        /// </summary>
        /// <returns>布尔值，表示此操作是否成功执行。</returns>
        public bool RightHalfScreen()
        {
            Func<bool> Method = () =>
            {
                if (_CanHighAsScreen())
                {
                    _RightHalfScreen(UpdateLayoutBehavior.Animate, UpdateLayoutEventType.Result);

                    return true;
                }
                else
                {
                    return false;
                }
            };

            return (bool)_Client.Invoke(Method);
        }

        /// <summary>
        /// 使窗口与桌面的高度相同。
        /// </summary>
        /// <returns>布尔值，表示此操作是否成功执行。</returns>
        public bool HighAsScreen()
        {
            Func<bool> Method = () =>
            {
                if (_CanHighAsScreen())
                {
                    _HighAsScreen(UpdateLayoutBehavior.Animate, UpdateLayoutEventType.Result);

                    return true;
                }
                else
                {
                    return false;
                }
            };

            return (bool)_Client.Invoke(Method);
        }

        /// <summary>
        /// 使窗口占据桌面的左上四分之一区域。
        /// </summary>
        /// <returns>布尔值，表示此操作是否成功执行。</returns>
        public bool TopLeftQuarterScreen()
        {
            Func<bool> Method = () =>
            {
                if (_CanQuarterScreen())
                {
                    _TopLeftQuarterScreen(UpdateLayoutBehavior.Animate, UpdateLayoutEventType.Result);

                    return true;
                }
                else
                {
                    return false;
                }
            };

            return (bool)_Client.Invoke(Method);
        }

        /// <summary>
        /// 使窗口占据桌面的右上四分之一区域。
        /// </summary>
        /// <returns>布尔值，表示此操作是否成功执行。</returns>
        public bool TopRightQuarterScreen()
        {
            Func<bool> Method = () =>
            {
                if (_CanQuarterScreen())
                {
                    _TopRightQuarterScreen(UpdateLayoutBehavior.Animate, UpdateLayoutEventType.Result);

                    return true;
                }
                else
                {
                    return false;
                }
            };

            return (bool)_Client.Invoke(Method);
        }

        /// <summary>
        /// 使窗口占据桌面的左下四分之一区域。
        /// </summary>
        /// <returns>布尔值，表示此操作是否成功执行。</returns>
        public bool BottomLeftQuarterScreen()
        {
            Func<bool> Method = () =>
            {
                if (_CanQuarterScreen())
                {
                    _BottomLeftQuarterScreen(UpdateLayoutBehavior.Animate, UpdateLayoutEventType.Result);

                    return true;
                }
                else
                {
                    return false;
                }
            };

            return (bool)_Client.Invoke(Method);
        }

        /// <summary>
        /// 使窗口占据桌面的右下四分之一区域。
        /// </summary>
        /// <returns>布尔值，表示此操作是否成功执行。</returns>
        public bool BottomRightQuarterScreen()
        {
            Func<bool> Method = () =>
            {
                if (_CanQuarterScreen())
                {
                    _BottomRightQuarterScreen(UpdateLayoutBehavior.Animate, UpdateLayoutEventType.Result);

                    return true;
                }
                else
                {
                    return false;
                }
            };

            return (bool)_Client.Invoke(Method);
        }

        /// <summary>
        /// 关闭窗口。
        /// </summary>
        /// <returns>布尔值，表示此操作是否成功执行。</returns>
        public bool Close()
        {
            Func<bool> Method = () =>
            {
                if (_CanClose())
                {
                    _Close();

                    return true;
                }
                else
                {
                    return false;
                }
            };

            return (bool)_Client.Invoke(Method);
        }

        //

        /// <summary>
        /// 引发 Loading 事件。
        /// </summary>
        public void OnLoading()
        {
            if (_Initialized)
            {
                Action Method = () => _OnLoading();

                _Client.Invoke(Method);
            }
        }

        /// <summary>
        /// 引发 Loaded 事件。
        /// </summary>
        public void OnLoaded()
        {
            if (_Initialized)
            {
                Action Method = () => _OnLoaded();

                _Client.Invoke(Method);
            }
        }

        /// <summary>
        /// 引发 Closing 事件。
        /// </summary>
        public void OnClosing()
        {
            if (_Initialized)
            {
                Action Method = () => _OnClosing();

                _Client.Invoke(Method);
            }
        }

        /// <summary>
        /// 引发 Closed 事件。
        /// </summary>
        public void OnClosed()
        {
            if (_Initialized)
            {
                Action Method = () => _OnClosed();

                _Client.Invoke(Method);
            }
        }

        /// <summary>
        /// 引发 Move 事件。
        /// </summary>
        public void OnMove()
        {
            if (_Initialized)
            {
                Action Method = () => _OnMove();

                _Client.Invoke(Method);
            }
        }

        /// <summary>
        /// 引发 LocationChanged 事件。
        /// </summary>
        public void OnLocationChanged()
        {
            if (_Initialized)
            {
                Action Method = () => _OnLocationChanged();

                _Client.Invoke(Method);
            }
        }

        /// <summary>
        /// 引发 Resize 事件。
        /// </summary>
        public void OnResize()
        {
            if (_Initialized)
            {
                Action Method = () => _OnResize();

                _Client.Invoke(Method);
            }
        }

        /// <summary>
        /// 引发 SizeChanged 事件。
        /// </summary>
        public void OnSizeChanged()
        {
            if (_Initialized)
            {
                Action Method = () => _OnSizeChanged();

                _Client.Invoke(Method);
            }
        }

        /// <summary>
        /// 引发 FormStateChanged 事件。
        /// </summary>
        public void OnFormStateChanged()
        {
            if (_Initialized)
            {
                Action Method = () => _OnFormStateChanged();

                _Client.Invoke(Method);
            }
        }

        /// <summary>
        /// 引发 EnabledChanged 事件。
        /// </summary>
        public void OnEnabledChanged()
        {
            if (_Initialized)
            {
                Action Method = () => _OnEnabledChanged();

                _Client.Invoke(Method);
            }
        }

        /// <summary>
        /// 引发 VisibleChanged 事件。
        /// </summary>
        public void OnVisibleChanged()
        {
            if (_Initialized)
            {
                Action Method = () => _OnVisibleChanged();

                _Client.Invoke(Method);
            }
        }

        /// <summary>
        /// 引发 OpacityChanged 事件。
        /// </summary>
        public void OnOpacityChanged()
        {
            if (_Initialized)
            {
                Action Method = () => _OnOpacityChanged();

                _Client.Invoke(Method);
            }
        }

        /// <summary>
        /// 引发 CaptionChanged 事件。
        /// </summary>
        public void OnCaptionChanged()
        {
            if (_Initialized)
            {
                Action Method = () => _OnCaptionChanged();

                _Client.Invoke(Method);
            }
        }

        /// <summary>
        /// 引发 ThemeChanged 事件。
        /// </summary>
        public void OnThemeChanged()
        {
            if (_Initialized)
            {
                Action Method = () => _OnThemeChanged();

                _Client.Invoke(Method);
            }
        }

        /// <summary>
        /// 引发 ThemeColorChanged 事件。
        /// </summary>
        public void OnThemeColorChanged()
        {
            if (_Initialized)
            {
                Action Method = () => _OnThemeColorChanged();

                _Client.Invoke(Method);
            }
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 判断两个 FormManager 对象是否相等。
        /// </summary>
        /// <param name="left">用于比较的第一个 FormManager 对象。</param>
        /// <param name="right">用于比较的第二个 FormManager 对象。</param>
        /// <returns>布尔值，表示此 FormManager 是否与指定的 FormManager 对象相等。</returns>
        public static bool Equals(FormManager left, FormManager right)
        {
            if (object.ReferenceEquals(left, right))
            {
                return true;
            }
            else if (left is null || right is null)
            {
                return false;
            }
            else
            {
                return left.Equals(right);
            }
        }

        #endregion

        #region 运算符

        /// <summary>
        /// 判断两个 FormManager 对象是否相等。
        /// </summary>
        /// <param name="left">运算符左侧比较的 FormManager 对象。</param>
        /// <param name="right">运算符右侧比较的 FormManager 对象。</param>
        /// <returns>布尔值，表示两个 FormManager 对象是否相等。</returns>
        public static bool operator ==(FormManager left, FormManager right)
        {
            if (object.ReferenceEquals(left, right))
            {
                return true;
            }
            else if (left is null || right is null)
            {
                return false;
            }
            else
            {
                return left.Equals(right);
            }
        }

        /// <summary>
        /// 判断两个 FormManager 对象是否不相等。
        /// </summary>
        /// <param name="left">运算符左侧比较的 FormManager 对象。</param>
        /// <param name="right">运算符右侧比较的 FormManager 对象。</param>
        /// <returns>布尔值，表示两个 FormManager 对象是否不相等。</returns>
        public static bool operator !=(FormManager left, FormManager right)
        {
            if (object.ReferenceEquals(left, right))
            {
                return false;
            }
            else if (left is null || right is null)
            {
                return true;
            }
            else
            {
                return !left.Equals(right);
            }
        }

        #endregion
    }
}