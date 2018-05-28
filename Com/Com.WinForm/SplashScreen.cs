/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2013-2018 chibayuki@foxmail.com

Com.WinForm.SplashScreen
Version 18.5.28.0000

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
        #region 私有与内部成员

        private FormManager Me; // 窗口管理器。

        //

        private Bitmap _FormSplashBitmap; // 启动屏幕绘图。

        private void _UpdateSplashBitmap() // 更新启动屏幕绘图。
        {
            if (_FormSplashBitmap != null)
            {
                _FormSplashBitmap.Dispose();
            }

            _FormSplashBitmap = new Bitmap(Math.Max(1, Panel_Splash.Width), Math.Max(1, Panel_Splash.Height));

            using (Graphics CreateSplashBmp = Graphics.FromImage(_FormSplashBitmap))
            {
                CreateSplashBmp.Clear(Me.RecommendColors.CaptionBar.ToColor());

                //

                Size SplashSize = new Size(Panel_SplashScreen.Width, Math.Max(24, Math.Min(Panel_SplashScreen.Height, 64)));

                RectangleF AppNameRect = new RectangleF();
                AppNameRect.Size = new SizeF(Math.Max(SplashSize.Height * 1.4F, Panel_SplashScreen.Width - SplashSize.Height * 1.4F), SplashSize.Height * 0.7F);

                string AppName = Application.ProductName;

                Font AppNameFont = Com.Text.GetSuitableFont(AppName, new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, 134), AppNameRect.Size);

                AppNameRect.Size = TextRenderer.MeasureText(AppName, AppNameFont);

                int AppLogoSize = Math.Max(24, (int)(AppNameRect.Height / 0.7F));

                AppNameRect.Location = new PointF(AppLogoSize * 1.4F, (SplashSize.Height - AppNameRect.Height) / 2);

                SplashSize.Width = (Int32)AppNameRect.Right;

                Panel_Splash.Size = SplashSize;
                Panel_Splash.Location = new Point((Panel_SplashScreen.Width - Panel_Splash.Width) / 2, (Panel_SplashScreen.Height - Panel_Splash.Height) / 2);

                PictureBox_AppLogo.Size = new Size(AppLogoSize, AppLogoSize);
                PictureBox_AppLogo.Location = new Point(0, (Panel_Splash.Height - PictureBox_AppLogo.Height) / 2);

                CreateSplashBmp.DrawString(AppName, AppNameFont, new SolidBrush(Me.RecommendColors.Caption.ToColor()), AppNameRect.Location);
            }
        }

        private void _RepaintSplashBitmap() // 更新并重绘启动屏幕绘图。
        {
            if (Panel_SplashScreen.Visible)
            {
                _UpdateSplashBitmap();

                if (_FormSplashBitmap != null)
                {
                    Panel_Splash.CreateGraphics().DrawImage(_FormSplashBitmap, new Point(0, 0));
                }
            }
        }

        #endregion

        #region 回调函数

        private void SplashScreen_Load(object sender, EventArgs e) // SplashScreen 的 Load 事件的回调函数。
        {
            PictureBox_AppLogo.Image = Me.Client.Icon.ToBitmap();

            //

            SplashScreen_SizeChanged(this, EventArgs.Empty);

            //

            Panel_Splash.Visible = false;
            Panel_SplashScreen.Visible = Me.IsMainForm;
        }

        private void SplashScreen_SizeChanged(object sender, EventArgs e) // SplashScreen 的 SizeChanged 事件的回调函数。
        {
            Panel_SplashScreen.Size = this.Size;

            //

            _RepaintSplashBitmap();
        }

        //

        private void Panel_Splash_Paint(object sender, PaintEventArgs e) // Panel_Splash 的 Paint 事件的回调函数。
        {
            if (Panel_SplashScreen.Visible)
            {
                if (_FormSplashBitmap == null)
                {
                    _UpdateSplashBitmap();
                }

                if (_FormSplashBitmap != null)
                {
                    e.Graphics.DrawImage(_FormSplashBitmap, new Point(0, 0));
                }
            }
        }

        #endregion

        #region 构造函数

        public SplashScreen(FormManager formManager) // 使用 FormManager 对象初始化 SplashScreen 的新实例。
        {
            InitializeComponent();

            //

            Me = formManager;
        }

        #endregion

        #region 方法

        public void OnLoading() // 在 Loading 事件发生时发生。
        {
            Panel_Splash.Visible = true;
        }

        public void OnClosing() // 在 Closing 事件发生时发生。
        {
            Panel_SplashScreen.Visible = false;
        }

        public void OnThemeChanged() // 在 ThemeChanged 事件发生时发生。
        {
            OnThemeColorChanged();
        }

        public void OnThemeColorChanged() // 在 ThemeColorChanged 事件发生时发生。
        {
            this.BackColor = Me.RecommendColors.FormBackground.ToColor();
            Panel_SplashScreen.BackColor = Me.RecommendColors.CaptionBar.ToColor();

            //

            _RepaintSplashBitmap();
        }

        #endregion
    }
}