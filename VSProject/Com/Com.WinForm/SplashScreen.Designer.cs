namespace Com.WinForm
{
    partial class SplashScreen
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.Panel_SplashScreen = new System.Windows.Forms.Panel();
            this.Panel_Splash = new System.Windows.Forms.Panel();
            this.PictureBox_AppLogo = new System.Windows.Forms.PictureBox();
            this.Panel_SplashScreen.SuspendLayout();
            this.Panel_Splash.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox_AppLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // Panel_SplashScreen
            // 
            this.Panel_SplashScreen.BackColor = System.Drawing.Color.Transparent;
            this.Panel_SplashScreen.Controls.Add(this.Panel_Splash);
            this.Panel_SplashScreen.Location = new System.Drawing.Point(0, 0);
            this.Panel_SplashScreen.Name = "Panel_SplashScreen";
            this.Panel_SplashScreen.Size = new System.Drawing.Size(300, 300);
            this.Panel_SplashScreen.TabIndex = 0;
            // 
            // Panel_Splash
            // 
            this.Panel_Splash.BackColor = System.Drawing.Color.Transparent;
            this.Panel_Splash.Controls.Add(this.PictureBox_AppLogo);
            this.Panel_Splash.Location = new System.Drawing.Point(50, 125);
            this.Panel_Splash.Name = "Panel_Splash";
            this.Panel_Splash.Size = new System.Drawing.Size(200, 50);
            this.Panel_Splash.TabIndex = 0;
            this.Panel_Splash.Paint += new System.Windows.Forms.PaintEventHandler(this.Panel_Splash_Paint);
            // 
            // PictureBox_AppLogo
            // 
            this.PictureBox_AppLogo.BackColor = System.Drawing.Color.Transparent;
            this.PictureBox_AppLogo.ErrorImage = null;
            this.PictureBox_AppLogo.InitialImage = null;
            this.PictureBox_AppLogo.Location = new System.Drawing.Point(0, 0);
            this.PictureBox_AppLogo.Name = "PictureBox_AppLogo";
            this.PictureBox_AppLogo.Size = new System.Drawing.Size(50, 50);
            this.PictureBox_AppLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PictureBox_AppLogo.TabIndex = 5;
            this.PictureBox_AppLogo.TabStop = false;
            // 
            // SplashScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(300, 300);
            this.Controls.Add(this.Panel_SplashScreen);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SplashScreen";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Load += new System.EventHandler(this.SplashScreen_Load);
            this.SizeChanged += new System.EventHandler(this.SplashScreen_SizeChanged);
            this.Panel_SplashScreen.ResumeLayout(false);
            this.Panel_Splash.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox_AppLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel Panel_SplashScreen;
        private System.Windows.Forms.Panel Panel_Splash;
        private System.Windows.Forms.PictureBox PictureBox_AppLogo;
    }
}