/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2013-2018 chibayuki@foxmail.com

Com.WinForm.FormManager
Version 18.5.23.0000

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
    /// 窗体管理器。
    /// </summary>
    public sealed class FormManager
    {
        #region 私有与内部成员

        private bool _Initialized = false; // 表示此 FormManager 对象是否已完成初始化的布尔值。

        private bool _LoadingNow = false; // 表示此 FormManager 对象是否正在执行 Loading 事件或 Loaded 事件的布尔值。
        private bool _ClosingNow = false; // 表示此 FormManager 对象是否正在执行 Closing 事件或 Closed 事件的布尔值。

        //

        private static List<FormManager> _FormManagerList = new List<FormManager>(1); // 窗体管理器列表。

        private bool _IsMainForm = false; // 表示此窗体是否为主窗体的布尔值。

        //

        private Form _FormClient = null; // 表示窗体工作区的 Form 对象。
        internal Form FormClient // 获取表示窗体工作区的 Form 对象。
        {
            get
            {
                return _FormClient;
            }
        }

        private FormTitleBar _FormTitleBar = null; // 表示窗体标题栏的 FormTitleBar 对象。
        internal FormTitleBar FormTitleBar // 获取表示窗体标题栏的 FormTitleBar 对象。
        {
            get
            {
                return _FormTitleBar;
            }
        }

        private FormBorder _FormBorder = null; // 表示窗体边框的 FormBorder 对象。
        internal FormBorder FormBorder // 获取表示窗体边框的 FormBorder 对象。
        {
            get
            {
                return _FormBorder;
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

        private FormManager _Owner = null; // 拥有此窗体的窗体的窗体管理器。
        private List<FormManager> _Owned = new List<FormManager>(0); // 表示此窗体拥有的所有窗体的窗体管理器列表。

        private void _AddOwned(FormManager formManager) // 向表示此窗体拥有的所有窗体的窗体管理器列表添加一个窗体管理器。
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

        private void _RemoveOwned(FormManager formManager) // 从表示此窗体拥有的所有窗体的窗体管理器列表删除一个窗体管理器。
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

        internal static Point _CursorPosition // 鼠标指针在桌面的位置。
        {
            get
            {
                return Cursor.Position;
            }
        }

        internal static Rectangle _PrimaryScreenClient // 主屏幕的工作区。
        {
            get
            {
                return Screen.PrimaryScreen.WorkingArea;
            }
        }

        internal static Rectangle _PrimaryScreenBounds // 主屏幕的边界。
        {
            get
            {
                return Screen.PrimaryScreen.Bounds;
            }
        }

        //

        private FormWindowState _LastFormWindowState = FormWindowState.Normal; // _FormClient.WindowState 的最终值。

        //

        private FormStyle _FormStyle = FormStyle.Sizable; // 窗体的样式。

        private bool _EnableFullScreen = true; // 表示是否允许窗体以全屏幕模式运行的布尔值。
        private bool _TopMost = false; // 表示是否允许窗体置于顶层的布尔值。
        private bool _ShowInTaskbar = true; // 表示窗体是否在任务栏显示的布尔值。

        //

        private int _FormTitleBarHeight = 32; // 窗体标题栏的高度。

        private const int _FormBorderSize = 8; // 窗体边框的大小。

        //

        private int __MinimumWidth = 0; // 窗体的最小宽度。
        private int __MinimumHeight = 0; // 窗体的最小高度。

        internal int _MinimumWidth // 获取或设置窗体的最小宽度。
        {
            get
            {
                return __MinimumWidth;
            }

            set
            {
                __MinimumWidth = Math.Max(0, value);
            }
        }

        internal int _MinimumHeight // 获取或设置窗体的最小高度。
        {
            get
            {
                return __MinimumHeight;
            }

            set
            {
                __MinimumHeight = Math.Max(0, value);
            }
        }

        internal int _MinimumBoundsWidth // 获取窗体的最小宽度。
        {
            get
            {
                return Math.Max(_FormTitleBarHeight + 46, Math.Min(_PrimaryScreenBounds.Width, __MinimumWidth));
            }
        }

        internal int _MinimumBoundsHeight // 获取窗体的最小高度。
        {
            get
            {
                return Math.Max(_FormTitleBarHeight + 2, Math.Min(_PrimaryScreenBounds.Height, __MinimumHeight));
            }
        }

        internal Size _MinimumBoundsSize // 获取窗体的最小大小。
        {
            get
            {
                return new Size(_MinimumBoundsWidth, _MinimumBoundsHeight);
            }
        }

        private int __MaximumWidth = 0; // 窗体的最大宽度。
        private int __MaximumHeight = 0; // 窗体的最大高度。

        internal int _MaximumWidth // 获取或设置窗体的最大宽度。
        {
            get
            {
                return __MaximumWidth;
            }

            set
            {
                __MaximumWidth = Math.Max(0, value);
            }
        }

        internal int _MaximumHeight // 获取或设置窗体的最大高度。
        {
            get
            {
                return __MaximumHeight;
            }

            set
            {
                __MaximumHeight = Math.Max(0, value);
            }
        }

        internal int _MaximumBoundsWidth // 获取窗体的最大宽度。
        {
            get
            {
                return Math.Max(1, (__MaximumWidth > 0 ? Math.Min(_PrimaryScreenBounds.Width, __MaximumWidth) : _PrimaryScreenBounds.Width));
            }
        }

        internal int _MaximumBoundsHeight // 获取窗体的最大高度。
        {
            get
            {
                return Math.Max(1, (__MaximumHeight > 0 ? Math.Min(_PrimaryScreenBounds.Height, __MaximumHeight) : _PrimaryScreenBounds.Height));
            }
        }

        internal Size _MaximumBoundsSize // 获取窗体的最大大小。
        {
            get
            {
                return new Size(_MaximumBoundsWidth, _MaximumBoundsHeight);
            }
        }

        //

        private FormState _FormState = FormState.Normal; // 窗体的状态。
        private FormState _FormState_BeforeFullScreen = FormState.Normal; // 进入全屏幕状态之前窗体的状态。

        //

        private bool _Enabled = true; // 表示窗体是否对用户交互作出响应的布尔值。
        private string _Caption = string.Empty; // 窗体的标题。
        private Font _CaptionFont = new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, 134); // 窗体标题的字体。
        private double _Opacity = 1.0; // 窗体的不透明度。
        private Theme _Theme = Theme.Colorful; // 窗体的主题。
        private ColorX _ThemeColor = ColorX.FromRGB(128, 128, 128); // 窗体的主题色。
        private bool _ShowFormTitleBarColor = true; // 表示是否在窗体标题栏上显示主题色的布尔值。
        private bool _EnableFormTitleBarTransparent = true; // 表示是否允许以半透明方式显示窗体标题栏的布尔值。
        private RecommendColors _RecommendColors = null; // 当前主题建议的颜色。

        //

        private int __Bounds_Current_X = int.MinValue; // 当前状态窗体在桌面的左边距。
        private int __Bounds_Current_Y = int.MinValue; // 当前状态窗体在桌面的上边距。
        private int __Bounds_Current_Width = 300; // 当前状态窗体的宽度。
        private int __Bounds_Current_Height = 300; // 当前状态窗体的高度。

        internal int _Bounds_Current_X // 获取或设置当前状态窗体在桌面的左边距。
        {
            get
            {
                return __Bounds_Current_X;
            }

            set
            {
                __Bounds_Current_X = value;
            }
        }

        internal int _Bounds_Current_Y // 获取或设置当前状态窗体在桌面的上边距。
        {
            get
            {
                return __Bounds_Current_Y;
            }

            set
            {
                __Bounds_Current_Y = value;
            }
        }

        internal int _Bounds_Current_Right // 获取当前状态窗体在桌面的右边距。
        {
            get
            {
                return (__Bounds_Current_X + __Bounds_Current_Width);
            }
        }

        internal int _Bounds_Current_Bottom // 获取当前状态窗体在桌面的下边距。
        {
            get
            {
                return (__Bounds_Current_Y + __Bounds_Current_Height);
            }
        }

        internal int _Bounds_Current_Width // 获取或设置当前状态窗体的宽度。
        {
            get
            {
                return __Bounds_Current_Width;
            }

            set
            {
                __Bounds_Current_Width = Math.Max(_MinimumBoundsWidth, Math.Min(value, _MaximumBoundsWidth));
            }
        }

        internal int _Bounds_Current_Height // 获取或设置当前状态窗体的高度。
        {
            get
            {
                return __Bounds_Current_Height;
            }

            set
            {
                __Bounds_Current_Height = Math.Max(_MinimumBoundsHeight, Math.Min(value, _MaximumBoundsHeight));
            }
        }

        internal Point _Bounds_Current_Location // 获取或设置当前状态窗体在桌面的位置。
        {
            get
            {
                return new Point(__Bounds_Current_X, __Bounds_Current_Y);
            }

            set
            {
                __Bounds_Current_X = value.X;
                __Bounds_Current_Y = value.Y;
            }
        }

        internal Size _Bounds_Current_Size // 获取或设置当前状态窗体的大小。
        {
            get
            {
                return new Size(__Bounds_Current_Width, __Bounds_Current_Height);
            }

            set
            {
                __Bounds_Current_Width = Math.Max(_MinimumBoundsWidth, Math.Min(value.Width, _MaximumBoundsWidth));
                __Bounds_Current_Height = Math.Max(_MinimumBoundsHeight, Math.Min(value.Height, _MaximumBoundsHeight));
            }
        }

        internal Rectangle _Bounds_Current // 获取或设置当前状态窗体的位置与大小。
        {
            get
            {
                return new Rectangle(__Bounds_Current_X, __Bounds_Current_Y, __Bounds_Current_Width, __Bounds_Current_Height);
            }

            set
            {
                __Bounds_Current_X = value.X;
                __Bounds_Current_Y = value.Y;
                __Bounds_Current_Width = Math.Max(_MinimumBoundsWidth, Math.Min(value.Width, _MaximumBoundsWidth));
                __Bounds_Current_Height = Math.Max(_MinimumBoundsHeight, Math.Min(value.Height, _MaximumBoundsHeight));
            }
        }

        private int __Bounds_Normal_X = int.MinValue; // 普通状态窗体在桌面的左边距。
        private int __Bounds_Normal_Y = int.MinValue; // 普通状态窗体在桌面的上边距。
        private int __Bounds_Normal_Width = 300; // 普通状态窗体的宽度。
        private int __Bounds_Normal_Height = 300; // 普通状态窗体的高度。

        internal int _Bounds_Normal_X // 获取或设置普通状态窗体在桌面的左边距。
        {
            get
            {
                return __Bounds_Normal_X;
            }

            set
            {
                __Bounds_Normal_X = value;
            }
        }

        internal int _Bounds_Normal_Y // 获取或设置普通状态窗体在桌面的上边距。
        {
            get
            {
                return __Bounds_Normal_Y;
            }

            set
            {
                __Bounds_Normal_Y = value;
            }
        }

        internal int _Bounds_Normal_Right // 获取普通状态窗体在桌面的右边距。
        {
            get
            {
                return (__Bounds_Normal_X + __Bounds_Normal_Width);
            }
        }

        internal int _Bounds_Normal_Bottom // 获取普通状态窗体在桌面的下边距。
        {
            get
            {
                return (__Bounds_Normal_Y + __Bounds_Normal_Height);
            }
        }

        internal int _Bounds_Normal_Width // 获取或设置普通状态窗体的宽度。
        {
            get
            {
                return __Bounds_Normal_Width;
            }

            set
            {
                __Bounds_Normal_Width = Math.Max(_MinimumBoundsWidth, Math.Min(value, _MaximumBoundsWidth));
            }
        }

        internal int _Bounds_Normal_Height // 获取或设置普通状态窗体的高度。
        {
            get
            {
                return __Bounds_Normal_Height;
            }

            set
            {
                __Bounds_Normal_Height = Math.Max(_MinimumBoundsHeight, Math.Min(value, _MaximumBoundsHeight));
            }
        }

        internal Point _Bounds_Normal_Location // 获取或设置普通状态窗体在桌面的位置。
        {
            get
            {
                return new Point(__Bounds_Normal_X, __Bounds_Normal_Y);
            }

            set
            {
                __Bounds_Normal_X = value.X;
                __Bounds_Normal_Y = value.Y;
            }
        }

        internal Size _Bounds_Normal_Size // 获取或设置普通状态窗体的大小。
        {
            get
            {
                return new Size(__Bounds_Normal_Width, __Bounds_Normal_Height);
            }

            set
            {
                __Bounds_Normal_Width = Math.Max(_MinimumBoundsWidth, Math.Min(value.Width, _MaximumBoundsWidth));
                __Bounds_Normal_Height = Math.Max(_MinimumBoundsHeight, Math.Min(value.Height, _MaximumBoundsHeight));
            }
        }

        internal Rectangle _Bounds_Normal // 获取或设置普通状态窗体的位置与大小。
        {
            get
            {
                return new Rectangle(__Bounds_Normal_X, __Bounds_Normal_Y, __Bounds_Normal_Width, __Bounds_Normal_Height);
            }

            set
            {
                __Bounds_Normal_X = value.X;
                __Bounds_Normal_Y = value.Y;
                __Bounds_Normal_Width = Math.Max(_MinimumBoundsWidth, Math.Min(value.Width, _MaximumBoundsWidth));
                __Bounds_Normal_Height = Math.Max(_MinimumBoundsHeight, Math.Min(value.Height, _MaximumBoundsHeight));
            }
        }

        private int __Bounds_QuarterScreen_X = 0; // 占据桌面四分之一状态窗体在桌面的左边距。
        private int __Bounds_QuarterScreen_Y = 0; // 占据桌面四分之一状态窗体在桌面的上边距。
        private int __Bounds_QuarterScreen_Width = 1; // 占据桌面四分之一状态窗体的宽度。
        private int __Bounds_QuarterScreen_Height = 1; // 占据桌面四分之一状态窗体的高度。

        internal int _Bounds_QuarterScreen_X // 获取或设置占据桌面四分之一状态窗体在桌面的左边距。
        {
            get
            {
                return __Bounds_QuarterScreen_X;
            }

            set
            {
                __Bounds_QuarterScreen_X = value;
            }
        }

        internal int _Bounds_QuarterScreen_Y // 获取或设置占据桌面四分之一状态窗体在桌面的上边距。
        {
            get
            {
                return __Bounds_QuarterScreen_Y;
            }

            set
            {
                __Bounds_QuarterScreen_Y = value;
            }
        }

        internal int _Bounds_QuarterScreen_Right // 获取占据桌面四分之一状态窗体在桌面的右边距。
        {
            get
            {
                return (__Bounds_QuarterScreen_X + __Bounds_QuarterScreen_Width);
            }
        }

        internal int _Bounds_QuarterScreen_Bottom // 获取占据桌面四分之一状态窗体在桌面的下边距。
        {
            get
            {
                return (__Bounds_QuarterScreen_Y + __Bounds_QuarterScreen_Height);
            }
        }

        internal int _Bounds_QuarterScreen_Width // 获取或设置占据桌面四分之一状态窗体的宽度。
        {
            get
            {
                return __Bounds_QuarterScreen_Width;
            }

            set
            {
                __Bounds_QuarterScreen_Width = Math.Max(_MinimumBoundsWidth, Math.Min(value, _MaximumBoundsWidth));
            }
        }

        internal int _Bounds_QuarterScreen_Height // 获取或设置占据桌面四分之一状态窗体的高度。
        {
            get
            {
                return __Bounds_QuarterScreen_Height;
            }

            set
            {
                __Bounds_QuarterScreen_Height = Math.Max(_MinimumBoundsHeight, Math.Min(value, _MaximumBoundsHeight));
            }
        }

        internal Point _Bounds_QuarterScreen_Location // 获取或设置占据桌面四分之一状态窗体在桌面的位置。
        {
            get
            {
                return new Point(__Bounds_QuarterScreen_X, __Bounds_QuarterScreen_Y);
            }

            set
            {
                __Bounds_QuarterScreen_X = value.X;
                __Bounds_QuarterScreen_Y = value.Y;
            }
        }

        internal Size _Bounds_QuarterScreen_Size // 获取或设置占据桌面四分之一状态窗体的大小。
        {
            get
            {
                return new Size(__Bounds_QuarterScreen_Width, __Bounds_QuarterScreen_Height);
            }

            set
            {
                __Bounds_QuarterScreen_Width = Math.Max(_MinimumBoundsWidth, Math.Min(value.Width, _MaximumBoundsWidth));
                __Bounds_QuarterScreen_Height = Math.Max(_MinimumBoundsHeight, Math.Min(value.Height, _MaximumBoundsHeight));
            }
        }

        internal Rectangle _Bounds_QuarterScreen // 获取或设置占据桌面四分之一状态窗体的位置与大小。
        {
            get
            {
                return new Rectangle(__Bounds_QuarterScreen_X, __Bounds_QuarterScreen_Y, __Bounds_QuarterScreen_Width, __Bounds_QuarterScreen_Height);
            }

            set
            {
                __Bounds_QuarterScreen_X = value.X;
                __Bounds_QuarterScreen_Y = value.Y;
                __Bounds_QuarterScreen_Width = Math.Max(_MinimumBoundsWidth, Math.Min(value.Width, _MaximumBoundsWidth));
                __Bounds_QuarterScreen_Height = Math.Max(_MinimumBoundsHeight, Math.Min(value.Height, _MaximumBoundsHeight));
            }
        }

        private int __Bounds_BeforeFullScreen_X = 0; // 进入全屏幕状态前窗体在桌面的左边距。
        private int __Bounds_BeforeFullScreen_Y = 0; // 进入全屏幕状态前窗体在桌面的上边距。
        private int __Bounds_BeforeFullScreen_Width = 1; // 进入全屏幕状态前窗体的宽度。
        private int __Bounds_BeforeFullScreen_Height = 1; // 进入全屏幕状态前窗体的高度。

        internal int _Bounds_BeforeFullScreen_X // 获取或设置进入全屏幕状态前窗体在桌面的左边距。
        {
            get
            {
                return __Bounds_BeforeFullScreen_X;
            }

            set
            {
                __Bounds_BeforeFullScreen_X = value;
            }
        }

        internal int _Bounds_BeforeFullScreen_Y // 获取或设置进入全屏幕状态前窗体在桌面的上边距。
        {
            get
            {
                return __Bounds_BeforeFullScreen_Y;
            }

            set
            {
                __Bounds_BeforeFullScreen_Y = value;
            }
        }

        internal int _Bounds_BeforeFullScreen_Right // 获取进入全屏幕状态前窗体在桌面的右边距。
        {
            get
            {
                return (__Bounds_BeforeFullScreen_X + __Bounds_BeforeFullScreen_Width);
            }
        }

        internal int _Bounds_BeforeFullScreen_Bottom // 获取进入全屏幕状态前窗体在桌面的下边距。
        {
            get
            {
                return (__Bounds_BeforeFullScreen_Y + __Bounds_BeforeFullScreen_Height);
            }
        }

        internal int _Bounds_BeforeFullScreen_Width // 获取或设置进入全屏幕状态前窗体的宽度。
        {
            get
            {
                return __Bounds_BeforeFullScreen_Width;
            }

            set
            {
                __Bounds_BeforeFullScreen_Width = Math.Max(_MinimumBoundsWidth, Math.Min(value, _MaximumBoundsWidth));
            }
        }

        internal int _Bounds_BeforeFullScreen_Height // 获取或设置进入全屏幕状态前窗体的高度。
        {
            get
            {
                return __Bounds_BeforeFullScreen_Height;
            }

            set
            {
                __Bounds_BeforeFullScreen_Height = Math.Max(_MinimumBoundsHeight, Math.Min(value, _MaximumBoundsHeight));
            }
        }

        internal Point _Bounds_BeforeFullScreen_Location // 获取或设置进入全屏幕状态前窗体在桌面的位置。
        {
            get
            {
                return new Point(__Bounds_BeforeFullScreen_X, __Bounds_BeforeFullScreen_Y);
            }

            set
            {
                __Bounds_BeforeFullScreen_X = value.X;
                __Bounds_BeforeFullScreen_Y = value.Y;
            }
        }

        internal Size _Bounds_BeforeFullScreen_Size // 获取或设置进入全屏幕状态前窗体的大小。
        {
            get
            {
                return new Size(__Bounds_BeforeFullScreen_Width, __Bounds_BeforeFullScreen_Height);
            }

            set
            {
                __Bounds_BeforeFullScreen_Width = Math.Max(_MinimumBoundsWidth, Math.Min(value.Width, _MaximumBoundsWidth));
                __Bounds_BeforeFullScreen_Height = Math.Max(_MinimumBoundsHeight, Math.Min(value.Height, _MaximumBoundsHeight));
            }
        }

        internal Rectangle _Bounds_BeforeFullScreen // 获取或设置进入全屏幕状态前窗体的位置与大小。
        {
            get
            {
                return new Rectangle(__Bounds_BeforeFullScreen_X, __Bounds_BeforeFullScreen_Y, __Bounds_BeforeFullScreen_Width, __Bounds_BeforeFullScreen_Height);
            }

            set
            {
                __Bounds_BeforeFullScreen_X = value.X;
                __Bounds_BeforeFullScreen_Y = value.Y;
                __Bounds_BeforeFullScreen_Width = Math.Max(_MinimumBoundsWidth, Math.Min(value.Width, _MaximumBoundsWidth));
                __Bounds_BeforeFullScreen_Height = Math.Max(_MinimumBoundsHeight, Math.Min(value.Height, _MaximumBoundsHeight));
            }
        }

        //

        internal void _UpdateLayout(UpdateLayoutEventType updateLayoutEventType) // 更新窗体布局。
        {
            _FormClient.Bounds = _SplashScreen.Bounds = new Rectangle(new Point(_Bounds_Current_X, _Bounds_Current_Y + _FormTitleBarHeight), new Size(_Bounds_Current_Width, _Bounds_Current_Height - _FormTitleBarHeight));

            if (_FormState != FormState.FullScreen)
            {
                _FormTitleBar.Bounds = new Rectangle(_Bounds_Current_Location, new Size(_Bounds_Current_Width, _FormTitleBarHeight));
            }

            _FormBorder.Bounds = new Rectangle(new Point(_Bounds_Current_X - _FormBorderSize, _Bounds_Current_Y - _FormBorderSize), new Size(_Bounds_Current_Width + 2 * _FormBorderSize, _Bounds_Current_Height + 2 * _FormBorderSize));

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

        private void _SetBoundsAndUpdateLayout(Rectangle bounds, UpdateLayoutBehavior updateLayoutBehavior, UpdateLayoutEventType updateLayoutEventType) // 设置窗体的位置与大小，并更新窗体布局。
        {
            if (updateLayoutBehavior == UpdateLayoutBehavior.None)
            {
                _Bounds_Current = bounds;
            }
            else if (updateLayoutBehavior == UpdateLayoutBehavior.Static)
            {
                _Bounds_Current = bounds;

                _UpdateLayout(updateLayoutEventType);
            }
            else if (updateLayoutBehavior == UpdateLayoutBehavior.Animate)
            {
                int MaxLTRB = Math.Max(1, Math.Max(Math.Abs(bounds.Left - _Bounds_Current.Left), Math.Max(Math.Abs(bounds.Top - _Bounds_Current.Top), Math.Max(Math.Abs(bounds.Right - _Bounds_Current.Right), Math.Abs(bounds.Bottom - _Bounds_Current.Bottom)))));
                int Delta = Math.Min(MaxLTRB, (int)(new PointD(_PrimaryScreenBounds.Size).VectorModule / 40.96));

                Rectangle oldBounds = Rectangle.FromLTRB(bounds.Left - (bounds.Left - _Bounds_Current.Left) * Delta / MaxLTRB, bounds.Top - (bounds.Top - _Bounds_Current.Top) * Delta / MaxLTRB, bounds.Right - (bounds.Right - _Bounds_Current.Right) * Delta / MaxLTRB, bounds.Bottom - (bounds.Bottom - _Bounds_Current.Bottom) * Delta / MaxLTRB);

                Animation.Frame Frame = (frameId, frameCount, msPerFrame) =>
                {
                    double Pct_F = (frameId == frameCount ? 1 : 1 - Math.Pow(1 - (double)frameId / frameCount, 2));

                    _Bounds_Current = new Rectangle((new PointD(oldBounds.Location) * (1 - Pct_F) + new PointD(bounds.Location) * Pct_F).ToPoint(), (new PointD(oldBounds.Size) * (1 - Pct_F) + new PointD(bounds.Size) * Pct_F).ToSize());

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

        private Predicate<EventArgs> _ReturnVerification = null; // 用于验证是否允许窗体还原的方法。
        private Predicate<EventArgs> _MaximizeVerification = null; // 设置用于验证是否允许窗体最大化的方法。
        private Predicate<EventArgs> _EnterFullScreenVerification = null; // 用于验证是否允许窗体进入全屏幕模式的方法。
        private Predicate<EventArgs> _ExitFullScreenVerification = null; // 用于验证是否允许窗体退出全屏幕模式的方法。
        private Predicate<EventArgs> _CloseVerification = null; // 设置用于验证是否允许窗体关闭的方法。

        internal bool _CanReturn() // 判断是否允许窗体还原。
        {
            return (_Initialized && (_FormState != FormState.FullScreen && _FormState != FormState.Normal) && (_ReturnVerification == null || (_ReturnVerification != null && _ReturnVerification(EventArgs.Empty))));
        }

        internal bool _CanMinimize() // 判断是否允许窗体最小化。
        {
            return (_Initialized && _FormStyle != FormStyle.Dialog && _FormClient.WindowState != FormWindowState.Minimized);
        }

        internal bool _CanMaximize() // 判断是否允许窗体最大化。
        {
            return (_Initialized && _FormStyle == FormStyle.Sizable && _FormClient.WindowState != FormWindowState.Minimized && (_FormState != FormState.FullScreen && _FormState != FormState.Maximized) && (_MaximizeVerification == null || (_MaximizeVerification != null && _MaximizeVerification(EventArgs.Empty))));
        }

        internal bool _CanEnterFullScreen() // 判断是否允许窗体进入全屏幕模式。
        {
            return (_Initialized && _EnableFullScreen && _FormState != FormState.FullScreen && _FormClient.WindowState != FormWindowState.Minimized && (_EnterFullScreenVerification == null || (_EnterFullScreenVerification != null && _EnterFullScreenVerification(EventArgs.Empty))));
        }

        internal bool _CanExitFullScreen() // 判断是否允许窗体退出全屏幕模式。
        {
            return (_Initialized && _FormClient.WindowState != FormWindowState.Minimized && _FormState == FormState.FullScreen && (_ExitFullScreenVerification == null || (_ExitFullScreenVerification != null && _ExitFullScreenVerification(EventArgs.Empty))));
        }

        internal bool _CanHighAsScreen() // 判断是否允许窗体与桌面的高度相同。
        {
            return (_Initialized && _FormStyle == FormStyle.Sizable && _FormClient.WindowState != FormWindowState.Minimized && _FormState != FormState.FullScreen);
        }

        internal bool _CanQuarterScreen() // 判断是否允许窗体占据桌面的四分之一区域。
        {
            return (_Initialized && _FormStyle == FormStyle.Sizable && _FormClient.WindowState != FormWindowState.Minimized && _FormState != FormState.FullScreen);
        }

        internal bool _CanClose() // 判断是否允许窗体关闭。
        {
            return (_Initialized && (_CloseVerification == null || (_CloseVerification != null && _CloseVerification(EventArgs.Empty))));
        }

        //

        private EventHandlerList _Events = new EventHandlerList(); // 窗体事件委托列表。

        private void _TrigEvent(object eventKey) // 引发窗体事件。
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

        private void _OnCaptionChanged() // 引发 CaptionChanged 事件。
        {
            _TrigEvent(EventKey.CaptionChanged);
        }

        private void _OnOpacityChanged() // 引发 OpacityChanged 事件。
        {
            _TrigEvent(EventKey.OpacityChanged);
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

        private void _Return(UpdateLayoutBehavior updateLayoutBehavior, UpdateLayoutEventType updateLayoutEventType) // 使窗体还原至普通大小。
        {
            _FormState = FormState.Normal;

            Rectangle OldBounds = _Bounds_Current;
            _Bounds_Current = _Bounds_Normal;

            _FormTitleBar.OnFormStateChanged();
            _FormBorder.OnFormStateChanged();

            Rectangle NewBounds = _Bounds_Current;
            _Bounds_Current = OldBounds;

            _SetBoundsAndUpdateLayout(NewBounds, updateLayoutBehavior, updateLayoutEventType);

            if (updateLayoutEventType != UpdateLayoutEventType.None)
            {
                _OnFormStateChanged();
            }
        }

        internal void _ReturnByMoveForm() // 通过移动最大化、与桌面高度相同或占据桌面四分之一的窗口，使窗体还原至普通大小。
        {
            _FormState = FormState.Normal;

            Rectangle OldBounds = _Bounds_Current;
            _Bounds_Current = _Bounds_Normal;

            _FormTitleBar.OnFormStateChanged();
            _FormBorder.OnFormStateChanged();

            Rectangle NewBounds = _Bounds_Current;
            _Bounds_Current = OldBounds;

            _SetBoundsAndUpdateLayout(NewBounds, UpdateLayoutBehavior.None, UpdateLayoutEventType.None);

            _OnFormStateChanged();
        }

        internal void _ReturnFromHighAsScreen() // 使窗体由与桌面高度相同状态还原至普通大小。
        {
            _FormState = FormState.Normal;

            Rectangle OldBounds = _Bounds_Current;
            _Bounds_Current_Y = _Bounds_Normal_Y;
            _Bounds_Current_Height = _Bounds_Normal_Height;

            _FormTitleBar.OnFormStateChanged();
            _FormBorder.OnFormStateChanged();

            Rectangle NewBounds = _Bounds_Current;
            _Bounds_Current = OldBounds;

            _SetBoundsAndUpdateLayout(NewBounds, UpdateLayoutBehavior.Static, UpdateLayoutEventType.All);

            _OnFormStateChanged();
        }

        private void _Minimize() // 使窗体最小化至任务栏。
        {
            _FormClient.WindowState = FormWindowState.Minimized;
        }

        private void _Maximize(UpdateLayoutBehavior updateLayoutBehavior, UpdateLayoutEventType updateLayoutEventType) // 使窗体最大化。
        {
            _FormState = FormState.Maximized;

            Rectangle OldBounds = _Bounds_Current;
            _Bounds_Current = _PrimaryScreenClient;

            _FormTitleBar.OnFormStateChanged();
            _FormBorder.OnFormStateChanged();

            Rectangle NewBounds = _Bounds_Current;
            _Bounds_Current = OldBounds;

            _SetBoundsAndUpdateLayout(NewBounds, updateLayoutBehavior, updateLayoutEventType);

            if (updateLayoutEventType != UpdateLayoutEventType.None)
            {
                _OnFormStateChanged();
            }
        }

        private void _EnterFullScreen(UpdateLayoutBehavior updateLayoutBehavior, UpdateLayoutEventType updateLayoutEventType) // 使窗体进入全屏幕模式。
        {
            _FormState_BeforeFullScreen = _FormState;
            _Bounds_BeforeFullScreen = _Bounds_Current;

            _FormState = FormState.FullScreen;

            Rectangle OldBounds = _Bounds_Current;
            _Bounds_Current = _PrimaryScreenBounds;

            _FormTitleBar.OnFormStateChanged();
            _FormBorder.OnFormStateChanged();

            Rectangle NewBounds = _Bounds_Current;
            _Bounds_Current = OldBounds;

            _SetBoundsAndUpdateLayout(NewBounds, updateLayoutBehavior, updateLayoutEventType);

            _FormClient.WindowState = _SplashScreen.WindowState = FormWindowState.Maximized;

            if (updateLayoutEventType != UpdateLayoutEventType.None)
            {
                _OnFormStateChanged();
            }
        }

        private void _ExitFullScreen(UpdateLayoutBehavior updateLayoutBehavior, UpdateLayoutEventType updateLayoutEventType) // 使窗体退出全屏幕模式。
        {
            if (_FormStyle != FormStyle.Sizable)
            {
                _FormState_BeforeFullScreen = FormState.Normal;
                _Bounds_BeforeFullScreen = _Bounds_Normal;
            }

            _FormState = _FormState_BeforeFullScreen;

            _FormClient.WindowState = _SplashScreen.WindowState = FormWindowState.Normal;

            Rectangle OldBounds = _Bounds_Current;
            _Bounds_Current = _Bounds_BeforeFullScreen;

            _FormTitleBar.OnFormStateChanged();
            _FormBorder.OnFormStateChanged();

            Rectangle NewBounds = _Bounds_Current;
            _Bounds_Current = OldBounds;

            if (_FormClient.WindowState == FormWindowState.Maximized)
            {
                _FormClient.WindowState = FormWindowState.Minimized;
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

        private void _LeftHalfScreen(UpdateLayoutBehavior updateLayoutBehavior, UpdateLayoutEventType updateLayoutEventType) // 使窗体占据桌面的左半区域。
        {
            _FormState = FormState.HighAsScreen;

            Rectangle OldBounds = _Bounds_Current;
            _Bounds_Current_Size = new Size(Math.Max(_MinimumBoundsWidth, Math.Min(_MaximumBoundsWidth, _PrimaryScreenClient.Width / 2)), Math.Max(_MinimumBoundsHeight, Math.Min(_MaximumBoundsHeight, _PrimaryScreenClient.Height)));
            _Bounds_Current_Location = _PrimaryScreenClient.Location;

            _FormTitleBar.OnFormStateChanged();
            _FormBorder.OnFormStateChanged();

            Rectangle NewBounds = _Bounds_Current;
            _Bounds_Current = OldBounds;

            _SetBoundsAndUpdateLayout(NewBounds, updateLayoutBehavior, updateLayoutEventType);

            if (updateLayoutEventType != UpdateLayoutEventType.None)
            {
                _OnFormStateChanged();
            }
        }

        private void _RightHalfScreen(UpdateLayoutBehavior updateLayoutBehavior, UpdateLayoutEventType updateLayoutEventType) // 使窗体占据桌面的右半区域。
        {
            _FormState = FormState.HighAsScreen;

            Rectangle OldBounds = _Bounds_Current;
            _Bounds_Current_Size = new Size(Math.Max(_MinimumBoundsWidth, Math.Min(_MaximumBoundsWidth, _PrimaryScreenClient.Width / 2)), Math.Max(_MinimumBoundsHeight, Math.Min(_MaximumBoundsHeight, _PrimaryScreenClient.Height)));
            _Bounds_Current_Location = new Point(_PrimaryScreenClient.Right - _Bounds_Current_Width, _PrimaryScreenClient.Y);

            _FormTitleBar.OnFormStateChanged();
            _FormBorder.OnFormStateChanged();

            Rectangle NewBounds = _Bounds_Current;
            _Bounds_Current = OldBounds;

            _SetBoundsAndUpdateLayout(NewBounds, updateLayoutBehavior, updateLayoutEventType);

            if (updateLayoutEventType != UpdateLayoutEventType.None)
            {
                _OnFormStateChanged();
            }
        }

        private void _HighAsScreen(UpdateLayoutBehavior updateLayoutBehavior, UpdateLayoutEventType updateLayoutEventType) // 使窗体与桌面的高度相同。
        {
            _FormState = FormState.HighAsScreen;

            Rectangle OldBounds = _Bounds_Current;
            _Bounds_Current_Height = Math.Max(_MinimumBoundsHeight, Math.Min(_MaximumBoundsHeight, _PrimaryScreenClient.Height));
            _Bounds_Current_Y = _PrimaryScreenClient.Y;

            _FormTitleBar.OnFormStateChanged();
            _FormBorder.OnFormStateChanged();

            Rectangle NewBounds = _Bounds_Current;
            _Bounds_Current = OldBounds;

            _SetBoundsAndUpdateLayout(NewBounds, updateLayoutBehavior, updateLayoutEventType);

            if (updateLayoutEventType != UpdateLayoutEventType.None)
            {
                _OnFormStateChanged();
            }
        }

        private void _TopLeftQuarterScreen(UpdateLayoutBehavior updateLayoutBehavior, UpdateLayoutEventType updateLayoutEventType) // 使窗体占据桌面的左上四分之一区域。
        {
            _FormState = FormState.QuarterScreen;

            Rectangle OldBounds = _Bounds_Current;
            _Bounds_Current_Size = _Bounds_QuarterScreen_Size = new Size(Math.Max(_MinimumBoundsWidth, Math.Min(_MaximumBoundsWidth, _PrimaryScreenClient.Width / 2)), Math.Max(_MinimumBoundsHeight, Math.Min(_MaximumBoundsHeight, _PrimaryScreenClient.Height / 2)));
            _Bounds_Current_Location = _Bounds_QuarterScreen_Location = _PrimaryScreenClient.Location;

            _FormTitleBar.OnFormStateChanged();
            _FormBorder.OnFormStateChanged();

            Rectangle NewBounds = _Bounds_Current;
            _Bounds_Current = OldBounds;

            _SetBoundsAndUpdateLayout(NewBounds, updateLayoutBehavior, updateLayoutEventType);

            if (updateLayoutEventType != UpdateLayoutEventType.None)
            {
                _OnFormStateChanged();
            }
        }

        private void _TopRightQuarterScreen(UpdateLayoutBehavior updateLayoutBehavior, UpdateLayoutEventType updateLayoutEventType) // 使窗体占据桌面的右上四分之一区域。
        {
            _FormState = FormState.QuarterScreen;

            Rectangle OldBounds = _Bounds_Current;
            _Bounds_Current_Size = _Bounds_QuarterScreen_Size = new Size(Math.Max(_MinimumBoundsWidth, Math.Min(_MaximumBoundsWidth, _PrimaryScreenClient.Width / 2)), Math.Max(_MinimumBoundsHeight, Math.Min(_MaximumBoundsHeight, _PrimaryScreenClient.Height / 2)));
            _Bounds_Current_Location = _Bounds_QuarterScreen_Location = new Point(_PrimaryScreenClient.Right - _Bounds_Current_Width, _PrimaryScreenClient.Y);

            _FormTitleBar.OnFormStateChanged();
            _FormBorder.OnFormStateChanged();

            Rectangle NewBounds = _Bounds_Current;
            _Bounds_Current = OldBounds;

            _SetBoundsAndUpdateLayout(NewBounds, updateLayoutBehavior, updateLayoutEventType);

            if (updateLayoutEventType != UpdateLayoutEventType.None)
            {
                _OnFormStateChanged();
            }
        }

        private void _BottomLeftQuarterScreen(UpdateLayoutBehavior updateLayoutBehavior, UpdateLayoutEventType updateLayoutEventType) // 使窗体占据桌面的左下四分之一区域。
        {
            _FormState = FormState.QuarterScreen;

            Rectangle OldBounds = _Bounds_Current;
            _Bounds_Current_Size = _Bounds_QuarterScreen_Size = new Size(Math.Max(_MinimumBoundsWidth, Math.Min(_MaximumBoundsWidth, _PrimaryScreenClient.Width / 2)), Math.Max(_MinimumBoundsHeight, Math.Min(_MaximumBoundsHeight, _PrimaryScreenClient.Height / 2)));
            _Bounds_Current_Location = _Bounds_QuarterScreen_Location = new Point(_PrimaryScreenClient.X, _PrimaryScreenClient.Bottom - _Bounds_Current_Height);

            _FormTitleBar.OnFormStateChanged();
            _FormBorder.OnFormStateChanged();

            Rectangle NewBounds = _Bounds_Current;
            _Bounds_Current = OldBounds;

            _SetBoundsAndUpdateLayout(NewBounds, updateLayoutBehavior, updateLayoutEventType);

            if (updateLayoutEventType != UpdateLayoutEventType.None)
            {
                _OnFormStateChanged();
            }
        }

        private void _BottomRightQuarterScreen(UpdateLayoutBehavior updateLayoutBehavior, UpdateLayoutEventType updateLayoutEventType) // 使窗体占据桌面的右下四分之一区域。
        {
            _FormState = FormState.QuarterScreen;

            Rectangle OldBounds = _Bounds_Current;
            _Bounds_Current_Size = _Bounds_QuarterScreen_Size = new Size(Math.Max(_MinimumBoundsWidth, Math.Min(_MaximumBoundsWidth, _PrimaryScreenClient.Width / 2)), Math.Max(_MinimumBoundsHeight, Math.Min(_MaximumBoundsHeight, _PrimaryScreenClient.Height / 2)));
            _Bounds_Current_Location = _Bounds_QuarterScreen_Location = new Point(_PrimaryScreenClient.Right - _Bounds_Current_Width, _PrimaryScreenClient.Bottom - _Bounds_Current_Height);

            _FormTitleBar.OnFormStateChanged();
            _FormBorder.OnFormStateChanged();

            Rectangle NewBounds = _Bounds_Current;
            _Bounds_Current = OldBounds;

            _SetBoundsAndUpdateLayout(NewBounds, updateLayoutBehavior, updateLayoutEventType);

            if (updateLayoutEventType != UpdateLayoutEventType.None)
            {
                _OnFormStateChanged();
            }
        }

        private void _Close() // 关闭窗体。
        {
            _FormClient.Closed -= FormClient_Closed;
            _FormTitleBar.Closed -= FormTitleBar_Closed;
            _FormBorder.Closed -= FormBorder_Closed;
            _SplashScreen.Closed -= SplashScreen_Closed;

            //

            if (!_LoadingNow && !_ClosingNow)
            {
                _ClosingNow = true;

                //

                _FormTitleBar.OnClosing();

                //

                _FormClosingAsyncWorker.RunWorkerAsync();
            }
            else
            {
                _SplashScreen.OnClosing();
                _SplashScreen.Visible = true;

                _FormClient.Visible = false;

                //

                _FormClient.ShowInTaskbar = false;

                //

                double Opacity = _Opacity;
                int Bounds_Current_Y = _Bounds_Current_Y;

                Animation.Frame Frame = (frameId, frameCount, msPerFrame) =>
                {
                    double Pct_F = (frameId == frameCount ? 1 : 1 - Math.Pow(1 - (double)frameId / frameCount, 2));

                    _Opacity = Opacity * (1 - Pct_F);

                    _SplashScreen.Opacity = _Opacity;
                    _FormTitleBar.Opacity = (_EnableFormTitleBarTransparent ? _Opacity * 0.9 : _Opacity);
                    _FormBorder.OnOpacityChanged();

                    _Bounds_Current_Y = Bounds_Current_Y - (int)(16 * Pct_F);

                    _UpdateLayout(UpdateLayoutEventType.None);
                };

                Animation.Show(Frame, 9, 15);

                //

                _FormClient.Close();
            }
        }

        private void _AltF4(object sender, EventArgs e) // 通过 Alt + F4 或其他非正常方式关闭窗体。
        {
            _FormClient.Closed -= FormClient_Closed;
            _FormTitleBar.Closed -= FormTitleBar_Closed;
            _FormBorder.Closed -= FormBorder_Closed;
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

            if (!object.ReferenceEquals(sender, _FormClient))
            {
                _FormClient.Close();
            }
        }

        //

        private void _Ctor(Form formClient, FormManager owner) // 为以 Form 对象与 FormManager 对象为参数的构造函数提供实现。
        {
            if (formClient == null)
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

            _FormClient = formClient;

            _FormTitleBar = new FormTitleBar(this);
            _FormBorder = new FormBorder(this);
            _SplashScreen = new SplashScreen(this);

            _FormTitleBar.Owner = _FormBorder.Owner = _SplashScreen.Owner = _FormClient;

            _FormTitleBar.Icon = _FormBorder.Icon = _SplashScreen.Icon = _FormClient.Icon;

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
                _Theme = _Owner._Theme;
                _ThemeColor = _Owner._ThemeColor;
                _ShowFormTitleBarColor = _Owner._ShowFormTitleBarColor;
            }

            //

            _FormClient.FormBorderStyle = FormBorderStyle.None;
            _FormClient.StartPosition = FormStartPosition.Manual;
            _FormClient.WindowState = FormWindowState.Normal;

            //

            _FormClient.Opacity = _FormTitleBar.Opacity = _FormBorder.Opacity = _SplashScreen.Opacity = 0;

            //

            _RecommendColors = new RecommendColors(_Theme, _ThemeColor, _ShowFormTitleBarColor);

            //

            _FormLoadingAsyncWorker = new BackgroundWorker();
            _FormLoadingAsyncWorker.DoWork += FormLoadingAsyncWorker_DoWork;
            _FormLoadingAsyncWorker.RunWorkerCompleted += FormLoadingAsyncWorker_RunWorkerCompleted;

            _FormClosingAsyncWorker = new BackgroundWorker();
            _FormClosingAsyncWorker.DoWork += FormClosingAsyncWorker_DoWork;
            _FormClosingAsyncWorker.RunWorkerCompleted += FormClosingAsyncWorker_RunWorkerCompleted;

            //

            _FormClient.Load += FormClient_Load;
            _FormClient.Closed += FormClient_Closed;
            _FormClient.SizeChanged += FormClient_SizeChanged;

            _FormTitleBar.Closed += FormTitleBar_Closed;

            _FormBorder.Closed += FormBorder_Closed;

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

        private void FormClient_Load(object sender, EventArgs e) // FormClient 的 Load 事件的回调函数。
        {
            _FormClient.Visible = false;

            //

            _FormTitleBar.Show();
            _FormBorder.Show();
            _SplashScreen.Show();

            //

            Size = _Bounds_Current_Size;

            if (X == int.MinValue || Y == int.MinValue)
            {
                Rectangle Rect = (_Owner != null ? _Owner.Bounds : _PrimaryScreenClient);

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

            Location = _Bounds_Current_Location;

            _UpdateLayout(UpdateLayoutEventType.None);

            //

            if (string.IsNullOrEmpty(_Caption))
            {
                if (!string.IsNullOrEmpty(_FormClient.Text))
                {
                    _Caption = _FormClient.Text;
                }
                else if (_IsMainForm)
                {
                    _Caption = Application.ProductName;
                }
            }

            if (_FormClient.Text != _Caption)
            {
                _FormClient.Text = _Caption;
            }

            //

            _FormTitleBar.OnFormStyleChanged();
            _FormBorder.OnFormStyleChanged();

            _FormTitleBar.OnEnableFullScreenChanged();
            _FormTitleBar.OnCaptionChanged();
            _FormTitleBar.OnThemeChanged();

            _SplashScreen.OnThemeChanged();

            //

            _FormClient.TopMost = _FormTitleBar.TopMost = _FormBorder.TopMost = _SplashScreen.TopMost = _TopMost;

            _FormClient.ShowInTaskbar = _ShowInTaskbar;

            //

            _FormClient.Enabled = _FormTitleBar.Enabled = _FormBorder.Enabled = _SplashScreen.Enabled = _Enabled;

            //

            _FormClient.BackColor = RecommendColors.FormBackground.ToColor();

            //

            _FormTitleBar.BringToFront();
            _FormClient.BringToFront();
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

            _FormBorder.Opacity = 1;
            _FormBorder.OnOpacityChanged();

            //

            double Opacity = _Opacity;
            int Bounds_Current_Y = _Bounds_Current_Y;

            Animation.Frame Frame = (frameId, frameCount, msPerFrame) =>
            {
                double Pct_F = (frameId == frameCount ? 1 : 1 - Math.Pow(1 - (double)frameId / frameCount, 2));

                _Opacity = Opacity * Pct_F;

                _SplashScreen.Opacity = _Opacity;
                _FormTitleBar.Opacity = (_EnableFormTitleBarTransparent ? _Opacity * 0.9 : _Opacity);
                _FormBorder.OnOpacityChanged();

                _Bounds_Current_Y = Bounds_Current_Y - (int)(16 * (1 - Pct_F));

                _UpdateLayout(UpdateLayoutEventType.None);
            };

            Animation.Show(Frame, 9, 15);

            //

            _FormTitleBar.OnLoading();
            _SplashScreen.OnLoading();

            //

            _Initialized = true;

            //

            _FormLoadingAsyncWorker.RunWorkerAsync();
        }

        private void FormClient_Closed(object sender, EventArgs e) // FormClient 的 Closed 事件的回调函数。
        {
            _AltF4(sender, e);
        }

        private void FormClient_SizeChanged(object sender, EventArgs e) // FormClient 的 SizeChanged 事件的回调函数。
        {
            if (_FormStyle == FormStyle.Dialog && _FormClient.WindowState == FormWindowState.Minimized)
            {
                _FormClient.WindowState = FormWindowState.Normal;
            }

            //

            if (_FormState != FormState.FullScreen && _FormClient.WindowState == FormWindowState.Maximized)
            {
                _FormClient.WindowState = FormWindowState.Normal;
            }
            else if (_FormState == FormState.FullScreen && _FormClient.WindowState == FormWindowState.Normal)
            {
                _FormClient.WindowState = FormWindowState.Maximized;
            }

            //

            if (_LastFormWindowState != _FormClient.WindowState)
            {
                FormWindowState _FormWindowState = _FormClient.WindowState;

                if ((_LastFormWindowState != FormWindowState.Minimized && _FormWindowState == FormWindowState.Minimized) || (_LastFormWindowState == FormWindowState.Minimized && _FormWindowState != FormWindowState.Minimized))
                {
                    _OnFormStateChanged();
                }

                _LastFormWindowState = _FormWindowState;
            }
        }

        private void FormTitleBar_Closed(object sender, EventArgs e) // FormTitleBar 的 Closed 事件的回调函数。
        {
            _AltF4(sender, e);
        }

        private void FormBorder_Closed(object sender, EventArgs e) // FormBorder 的 Closed 事件的回调函数。
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
            _FormClient.Visible = true;
            _FormClient.Opacity = _Opacity;

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

            _FormClient.Visible = false;

            //

            _FormClient.ShowInTaskbar = false;

            //

            double Opacity = _Opacity;
            int Bounds_Current_Y = _Bounds_Current_Y;

            Animation.Frame Frame = (frameId, frameCount, msPerFrame) =>
            {
                double Pct_F = (frameId == frameCount ? 1 : 1 - Math.Pow(1 - (double)frameId / frameCount, 2));

                _Opacity = Opacity * (1 - Pct_F);

                _SplashScreen.Opacity = _Opacity;
                _FormTitleBar.Opacity = (_EnableFormTitleBarTransparent ? _Opacity * 0.9 : _Opacity);
                _FormBorder.OnOpacityChanged();

                _Bounds_Current_Y = Bounds_Current_Y - (int)(16 * Pct_F);

                _UpdateLayout(UpdateLayoutEventType.None);
            };

            Animation.Show(Frame, 9, 15);

            //

            _OnClosed();

            //

            _FormClient.Close();
        }

        #endregion

        #region 构造与析构函数

        /// <summary>
        /// 使用 Form 对象初始化 FormManager 的新实例。
        /// </summary>
        /// <param name="formClient">Form 对象，表示窗体工作区。</param>
        public FormManager(Form formClient)
        {
            _Ctor(formClient, null);
        }

        /// <summary>
        /// 使用 Form 对象与 FormManager 对象初始化 FormManager 的新实例。
        /// </summary>
        /// <param name="formClient">Form 对象，表示窗体工作区。</param>
        /// <param name="owner">FormManager 对象，表示拥有此窗体的窗体的窗体管理器。</param>
        public FormManager(Form formClient, FormManager owner)
        {
            _Ctor(formClient, owner);
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
        /// 获取表示此窗体是否为主窗体的布尔值。
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
        /// 获取拥有此窗体的窗体的窗体管理器。
        /// </summary>
        public FormManager Owner
        {
            get
            {
                return _Owner;
            }
        }

        /// <summary>
        /// 获取表示此窗体拥有的所有窗体的窗体管理器列表。
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
        /// 获取或设置窗体的样式。
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
                        _FormTitleBar.OnFormStyleChanged();
                        _FormBorder.OnFormStyleChanged();

                        if (_FormStyle == FormStyle.Dialog && _FormClient.WindowState == FormWindowState.Minimized)
                        {
                            if (_FormState == FormState.FullScreen)
                            {
                                _FormClient.WindowState = FormWindowState.Maximized;
                            }
                            else
                            {
                                _FormClient.WindowState = FormWindowState.Normal;
                            }
                        }
                        else if (_FormStyle != FormStyle.Sizable && (_FormState != FormState.Normal && _FormState != FormState.FullScreen))
                        {
                            _Return(UpdateLayoutBehavior.Animate, UpdateLayoutEventType.All);
                        }

                        _OnFormStyleChanged();
                    };

                    _FormClient.Invoke(InvokeMethod);
                }
            }
        }

        /// <summary>
        /// 获取或设置表示是否允许窗体以全屏幕模式运行的布尔值。
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
                        _FormTitleBar.OnEnableFullScreenChanged();

                        if (!_EnableFullScreen && _FormState == FormState.FullScreen)
                        {
                            _ExitFullScreen(UpdateLayoutBehavior.Animate, UpdateLayoutEventType.All);
                        }

                        _OnFormStyleChanged();
                    };

                    _FormClient.Invoke(InvokeMethod);
                }
            }
        }

        /// <summary>
        /// 获取或设置表示是否允许窗体置于顶层的布尔值。
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
                        _FormClient.TopMost = _TopMost;

                        _OnFormStyleChanged();
                    };

                    _FormClient.Invoke(InvokeMethod);
                }
            }
        }

        /// <summary>
        /// 获取或设置表示窗体是否在任务栏显示的布尔值。
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
                        _FormClient.ShowInTaskbar = _ShowInTaskbar;

                        _FormBorder.BringToFront();
                        _FormTitleBar.BringToFront();
                        _FormClient.BringToFront();
                        _FormClient.Focus();

                        _OnFormStyleChanged();
                    };

                    _FormClient.Invoke(InvokeMethod);
                }
            }
        }

        //

        /// <summary>
        /// 获取或设置窗体标题栏的高度。
        /// </summary>
        public int FormTitleBarHeight
        {
            get
            {
                return _FormTitleBarHeight;
            }

            set
            {
                _FormTitleBarHeight = Math.Max(24, Math.Min(_PrimaryScreenBounds.Height, value));

                if (_Initialized)
                {
                    Action InvokeMethod = () =>
                    {
                        _UpdateLayout(UpdateLayoutEventType.All);
                    };

                    _FormClient.Invoke(InvokeMethod);
                }
            }
        }

        /// <summary>
        /// 获取窗体边框的大小。
        /// </summary>
        public int FormBorderSize
        {
            get
            {
                return _FormBorderSize;
            }
        }

        //

        /// <summary>
        /// 获取或设置窗体的最小大小。
        /// </summary>
        public Size MinimumSize
        {
            get
            {
                return new Size(__MinimumWidth, __MinimumHeight);
            }

            set
            {
                if (!_Initialized)
                {
                    __MinimumWidth = Math.Max(0, value.Width);
                    __MinimumHeight = Math.Max(0, value.Height);
                }
            }
        }

        /// <summary>
        /// 获取或设置窗体的最大大小。
        /// </summary>
        public Size MaximumSize
        {
            get
            {
                return new Size(__MaximumWidth, __MaximumHeight);
            }

            set
            {
                if (!_Initialized)
                {
                    __MaximumWidth = Math.Max(0, value.Width);
                    __MaximumHeight = Math.Max(0, value.Height);
                }
            }
        }

        //

        /// <summary>
        /// 获取或设置窗体的状态。
        /// </summary>
        public FormState FormState
        {
            get
            {
                if (_FormClient.WindowState == FormWindowState.Minimized)
                {
                    return FormState.Minimized;
                }

                return _FormState;
            }

            set
            {
                Action InvokeMethod = () =>
                {
                    if (_FormClient.WindowState == FormWindowState.Minimized)
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

                _FormClient.Invoke(InvokeMethod);
            }
        }

        //

        /// <summary>
        /// 获取或设置表示窗体是否对用户交互作出响应的布尔值。
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
                        _FormClient.Enabled = _FormTitleBar.Enabled = _FormBorder.Enabled = _SplashScreen.Enabled = _Enabled;

                        _OnEnabledChanged();
                    };

                    _FormClient.Invoke(InvokeMethod);
                }
            }
        }

        /// <summary>
        /// 获取或设置窗体的标题。
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
                        _FormClient.Text = _Caption;

                        _FormTitleBar.OnCaptionChanged();

                        _OnCaptionChanged();
                    };

                    _FormClient.Invoke(InvokeMethod);
                }
            }
        }

        /// <summary>
        /// 获取或设置窗体标题的字体。
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
                        _FormTitleBar.OnCaptionChanged();
                    };

                    _FormClient.Invoke(InvokeMethod);
                }
            }
        }

        /// <summary>
        /// 获取或设置窗体的不透明度，取值范围为 [0, 1] 或 (1, 100]。
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
                        _FormClient.Opacity = _SplashScreen.Opacity = _Opacity;
                        _FormTitleBar.Opacity = (_EnableFormTitleBarTransparent ? _Opacity * 0.9 : _Opacity);

                        _FormBorder.OnOpacityChanged();

                        _OnOpacityChanged();
                    };

                    _FormClient.Invoke(InvokeMethod);
                }
            }
        }

        /// <summary>
        /// 获取或设置窗体的主题。
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

                _RecommendColors._Ctor(_Theme, _ThemeColor, _ShowFormTitleBarColor);

                if (_Initialized)
                {
                    Action InvokeMethod = () =>
                    {
                        _FormClient.BackColor = RecommendColors.FormBackground.ToColor();

                        _FormTitleBar.OnThemeChanged();
                        _SplashScreen.OnThemeChanged();

                        _OnThemeChanged();
                    };

                    _FormClient.Invoke(InvokeMethod);
                }
            }
        }

        /// <summary>
        /// 获取或设置窗体的主题色。
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

                _RecommendColors._Ctor(_Theme, _ThemeColor, _ShowFormTitleBarColor);

                if (_Initialized)
                {
                    Action InvokeMethod = () =>
                    {
                        _FormClient.BackColor = RecommendColors.FormBackground.ToColor();

                        _FormTitleBar.OnThemeColorChanged();
                        _SplashScreen.OnThemeColorChanged();

                        _OnThemeColorChanged();
                    };

                    _FormClient.Invoke(InvokeMethod);
                }
            }
        }

        /// <summary>
        /// 获取或设置表示是否在窗体标题栏上显示主题色的布尔值。
        /// </summary>
        public bool ShowFormTitleBarColor
        {
            get
            {
                return _ShowFormTitleBarColor;
            }

            set
            {
                _ShowFormTitleBarColor = value;

                _RecommendColors._Ctor(_Theme, _ThemeColor, _ShowFormTitleBarColor);

                if (_Initialized)
                {
                    Action InvokeMethod = () =>
                    {
                        _FormTitleBar.OnThemeChanged();
                        _SplashScreen.OnThemeChanged();
                    };

                    _FormClient.Invoke(InvokeMethod);
                }
            }
        }

        /// <summary>
        /// 获取或设置表示是否允许以半透明方式显示窗体标题栏的布尔值。
        /// </summary>
        public bool EnableFormTitleBarTransparent
        {
            get
            {
                return _EnableFormTitleBarTransparent;
            }

            set
            {
                _EnableFormTitleBarTransparent = value;

                if (_Initialized)
                {
                    Action InvokeMethod = () =>
                    {
                        _FormTitleBar.Opacity = (_EnableFormTitleBarTransparent ? _Opacity * 0.9 : _Opacity);
                    };

                    _FormClient.Invoke(InvokeMethod);
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
        /// 获取或设置窗体在桌面的左边距。
        /// </summary>
        public int X
        {
            get
            {
                return _Bounds_Current_X;
            }

            set
            {
                int _X = Math.Max(_PrimaryScreenBounds.X - Width + 1, Math.Min(value, _PrimaryScreenBounds.Right - 1));

                switch (_FormState)
                {
                    case FormState.Normal:
                        _Bounds_Current_X = _Bounds_Normal_X = _X;
                        break;

                    case FormState.QuarterScreen:
                        _Bounds_Current_X = _Bounds_QuarterScreen_X = _X;
                        break;

                    case FormState.HighAsScreen:
                        _Bounds_Current_X = _Bounds_Normal_X = _X;
                        break;
                }

                if (_Initialized)
                {
                    Action InvokeMethod = () =>
                    {
                        _UpdateLayout(UpdateLayoutEventType.All);
                    };

                    _FormClient.Invoke(InvokeMethod);
                }
            }
        }

        /// <summary>
        /// 获取或设置窗体在桌面的上边距。
        /// </summary>
        public int Y
        {
            get
            {
                return _Bounds_Current_Y;
            }

            set
            {
                int _Y = Math.Max(_PrimaryScreenBounds.Y - Height + 1, Math.Min(value, _PrimaryScreenBounds.Bottom - 1));

                switch (_FormState)
                {
                    case FormState.Normal:
                        _Bounds_Current_Y = _Bounds_Normal_Y = _Y;
                        break;

                    case FormState.QuarterScreen:
                        _Bounds_Current_Y = _Bounds_QuarterScreen_Y = _Y;
                        break;
                }

                if (_Initialized)
                {
                    Action InvokeMethod = () =>
                    {
                        _UpdateLayout(UpdateLayoutEventType.All);
                    };

                    _FormClient.Invoke(InvokeMethod);
                }
            }
        }

        /// <summary>
        /// 获取或设置窗体在桌面的左边距。
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
        /// 获取窗体在桌面的右边距。
        /// </summary>
        public int Right
        {
            get
            {
                return (X + Width);
            }
        }

        /// <summary>
        /// 获取或设置窗体在桌面的上边距。
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
        /// 获取窗体在桌面的下边距。
        /// </summary>
        public int Bottom
        {
            get
            {
                return (Y + Height);
            }
        }

        /// <summary>
        /// 获取或设置窗体在桌面的位置。
        /// </summary>
        public Point Location
        {
            get
            {
                return _Bounds_Current_Location;
            }

            set
            {
                Point _Location = new Point(Math.Max(_PrimaryScreenBounds.X - Width + 1, Math.Min(value.X, _PrimaryScreenBounds.Right - 1)), Math.Max(_PrimaryScreenBounds.Y - Height + 1, Math.Min(value.Y, _PrimaryScreenBounds.Bottom - 1)));

                switch (_FormState)
                {
                    case FormState.Normal:
                        _Bounds_Current_Location = _Bounds_Normal_Location = _Location;
                        break;

                    case FormState.QuarterScreen:
                        _Bounds_Current_Location = _Bounds_QuarterScreen_Location = _Location;
                        break;

                    case FormState.HighAsScreen:
                        _Bounds_Current_X = _Bounds_Normal_X = _Location.X;
                        break;
                }

                if (_Initialized)
                {
                    Action InvokeMethod = () =>
                    {
                        _UpdateLayout(UpdateLayoutEventType.All);
                    };

                    _FormClient.Invoke(InvokeMethod);
                }
            }
        }

        /// <summary>
        /// 获取或设置窗体的宽度。
        /// </summary>
        public int Width
        {
            get
            {
                return _Bounds_Current_Width;
            }

            set
            {
                switch (_FormState)
                {
                    case FormState.Normal:
                        _Bounds_Current_Width = _Bounds_Normal_Width = value;
                        break;

                    case FormState.QuarterScreen:
                        _Bounds_Current_Width = _Bounds_QuarterScreen_Width = value;
                        break;

                    case FormState.HighAsScreen:
                        _Bounds_Current_Width = _Bounds_Normal_Width = value;
                        break;
                }

                if (_Initialized)
                {
                    Action InvokeMethod = () =>
                    {
                        _UpdateLayout(UpdateLayoutEventType.All);
                    };

                    _FormClient.Invoke(InvokeMethod);
                }
            }
        }

        /// <summary>
        /// 获取或设置窗体的高度。
        /// </summary>
        public int Height
        {
            get
            {
                return _Bounds_Current_Height;
            }

            set
            {
                switch (_FormState)
                {
                    case FormState.Normal:
                        _Bounds_Current_Height = _Bounds_Normal_Height = value;
                        break;

                    case FormState.QuarterScreen:
                        _Bounds_Current_Height = _Bounds_QuarterScreen_Height = value;
                        break;
                }

                if (_Initialized)
                {
                    Action InvokeMethod = () =>
                    {
                        _UpdateLayout(UpdateLayoutEventType.All);
                    };

                    _FormClient.Invoke(InvokeMethod);
                }
            }
        }

        /// <summary>
        /// 获取或设置窗体的大小。
        /// </summary>
        public Size Size
        {
            get
            {
                return _Bounds_Current_Size;
            }

            set
            {
                switch (_FormState)
                {
                    case FormState.Normal:
                        _Bounds_Current_Size = _Bounds_Normal_Size = value;
                        break;

                    case FormState.QuarterScreen:
                        _Bounds_Current_Size = _Bounds_QuarterScreen_Size = value;
                        break;

                    case FormState.HighAsScreen:
                        _Bounds_Current_Width = _Bounds_Normal_Width = value.Width;
                        break;
                }

                if (_Initialized)
                {
                    Action InvokeMethod = () =>
                    {
                        _UpdateLayout(UpdateLayoutEventType.All);
                    };

                    _FormClient.Invoke(InvokeMethod);
                }
            }
        }

        /// <summary>
        /// 获取或设置窗体的位置与大小。
        /// </summary>
        public Rectangle Bounds
        {
            get
            {
                return _Bounds_Current;
            }

            set
            {
                Rectangle _Bounds = new Rectangle();
                _Bounds.Size = new Size(Math.Max(_MinimumBoundsWidth, Math.Min(value.Width, _MaximumBoundsWidth)), Math.Max(_MinimumBoundsHeight, Math.Min(value.Height, _MaximumBoundsHeight)));
                _Bounds.Location = new Point(Math.Max(_PrimaryScreenBounds.X - _Bounds.Width + 1, Math.Min(value.X, _PrimaryScreenBounds.Right - 1)), Math.Max(_PrimaryScreenBounds.Y - _Bounds.Height + 1, Math.Min(value.Y, _PrimaryScreenBounds.Bottom - 1)));

                switch (_FormState)
                {
                    case FormState.Normal:
                        _Bounds_Current = _Bounds_Normal = _Bounds;
                        break;

                    case FormState.QuarterScreen:
                        _Bounds_Current = _Bounds_QuarterScreen = _Bounds;
                        break;

                    case FormState.HighAsScreen:
                        _Bounds_Current_X = _Bounds_Normal_X = _Bounds.X;
                        _Bounds_Current_Width = _Bounds_Normal_Width = _Bounds.Width;
                        break;
                }

                if (_Initialized)
                {
                    Action InvokeMethod = () =>
                    {
                        _UpdateLayout(UpdateLayoutEventType.All);
                    };

                    _FormClient.Invoke(InvokeMethod);
                }
            }
        }

        //

        /// <summary>
        /// 获取或设置窗体的工作区的位置。
        /// </summary>
        public Point ClientLocation
        {
            get
            {
                return new Point(_Bounds_Current_X, _Bounds_Current_Y + _FormTitleBarHeight);
            }

            set
            {
                Location = new Point(value.X, value.Y - _FormTitleBarHeight);
            }
        }

        /// <summary>
        /// 获取或设置窗体的工作区的大小。
        /// </summary>
        public Size ClientSize
        {
            get
            {
                return new Size(_Bounds_Current_Width, _Bounds_Current_Height - _FormTitleBarHeight);
            }

            set
            {
                Size = new Size(value.Width, value.Height + _FormTitleBarHeight);
            }
        }

        /// <summary>
        /// 获取或设置窗体的工作区的位置与大小。
        /// </summary>
        public Rectangle ClientBounds
        {
            get
            {
                return new Rectangle(new Point(_Bounds_Current_X, _Bounds_Current_Y + _FormTitleBarHeight), new Size(_Bounds_Current_Width, _Bounds_Current_Height - _FormTitleBarHeight));
            }

            set
            {
                Bounds = new Rectangle(new Point(value.X, value.Y - _FormTitleBarHeight), new Size(value.Width, value.Height + _FormTitleBarHeight));
            }
        }

        //

        /// <summary>
        /// 设置用于验证是否允许窗体还原的方法。
        /// </summary>
        public Predicate<EventArgs> ReturnVerification
        {
            set
            {
                _ReturnVerification = value;
            }
        }

        /// <summary>
        /// 设置用于验证是否允许窗体最大化的方法。
        /// </summary>
        public Predicate<EventArgs> MaximizeVerification
        {
            set
            {
                _MaximizeVerification = value;
            }
        }

        /// <summary>
        /// 设置用于验证是否允许窗体进入全屏的方法。
        /// </summary>
        public Predicate<EventArgs> EnterFullScreenVerification
        {
            set
            {
                _EnterFullScreenVerification = value;
            }
        }

        /// <summary>
        /// 设置用于验证是否允许窗体退出全屏的方法。
        /// </summary>
        public Predicate<EventArgs> ExitFullScreenVerification
        {
            set
            {
                _ExitFullScreenVerification = value;
            }
        }

        /// <summary>
        /// 设置用于验证是否允许窗体关闭的方法。
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
        /// 在窗体加载时发生。
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
        /// 在窗体加载后发生。
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
        /// 在窗体关闭时发生。
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
        /// 在窗体关闭后发生。
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
        /// 在窗体移动时发生。
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
        /// 在窗体的位置更改时发生。
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
        /// 在窗体的大小调整时发生。
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
        /// 在窗体的大小更改时发生。
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
        /// 在窗体的样式更改时发生。
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
        /// 在窗体的状态更改时发生。
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
        /// 在表示窗体是否对用户交互作出响应的布尔值更改时发生。
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
        /// 在窗体的标题更改时发生。
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
        /// 在窗体的不透明度更改时发生。
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
        /// 在窗体的主题更改时发生。
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
        /// 在窗体的主题色更改时发生。
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
        /// 使窗体还原至普通大小。
        /// </summary>
        public bool Return()
        {
            Func<bool> InvokeMethod = () =>
            {
                if (_FormClient.WindowState == FormWindowState.Minimized)
                {
                    if (_FormState == FormState.FullScreen)
                    {
                        _FormClient.WindowState = FormWindowState.Maximized;
                    }
                    else
                    {
                        _FormClient.WindowState = FormWindowState.Normal;
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

            return (bool)_FormClient.Invoke(InvokeMethod);
        }

        /// <summary>
        /// 使窗体最小化至任务栏。
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

            return (bool)_FormClient.Invoke(InvokeMethod);
        }

        /// <summary>
        /// 使窗体最大化。
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

            return (bool)_FormClient.Invoke(InvokeMethod);
        }

        /// <summary>
        /// 使窗体进入全屏幕模式。
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

            return (bool)_FormClient.Invoke(InvokeMethod);
        }

        /// <summary>
        /// 使窗体退出全屏幕模式。
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

            return (bool)_FormClient.Invoke(InvokeMethod);
        }

        /// <summary>
        /// 使窗体占据桌面的左半区域。
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

            return (bool)_FormClient.Invoke(InvokeMethod);
        }

        /// <summary>
        /// 使窗体占据桌面的右半区域。
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

            return (bool)_FormClient.Invoke(InvokeMethod);
        }

        /// <summary>
        /// 使窗体与桌面的高度相同。
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

            return (bool)_FormClient.Invoke(InvokeMethod);
        }

        /// <summary>
        /// 使窗体占据桌面的左上四分之一区域。
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

            return (bool)_FormClient.Invoke(InvokeMethod);
        }

        /// <summary>
        /// 使窗体占据桌面的右上四分之一区域。
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

            return (bool)_FormClient.Invoke(InvokeMethod);
        }

        /// <summary>
        /// 使窗体占据桌面的左下四分之一区域。
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

            return (bool)_FormClient.Invoke(InvokeMethod);
        }

        /// <summary>
        /// 使窗体占据桌面的右下四分之一区域。
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

            return (bool)_FormClient.Invoke(InvokeMethod);
        }

        /// <summary>
        /// 关闭窗体。
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

            return (bool)_FormClient.Invoke(InvokeMethod);
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