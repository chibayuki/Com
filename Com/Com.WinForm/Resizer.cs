/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2018 chibayuki@foxmail.com

Com.WinForm.Resizer
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
    internal partial class Resizer : Form // 窗口大小调节器。
    {
        #region 私有与内部成员

        private FormManager Me; // 窗口管理器。

        //

        private Point _CursorPositionOfMe = new Point(); // 鼠标指针在窗口的坐标。

        private bool _MeIsResizing = false; // 是否正在改变窗口大小。

        //

        private Bitmap _ResizerBitmap; // 窗口大小调节器绘图。

        private void _UpdateResizerBitmap() // 更新窗口大小调节器绘图。
        {
            if (_ResizerBitmap != null)
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
                                int N = (Pn.Width >= 3F ? 1 : (Pn.Width == 2F ? 2 : (Pn.Width == 1 ? 3 : 0)));

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

        private void _RepaintResizerBitmap() // 更新并重绘窗口大小调节器绘图。
        {
            _UpdateResizerBitmap();

            if (_ResizerBitmap != null)
            {
                Painting2D.PaintImageOnTransparentForm(this, _ResizerBitmap, Me.Opacity);
            }
        }

        //

        private DateTime _LastUpdateLayout = new DateTime(); // 上次更新窗口大小调节器布局的日期时间。

        private void _TryToUpdateLayout() // 尝试更新窗口大小调节器布局。
        {
            if (!BackgroundWorker_UpdateLayoutDelay.IsBusy)
            {
                BackgroundWorker_UpdateLayoutDelay.RunWorkerAsync();
            }
        }

        #endregion

        #region 回调函数

        private void Resizer_Load(object sender, EventArgs e) // Resizer 的 Load 事件的回调函数。
        {
            Label_Top.BackColor = Label_Bottom.BackColor = Label_Left.BackColor = Label_Right.BackColor = Label_TopLeft.BackColor = Label_TopRight.BackColor = Label_BottomLeft.BackColor = Label_BottomRight.BackColor = Color.Transparent;

            //

            OnFormStyleChanged();

            //

            Resizer_SizeChanged(this, EventArgs.Empty);
        }

        private void Resizer_SizeChanged(object sender, EventArgs e) // Resizer 的 SizeChanged 事件的回调函数。
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
            Label_TopLeft.Size = Label_TopRight.Size = Label_BottomLeft.Size = Label_BottomRight.Size = new Size(4 * Me.ResizerSize, 4 * Me.ResizerSize);

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

        private void Panel_Border_Paint(object sender, PaintEventArgs e) // Panel_Border 的 Paint 事件的回调函数。
        {
            _RepaintResizerBitmap();
        }

        //

        private void Label_Top_MouseDoubleClick(object sender, MouseEventArgs e) // Label_Top 的 MouseDoubleClick 事件的回调函数。
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

        private void Label_Top_MouseDown(object sender, MouseEventArgs e) // Label_Top 的 MouseDown 事件的回调函数。
        {
            Me.CaptionBar.BringToFront();
            Me.Client.BringToFront();
            Me.Client.Focus();

            if (e.Button == MouseButtons.Left)
            {
                _CursorPositionOfMe = Geometry.GetCursorPositionOfControl(Panel_FormBounds);

                _MeIsResizing = true;

                Cursor.Clip = FormManager.PrimaryScreenClient;
            }
        }

        private void Label_Top_MouseUp(object sender, MouseEventArgs e) // Label_Top 的 MouseUp 事件的回调函数。
        {
            Me.CaptionBar.BringToFront();
            Me.Client.BringToFront();
            Me.Client.Focus();

            if (e.Button == MouseButtons.Left)
            {
                if (Me.FormState == FormState.Normal)
                {
                    if (FormManager.CursorPosition.Y <= FormManager.PrimaryScreenClient.Y)
                    {
                        if (!Me.HighAsScreen())
                        {
                            Me.UpdateLayout(UpdateLayoutEventType.Result);
                        }
                    }
                    else
                    {
                        Me.Bounds_Normal = Me.Bounds_Current;

                        Me.UpdateLayout(UpdateLayoutEventType.Result);
                    }
                }
                else if (Me.FormState == FormState.QuarterScreen)
                {
                    Me.Bounds_QuarterScreen_Y = Me.Bounds_Current_Y;
                    Me.Bounds_QuarterScreen_Height = Me.Bounds_Current_Height;

                    Me.UpdateLayout(UpdateLayoutEventType.Result);
                }
            }

            _MeIsResizing = false;

            Cursor.Clip = FormManager.PrimaryScreenBounds;
        }

        private void Label_Top_MouseMove(object sender, MouseEventArgs e) // Label_Top 的 MouseMove 事件的回调函数。
        {
            if (_MeIsResizing)
            {
                if (Me.FormState == FormState.HighAsScreen)
                {
                    Me.ReturnFromHighAsScreen();
                }

                if (Me.FormState == FormState.Normal)
                {
                    if (Me.Bounds_Normal_Bottom - (FormManager.CursorPosition.Y - _CursorPositionOfMe.Y) >= Me.MinimumBoundsHeight)
                    {
                        if (Me.Bounds_Normal_Bottom - (FormManager.CursorPosition.Y - _CursorPositionOfMe.Y) <= Math.Min(FormManager.PrimaryScreenBounds.Height, Me.MaximumBoundsHeight))
                        {
                            Me.Bounds_Current_Y = FormManager.CursorPosition.Y - _CursorPositionOfMe.Y;
                            Me.Bounds_Current_Height = Me.Bounds_Normal_Bottom - Me.Bounds_Current_Y;
                        }
                        else
                        {
                            Me.Bounds_Current_Y = Me.Bounds_Normal_Bottom - Math.Min(FormManager.PrimaryScreenBounds.Height, Me.MaximumBoundsHeight);
                            Me.Bounds_Current_Height = Math.Min(FormManager.PrimaryScreenBounds.Height, Me.MaximumBoundsHeight);
                        }
                    }
                    else
                    {
                        Me.Bounds_Current_Y = Me.Bounds_Normal_Bottom - Me.MinimumBoundsHeight;
                        Me.Bounds_Current_Height = Me.MinimumBoundsHeight;
                    }

                    _TryToUpdateLayout();
                }
                else if (Me.FormState == FormState.QuarterScreen)
                {
                    if (Me.Bounds_QuarterScreen_Bottom - (FormManager.CursorPosition.Y - _CursorPositionOfMe.Y) >= Me.MinimumBoundsHeight)
                    {
                        if (Me.Bounds_QuarterScreen_Bottom - (FormManager.CursorPosition.Y - _CursorPositionOfMe.Y) <= Math.Min(FormManager.PrimaryScreenBounds.Height, Me.MaximumBoundsHeight))
                        {
                            Me.Bounds_Current_Y = FormManager.CursorPosition.Y - _CursorPositionOfMe.Y;
                            Me.Bounds_Current_Height = Me.Bounds_QuarterScreen_Bottom - Me.Bounds_Current_Y;
                        }
                        else
                        {
                            Me.Bounds_Current_Y = Me.Bounds_QuarterScreen_Bottom - Math.Min(FormManager.PrimaryScreenBounds.Height, Me.MaximumBoundsHeight);
                            Me.Bounds_Current_Height = Math.Min(FormManager.PrimaryScreenBounds.Height, Me.MaximumBoundsHeight);
                        }
                    }
                    else
                    {
                        Me.Bounds_Current_Y = Me.Bounds_QuarterScreen_Bottom - Me.MinimumBoundsHeight;
                        Me.Bounds_Current_Height = Me.MinimumBoundsHeight;
                    }

                    _TryToUpdateLayout();
                }
            }
        }

        private void Label_Bottom_MouseDoubleClick(object sender, MouseEventArgs e) // Label_Bottom 的 MouseDoubleClick 事件的回调函数。
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

        private void Label_Bottom_MouseDown(object sender, MouseEventArgs e) // Label_Bottom 的 MouseDown 事件的回调函数。
        {
            Me.CaptionBar.BringToFront();
            Me.Client.BringToFront();
            Me.Client.Focus();

            if (e.Button == MouseButtons.Left)
            {
                _CursorPositionOfMe = Geometry.GetCursorPositionOfControl(Panel_FormBounds);

                _MeIsResizing = true;

                Cursor.Clip = FormManager.PrimaryScreenClient;
            }
        }

        private void Label_Bottom_MouseUp(object sender, MouseEventArgs e) // Label_Bottom 的 MouseUp 事件的回调函数。
        {
            Me.CaptionBar.BringToFront();
            Me.Client.BringToFront();
            Me.Client.Focus();

            if (e.Button == MouseButtons.Left)
            {
                if (Me.FormState == FormState.Normal)
                {
                    if (FormManager.CursorPosition.Y >= FormManager.PrimaryScreenClient.Bottom - 1)
                    {
                        if (!Me.HighAsScreen())
                        {
                            Me.UpdateLayout(UpdateLayoutEventType.Result);
                        }
                    }
                    else
                    {
                        Me.Bounds_Normal = Me.Bounds_Current;

                        Me.UpdateLayout(UpdateLayoutEventType.Result);
                    }
                }
                else if (Me.FormState == FormState.QuarterScreen)
                {
                    Me.Bounds_QuarterScreen_Y = Me.Bounds_Current_Y;
                    Me.Bounds_QuarterScreen_Height = Me.Bounds_Current_Height;

                    Me.UpdateLayout(UpdateLayoutEventType.Result);
                }
            }

            _MeIsResizing = false;

            Cursor.Clip = FormManager.PrimaryScreenBounds;
        }

        private void Label_Bottom_MouseMove(object sender, MouseEventArgs e) // Label_Bottom 的 MouseMove 事件的回调函数。
        {
            if (_MeIsResizing)
            {
                if (Me.FormState == FormState.HighAsScreen)
                {
                    _CursorPositionOfMe.Y = Me.Bounds_Normal_Height - (Me.Bounds_Current_Height - _CursorPositionOfMe.Y);

                    Me.ReturnFromHighAsScreen();
                }

                if (Me.FormState == FormState.Normal)
                {
                    if (Me.Bounds_Normal_Height + FormManager.CursorPosition.Y - Me.Bounds_Normal_Y - _CursorPositionOfMe.Y >= Me.MinimumBoundsHeight)
                    {
                        Me.Bounds_Current_Y = Me.Bounds_Normal_Y;
                        Me.Bounds_Current_Height = Math.Min(Math.Min(FormManager.PrimaryScreenBounds.Height, Me.MaximumBoundsHeight), Me.Bounds_Normal_Height - Me.Bounds_Normal_Y + FormManager.CursorPosition.Y - _CursorPositionOfMe.Y);
                    }
                    else
                    {
                        Me.Bounds_Current_Y = Me.Bounds_Normal_Y;
                        Me.Bounds_Current_Height = Me.MinimumBoundsHeight;
                    }

                    _TryToUpdateLayout();
                }
                else if (Me.FormState == FormState.QuarterScreen)
                {
                    if (Me.Bounds_QuarterScreen_Height + FormManager.CursorPosition.Y - Me.Bounds_QuarterScreen_Y - _CursorPositionOfMe.Y <= Math.Min(FormManager.PrimaryScreenBounds.Height, Me.MaximumBoundsHeight) && Me.Bounds_QuarterScreen_Height + FormManager.CursorPosition.Y - Me.Bounds_QuarterScreen_Y - _CursorPositionOfMe.Y >= Me.MinimumBoundsHeight)
                    {
                        Me.Bounds_Current_Y = Me.Bounds_QuarterScreen_Y;
                        Me.Bounds_Current_Height = Math.Min(Math.Min(FormManager.PrimaryScreenBounds.Height, Me.MaximumBoundsHeight), Me.Bounds_QuarterScreen_Height - Me.Bounds_QuarterScreen_Y + FormManager.CursorPosition.Y - _CursorPositionOfMe.Y);
                    }
                    else
                    {
                        Me.Bounds_Current_Y = Me.Bounds_QuarterScreen_Y;
                        Me.Bounds_Current_Height = Me.MinimumBoundsHeight;
                    }

                    _TryToUpdateLayout();
                }
            }
        }

        private void Label_Left_MouseDown(object sender, MouseEventArgs e) // Label_Left 的 MouseDown 事件的回调函数。
        {
            Me.CaptionBar.BringToFront();
            Me.Client.BringToFront();
            Me.Client.Focus();

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

        private void Label_Left_MouseUp(object sender, MouseEventArgs e) // Label_Left 的 MouseUp 事件的回调函数。
        {
            Me.CaptionBar.BringToFront();
            Me.Client.BringToFront();
            Me.Client.Focus();

            if (e.Button == MouseButtons.Left)
            {
                if (Me.FormState == FormState.Normal || Me.FormState == FormState.HighAsScreen)
                {
                    Me.Bounds_Normal_X = Me.Bounds_Current_X;
                    Me.Bounds_Normal_Width = Me.Bounds_Current_Width;

                    Me.UpdateLayout(UpdateLayoutEventType.Result);
                }
                else if (Me.FormState == FormState.QuarterScreen)
                {
                    Me.Bounds_QuarterScreen_X = Me.Bounds_Current_X;
                    Me.Bounds_QuarterScreen_Width = Me.Bounds_Current_Width;

                    Me.UpdateLayout(UpdateLayoutEventType.Result);
                }
            }

            _MeIsResizing = false;

            Cursor.Clip = FormManager.PrimaryScreenBounds;
        }

        private void Label_Left_MouseMove(object sender, MouseEventArgs e) // Label_Left 的 MouseMove 事件的回调函数。
        {
            if (_MeIsResizing)
            {
                if (Me.FormState == FormState.Normal || Me.FormState == FormState.HighAsScreen)
                {
                    if (Me.Bounds_Normal_Right - (FormManager.CursorPosition.X - _CursorPositionOfMe.X) >= Me.MinimumBoundsWidth)
                    {
                        if (Me.Bounds_Normal_Right - (FormManager.CursorPosition.X - _CursorPositionOfMe.X) <= Math.Min(FormManager.PrimaryScreenBounds.Width, Me.MaximumBoundsWidth))
                        {
                            Me.Bounds_Current_X = FormManager.CursorPosition.X - _CursorPositionOfMe.X;
                            Me.Bounds_Current_Width = Me.Bounds_Normal_Right - Me.Bounds_Current_X;
                        }
                        else
                        {
                            Me.Bounds_Current_X = Me.Bounds_Normal_Right - Math.Min(FormManager.PrimaryScreenBounds.Width, Me.MaximumBoundsWidth);
                            Me.Bounds_Current_Width = Math.Min(FormManager.PrimaryScreenBounds.Width, Me.MaximumBoundsWidth);
                        }
                    }
                    else
                    {
                        Me.Bounds_Current_X = Me.Bounds_Normal_Right - Me.MinimumBoundsWidth;
                        Me.Bounds_Current_Width = Me.MinimumBoundsWidth;
                    }

                    _TryToUpdateLayout();
                }
                else if (Me.FormState == FormState.QuarterScreen)
                {
                    if (Me.Bounds_QuarterScreen_Right - (FormManager.CursorPosition.X - _CursorPositionOfMe.X) >= Me.MinimumBoundsWidth)
                    {
                        if (Me.Bounds_QuarterScreen_Right - (FormManager.CursorPosition.X - _CursorPositionOfMe.X) <= Math.Min(FormManager.PrimaryScreenBounds.Width, Me.MaximumBoundsWidth))
                        {
                            Me.Bounds_Current_X = FormManager.CursorPosition.X - _CursorPositionOfMe.X;
                            Me.Bounds_Current_Width = Me.Bounds_QuarterScreen_Right - Me.Bounds_Current_X;
                        }
                        else
                        {
                            Me.Bounds_Current_X = Me.Bounds_QuarterScreen_Right - Math.Min(FormManager.PrimaryScreenBounds.Width, Me.MaximumBoundsWidth);
                            Me.Bounds_Current_Width = Math.Min(FormManager.PrimaryScreenBounds.Width, Me.MaximumBoundsWidth);
                        }
                    }
                    else
                    {
                        Me.Bounds_Current_X = Me.Bounds_QuarterScreen_Right - Me.MinimumBoundsWidth;
                        Me.Bounds_Current_Width = Me.MinimumBoundsWidth;
                    }

                    _TryToUpdateLayout();
                }
            }
        }

        private void Label_Right_MouseDown(object sender, MouseEventArgs e) // Label_Right 的 MouseDown 事件的回调函数。
        {
            Me.CaptionBar.BringToFront();
            Me.Client.BringToFront();
            Me.Client.Focus();

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

        private void Label_Right_MouseUp(object sender, MouseEventArgs e) // Label_Right 的 MouseUp 事件的回调函数。
        {
            Me.CaptionBar.BringToFront();
            Me.Client.BringToFront();
            Me.Client.Focus();

            if (e.Button == MouseButtons.Left)
            {
                if (Me.FormState == FormState.Normal || Me.FormState == FormState.HighAsScreen)
                {
                    Me.Bounds_Normal_Width = Me.Bounds_Current_Width;

                    Me.UpdateLayout(UpdateLayoutEventType.Result);
                }
                else if (Me.FormState == FormState.QuarterScreen)
                {
                    Me.Bounds_QuarterScreen_Width = Me.Bounds_Current_Width;

                    Me.UpdateLayout(UpdateLayoutEventType.Result);
                }
            }

            _MeIsResizing = false;

            Cursor.Clip = FormManager.PrimaryScreenBounds;
        }

        private void Label_Right_MouseMove(object sender, MouseEventArgs e) // Label_Right 的 MouseMove 事件的回调函数。
        {
            if (_MeIsResizing)
            {
                if (Me.FormState == FormState.Normal || Me.FormState == FormState.HighAsScreen)
                {
                    if (Me.Bounds_Normal_Width + FormManager.CursorPosition.X - Me.Bounds_Normal_X - _CursorPositionOfMe.X >= Me.MinimumBoundsWidth)
                    {
                        Me.Bounds_Current_X = Me.Bounds_Normal_X;
                        Me.Bounds_Current_Width = Math.Min(Math.Min(FormManager.PrimaryScreenBounds.Width, Me.MaximumBoundsWidth), Me.Bounds_Normal_Width - Me.Bounds_Normal_X + FormManager.CursorPosition.X - _CursorPositionOfMe.X);
                    }
                    else
                    {
                        Me.Bounds_Current_X = Me.Bounds_Normal_X;
                        Me.Bounds_Current_Width = Me.MinimumBoundsWidth;
                    }

                    _TryToUpdateLayout();
                }
                else if (Me.FormState == FormState.QuarterScreen)
                {
                    if (Me.Bounds_QuarterScreen_Width + FormManager.CursorPosition.X - Me.Bounds_QuarterScreen_X - _CursorPositionOfMe.X >= Me.MinimumBoundsWidth)
                    {
                        Me.Bounds_Current_X = Me.Bounds_QuarterScreen_X;
                        Me.Bounds_Current_Width = Math.Min(Math.Min(FormManager.PrimaryScreenBounds.Width, Me.MaximumBoundsWidth), Me.Bounds_QuarterScreen_Width - Me.Bounds_QuarterScreen_X + FormManager.CursorPosition.X - _CursorPositionOfMe.X);
                    }
                    else
                    {
                        Me.Bounds_Current_X = Me.Bounds_QuarterScreen_X;
                        Me.Bounds_Current_Width = Me.MinimumBoundsWidth;
                    }

                    _TryToUpdateLayout();
                }
            }
        }

        private void Label_TopLeft_MouseDown(object sender, MouseEventArgs e) // Label_TopLeft 的 MouseDown 事件的回调函数。
        {
            Me.CaptionBar.BringToFront();
            Me.Client.BringToFront();
            Me.Client.Focus();

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

        private void Label_TopLeft_MouseUp(object sender, MouseEventArgs e) // Label_TopLeft 的 MouseUp 事件的回调函数。
        {
            Me.CaptionBar.BringToFront();
            Me.Client.BringToFront();
            Me.Client.Focus();

            if (e.Button == MouseButtons.Left)
            {
                if (Me.FormState == FormState.Normal)
                {
                    if (FormManager.CursorPosition.Y <= FormManager.PrimaryScreenClient.Y)
                    {
                        Me.Bounds_Normal_X = Me.Bounds_Current_X;
                        Me.Bounds_Normal_Width = Me.Bounds_Current_Width;

                        if (!Me.HighAsScreen())
                        {
                            Me.UpdateLayout(UpdateLayoutEventType.Result);
                        }
                    }
                    else
                    {
                        Me.Bounds_Normal = Me.Bounds_Current;

                        Me.UpdateLayout(UpdateLayoutEventType.Result);
                    }
                }
                else if (Me.FormState == FormState.QuarterScreen)
                {
                    Me.Bounds_QuarterScreen = Me.Bounds_Current;

                    Me.UpdateLayout(UpdateLayoutEventType.Result);
                }
            }

            _MeIsResizing = false;

            Cursor.Clip = FormManager.PrimaryScreenBounds;
        }

        private void Label_TopLeft_MouseMove(object sender, MouseEventArgs e) // Label_TopLeft 的 MouseMove 事件的回调函数。
        {
            if (_MeIsResizing)
            {
                if (Me.FormState == FormState.HighAsScreen)
                {
                    Me.ReturnFromHighAsScreen();
                }

                if (Me.FormState == FormState.Normal)
                {
                    if (Me.Bounds_Normal_Bottom - (FormManager.CursorPosition.Y - _CursorPositionOfMe.Y) >= Me.MinimumBoundsHeight)
                    {
                        if (Me.Bounds_Normal_Bottom - (FormManager.CursorPosition.Y - _CursorPositionOfMe.Y) <= Math.Min(FormManager.PrimaryScreenBounds.Height, Me.MaximumBoundsHeight))
                        {
                            Me.Bounds_Current_Y = FormManager.CursorPosition.Y - _CursorPositionOfMe.Y;
                            Me.Bounds_Current_Height = Me.Bounds_Normal_Bottom - Me.Bounds_Current_Y;
                        }
                        else
                        {
                            Me.Bounds_Current_Y = Me.Bounds_Normal_Bottom - Math.Min(FormManager.PrimaryScreenBounds.Height, Me.MaximumBoundsHeight);
                            Me.Bounds_Current_Height = Math.Min(FormManager.PrimaryScreenBounds.Height, Me.MaximumBoundsHeight);
                        }
                    }
                    else
                    {
                        Me.Bounds_Current_Y = Me.Bounds_Normal_Bottom - Me.MinimumBoundsHeight;
                        Me.Bounds_Current_Height = Me.MinimumBoundsHeight;
                    }

                    if (Me.Bounds_Normal_Right - (FormManager.CursorPosition.X - _CursorPositionOfMe.X) >= Me.MinimumBoundsWidth)
                    {
                        if (Me.Bounds_Normal_Right - (FormManager.CursorPosition.X - _CursorPositionOfMe.X) <= Math.Min(FormManager.PrimaryScreenBounds.Width, Me.MaximumBoundsWidth))
                        {
                            Me.Bounds_Current_X = FormManager.CursorPosition.X - _CursorPositionOfMe.X;
                            Me.Bounds_Current_Width = Me.Bounds_Normal_Right - Me.Bounds_Current_X;
                        }
                        else
                        {
                            Me.Bounds_Current_X = Me.Bounds_Normal_Right - Math.Min(FormManager.PrimaryScreenBounds.Width, Me.MaximumBoundsWidth);
                            Me.Bounds_Current_Width = Math.Min(FormManager.PrimaryScreenBounds.Width, Me.MaximumBoundsWidth);
                        }
                    }
                    else
                    {
                        Me.Bounds_Current_X = Me.Bounds_Normal_Right - Me.MinimumBoundsWidth;
                        Me.Bounds_Current_Width = Me.MinimumBoundsWidth;
                    }

                    _TryToUpdateLayout();
                }
                else if (Me.FormState == FormState.QuarterScreen)
                {
                    if (Me.Bounds_QuarterScreen_Bottom - (FormManager.CursorPosition.Y - _CursorPositionOfMe.Y) >= Me.MinimumBoundsHeight)
                    {
                        if (Me.Bounds_QuarterScreen_Bottom - (FormManager.CursorPosition.Y - _CursorPositionOfMe.Y) <= Math.Min(FormManager.PrimaryScreenBounds.Height, Me.MaximumBoundsHeight))
                        {
                            Me.Bounds_Current_Y = FormManager.CursorPosition.Y - _CursorPositionOfMe.Y;
                            Me.Bounds_Current_Height = Me.Bounds_QuarterScreen_Bottom - Me.Bounds_Current_Y;
                        }
                        else
                        {
                            Me.Bounds_Current_Y = Me.Bounds_QuarterScreen_Bottom - Math.Min(FormManager.PrimaryScreenBounds.Height, Me.MaximumBoundsHeight);
                            Me.Bounds_Current_Height = Math.Min(FormManager.PrimaryScreenBounds.Height, Me.MaximumBoundsHeight);
                        }
                    }
                    else
                    {
                        Me.Bounds_Current_Y = Me.Bounds_QuarterScreen_Bottom - Me.MinimumBoundsHeight;
                        Me.Bounds_Current_Height = Me.MinimumBoundsHeight;
                    }

                    if (Me.Bounds_QuarterScreen_Right - (FormManager.CursorPosition.X - _CursorPositionOfMe.X) >= Me.MinimumBoundsWidth)
                    {
                        if (Me.Bounds_QuarterScreen_Right - (FormManager.CursorPosition.X - _CursorPositionOfMe.X) <= Math.Min(FormManager.PrimaryScreenBounds.Width, Me.MaximumBoundsWidth))
                        {
                            Me.Bounds_Current_X = FormManager.CursorPosition.X - _CursorPositionOfMe.X;
                            Me.Bounds_Current_Width = Me.Bounds_QuarterScreen_Right - Me.Bounds_Current_X;
                        }
                        else
                        {
                            Me.Bounds_Current_X = Me.Bounds_QuarterScreen_Right - Math.Min(FormManager.PrimaryScreenBounds.Width, Me.MaximumBoundsWidth);
                            Me.Bounds_Current_Width = Math.Min(FormManager.PrimaryScreenBounds.Width, Me.MaximumBoundsWidth);
                        }
                    }
                    else
                    {
                        Me.Bounds_Current_X = Me.Bounds_QuarterScreen_Right - Me.MinimumBoundsWidth;
                        Me.Bounds_Current_Width = Me.MinimumBoundsWidth;
                    }

                    _TryToUpdateLayout();
                }
            }
        }

        private void Label_TopRight_MouseDown(object sender, MouseEventArgs e) // Label_TopRight 的 MouseDown 事件的回调函数。
        {
            Me.CaptionBar.BringToFront();
            Me.Client.BringToFront();
            Me.Client.Focus();

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

        private void Label_TopRight_MouseUp(object sender, MouseEventArgs e) // Label_TopRight 的 MouseUp 事件的回调函数。
        {
            Me.CaptionBar.BringToFront();
            Me.Client.BringToFront();
            Me.Client.Focus();

            if (e.Button == MouseButtons.Left)
            {
                if (Me.FormState == FormState.Normal)
                {
                    if (FormManager.CursorPosition.Y <= FormManager.PrimaryScreenClient.Y)
                    {
                        Me.Bounds_Normal_X = Me.Bounds_Current_X;
                        Me.Bounds_Normal_Width = Me.Bounds_Current_Width;

                        if (!Me.HighAsScreen())
                        {
                            Me.UpdateLayout(UpdateLayoutEventType.Result);
                        }
                    }
                    else
                    {
                        Me.Bounds_Normal = Me.Bounds_Current;

                        Me.UpdateLayout(UpdateLayoutEventType.Result);
                    }
                }
                else if (Me.FormState == FormState.QuarterScreen)
                {
                    Me.Bounds_QuarterScreen = Me.Bounds_Current;

                    Me.UpdateLayout(UpdateLayoutEventType.Result);
                }
            }

            _MeIsResizing = false;

            Cursor.Clip = FormManager.PrimaryScreenBounds;
        }

        private void Label_TopRight_MouseMove(object sender, MouseEventArgs e) // Label_TopRight 的 MouseMove 事件的回调函数。
        {
            if (_MeIsResizing)
            {
                if (Me.FormState == FormState.HighAsScreen)
                {
                    Me.ReturnFromHighAsScreen();
                }

                if (Me.FormState == FormState.Normal)
                {
                    if (Me.Bounds_Normal_Bottom - (FormManager.CursorPosition.Y - _CursorPositionOfMe.Y) >= Me.MinimumBoundsHeight)
                    {
                        if (Me.Bounds_Normal_Bottom - (FormManager.CursorPosition.Y - _CursorPositionOfMe.Y) <= Math.Min(FormManager.PrimaryScreenBounds.Height, Me.MaximumBoundsHeight))
                        {
                            Me.Bounds_Current_Y = FormManager.CursorPosition.Y - _CursorPositionOfMe.Y;
                            Me.Bounds_Current_Height = Me.Bounds_Normal_Bottom - Me.Bounds_Current_Y;
                        }
                        else
                        {
                            Me.Bounds_Current_Y = Me.Bounds_Normal_Bottom - Math.Min(FormManager.PrimaryScreenBounds.Height, Me.MaximumBoundsHeight);
                            Me.Bounds_Current_Height = Math.Min(FormManager.PrimaryScreenBounds.Height, Me.MaximumBoundsHeight);
                        }
                    }
                    else
                    {
                        Me.Bounds_Current_Y = Me.Bounds_Normal_Bottom - Me.MinimumBoundsHeight;
                        Me.Bounds_Current_Height = Me.MinimumBoundsHeight;
                    }

                    if (Me.Bounds_Normal_Width + FormManager.CursorPosition.X - Me.Bounds_Normal_X - _CursorPositionOfMe.X >= Me.MinimumBoundsWidth)
                    {
                        Me.Bounds_Current_X = Me.Bounds_Normal_X;
                        Me.Bounds_Current_Width = Math.Min(Math.Min(FormManager.PrimaryScreenBounds.Width, Me.MaximumBoundsWidth), Me.Bounds_Normal_Width - Me.Bounds_Normal_X + FormManager.CursorPosition.X - _CursorPositionOfMe.X);
                    }
                    else
                    {
                        Me.Bounds_Current_X = Me.Bounds_Normal_X;
                        Me.Bounds_Current_Width = Me.MinimumBoundsWidth;
                    }

                    _TryToUpdateLayout();
                }
                else if (Me.FormState == FormState.QuarterScreen)
                {
                    if (Me.Bounds_QuarterScreen_Bottom - (FormManager.CursorPosition.Y - _CursorPositionOfMe.Y) >= Me.MinimumBoundsHeight)
                    {
                        if (Me.Bounds_QuarterScreen_Bottom - (FormManager.CursorPosition.Y - _CursorPositionOfMe.Y) <= Math.Min(FormManager.PrimaryScreenBounds.Height, Me.MaximumBoundsHeight))
                        {
                            Me.Bounds_Current_Y = FormManager.CursorPosition.Y - _CursorPositionOfMe.Y;
                            Me.Bounds_Current_Height = Me.Bounds_QuarterScreen_Bottom - Me.Bounds_Current_Y;
                        }
                        else
                        {
                            Me.Bounds_Current_Y = Me.Bounds_QuarterScreen_Bottom - Math.Min(FormManager.PrimaryScreenBounds.Height, Me.MaximumBoundsHeight);
                            Me.Bounds_Current_Height = Math.Min(FormManager.PrimaryScreenBounds.Height, Me.MaximumBoundsHeight);
                        }
                    }
                    else
                    {
                        Me.Bounds_Current_Y = Me.Bounds_QuarterScreen_Bottom - Me.MinimumBoundsHeight;
                        Me.Bounds_Current_Height = Me.MinimumBoundsHeight;
                    }

                    if (Me.Bounds_QuarterScreen_Width + FormManager.CursorPosition.X - Me.Bounds_QuarterScreen_X - _CursorPositionOfMe.X >= Me.MinimumBoundsWidth)
                    {
                        Me.Bounds_Current_X = Me.Bounds_QuarterScreen_X;
                        Me.Bounds_Current_Width = Math.Min(Math.Min(FormManager.PrimaryScreenBounds.Width, Me.MaximumBoundsWidth), Me.Bounds_QuarterScreen_Width - Me.Bounds_QuarterScreen_X + FormManager.CursorPosition.X - _CursorPositionOfMe.X);
                    }
                    else
                    {
                        Me.Bounds_Current_X = Me.Bounds_QuarterScreen_X;
                        Me.Bounds_Current_Width = Me.MinimumBoundsWidth;
                    }

                    _TryToUpdateLayout();
                }
            }
        }

        private void Label_BottomLeft_MouseDown(object sender, MouseEventArgs e) // Label_BottomLeft 的 MouseDown 事件的回调函数。
        {
            Me.CaptionBar.BringToFront();
            Me.Client.BringToFront();
            Me.Client.Focus();

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

        private void Label_BottomLeft_MouseUp(object sender, MouseEventArgs e) // Label_BottomLeft 的 MouseUp 事件的回调函数。
        {
            Me.CaptionBar.BringToFront();
            Me.Client.BringToFront();
            Me.Client.Focus();

            if (e.Button == MouseButtons.Left)
            {
                if (Me.FormState == FormState.Normal)
                {
                    if (FormManager.CursorPosition.Y >= FormManager.PrimaryScreenClient.Bottom - 1)
                    {
                        Me.Bounds_Normal_X = Me.Bounds_Current_X;
                        Me.Bounds_Normal_Width = Me.Bounds_Current_Width;

                        if (!Me.HighAsScreen())
                        {
                            Me.UpdateLayout(UpdateLayoutEventType.Result);
                        }
                    }
                    else
                    {
                        Me.Bounds_Normal = Me.Bounds_Current;

                        Me.UpdateLayout(UpdateLayoutEventType.Result);
                    }
                }
                else if (Me.FormState == FormState.QuarterScreen)
                {
                    Me.Bounds_QuarterScreen = Me.Bounds_Current;

                    Me.UpdateLayout(UpdateLayoutEventType.Result);
                }
            }

            _MeIsResizing = false;

            Cursor.Clip = FormManager.PrimaryScreenBounds;
        }

        private void Label_BottomLeft_MouseMove(object sender, MouseEventArgs e) // Label_BottomLeft 的 MouseMove 事件的回调函数。
        {
            if (_MeIsResizing)
            {
                if (Me.FormState == FormState.HighAsScreen)
                {
                    _CursorPositionOfMe.Y = Me.Bounds_Normal_Height - (Me.Bounds_Current_Height - _CursorPositionOfMe.Y);

                    Me.ReturnFromHighAsScreen();
                }

                if (Me.FormState == FormState.Normal)
                {
                    if (Me.Bounds_Normal_Height + FormManager.CursorPosition.Y - Me.Bounds_Normal_Y - _CursorPositionOfMe.Y >= Me.MinimumBoundsHeight)
                    {
                        Me.Bounds_Current_Y = Me.Bounds_Normal_Y;
                        Me.Bounds_Current_Height = Math.Min(Math.Min(FormManager.PrimaryScreenBounds.Height, Me.MaximumBoundsHeight), Me.Bounds_Normal_Height - Me.Bounds_Normal_Y + FormManager.CursorPosition.Y - _CursorPositionOfMe.Y);
                    }
                    else
                    {
                        Me.Bounds_Current_Y = Me.Bounds_Normal_Y;
                        Me.Bounds_Current_Height = Me.MinimumBoundsHeight;
                    }

                    if (Me.Bounds_Normal_Right - (FormManager.CursorPosition.X - _CursorPositionOfMe.X) >= Me.MinimumBoundsWidth)
                    {
                        if (Me.Bounds_Normal_Right - (FormManager.CursorPosition.X - _CursorPositionOfMe.X) <= Math.Min(FormManager.PrimaryScreenBounds.Width, Me.MaximumBoundsWidth))
                        {
                            Me.Bounds_Current_X = FormManager.CursorPosition.X - _CursorPositionOfMe.X;
                            Me.Bounds_Current_Width = Me.Bounds_Normal_Right - Me.Bounds_Current_X;
                        }
                        else
                        {
                            Me.Bounds_Current_X = Me.Bounds_Normal_Right - Math.Min(FormManager.PrimaryScreenBounds.Width, Me.MaximumBoundsWidth);
                            Me.Bounds_Current_Width = Math.Min(FormManager.PrimaryScreenBounds.Width, Me.MaximumBoundsWidth);
                        }
                    }
                    else
                    {
                        Me.Bounds_Current_X = Me.Bounds_Normal_Right - Me.MinimumBoundsWidth;
                        Me.Bounds_Current_Width = Me.MinimumBoundsWidth;
                    }

                    _TryToUpdateLayout();
                }
                else if (Me.FormState == FormState.QuarterScreen)
                {
                    if (Me.Bounds_QuarterScreen_Height + FormManager.CursorPosition.Y - Me.Bounds_QuarterScreen_Y - _CursorPositionOfMe.Y <= Math.Min(FormManager.PrimaryScreenBounds.Height, Me.MaximumBoundsHeight) && Me.Bounds_QuarterScreen_Height + FormManager.CursorPosition.Y - Me.Bounds_QuarterScreen_Y - _CursorPositionOfMe.Y >= Me.MinimumBoundsHeight)
                    {
                        Me.Bounds_Current_Y = Me.Bounds_QuarterScreen_Y;
                        Me.Bounds_Current_Height = Math.Min(Math.Min(FormManager.PrimaryScreenBounds.Height, Me.MaximumBoundsHeight), Me.Bounds_QuarterScreen_Height - Me.Bounds_QuarterScreen_Y + FormManager.CursorPosition.Y - _CursorPositionOfMe.Y);
                    }
                    else
                    {
                        Me.Bounds_Current_Y = Me.Bounds_QuarterScreen_Y;
                        Me.Bounds_Current_Height = Me.MinimumBoundsHeight;
                    }

                    if (Me.Bounds_QuarterScreen_Right - (FormManager.CursorPosition.X - _CursorPositionOfMe.X) >= Me.MinimumBoundsWidth)
                    {
                        if (Me.Bounds_QuarterScreen_Right - (FormManager.CursorPosition.X - _CursorPositionOfMe.X) <= Math.Min(FormManager.PrimaryScreenBounds.Width, Me.MaximumBoundsWidth))
                        {
                            Me.Bounds_Current_X = FormManager.CursorPosition.X - _CursorPositionOfMe.X;
                            Me.Bounds_Current_Width = Me.Bounds_QuarterScreen_Right - Me.Bounds_Current_X;
                        }
                        else
                        {
                            Me.Bounds_Current_X = Me.Bounds_QuarterScreen_Right - Math.Min(FormManager.PrimaryScreenBounds.Width, Me.MaximumBoundsWidth);
                            Me.Bounds_Current_Width = Math.Min(FormManager.PrimaryScreenBounds.Width, Me.MaximumBoundsWidth);
                        }
                    }
                    else
                    {
                        Me.Bounds_Current_X = Me.Bounds_QuarterScreen_Right - Me.MinimumBoundsWidth;
                        Me.Bounds_Current_Width = Me.MinimumBoundsWidth;
                    }

                    _TryToUpdateLayout();
                }
            }
        }

        private void Label_BottomRight_MouseDown(object sender, MouseEventArgs e) // Label_BottomRight 的 MouseDown 事件的回调函数。
        {
            Me.CaptionBar.BringToFront();
            Me.Client.BringToFront();
            Me.Client.Focus();

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

        private void Label_BottomRight_MouseUp(object sender, MouseEventArgs e) // Label_BottomRight 的 MouseUp 事件的回调函数。
        {
            Me.CaptionBar.BringToFront();
            Me.Client.BringToFront();
            Me.Client.Focus();

            if (e.Button == MouseButtons.Left)
            {
                if (Me.FormState == FormState.Normal)
                {
                    if (FormManager.CursorPosition.Y >= FormManager.PrimaryScreenClient.Bottom - 1)
                    {
                        Me.Bounds_Normal_X = Me.Bounds_Current_X;
                        Me.Bounds_Normal_Width = Me.Bounds_Current_Width;

                        if (!Me.HighAsScreen())
                        {
                            Me.UpdateLayout(UpdateLayoutEventType.Result);
                        }
                    }
                    else
                    {
                        Me.Bounds_Normal = Me.Bounds_Current;

                        Me.UpdateLayout(UpdateLayoutEventType.Result);
                    }
                }
                else if (Me.FormState == FormState.QuarterScreen)
                {
                    Me.Bounds_QuarterScreen = Me.Bounds_Current;

                    Me.UpdateLayout(UpdateLayoutEventType.Result);
                }
            }

            _MeIsResizing = false;

            Cursor.Clip = FormManager.PrimaryScreenBounds;
        }

        private void Label_BottomRight_MouseMove(object sender, MouseEventArgs e) // Label_BottomRight 的 MouseMove 事件的回调函数。
        {
            if (_MeIsResizing)
            {
                if (Me.FormState == FormState.HighAsScreen)
                {
                    _CursorPositionOfMe.Y = Me.Bounds_Normal_Height - (Me.Bounds_Current_Height - _CursorPositionOfMe.Y);

                    Me.ReturnFromHighAsScreen();
                }

                if (Me.FormState == FormState.Normal)
                {
                    if (Me.Bounds_Normal_Height + FormManager.CursorPosition.Y - Me.Bounds_Normal_Y - _CursorPositionOfMe.Y >= Me.MinimumBoundsHeight)
                    {
                        Me.Bounds_Current_Y = Me.Bounds_Normal_Y;
                        Me.Bounds_Current_Height = Math.Min(Math.Min(FormManager.PrimaryScreenBounds.Height, Me.MaximumBoundsHeight), Me.Bounds_Normal_Height - Me.Bounds_Normal_Y + FormManager.CursorPosition.Y - _CursorPositionOfMe.Y);
                    }
                    else
                    {
                        Me.Bounds_Current_Y = Me.Bounds_Normal_Y;
                        Me.Bounds_Current_Height = Me.MinimumBoundsHeight;
                    }

                    if (Me.Bounds_Normal_Width + FormManager.CursorPosition.X - Me.Bounds_Normal_X - _CursorPositionOfMe.X >= Me.MinimumBoundsWidth)
                    {
                        Me.Bounds_Current_X = Me.Bounds_Normal_X;
                        Me.Bounds_Current_Width = Math.Min(Math.Min(FormManager.PrimaryScreenBounds.Width, Me.MaximumBoundsWidth), Me.Bounds_Normal_Width - Me.Bounds_Normal_X + FormManager.CursorPosition.X - _CursorPositionOfMe.X);
                    }
                    else
                    {
                        Me.Bounds_Current_X = Me.Bounds_Normal_X;
                        Me.Bounds_Current_Width = Me.MinimumBoundsWidth;
                    }

                    _TryToUpdateLayout();
                }
                else if (Me.FormState == FormState.QuarterScreen)
                {
                    if (Me.Bounds_QuarterScreen_Height + FormManager.CursorPosition.Y - Me.Bounds_QuarterScreen_Y - _CursorPositionOfMe.Y <= Math.Min(FormManager.PrimaryScreenBounds.Height, Me.MaximumBoundsHeight) && Me.Bounds_QuarterScreen_Height + FormManager.CursorPosition.Y - Me.Bounds_QuarterScreen_Y - _CursorPositionOfMe.Y >= Me.MinimumBoundsHeight)
                    {
                        Me.Bounds_Current_Y = Me.Bounds_QuarterScreen_Y;
                        Me.Bounds_Current_Height = Math.Min(Math.Min(FormManager.PrimaryScreenBounds.Height, Me.MaximumBoundsHeight), Me.Bounds_QuarterScreen_Height - Me.Bounds_QuarterScreen_Y + FormManager.CursorPosition.Y - _CursorPositionOfMe.Y);
                    }
                    else
                    {
                        Me.Bounds_Current_Y = Me.Bounds_QuarterScreen_Y;
                        Me.Bounds_Current_Height = Me.MinimumBoundsHeight;
                    }

                    if (Me.Bounds_QuarterScreen_Width + FormManager.CursorPosition.X - Me.Bounds_QuarterScreen_X - _CursorPositionOfMe.X >= Me.MinimumBoundsWidth)
                    {
                        Me.Bounds_Current_X = Me.Bounds_QuarterScreen_X;
                        Me.Bounds_Current_Width = Math.Min(Math.Min(FormManager.PrimaryScreenBounds.Width, Me.MaximumBoundsWidth), Me.Bounds_QuarterScreen_Width - Me.Bounds_QuarterScreen_X + FormManager.CursorPosition.X - _CursorPositionOfMe.X);
                    }
                    else
                    {
                        Me.Bounds_Current_X = Me.Bounds_QuarterScreen_X;
                        Me.Bounds_Current_Width = Me.MinimumBoundsWidth;
                    }

                    _TryToUpdateLayout();
                }
            }
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

        #endregion

        #region 构造函数

        public Resizer(FormManager formManager) // 使用 FormManager 对象初始化 Resizer 的新实例。
        {
            InitializeComponent();

            //

            Me = formManager;
        }

        #endregion

        #region 方法

        public void OnFormStyleChanged() // 在 FormStyleChanged 事件发生时发生。
        {
            Label_Top.Enabled = Label_Bottom.Enabled = Label_Left.Enabled = Label_Right.Enabled = Label_TopLeft.Enabled = Label_TopRight.Enabled = Label_BottomLeft.Enabled = Label_BottomRight.Enabled = (Me.FormStyle == FormStyle.Sizable && (Me.FormState != FormState.Maximized && Me.FormState != FormState.FullScreen));
        }

        public void OnFormStateChanged() // 在 FormStateChanged 事件发生时发生。
        {
            Label_Top.Enabled = Label_Bottom.Enabled = Label_Left.Enabled = Label_Right.Enabled = Label_TopLeft.Enabled = Label_TopRight.Enabled = Label_BottomLeft.Enabled = Label_BottomRight.Enabled = (Me.FormStyle == FormStyle.Sizable && (Me.FormState != FormState.Maximized && Me.FormState != FormState.FullScreen));

            //

            this.Visible = (Me.FormState != FormState.Maximized && Me.FormState != FormState.FullScreen);

            if (this.Visible)
            {
                Me.CaptionBar.BringToFront();
                Me.Client.BringToFront();
                Me.Client.Focus();
            }
        }

        public void OnThemeChanged() // 在 ThemeChanged 事件发生时发生。
        {
            _RepaintResizerBitmap();
        }

        #endregion
    }
}