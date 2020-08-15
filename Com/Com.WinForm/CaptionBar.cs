/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2020 chibayuki@foxmail.com

Com.WinForm.CaptionBar
Version 20.8.15.1420

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
using System.Drawing.Imaging;
using System.Threading;

namespace Com.WinForm
{
    internal partial class CaptionBar : Form // 标题栏。
    {
        #region 非公开成员

        private FormManager Me; // 窗口管理器。

        //

        private Point _CursorPositionOfMe = new Point(); // 鼠标指针在窗口的坐标。

        private bool _MeWillMove = false; // 是否即将移动窗口。
        private bool _MeIsMoving = false; // 是否正在移动窗口。

        private const int _ExtendDist = 4; // 扩展距离，用于某些鼠标动作的判定。

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

        // 更新标题栏绘图。
        private void _UpdateCaptionBarBitmap()
        {
            if (!(_CaptionBarBitmap is null))
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

                if (!(Me.CaptionBarBackgroundImage is null))
                {
                    Bitmap BkgImg = Me.CaptionBarBackgroundImage;

                    if (BkgImg.Width > _CaptionBarBitmap.Width || BkgImg.Height > _CaptionBarBitmap.Height)
                    {
                        BkgImg = BkgImg.Clone(new Rectangle(new Point(0, 0), new Size(Math.Min(BkgImg.Width, _CaptionBarBitmap.Width), Math.Min(BkgImg.Height, _CaptionBarBitmap.Height))), BkgImg.PixelFormat);
                    }

                    CreateFormCaptionBmp.DrawImage(BkgImg, new Point(0, 0));
                }

                //

                if (Me.ShowCaption && Me.Caption.Length > 0)
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

                        if (Me.ShowCaptionBarColor && Me.IsActive)
                        {
                            Color Cr_Caption_Bk = (!RecommendColors.BackColorFitLightText(Me.RecommendColors.CaptionBar) ? Color.FromArgb(128, Color.White) : Color.FromArgb(96, Color.Black));

                            CreateFormCaptionBmp.DrawString(Caption, CaptionFont, new SolidBrush(Cr_Caption_Bk), new PointF(CaptionLocF.X - 0.5F, CaptionLocF.Y));
                            CreateFormCaptionBmp.DrawString(Caption, CaptionFont, new SolidBrush(Cr_Caption_Bk), new PointF(CaptionLocF.X + 0.5F, CaptionLocF.Y));
                            CreateFormCaptionBmp.DrawString(Caption, CaptionFont, new SolidBrush(Cr_Caption_Bk), new PointF(CaptionLocF.X, CaptionLocF.Y - 0.5F));
                            CreateFormCaptionBmp.DrawString(Caption, CaptionFont, new SolidBrush(Cr_Caption_Bk), new PointF(CaptionLocF.X, CaptionLocF.Y + 0.5F));
                        }

                        Color Cr_Caption_Fr = Me.RecommendColors.Caption.ToColor();

                        CreateFormCaptionBmp.DrawString(Caption, CaptionFont, new SolidBrush(Cr_Caption_Fr), CaptionLocF);
                    }
                }
            }
        }

        // 更新并重绘标题栏绘图。
        private void _RepaintCaptionBarBitmap()
        {
            if (Me.FormState != FormState.FullScreen)
            {
                _UpdateCaptionBarBitmap();

                if (!(_CaptionBarBitmap is null))
                {
                    Panel_CaptionBar.CreateGraphics().DrawImage(_CaptionBarBitmap, new Point(0, 0));

                    Panel_FormIcon.Refresh();
                    Panel_ControlBox.Refresh();
                }
            }
        }

        //

        // 在 FormStyleChanged 或 FormStateChanged 事件发生时更新控件的属性。
        private void _UpdateForStyleOrStateChanged()
        {
            ToolStripMenuItem_Maximize.Enabled = (Me.FormState != FormState.Maximized);
            ToolStripMenuItem_Return.Enabled = (Me.FormState != FormState.Normal);

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
                EnableMinimize = Me.EnableMinimize;
                EnableMaximize = false;
                EnableReturn = false;
            }
            else
            {
                Panel_FormIcon.Visible = Me.ShowIconOnCaptionBar;

                EnableFullScreen = Me.EnableFullScreen;
                EnableMinimize = Me.EnableMinimize;
                EnableMaximize = Me.EnableMaximize;

                switch (Me.FormStyle)
                {
                    case FormStyle.Sizable:
                        EnableReturn = true;
                        break;

                    case FormStyle.Fixed:
                        EnableReturn = false;
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

        // 更新控制按钮的图像。
        private void _UpdateControlBoxButtonImage()
        {
            Bitmap[,,] FullScreenImage = new Bitmap[2, 2, 2]
            {
                {
                    { Properties.Resources.ControlBox_Active_DarkImage_ExitFullScreen_16, Properties.Resources.ControlBox_Inactive_DarkImage_ExitFullScreen_16 },
                    { Properties.Resources.ControlBox_Active_DarkImage_EnterFullScreen_16, Properties.Resources.ControlBox_Inactive_DarkImage_EnterFullScreen_16 }
                },
                {
                    { Properties.Resources.ControlBox_Active_LightImage_ExitFullScreen_16, Properties.Resources.ControlBox_Inactive_LightImage_ExitFullScreen_16 },
                    { Properties.Resources.ControlBox_Active_LightImage_EnterFullScreen_16, Properties.Resources.ControlBox_Inactive_LightImage_EnterFullScreen_16 }
                }
            };

            PictureBox_FullScreen.Image = FullScreenImage[(!RecommendColors.BackColorFitLightText(_FullScreenButtonIsPointed || _FullScreenButtonIsPressed ? (ColorX)PictureBox_FullScreen.BackColor : Me.RecommendColors.CaptionBar) ? 0 : 1), (Me.ActualFormState == FormState.FullScreen ? 0 : 1), (_FullScreenButtonIsPointed || _FullScreenButtonIsPressed || Me.IsActive ? 0 : 1)];

            Bitmap[,] MinimizeImage = new Bitmap[2, 2]
            {
                { Properties.Resources.ControlBox_Active_DarkImage_Minimize_16, Properties.Resources.ControlBox_Inactive_DarkImage_Minimize_16 },
                { Properties.Resources.ControlBox_Active_LightImage_Minimize_16, Properties.Resources.ControlBox_Inactive_LightImage_Minimize_16 }
            };

            PictureBox_Minimize.Image = MinimizeImage[(!RecommendColors.BackColorFitLightText(_MinimizeButtonIsPointed || _MinimizeButtonIsPressed ? (ColorX)PictureBox_Minimize.BackColor : Me.RecommendColors.CaptionBar) ? 0 : 1), (_MinimizeButtonIsPointed || _MinimizeButtonIsPressed || Me.IsActive ? 0 : 1)];

            Bitmap[,,] MaximizeImage = new Bitmap[2, 2, 2]
            {
                {
                    { Properties.Resources.ControlBox_Active_DarkImage_Return_16, Properties.Resources.ControlBox_Inactive_DarkImage_Return_16 },
                    { Properties.Resources.ControlBox_Active_DarkImage_Maximize_16, Properties.Resources.ControlBox_Inactive_DarkImage_Maximize_16 }
                },
                {
                    { Properties.Resources.ControlBox_Active_LightImage_Return_16, Properties.Resources.ControlBox_Inactive_LightImage_Return_16 },
                    { Properties.Resources.ControlBox_Active_LightImage_Maximize_16, Properties.Resources.ControlBox_Inactive_LightImage_Maximize_16 }
                }
            };

            PictureBox_Maximize.Image = MaximizeImage[(!RecommendColors.BackColorFitLightText(_MaximizeButtonIsPointed || _MaximizeButtonIsPressed ? (ColorX)PictureBox_Maximize.BackColor : Me.RecommendColors.CaptionBar) ? 0 : 1), (Me.ActualFormState == FormState.Maximized ? 0 : 1), (_MaximizeButtonIsPointed || _MaximizeButtonIsPressed || Me.IsActive ? 0 : 1)];

            Bitmap[,] ExitImage = new Bitmap[2, 2]
            {
                { Properties.Resources.ControlBox_Active_DarkImage_Exit_16, Properties.Resources.ControlBox_Inactive_DarkImage_Exit_16 },
                { Properties.Resources.ControlBox_Active_LightImage_Exit_16, Properties.Resources.ControlBox_Inactive_LightImage_Exit_16 }
            };

            PictureBox_Exit.Image = ExitImage[(!RecommendColors.BackColorFitLightText(_ExitButtonIsPointed || _ExitButtonIsPressed ? (ColorX)PictureBox_Exit.BackColor : Me.RecommendColors.CaptionBar) ? 0 : 1), (_ExitButtonIsPointed || _ExitButtonIsPressed || Me.IsActive ? 0 : 1)];

            PictureBox_FullScreen.Refresh();
            PictureBox_Minimize.Refresh();
            PictureBox_Maximize.Refresh();
            PictureBox_Exit.Refresh();
        }

        // 更新控制按钮的背景颜色。
        private void _UpdateControlBoxButtonBackColor(Control control, ColorX backColor)
        {
            if (Me.Effect.HasFlag(Effect.Fade))
            {
                ColorX OldBackColor = control.BackColor;

                Animation.Frame Frame = (frameId, frameCount, msPerFrame) =>
                {
                    double Progress = (frameId == frameCount ? 1 : (double)frameId / frameCount);

                    if (frameId == frameCount)
                    {
                        control.BackColor = backColor.ToColor();
                    }
                    else
                    {
                        control.BackColor = ColorManipulation.BlendByRGB(backColor, OldBackColor, Progress).ToColor();
                    }

                    control.Refresh();
                };

                Animation.Show(Frame, 6, 15);
            }
            else
            {
                control.BackColor = backColor.ToColor();
                control.Refresh();
            }
        }

        //

        private DateTime _LastUpdateLayout = new DateTime(); // 上次更新窗口布局的日期时间。

        private UpdateLayoutEventType _UpdateLayoutEventType = UpdateLayoutEventType.None; // 尝试更新窗口布局时希望触发的事件类型。

        private bool _UpdateLayoutCanceled = false; // 是否已取消更新窗口布局。

        // 尝试更新窗口布局。
        private void _TryToUpdateLayout(UpdateLayoutEventType updateLayoutEventType)
        {
            if (!BackgroundWorker_UpdateLayoutDelay.IsBusy)
            {
                _UpdateLayoutEventType = updateLayoutEventType;

                _UpdateLayoutCanceled = false;

                BackgroundWorker_UpdateLayoutDelay.RunWorkerAsync();
            }
        }

        // 取消更新窗口布局。
        private void _CancelUpdateLayout()
        {
            _UpdateLayoutCanceled = true;
        }

        #endregion

        #region 回调函数

        // CaptionBar 的 Load 事件的回调函数。
        private void CaptionBar_Load(object sender, EventArgs e)
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

        // CaptionBar 的 LocationChanged 事件的回调函数。
        private void CaptionBar_LocationChanged(object sender, EventArgs e)
        {
            _RepaintCaptionBarBitmap();
        }

        // CaptionBar 的 SizeChanged 事件的回调函数。
        private void CaptionBar_SizeChanged(object sender, EventArgs e)
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

        // Panel_CaptionBar 的 Paint 事件的回调函数。
        private void Panel_CaptionBar_Paint(object sender, PaintEventArgs e)
        {
            if (Me.FormState != FormState.FullScreen)
            {
                if (_CaptionBarBitmap is null)
                {
                    _UpdateCaptionBarBitmap();
                }

                if (!(_CaptionBarBitmap is null))
                {
                    e.Graphics.DrawImage(_CaptionBarBitmap, new Point(0, 0));
                }
            }
        }

        // Panel_CaptionBar 的 MouseDoubleClick 事件的回调函数。
        private void Panel_CaptionBar_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (_MeIsMoving)
                {
                    Panel_CaptionBar_MouseUp(sender, e);
                }
                else
                {
                    if (Me.EnableMaximize)
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
                    else
                    {
                        if (Me.FormState != FormState.Normal && Me.FormState != FormState.FullScreen)
                        {
                            Me.Return();
                        }
                    }
                }
            }
        }

        // Panel_CaptionBar 的 MouseDown 事件的回调函数。
        private void Panel_CaptionBar_MouseDown(object sender, MouseEventArgs e)
        {
            Me.Client.BringToFront();
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

        // Panel_CaptionBar 的 MouseUp 事件的回调函数。
        private void Panel_CaptionBar_MouseUp(object sender, MouseEventArgs e)
        {
            Me.Client.BringToFront();
            Me.Client.Focus();

            if (_MeIsMoving)
            {
                Point CursorPosition = Cursor.Position;
                Rectangle CurScrClient = FormManager.PrimaryScreenClient;
                Rectangle CurScrBounds = FormManager.PrimaryScreenBounds;

                Action ReleaseAndCheckY = () =>
                {
                    Me.Bounds_Current_Y = Math.Max(Me.Bounds_Current_Location.Y, CurScrClient.Y);

                    Me.UpdateLayout(UpdateLayoutEventType.LocationChanged);

                    if (Me.FormState != FormState.HighAsScreen)
                    {
                        Me.Bounds_Normal_Location = Me.Bounds_Current_Location;
                    }
                };

                if (Me.FormStyle == FormStyle.Sizable || Me.EnableMaximize)
                {
                    if (CursorPosition.X >= CurScrClient.X && CursorPosition.X <= CurScrClient.X + _ExtendDist)
                    {
                        if (Me.FormStyle == FormStyle.Sizable)
                        {
                            if (CursorPosition.Y >= CurScrClient.Y && CursorPosition.Y <= CurScrClient.Y + _ExtendDist)
                            {
                                _CancelUpdateLayout();

                                if (!Me.TopLeftQuarterScreen())
                                {
                                    ReleaseAndCheckY();
                                }
                            }
                            else if (CursorPosition.Y >= CurScrClient.Bottom - _ExtendDist && CursorPosition.Y <= CurScrBounds.Bottom)
                            {
                                _CancelUpdateLayout();

                                if (!Me.BottomLeftQuarterScreen())
                                {
                                    ReleaseAndCheckY();
                                }
                            }
                            else
                            {
                                _CancelUpdateLayout();

                                if (!Me.LeftHalfScreen())
                                {
                                    ReleaseAndCheckY();
                                }
                            }
                        }
                    }
                    else if (CursorPosition.X >= CurScrClient.Right - _ExtendDist && CursorPosition.X <= CurScrBounds.Right)
                    {
                        if (Me.FormStyle == FormStyle.Sizable)
                        {
                            if (CursorPosition.Y >= CurScrClient.Y && CursorPosition.Y <= CurScrClient.Y + _ExtendDist)
                            {
                                _CancelUpdateLayout();

                                if (!Me.TopRightQuarterScreen())
                                {
                                    ReleaseAndCheckY();
                                }
                            }
                            else if (CursorPosition.Y >= CurScrClient.Bottom - _ExtendDist && CursorPosition.Y <= CurScrBounds.Bottom)
                            {
                                _CancelUpdateLayout();

                                if (!Me.BottomRightQuarterScreen())
                                {
                                    ReleaseAndCheckY();
                                }
                            }
                            else
                            {
                                _CancelUpdateLayout();

                                if (!Me.RightHalfScreen())
                                {
                                    ReleaseAndCheckY();
                                }
                            }
                        }
                    }
                    else if (CursorPosition.Y >= CurScrClient.Y && CursorPosition.Y <= CurScrClient.Y + _ExtendDist)
                    {
                        if (Me.FormState == FormState.Normal)
                        {
                            if (Me.EnableMaximize)
                            {
                                _CancelUpdateLayout();

                                if (!Me.Maximize())
                                {
                                    ReleaseAndCheckY();
                                }
                            }
                            else
                            {
                                _CancelUpdateLayout();

                                ReleaseAndCheckY();
                            }
                        }
                    }
                    else
                    {
                        _CancelUpdateLayout();

                        ReleaseAndCheckY();
                    }
                }
                else
                {
                    _CancelUpdateLayout();

                    ReleaseAndCheckY();
                }
            }

            _MeWillMove = _MeIsMoving = false;

            Cursor.Clip = FormManager.PrimaryScreenBounds;
        }

        // Panel_CaptionBar 的 MouseMove 事件的回调函数。
        private void Panel_CaptionBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (_MeWillMove)
            {
                Point CurPt = Geometry.GetCursorPositionOfControl(Panel_CaptionBar);

                if (((PointD)CurPt).DistanceFrom(_CursorPositionOfMe) >= _ExtendDist)
                {
                    _MeWillMove = false;
                    _MeIsMoving = true;
                }
            }

            if (_MeIsMoving)
            {
                if (Me.FormState != FormState.FullScreen)
                {
                    Point CursorPosition = Cursor.Position;
                    Rectangle CurScrClient = FormManager.PrimaryScreenClient;
                    Rectangle CurScrBounds = FormManager.PrimaryScreenBounds;

                    if ((Me.FormState == FormState.Maximized && (CursorPosition.Y > CurScrClient.Y + _ExtendDist || (CursorPosition.X >= CurScrClient.X && CursorPosition.X <= CurScrClient.X + _ExtendDist) || (CursorPosition.X >= CurScrClient.Right - _ExtendDist && CursorPosition.X <= CurScrBounds.Right))) || (Me.FormState == FormState.HighAsScreen && CursorPosition.Y > CurScrClient.Y + Me.CaptionBarHeight + _ExtendDist) || Me.FormState == FormState.QuarterScreen)
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

                            Me.Bounds_Current_Location = new Point(CursorPosition.X - _CursorPositionOfMe.X, CursorPosition.Y - _CursorPositionOfMe.Y);

                            Me.UpdateLayout(UpdateLayoutEventType.Result);

                            Me.OnFormStateChanged();
                        }
                    }
                    else
                    {
                        if (Me.FormState == FormState.Normal)
                        {
                            Me.Bounds_Current_Location = new Point(CursorPosition.X - _CursorPositionOfMe.X, CursorPosition.Y - _CursorPositionOfMe.Y);

                            _TryToUpdateLayout(UpdateLayoutEventType.Move);
                        }
                        else if (Me.FormState == FormState.HighAsScreen)
                        {
                            Me.Bounds_Current_X = CursorPosition.X - _CursorPositionOfMe.X;

                            _TryToUpdateLayout(UpdateLayoutEventType.Move);
                        }
                    }
                }
            }
        }

        //

        // PictureBox_FormIcon 的 MouseDown 事件的回调函数。
        private void PictureBox_FormIcon_MouseDown(object sender, MouseEventArgs e)
        {
            Me.Client.BringToFront();
            Me.Client.Focus();

            if (e.Button == MouseButtons.Left)
            {
                ContextMenuStrip_Main.Show(Panel_CaptionBar, new Point(0, Panel_FormIcon.Height));
            }
        }

        // PictureBox_FormIcon 的 MouseDoubleClick 事件的回调函数。
        private void PictureBox_FormIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ContextMenuStrip_Main.Hide();

                Me.Close();
            }
        }

        // Panel_FormIcon 的 MouseDown 事件的回调函数。
        private void Panel_FormIcon_MouseDown(object sender, MouseEventArgs e)
        {
            PictureBox_FormIcon_MouseDown(sender, e);
        }

        // Panel_FormIcon 的 MouseDoubleClick 事件的回调函数。
        private void Panel_FormIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            PictureBox_FormIcon_MouseDoubleClick(sender, e);
        }

        //

        // PictureBox_FullScreen 的 MouseClick 事件的回调函数。
        private void PictureBox_FullScreen_MouseClick(object sender, MouseEventArgs e)
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

        // PictureBox_FullScreen 的 MouseEnter 事件的回调函数。
        private void PictureBox_FullScreen_MouseEnter(object sender, EventArgs e)
        {
            _FullScreenButtonIsPointed = true;

            _UpdateControlBoxButtonBackColor(PictureBox_FullScreen, Me.RecommendColors.ControlButton_DEC);

            _UpdateControlBoxButtonImage();
        }

        // PictureBox_FullScreen 的 MouseLeave 事件的回调函数。
        private void PictureBox_FullScreen_MouseLeave(object sender, EventArgs e)
        {
            _FullScreenButtonIsPointed = false;
            _FullScreenButtonIsPressed = false;

            _UpdateControlBoxButtonBackColor(PictureBox_FullScreen, Me.RecommendColors.ControlButton);

            _UpdateControlBoxButtonImage();
        }

        // PictureBox_FullScreen 的 MouseDown 事件的回调函数。
        private void PictureBox_FullScreen_MouseDown(object sender, MouseEventArgs e)
        {
            Me.Client.BringToFront();
            Me.Client.Focus();

            if (e.Button == MouseButtons.Left)
            {
                _FullScreenButtonIsPressed = true;

                _UpdateControlBoxButtonBackColor(PictureBox_FullScreen, Me.RecommendColors.ControlButton_INC);

                _UpdateControlBoxButtonImage();
            }
        }

        // PictureBox_FullScreen 的 MouseUp 事件的回调函数。
        private void PictureBox_FullScreen_MouseUp(object sender, MouseEventArgs e)
        {
            Me.Client.BringToFront();
            Me.Client.Focus();

            if (e.Button == MouseButtons.Left)
            {
                _FullScreenButtonIsPressed = false;

                if (Geometry.CursorIsInControl(PictureBox_FullScreen))
                {
                    _FullScreenButtonIsPointed = true;

                    _UpdateControlBoxButtonBackColor(PictureBox_FullScreen, Me.RecommendColors.ControlButton_DEC);

                    _UpdateControlBoxButtonImage();
                }
                else
                {
                    PictureBox_FullScreen_MouseLeave(sender, e);
                }
            }
        }

        // PictureBox_Minimize 的 MouseClick 事件的回调函数。
        private void PictureBox_Minimize_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Me.Minimize();
            }
        }

        // PictureBox_Minimize 的 MouseEnter 事件的回调函数。
        private void PictureBox_Minimize_MouseEnter(object sender, EventArgs e)
        {
            _MinimizeButtonIsPointed = true;

            _UpdateControlBoxButtonBackColor(PictureBox_Minimize, Me.RecommendColors.ControlButton_DEC);

            _UpdateControlBoxButtonImage();
        }

        // PictureBox_Minimize 的 MouseLeave 事件的回调函数。
        private void PictureBox_Minimize_MouseLeave(object sender, EventArgs e)
        {
            _MinimizeButtonIsPointed = false;
            _MinimizeButtonIsPressed = false;

            _UpdateControlBoxButtonBackColor(PictureBox_Minimize, Me.RecommendColors.ControlButton);

            _UpdateControlBoxButtonImage();
        }

        // PictureBox_Minimize 的 MouseDown 事件的回调函数。
        private void PictureBox_Minimize_MouseDown(object sender, MouseEventArgs e)
        {
            Me.Client.BringToFront();
            Me.Client.Focus();

            if (e.Button == MouseButtons.Left)
            {
                _MinimizeButtonIsPressed = true;

                _UpdateControlBoxButtonBackColor(PictureBox_Minimize, Me.RecommendColors.ControlButton_INC);

                _UpdateControlBoxButtonImage();
            }
        }

        // PictureBox_Minimize 的 MouseUp 事件的回调函数。
        private void PictureBox_Minimize_MouseUp(object sender, MouseEventArgs e)
        {
            Me.Client.BringToFront();
            Me.Client.Focus();

            if (e.Button == MouseButtons.Left)
            {
                _MinimizeButtonIsPressed = false;

                if (Geometry.CursorIsInControl(PictureBox_Minimize))
                {
                    _MinimizeButtonIsPointed = true;

                    _UpdateControlBoxButtonBackColor(PictureBox_Minimize, Me.RecommendColors.ControlButton_DEC);

                    _UpdateControlBoxButtonImage();
                }
                else
                {
                    PictureBox_Minimize_MouseLeave(sender, e);
                }
            }
        }

        // PictureBox_Maximize 的 MouseClick 事件的回调函数。
        private void PictureBox_Maximize_MouseClick(object sender, MouseEventArgs e)
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

        // PictureBox_Maximize 的 MouseEnter 事件的回调函数。
        private void PictureBox_Maximize_MouseEnter(object sender, EventArgs e)
        {
            _MaximizeButtonIsPointed = true;

            _UpdateControlBoxButtonBackColor(PictureBox_Maximize, Me.RecommendColors.ControlButton_DEC);

            _UpdateControlBoxButtonImage();
        }

        // PictureBox_Maximize 的 MouseLeave 事件的回调函数。
        private void PictureBox_Maximize_MouseLeave(object sender, EventArgs e)
        {
            _MaximizeButtonIsPointed = false;
            _MaximizeButtonIsPressed = false;

            _UpdateControlBoxButtonBackColor(PictureBox_Maximize, Me.RecommendColors.ControlButton);

            _UpdateControlBoxButtonImage();
        }

        // PictureBox_Maximize 的 MouseDown 事件的回调函数。
        private void PictureBox_Maximize_MouseDown(object sender, MouseEventArgs e)
        {
            Me.Client.BringToFront();
            Me.Client.Focus();

            if (e.Button == MouseButtons.Left)
            {
                _MaximizeButtonIsPressed = true;

                _UpdateControlBoxButtonBackColor(PictureBox_Maximize, Me.RecommendColors.ControlButton_INC);

                _UpdateControlBoxButtonImage();
            }
        }

        // PictureBox_Maximize 的 MouseUp 事件的回调函数。
        private void PictureBox_Maximize_MouseUp(object sender, MouseEventArgs e)
        {
            Me.Client.BringToFront();
            Me.Client.Focus();

            if (e.Button == MouseButtons.Left)
            {
                _MaximizeButtonIsPressed = false;

                if (Geometry.CursorIsInControl(PictureBox_Maximize))
                {
                    _MaximizeButtonIsPointed = true;

                    _UpdateControlBoxButtonBackColor(PictureBox_Maximize, Me.RecommendColors.ControlButton_DEC);

                    _UpdateControlBoxButtonImage();
                }
                else
                {
                    PictureBox_Maximize_MouseLeave(sender, e);
                }
            }
        }

        // PictureBox_Exit 的 MouseClick 事件的回调函数。
        private void PictureBox_Exit_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Me.Close();
            }
        }

        // PictureBox_Exit 的 MouseEnter 事件的回调函数。
        private void PictureBox_Exit_MouseEnter(object sender, EventArgs e)
        {
            _ExitButtonIsPointed = true;

            _UpdateControlBoxButtonBackColor(PictureBox_Exit, Me.RecommendColors.ExitButton_DEC);

            _UpdateControlBoxButtonImage();
        }

        // PictureBox_Exit 的 MouseLeave 事件的回调函数。
        private void PictureBox_Exit_MouseLeave(object sender, EventArgs e)
        {
            _ExitButtonIsPointed = false;
            _ExitButtonIsPressed = false;

            _UpdateControlBoxButtonBackColor(PictureBox_Exit, Me.RecommendColors.ExitButton);

            _UpdateControlBoxButtonImage();
        }

        // PictureBox_Exit 的 MouseDown 事件的回调函数。
        private void PictureBox_Exit_MouseDown(object sender, MouseEventArgs e)
        {
            Me.Client.BringToFront();
            Me.Client.Focus();

            if (e.Button == MouseButtons.Left)
            {
                _ExitButtonIsPressed = true;

                _UpdateControlBoxButtonBackColor(PictureBox_Exit, Me.RecommendColors.ExitButton_INC);

                _UpdateControlBoxButtonImage();
            }
        }

        // PictureBox_Exit 的 MouseUp 事件的回调函数。
        private void PictureBox_Exit_MouseUp(object sender, MouseEventArgs e)
        {
            Me.Client.BringToFront();
            Me.Client.Focus();

            if (e.Button == MouseButtons.Left)
            {
                _ExitButtonIsPressed = false;

                if (Geometry.CursorIsInControl(PictureBox_Exit))
                {
                    _ExitButtonIsPointed = true;

                    _UpdateControlBoxButtonBackColor(PictureBox_Exit, Me.RecommendColors.ExitButton_DEC);

                    _UpdateControlBoxButtonImage();
                }
                else
                {
                    PictureBox_Exit_MouseLeave(sender, e);
                }
            }
        }

        //

        // ContextMenuStrip_Main 的 Enter 事件的回调函数。
        private void ContextMenuStrip_Main_Enter(object sender, EventArgs e)
        {
            Me.MainMenuIsActive = true;
        }

        // ContextMenuStrip_Main 的 Leave 事件的回调函数。
        private void ContextMenuStrip_Main_Leave(object sender, EventArgs e)
        {
            Me.MainMenuIsActive = false;
        }

        // ToolStripMenuItem_Return 的 Click 事件的回调函数。
        private void ToolStripMenuItem_Return_Click(object sender, EventArgs e)
        {
            Me.Return();
        }

        // ToolStripMenuItem_Minimize 的 Click 事件的回调函数。
        private void ToolStripMenuItem_Minimize_Click(object sender, EventArgs e)
        {
            Me.Minimize();
        }

        // ToolStripMenuItem_Maximize 的 Click 事件的回调函数。
        private void ToolStripMenuItem_Maximize_Click(object sender, EventArgs e)
        {
            Me.Maximize();
        }

        // ToolStripMenuItem_Exit 的 Click 事件的回调函数。
        private void ToolStripMenuItem_Exit_Click(object sender, EventArgs e)
        {
            Me.Close();
        }

        //

        // BackgroundWorker_UpdateLayoutDelay 的 DoWork 事件的回调函数。
        private void BackgroundWorker_UpdateLayoutDelay_DoWork(object sender, DoWorkEventArgs e)
        {
            while ((DateTime.Now - _LastUpdateLayout).TotalMilliseconds < 16)
            {
                Thread.Sleep(4);
            }
        }

        // BackgroundWorker_UpdateLayoutDelay 的 RunWorkerCompleted 事件的回调函数。
        private void BackgroundWorker_UpdateLayoutDelay_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!_UpdateLayoutCanceled)
            {
                Me.UpdateLayout(_UpdateLayoutEventType);

                _LastUpdateLayout = DateTime.Now;
            }
        }

        //

        // Timer_FullScreenMonitor 的 Tick 事件的回调函数。
        private void Timer_FullScreenMonitor_Tick(object sender, EventArgs e)
        {
            if (Me.FormState == FormState.FullScreen)
            {
                double _Opacity = Me.Opacity * Me.CaptionBarOpacityRatio;

                Animation.Frame FrameShow = (frameId, frameCount, msPerFrame) =>
                {
                    double Progress = (frameId == frameCount ? 1 : 1 - Math.Pow(1 - (double)frameId / frameCount, 2));

                    this.Opacity = _Opacity * Progress;
                    this.Height = (int)(Panel_ControlBox.Height * Progress);

                    Panel_CaptionBar.Top = this.Height - Panel_CaptionBar.Height;
                };

                Animation.Frame FrameHide = (frameId, frameCount, msPerFrame) =>
                {
                    double Progress = (frameId == frameCount ? 1 : 1 - Math.Pow(1 - (double)frameId / frameCount, 2));

                    this.Opacity = _Opacity * (1 - Progress);
                    this.Height = (int)(Panel_ControlBox.Height * (1 - Progress));

                    Panel_CaptionBar.Top = this.Height - Panel_CaptionBar.Height;
                };

                Action Show = () =>
                {
                    this.Opacity = _Opacity;
                    this.Height = Panel_ControlBox.Height;

                    Panel_CaptionBar.Top = this.Height - Panel_CaptionBar.Height;
                };

                Action Hide = () =>
                {
                    this.Opacity = 0;
                    this.Height = 0;

                    Panel_CaptionBar.Top = -Panel_CaptionBar.Height;
                };

                if (Me.Enabled)
                {
                    Point CursorPosition = Cursor.Position;

                    if (this.Height == Panel_ControlBox.Height)
                    {
                        if (CursorPosition.Y > Me.Y + Panel_CaptionBar.Height * 2)
                        {
                            if (Me.Effect.HasFlag(Effect.SmoothShift) && Me.Effect.HasFlag(Effect.Fade))
                            {
                                Animation.Show(FrameHide, 9, 15);
                            }
                            else
                            {
                                Hide();
                            }
                        }
                    }
                    else
                    {
                        if (CursorPosition.Y <= Me.Y + _ExtendDist && CursorPosition.X >= Me.Right - Panel_CaptionBar.Width && CursorPosition.X <= Me.Right)
                        {
                            this.BringToFront();

                            if (Me.Effect.HasFlag(Effect.SmoothShift) && Me.Effect.HasFlag(Effect.Fade))
                            {
                                Animation.Show(FrameShow, 9, 15);
                            }
                            else
                            {
                                Show();
                            }
                        }
                    }
                }
                else
                {
                    if (this.Height == Panel_ControlBox.Height)
                    {
                        if (Me.Effect.HasFlag(Effect.SmoothShift) && Me.Effect.HasFlag(Effect.Fade))
                        {
                            Animation.Show(FrameHide, 9, 15);
                        }
                        else
                        {
                            Hide();
                        }
                    }
                }
            }
        }

        #endregion

        #region 构造函数

        // 使用 FormManager 对象初始化 CaptionBar 的新实例。
        public CaptionBar(FormManager formManager)
        {
            InitializeComponent();

            //

            Me = formManager;
        }

        #endregion

        #region 方法

        // 在 Loading 事件发生时发生。
        public void OnLoading()
        {
            Panel_FormIcon.Visible = Me.ShowIconOnCaptionBar;
            Panel_ControlBox.Visible = true;
        }

        // 在 Closing 事件发生时发生。
        public void OnClosing()
        {
            PictureBox_Exit.Enabled = ToolStripMenuItem_Exit.Enabled = false;

            PictureBox_FormIcon.MouseDoubleClick -= PictureBox_FormIcon_MouseDoubleClick;
            Panel_FormIcon.MouseDoubleClick -= Panel_FormIcon_MouseDoubleClick;
        }

        // 在 FormStyleChanged 事件发生时发生。
        public void OnFormStyleChanged()
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

        // 在 FormStateChanged 事件发生时发生。
        public void OnFormStateChanged()
        {
            _UpdateForStyleOrStateChanged();

            _UpdateControlBoxButtonImage();

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

            Timer_FullScreenMonitor.Enabled = (Me.FormState == FormState.FullScreen);
        }

        // 在 CaptionChanged 事件发生时发生。
        public void OnCaptionChanged()
        {
            _RepaintCaptionBarBitmap();
        }

        // 在 ThemeChanged 事件发生时发生。
        public void OnThemeChanged()
        {
            this.BackColor = Me.RecommendColors.CaptionBar.ToColor();

            //

            _RepaintCaptionBarBitmap();

            //

            PictureBox_FullScreen.BackColor = (_FullScreenButtonIsPressed ? Me.RecommendColors.ControlButton_INC : (_FullScreenButtonIsPointed ? Me.RecommendColors.ControlButton_DEC : Me.RecommendColors.ControlButton)).ToColor();
            PictureBox_Minimize.BackColor = (_MinimizeButtonIsPressed ? Me.RecommendColors.ControlButton_INC : (_MinimizeButtonIsPointed ? Me.RecommendColors.ControlButton_DEC : Me.RecommendColors.ControlButton)).ToColor();
            PictureBox_Maximize.BackColor = (_MaximizeButtonIsPressed ? Me.RecommendColors.ControlButton_INC : (_MaximizeButtonIsPointed ? Me.RecommendColors.ControlButton_DEC : Me.RecommendColors.ControlButton)).ToColor();
            PictureBox_Exit.BackColor = (_ExitButtonIsPressed ? Me.RecommendColors.ExitButton_INC : (_ExitButtonIsPointed ? Me.RecommendColors.ExitButton_DEC : Me.RecommendColors.ExitButton)).ToColor();

            _UpdateControlBoxButtonImage();

            //

            ContextMenuStrip_Main.BackColor = Me.RecommendColors.MenuItemBackground.ToColor();
            ToolStripMenuItem_Return.ForeColor = ToolStripMenuItem_Minimize.ForeColor = ToolStripMenuItem_Maximize.ForeColor = ToolStripMenuItem_Exit.ForeColor = Me.RecommendColors.MenuItemText.ToColor();
        }

        // 在 Activated 事件发生时发生。
        public void OnActivated()
        {
            PictureBox_FormIcon.Image = Me.Client.Icon.ToBitmap();
        }

        // 在 Deactivate 事件发生时发生。
        public void OnDeactivate()
        {
            ColorMatrix CrMtrx = new ColorMatrix();

            for (int i = 0; i < 3; i++)
            {
                CrMtrx[0, i] = 0.299F;
                CrMtrx[1, i] = 0.587F;
                CrMtrx[2, i] = 0.114F;
            }

            ImageAttributes ImgAttr = new ImageAttributes();
            ImgAttr.SetColorMatrix(CrMtrx, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            Bitmap Img = Me.Client.Icon.ToBitmap();

            Bitmap GrayscaleImg = new Bitmap(Img.Width, Img.Height);
            Graphics Grap = Graphics.FromImage(GrayscaleImg);

            Grap.DrawImage(Img, new Rectangle(0, 0, Img.Width, Img.Height), 0, 0, Img.Width, Img.Height, GraphicsUnit.Pixel, ImgAttr);

            PictureBox_FormIcon.Image = GrayscaleImg;
        }

        #endregion
    }
}