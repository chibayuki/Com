namespace Com.WinForm
{
    partial class CaptionBar
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CaptionBar));
            this.ImageList_ControlBox_LightImage = new System.Windows.Forms.ImageList(this.components);
            this.ContextMenuStrip_Main = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItem_Return = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_Minimize = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_Maximize = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator_Main = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripMenuItem_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.ImageList_ControlBox_DarkImage = new System.Windows.Forms.ImageList(this.components);
            this.Panel_CaptionBar = new System.Windows.Forms.Panel();
            this.Panel_FormIcon = new System.Windows.Forms.Panel();
            this.PictureBox_FormIcon = new System.Windows.Forms.PictureBox();
            this.Panel_ControlBox = new System.Windows.Forms.Panel();
            this.PictureBox_FullScreen = new System.Windows.Forms.PictureBox();
            this.PictureBox_Minimize = new System.Windows.Forms.PictureBox();
            this.PictureBox_Maximize = new System.Windows.Forms.PictureBox();
            this.PictureBox_Exit = new System.Windows.Forms.PictureBox();
            this.ToolTip_ControlBox = new System.Windows.Forms.ToolTip(this.components);
            this.BackgroundWorker_UpdateLayoutDelay = new System.ComponentModel.BackgroundWorker();
            this.ContextMenuStrip_Main.SuspendLayout();
            this.Panel_CaptionBar.SuspendLayout();
            this.Panel_FormIcon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox_FormIcon)).BeginInit();
            this.Panel_ControlBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox_FullScreen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox_Minimize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox_Maximize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox_Exit)).BeginInit();
            this.SuspendLayout();
            // 
            // ImageList_ControlBox_LightImage
            // 
            this.ImageList_ControlBox_LightImage.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImageList_ControlBox_LightImage.ImageStream")));
            this.ImageList_ControlBox_LightImage.TransparentColor = System.Drawing.Color.Transparent;
            this.ImageList_ControlBox_LightImage.Images.SetKeyName(0, "ControlBox_LightImage_Minimize_16.png");
            this.ImageList_ControlBox_LightImage.Images.SetKeyName(1, "ControlBox_LightImage_Maximize_16.png");
            this.ImageList_ControlBox_LightImage.Images.SetKeyName(2, "ControlBox_LightImage_Return_16.png");
            this.ImageList_ControlBox_LightImage.Images.SetKeyName(3, "ControlBox_LightImage_Exit_Normal_16.png");
            this.ImageList_ControlBox_LightImage.Images.SetKeyName(4, "ControlBox_LightImage_Exit_MouseOver_16.png");
            this.ImageList_ControlBox_LightImage.Images.SetKeyName(5, "ControlBox_LightImage_EnterFullScreen_16.png");
            this.ImageList_ControlBox_LightImage.Images.SetKeyName(6, "ControlBox_LightImage_ExitFullScreen_16.png");
            // 
            // ContextMenuStrip_Main
            // 
            this.ContextMenuStrip_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_Return,
            this.ToolStripMenuItem_Minimize,
            this.ToolStripMenuItem_Maximize,
            this.ToolStripSeparator_Main,
            this.ToolStripMenuItem_Exit});
            this.ContextMenuStrip_Main.Name = "ContextMenuStrip_Main";
            this.ContextMenuStrip_Main.Size = new System.Drawing.Size(184, 108);
            this.ContextMenuStrip_Main.VisibleChanged += new System.EventHandler(this.ContextMenuStrip_Main_VisibleChanged);
            // 
            // ToolStripMenuItem_Return
            // 
            this.ToolStripMenuItem_Return.BackColor = System.Drawing.Color.Transparent;
            this.ToolStripMenuItem_Return.Enabled = false;
            this.ToolStripMenuItem_Return.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.ToolStripMenuItem_Return.Image = global::Com.Properties.Resources.MenuItem_Return_16;
            this.ToolStripMenuItem_Return.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.ToolStripMenuItem_Return.Name = "ToolStripMenuItem_Return";
            this.ToolStripMenuItem_Return.Size = new System.Drawing.Size(183, 22);
            this.ToolStripMenuItem_Return.Text = "还原(&R)";
            this.ToolStripMenuItem_Return.Click += new System.EventHandler(this.ToolStripMenuItem_Return_Click);
            // 
            // ToolStripMenuItem_Minimize
            // 
            this.ToolStripMenuItem_Minimize.BackColor = System.Drawing.Color.Transparent;
            this.ToolStripMenuItem_Minimize.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ToolStripMenuItem_Minimize.Image = global::Com.Properties.Resources.MenuItem_Minimize_16;
            this.ToolStripMenuItem_Minimize.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.ToolStripMenuItem_Minimize.Name = "ToolStripMenuItem_Minimize";
            this.ToolStripMenuItem_Minimize.Size = new System.Drawing.Size(183, 22);
            this.ToolStripMenuItem_Minimize.Text = "最小化(&N)";
            this.ToolStripMenuItem_Minimize.Click += new System.EventHandler(this.ToolStripMenuItem_Minimize_Click);
            // 
            // ToolStripMenuItem_Maximize
            // 
            this.ToolStripMenuItem_Maximize.BackColor = System.Drawing.Color.Transparent;
            this.ToolStripMenuItem_Maximize.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.ToolStripMenuItem_Maximize.Image = global::Com.Properties.Resources.MenuItem_Maximize_16;
            this.ToolStripMenuItem_Maximize.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.ToolStripMenuItem_Maximize.Name = "ToolStripMenuItem_Maximize";
            this.ToolStripMenuItem_Maximize.Size = new System.Drawing.Size(183, 22);
            this.ToolStripMenuItem_Maximize.Text = "最大化(&X)";
            this.ToolStripMenuItem_Maximize.Click += new System.EventHandler(this.ToolStripMenuItem_Maximize_Click);
            // 
            // ToolStripSeparator_Main
            // 
            this.ToolStripSeparator_Main.BackColor = System.Drawing.Color.Transparent;
            this.ToolStripSeparator_Main.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.ToolStripSeparator_Main.Name = "ToolStripSeparator_Main";
            this.ToolStripSeparator_Main.Size = new System.Drawing.Size(180, 6);
            // 
            // ToolStripMenuItem_Exit
            // 
            this.ToolStripMenuItem_Exit.BackColor = System.Drawing.Color.Transparent;
            this.ToolStripMenuItem_Exit.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ToolStripMenuItem_Exit.Image = global::Com.Properties.Resources.MenuItem_Exit_16;
            this.ToolStripMenuItem_Exit.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.ToolStripMenuItem_Exit.Name = "ToolStripMenuItem_Exit";
            this.ToolStripMenuItem_Exit.ShortcutKeyDisplayString = "    Alt+F4";
            this.ToolStripMenuItem_Exit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.ToolStripMenuItem_Exit.Size = new System.Drawing.Size(183, 22);
            this.ToolStripMenuItem_Exit.Text = "关闭(&C)";
            this.ToolStripMenuItem_Exit.Click += new System.EventHandler(this.ToolStripMenuItem_Exit_Click);
            // 
            // ImageList_ControlBox_DarkImage
            // 
            this.ImageList_ControlBox_DarkImage.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImageList_ControlBox_DarkImage.ImageStream")));
            this.ImageList_ControlBox_DarkImage.TransparentColor = System.Drawing.Color.Transparent;
            this.ImageList_ControlBox_DarkImage.Images.SetKeyName(0, "ControlBox_DarkImage_Minimize_16.png");
            this.ImageList_ControlBox_DarkImage.Images.SetKeyName(1, "ControlBox_DarkImage_Maximize_16.png");
            this.ImageList_ControlBox_DarkImage.Images.SetKeyName(2, "ControlBox_DarkImage_Return_16.png");
            this.ImageList_ControlBox_DarkImage.Images.SetKeyName(3, "ControlBox_DarkImage_Exit_Normal_16.png");
            this.ImageList_ControlBox_DarkImage.Images.SetKeyName(4, "ControlBox_DarkImage_Exit_MouseOver_16.png");
            this.ImageList_ControlBox_DarkImage.Images.SetKeyName(5, "ControlBox_DarkImage_EnterFullScreen_16.png");
            this.ImageList_ControlBox_DarkImage.Images.SetKeyName(6, "ControlBox_DarkImage_ExitFullScreen_16.png");
            // 
            // Panel_CaptionBar
            // 
            this.Panel_CaptionBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Panel_CaptionBar.ContextMenuStrip = this.ContextMenuStrip_Main;
            this.Panel_CaptionBar.Controls.Add(this.Panel_FormIcon);
            this.Panel_CaptionBar.Controls.Add(this.Panel_ControlBox);
            this.Panel_CaptionBar.Location = new System.Drawing.Point(0, 0);
            this.Panel_CaptionBar.Name = "Panel_CaptionBar";
            this.Panel_CaptionBar.Size = new System.Drawing.Size(300, 32);
            this.Panel_CaptionBar.TabIndex = 0;
            this.Panel_CaptionBar.Paint += new System.Windows.Forms.PaintEventHandler(this.Panel_CaptionBar_Paint);
            this.Panel_CaptionBar.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Panel_CaptionBar_MouseDoubleClick);
            this.Panel_CaptionBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Panel_CaptionBar_MouseDown);
            this.Panel_CaptionBar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Panel_CaptionBar_MouseMove);
            this.Panel_CaptionBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Panel_CaptionBar_MouseUp);
            // 
            // Panel_FormIcon
            // 
            this.Panel_FormIcon.BackColor = System.Drawing.Color.Transparent;
            this.Panel_FormIcon.Controls.Add(this.PictureBox_FormIcon);
            this.Panel_FormIcon.Location = new System.Drawing.Point(0, 0);
            this.Panel_FormIcon.Name = "Panel_FormIcon";
            this.Panel_FormIcon.Size = new System.Drawing.Size(32, 32);
            this.Panel_FormIcon.TabIndex = 0;
            this.Panel_FormIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Panel_FormIcon_MouseDoubleClick);
            this.Panel_FormIcon.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Panel_FormIcon_MouseDown);
            // 
            // PictureBox_FormIcon
            // 
            this.PictureBox_FormIcon.BackColor = System.Drawing.Color.Transparent;
            this.PictureBox_FormIcon.ErrorImage = null;
            this.PictureBox_FormIcon.InitialImage = null;
            this.PictureBox_FormIcon.Location = new System.Drawing.Point(4, 4);
            this.PictureBox_FormIcon.Name = "PictureBox_FormIcon";
            this.PictureBox_FormIcon.Size = new System.Drawing.Size(24, 24);
            this.PictureBox_FormIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PictureBox_FormIcon.TabIndex = 0;
            this.PictureBox_FormIcon.TabStop = false;
            this.PictureBox_FormIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.PictureBox_FormIcon_MouseDoubleClick);
            this.PictureBox_FormIcon.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PictureBox_FormIcon_MouseDown);
            // 
            // Panel_ControlBox
            // 
            this.Panel_ControlBox.BackColor = System.Drawing.Color.Transparent;
            this.Panel_ControlBox.Controls.Add(this.PictureBox_FullScreen);
            this.Panel_ControlBox.Controls.Add(this.PictureBox_Minimize);
            this.Panel_ControlBox.Controls.Add(this.PictureBox_Maximize);
            this.Panel_ControlBox.Controls.Add(this.PictureBox_Exit);
            this.Panel_ControlBox.Location = new System.Drawing.Point(116, 0);
            this.Panel_ControlBox.Name = "Panel_ControlBox";
            this.Panel_ControlBox.Size = new System.Drawing.Size(184, 32);
            this.Panel_ControlBox.TabIndex = 0;
            // 
            // PictureBox_FullScreen
            // 
            this.PictureBox_FullScreen.BackColor = System.Drawing.Color.Transparent;
            this.PictureBox_FullScreen.ErrorImage = null;
            this.PictureBox_FullScreen.Image = global::Com.Properties.Resources.ControlBox_LightImage_EnterFullScreen_16;
            this.PictureBox_FullScreen.InitialImage = null;
            this.PictureBox_FullScreen.Location = new System.Drawing.Point(0, 0);
            this.PictureBox_FullScreen.Name = "PictureBox_FullScreen";
            this.PictureBox_FullScreen.Size = new System.Drawing.Size(46, 32);
            this.PictureBox_FullScreen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.PictureBox_FullScreen.TabIndex = 0;
            this.PictureBox_FullScreen.TabStop = false;
            this.ToolTip_ControlBox.SetToolTip(this.PictureBox_FullScreen, "全屏");
            this.PictureBox_FullScreen.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PictureBox_FullScreen_MouseClick);
            this.PictureBox_FullScreen.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PictureBox_FullScreen_MouseDown);
            this.PictureBox_FullScreen.MouseEnter += new System.EventHandler(this.PictureBox_FullScreen_MouseEnter);
            this.PictureBox_FullScreen.MouseLeave += new System.EventHandler(this.PictureBox_FullScreen_MouseLeave);
            this.PictureBox_FullScreen.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PictureBox_FullScreen_MouseUp);
            // 
            // PictureBox_Minimize
            // 
            this.PictureBox_Minimize.BackColor = System.Drawing.Color.Transparent;
            this.PictureBox_Minimize.ErrorImage = null;
            this.PictureBox_Minimize.Image = global::Com.Properties.Resources.ControlBox_LightImage_Minimize_16;
            this.PictureBox_Minimize.InitialImage = null;
            this.PictureBox_Minimize.Location = new System.Drawing.Point(46, 0);
            this.PictureBox_Minimize.Name = "PictureBox_Minimize";
            this.PictureBox_Minimize.Size = new System.Drawing.Size(46, 32);
            this.PictureBox_Minimize.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.PictureBox_Minimize.TabIndex = 0;
            this.PictureBox_Minimize.TabStop = false;
            this.ToolTip_ControlBox.SetToolTip(this.PictureBox_Minimize, "最小化");
            this.PictureBox_Minimize.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PictureBox_Minimize_MouseClick);
            this.PictureBox_Minimize.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PictureBox_Minimize_MouseDown);
            this.PictureBox_Minimize.MouseEnter += new System.EventHandler(this.PictureBox_Minimize_MouseEnter);
            this.PictureBox_Minimize.MouseLeave += new System.EventHandler(this.PictureBox_Minimize_MouseLeave);
            this.PictureBox_Minimize.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PictureBox_Minimize_MouseUp);
            // 
            // PictureBox_Maximize
            // 
            this.PictureBox_Maximize.BackColor = System.Drawing.Color.Transparent;
            this.PictureBox_Maximize.ErrorImage = null;
            this.PictureBox_Maximize.Image = global::Com.Properties.Resources.ControlBox_LightImage_Maximize_16;
            this.PictureBox_Maximize.InitialImage = null;
            this.PictureBox_Maximize.Location = new System.Drawing.Point(92, 0);
            this.PictureBox_Maximize.Name = "PictureBox_Maximize";
            this.PictureBox_Maximize.Size = new System.Drawing.Size(46, 32);
            this.PictureBox_Maximize.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.PictureBox_Maximize.TabIndex = 0;
            this.PictureBox_Maximize.TabStop = false;
            this.ToolTip_ControlBox.SetToolTip(this.PictureBox_Maximize, "最大化");
            this.PictureBox_Maximize.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PictureBox_Maximize_MouseClick);
            this.PictureBox_Maximize.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PictureBox_Maximize_MouseDown);
            this.PictureBox_Maximize.MouseEnter += new System.EventHandler(this.PictureBox_Maximize_MouseEnter);
            this.PictureBox_Maximize.MouseLeave += new System.EventHandler(this.PictureBox_Maximize_MouseLeave);
            this.PictureBox_Maximize.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PictureBox_Maximize_MouseUp);
            // 
            // PictureBox_Exit
            // 
            this.PictureBox_Exit.BackColor = System.Drawing.Color.Transparent;
            this.PictureBox_Exit.ErrorImage = null;
            this.PictureBox_Exit.Image = global::Com.Properties.Resources.ControlBox_LightImage_Exit_Normal_16;
            this.PictureBox_Exit.InitialImage = null;
            this.PictureBox_Exit.Location = new System.Drawing.Point(138, 0);
            this.PictureBox_Exit.Name = "PictureBox_Exit";
            this.PictureBox_Exit.Size = new System.Drawing.Size(46, 32);
            this.PictureBox_Exit.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.PictureBox_Exit.TabIndex = 0;
            this.PictureBox_Exit.TabStop = false;
            this.ToolTip_ControlBox.SetToolTip(this.PictureBox_Exit, "关闭");
            this.PictureBox_Exit.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PictureBox_Exit_MouseClick);
            this.PictureBox_Exit.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PictureBox_Exit_MouseDown);
            this.PictureBox_Exit.MouseEnter += new System.EventHandler(this.PictureBox_Exit_MouseEnter);
            this.PictureBox_Exit.MouseLeave += new System.EventHandler(this.PictureBox_Exit_MouseLeave);
            this.PictureBox_Exit.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PictureBox_Exit_MouseUp);
            // 
            // ToolTip_ControlBox
            // 
            this.ToolTip_ControlBox.ShowAlways = true;
            // 
            // BackgroundWorker_UpdateLayoutDelay
            // 
            this.BackgroundWorker_UpdateLayoutDelay.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker_UpdateLayoutDelay_DoWork);
            this.BackgroundWorker_UpdateLayoutDelay.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorker_UpdateLayoutDelay_RunWorkerCompleted);
            // 
            // CaptionBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(300, 300);
            this.Controls.Add(this.Panel_CaptionBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CaptionBar";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Load += new System.EventHandler(this.CaptionBar_Load);
            this.LocationChanged += new System.EventHandler(this.CaptionBar_LocationChanged);
            this.SizeChanged += new System.EventHandler(this.CaptionBar_SizeChanged);
            this.ContextMenuStrip_Main.ResumeLayout(false);
            this.Panel_CaptionBar.ResumeLayout(false);
            this.Panel_FormIcon.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox_FormIcon)).EndInit();
            this.Panel_ControlBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox_FullScreen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox_Minimize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox_Maximize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox_Exit)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ImageList ImageList_ControlBox_LightImage;
        private System.Windows.Forms.ContextMenuStrip ContextMenuStrip_Main;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Return;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Minimize;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Maximize;
        private System.Windows.Forms.ToolStripSeparator ToolStripSeparator_Main;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Exit;
        private System.Windows.Forms.ImageList ImageList_ControlBox_DarkImage;
        private System.Windows.Forms.Panel Panel_CaptionBar;
        private System.Windows.Forms.Panel Panel_ControlBox;
        private System.Windows.Forms.Panel Panel_FormIcon;
        private System.Windows.Forms.PictureBox PictureBox_FormIcon;
        private System.Windows.Forms.ToolTip ToolTip_ControlBox;
        private System.ComponentModel.BackgroundWorker BackgroundWorker_UpdateLayoutDelay;
        private System.Windows.Forms.PictureBox PictureBox_FullScreen;
        private System.Windows.Forms.PictureBox PictureBox_Minimize;
        private System.Windows.Forms.PictureBox PictureBox_Exit;
        private System.Windows.Forms.PictureBox PictureBox_Maximize;
    }
}