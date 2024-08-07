﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2024 chibayuki@foxmail.com

Com.WinForm.Resizer
Version 24.7.21.1040

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
    internal partial class Resizer : Form // 窗口大小调节器。
    {
        #region 非公开成员

        private FormManager Me; // 窗口管理器。

        //

        private Point _CursorPositionOfMe = new Point(); // 鼠标指针在窗口的坐标。

        private bool _MeIsResizing = false; // 是否正在改变窗口大小。

        //

        private Bitmap _ResizerBitmap; // 窗口大小调节器绘图。

        // 更新窗口大小调节器绘图。
        private void _UpdateResizerBitmap()
        {
            if (!(_ResizerBitmap is null))
            {
                _ResizerBitmap.Dispose();
            }

            _ResizerBitmap = new Bitmap(Math.Max(1, Panel_Border.Width), Math.Max(1, Panel_Border.Height));

            if (Me.FormState != FormState.Maximized && Me.FormState != FormState.FullScreen)
            {
                using (Graphics CreateResizerBmp = Graphics.FromImage(_ResizerBitmap))
                {
                    CreateResizerBmp.SmoothingMode = SmoothingMode.AntiAlias;

                    //

                    Color ShadowColor = Me.RecommendColors.Shadow.ToColor();
                    int ShadowOpacity = (int)(255 * Me.ShadowOpacityRatio);

                    for (int i = 0; i < Me.ResizerSize; i++)
                    {
                        using (Pen Pn = new Pen(Color.FromArgb(ShadowOpacity * (i + 1) * (i + 1) / Me.ResizerSize / Me.ResizerSize, ShadowColor), Math.Max(1F, Math.Min(Me.ResizerSize - i, 3F))))
                        {
                            using (GraphicsPath Path = Geometry.CreateRoundedRectanglePath(new Rectangle(new Point(i, i), new Size(_ResizerBitmap.Width - 2 * i - 1, _ResizerBitmap.Height - 2 * i - 1)), Me.ResizerSize - i + 2))
                            {
                                int N = Pn.Width >= 3F ? 1 : (Pn.Width == 2F ? 2 : (Pn.Width == 1 ? 3 : 0));

                                for (int j = 0; j < N; j++)
                                {
                                    CreateResizerBmp.DrawPath(Pn, Path);
                                }
                            }
                        }
                    }
                }
            }
        }

        // 更新并重绘窗口大小调节器绘图。
        private void _RepaintResizerBitmap()
        {
            _UpdateResizerBitmap();

            if (!(_ResizerBitmap is null))
            {
                Painting2D.PaintImageOnTransparentForm(this, _ResizerBitmap, Me.Opacity);
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
        private void _CancelUpdateLayout() => _UpdateLayoutCanceled = true;

        #endregion

        #region 回调函数

        // Resizer 的 Load 事件的回调函数。
        private void Resizer_Load(object sender, EventArgs e)
        {
            Label_Top.BackColor = Label_Bottom.BackColor = Label_Left.BackColor = Label_Right.BackColor = Label_TopLeft.BackColor = Label_TopRight.BackColor = Label_BottomLeft.BackColor = Label_BottomRight.BackColor = Color.Transparent;

            //

            OnFormStyleChanged();

            //

            Resizer_SizeChanged(this, EventArgs.Empty);
        }

        // Resizer 的 SizeChanged 事件的回调函数。
        private void Resizer_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }

            //

            Panel_Border.Size = new Size(Me.Width + 2 * Me.ResizerSize, Me.Height + 2 * Me.ResizerSize);

            Panel_FormBounds.Bounds = new Rectangle(new Point(Me.ResizerSize, Me.ResizerSize), Me.Size);

            Label_Top.Size = new Size(Panel_Border.Width - Label_TopLeft.Width - Label_TopRight.Width, Me.ResizerSize);
            Label_Bottom.Size = new Size(Panel_Border.Width - Label_BottomLeft.Width - Label_BottomRight.Width, Me.ResizerSize);
            Label_Left.Size = new Size(Me.ResizerSize, Panel_Border.Height - Label_TopLeft.Height - Label_BottomLeft.Height);
            Label_Right.Size = new Size(Me.ResizerSize, Panel_Border.Height - Label_TopRight.Height - Label_BottomRight.Height);
            Label_TopLeft.Size = Label_TopRight.Size = Label_BottomLeft.Size = Label_BottomRight.Size = new Size(2 * Me.ResizerSize, 2 * Me.ResizerSize);

            Label_Top.Location = new Point(Label_TopLeft.Width, 0);
            Label_Bottom.Location = new Point(Label_BottomLeft.Width, Panel_Border.Height - Label_Bottom.Height);
            Label_Left.Location = new Point(0, Label_TopLeft.Height);
            Label_Right.Location = new Point(Panel_Border.Width - Label_Right.Width, Label_TopRight.Height);
            Label_TopLeft.Location = new Point(0, 0);
            Label_TopRight.Location = new Point(Panel_Border.Width - Label_TopRight.Width, 0);
            Label_BottomLeft.Location = new Point(0, Panel_Border.Height - Label_BottomLeft.Height);
            Label_BottomRight.Location = new Point(Panel_Border.Width - Label_BottomRight.Width, Panel_Border.Height - Label_BottomRight.Height);

            //

            _RepaintResizerBitmap();
        }

        //

        // Panel_Border 的 Paint 事件的回调函数。
        private void Panel_Border_Paint(object sender, PaintEventArgs e) => _RepaintResizerBitmap();

        //

        // Label_Top 的 MouseDoubleClick 事件的回调函数。
        private void Label_Top_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (Me.FormStyle == FormStyle.Sizable)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (Me.FormState == FormState.HighAsScreen)
                    {
                        Me.Return();
                    }
                    else
                    {
                        Me.HighAsScreen();
                    }
                }
            }
        }

        // Label_Top 的 MouseDown 事件的回调函数。
        private void Label_Top_MouseDown(object sender, MouseEventArgs e)
        {
            Me.CaptionBar.BringToFront();
            Me.Client.BringToFront();
            Me.Client.Focus();

            if (Me.FormStyle == FormStyle.Sizable)
            {
                if (e.Button == MouseButtons.Left)
                {
                    _CursorPositionOfMe = Geometry.GetCursorPositionOfControl(Panel_FormBounds);

                    _MeIsResizing = true;

                    Cursor.Clip = FormManager.PrimaryScreenClient;
                }
            }
        }

        // Label_Top 的 MouseUp 事件的回调函数。
        private void Label_Top_MouseUp(object sender, MouseEventArgs e)
        {
            Me.CaptionBar.BringToFront();
            Me.Client.BringToFront();
            Me.Client.Focus();

            if (Me.FormStyle == FormStyle.Sizable)
            {
                if (_MeIsResizing)
                {
                    if (Me.FormState == FormState.Normal)
                    {
                        if (Cursor.Position.Y <= FormManager.PrimaryScreenClient.Y)
                        {
                            _CancelUpdateLayout();

                            if (!Me.HighAsScreen())
                            {
                                Me.UpdateLayout(UpdateLayoutEventType.Result);
                            }
                        }
                        else
                        {
                            _CancelUpdateLayout();

                            Me.Bounds_Normal = Me.Bounds_Current;

                            Me.UpdateLayout(UpdateLayoutEventType.Result);
                        }
                    }
                    else if (Me.FormState == FormState.QuarterScreen)
                    {
                        _CancelUpdateLayout();

                        Me.Bounds_QuarterScreen_Y = Me.Bounds_Current_Y;
                        Me.Bounds_QuarterScreen_Height = Me.Bounds_Current_Height;

                        Me.UpdateLayout(UpdateLayoutEventType.Result);
                    }
                }

                _MeIsResizing = false;

                Cursor.Clip = FormManager.PrimaryScreenBounds;
            }
        }

        // Label_Top 的 MouseMove 事件的回调函数。
        private void Label_Top_MouseMove(object sender, MouseEventArgs e)
        {
            if (_MeIsResizing)
            {
                if (Me.FormState == FormState.HighAsScreen)
                {
                    Me.ReturnFromHighAsScreen();
                }

                Point CursorPosition = Cursor.Position;
                Rectangle CurScrBounds = FormManager.PrimaryScreenBounds;

                if (Me.FormState == FormState.Normal)
                {
                    if (Me.Bounds_Normal_Bottom - (CursorPosition.Y - _CursorPositionOfMe.Y) >= Me.MinimumHeight)
                    {
                        if (Me.Bounds_Normal_Bottom - (CursorPosition.Y - _CursorPositionOfMe.Y) <= Math.Min(CurScrBounds.Height, Me.MaximumHeight))
                        {
                            Me.Bounds_Current_Y = CursorPosition.Y - _CursorPositionOfMe.Y;
                            Me.Bounds_Current_Height = Me.Bounds_Normal_Bottom - Me.Bounds_Current_Y;
                        }
                        else
                        {
                            Me.Bounds_Current_Y = Me.Bounds_Normal_Bottom - Math.Min(CurScrBounds.Height, Me.MaximumHeight);
                            Me.Bounds_Current_Height = Math.Min(CurScrBounds.Height, Me.MaximumHeight);
                        }
                    }
                    else
                    {
                        Me.Bounds_Current_Y = Me.Bounds_Normal_Bottom - Me.MinimumHeight;
                        Me.Bounds_Current_Height = Me.MinimumHeight;
                    }

                    _TryToUpdateLayout(UpdateLayoutEventType.Process);
                }
                else if (Me.FormState == FormState.QuarterScreen)
                {
                    if (Me.Bounds_QuarterScreen_Bottom - (CursorPosition.Y - _CursorPositionOfMe.Y) >= Me.MinimumHeight)
                    {
                        if (Me.Bounds_QuarterScreen_Bottom - (CursorPosition.Y - _CursorPositionOfMe.Y) <= Math.Min(CurScrBounds.Height, Me.MaximumHeight))
                        {
                            Me.Bounds_Current_Y = CursorPosition.Y - _CursorPositionOfMe.Y;
                            Me.Bounds_Current_Height = Me.Bounds_QuarterScreen_Bottom - Me.Bounds_Current_Y;
                        }
                        else
                        {
                            Me.Bounds_Current_Y = Me.Bounds_QuarterScreen_Bottom - Math.Min(CurScrBounds.Height, Me.MaximumHeight);
                            Me.Bounds_Current_Height = Math.Min(CurScrBounds.Height, Me.MaximumHeight);
                        }
                    }
                    else
                    {
                        Me.Bounds_Current_Y = Me.Bounds_QuarterScreen_Bottom - Me.MinimumHeight;
                        Me.Bounds_Current_Height = Me.MinimumHeight;
                    }

                    _TryToUpdateLayout(UpdateLayoutEventType.Process);
                }
            }
        }

        // Label_Bottom 的 MouseDoubleClick 事件的回调函数。
        private void Label_Bottom_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (Me.FormStyle == FormStyle.Sizable)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (Me.FormState == FormState.HighAsScreen)
                    {
                        Me.Return();
                    }
                    else
                    {
                        Me.HighAsScreen();
                    }
                }
            }
        }

        // Label_Bottom 的 MouseDown 事件的回调函数。
        private void Label_Bottom_MouseDown(object sender, MouseEventArgs e)
        {
            Me.CaptionBar.BringToFront();
            Me.Client.BringToFront();
            Me.Client.Focus();

            if (Me.FormStyle == FormStyle.Sizable)
            {
                if (e.Button == MouseButtons.Left)
                {
                    _CursorPositionOfMe = Geometry.GetCursorPositionOfControl(Panel_FormBounds);

                    _MeIsResizing = true;

                    Cursor.Clip = FormManager.PrimaryScreenClient;
                }
            }
        }

        // Label_Bottom 的 MouseUp 事件的回调函数。
        private void Label_Bottom_MouseUp(object sender, MouseEventArgs e)
        {
            Me.CaptionBar.BringToFront();
            Me.Client.BringToFront();
            Me.Client.Focus();

            if (Me.FormStyle == FormStyle.Sizable)
            {
                if (_MeIsResizing)
                {
                    if (Me.FormState == FormState.Normal)
                    {
                        if (Cursor.Position.Y >= FormManager.PrimaryScreenClient.Bottom - 1)
                        {
                            _CancelUpdateLayout();

                            if (!Me.HighAsScreen())
                            {
                                Me.UpdateLayout(UpdateLayoutEventType.SizeChanged);
                            }
                        }
                        else
                        {
                            _CancelUpdateLayout();

                            Me.Bounds_Normal = Me.Bounds_Current;

                            Me.UpdateLayout(UpdateLayoutEventType.SizeChanged);
                        }
                    }
                    else if (Me.FormState == FormState.QuarterScreen)
                    {
                        _CancelUpdateLayout();

                        Me.Bounds_QuarterScreen_Y = Me.Bounds_Current_Y;
                        Me.Bounds_QuarterScreen_Height = Me.Bounds_Current_Height;

                        Me.UpdateLayout(UpdateLayoutEventType.SizeChanged);
                    }
                }

                _MeIsResizing = false;

                Cursor.Clip = FormManager.PrimaryScreenBounds;
            }
        }

        // Label_Bottom 的 MouseMove 事件的回调函数。
        private void Label_Bottom_MouseMove(object sender, MouseEventArgs e)
        {
            if (_MeIsResizing)
            {
                if (Me.FormState == FormState.HighAsScreen)
                {
                    _CursorPositionOfMe.Y = Me.Bounds_Normal_Height - (Me.Bounds_Current_Height - _CursorPositionOfMe.Y);

                    Me.ReturnFromHighAsScreen();
                }

                Point CursorPosition = Cursor.Position;
                Rectangle CurScrBounds = FormManager.PrimaryScreenBounds;

                if (Me.FormState == FormState.Normal)
                {
                    if (Me.Bounds_Normal_Height + CursorPosition.Y - Me.Bounds_Normal_Y - _CursorPositionOfMe.Y >= Me.MinimumHeight)
                    {
                        Me.Bounds_Current_Y = Me.Bounds_Normal_Y;
                        Me.Bounds_Current_Height = Math.Min(Math.Min(CurScrBounds.Height, Me.MaximumHeight), Me.Bounds_Normal_Height - Me.Bounds_Normal_Y + CursorPosition.Y - _CursorPositionOfMe.Y);
                    }
                    else
                    {
                        Me.Bounds_Current_Y = Me.Bounds_Normal_Y;
                        Me.Bounds_Current_Height = Me.MinimumHeight;
                    }

                    _TryToUpdateLayout(UpdateLayoutEventType.Resize);
                }
                else if (Me.FormState == FormState.QuarterScreen)
                {
                    if (Me.Bounds_QuarterScreen_Height + CursorPosition.Y - Me.Bounds_QuarterScreen_Y - _CursorPositionOfMe.Y <= Math.Min(CurScrBounds.Height, Me.MaximumHeight) && Me.Bounds_QuarterScreen_Height + CursorPosition.Y - Me.Bounds_QuarterScreen_Y - _CursorPositionOfMe.Y >= Me.MinimumHeight)
                    {
                        Me.Bounds_Current_Y = Me.Bounds_QuarterScreen_Y;
                        Me.Bounds_Current_Height = Math.Min(Math.Min(CurScrBounds.Height, Me.MaximumHeight), Me.Bounds_QuarterScreen_Height - Me.Bounds_QuarterScreen_Y + CursorPosition.Y - _CursorPositionOfMe.Y);
                    }
                    else
                    {
                        Me.Bounds_Current_Y = Me.Bounds_QuarterScreen_Y;
                        Me.Bounds_Current_Height = Me.MinimumHeight;
                    }

                    _TryToUpdateLayout(UpdateLayoutEventType.Resize);
                }
            }
        }

        // Label_Left 的 MouseDown 事件的回调函数。
        private void Label_Left_MouseDown(object sender, MouseEventArgs e)
        {
            Me.CaptionBar.BringToFront();
            Me.Client.BringToFront();
            Me.Client.Focus();

            if (Me.FormStyle == FormStyle.Sizable)
            {
                if (e.Button == MouseButtons.Left)
                {
                    _CursorPositionOfMe = Geometry.GetCursorPositionOfControl(Panel_FormBounds);

                    if (Me.FormState == FormState.Normal || Me.FormState == FormState.HighAsScreen)
                    {
                        Me.Bounds_Normal_X = Me.Bounds_Current_X;
                        Me.Bounds_Normal_Width = Me.Bounds_Current_Width;
                    }

                    _MeIsResizing = true;

                    Cursor.Clip = FormManager.PrimaryScreenClient;
                }
            }
        }

        // Label_Left 的 MouseUp 事件的回调函数。
        private void Label_Left_MouseUp(object sender, MouseEventArgs e)
        {
            Me.CaptionBar.BringToFront();
            Me.Client.BringToFront();
            Me.Client.Focus();

            if (Me.FormStyle == FormStyle.Sizable)
            {
                if (_MeIsResizing)
                {
                    if (Me.FormState == FormState.Normal || Me.FormState == FormState.HighAsScreen)
                    {
                        _CancelUpdateLayout();

                        Me.Bounds_Normal_X = Me.Bounds_Current_X;
                        Me.Bounds_Normal_Width = Me.Bounds_Current_Width;

                        Me.UpdateLayout(UpdateLayoutEventType.Result);
                    }
                    else if (Me.FormState == FormState.QuarterScreen)
                    {
                        _CancelUpdateLayout();

                        Me.Bounds_QuarterScreen_X = Me.Bounds_Current_X;
                        Me.Bounds_QuarterScreen_Width = Me.Bounds_Current_Width;

                        Me.UpdateLayout(UpdateLayoutEventType.Result);
                    }
                }

                _MeIsResizing = false;

                Cursor.Clip = FormManager.PrimaryScreenBounds;
            }
        }

        // Label_Left 的 MouseMove 事件的回调函数。
        private void Label_Left_MouseMove(object sender, MouseEventArgs e)
        {
            if (_MeIsResizing)
            {
                Point CursorPosition = Cursor.Position;
                Rectangle CurScrBounds = FormManager.PrimaryScreenBounds;

                if (Me.FormState == FormState.Normal || Me.FormState == FormState.HighAsScreen)
                {
                    if (Me.Bounds_Normal_Right - (CursorPosition.X - _CursorPositionOfMe.X) >= Me.MinimumWidth)
                    {
                        if (Me.Bounds_Normal_Right - (CursorPosition.X - _CursorPositionOfMe.X) <= Math.Min(CurScrBounds.Width, Me.MaximumWidth))
                        {
                            Me.Bounds_Current_X = CursorPosition.X - _CursorPositionOfMe.X;
                            Me.Bounds_Current_Width = Me.Bounds_Normal_Right - Me.Bounds_Current_X;
                        }
                        else
                        {
                            Me.Bounds_Current_X = Me.Bounds_Normal_Right - Math.Min(CurScrBounds.Width, Me.MaximumWidth);
                            Me.Bounds_Current_Width = Math.Min(CurScrBounds.Width, Me.MaximumWidth);
                        }
                    }
                    else
                    {
                        Me.Bounds_Current_X = Me.Bounds_Normal_Right - Me.MinimumWidth;
                        Me.Bounds_Current_Width = Me.MinimumWidth;
                    }

                    _TryToUpdateLayout(UpdateLayoutEventType.Process);
                }
                else if (Me.FormState == FormState.QuarterScreen)
                {
                    if (Me.Bounds_QuarterScreen_Right - (CursorPosition.X - _CursorPositionOfMe.X) >= Me.MinimumWidth)
                    {
                        if (Me.Bounds_QuarterScreen_Right - (CursorPosition.X - _CursorPositionOfMe.X) <= Math.Min(CurScrBounds.Width, Me.MaximumWidth))
                        {
                            Me.Bounds_Current_X = CursorPosition.X - _CursorPositionOfMe.X;
                            Me.Bounds_Current_Width = Me.Bounds_QuarterScreen_Right - Me.Bounds_Current_X;
                        }
                        else
                        {
                            Me.Bounds_Current_X = Me.Bounds_QuarterScreen_Right - Math.Min(CurScrBounds.Width, Me.MaximumWidth);
                            Me.Bounds_Current_Width = Math.Min(CurScrBounds.Width, Me.MaximumWidth);
                        }
                    }
                    else
                    {
                        Me.Bounds_Current_X = Me.Bounds_QuarterScreen_Right - Me.MinimumWidth;
                        Me.Bounds_Current_Width = Me.MinimumWidth;
                    }

                    _TryToUpdateLayout(UpdateLayoutEventType.Process);
                }
            }
        }

        // Label_Right 的 MouseDown 事件的回调函数。
        private void Label_Right_MouseDown(object sender, MouseEventArgs e)
        {
            Me.CaptionBar.BringToFront();
            Me.Client.BringToFront();
            Me.Client.Focus();

            if (Me.FormStyle == FormStyle.Sizable)
            {
                if (e.Button == MouseButtons.Left)
                {
                    _CursorPositionOfMe = Geometry.GetCursorPositionOfControl(Panel_FormBounds);

                    if (Me.FormState == FormState.Normal || Me.FormState == FormState.HighAsScreen)
                    {
                        Me.Bounds_Normal_X = Me.Bounds_Current_X;
                        Me.Bounds_Normal_Width = Me.Bounds_Current_Width;
                    }

                    _MeIsResizing = true;

                    Cursor.Clip = FormManager.PrimaryScreenClient;
                }
            }
        }

        // Label_Right 的 MouseUp 事件的回调函数。
        private void Label_Right_MouseUp(object sender, MouseEventArgs e)
        {
            Me.CaptionBar.BringToFront();
            Me.Client.BringToFront();
            Me.Client.Focus();

            if (Me.FormStyle == FormStyle.Sizable)
            {
                if (_MeIsResizing)
                {
                    if (Me.FormState == FormState.Normal || Me.FormState == FormState.HighAsScreen)
                    {
                        _CancelUpdateLayout();

                        Me.Bounds_Normal_Width = Me.Bounds_Current_Width;

                        Me.UpdateLayout(UpdateLayoutEventType.SizeChanged);
                    }
                    else if (Me.FormState == FormState.QuarterScreen)
                    {
                        _CancelUpdateLayout();

                        Me.Bounds_QuarterScreen_Width = Me.Bounds_Current_Width;

                        Me.UpdateLayout(UpdateLayoutEventType.SizeChanged);
                    }
                }

                _MeIsResizing = false;

                Cursor.Clip = FormManager.PrimaryScreenBounds;
            }
        }

        // Label_Right 的 MouseMove 事件的回调函数。
        private void Label_Right_MouseMove(object sender, MouseEventArgs e)
        {
            if (_MeIsResizing)
            {
                Point CursorPosition = Cursor.Position;
                Rectangle CurScrBounds = FormManager.PrimaryScreenBounds;

                if (Me.FormState == FormState.Normal || Me.FormState == FormState.HighAsScreen)
                {
                    if (Me.Bounds_Normal_Width + CursorPosition.X - Me.Bounds_Normal_X - _CursorPositionOfMe.X >= Me.MinimumWidth)
                    {
                        Me.Bounds_Current_X = Me.Bounds_Normal_X;
                        Me.Bounds_Current_Width = Math.Min(Math.Min(CurScrBounds.Width, Me.MaximumWidth), Me.Bounds_Normal_Width - Me.Bounds_Normal_X + CursorPosition.X - _CursorPositionOfMe.X);
                    }
                    else
                    {
                        Me.Bounds_Current_X = Me.Bounds_Normal_X;
                        Me.Bounds_Current_Width = Me.MinimumWidth;
                    }

                    _TryToUpdateLayout(UpdateLayoutEventType.Resize);
                }
                else if (Me.FormState == FormState.QuarterScreen)
                {
                    if (Me.Bounds_QuarterScreen_Width + CursorPosition.X - Me.Bounds_QuarterScreen_X - _CursorPositionOfMe.X >= Me.MinimumWidth)
                    {
                        Me.Bounds_Current_X = Me.Bounds_QuarterScreen_X;
                        Me.Bounds_Current_Width = Math.Min(Math.Min(CurScrBounds.Width, Me.MaximumWidth), Me.Bounds_QuarterScreen_Width - Me.Bounds_QuarterScreen_X + CursorPosition.X - _CursorPositionOfMe.X);
                    }
                    else
                    {
                        Me.Bounds_Current_X = Me.Bounds_QuarterScreen_X;
                        Me.Bounds_Current_Width = Me.MinimumWidth;
                    }

                    _TryToUpdateLayout(UpdateLayoutEventType.Resize);
                }
            }
        }

        // Label_TopLeft 的 MouseDown 事件的回调函数。
        private void Label_TopLeft_MouseDown(object sender, MouseEventArgs e)
        {
            Me.CaptionBar.BringToFront();
            Me.Client.BringToFront();
            Me.Client.Focus();

            if (Me.FormStyle == FormStyle.Sizable)
            {
                if (e.Button == MouseButtons.Left)
                {
                    _CursorPositionOfMe = Geometry.GetCursorPositionOfControl(Panel_FormBounds);

                    if (Me.FormState == FormState.Normal || Me.FormState == FormState.HighAsScreen)
                    {
                        Me.Bounds_Normal_X = Me.Bounds_Current_X;
                        Me.Bounds_Normal_Width = Me.Bounds_Current_Width;
                    }

                    _MeIsResizing = true;

                    Cursor.Clip = FormManager.PrimaryScreenClient;
                }
            }
        }

        // Label_TopLeft 的 MouseUp 事件的回调函数。
        private void Label_TopLeft_MouseUp(object sender, MouseEventArgs e)
        {
            Me.CaptionBar.BringToFront();
            Me.Client.BringToFront();
            Me.Client.Focus();

            if (Me.FormStyle == FormStyle.Sizable)
            {
                if (_MeIsResizing)
                {
                    if (Me.FormState == FormState.Normal)
                    {
                        if (Cursor.Position.Y <= FormManager.PrimaryScreenClient.Y)
                        {
                            _CancelUpdateLayout();

                            Me.Bounds_Normal_X = Me.Bounds_Current_X;
                            Me.Bounds_Normal_Width = Me.Bounds_Current_Width;

                            if (!Me.HighAsScreen())
                            {
                                Me.UpdateLayout(UpdateLayoutEventType.Result);
                            }
                        }
                        else
                        {
                            _CancelUpdateLayout();

                            Me.Bounds_Normal = Me.Bounds_Current;

                            Me.UpdateLayout(UpdateLayoutEventType.Result);
                        }
                    }
                    else if (Me.FormState == FormState.QuarterScreen)
                    {
                        _CancelUpdateLayout();

                        Me.Bounds_QuarterScreen = Me.Bounds_Current;

                        Me.UpdateLayout(UpdateLayoutEventType.Result);
                    }
                }

                _MeIsResizing = false;

                Cursor.Clip = FormManager.PrimaryScreenBounds;
            }
        }

        // Label_TopLeft 的 MouseMove 事件的回调函数。
        private void Label_TopLeft_MouseMove(object sender, MouseEventArgs e)
        {
            if (_MeIsResizing)
            {
                if (Me.FormState == FormState.HighAsScreen)
                {
                    Me.ReturnFromHighAsScreen();
                }

                Point CursorPosition = Cursor.Position;
                Rectangle CurScrBounds = FormManager.PrimaryScreenBounds;

                if (Me.FormState == FormState.Normal)
                {
                    if (Me.Bounds_Normal_Bottom - (CursorPosition.Y - _CursorPositionOfMe.Y) >= Me.MinimumHeight)
                    {
                        if (Me.Bounds_Normal_Bottom - (CursorPosition.Y - _CursorPositionOfMe.Y) <= Math.Min(CurScrBounds.Height, Me.MaximumHeight))
                        {
                            Me.Bounds_Current_Y = CursorPosition.Y - _CursorPositionOfMe.Y;
                            Me.Bounds_Current_Height = Me.Bounds_Normal_Bottom - Me.Bounds_Current_Y;
                        }
                        else
                        {
                            Me.Bounds_Current_Y = Me.Bounds_Normal_Bottom - Math.Min(CurScrBounds.Height, Me.MaximumHeight);
                            Me.Bounds_Current_Height = Math.Min(CurScrBounds.Height, Me.MaximumHeight);
                        }
                    }
                    else
                    {
                        Me.Bounds_Current_Y = Me.Bounds_Normal_Bottom - Me.MinimumHeight;
                        Me.Bounds_Current_Height = Me.MinimumHeight;
                    }

                    if (Me.Bounds_Normal_Right - (CursorPosition.X - _CursorPositionOfMe.X) >= Me.MinimumWidth)
                    {
                        if (Me.Bounds_Normal_Right - (CursorPosition.X - _CursorPositionOfMe.X) <= Math.Min(CurScrBounds.Width, Me.MaximumWidth))
                        {
                            Me.Bounds_Current_X = CursorPosition.X - _CursorPositionOfMe.X;
                            Me.Bounds_Current_Width = Me.Bounds_Normal_Right - Me.Bounds_Current_X;
                        }
                        else
                        {
                            Me.Bounds_Current_X = Me.Bounds_Normal_Right - Math.Min(CurScrBounds.Width, Me.MaximumWidth);
                            Me.Bounds_Current_Width = Math.Min(CurScrBounds.Width, Me.MaximumWidth);
                        }
                    }
                    else
                    {
                        Me.Bounds_Current_X = Me.Bounds_Normal_Right - Me.MinimumWidth;
                        Me.Bounds_Current_Width = Me.MinimumWidth;
                    }

                    _TryToUpdateLayout(UpdateLayoutEventType.Process);
                }
                else if (Me.FormState == FormState.QuarterScreen)
                {
                    if (Me.Bounds_QuarterScreen_Bottom - (CursorPosition.Y - _CursorPositionOfMe.Y) >= Me.MinimumHeight)
                    {
                        if (Me.Bounds_QuarterScreen_Bottom - (CursorPosition.Y - _CursorPositionOfMe.Y) <= Math.Min(CurScrBounds.Height, Me.MaximumHeight))
                        {
                            Me.Bounds_Current_Y = CursorPosition.Y - _CursorPositionOfMe.Y;
                            Me.Bounds_Current_Height = Me.Bounds_QuarterScreen_Bottom - Me.Bounds_Current_Y;
                        }
                        else
                        {
                            Me.Bounds_Current_Y = Me.Bounds_QuarterScreen_Bottom - Math.Min(CurScrBounds.Height, Me.MaximumHeight);
                            Me.Bounds_Current_Height = Math.Min(CurScrBounds.Height, Me.MaximumHeight);
                        }
                    }
                    else
                    {
                        Me.Bounds_Current_Y = Me.Bounds_QuarterScreen_Bottom - Me.MinimumHeight;
                        Me.Bounds_Current_Height = Me.MinimumHeight;
                    }

                    if (Me.Bounds_QuarterScreen_Right - (CursorPosition.X - _CursorPositionOfMe.X) >= Me.MinimumWidth)
                    {
                        if (Me.Bounds_QuarterScreen_Right - (CursorPosition.X - _CursorPositionOfMe.X) <= Math.Min(CurScrBounds.Width, Me.MaximumWidth))
                        {
                            Me.Bounds_Current_X = CursorPosition.X - _CursorPositionOfMe.X;
                            Me.Bounds_Current_Width = Me.Bounds_QuarterScreen_Right - Me.Bounds_Current_X;
                        }
                        else
                        {
                            Me.Bounds_Current_X = Me.Bounds_QuarterScreen_Right - Math.Min(CurScrBounds.Width, Me.MaximumWidth);
                            Me.Bounds_Current_Width = Math.Min(CurScrBounds.Width, Me.MaximumWidth);
                        }
                    }
                    else
                    {
                        Me.Bounds_Current_X = Me.Bounds_QuarterScreen_Right - Me.MinimumWidth;
                        Me.Bounds_Current_Width = Me.MinimumWidth;
                    }

                    _TryToUpdateLayout(UpdateLayoutEventType.Process);
                }
            }
        }

        // Label_TopRight 的 MouseDown 事件的回调函数。
        private void Label_TopRight_MouseDown(object sender, MouseEventArgs e)
        {
            Me.CaptionBar.BringToFront();
            Me.Client.BringToFront();
            Me.Client.Focus();

            if (Me.FormStyle == FormStyle.Sizable)
            {
                if (e.Button == MouseButtons.Left)
                {
                    _CursorPositionOfMe = Geometry.GetCursorPositionOfControl(Panel_FormBounds);

                    if (Me.FormState == FormState.Normal || Me.FormState == FormState.HighAsScreen)
                    {
                        Me.Bounds_Normal_X = Me.Bounds_Current_X;
                        Me.Bounds_Normal_Width = Me.Bounds_Current_Width;
                    }

                    _MeIsResizing = true;

                    Cursor.Clip = FormManager.PrimaryScreenClient;
                }
            }
        }

        // Label_TopRight 的 MouseUp 事件的回调函数。
        private void Label_TopRight_MouseUp(object sender, MouseEventArgs e)
        {
            Me.CaptionBar.BringToFront();
            Me.Client.BringToFront();
            Me.Client.Focus();

            if (Me.FormStyle == FormStyle.Sizable)
            {
                if (_MeIsResizing)
                {
                    if (Me.FormState == FormState.Normal)
                    {
                        if (Cursor.Position.Y <= FormManager.PrimaryScreenClient.Y)
                        {
                            _CancelUpdateLayout();

                            Me.Bounds_Normal_X = Me.Bounds_Current_X;
                            Me.Bounds_Normal_Width = Me.Bounds_Current_Width;

                            if (!Me.HighAsScreen())
                            {
                                Me.UpdateLayout(UpdateLayoutEventType.Result);
                            }
                        }
                        else
                        {
                            _CancelUpdateLayout();

                            Me.Bounds_Normal = Me.Bounds_Current;

                            Me.UpdateLayout(UpdateLayoutEventType.Result);
                        }
                    }
                    else if (Me.FormState == FormState.QuarterScreen)
                    {
                        _CancelUpdateLayout();

                        Me.Bounds_QuarterScreen = Me.Bounds_Current;

                        Me.UpdateLayout(UpdateLayoutEventType.Result);
                    }
                }

                _MeIsResizing = false;

                Cursor.Clip = FormManager.PrimaryScreenBounds;
            }
        }

        // Label_TopRight 的 MouseMove 事件的回调函数。
        private void Label_TopRight_MouseMove(object sender, MouseEventArgs e)
        {
            if (_MeIsResizing)
            {
                if (Me.FormState == FormState.HighAsScreen)
                {
                    Me.ReturnFromHighAsScreen();
                }

                Point CursorPosition = Cursor.Position;
                Rectangle CurScrBounds = FormManager.PrimaryScreenBounds;

                if (Me.FormState == FormState.Normal)
                {
                    if (Me.Bounds_Normal_Bottom - (CursorPosition.Y - _CursorPositionOfMe.Y) >= Me.MinimumHeight)
                    {
                        if (Me.Bounds_Normal_Bottom - (CursorPosition.Y - _CursorPositionOfMe.Y) <= Math.Min(CurScrBounds.Height, Me.MaximumHeight))
                        {
                            Me.Bounds_Current_Y = CursorPosition.Y - _CursorPositionOfMe.Y;
                            Me.Bounds_Current_Height = Me.Bounds_Normal_Bottom - Me.Bounds_Current_Y;
                        }
                        else
                        {
                            Me.Bounds_Current_Y = Me.Bounds_Normal_Bottom - Math.Min(CurScrBounds.Height, Me.MaximumHeight);
                            Me.Bounds_Current_Height = Math.Min(CurScrBounds.Height, Me.MaximumHeight);
                        }
                    }
                    else
                    {
                        Me.Bounds_Current_Y = Me.Bounds_Normal_Bottom - Me.MinimumHeight;
                        Me.Bounds_Current_Height = Me.MinimumHeight;
                    }

                    if (Me.Bounds_Normal_Width + CursorPosition.X - Me.Bounds_Normal_X - _CursorPositionOfMe.X >= Me.MinimumWidth)
                    {
                        Me.Bounds_Current_X = Me.Bounds_Normal_X;
                        Me.Bounds_Current_Width = Math.Min(Math.Min(CurScrBounds.Width, Me.MaximumWidth), Me.Bounds_Normal_Width - Me.Bounds_Normal_X + CursorPosition.X - _CursorPositionOfMe.X);
                    }
                    else
                    {
                        Me.Bounds_Current_X = Me.Bounds_Normal_X;
                        Me.Bounds_Current_Width = Me.MinimumWidth;
                    }

                    _TryToUpdateLayout(UpdateLayoutEventType.Process);
                }
                else if (Me.FormState == FormState.QuarterScreen)
                {
                    if (Me.Bounds_QuarterScreen_Bottom - (CursorPosition.Y - _CursorPositionOfMe.Y) >= Me.MinimumHeight)
                    {
                        if (Me.Bounds_QuarterScreen_Bottom - (CursorPosition.Y - _CursorPositionOfMe.Y) <= Math.Min(CurScrBounds.Height, Me.MaximumHeight))
                        {
                            Me.Bounds_Current_Y = CursorPosition.Y - _CursorPositionOfMe.Y;
                            Me.Bounds_Current_Height = Me.Bounds_QuarterScreen_Bottom - Me.Bounds_Current_Y;
                        }
                        else
                        {
                            Me.Bounds_Current_Y = Me.Bounds_QuarterScreen_Bottom - Math.Min(CurScrBounds.Height, Me.MaximumHeight);
                            Me.Bounds_Current_Height = Math.Min(CurScrBounds.Height, Me.MaximumHeight);
                        }
                    }
                    else
                    {
                        Me.Bounds_Current_Y = Me.Bounds_QuarterScreen_Bottom - Me.MinimumHeight;
                        Me.Bounds_Current_Height = Me.MinimumHeight;
                    }

                    if (Me.Bounds_QuarterScreen_Width + CursorPosition.X - Me.Bounds_QuarterScreen_X - _CursorPositionOfMe.X >= Me.MinimumWidth)
                    {
                        Me.Bounds_Current_X = Me.Bounds_QuarterScreen_X;
                        Me.Bounds_Current_Width = Math.Min(Math.Min(CurScrBounds.Width, Me.MaximumWidth), Me.Bounds_QuarterScreen_Width - Me.Bounds_QuarterScreen_X + CursorPosition.X - _CursorPositionOfMe.X);
                    }
                    else
                    {
                        Me.Bounds_Current_X = Me.Bounds_QuarterScreen_X;
                        Me.Bounds_Current_Width = Me.MinimumWidth;
                    }

                    _TryToUpdateLayout(UpdateLayoutEventType.Process);
                }
            }
        }

        // Label_BottomLeft 的 MouseDown 事件的回调函数。
        private void Label_BottomLeft_MouseDown(object sender, MouseEventArgs e)
        {
            Me.CaptionBar.BringToFront();
            Me.Client.BringToFront();
            Me.Client.Focus();

            if (Me.FormStyle == FormStyle.Sizable)
            {
                if (e.Button == MouseButtons.Left)
                {
                    _CursorPositionOfMe = Geometry.GetCursorPositionOfControl(Panel_FormBounds);

                    if (Me.FormState == FormState.Normal || Me.FormState == FormState.HighAsScreen)
                    {
                        Me.Bounds_Normal_X = Me.Bounds_Current_X;
                        Me.Bounds_Normal_Width = Me.Bounds_Current_Width;
                    }

                    _MeIsResizing = true;

                    Cursor.Clip = FormManager.PrimaryScreenClient;
                }
            }
        }

        // Label_BottomLeft 的 MouseUp 事件的回调函数。
        private void Label_BottomLeft_MouseUp(object sender, MouseEventArgs e)
        {
            Me.CaptionBar.BringToFront();
            Me.Client.BringToFront();
            Me.Client.Focus();

            if (Me.FormStyle == FormStyle.Sizable)
            {
                if (_MeIsResizing)
                {
                    if (Me.FormState == FormState.Normal)
                    {
                        if (Cursor.Position.Y >= FormManager.PrimaryScreenClient.Bottom - 1)
                        {
                            _CancelUpdateLayout();

                            Me.Bounds_Normal_X = Me.Bounds_Current_X;
                            Me.Bounds_Normal_Width = Me.Bounds_Current_Width;

                            if (!Me.HighAsScreen())
                            {
                                Me.UpdateLayout(UpdateLayoutEventType.Result);
                            }
                        }
                        else
                        {
                            _CancelUpdateLayout();

                            Me.Bounds_Normal = Me.Bounds_Current;

                            Me.UpdateLayout(UpdateLayoutEventType.Result);
                        }
                    }
                    else if (Me.FormState == FormState.QuarterScreen)
                    {
                        _CancelUpdateLayout();

                        Me.Bounds_QuarterScreen = Me.Bounds_Current;

                        Me.UpdateLayout(UpdateLayoutEventType.Result);
                    }
                }

                _MeIsResizing = false;

                Cursor.Clip = FormManager.PrimaryScreenBounds;
            }
        }

        // Label_BottomLeft 的 MouseMove 事件的回调函数。
        private void Label_BottomLeft_MouseMove(object sender, MouseEventArgs e)
        {
            if (_MeIsResizing)
            {
                if (Me.FormState == FormState.HighAsScreen)
                {
                    _CursorPositionOfMe.Y = Me.Bounds_Normal_Height - (Me.Bounds_Current_Height - _CursorPositionOfMe.Y);

                    Me.ReturnFromHighAsScreen();
                }

                Point CursorPosition = Cursor.Position;
                Rectangle CurScrBounds = FormManager.PrimaryScreenBounds;

                if (Me.FormState == FormState.Normal)
                {
                    if (Me.Bounds_Normal_Height + CursorPosition.Y - Me.Bounds_Normal_Y - _CursorPositionOfMe.Y >= Me.MinimumHeight)
                    {
                        Me.Bounds_Current_Y = Me.Bounds_Normal_Y;
                        Me.Bounds_Current_Height = Math.Min(Math.Min(CurScrBounds.Height, Me.MaximumHeight), Me.Bounds_Normal_Height - Me.Bounds_Normal_Y + CursorPosition.Y - _CursorPositionOfMe.Y);
                    }
                    else
                    {
                        Me.Bounds_Current_Y = Me.Bounds_Normal_Y;
                        Me.Bounds_Current_Height = Me.MinimumHeight;
                    }

                    if (Me.Bounds_Normal_Right - (CursorPosition.X - _CursorPositionOfMe.X) >= Me.MinimumWidth)
                    {
                        if (Me.Bounds_Normal_Right - (CursorPosition.X - _CursorPositionOfMe.X) <= Math.Min(CurScrBounds.Width, Me.MaximumWidth))
                        {
                            Me.Bounds_Current_X = CursorPosition.X - _CursorPositionOfMe.X;
                            Me.Bounds_Current_Width = Me.Bounds_Normal_Right - Me.Bounds_Current_X;
                        }
                        else
                        {
                            Me.Bounds_Current_X = Me.Bounds_Normal_Right - Math.Min(CurScrBounds.Width, Me.MaximumWidth);
                            Me.Bounds_Current_Width = Math.Min(CurScrBounds.Width, Me.MaximumWidth);
                        }
                    }
                    else
                    {
                        Me.Bounds_Current_X = Me.Bounds_Normal_Right - Me.MinimumWidth;
                        Me.Bounds_Current_Width = Me.MinimumWidth;
                    }

                    _TryToUpdateLayout(UpdateLayoutEventType.Process);
                }
                else if (Me.FormState == FormState.QuarterScreen)
                {
                    if (Me.Bounds_QuarterScreen_Height + CursorPosition.Y - Me.Bounds_QuarterScreen_Y - _CursorPositionOfMe.Y <= Math.Min(CurScrBounds.Height, Me.MaximumHeight) && Me.Bounds_QuarterScreen_Height + CursorPosition.Y - Me.Bounds_QuarterScreen_Y - _CursorPositionOfMe.Y >= Me.MinimumHeight)
                    {
                        Me.Bounds_Current_Y = Me.Bounds_QuarterScreen_Y;
                        Me.Bounds_Current_Height = Math.Min(Math.Min(CurScrBounds.Height, Me.MaximumHeight), Me.Bounds_QuarterScreen_Height - Me.Bounds_QuarterScreen_Y + CursorPosition.Y - _CursorPositionOfMe.Y);
                    }
                    else
                    {
                        Me.Bounds_Current_Y = Me.Bounds_QuarterScreen_Y;
                        Me.Bounds_Current_Height = Me.MinimumHeight;
                    }

                    if (Me.Bounds_QuarterScreen_Right - (CursorPosition.X - _CursorPositionOfMe.X) >= Me.MinimumWidth)
                    {
                        if (Me.Bounds_QuarterScreen_Right - (CursorPosition.X - _CursorPositionOfMe.X) <= Math.Min(CurScrBounds.Width, Me.MaximumWidth))
                        {
                            Me.Bounds_Current_X = CursorPosition.X - _CursorPositionOfMe.X;
                            Me.Bounds_Current_Width = Me.Bounds_QuarterScreen_Right - Me.Bounds_Current_X;
                        }
                        else
                        {
                            Me.Bounds_Current_X = Me.Bounds_QuarterScreen_Right - Math.Min(CurScrBounds.Width, Me.MaximumWidth);
                            Me.Bounds_Current_Width = Math.Min(CurScrBounds.Width, Me.MaximumWidth);
                        }
                    }
                    else
                    {
                        Me.Bounds_Current_X = Me.Bounds_QuarterScreen_Right - Me.MinimumWidth;
                        Me.Bounds_Current_Width = Me.MinimumWidth;
                    }

                    _TryToUpdateLayout(UpdateLayoutEventType.Process);
                }
            }
        }

        // Label_BottomRight 的 MouseDown 事件的回调函数。
        private void Label_BottomRight_MouseDown(object sender, MouseEventArgs e)
        {
            Me.CaptionBar.BringToFront();
            Me.Client.BringToFront();
            Me.Client.Focus();

            if (Me.FormStyle == FormStyle.Sizable)
            {
                if (e.Button == MouseButtons.Left)
                {
                    _CursorPositionOfMe = Geometry.GetCursorPositionOfControl(Panel_FormBounds);

                    if (Me.FormState == FormState.Normal || Me.FormState == FormState.HighAsScreen)
                    {
                        Me.Bounds_Normal_X = Me.Bounds_Current_X;
                        Me.Bounds_Normal_Width = Me.Bounds_Current_Width;
                    }

                    _MeIsResizing = true;

                    Cursor.Clip = FormManager.PrimaryScreenClient;
                }
            }
        }

        // Label_BottomRight 的 MouseUp 事件的回调函数。
        private void Label_BottomRight_MouseUp(object sender, MouseEventArgs e)
        {
            Me.CaptionBar.BringToFront();
            Me.Client.BringToFront();
            Me.Client.Focus();

            if (Me.FormStyle == FormStyle.Sizable)
            {
                if (_MeIsResizing)
                {
                    if (Me.FormState == FormState.Normal)
                    {
                        if (Cursor.Position.Y >= FormManager.PrimaryScreenClient.Bottom - 1)
                        {
                            _CancelUpdateLayout();

                            Me.Bounds_Normal_X = Me.Bounds_Current_X;
                            Me.Bounds_Normal_Width = Me.Bounds_Current_Width;

                            if (!Me.HighAsScreen())
                            {
                                Me.UpdateLayout(UpdateLayoutEventType.SizeChanged);
                            }
                        }
                        else
                        {
                            _CancelUpdateLayout();

                            Me.Bounds_Normal = Me.Bounds_Current;

                            Me.UpdateLayout(UpdateLayoutEventType.SizeChanged);
                        }
                    }
                    else if (Me.FormState == FormState.QuarterScreen)
                    {
                        _CancelUpdateLayout();

                        Me.Bounds_QuarterScreen = Me.Bounds_Current;

                        Me.UpdateLayout(UpdateLayoutEventType.SizeChanged);
                    }
                }

                _MeIsResizing = false;

                Cursor.Clip = FormManager.PrimaryScreenBounds;
            }
        }

        // Label_BottomRight 的 MouseMove 事件的回调函数。
        private void Label_BottomRight_MouseMove(object sender, MouseEventArgs e)
        {
            if (_MeIsResizing)
            {
                if (Me.FormState == FormState.HighAsScreen)
                {
                    _CursorPositionOfMe.Y = Me.Bounds_Normal_Height - (Me.Bounds_Current_Height - _CursorPositionOfMe.Y);

                    Me.ReturnFromHighAsScreen();
                }

                Point CursorPosition = Cursor.Position;
                Rectangle CurScrBounds = FormManager.PrimaryScreenBounds;

                if (Me.FormState == FormState.Normal)
                {
                    if (Me.Bounds_Normal_Height + CursorPosition.Y - Me.Bounds_Normal_Y - _CursorPositionOfMe.Y >= Me.MinimumHeight)
                    {
                        Me.Bounds_Current_Y = Me.Bounds_Normal_Y;
                        Me.Bounds_Current_Height = Math.Min(Math.Min(CurScrBounds.Height, Me.MaximumHeight), Me.Bounds_Normal_Height - Me.Bounds_Normal_Y + CursorPosition.Y - _CursorPositionOfMe.Y);
                    }
                    else
                    {
                        Me.Bounds_Current_Y = Me.Bounds_Normal_Y;
                        Me.Bounds_Current_Height = Me.MinimumHeight;
                    }

                    if (Me.Bounds_Normal_Width + CursorPosition.X - Me.Bounds_Normal_X - _CursorPositionOfMe.X >= Me.MinimumWidth)
                    {
                        Me.Bounds_Current_X = Me.Bounds_Normal_X;
                        Me.Bounds_Current_Width = Math.Min(Math.Min(CurScrBounds.Width, Me.MaximumWidth), Me.Bounds_Normal_Width - Me.Bounds_Normal_X + CursorPosition.X - _CursorPositionOfMe.X);
                    }
                    else
                    {
                        Me.Bounds_Current_X = Me.Bounds_Normal_X;
                        Me.Bounds_Current_Width = Me.MinimumWidth;
                    }

                    _TryToUpdateLayout(UpdateLayoutEventType.Resize);
                }
                else if (Me.FormState == FormState.QuarterScreen)
                {
                    if (Me.Bounds_QuarterScreen_Height + CursorPosition.Y - Me.Bounds_QuarterScreen_Y - _CursorPositionOfMe.Y <= Math.Min(CurScrBounds.Height, Me.MaximumHeight) && Me.Bounds_QuarterScreen_Height + CursorPosition.Y - Me.Bounds_QuarterScreen_Y - _CursorPositionOfMe.Y >= Me.MinimumHeight)
                    {
                        Me.Bounds_Current_Y = Me.Bounds_QuarterScreen_Y;
                        Me.Bounds_Current_Height = Math.Min(Math.Min(CurScrBounds.Height, Me.MaximumHeight), Me.Bounds_QuarterScreen_Height - Me.Bounds_QuarterScreen_Y + CursorPosition.Y - _CursorPositionOfMe.Y);
                    }
                    else
                    {
                        Me.Bounds_Current_Y = Me.Bounds_QuarterScreen_Y;
                        Me.Bounds_Current_Height = Me.MinimumHeight;
                    }

                    if (Me.Bounds_QuarterScreen_Width + CursorPosition.X - Me.Bounds_QuarterScreen_X - _CursorPositionOfMe.X >= Me.MinimumWidth)
                    {
                        Me.Bounds_Current_X = Me.Bounds_QuarterScreen_X;
                        Me.Bounds_Current_Width = Math.Min(Math.Min(CurScrBounds.Width, Me.MaximumWidth), Me.Bounds_QuarterScreen_Width - Me.Bounds_QuarterScreen_X + CursorPosition.X - _CursorPositionOfMe.X);
                    }
                    else
                    {
                        Me.Bounds_Current_X = Me.Bounds_QuarterScreen_X;
                        Me.Bounds_Current_Width = Me.MinimumWidth;
                    }

                    _TryToUpdateLayout(UpdateLayoutEventType.Resize);
                }
            }
        }

        //

        // BackgroundWorker_UpdateLayoutDelay 的 DoWork 事件的回调函数。
        private void BackgroundWorker_UpdateLayoutDelay_DoWork(object sender, DoWorkEventArgs e)
        {
            while ((DateTime.UtcNow - _LastUpdateLayout).TotalMilliseconds < 16)
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

                _LastUpdateLayout = DateTime.UtcNow;
            }
        }

        #endregion

        #region 构造函数

        // 使用 FormManager 对象初始化 Resizer 的新实例。
        public Resizer(FormManager formManager)
        {
            InitializeComponent();

            //

            Me = formManager;
        }

        #endregion

        #region 方法

        // 在 FormStyleChanged 事件发生时发生。
        public void OnFormStyleChanged()
        {
            if (Me.FormStyle == FormStyle.Sizable)
            {
                Label_Top.Cursor = Label_Bottom.Cursor = Cursors.SizeNS;
                Label_Left.Cursor = Label_Right.Cursor = Cursors.SizeWE;
                Label_TopLeft.Cursor = Label_BottomRight.Cursor = Cursors.SizeNWSE;
                Label_TopRight.Cursor = Label_BottomLeft.Cursor = Cursors.SizeNESW;
            }
            else
            {
                Label_Top.Cursor = Label_Bottom.Cursor = Label_Left.Cursor = Label_Right.Cursor = Label_TopLeft.Cursor = Label_TopRight.Cursor = Label_BottomLeft.Cursor = Label_BottomRight.Cursor = Cursors.Default;
            }
        }

        // 在 FormStateChanged 事件发生时发生。
        public void OnFormStateChanged()
        {
            this.Visible = Me.FormState != FormState.Maximized && Me.FormState != FormState.FullScreen;

            if (this.Visible)
            {
                Me.CaptionBar.BringToFront();
                Me.Client.BringToFront();
                Me.Client.Focus();
            }
        }

        // 在 ThemeChanged 事件发生时发生。
        public void OnThemeChanged() => _RepaintResizerBitmap();

        #endregion
    }
}