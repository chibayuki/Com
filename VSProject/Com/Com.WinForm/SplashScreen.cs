/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2024 chibayuki@foxmail.com

Com.WinForm.SplashScreen
Version 20.10.27.1900

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

namespace Com.WinForm
{
    internal partial class SplashScreen : Form // 启动屏幕。
    {
        #region 非公开成员

        private FormManager Me; // 窗口管理器。

        //

        private Bitmap _FormSplashBitmap; // 启动屏幕绘图。

        // 更新启动屏幕绘图。
        private void _UpdateSplashBitmap()
        {
            Size SplashSize = new Size(Panel_SplashScreen.Width, Math.Max(24, Math.Min(Panel_SplashScreen.Height, 64)));

            RectangleF AppNameRect = new RectangleF();
            AppNameRect.Size = new SizeF(Math.Max(SplashSize.Height * 1.4F, Panel_SplashScreen.Width - SplashSize.Height * 1.4F), SplashSize.Height * 0.7F);

            string AppName = Application.ProductName;

            Font AppNameFont = Com.Text.GetSuitableFont(AppName, new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, 134), AppNameRect.Size);

            AppNameRect.Size = TextRenderer.MeasureText(AppName, AppNameFont);

            int AppLogoSize = Math.Max(24, (int)(AppNameRect.Height / 0.7F));

            AppNameRect.Location = new PointF(AppLogoSize * 1.4F, (SplashSize.Height - AppNameRect.Height) / 2);

            SplashSize.Width = (int)AppNameRect.Right;

            Panel_Splash.Size = SplashSize;
            Panel_Splash.Location = new Point((Panel_SplashScreen.Width - Panel_Splash.Width) / 2, (Panel_SplashScreen.Height - Panel_Splash.Height) / 2);

            PictureBox_AppLogo.Size = new Size(AppLogoSize, AppLogoSize);
            PictureBox_AppLogo.Location = new Point(0, (Panel_Splash.Height - PictureBox_AppLogo.Height) / 2);

            //

            if (!(_FormSplashBitmap is null))
            {
                _FormSplashBitmap.Dispose();
            }

            _FormSplashBitmap = new Bitmap(Math.Max(1, Panel_Splash.Width), Math.Max(1, Panel_Splash.Height));

            using (Graphics CreateSplashBmp = Graphics.FromImage(_FormSplashBitmap))
            {
                CreateSplashBmp.Clear(Me.RecommendColors.CaptionBar.ToColor());

                CreateSplashBmp.DrawString(AppName, AppNameFont, new SolidBrush(Me.RecommendColors.Caption.ToColor()), AppNameRect.Location);
            }
        }

        // 更新并重绘启动屏幕绘图。
        private void _RepaintSplashBitmap()
        {
            if (Panel_SplashScreen.Visible)
            {
                _UpdateSplashBitmap();

                if (!(_FormSplashBitmap is null))
                {
                    Panel_Splash.CreateGraphics().DrawImage(_FormSplashBitmap, new Point(0, 0));
                }
            }
        }

        #endregion

        #region 回调函数

        // SplashScreen 的 Load 事件的回调函数。
        private void SplashScreen_Load(object sender, EventArgs e)
        {
            PictureBox_AppLogo.Image = Me.Client.Icon.ToBitmap();

            //

            SplashScreen_SizeChanged(this, EventArgs.Empty);

            //

            Panel_Splash.Visible = false;
            Panel_SplashScreen.Visible = Me.IsMainForm;
        }

        // SplashScreen 的 SizeChanged 事件的回调函数。
        private void SplashScreen_SizeChanged(object sender, EventArgs e)
        {
            Panel_SplashScreen.Size = this.Size;

            //

            _RepaintSplashBitmap();
        }

        //

        // Panel_Splash 的 Paint 事件的回调函数。
        private void Panel_Splash_Paint(object sender, PaintEventArgs e)
        {
            if (Panel_SplashScreen.Visible)
            {
                if (_FormSplashBitmap is null)
                {
                    _UpdateSplashBitmap();
                }

                if (!(_FormSplashBitmap is null))
                {
                    e.Graphics.DrawImage(_FormSplashBitmap, new Point(0, 0));
                }
            }
        }

        #endregion

        #region 构造函数

        // 使用 FormManager 对象初始化 SplashScreen 的新实例。
        public SplashScreen(FormManager formManager)
        {
            InitializeComponent();

            //

            Me = formManager;
        }

        #endregion

        #region 方法

        // 在 Loading 事件发生时发生。
        public void OnLoading() => Panel_Splash.Visible = true;

        // 在 Closing 事件发生时发生。
        public void OnClosing() => Panel_SplashScreen.Visible = false;

        // 在 ThemeChanged 事件发生时发生。
        public void OnThemeChanged()
        {
            this.BackColor = Me.RecommendColors.FormBackground.ToColor();
            Panel_SplashScreen.BackColor = Me.RecommendColors.CaptionBar.ToColor();

            //

            _RepaintSplashBitmap();
        }

        #endregion
    }
}