/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2018 chibayuki@foxmail.com

Com.WinForm.CaptionBar
Version 18.7.6.2250

This file is part of Com

Com is released under the GPLv3 license
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Drawing.Drawing2D;
using System.Threading;

namespace Com.WinForm
{
    internal partial class CaptionBar : Form // 标题栏。
    {
        #region 私有与内部成员

        private FormManager Me; // 窗口管理器。

        //

        private Point _CursorPositionOfMe = new Point(); // 鼠标指针在窗口的坐标。

        private bool _MeWillMove = false; // 是否即将移动窗口。
        private bool _MeIsMoving = false; // 是否正在移动窗口。

        private const int _ExtendDist = 2; // 扩展距离，用于某些鼠标动作的判定。

        //

        private bool _FullScreenButtonIsPointed = false; // 是否正在指向全屏幕按钮。
        private bool _FullScreenButtonIsPressed = false; // 是否正在按下全屏幕按钮。
        private bool _MinimizeButtonIsPointed = false; // 是否正在指向最小化按钮。
        private bool _MinimizeButtonIsPressed = false; // 是否正在按下最小化按钮。
        private bool _MaximizeButtonIsPointed = false; // 是否正在指向最大化按钮。
        private bool _MaximizeButtonIsPressed = false; // 是否正在按下最大化按钮。
        private bool _ExitButtonIsPointed = false; // 是否正在指向关闭按钮。
        private bool _ExitButtonIsPressed = false; // 是否正在按下关闭按钮。

        //

        private Bitmap _CaptionBarBitmap; // 标题栏绘图。

        private void _UpdateCaptionBarBitmap() // 更新标题栏绘图。
        {
            if (_CaptionBarBitmap != null)
            {
                _CaptionBarBitmap.Dispose();
            }

            _CaptionBarBitmap = new Bitmap(Math.Max(1, Panel_CaptionBar.Width), Math.Max(1, Panel_CaptionBar.Height));

            using (Graphics CreateFormCaptionBmp = Graphics.FromImage(_CaptionBarBitmap))
            {
                CreateFormCaptionBmp.SmoothingMode = SmoothingMode.AntiAlias;

                //

                CreateFormCaptionBmp.Clear(Me.RecommendColors.CaptionBar.ToColor());

                //

                if (Me.CaptionBarBackgroundImage != null)
                {
                    Bitmap BkgImg = Me.CaptionBarBackgroundImage;

                    if (BkgImg.Width > _CaptionBarBitmap.Width || BkgImg.Height > _CaptionBarBitmap.Height)
                    {
                        BkgImg = BkgImg.Clone(new Rectangle(new Point(0, 0), new Size(Math.Min(BkgImg.Width, _CaptionBarBitmap.Width), Math.Min(BkgImg.Height, _CaptionBarBitmap.Height))), BkgImg.PixelFormat);
                    }

                    CreateFormCaptionBmp.DrawImage(BkgImg, new Point(0, 0));
                }

                //

                if (Me.Caption.Length > 0)
                {
                    Rectangle CaptionArea = new Rectangle(new Point((Me.ShowIconOnCaptionBar ? Panel_FormIcon.Right : 0), 0), new Size(Math.Max(1, Panel_ControlBox.Left - (Me.ShowIconOnCaptionBar ? Panel_FormIcon.Right : 0)), Math.Max(1, Panel_CaptionBar.Height)));

                    Font CaptionFont = Me.CaptionFont;

                    string Caption = Me.Caption;

                    SizeF CaptionSizeF = TextRenderer.MeasureText(Caption, CaptionFont);

                    if (CaptionSizeF.Width > CaptionArea.Width && Caption.Length > 1)
                    {
                        for (int i = Caption.Length - 1; i >= 1; i--)
                        {
                            Caption = Caption.Substring(0, i);

                            SizeF CapSizeF = TextRenderer.MeasureText(Caption + "...", CaptionFont);

                            if (CapSizeF.Width <= CaptionArea.Width)
                            {
                                Caption += "...";

                                break;
                            }
                        }

                        CaptionSizeF = TextRenderer.MeasureText(Caption, CaptionFont);
                    }

                    if (CaptionSizeF.Width <= CaptionArea.Width)
                    {
                        RectangleF CaptionBounds = new RectangleF(new PointF(CaptionArea.X, Math.Max(0, CaptionArea.Y + (Math.Min(Panel_CaptionBar.Height, Panel_ControlBox.Height) - CaptionSizeF.Height) / 2)), new SizeF(CaptionArea.Width, Math.Max(1, CaptionArea.Height - Math.Max(0, Math.Min(Panel_CaptionBar.Height, Panel_ControlBox.Height) - CaptionSizeF.Height))));

                        PointF CaptionLocF = CaptionBounds.Location;

                        ContentAlignment CA = Me.CaptionAlign;

                        if (CA == ContentAlignment.TopLeft || CA == ContentAlignment.MiddleLeft || CA == ContentAlignment.BottomLeft)
                        {
                            CaptionLocF.X = CaptionBounds.X;
                        }
                        else if (CA == ContentAlignment.TopCenter || CA == ContentAlignment.MiddleCenter || CA == ContentAlignment.BottomCenter)
                        {
                            CaptionLocF.X = CaptionBounds.X + Math.Max(0, (CaptionBounds.Width - CaptionSizeF.Width) / 2);
                        }
                        else if (CA == ContentAlignment.TopRight || CA == ContentAlignment.MiddleRight || CA == ContentAlignment.BottomRight)
                        {
                            CaptionLocF.X = CaptionBounds.X + Math.Max(0, CaptionBounds.Width - CaptionSizeF.Width);
                        }

                        if (CA == ContentAlignment.TopLeft || CA == ContentAlignment.TopCenter || CA == ContentAlignment.TopRight)
                        {
                            CaptionLocF.Y = CaptionBounds.Y;
                        }
                        else if (CA == ContentAlignment.MiddleLeft || CA == ContentAlignment.MiddleCenter || CA == ContentAlignment.MiddleRight)
                        {
                            CaptionLocF.Y = CaptionBounds.Y + Math.Max(0, (CaptionBounds.Height - CaptionSizeF.Height) / 2);
                        }
                        else if (CA == ContentAlignment.BottomLeft || CA == ContentAlignment.BottomCenter || CA == ContentAlignment.BottomRight)
                        {
                            CaptionLocF.Y = CaptionBounds.Y + Math.Max(0, CaptionBounds.Height - CaptionSizeF.Height);
                        }

                        Color Cr_Caption_Fr = Me.RecommendColors.Caption.ToColor();
                        Color Cr_Caption_Bk_Outer, Cr_Caption_Bk_Inner;

                        if (!RecommendColors.BackColorFitLightText(Me.RecommendColors.CaptionBar))
                        {
                            Cr_Caption_Bk_Outer = Color.FromArgb(48, Color.White);
                            Cr_Caption_Bk_Inner = Color.FromArgb(96, Color.White);
                        }
                        else
                        {
                            Cr_Caption_Bk_Outer = Color.FromArgb(32, Color.Black);
                            Cr_Caption_Bk_Inner = Color.FromArgb(64, Color.Black);
                        }

                        CreateFormCaptionBmp.DrawString(Caption, CaptionFont, new SolidBrush(Cr_Caption_Bk_Outer), new PointF(CaptionLocF.X - 2, CaptionLocF.Y - 2));
                        CreateFormCaptionBmp.DrawString(Caption, CaptionFont, new SolidBrush(Cr_Caption_Bk_Outer), new PointF(CaptionLocF.X + 2, CaptionLocF.Y - 2));
                        CreateFormCaptionBmp.DrawString(Caption, CaptionFont, new SolidBrush(Cr_Caption_Bk_Outer), new PointF(CaptionLocF.X + 2, CaptionLocF.Y + 2));
                        CreateFormCaptionBmp.DrawString(Caption, CaptionFont, new SolidBrush(Cr_Caption_Bk_Outer), new PointF(CaptionLocF.X - 2, CaptionLocF.Y + 2));

                        CreateFormCaptionBmp.DrawString(Caption, CaptionFont, new SolidBrush(Cr_Caption_Bk_Inner), new PointF(CaptionLocF.X - 1, CaptionLocF.Y));
                        CreateFormCaptionBmp.DrawString(Caption, CaptionFont, new SolidBrush(Cr_Caption_Bk_Inner), new PointF(CaptionLocF.X, CaptionLocF.Y - 1));
                        CreateFormCaptionBmp.DrawString(Caption, CaptionFont, new SolidBrush(Cr_Caption_Bk_Inner), new PointF(CaptionLocF.X + 1, CaptionLocF.Y));
                        CreateFormCaptionBmp.DrawString(Caption, CaptionFont, new SolidBrush(Cr_Caption_Bk_Inner), new PointF(CaptionLocF.X, CaptionLocF.Y + 1));

                        CreateFormCaptionBmp.DrawString(Caption, CaptionFont, new SolidBrush(Cr_Caption_Fr), CaptionLocF);
                    }
                }
            }
        }

        private void _RepaintCaptionBarBitmap() // 更新并重绘标题栏绘图。
        {
            if (Me.FormState != FormState.FullScreen)
            {
                _UpdateCaptionBarBitmap();

                if (_CaptionBarBitmap != null)
                {
                    Panel_CaptionBar.CreateGraphics().DrawImage(_CaptionBarBitmap, new Point(0, 0));

                    Panel_FormIcon.Refresh();
                    Panel_ControlBox.Refresh();
                }
            }
        }

        //

        private void _UpdateForStyleOrStateChanged() // 在 FormStyleChanged 或 FormStateChanged 事件发生时更新控件的属性。
        {
            ToolStripMenuItem_Return.Enabled = (Me.FormState != FormState.Normal);
            ToolStripMenuItem_Maximize.Enabled = (Me.FormState != FormState.Maximized);

            //

            ToolTip_ControlBox.Hide(PictureBox_FullScreen);
            ToolTip_ControlBox.SetToolTip(PictureBox_FullScreen, (Me.FormState == FormState.FullScreen ? "退出全屏" : "全屏"));

            ToolTip_ControlBox.Hide(PictureBox_Maximize);
            ToolTip_ControlBox.SetToolTip(PictureBox_Maximize, (Me.FormState == FormState.Maximized ? "向下还原" : "最大化"));

            //

            bool EnableFullScreen = false, EnableMinimize = false, EnableMaximize = false, EnableReturn = false;

            if (Me.FormState == FormState.FullScreen)
            {
                Panel_FormIcon.Visible = false;

                EnableFullScreen = true;
                EnableMaximize = false;
                EnableReturn = false;

                switch (Me.FormStyle)
                {
                    case FormStyle.Sizable:
                    case FormStyle.Fixed:
                        EnableMinimize = true;
                        break;

                    case FormStyle.Dialog:
                        EnableMinimize = false;
                        break;
                }
            }
            else
            {
                Panel_FormIcon.Visible = Me.ShowIconOnCaptionBar;

                EnableFullScreen = Me.EnableFullScreen;

                switch (Me.FormStyle)
                {
                    case FormStyle.Sizable:
                        {
                            EnableMinimize = true;
                            EnableMaximize = true;
                            EnableReturn = true;
                        }
                        break;

                    case FormStyle.Fixed:
                        {
                            EnableMinimize = true;
                            EnableMaximize = false;
                            EnableReturn = false;
                        }
                        break;

                    case FormStyle.Dialog:
                        {
                            EnableMinimize = false;
                            EnableMaximize = false;
                            EnableReturn = false;
                        }
                        break;
                }
            }

            PictureBox_FullScreen.Visible = EnableFullScreen;
            ToolStripMenuItem_Minimize.Visible = PictureBox_Minimize.Visible = EnableMinimize;
            ToolStripMenuItem_Maximize.Visible = PictureBox_Maximize.Visible = EnableMaximize;
            ToolStripMenuItem_Return.Visible = EnableReturn;

            ToolStripSeparator_Main.Visible = (EnableReturn || EnableMinimize || EnableMaximize);

            PictureBox_Minimize.Left = (EnableFullScreen ? PictureBox_FullScreen.Right : PictureBox_FullScreen.Left);
            PictureBox_Maximize.Left = (EnableMinimize ? PictureBox_Minimize.Right : PictureBox_Minimize.Left);
            PictureBox_Exit.Left = (EnableMaximize ? PictureBox_Maximize.Right : PictureBox_Maximize.Left);

            Panel_ControlBox.Width = PictureBox_Exit.Right;
        }

        private void _UpdateControlBoxImage() // 更新控制按钮的图像。
        {
            PictureBox_FullScreen.Image = (!RecommendColors.BackColorFitLightText(_FullScreenButtonIsPointed || _FullScreenButtonIsPressed ? PictureBox_FullScreen.BackColor : this.BackColor) ? (Me.FormState == FormState.FullScreen ? (Me.IsActive ? Properties.Resources.ControlBox_Active_DarkImage_ExitFullScreen_16 : Properties.Resources.ControlBox_Inactive_DarkImage_ExitFullScreen_16) : (Me.IsActive ? Properties.Resources.ControlBox_Active_DarkImage_EnterFullScreen_16 : Properties.Resources.ControlBox_Inactive_DarkImage_EnterFullScreen_16)) : (Me.FormState == FormState.FullScreen ? (Me.IsActive ? Properties.Resources.ControlBox_Active_LightImage_ExitFullScreen_16 : Properties.Resources.ControlBox_Inactive_LightImage_ExitFullScreen_16) : (Me.IsActive ? Properties.Resources.ControlBox_Active_LightImage_EnterFullScreen_16 : Properties.Resources.ControlBox_Inactive_LightImage_EnterFullScreen_16)));
            PictureBox_Minimize.Image = (!RecommendColors.BackColorFitLightText(_MinimizeButtonIsPointed || _MinimizeButtonIsPressed ? PictureBox_Minimize.BackColor : this.BackColor) ? (Me.IsActive ? Properties.Resources.ControlBox_Active_DarkImage_Minimize_16 : Properties.Resources.ControlBox_Inactive_DarkImage_Minimize_16) : (Me.IsActive ? Properties.Resources.ControlBox_Active_LightImage_Minimize_16 : Properties.Resources.ControlBox_Inactive_LightImage_Minimize_16));
            PictureBox_Maximize.Image = (!RecommendColors.BackColorFitLightText(_MaximizeButtonIsPointed || _MaximizeButtonIsPressed ? PictureBox_Maximize.BackColor : this.BackColor) ? (Me.FormState == FormState.Maximized ? (Me.IsActive ? Properties.Resources.ControlBox_Active_DarkImage_Return_16 : Properties.Resources.ControlBox_Inactive_DarkImage_Return_16) : (Me.IsActive ? Properties.Resources.ControlBox_Active_DarkImage_Maximize_16 : Properties.Resources.ControlBox_Inactive_DarkImage_Maximize_16)) : (Me.FormState == FormState.Maximized ? (Me.IsActive ? Properties.Resources.ControlBox_Active_LightImage_Return_16 : Properties.Resources.ControlBox_Inactive_LightImage_Return_16) : (Me.IsActive ? Properties.Resources.ControlBox_Active_LightImage_Maximize_16 : Properties.Resources.ControlBox_Inactive_LightImage_Maximize_16)));
            PictureBox_Exit.Image = (!RecommendColors.BackColorFitLightText(_ExitButtonIsPointed || _ExitButtonIsPressed ? PictureBox_Exit.BackColor : this.BackColor) ? (Me.IsActive ? Properties.Resources.ControlBox_Active_DarkImage_Exit_16 : Properties.Resources.ControlBox_Inactive_DarkImage_Exit_16) : (Me.IsActive ? Properties.Resources.ControlBox_Active_LightImage_Exit_16 : Properties.Resources.ControlBox_Inactive_LightImage_Exit_16));
        }

        //

        private DateTime _LastUpdateLayout = new DateTime(); // 上次更新标题栏布局的日期时间。

        private void _TryToUpdateLayout() // 尝试更新标题栏布局。
        {
            if (!BackgroundWorker_UpdateLayoutDelay.IsBusy)
            {
                BackgroundWorker_UpdateLayoutDelay.RunWorkerAsync();
            }
        }

        #endregion

        #region 回调函数

        private void CaptionBar_Load(object sender, EventArgs e) // CaptionBar 的 Load 事件的回调函数。
        {
            Panel_CaptionBar.BackColor = Color.Transparent;

            //

            PictureBox_FormIcon.Image = Me.Client.Icon.ToBitmap();

            //

            PictureBox_FullScreen.Size = PictureBox_Minimize.Size = PictureBox_Maximize.Size = PictureBox_Exit.Size = Me.ControlBoxButtonSize;

            Panel_ControlBox.Height = Me.ControlBoxButtonHeight;

            //

            _UpdateForStyleOrStateChanged();

            //

            OnThemeChanged();

            //

            CaptionBar_SizeChanged(this, EventArgs.Empty);

            //

            Panel_FormIcon.Visible = Panel_ControlBox.Visible = false;
        }

        private void CaptionBar_LocationChanged(object sender, EventArgs e) // CaptionBar 的 LocationChanged 事件的回调函数。
        {
            _RepaintCaptionBarBitmap();
        }

        private void CaptionBar_SizeChanged(object sender, EventArgs e) // CaptionBar 的 SizeChanged 事件的回调函数。
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }

            //

            if (Me.FormState == FormState.FullScreen)
            {
                Panel_CaptionBar.Size = Panel_ControlBox.Size;
            }
            else
            {
                Panel_CaptionBar.Size = new Size(Me.Width, Me.CaptionBarHeight);
            }

            Panel_FormIcon.Size = new Size(Math.Min(Panel_CaptionBar.Height, Panel_ControlBox.Height), Math.Min(Panel_CaptionBar.Height, Panel_ControlBox.Height));
            PictureBox_FormIcon.Location = new Point((Panel_FormIcon.Width - PictureBox_FormIcon.Width) / 2, (Panel_FormIcon.Height - PictureBox_FormIcon.Height) / 2);

            Panel_ControlBox.Location = new Point(Panel_CaptionBar.Width - Panel_ControlBox.Width, Math.Min(0, (Panel_CaptionBar.Height - Panel_ControlBox.Height) / 2));

            //

            _RepaintCaptionBarBitmap();
        }

        //

        private void Panel_CaptionBar_Paint(object sender, PaintEventArgs e) // Panel_CaptionBar 的 Paint 事件的回调函数。
        {
            if (Me.FormState != FormState.FullScreen)
            {
                if (_CaptionBarBitmap == null)
                {
                    _UpdateCaptionBarBitmap();
                }

                if (_CaptionBarBitmap != null)
                {
                    e.Graphics.DrawImage(_CaptionBarBitmap, new Point(0, 0));
                }
            }
        }

        private void Panel_CaptionBar_MouseDoubleClick(object sender, MouseEventArgs e) // Panel_CaptionBar 的 MouseDoubleClick 事件的回调函数。
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Me.FormStyle == FormStyle.Sizable)
                {
                    if (Me.FormState == FormState.Maximized)
                    {
                        Me.Return();
                    }
                    else if (Me.FormState != FormState.FullScreen)
                    {
                        Me.Maximize();
                    }
                }
            }
        }

        private void Panel_CaptionBar_MouseDown(object sender, MouseEventArgs e) // Panel_CaptionBar 的 MouseDown 事件的回调函数。
        {
            Me.Client.Focus();

            if (e.Button == MouseButtons.Left)
            {
                if (Me.FormState != FormState.FullScreen)
                {
                    _CursorPositionOfMe = Geometry.GetCursorPositionOfControl(Panel_CaptionBar);

                    _MeWillMove = true;

                    Cursor.Clip = FormManager.PrimaryScreenClient;
                }
            }
        }

        private void Panel_CaptionBar_MouseUp(object sender, MouseEventArgs e) // Panel_CaptionBar 的 MouseUp 事件的回调函数。
        {
            Me.Client.Focus();

            if (_MeIsMoving == true && e.Button == MouseButtons.Left)
            {
                Action ReleaseAndCheckY = () =>
                {
                    Me.Bounds_Current_Y = Math.Max(Me.Bounds_Current_Location.Y, FormManager.PrimaryScreenClient.Y);

                    Me.UpdateLayout(UpdateLayoutEventType.LocationChanged);

                    if (Me.FormState != FormState.HighAsScreen)
                    {
                        Me.Bounds_Normal_Location = Me.Bounds_Current_Location;
                    }
                };

                if (Me.FormStyle == FormStyle.Sizable)
                {
                    if (FormManager.CursorPosition.X >= FormManager.PrimaryScreenClient.X && FormManager.CursorPosition.X <= FormManager.PrimaryScreenClient.X + _ExtendDist)
                    {
                        if (FormManager.CursorPosition.Y >= FormManager.PrimaryScreenClient.Y && FormManager.CursorPosition.Y <= FormManager.PrimaryScreenClient.Y + _ExtendDist)
                        {
                            if (!Me.TopLeftQuarterScreen())
                            {
                                ReleaseAndCheckY();
                            }
                        }
                        else if (FormManager.CursorPosition.Y >= FormManager.PrimaryScreenClient.Bottom - _ExtendDist && FormManager.CursorPosition.Y <= FormManager.PrimaryScreenBounds.Bottom)
                        {
                            if (!Me.BottomLeftQuarterScreen())
                            {
                                ReleaseAndCheckY();
                            }
                        }
                        else
                        {
                            if (!Me.LeftHalfScreen())
                            {
                                ReleaseAndCheckY();
                            }
                        }
                    }
                    else if (FormManager.CursorPosition.X >= FormManager.PrimaryScreenClient.Right - _ExtendDist && FormManager.CursorPosition.X <= FormManager.PrimaryScreenBounds.Right)
                    {
                        if (FormManager.CursorPosition.Y >= FormManager.PrimaryScreenClient.Y && FormManager.CursorPosition.Y <= FormManager.PrimaryScreenClient.Y + _ExtendDist)
                        {
                            if (!Me.TopRightQuarterScreen())
                            {
                                ReleaseAndCheckY();
                            }
                        }
                        else if (FormManager.CursorPosition.Y >= FormManager.PrimaryScreenClient.Bottom - _ExtendDist && FormManager.CursorPosition.Y <= FormManager.PrimaryScreenBounds.Bottom)
                        {
                            if (!Me.BottomRightQuarterScreen())
                            {
                                ReleaseAndCheckY();
                            }
                        }
                        else
                        {
                            if (!Me.RightHalfScreen())
                            {
                                ReleaseAndCheckY();
                            }
                        }
                    }
                    else if (FormManager.CursorPosition.Y >= FormManager.PrimaryScreenClient.Y && FormManager.CursorPosition.Y <= FormManager.PrimaryScreenClient.Y + _ExtendDist)
                    {
                        if (Me.FormState == FormState.Normal)
                        {
                            if (!Me.Maximize())
                            {
                                ReleaseAndCheckY();
                            }
                        }
                    }
                    else
                    {
                        ReleaseAndCheckY();
                    }
                }
                else
                {
                    ReleaseAndCheckY();
                }
            }

            _MeWillMove = _MeIsMoving = false;

            Cursor.Clip = FormManager.PrimaryScreenBounds;
        }

        private void Panel_CaptionBar_MouseMove(object sender, MouseEventArgs e) // Panel_CaptionBar 的 MouseMove 事件的回调函数。
        {
            if (_MeWillMove)
            {
                Point CurPt = Geometry.GetCursorPositionOfControl(Panel_CaptionBar);

                if (PointD.DistanceBetween(new PointD(CurPt), new PointD(_CursorPositionOfMe)) >= _ExtendDist)
                {
                    _MeWillMove = false;
                    _MeIsMoving = true;
                }
            }

            if (_MeIsMoving)
            {
                if (Me.FormState != FormState.FullScreen)
                {
                    if ((Me.FormState == FormState.Maximized && (FormManager.CursorPosition.Y > FormManager.PrimaryScreenClient.Y + _ExtendDist || (FormManager.CursorPosition.X >= FormManager.PrimaryScreenClient.X && FormManager.CursorPosition.X <= FormManager.PrimaryScreenClient.X + _ExtendDist) || (FormManager.CursorPosition.X >= FormManager.PrimaryScreenClient.Right - _ExtendDist && FormManager.CursorPosition.X <= FormManager.PrimaryScreenBounds.Right))) || (Me.FormState == FormState.HighAsScreen && FormManager.CursorPosition.Y > FormManager.PrimaryScreenClient.Y + Me.CaptionBarHeight + _ExtendDist) || Me.FormState == FormState.QuarterScreen)
                    {
                        if (_CursorPositionOfMe.X >= Me.Bounds_Current_Width - Me.Bounds_Normal_Width / 2)
                        {
                            _CursorPositionOfMe.X = Me.Bounds_Normal_Width - (Me.Bounds_Current_Width - _CursorPositionOfMe.X);
                        }
                        else if (_CursorPositionOfMe.X > Me.Bounds_Normal_Width / 2 && _CursorPositionOfMe.X < Me.Bounds_Current_Width - Me.Bounds_Normal_Width / 2)
                        {
                            _CursorPositionOfMe.X = Me.Bounds_Normal_Width / 2;
                        }

                        if (Me.CanReturn())
                        {
                            Me.ReturnByMoveForm();

                            Me.Bounds_Current_Location = new Point(FormManager.CursorPosition.X - _CursorPositionOfMe.X, FormManager.CursorPosition.Y - _CursorPositionOfMe.Y);

                            Me.UpdateLayout(UpdateLayoutEventType.Result);
                        }
                    }
                    else
                    {
                        if (Me.FormState == FormState.Normal)
                        {
                            Me.Bounds_Current_Location = new Point(FormManager.CursorPosition.X - _CursorPositionOfMe.X, FormManager.CursorPosition.Y - _CursorPositionOfMe.Y);

                            _TryToUpdateLayout();
                        }
                        else if (Me.FormState == FormState.HighAsScreen)
                        {
                            Me.Bounds_Current_X = FormManager.CursorPosition.X - _CursorPositionOfMe.X;

                            _TryToUpdateLayout();
                        }
                    }
                }
            }
        }

        //

        private void PictureBox_FormIcon_MouseDown(object sender, MouseEventArgs e) // PictureBox_FormIcon 的 MouseDown 事件的回调函数。
        {
            Me.Client.Focus();

            if (e.Button == MouseButtons.Left)
            {
                ContextMenuStrip_Main.Show(Panel_CaptionBar, new Point(0, Panel_FormIcon.Height));
            }
        }

        private void PictureBox_FormIcon_MouseDoubleClick(object sender, MouseEventArgs e) // PictureBox_FormIcon 的 MouseDoubleClick 事件的回调函数。
        {
            if (e.Button == MouseButtons.Left)
            {
                ContextMenuStrip_Main.Hide();

                Me.Close();
            }
        }

        private void Panel_FormIcon_MouseDown(object sender, MouseEventArgs e) // Panel_FormIcon 的 MouseDown 事件的回调函数。
        {
            PictureBox_FormIcon_MouseDown(sender, e);
        }

        private void Panel_FormIcon_MouseDoubleClick(object sender, MouseEventArgs e) // Panel_FormIcon 的 MouseDoubleClick 事件的回调函数。
        {
            PictureBox_FormIcon_MouseDoubleClick(sender, e);
        }

        //

        private void PictureBox_FullScreen_MouseClick(object sender, MouseEventArgs e) // PictureBox_FullScreen 的 MouseClick 事件的回调函数。
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Me.FormState == FormState.FullScreen)
                {
                    Me.ExitFullScreen();
                }
                else
                {
                    Me.EnterFullScreen();
                }
            }
        }

        private void PictureBox_FullScreen_MouseEnter(object sender, EventArgs e) // PictureBox_FullScreen 的 MouseEnter 事件的回调函数。
        {
            _FullScreenButtonIsPointed = true;

            PictureBox_FullScreen.BackColor = Me.RecommendColors.ControlButton_DEC.ToColor();

            _UpdateControlBoxImage();
        }

        private void PictureBox_FullScreen_MouseLeave(object sender, EventArgs e) // PictureBox_FullScreen 的 MouseLeave 事件的回调函数。
        {
            _FullScreenButtonIsPointed = false;

            PictureBox_FullScreen.BackColor = Me.RecommendColors.ControlButton.ToColor();

            _UpdateControlBoxImage();
        }

        private void PictureBox_FullScreen_MouseDown(object sender, MouseEventArgs e) // PictureBox_FullScreen 的 MouseDown 事件的回调函数。
        {
            Me.Client.Focus();

            if (e.Button == MouseButtons.Left)
            {
                _FullScreenButtonIsPressed = true;

                PictureBox_FullScreen.BackColor = Me.RecommendColors.ControlButton_INC.ToColor();

                _UpdateControlBoxImage();
            }
        }

        private void PictureBox_FullScreen_MouseUp(object sender, MouseEventArgs e) // PictureBox_FullScreen 的 MouseUp 事件的回调函数。
        {
            Me.Client.Focus();

            if (e.Button == MouseButtons.Left)
            {
                _FullScreenButtonIsPressed = false;

                if (Geometry.CursorIsInControl(PictureBox_FullScreen))
                {
                    _FullScreenButtonIsPointed = true;

                    PictureBox_FullScreen.BackColor = Me.RecommendColors.ControlButton_DEC.ToColor();

                    _UpdateControlBoxImage();
                }
                else
                {
                    PictureBox_FullScreen_MouseLeave(sender, e);
                }
            }
        }

        private void PictureBox_Minimize_MouseClick(object sender, MouseEventArgs e) // PictureBox_Minimize 的 MouseClick 事件的回调函数。
        {
            if (e.Button == MouseButtons.Left)
            {
                Me.Minimize();
            }
        }

        private void PictureBox_Minimize_MouseEnter(object sender, EventArgs e) // PictureBox_Minimize 的 MouseEnter 事件的回调函数。
        {
            _MinimizeButtonIsPointed = true;

            PictureBox_Minimize.BackColor = Me.RecommendColors.ControlButton_DEC.ToColor();

            _UpdateControlBoxImage();
        }

        private void PictureBox_Minimize_MouseLeave(object sender, EventArgs e) // PictureBox_Minimize 的 MouseLeave 事件的回调函数。
        {
            _MinimizeButtonIsPointed = false;

            PictureBox_Minimize.BackColor = Me.RecommendColors.ControlButton.ToColor();

            _UpdateControlBoxImage();
        }

        private void PictureBox_Minimize_MouseDown(object sender, MouseEventArgs e) // PictureBox_Minimize 的 MouseDown 事件的回调函数。
        {
            Me.Client.Focus();

            if (e.Button == MouseButtons.Left)
            {
                _MinimizeButtonIsPressed = true;

                PictureBox_Minimize.BackColor = Me.RecommendColors.ControlButton_INC.ToColor();

                _UpdateControlBoxImage();
            }
        }

        private void PictureBox_Minimize_MouseUp(object sender, MouseEventArgs e) // PictureBox_Minimize 的 MouseUp 事件的回调函数。
        {
            Me.Client.Focus();

            if (e.Button == MouseButtons.Left)
            {
                _MinimizeButtonIsPressed = false;

                if (Geometry.CursorIsInControl(PictureBox_Minimize))
                {
                    _MinimizeButtonIsPointed = true;

                    PictureBox_Minimize.BackColor = Me.RecommendColors.ControlButton_DEC.ToColor();

                    _UpdateControlBoxImage();
                }
                else
                {
                    PictureBox_Minimize_MouseLeave(sender, e);
                }
            }
        }

        private void PictureBox_Maximize_MouseClick(object sender, MouseEventArgs e) // PictureBox_Maximize 的 MouseClick 事件的回调函数。
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Me.FormState != FormState.Maximized)
                {
                    Me.Maximize();
                }
                else
                {
                    Me.Return();
                }
            }
        }

        private void PictureBox_Maximize_MouseEnter(object sender, EventArgs e) // PictureBox_Maximize 的 MouseEnter 事件的回调函数。
        {
            _MaximizeButtonIsPointed = true;

            PictureBox_Maximize.BackColor = Me.RecommendColors.ControlButton_DEC.ToColor();

            _UpdateControlBoxImage();
        }

        private void PictureBox_Maximize_MouseLeave(object sender, EventArgs e) // PictureBox_Maximize 的 MouseLeave 事件的回调函数。
        {
            _MaximizeButtonIsPointed = false;

            PictureBox_Maximize.BackColor = Me.RecommendColors.ControlButton.ToColor();

            _UpdateControlBoxImage();
        }

        private void PictureBox_Maximize_MouseDown(object sender, MouseEventArgs e) // PictureBox_Maximize 的 MouseDown 事件的回调函数。
        {
            Me.Client.Focus();

            if (e.Button == MouseButtons.Left)
            {
                _MaximizeButtonIsPressed = true;

                PictureBox_Maximize.BackColor = Me.RecommendColors.ControlButton_INC.ToColor();

                _UpdateControlBoxImage();
            }
        }

        private void PictureBox_Maximize_MouseUp(object sender, MouseEventArgs e) // PictureBox_Maximize 的 MouseUp 事件的回调函数。
        {
            Me.Client.Focus();

            if (e.Button == MouseButtons.Left)
            {
                _MaximizeButtonIsPressed = false;

                if (Geometry.CursorIsInControl(PictureBox_Maximize))
                {
                    _MaximizeButtonIsPointed = true;

                    PictureBox_Maximize.BackColor = Me.RecommendColors.ControlButton_DEC.ToColor();

                    _UpdateControlBoxImage();
                }
                else
                {
                    PictureBox_Maximize_MouseLeave(sender, e);
                }
            }
        }

        private void PictureBox_Exit_MouseClick(object sender, MouseEventArgs e) // PictureBox_Exit 的 MouseClick 事件的回调函数。
        {
            if (e.Button == MouseButtons.Left)
            {
                Me.Close();
            }
        }

        private void PictureBox_Exit_MouseEnter(object sender, EventArgs e) // PictureBox_Exit 的 MouseEnter 事件的回调函数。
        {
            _ExitButtonIsPointed = true;

            PictureBox_Exit.BackColor = Me.RecommendColors.ExitButton_DEC.ToColor();

            _UpdateControlBoxImage();
        }

        private void PictureBox_Exit_MouseLeave(object sender, EventArgs e) // PictureBox_Exit 的 MouseLeave 事件的回调函数。
        {
            _ExitButtonIsPointed = false;

            PictureBox_Exit.BackColor = Me.RecommendColors.ExitButton.ToColor();

            _UpdateControlBoxImage();
        }

        private void PictureBox_Exit_MouseDown(object sender, MouseEventArgs e) // PictureBox_Exit 的 MouseDown 事件的回调函数。
        {
            Me.Client.Focus();

            if (e.Button == MouseButtons.Left)
            {
                _ExitButtonIsPressed = true;

                PictureBox_Exit.BackColor = Me.RecommendColors.ExitButton_INC.ToColor();

                _UpdateControlBoxImage();
            }
        }

        private void PictureBox_Exit_MouseUp(object sender, MouseEventArgs e) // PictureBox_Exit 的 MouseUp 事件的回调函数。
        {
            Me.Client.Focus();

            if (e.Button == MouseButtons.Left)
            {
                _ExitButtonIsPressed = false;

                if (Geometry.CursorIsInControl(PictureBox_Exit))
                {
                    _ExitButtonIsPointed = true;

                    PictureBox_Exit.BackColor = Me.RecommendColors.ExitButton_DEC.ToColor();

                    _UpdateControlBoxImage();
                }
                else
                {
                    PictureBox_Exit_MouseLeave(sender, e);
                }
            }
        }

        //

        private void ContextMenuStrip_Main_Enter(object sender, EventArgs e) // ContextMenuStrip_Main 的 Enter 事件的回调函数。
        {
            Me.MainMenuIsActive = true;
        }

        private void ContextMenuStrip_Main_Leave(object sender, EventArgs e) // ContextMenuStrip_Main 的 Leave 事件的回调函数。
        {
            Me.MainMenuIsActive = false;
        }

        private void ToolStripMenuItem_Return_Click(object sender, EventArgs e) // ToolStripMenuItem_Return 的 Click 事件的回调函数。
        {
            Me.Return();
        }

        private void ToolStripMenuItem_Minimize_Click(object sender, EventArgs e) // ToolStripMenuItem_Minimize 的 Click 事件的回调函数。
        {
            Me.Minimize();
        }

        private void ToolStripMenuItem_Maximize_Click(object sender, EventArgs e) // ToolStripMenuItem_Maximize 的 Click 事件的回调函数。
        {
            Me.Maximize();
        }

        private void ToolStripMenuItem_Exit_Click(object sender, EventArgs e) // ToolStripMenuItem_Exit 的 Click 事件的回调函数。
        {
            Me.Close();
        }

        //

        private void BackgroundWorker_UpdateLayoutDelay_DoWork(object sender, DoWorkEventArgs e) // BackgroundWorker_UpdateLayoutDelay 的 DoWork 事件的回调函数。
        {
            while ((DateTime.Now - _LastUpdateLayout).TotalMilliseconds < 16)
            {
                Thread.Sleep(2);
            }
        }

        private void BackgroundWorker_UpdateLayoutDelay_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) // BackgroundWorker_UpdateLayoutDelay 的 RunWorkerCompleted 事件的回调函数。
        {
            Me.UpdateLayout(UpdateLayoutEventType.Process);

            _LastUpdateLayout = DateTime.Now;
        }

        //

        private void Timer_FullScreenMonitor_Tick(object sender, EventArgs e) // Timer_FullScreenMonitor 的 Tick 事件的回调函数。
        {
            if (Me.FormState == FormState.FullScreen)
            {
                double _Opacity = Me.Opacity * Me.CaptionBarOpacityRatio;

                Animation.Frame FrameShow = (frameId, frameCount, msPerFrame) =>
                {
                    double Pct_F = (frameId == frameCount ? 1 : 1 - Math.Pow(1 - (double)frameId / frameCount, 2));

                    this.Opacity = _Opacity * Pct_F;
                    this.Height = (int)(Panel_ControlBox.Height * Pct_F);

                    Panel_CaptionBar.Top = this.Height - Panel_CaptionBar.Height;
                };

                Animation.Frame FrameHide = (frameId, frameCount, msPerFrame) =>
                {
                    double Pct_F = (frameId == frameCount ? 1 : 1 - Math.Pow(1 - (double)frameId / frameCount, 2));

                    this.Opacity = _Opacity * (1 - Pct_F);
                    this.Height = (int)(Panel_ControlBox.Height * (1 - Pct_F));

                    Panel_CaptionBar.Top = this.Height - Panel_CaptionBar.Height;
                };

                if (Me.Enabled)
                {
                    if (this.Height == Panel_ControlBox.Height)
                    {
                        if (FormManager.CursorPosition.Y > Me.Y + Panel_CaptionBar.Height * 2)
                        {
                            Animation.Show(FrameHide, 9, 15);
                        }
                    }
                    else
                    {
                        if (FormManager.CursorPosition.Y <= Me.Y + _ExtendDist && FormManager.CursorPosition.X >= Me.Right - Panel_CaptionBar.Width && FormManager.CursorPosition.X <= Me.Right)
                        {
                            this.BringToFront();

                            Animation.Show(FrameShow, 9, 15);
                        }
                    }
                }
                else
                {
                    if (this.Height == Panel_ControlBox.Height)
                    {
                        Animation.Show(FrameHide, 9, 15);
                    }
                }
            }
        }

        #endregion

        #region 构造函数

        public CaptionBar(FormManager formManager) // 使用 FormManager 对象初始化 CaptionBar 的新实例。
        {
            InitializeComponent();

            //

            Me = formManager;
        }

        #endregion

        #region 方法

        public void OnLoading() // 在 Loading 事件发生时发生。
        {
            Panel_FormIcon.Visible = Me.ShowIconOnCaptionBar;
            Panel_ControlBox.Visible = true;
        }

        public void OnClosing() // 在 Closing 事件发生时发生。
        {
            PictureBox_Exit.Enabled = ToolStripMenuItem_Exit.Enabled = false;

            PictureBox_FormIcon.MouseDoubleClick -= PictureBox_FormIcon_MouseDoubleClick;
            Panel_FormIcon.MouseDoubleClick -= Panel_FormIcon_MouseDoubleClick;
        }

        public void OnFormStyleChanged() // 在 FormStyleChanged 事件发生时发生。
        {
            _UpdateForStyleOrStateChanged();

            //

            if (Me.FormState == FormState.FullScreen)
            {
                this.Width = Panel_ControlBox.Width;
                this.Left = Me.Right - Panel_ControlBox.Width;
            }

            //

            CaptionBar_SizeChanged(this, EventArgs.Empty);
        }

        public void OnFormStateChanged() // 在 FormStateChanged 事件发生时发生。
        {
            _UpdateForStyleOrStateChanged();

            _UpdateControlBoxImage();

            //

            if (Me.FormState == FormState.FullScreen)
            {
                this.Size = Panel_ControlBox.Size;
                this.Location = new Point(Me.Right - Panel_ControlBox.Width, Me.Y);
            }
            else
            {
                if (Me.PreviousFormState == FormState.FullScreen)
                {
                    this.Opacity = Me.Opacity * Me.CaptionBarOpacityRatio;

                    Panel_CaptionBar.Top = 0;
                }

                CaptionBar_SizeChanged(this, EventArgs.Empty);
            }

            //

            if (Me.FormState == FormState.FullScreen)
            {
                Timer_FullScreenMonitor.Enabled = true;
            }
            else
            {
                Timer_FullScreenMonitor.Enabled = false;
            }
        }

        public void OnCaptionChanged() // 在 CaptionChanged 事件发生时发生。
        {
            _RepaintCaptionBarBitmap();
        }

        public void OnThemeChanged() // 在 ThemeChanged 事件发生时发生。
        {
            this.BackColor = Me.RecommendColors.CaptionBar.ToColor();

            //

            PictureBox_FullScreen.BackColor = (_FullScreenButtonIsPressed ? Me.RecommendColors.ControlButton_INC : (_FullScreenButtonIsPointed ? Me.RecommendColors.ControlButton_DEC : Me.RecommendColors.ControlButton)).ToColor();
            PictureBox_Minimize.BackColor = (_MinimizeButtonIsPressed ? Me.RecommendColors.ControlButton_INC : (_MinimizeButtonIsPointed ? Me.RecommendColors.ControlButton_DEC : Me.RecommendColors.ControlButton)).ToColor();
            PictureBox_Maximize.BackColor = (_MaximizeButtonIsPressed ? Me.RecommendColors.ControlButton_INC : (_MaximizeButtonIsPointed ? Me.RecommendColors.ControlButton_DEC : Me.RecommendColors.ControlButton)).ToColor();
            PictureBox_Exit.BackColor = (_ExitButtonIsPressed ? Me.RecommendColors.ExitButton_INC : (_ExitButtonIsPointed ? Me.RecommendColors.ExitButton_DEC : Me.RecommendColors.ExitButton)).ToColor();

            _UpdateControlBoxImage();

            //

            ContextMenuStrip_Main.BackColor = Me.RecommendColors.MenuItemBackground.ToColor();
            ToolStripMenuItem_Return.ForeColor = ToolStripMenuItem_Minimize.ForeColor = ToolStripMenuItem_Maximize.ForeColor = ToolStripMenuItem_Exit.ForeColor = Me.RecommendColors.MenuItemText.ToColor();

            //

            _UpdateControlBoxImage();

            _RepaintCaptionBarBitmap();
        }

        #endregion
    }
}