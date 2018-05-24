/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2013-2018 chibayuki@foxmail.com

Com.WinForm.FormTitleBar
Version 18.5.24.0000

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
    internal partial class FormTitleBar : Form // 窗体标题栏。
    {
        #region 私有与内部成员

        private FormManager Me; // 窗体管理器。

        //

        private Point _CursorPositionOfMe = new Point(); // 鼠标指针在窗体的坐标。

        private bool _MeWillMove = false; // 是否即将移动窗体。
        private bool _MeIsMoving = false; // 是否正在移动窗体。

        private const int _ExtendDist = 2; // 扩展距离，用于某些鼠标动作的判定。

        //

        private Bitmap _FormCaptionBitmap; // 窗体标题栏绘图。

        private void _RefreshFormCaptionBitmap() // 更新窗体标题栏绘图。
        {
            if (_FormCaptionBitmap != null)
            {
                _FormCaptionBitmap.Dispose();
            }

            _FormCaptionBitmap = new Bitmap(Math.Max(1, Panel_TitleBar.Width), Math.Max(1, Math.Min(Panel_TitleBar.Height, Panel_ControlBox.Height)));

            using (Graphics CreateFormCaptionBmp = Graphics.FromImage(_FormCaptionBitmap))
            {
                CreateFormCaptionBmp.SmoothingMode = SmoothingMode.AntiAlias;

                //

                CreateFormCaptionBmp.Clear(Me.RecommendColors.FormTitleBar.ToColor());

                //

                if (Me.FormTitleBarBackgroundImage != null)
                {
                    CreateFormCaptionBmp.DrawImage(Me.FormTitleBarBackgroundImage, new Point(0, 0));
                }

                //

                if (Me.Caption.Length > 0)
                {
                    Rectangle CaptionArea = new Rectangle(new Point(Panel_FormIcon.Right, 0), new Size(Math.Max(1, Panel_ControlBox.Left - Panel_FormIcon.Right), Math.Max(1, Panel_FormIcon.Height)));

                    Font CaptionFont = Me.CaptionFont;

                    string Caption = Me.Caption;

                    SizeF CaptionSizeF = TextRenderer.MeasureText(Caption, CaptionFont);

                    if (CaptionSizeF.Width > CaptionArea.Width && Caption.Length > 1)
                    {
                        for (int i = Caption.Length - 1; i >= 1; i--)
                        {
                            Caption = Caption.Substring(0, i);

                            SizeF FCSizeF = TextRenderer.MeasureText(Caption + "...", CaptionFont);

                            if (FCSizeF.Width <= CaptionArea.Width)
                            {
                                Caption += "...";

                                break;
                            }
                        }

                        CaptionSizeF = TextRenderer.MeasureText(Caption, CaptionFont);
                    }

                    if (CaptionSizeF.Width <= CaptionArea.Width)
                    {
                        PointF CaptionLocF = new PointF(CaptionArea.X + (CaptionArea.Width - CaptionSizeF.Width) / 2, CaptionArea.Y + (CaptionArea.Height - CaptionSizeF.Height) / 2);

                        Color Cr_Caption_Fr = Me.RecommendColors.FormCaption.ToColor();
                        Color Cr_Caption_Bk_Outer, Cr_Caption_Bk_Inner;

                        if (!RecommendColors._BackColorFitLightText(Me.RecommendColors.FormCaption))
                        {
                            Cr_Caption_Bk_Outer = Color.FromArgb(32, Color.Black);
                            Cr_Caption_Bk_Inner = Color.FromArgb(64, Color.Black);
                        }
                        else
                        {
                            Cr_Caption_Bk_Outer = Color.FromArgb(48, Color.White);
                            Cr_Caption_Bk_Inner = Color.FromArgb(96, Color.White);
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

        private void _RepaintFormCaptionBitmap() // 更新并重绘窗体标题栏绘图。
        {
            if (Me.FormState != FormState.FullScreen)
            {
                _RefreshFormCaptionBitmap();

                if (_FormCaptionBitmap != null)
                {
                    Panel_TitleBar.CreateGraphics().DrawImage(_FormCaptionBitmap, new Point(0, 0));

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
                Panel_FormIcon.Visible = true;

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
            bool Flag = !RecommendColors._BackColorFitLightText(Me.RecommendColors.FormTitleBar);

            PictureBox_FullScreen.Image = (Flag ? (Me.FormState == FormState.FullScreen ? Properties.Resources.ControlBox_DarkImage_ExitFullScreen_16 : Properties.Resources.ControlBox_DarkImage_EnterFullScreen_16) : (Me.FormState == FormState.FullScreen ? Properties.Resources.ControlBox_LightImage_ExitFullScreen_16 : Properties.Resources.ControlBox_LightImage_EnterFullScreen_16));
            PictureBox_Minimize.Image = (Flag ? Properties.Resources.ControlBox_DarkImage_Minimize_16 : Properties.Resources.ControlBox_LightImage_Minimize_16);
            PictureBox_Maximize.Image = (Flag ? (Me.FormState == FormState.Maximized ? Properties.Resources.ControlBox_DarkImage_Return_16 : Properties.Resources.ControlBox_DarkImage_Maximize_16) : (Me.FormState == FormState.Maximized ? Properties.Resources.ControlBox_LightImage_Return_16 : Properties.Resources.ControlBox_LightImage_Maximize_16));
            PictureBox_Exit.Image = (Flag ? Properties.Resources.ControlBox_DarkImage_Exit_Normal_16 : Properties.Resources.ControlBox_LightImage_Exit_Normal_16);
        }

        //

        private DateTime _LastUpdateLayout = new DateTime(); // 上次更新窗体标题栏布局的日期时间。

        private void _TryToUpdateLayout() // 尝试更新窗体标题栏布局。
        {
            if (!BackgroundWorker_UpdateLayoutDelay.IsBusy)
            {
                BackgroundWorker_UpdateLayoutDelay.RunWorkerAsync();
            }
        }

        #endregion

        #region 回调函数

        private void FormTitleBar_Load(object sender, EventArgs e) // FormTitleBar 的 Load 事件的回调函数。
        {
            PictureBox_FormIcon.Image = Me.FormClient.Icon.ToBitmap();

            //

            _UpdateForStyleOrStateChanged();

            //

            OnThemeChanged();

            //

            FormTitleBar_SizeChanged(this, EventArgs.Empty);

            //

            Panel_FormIcon.Visible = Panel_ControlBox.Visible = false;
        }

        private void FormTitleBar_LocationChanged(object sender, EventArgs e) // FormTitleBar 的 LocationChanged 事件的回调函数。
        {
            _RepaintFormCaptionBitmap();
        }

        private void FormTitleBar_SizeChanged(object sender, EventArgs e) // FormTitleBar 的 SizeChanged 事件的回调函数。
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }

            //

            Panel_TitleBar.Size = new Size(Me.Width, Me.FormTitleBarHeight);

            if (Me.FormState == FormState.FullScreen)
            {
                this.Size = Panel_TitleBar.Size = Panel_ControlBox.Size;
                this.Location = new Point(Me.Right - Panel_ControlBox.Width, Me.Y);
            }

            Panel_FormIcon.Size = new Size(Math.Min(Panel_TitleBar.Height, Panel_ControlBox.Height), Math.Min(Panel_TitleBar.Height, Panel_ControlBox.Height));
            PictureBox_FormIcon.Location = new Point((Panel_FormIcon.Width - PictureBox_FormIcon.Width) / 2, (Panel_FormIcon.Height - PictureBox_FormIcon.Height) / 2);

            Panel_ControlBox.Location = new Point(Panel_TitleBar.Width - Panel_ControlBox.Width, Math.Min(0, (Panel_TitleBar.Height - Panel_ControlBox.Height) / 2));

            //

            _RepaintFormCaptionBitmap();
        }

        //

        private void Panel_TitleBar_Paint(object sender, PaintEventArgs e) // Panel_TitleBar 的 Paint 事件的回调函数。
        {
            if (Me.FormState != FormState.FullScreen)
            {
                if (_FormCaptionBitmap == null)
                {
                    _RefreshFormCaptionBitmap();
                }

                if (_FormCaptionBitmap != null)
                {
                    e.Graphics.DrawImage(_FormCaptionBitmap, new Point(0, 0));
                }
            }
        }

        private void Panel_TitleBar_MouseDoubleClick(object sender, MouseEventArgs e) // Panel_TitleBar 的 MouseDoubleClick 事件的回调函数。
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

        private void Panel_TitleBar_MouseDown(object sender, MouseEventArgs e) // Panel_TitleBar 的 MouseDown 事件的回调函数。
        {
            Me.FormClient.Focus();

            if (e.Button == MouseButtons.Left)
            {
                if (Me.FormState != FormState.FullScreen)
                {
                    _CursorPositionOfMe = Geometry.GetCursorPositionOfControl(Panel_TitleBar);

                    _MeWillMove = true;

                    Cursor.Clip = FormManager._PrimaryScreenClient;
                }
            }
        }

        private void Panel_TitleBar_MouseUp(object sender, MouseEventArgs e) // Panel_TitleBar 的 MouseUp 事件的回调函数。
        {
            Me.FormClient.Focus();

            if (_MeIsMoving == true && e.Button == MouseButtons.Left)
            {
                Action ReleaseAndCheckY = () =>
                {
                    Me._Bounds_Current_Y = Math.Max(Me._Bounds_Current_Location.Y, FormManager._PrimaryScreenClient.Y);

                    Me._UpdateLayout(UpdateLayoutEventType.All);

                    if (Me.FormState != FormState.HighAsScreen)
                    {
                        Me._Bounds_Normal_Location = Me._Bounds_Current_Location;
                    }
                };

                if (Me.FormStyle == FormStyle.Sizable)
                {
                    if (FormManager._CursorPosition.X >= FormManager._PrimaryScreenClient.X && FormManager._CursorPosition.X <= FormManager._PrimaryScreenClient.X + _ExtendDist)
                    {
                        if (FormManager._CursorPosition.Y >= FormManager._PrimaryScreenClient.Y && FormManager._CursorPosition.Y <= FormManager._PrimaryScreenClient.Y + _ExtendDist)
                        {
                            if (!Me.TopLeftQuarterScreen())
                            {
                                ReleaseAndCheckY();
                            }
                        }
                        else if (FormManager._CursorPosition.Y >= FormManager._PrimaryScreenClient.Bottom - _ExtendDist && FormManager._CursorPosition.Y <= FormManager._PrimaryScreenBounds.Bottom)
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
                    else if (FormManager._CursorPosition.X >= FormManager._PrimaryScreenClient.Right - _ExtendDist && FormManager._CursorPosition.X <= FormManager._PrimaryScreenBounds.Right)
                    {
                        if (FormManager._CursorPosition.Y >= FormManager._PrimaryScreenClient.Y && FormManager._CursorPosition.Y <= FormManager._PrimaryScreenClient.Y + _ExtendDist)
                        {
                            if (!Me.TopRightQuarterScreen())
                            {
                                ReleaseAndCheckY();
                            }
                        }
                        else if (FormManager._CursorPosition.Y >= FormManager._PrimaryScreenClient.Bottom - _ExtendDist && FormManager._CursorPosition.Y <= FormManager._PrimaryScreenBounds.Bottom)
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
                    else if (FormManager._CursorPosition.Y >= FormManager._PrimaryScreenClient.Y && FormManager._CursorPosition.Y <= FormManager._PrimaryScreenClient.Y + _ExtendDist)
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

            Cursor.Clip = FormManager._PrimaryScreenBounds;
        }

        private void Panel_TitleBar_MouseMove(object sender, MouseEventArgs e) // Panel_TitleBar 的 MouseMove 事件的回调函数。
        {
            if (_MeWillMove)
            {
                Point CurPt = Geometry.GetCursorPositionOfControl(Panel_TitleBar);

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
                    if ((Me.FormState == FormState.Maximized && (FormManager._CursorPosition.Y > FormManager._PrimaryScreenClient.Y + _ExtendDist || (FormManager._CursorPosition.X >= FormManager._PrimaryScreenClient.X && FormManager._CursorPosition.X <= FormManager._PrimaryScreenClient.X + _ExtendDist) || (FormManager._CursorPosition.X >= FormManager._PrimaryScreenClient.Right - _ExtendDist && FormManager._CursorPosition.X <= FormManager._PrimaryScreenBounds.Right))) || (Me.FormState == FormState.HighAsScreen && FormManager._CursorPosition.Y > FormManager._PrimaryScreenClient.Y + Me.FormTitleBarHeight + _ExtendDist) || Me.FormState == FormState.QuarterScreen)
                    {
                        if (_CursorPositionOfMe.X >= Me._Bounds_Current_Width - Me._Bounds_Normal_Width / 2)
                        {
                            _CursorPositionOfMe.X = Me._Bounds_Normal_Width - (Me._Bounds_Current_Width - _CursorPositionOfMe.X);
                        }
                        else if (_CursorPositionOfMe.X > Me._Bounds_Normal_Width / 2 && _CursorPositionOfMe.X < Me._Bounds_Current_Width - Me._Bounds_Normal_Width / 2)
                        {
                            _CursorPositionOfMe.X = Me._Bounds_Normal_Width / 2;
                        }

                        if (Me._CanReturn())
                        {
                            Me._ReturnByMoveForm();

                            Me._Bounds_Current_Location = new Point(FormManager._CursorPosition.X - _CursorPositionOfMe.X, FormManager._CursorPosition.Y - _CursorPositionOfMe.Y);

                            Me._UpdateLayout(UpdateLayoutEventType.All);
                        }
                    }
                    else
                    {
                        if (Me.FormState == FormState.Normal)
                        {
                            Me._Bounds_Current_Location = new Point(FormManager._CursorPosition.X - _CursorPositionOfMe.X, FormManager._CursorPosition.Y - _CursorPositionOfMe.Y);

                            _TryToUpdateLayout();
                        }
                        else if (Me.FormState == FormState.HighAsScreen)
                        {
                            Me._Bounds_Current_X = FormManager._CursorPosition.X - _CursorPositionOfMe.X;

                            _TryToUpdateLayout();
                        }
                    }
                }
            }
        }

        //

        private void PictureBox_FormIcon_MouseDown(object sender, MouseEventArgs e) // PictureBox_FormIcon 的 MouseDown 事件的回调函数。
        {
            if (e.Button == MouseButtons.Left)
            {
                ContextMenuStrip_Main.Show(Panel_TitleBar, new Point(0, Panel_FormIcon.Height));
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
            Me.FormClient.Focus();

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
            PictureBox_FullScreen.BackColor = Me.RecommendColors._ControlButton_DEC.ToColor();
        }

        private void PictureBox_FullScreen_MouseLeave(object sender, EventArgs e) // PictureBox_FullScreen 的 MouseLeave 事件的回调函数。
        {
            PictureBox_FullScreen.BackColor = Me.RecommendColors._ControlButton.ToColor();
        }

        private void PictureBox_FullScreen_MouseDown(object sender, MouseEventArgs e) // PictureBox_FullScreen 的 MouseDown 事件的回调函数。
        {
            if (e.Button == MouseButtons.Left)
            {
                PictureBox_FullScreen.BackColor = Me.RecommendColors._ControlButton_INC.ToColor();
            }
        }

        private void PictureBox_FullScreen_MouseUp(object sender, MouseEventArgs e) // PictureBox_FullScreen 的 MouseUp 事件的回调函数。
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Geometry.CursorIsInControl(PictureBox_FullScreen))
                {
                    PictureBox_FullScreen.BackColor = Me.RecommendColors._ControlButton_DEC.ToColor();
                }
                else
                {
                    PictureBox_FullScreen_MouseLeave(sender, e);
                }
            }
        }

        private void PictureBox_Minimize_MouseClick(object sender, MouseEventArgs e) // PictureBox_Minimize 的 MouseClick 事件的回调函数。
        {
            Me.FormClient.Focus();

            if (e.Button == MouseButtons.Left)
            {
                Me.Minimize();
            }
        }

        private void PictureBox_Minimize_MouseEnter(object sender, EventArgs e) // PictureBox_Minimize 的 MouseEnter 事件的回调函数。
        {
            PictureBox_Minimize.BackColor = Me.RecommendColors._ControlButton_DEC.ToColor();
        }

        private void PictureBox_Minimize_MouseLeave(object sender, EventArgs e) // PictureBox_Minimize 的 MouseLeave 事件的回调函数。
        {
            PictureBox_Minimize.BackColor = Me.RecommendColors._ControlButton.ToColor();
        }

        private void PictureBox_Minimize_MouseDown(object sender, MouseEventArgs e) // PictureBox_Minimize 的 MouseDown 事件的回调函数。
        {
            if (e.Button == MouseButtons.Left)
            {
                PictureBox_Minimize.BackColor = Me.RecommendColors._ControlButton_INC.ToColor();
            }
        }

        private void PictureBox_Minimize_MouseUp(object sender, MouseEventArgs e) // PictureBox_Minimize 的 MouseUp 事件的回调函数。
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Geometry.CursorIsInControl(PictureBox_Minimize))
                {
                    PictureBox_Minimize.BackColor = Me.RecommendColors._ControlButton_DEC.ToColor();
                }
                else
                {
                    PictureBox_Minimize_MouseLeave(sender, e);
                }
            }
        }

        private void PictureBox_Maximize_MouseClick(object sender, MouseEventArgs e) // PictureBox_Maximize 的 MouseClick 事件的回调函数。
        {
            Me.FormClient.Focus();

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
            PictureBox_Maximize.BackColor = Me.RecommendColors._ControlButton_DEC.ToColor();
        }

        private void PictureBox_Maximize_MouseLeave(object sender, EventArgs e) // PictureBox_Maximize 的 MouseLeave 事件的回调函数。
        {
            PictureBox_Maximize.BackColor = Me.RecommendColors._ControlButton.ToColor();
        }

        private void PictureBox_Maximize_MouseDown(object sender, MouseEventArgs e) // PictureBox_Maximize 的 MouseDown 事件的回调函数。
        {
            if (e.Button == MouseButtons.Left)
            {
                PictureBox_Maximize.BackColor = Me.RecommendColors._ControlButton_INC.ToColor();
            }
        }

        private void PictureBox_Maximize_MouseUp(object sender, MouseEventArgs e) // PictureBox_Maximize 的 MouseUp 事件的回调函数。
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Geometry.CursorIsInControl(PictureBox_Maximize))
                {
                    PictureBox_Maximize.BackColor = Me.RecommendColors._ControlButton_DEC.ToColor();
                }
                else
                {
                    PictureBox_Maximize_MouseLeave(sender, e);
                }
            }
        }

        private void PictureBox_Exit_MouseClick(object sender, MouseEventArgs e) // PictureBox_Exit 的 MouseClick 事件的回调函数。
        {
            Me.FormClient.Focus();

            if (e.Button == MouseButtons.Left)
            {
                Me.Close();
            }
        }

        private void PictureBox_Exit_MouseEnter(object sender, EventArgs e) // PictureBox_Exit 的 MouseEnter 事件的回调函数。
        {
            PictureBox_Exit.BackColor = Me.RecommendColors._ExitButton_DEC.ToColor();
        }

        private void PictureBox_Exit_MouseLeave(object sender, EventArgs e) // PictureBox_Exit 的 MouseLeave 事件的回调函数。
        {
            PictureBox_Exit.BackColor = Me.RecommendColors._ExitButton.ToColor();
        }

        private void PictureBox_Exit_MouseDown(object sender, MouseEventArgs e) // PictureBox_Exit 的 MouseDown 事件的回调函数。
        {
            if (e.Button == MouseButtons.Left)
            {
                PictureBox_Exit.BackColor = Me.RecommendColors._ExitButton_INC.ToColor();
            }
        }

        private void PictureBox_Exit_MouseUp(object sender, MouseEventArgs e) // PictureBox_Exit 的 MouseUp 事件的回调函数。
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Geometry.CursorIsInControl(PictureBox_Exit))
                {
                    PictureBox_Exit.BackColor = Me.RecommendColors._ExitButton_DEC.ToColor();
                }
                else
                {
                    PictureBox_Exit_MouseLeave(sender, e);
                }
            }
        }

        //

        private void ContextMenuStrip_Main_VisibleChanged(object sender, EventArgs e) // ContextMenuStrip_Main 的 VisibleChanged 事件的回调函数。
        {
            if (!ContextMenuStrip_Main.Visible)
            {
                Me.FormClient.Focus();
            }
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
            Me._UpdateLayout(UpdateLayoutEventType.Manual);

            _LastUpdateLayout = DateTime.Now;
        }

        #endregion

        #region 构造与析构函数

        public FormTitleBar(FormManager formManager) // 使用 FormManager 对象初始化 FormTitleBar 的新实例。
        {
            InitializeComponent();

            //

            Me = formManager;
        }

        #endregion

        #region 方法

        public void OnLoading() // 在 Loading 事件发生时发生。
        {
            Panel_FormIcon.Visible = Panel_ControlBox.Visible = true;
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

            FormTitleBar_SizeChanged(this, EventArgs.Empty);
        }

        public void OnFormStateChanged() // 在 FormStateChanged 事件发生时发生。
        {
            _UpdateForStyleOrStateChanged();

            _UpdateControlBoxImage();

            //

            FormTitleBar_SizeChanged(this, EventArgs.Empty);
        }

        public void OnCaptionChanged() // 在 CaptionChanged 事件发生时发生。
        {
            _RepaintFormCaptionBitmap();
        }

        public void OnThemeChanged() // 在 ThemeChanged 事件发生时发生。
        {
            OnThemeColorChanged();
        }

        public void OnThemeColorChanged() // 在 ThemeColorChanged 事件发生时发生。
        {
            Panel_TitleBar.BackColor = Me.RecommendColors.FormTitleBar.ToColor();

            //

            PictureBox_FullScreen.BackColor = PictureBox_Minimize.BackColor = PictureBox_Maximize.BackColor = Me.RecommendColors._ControlButton.ToColor();
            PictureBox_Exit.BackColor = Me.RecommendColors._ExitButton.ToColor();

            //

            ContextMenuStrip_Main.BackColor = Me.RecommendColors.MenuItemBackground.ToColor();
            ToolStripMenuItem_Return.ForeColor = ToolStripMenuItem_Minimize.ForeColor = ToolStripMenuItem_Maximize.ForeColor = ToolStripMenuItem_Exit.ForeColor = Me.RecommendColors.MenuItemText.ToColor();

            //

            _UpdateControlBoxImage();

            _RepaintFormCaptionBitmap();
        }

        #endregion
    }
}
