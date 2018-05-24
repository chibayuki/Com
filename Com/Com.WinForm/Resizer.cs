/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2013-2018 chibayuki@foxmail.com

Com.WinForm.FormBorder
Version 18.5.23.0000

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
    internal partial class FormBorder : Form // 窗体边框。
    {
        #region 私有与内部成员

        private FormManager Me; // 窗体管理器。

        //

        private Point _CursorPositionOfMe = new Point(); // 鼠标指针在窗体的坐标。

        private bool _MeIsResizing = false; // 是否正在改变窗体大小。

        //

        private Bitmap _FormBorderBitmap; // 窗体边框绘图。

        private void _RefreshFormBorderBitmap() // 更新窗体边框绘图。
        {
            if (_FormBorderBitmap != null)
            {
                _FormBorderBitmap.Dispose();
            }

            _FormBorderBitmap = new Bitmap(Math.Max(1, Panel_Border.Width), Math.Max(1, Panel_Border.Height));

            if (Me.FormState != FormState.Maximized && Me.FormState != FormState.FullScreen)
            {
                using (Graphics CreateFormBorderBmp = Graphics.FromImage(_FormBorderBitmap))
                {
                    CreateFormBorderBmp.SmoothingMode = SmoothingMode.AntiAlias;

                    //

                    for (int i = 0; i <= Me.FormBorderSize; i++)
                    {
                        using (Pen Pn = new Pen(Color.FromArgb(24 * (i + 1) * (i + 1) / (Me.FormBorderSize + 1) / (Me.FormBorderSize + 1), Color.Black), Math.Max(1F, Math.Min(Me.FormBorderSize - i, 3F))))
                        {
                            using (GraphicsPath Path = Geometry.CreateRoundedRectanglePath(new Rectangle(new Point(i, i), new Size(_FormBorderBitmap.Width - (2 * i) - 1, _FormBorderBitmap.Height - (2 * i) - 1)), Me.FormBorderSize - i + 2))
                            {
                                for (int j = 0; j < (Pn.Width < 3F ? 2 : 1); j++)
                                {
                                    CreateFormBorderBmp.DrawPath(Pn, Path);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void _RepaintFormBorderBitmap() // 更新并重绘窗体边框绘图。
        {
            _RefreshFormBorderBitmap();

            if (_FormBorderBitmap != null)
            {
                Painting2D.PaintImageOnTransparentForm(this, _FormBorderBitmap, Me.Opacity);
            }
        }

        //

        private DateTime _LastUpdateLayout = new DateTime(); // 上次更新窗体边框布局的日期时间。

        private void _TryToUpdateLayout() // 尝试更新窗体边框布局。
        {
            if (!BackgroundWorker_UpdateLayoutDelay.IsBusy)
            {
                BackgroundWorker_UpdateLayoutDelay.RunWorkerAsync();
            }
        }

        #endregion

        #region 回调函数

        private void FormBorder_Load(object sender, EventArgs e) // FormBorder 的 Load 事件的回调函数。
        {
            Label_Top.BackColor = Label_Bottom.BackColor = Label_Left.BackColor = Label_Right.BackColor = Label_TopLeft.BackColor = Label_TopRight.BackColor = Label_BottomLeft.BackColor = Label_BottomRight.BackColor = Color.Transparent;

            //

            OnFormStyleChanged();

            //

            FormBorder_SizeChanged(this, EventArgs.Empty);
        }

        private void FormBorder_SizeChanged(object sender, EventArgs e) // FormBorder 的 SizeChanged 事件的回调函数。
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }

            //

            Panel_Border.Size = new Size(Me.Width + 2 * Me.FormBorderSize, Me.Height + 2 * Me.FormBorderSize);

            Panel_FormBounds.Bounds = new Rectangle(new Point(Me.FormBorderSize, Me.FormBorderSize), Me.Size);

            Label_Top.Size = new Size(Panel_Border.Width - Label_TopLeft.Width - Label_TopRight.Width, Me.FormBorderSize);
            Label_Bottom.Size = new Size(Panel_Border.Width - Label_BottomLeft.Width - Label_BottomRight.Width, Me.FormBorderSize);
            Label_Left.Size = new Size(Me.FormBorderSize, Panel_Border.Height - Label_TopLeft.Height - Label_BottomLeft.Height);
            Label_Right.Size = new Size(Me.FormBorderSize, Panel_Border.Height - Label_TopRight.Height - Label_BottomRight.Height);
            Label_TopLeft.Size = Label_TopRight.Size = Label_BottomLeft.Size = Label_BottomRight.Size = new Size(4 * Me.FormBorderSize, 4 * Me.FormBorderSize);

            Label_Top.Location = new Point(Label_TopLeft.Width, 0);
            Label_Bottom.Location = new Point(Label_BottomLeft.Width, Panel_Border.Height - Label_Bottom.Height);
            Label_Left.Location = new Point(0, Label_TopLeft.Height);
            Label_Right.Location = new Point(Panel_Border.Width - Label_Right.Width, Label_TopRight.Height);
            Label_TopLeft.Location = new Point(0, 0);
            Label_TopRight.Location = new Point(Panel_Border.Width - Label_TopRight.Width, 0);
            Label_BottomLeft.Location = new Point(0, Panel_Border.Height - Label_BottomLeft.Height);
            Label_BottomRight.Location = new Point(Panel_Border.Width - Label_BottomRight.Width, Panel_Border.Height - Label_BottomRight.Height);

            //

            _RepaintFormBorderBitmap();
        }

        //

        private void Panel_Border_Paint(object sender, PaintEventArgs e) // Panel_Border 的 Paint 事件的回调函数。
        {
            _RepaintFormBorderBitmap();
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
            Me.FormTitleBar.BringToFront();
            Me.FormClient.BringToFront();
            Me.FormClient.Focus();

            if (e.Button == MouseButtons.Left)
            {
                _CursorPositionOfMe = Geometry.GetCursorPositionOfControl(Panel_FormBounds);

                _MeIsResizing = true;

                Cursor.Clip = FormManager._PrimaryScreenClient;
            }
        }

        private void Label_Top_MouseUp(object sender, MouseEventArgs e) // Label_Top 的 MouseUp 事件的回调函数。
        {
            Me.FormTitleBar.BringToFront();
            Me.FormClient.BringToFront();
            Me.FormClient.Focus();

            if (e.Button == MouseButtons.Left)
            {
                if (Me.FormState == FormState.Normal)
                {
                    if (FormManager._CursorPosition.Y <= FormManager._PrimaryScreenClient.Y)
                    {
                        if (!Me.HighAsScreen())
                        {
                            Me._UpdateLayout(UpdateLayoutEventType.All);
                        }
                    }
                    else
                    {
                        Me._Bounds_Normal = Me._Bounds_Current;

                        Me._UpdateLayout(UpdateLayoutEventType.All);
                    }
                }
                else if (Me.FormState == FormState.QuarterScreen)
                {
                    Me._Bounds_QuarterScreen_Y = Me._Bounds_Current_Y;
                    Me._Bounds_QuarterScreen_Height = Me._Bounds_Current_Height;

                    Me._UpdateLayout(UpdateLayoutEventType.All);
                }
            }

            Cursor.Clip = FormManager._PrimaryScreenBounds;

            _MeIsResizing = false;
        }

        private void Label_Top_MouseMove(object sender, MouseEventArgs e) // Label_Top 的 MouseMove 事件的回调函数。
        {
            if (_MeIsResizing)
            {
                if (Me.FormState == FormState.HighAsScreen)
                {
                    Me._ReturnFromHighAsScreen();
                }

                if (Me.FormState == FormState.Normal)
                {
                    if (Me._Bounds_Normal_Bottom - (FormManager._CursorPosition.Y - _CursorPositionOfMe.Y) >= Me._MinimumBoundsHeight)
                    {
                        if (Me._Bounds_Normal_Bottom - (FormManager._CursorPosition.Y - _CursorPositionOfMe.Y) <= Math.Min(FormManager._PrimaryScreenBounds.Height, Me._MaximumBoundsHeight))
                        {
                            Me._Bounds_Current_Y = FormManager._CursorPosition.Y - _CursorPositionOfMe.Y;
                            Me._Bounds_Current_Height = Me._Bounds_Normal_Bottom - Me._Bounds_Current_Y;
                        }
                        else
                        {
                            Me._Bounds_Current_Y = Me._Bounds_Normal_Bottom - Math.Min(FormManager._PrimaryScreenBounds.Height, Me._MaximumBoundsHeight);
                            Me._Bounds_Current_Height = Math.Min(FormManager._PrimaryScreenBounds.Height, Me._MaximumBoundsHeight);
                        }
                    }
                    else
                    {
                        Me._Bounds_Current_Y = Me._Bounds_Normal_Bottom - Me._MinimumBoundsHeight;
                        Me._Bounds_Current_Height = Me._MinimumBoundsHeight;
                    }

                    _TryToUpdateLayout();
                }
                else if (Me.FormState == FormState.QuarterScreen)
                {
                    if (Me._Bounds_QuarterScreen_Bottom - (FormManager._CursorPosition.Y - _CursorPositionOfMe.Y) >= Me._MinimumBoundsHeight)
                    {
                        if (Me._Bounds_QuarterScreen_Bottom - (FormManager._CursorPosition.Y - _CursorPositionOfMe.Y) <= Math.Min(FormManager._PrimaryScreenBounds.Height, Me._MaximumBoundsHeight))
                        {
                            Me._Bounds_Current_Y = FormManager._CursorPosition.Y - _CursorPositionOfMe.Y;
                            Me._Bounds_Current_Height = Me._Bounds_QuarterScreen_Bottom - Me._Bounds_Current_Y;
                        }
                        else
                        {
                            Me._Bounds_Current_Y = Me._Bounds_QuarterScreen_Bottom - Math.Min(FormManager._PrimaryScreenBounds.Height, Me._MaximumBoundsHeight);
                            Me._Bounds_Current_Height = Math.Min(FormManager._PrimaryScreenBounds.Height, Me._MaximumBoundsHeight);
                        }
                    }
                    else
                    {
                        Me._Bounds_Current_Y = Me._Bounds_QuarterScreen_Bottom - Me._MinimumBoundsHeight;
                        Me._Bounds_Current_Height = Me._MinimumBoundsHeight;
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
            Me.FormTitleBar.BringToFront();
            Me.FormClient.BringToFront();
            Me.FormClient.Focus();

            if (e.Button == MouseButtons.Left)
            {
                _CursorPositionOfMe = Geometry.GetCursorPositionOfControl(Panel_FormBounds);

                _MeIsResizing = true;

                Cursor.Clip = FormManager._PrimaryScreenClient;
            }
        }

        private void Label_Bottom_MouseUp(object sender, MouseEventArgs e) // Label_Bottom 的 MouseUp 事件的回调函数。
        {
            Me.FormTitleBar.BringToFront();
            Me.FormClient.BringToFront();
            Me.FormClient.Focus();

            if (e.Button == MouseButtons.Left)
            {
                if (Me.FormState == FormState.Normal)
                {
                    if (FormManager._CursorPosition.Y >= FormManager._PrimaryScreenClient.Bottom - 1)
                    {
                        if (!Me.HighAsScreen())
                        {
                            Me._UpdateLayout(UpdateLayoutEventType.All);
                        }
                    }
                    else
                    {
                        Me._Bounds_Normal = Me._Bounds_Current;

                        Me._UpdateLayout(UpdateLayoutEventType.All);
                    }
                }
                else if (Me.FormState == FormState.QuarterScreen)
                {
                    Me._Bounds_QuarterScreen_Y = Me._Bounds_Current_Y;
                    Me._Bounds_QuarterScreen_Height = Me._Bounds_Current_Height;

                    Me._UpdateLayout(UpdateLayoutEventType.All);
                }
            }

            Cursor.Clip = FormManager._PrimaryScreenBounds;

            _MeIsResizing = false;
        }

        private void Label_Bottom_MouseMove(object sender, MouseEventArgs e) // Label_Bottom 的 MouseMove 事件的回调函数。
        {
            if (_MeIsResizing)
            {
                if (Me.FormState == FormState.HighAsScreen)
                {
                    _CursorPositionOfMe.Y = Me._Bounds_Normal_Height - (Me._Bounds_Current_Height - _CursorPositionOfMe.Y);

                    Me._ReturnFromHighAsScreen();
                }

                if (Me.FormState == FormState.Normal)
                {
                    if (Me._Bounds_Normal_Height + FormManager._CursorPosition.Y - Me._Bounds_Normal_Y - _CursorPositionOfMe.Y >= Me._MinimumBoundsHeight)
                    {
                        Me._Bounds_Current_Y = Me._Bounds_Normal_Y;
                        Me._Bounds_Current_Height = Math.Min(Math.Min(FormManager._PrimaryScreenBounds.Height, Me._MaximumBoundsHeight), Me._Bounds_Normal_Height - Me._Bounds_Normal_Y + FormManager._CursorPosition.Y - _CursorPositionOfMe.Y);
                    }
                    else
                    {
                        Me._Bounds_Current_Y = Me._Bounds_Normal_Y;
                        Me._Bounds_Current_Height = Me._MinimumBoundsHeight;
                    }

                    _TryToUpdateLayout();
                }
                else if (Me.FormState == FormState.QuarterScreen)
                {
                    if (Me._Bounds_QuarterScreen_Height + FormManager._CursorPosition.Y - Me._Bounds_QuarterScreen_Y - _CursorPositionOfMe.Y <= Math.Min(FormManager._PrimaryScreenBounds.Height, Me._MaximumBoundsHeight) && Me._Bounds_QuarterScreen_Height + FormManager._CursorPosition.Y - Me._Bounds_QuarterScreen_Y - _CursorPositionOfMe.Y >= Me._MinimumBoundsHeight)
                    {
                        Me._Bounds_Current_Y = Me._Bounds_QuarterScreen_Y;
                        Me._Bounds_Current_Height = Math.Min(Math.Min(FormManager._PrimaryScreenBounds.Height, Me._MaximumBoundsHeight), Me._Bounds_QuarterScreen_Height - Me._Bounds_QuarterScreen_Y + FormManager._CursorPosition.Y - _CursorPositionOfMe.Y);
                    }
                    else
                    {
                        Me._Bounds_Current_Y = Me._Bounds_QuarterScreen_Y;
                        Me._Bounds_Current_Height = Me._MinimumBoundsHeight;
                    }

                    _TryToUpdateLayout();
                }
            }
        }

        private void Label_Left_MouseDown(object sender, MouseEventArgs e) // Label_Left 的 MouseDown 事件的回调函数。
        {
            Me.FormTitleBar.BringToFront();
            Me.FormClient.BringToFront();
            Me.FormClient.Focus();

            if (e.Button == MouseButtons.Left)
            {
                _CursorPositionOfMe = Geometry.GetCursorPositionOfControl(Panel_FormBounds);

                if (Me.FormState == FormState.Normal || Me.FormState == FormState.HighAsScreen)
                {
                    Me._Bounds_Normal_X = Me._Bounds_Current_X;
                    Me._Bounds_Normal_Width = Me._Bounds_Current_Width;
                }

                _MeIsResizing = true;

                Cursor.Clip = FormManager._PrimaryScreenClient;
            }
        }

        private void Label_Left_MouseUp(object sender, MouseEventArgs e) // Label_Left 的 MouseUp 事件的回调函数。
        {
            Me.FormTitleBar.BringToFront();
            Me.FormClient.BringToFront();
            Me.FormClient.Focus();

            if (e.Button == MouseButtons.Left)
            {
                if (Me.FormState == FormState.Normal || Me.FormState == FormState.HighAsScreen)
                {
                    Me._Bounds_Normal_X = Me._Bounds_Current_X;
                    Me._Bounds_Normal_Width = Me._Bounds_Current_Width;

                    Me._UpdateLayout(UpdateLayoutEventType.All);
                }
                else if (Me.FormState == FormState.QuarterScreen)
                {
                    Me._Bounds_QuarterScreen_X = Me._Bounds_Current_X;
                    Me._Bounds_QuarterScreen_Width = Me._Bounds_Current_Width;

                    Me._UpdateLayout(UpdateLayoutEventType.All);
                }
            }

            Cursor.Clip = FormManager._PrimaryScreenBounds;

            _MeIsResizing = false;
        }

        private void Label_Left_MouseMove(object sender, MouseEventArgs e) // Label_Left 的 MouseMove 事件的回调函数。
        {
            if (_MeIsResizing)
            {
                if (Me.FormState == FormState.Normal || Me.FormState == FormState.HighAsScreen)
                {
                    if (Me._Bounds_Normal_Right - (FormManager._CursorPosition.X - _CursorPositionOfMe.X) >= Me._MinimumBoundsWidth)
                    {
                        if (Me._Bounds_Normal_Right - (FormManager._CursorPosition.X - _CursorPositionOfMe.X) <= Math.Min(FormManager._PrimaryScreenBounds.Width, Me._MaximumBoundsWidth))
                        {
                            Me._Bounds_Current_X = FormManager._CursorPosition.X - _CursorPositionOfMe.X;
                            Me._Bounds_Current_Width = Me._Bounds_Normal_Right - Me._Bounds_Current_X;
                        }
                        else
                        {
                            Me._Bounds_Current_X = Me._Bounds_Normal_Right - Math.Min(FormManager._PrimaryScreenBounds.Width, Me._MaximumBoundsWidth);
                            Me._Bounds_Current_Width = Math.Min(FormManager._PrimaryScreenBounds.Width, Me._MaximumBoundsWidth);
                        }
                    }
                    else
                    {
                        Me._Bounds_Current_X = Me._Bounds_Normal_Right - Me._MinimumBoundsWidth;
                        Me._Bounds_Current_Width = Me._MinimumBoundsWidth;
                    }

                    _TryToUpdateLayout();
                }
                else if (Me.FormState == FormState.QuarterScreen)
                {
                    if (Me._Bounds_QuarterScreen_Right - (FormManager._CursorPosition.X - _CursorPositionOfMe.X) >= Me._MinimumBoundsWidth)
                    {
                        if (Me._Bounds_QuarterScreen_Right - (FormManager._CursorPosition.X - _CursorPositionOfMe.X) <= Math.Min(FormManager._PrimaryScreenBounds.Width, Me._MaximumBoundsWidth))
                        {
                            Me._Bounds_Current_X = FormManager._CursorPosition.X - _CursorPositionOfMe.X;
                            Me._Bounds_Current_Width = Me._Bounds_QuarterScreen_Right - Me._Bounds_Current_X;
                        }
                        else
                        {
                            Me._Bounds_Current_X = Me._Bounds_QuarterScreen_Right - Math.Min(FormManager._PrimaryScreenBounds.Width, Me._MaximumBoundsWidth);
                            Me._Bounds_Current_Width = Math.Min(FormManager._PrimaryScreenBounds.Width, Me._MaximumBoundsWidth);
                        }
                    }
                    else
                    {
                        Me._Bounds_Current_X = Me._Bounds_QuarterScreen_Right - Me._MinimumBoundsWidth;
                        Me._Bounds_Current_Width = Me._MinimumBoundsWidth;
                    }

                    _TryToUpdateLayout();
                }
            }
        }

        private void Label_Right_MouseDown(object sender, MouseEventArgs e) // Label_Right 的 MouseDown 事件的回调函数。
        {
            Me.FormTitleBar.BringToFront();
            Me.FormClient.BringToFront();
            Me.FormClient.Focus();

            if (e.Button == MouseButtons.Left)
            {
                _CursorPositionOfMe = Geometry.GetCursorPositionOfControl(Panel_FormBounds);

                if (Me.FormState == FormState.Normal || Me.FormState == FormState.HighAsScreen)
                {
                    Me._Bounds_Normal_X = Me._Bounds_Current_X;
                    Me._Bounds_Normal_Width = Me._Bounds_Current_Width;
                }

                _MeIsResizing = true;

                Cursor.Clip = FormManager._PrimaryScreenClient;
            }
        }

        private void Label_Right_MouseUp(object sender, MouseEventArgs e) // Label_Right 的 MouseUp 事件的回调函数。
        {
            Me.FormTitleBar.BringToFront();
            Me.FormClient.BringToFront();
            Me.FormClient.Focus();

            if (e.Button == MouseButtons.Left)
            {
                if (Me.FormState == FormState.Normal || Me.FormState == FormState.HighAsScreen)
                {
                    Me._Bounds_Normal_Width = Me._Bounds_Current_Width;

                    Me._UpdateLayout(UpdateLayoutEventType.All);
                }
                else if (Me.FormState == FormState.QuarterScreen)
                {
                    Me._Bounds_QuarterScreen_Width = Me._Bounds_Current_Width;

                    Me._UpdateLayout(UpdateLayoutEventType.All);
                }
            }

            Cursor.Clip = FormManager._PrimaryScreenBounds;

            _MeIsResizing = false;
        }

        private void Label_Right_MouseMove(object sender, MouseEventArgs e) // Label_Right 的 MouseMove 事件的回调函数。
        {
            if (_MeIsResizing)
            {
                if (Me.FormState == FormState.Normal || Me.FormState == FormState.HighAsScreen)
                {
                    if (Me._Bounds_Normal_Width + FormManager._CursorPosition.X - Me._Bounds_Normal_X - _CursorPositionOfMe.X >= Me._MinimumBoundsWidth)
                    {
                        Me._Bounds_Current_X = Me._Bounds_Normal_X;
                        Me._Bounds_Current_Width = Math.Min(Math.Min(FormManager._PrimaryScreenBounds.Width, Me._MaximumBoundsWidth), Me._Bounds_Normal_Width - Me._Bounds_Normal_X + FormManager._CursorPosition.X - _CursorPositionOfMe.X);
                    }
                    else
                    {
                        Me._Bounds_Current_X = Me._Bounds_Normal_X;
                        Me._Bounds_Current_Width = Me._MinimumBoundsWidth;
                    }

                    _TryToUpdateLayout();
                }
                else if (Me.FormState == FormState.QuarterScreen)
                {
                    if (Me._Bounds_QuarterScreen_Width + FormManager._CursorPosition.X - Me._Bounds_QuarterScreen_X - _CursorPositionOfMe.X >= Me._MinimumBoundsWidth)
                    {
                        Me._Bounds_Current_X = Me._Bounds_QuarterScreen_X;
                        Me._Bounds_Current_Width = Math.Min(Math.Min(FormManager._PrimaryScreenBounds.Width, Me._MaximumBoundsWidth), Me._Bounds_QuarterScreen_Width - Me._Bounds_QuarterScreen_X + FormManager._CursorPosition.X - _CursorPositionOfMe.X);
                    }
                    else
                    {
                        Me._Bounds_Current_X = Me._Bounds_QuarterScreen_X;
                        Me._Bounds_Current_Width = Me._MinimumBoundsWidth;
                    }

                    _TryToUpdateLayout();
                }
            }
        }

        private void Label_TopLeft_MouseDown(object sender, MouseEventArgs e) // Label_TopLeft 的 MouseDown 事件的回调函数。
        {
            Me.FormTitleBar.BringToFront();
            Me.FormClient.BringToFront();
            Me.FormClient.Focus();

            if (e.Button == MouseButtons.Left)
            {
                _CursorPositionOfMe = Geometry.GetCursorPositionOfControl(Panel_FormBounds);

                if (Me.FormState == FormState.Normal || Me.FormState == FormState.HighAsScreen)
                {
                    Me._Bounds_Normal_X = Me._Bounds_Current_X;
                    Me._Bounds_Normal_Width = Me._Bounds_Current_Width;
                }

                _MeIsResizing = true;

                Cursor.Clip = FormManager._PrimaryScreenClient;
            }
        }

        private void Label_TopLeft_MouseUp(object sender, MouseEventArgs e) // Label_TopLeft 的 MouseUp 事件的回调函数。
        {
            Me.FormTitleBar.BringToFront();
            Me.FormClient.BringToFront();
            Me.FormClient.Focus();

            if (e.Button == MouseButtons.Left)
            {
                if (Me.FormState == FormState.Normal)
                {
                    if (FormManager._CursorPosition.Y <= FormManager._PrimaryScreenClient.Y)
                    {
                        Me._Bounds_Normal_X = Me._Bounds_Current_X;
                        Me._Bounds_Normal_Width = Me._Bounds_Current_Width;

                        if (!Me.HighAsScreen())
                        {
                            Me._UpdateLayout(UpdateLayoutEventType.All);
                        }
                    }
                    else
                    {
                        Me._Bounds_Normal = Me._Bounds_Current;

                        Me._UpdateLayout(UpdateLayoutEventType.All);
                    }
                }
                else if (Me.FormState == FormState.QuarterScreen)
                {
                    Me._Bounds_QuarterScreen = Me._Bounds_Current;

                    Me._UpdateLayout(UpdateLayoutEventType.All);
                }
            }

            Cursor.Clip = FormManager._PrimaryScreenBounds;

            _MeIsResizing = false;
        }

        private void Label_TopLeft_MouseMove(object sender, MouseEventArgs e) // Label_TopLeft 的 MouseMove 事件的回调函数。
        {
            if (_MeIsResizing)
            {
                if (Me.FormState == FormState.HighAsScreen)
                {
                    Me._ReturnFromHighAsScreen();
                }

                if (Me.FormState == FormState.Normal)
                {
                    if (Me._Bounds_Normal_Bottom - (FormManager._CursorPosition.Y - _CursorPositionOfMe.Y) >= Me._MinimumBoundsHeight)
                    {
                        if (Me._Bounds_Normal_Bottom - (FormManager._CursorPosition.Y - _CursorPositionOfMe.Y) <= Math.Min(FormManager._PrimaryScreenBounds.Height, Me._MaximumBoundsHeight))
                        {
                            Me._Bounds_Current_Y = FormManager._CursorPosition.Y - _CursorPositionOfMe.Y;
                            Me._Bounds_Current_Height = Me._Bounds_Normal_Bottom - Me._Bounds_Current_Y;
                        }
                        else
                        {
                            Me._Bounds_Current_Y = Me._Bounds_Normal_Bottom - Math.Min(FormManager._PrimaryScreenBounds.Height, Me._MaximumBoundsHeight);
                            Me._Bounds_Current_Height = Math.Min(FormManager._PrimaryScreenBounds.Height, Me._MaximumBoundsHeight);
                        }
                    }
                    else
                    {
                        Me._Bounds_Current_Y = Me._Bounds_Normal_Bottom - Me._MinimumBoundsHeight;
                        Me._Bounds_Current_Height = Me._MinimumBoundsHeight;
                    }

                    if (Me._Bounds_Normal_Right - (FormManager._CursorPosition.X - _CursorPositionOfMe.X) >= Me._MinimumBoundsWidth)
                    {
                        if (Me._Bounds_Normal_Right - (FormManager._CursorPosition.X - _CursorPositionOfMe.X) <= Math.Min(FormManager._PrimaryScreenBounds.Width, Me._MaximumBoundsWidth))
                        {
                            Me._Bounds_Current_X = FormManager._CursorPosition.X - _CursorPositionOfMe.X;
                            Me._Bounds_Current_Width = Me._Bounds_Normal_Right - Me._Bounds_Current_X;
                        }
                        else
                        {
                            Me._Bounds_Current_X = Me._Bounds_Normal_Right - Math.Min(FormManager._PrimaryScreenBounds.Width, Me._MaximumBoundsWidth);
                            Me._Bounds_Current_Width = Math.Min(FormManager._PrimaryScreenBounds.Width, Me._MaximumBoundsWidth);
                        }
                    }
                    else
                    {
                        Me._Bounds_Current_X = Me._Bounds_Normal_Right - Me._MinimumBoundsWidth;
                        Me._Bounds_Current_Width = Me._MinimumBoundsWidth;
                    }

                    _TryToUpdateLayout();
                }
                else if (Me.FormState == FormState.QuarterScreen)
                {
                    if (Me._Bounds_QuarterScreen_Bottom - (FormManager._CursorPosition.Y - _CursorPositionOfMe.Y) >= Me._MinimumBoundsHeight)
                    {
                        if (Me._Bounds_QuarterScreen_Bottom - (FormManager._CursorPosition.Y - _CursorPositionOfMe.Y) <= Math.Min(FormManager._PrimaryScreenBounds.Height, Me._MaximumBoundsHeight))
                        {
                            Me._Bounds_Current_Y = FormManager._CursorPosition.Y - _CursorPositionOfMe.Y;
                            Me._Bounds_Current_Height = Me._Bounds_QuarterScreen_Bottom - Me._Bounds_Current_Y;
                        }
                        else
                        {
                            Me._Bounds_Current_Y = Me._Bounds_QuarterScreen_Bottom - Math.Min(FormManager._PrimaryScreenBounds.Height, Me._MaximumBoundsHeight);
                            Me._Bounds_Current_Height = Math.Min(FormManager._PrimaryScreenBounds.Height, Me._MaximumBoundsHeight);
                        }
                    }
                    else
                    {
                        Me._Bounds_Current_Y = Me._Bounds_QuarterScreen_Bottom - Me._MinimumBoundsHeight;
                        Me._Bounds_Current_Height = Me._MinimumBoundsHeight;
                    }

                    if (Me._Bounds_QuarterScreen_Right - (FormManager._CursorPosition.X - _CursorPositionOfMe.X) >= Me._MinimumBoundsWidth)
                    {
                        if (Me._Bounds_QuarterScreen_Right - (FormManager._CursorPosition.X - _CursorPositionOfMe.X) <= Math.Min(FormManager._PrimaryScreenBounds.Width, Me._MaximumBoundsWidth))
                        {
                            Me._Bounds_Current_X = FormManager._CursorPosition.X - _CursorPositionOfMe.X;
                            Me._Bounds_Current_Width = Me._Bounds_QuarterScreen_Right - Me._Bounds_Current_X;
                        }
                        else
                        {
                            Me._Bounds_Current_X = Me._Bounds_QuarterScreen_Right - Math.Min(FormManager._PrimaryScreenBounds.Width, Me._MaximumBoundsWidth);
                            Me._Bounds_Current_Width = Math.Min(FormManager._PrimaryScreenBounds.Width, Me._MaximumBoundsWidth);
                        }
                    }
                    else
                    {
                        Me._Bounds_Current_X = Me._Bounds_QuarterScreen_Right - Me._MinimumBoundsWidth;
                        Me._Bounds_Current_Width = Me._MinimumBoundsWidth;
                    }

                    _TryToUpdateLayout();
                }
            }
        }

        private void Label_TopRight_MouseDown(object sender, MouseEventArgs e) // Label_TopRight 的 MouseDown 事件的回调函数。
        {
            Me.FormTitleBar.BringToFront();
            Me.FormClient.BringToFront();
            Me.FormClient.Focus();

            if (e.Button == MouseButtons.Left)
            {
                _CursorPositionOfMe = Geometry.GetCursorPositionOfControl(Panel_FormBounds);

                if (Me.FormState == FormState.Normal || Me.FormState == FormState.HighAsScreen)
                {
                    Me._Bounds_Normal_X = Me._Bounds_Current_X;
                    Me._Bounds_Normal_Width = Me._Bounds_Current_Width;
                }

                _MeIsResizing = true;

                Cursor.Clip = FormManager._PrimaryScreenClient;
            }
        }

        private void Label_TopRight_MouseUp(object sender, MouseEventArgs e) // Label_TopRight 的 MouseUp 事件的回调函数。
        {
            Me.FormTitleBar.BringToFront();
            Me.FormClient.BringToFront();
            Me.FormClient.Focus();

            if (e.Button == MouseButtons.Left)
            {
                if (Me.FormState == FormState.Normal)
                {
                    if (FormManager._CursorPosition.Y <= FormManager._PrimaryScreenClient.Y)
                    {
                        Me._Bounds_Normal_X = Me._Bounds_Current_X;
                        Me._Bounds_Normal_Width = Me._Bounds_Current_Width;

                        if (!Me.HighAsScreen())
                        {
                            Me._UpdateLayout(UpdateLayoutEventType.All);
                        }
                    }
                    else
                    {
                        Me._Bounds_Normal = Me._Bounds_Current;

                        Me._UpdateLayout(UpdateLayoutEventType.All);
                    }
                }
                else if (Me.FormState == FormState.QuarterScreen)
                {
                    Me._Bounds_QuarterScreen = Me._Bounds_Current;

                    Me._UpdateLayout(UpdateLayoutEventType.All);
                }
            }

            Cursor.Clip = FormManager._PrimaryScreenBounds;

            _MeIsResizing = false;
        }

        private void Label_TopRight_MouseMove(object sender, MouseEventArgs e) // Label_TopRight 的 MouseMove 事件的回调函数。
        {
            if (_MeIsResizing)
            {
                if (Me.FormState == FormState.HighAsScreen)
                {
                    Me._ReturnFromHighAsScreen();
                }

                if (Me.FormState == FormState.Normal)
                {
                    if (Me._Bounds_Normal_Bottom - (FormManager._CursorPosition.Y - _CursorPositionOfMe.Y) >= Me._MinimumBoundsHeight)
                    {
                        if (Me._Bounds_Normal_Bottom - (FormManager._CursorPosition.Y - _CursorPositionOfMe.Y) <= Math.Min(FormManager._PrimaryScreenBounds.Height, Me._MaximumBoundsHeight))
                        {
                            Me._Bounds_Current_Y = FormManager._CursorPosition.Y - _CursorPositionOfMe.Y;
                            Me._Bounds_Current_Height = Me._Bounds_Normal_Bottom - Me._Bounds_Current_Y;
                        }
                        else
                        {
                            Me._Bounds_Current_Y = Me._Bounds_Normal_Bottom - Math.Min(FormManager._PrimaryScreenBounds.Height, Me._MaximumBoundsHeight);
                            Me._Bounds_Current_Height = Math.Min(FormManager._PrimaryScreenBounds.Height, Me._MaximumBoundsHeight);
                        }
                    }
                    else
                    {
                        Me._Bounds_Current_Y = Me._Bounds_Normal_Bottom - Me._MinimumBoundsHeight;
                        Me._Bounds_Current_Height = Me._MinimumBoundsHeight;
                    }

                    if (Me._Bounds_Normal_Width + FormManager._CursorPosition.X - Me._Bounds_Normal_X - _CursorPositionOfMe.X >= Me._MinimumBoundsWidth)
                    {
                        Me._Bounds_Current_X = Me._Bounds_Normal_X;
                        Me._Bounds_Current_Width = Math.Min(Math.Min(FormManager._PrimaryScreenBounds.Width, Me._MaximumBoundsWidth), Me._Bounds_Normal_Width - Me._Bounds_Normal_X + FormManager._CursorPosition.X - _CursorPositionOfMe.X);
                    }
                    else
                    {
                        Me._Bounds_Current_X = Me._Bounds_Normal_X;
                        Me._Bounds_Current_Width = Me._MinimumBoundsWidth;
                    }

                    _TryToUpdateLayout();
                }
                else if (Me.FormState == FormState.QuarterScreen)
                {
                    if (Me._Bounds_QuarterScreen_Bottom - (FormManager._CursorPosition.Y - _CursorPositionOfMe.Y) >= Me._MinimumBoundsHeight)
                    {
                        if (Me._Bounds_QuarterScreen_Bottom - (FormManager._CursorPosition.Y - _CursorPositionOfMe.Y) <= Math.Min(FormManager._PrimaryScreenBounds.Height, Me._MaximumBoundsHeight))
                        {
                            Me._Bounds_Current_Y = FormManager._CursorPosition.Y - _CursorPositionOfMe.Y;
                            Me._Bounds_Current_Height = Me._Bounds_QuarterScreen_Bottom - Me._Bounds_Current_Y;
                        }
                        else
                        {
                            Me._Bounds_Current_Y = Me._Bounds_QuarterScreen_Bottom - Math.Min(FormManager._PrimaryScreenBounds.Height, Me._MaximumBoundsHeight);
                            Me._Bounds_Current_Height = Math.Min(FormManager._PrimaryScreenBounds.Height, Me._MaximumBoundsHeight);
                        }
                    }
                    else
                    {
                        Me._Bounds_Current_Y = Me._Bounds_QuarterScreen_Bottom - Me._MinimumBoundsHeight;
                        Me._Bounds_Current_Height = Me._MinimumBoundsHeight;
                    }

                    if (Me._Bounds_QuarterScreen_Width + FormManager._CursorPosition.X - Me._Bounds_QuarterScreen_X - _CursorPositionOfMe.X >= Me._MinimumBoundsWidth)
                    {
                        Me._Bounds_Current_X = Me._Bounds_QuarterScreen_X;
                        Me._Bounds_Current_Width = Math.Min(Math.Min(FormManager._PrimaryScreenBounds.Width, Me._MaximumBoundsWidth), Me._Bounds_QuarterScreen_Width - Me._Bounds_QuarterScreen_X + FormManager._CursorPosition.X - _CursorPositionOfMe.X);
                    }
                    else
                    {
                        Me._Bounds_Current_X = Me._Bounds_QuarterScreen_X;
                        Me._Bounds_Current_Width = Me._MinimumBoundsWidth;
                    }

                    _TryToUpdateLayout();
                }
            }
        }

        private void Label_BottomLeft_MouseDown(object sender, MouseEventArgs e) // Label_BottomLeft 的 MouseDown 事件的回调函数。
        {
            Me.FormTitleBar.BringToFront();
            Me.FormClient.BringToFront();
            Me.FormClient.Focus();

            if (e.Button == MouseButtons.Left)
            {
                _CursorPositionOfMe = Geometry.GetCursorPositionOfControl(Panel_FormBounds);

                if (Me.FormState == FormState.Normal || Me.FormState == FormState.HighAsScreen)
                {
                    Me._Bounds_Normal_X = Me._Bounds_Current_X;
                    Me._Bounds_Normal_Width = Me._Bounds_Current_Width;
                }

                _MeIsResizing = true;

                Cursor.Clip = FormManager._PrimaryScreenClient;
            }
        }

        private void Label_BottomLeft_MouseUp(object sender, MouseEventArgs e) // Label_BottomLeft 的 MouseUp 事件的回调函数。
        {
            Me.FormTitleBar.BringToFront();
            Me.FormClient.BringToFront();
            Me.FormClient.Focus();

            if (e.Button == MouseButtons.Left)
            {
                if (Me.FormState == FormState.Normal)
                {
                    if (FormManager._CursorPosition.Y >= FormManager._PrimaryScreenClient.Bottom - 1)
                    {
                        Me._Bounds_Normal_X = Me._Bounds_Current_X;
                        Me._Bounds_Normal_Width = Me._Bounds_Current_Width;

                        if (!Me.HighAsScreen())
                        {
                            Me._UpdateLayout(UpdateLayoutEventType.All);
                        }
                    }
                    else
                    {
                        Me._Bounds_Normal = Me._Bounds_Current;

                        Me._UpdateLayout(UpdateLayoutEventType.All);
                    }
                }
                else if (Me.FormState == FormState.QuarterScreen)
                {
                    Me._Bounds_QuarterScreen = Me._Bounds_Current;

                    Me._UpdateLayout(UpdateLayoutEventType.All);
                }
            }

            Cursor.Clip = FormManager._PrimaryScreenBounds;

            _MeIsResizing = false;
        }

        private void Label_BottomLeft_MouseMove(object sender, MouseEventArgs e) // Label_BottomLeft 的 MouseMove 事件的回调函数。
        {
            if (_MeIsResizing)
            {
                if (Me.FormState == FormState.HighAsScreen)
                {
                    _CursorPositionOfMe.Y = Me._Bounds_Normal_Height - (Me._Bounds_Current_Height - _CursorPositionOfMe.Y);

                    Me._ReturnFromHighAsScreen();
                }

                if (Me.FormState == FormState.Normal)
                {
                    if (Me._Bounds_Normal_Height + FormManager._CursorPosition.Y - Me._Bounds_Normal_Y - _CursorPositionOfMe.Y >= Me._MinimumBoundsHeight)
                    {
                        Me._Bounds_Current_Y = Me._Bounds_Normal_Y;
                        Me._Bounds_Current_Height = Math.Min(Math.Min(FormManager._PrimaryScreenBounds.Height, Me._MaximumBoundsHeight), Me._Bounds_Normal_Height - Me._Bounds_Normal_Y + FormManager._CursorPosition.Y - _CursorPositionOfMe.Y);
                    }
                    else
                    {
                        Me._Bounds_Current_Y = Me._Bounds_Normal_Y;
                        Me._Bounds_Current_Height = Me._MinimumBoundsHeight;
                    }

                    if (Me._Bounds_Normal_Right - (FormManager._CursorPosition.X - _CursorPositionOfMe.X) >= Me._MinimumBoundsWidth)
                    {
                        if (Me._Bounds_Normal_Right - (FormManager._CursorPosition.X - _CursorPositionOfMe.X) <= Math.Min(FormManager._PrimaryScreenBounds.Width, Me._MaximumBoundsWidth))
                        {
                            Me._Bounds_Current_X = FormManager._CursorPosition.X - _CursorPositionOfMe.X;
                            Me._Bounds_Current_Width = Me._Bounds_Normal_Right - Me._Bounds_Current_X;
                        }
                        else
                        {
                            Me._Bounds_Current_X = Me._Bounds_Normal_Right - Math.Min(FormManager._PrimaryScreenBounds.Width, Me._MaximumBoundsWidth);
                            Me._Bounds_Current_Width = Math.Min(FormManager._PrimaryScreenBounds.Width, Me._MaximumBoundsWidth);
                        }
                    }
                    else
                    {
                        Me._Bounds_Current_X = Me._Bounds_Normal_Right - Me._MinimumBoundsWidth;
                        Me._Bounds_Current_Width = Me._MinimumBoundsWidth;
                    }

                    _TryToUpdateLayout();
                }
                else if (Me.FormState == FormState.QuarterScreen)
                {
                    if (Me._Bounds_QuarterScreen_Height + FormManager._CursorPosition.Y - Me._Bounds_QuarterScreen_Y - _CursorPositionOfMe.Y <= Math.Min(FormManager._PrimaryScreenBounds.Height, Me._MaximumBoundsHeight) && Me._Bounds_QuarterScreen_Height + FormManager._CursorPosition.Y - Me._Bounds_QuarterScreen_Y - _CursorPositionOfMe.Y >= Me._MinimumBoundsHeight)
                    {
                        Me._Bounds_Current_Y = Me._Bounds_QuarterScreen_Y;
                        Me._Bounds_Current_Height = Math.Min(Math.Min(FormManager._PrimaryScreenBounds.Height, Me._MaximumBoundsHeight), Me._Bounds_QuarterScreen_Height - Me._Bounds_QuarterScreen_Y + FormManager._CursorPosition.Y - _CursorPositionOfMe.Y);
                    }
                    else
                    {
                        Me._Bounds_Current_Y = Me._Bounds_QuarterScreen_Y;
                        Me._Bounds_Current_Height = Me._MinimumBoundsHeight;
                    }

                    if (Me._Bounds_QuarterScreen_Right - (FormManager._CursorPosition.X - _CursorPositionOfMe.X) >= Me._MinimumBoundsWidth)
                    {
                        if (Me._Bounds_QuarterScreen_Right - (FormManager._CursorPosition.X - _CursorPositionOfMe.X) <= Math.Min(FormManager._PrimaryScreenBounds.Width, Me._MaximumBoundsWidth))
                        {
                            Me._Bounds_Current_X = FormManager._CursorPosition.X - _CursorPositionOfMe.X;
                            Me._Bounds_Current_Width = Me._Bounds_QuarterScreen_Right - Me._Bounds_Current_X;
                        }
                        else
                        {
                            Me._Bounds_Current_X = Me._Bounds_QuarterScreen_Right - Math.Min(FormManager._PrimaryScreenBounds.Width, Me._MaximumBoundsWidth);
                            Me._Bounds_Current_Width = Math.Min(FormManager._PrimaryScreenBounds.Width, Me._MaximumBoundsWidth);
                        }
                    }
                    else
                    {
                        Me._Bounds_Current_X = Me._Bounds_QuarterScreen_Right - Me._MinimumBoundsWidth;
                        Me._Bounds_Current_Width = Me._MinimumBoundsWidth;
                    }

                    _TryToUpdateLayout();
                }
            }
        }

        private void Label_BottomRight_MouseDown(object sender, MouseEventArgs e) // Label_BottomRight 的 MouseDown 事件的回调函数。
        {
            Me.FormTitleBar.BringToFront();
            Me.FormClient.BringToFront();
            Me.FormClient.Focus();

            if (e.Button == MouseButtons.Left)
            {
                _CursorPositionOfMe = Geometry.GetCursorPositionOfControl(Panel_FormBounds);

                if (Me.FormState == FormState.Normal || Me.FormState == FormState.HighAsScreen)
                {
                    Me._Bounds_Normal_X = Me._Bounds_Current_X;
                    Me._Bounds_Normal_Width = Me._Bounds_Current_Width;
                }

                _MeIsResizing = true;

                Cursor.Clip = FormManager._PrimaryScreenClient;
            }
        }

        private void Label_BottomRight_MouseUp(object sender, MouseEventArgs e) // Label_BottomRight 的 MouseUp 事件的回调函数。
        {
            Me.FormTitleBar.BringToFront();
            Me.FormClient.BringToFront();
            Me.FormClient.Focus();

            if (e.Button == MouseButtons.Left)
            {
                if (Me.FormState == FormState.Normal)
                {
                    if (FormManager._CursorPosition.Y >= FormManager._PrimaryScreenClient.Bottom - 1)
                    {
                        Me._Bounds_Normal_X = Me._Bounds_Current_X;
                        Me._Bounds_Normal_Width = Me._Bounds_Current_Width;

                        if (!Me.HighAsScreen())
                        {
                            Me._UpdateLayout(UpdateLayoutEventType.All);
                        }
                    }
                    else
                    {
                        Me._Bounds_Normal = Me._Bounds_Current;

                        Me._UpdateLayout(UpdateLayoutEventType.All);
                    }
                }
                else if (Me.FormState == FormState.QuarterScreen)
                {
                    Me._Bounds_QuarterScreen = Me._Bounds_Current;

                    Me._UpdateLayout(UpdateLayoutEventType.All);
                }
            }

            Cursor.Clip = FormManager._PrimaryScreenBounds;

            _MeIsResizing = false;
        }

        private void Label_BottomRight_MouseMove(object sender, MouseEventArgs e) // Label_BottomRight 的 MouseMove 事件的回调函数。
        {
            if (_MeIsResizing)
            {
                if (Me.FormState == FormState.HighAsScreen)
                {
                    _CursorPositionOfMe.Y = Me._Bounds_Normal_Height - (Me._Bounds_Current_Height - _CursorPositionOfMe.Y);

                    Me._ReturnFromHighAsScreen();
                }

                if (Me.FormState == FormState.Normal)
                {
                    if (Me._Bounds_Normal_Height + FormManager._CursorPosition.Y - Me._Bounds_Normal_Y - _CursorPositionOfMe.Y >= Me._MinimumBoundsHeight)
                    {
                        Me._Bounds_Current_Y = Me._Bounds_Normal_Y;
                        Me._Bounds_Current_Height = Math.Min(Math.Min(FormManager._PrimaryScreenBounds.Height, Me._MaximumBoundsHeight), Me._Bounds_Normal_Height - Me._Bounds_Normal_Y + FormManager._CursorPosition.Y - _CursorPositionOfMe.Y);
                    }
                    else
                    {
                        Me._Bounds_Current_Y = Me._Bounds_Normal_Y;
                        Me._Bounds_Current_Height = Me._MinimumBoundsHeight;
                    }

                    if (Me._Bounds_Normal_Width + FormManager._CursorPosition.X - Me._Bounds_Normal_X - _CursorPositionOfMe.X >= Me._MinimumBoundsWidth)
                    {
                        Me._Bounds_Current_X = Me._Bounds_Normal_X;
                        Me._Bounds_Current_Width = Math.Min(Math.Min(FormManager._PrimaryScreenBounds.Width, Me._MaximumBoundsWidth), Me._Bounds_Normal_Width - Me._Bounds_Normal_X + FormManager._CursorPosition.X - _CursorPositionOfMe.X);
                    }
                    else
                    {
                        Me._Bounds_Current_X = Me._Bounds_Normal_X;
                        Me._Bounds_Current_Width = Me._MinimumBoundsWidth;
                    }

                    _TryToUpdateLayout();
                }
                else if (Me.FormState == FormState.QuarterScreen)
                {
                    if (Me._Bounds_QuarterScreen_Height + FormManager._CursorPosition.Y - Me._Bounds_QuarterScreen_Y - _CursorPositionOfMe.Y <= Math.Min(FormManager._PrimaryScreenBounds.Height, Me._MaximumBoundsHeight) && Me._Bounds_QuarterScreen_Height + FormManager._CursorPosition.Y - Me._Bounds_QuarterScreen_Y - _CursorPositionOfMe.Y >= Me._MinimumBoundsHeight)
                    {
                        Me._Bounds_Current_Y = Me._Bounds_QuarterScreen_Y;
                        Me._Bounds_Current_Height = Math.Min(Math.Min(FormManager._PrimaryScreenBounds.Height, Me._MaximumBoundsHeight), Me._Bounds_QuarterScreen_Height - Me._Bounds_QuarterScreen_Y + FormManager._CursorPosition.Y - _CursorPositionOfMe.Y);
                    }
                    else
                    {
                        Me._Bounds_Current_Y = Me._Bounds_QuarterScreen_Y;
                        Me._Bounds_Current_Height = Me._MinimumBoundsHeight;
                    }

                    if (Me._Bounds_QuarterScreen_Width + FormManager._CursorPosition.X - Me._Bounds_QuarterScreen_X - _CursorPositionOfMe.X >= Me._MinimumBoundsWidth)
                    {
                        Me._Bounds_Current_X = Me._Bounds_QuarterScreen_X;
                        Me._Bounds_Current_Width = Math.Min(Math.Min(FormManager._PrimaryScreenBounds.Width, Me._MaximumBoundsWidth), Me._Bounds_QuarterScreen_Width - Me._Bounds_QuarterScreen_X + FormManager._CursorPosition.X - _CursorPositionOfMe.X);
                    }
                    else
                    {
                        Me._Bounds_Current_X = Me._Bounds_QuarterScreen_X;
                        Me._Bounds_Current_Width = Me._MinimumBoundsWidth;
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
            Me._UpdateLayout(UpdateLayoutEventType.Manual);

            _LastUpdateLayout = DateTime.Now;
        }

        #endregion

        #region 构造与析构函数

        public FormBorder(FormManager formController) // 使用 FormManager 对象初始化 FormBorder 的新实例。
        {
            InitializeComponent();

            //

            Me = formController;
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
                Me.FormTitleBar.BringToFront();
                Me.FormClient.BringToFront();
                Me.FormClient.Focus();
            }
        }

        public void OnOpacityChanged() // 在 OpacityChanged 事件发生时发生。
        {
            _RepaintFormBorderBitmap();
        }

        #endregion
    }
}
