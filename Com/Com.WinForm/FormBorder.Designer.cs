namespace Com.WinForm
{
    partial class FormBorder
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
            this.Label_Left = new System.Windows.Forms.Label();
            this.Label_Bottom = new System.Windows.Forms.Label();
            this.Label_BottomRight = new System.Windows.Forms.Label();
            this.Label_BottomLeft = new System.Windows.Forms.Label();
            this.Label_Top = new System.Windows.Forms.Label();
            this.Label_TopRight = new System.Windows.Forms.Label();
            this.Label_Right = new System.Windows.Forms.Label();
            this.Label_TopLeft = new System.Windows.Forms.Label();
            this.Panel_FormBounds = new System.Windows.Forms.Panel();
            this.Panel_Border = new System.Windows.Forms.Panel();
            this.BackgroundWorker_UpdateLayoutDelay = new System.ComponentModel.BackgroundWorker();
            this.Panel_Border.SuspendLayout();
            this.SuspendLayout();
            // 
            // Label_Left
            // 
            this.Label_Left.BackColor = System.Drawing.Color.Gray;
            this.Label_Left.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.Label_Left.Location = new System.Drawing.Point(0, 20);
            this.Label_Left.Name = "Label_Left";
            this.Label_Left.Size = new System.Drawing.Size(5, 260);
            this.Label_Left.TabIndex = 0;
            this.Label_Left.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Label_Left_MouseDown);
            this.Label_Left.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Label_Left_MouseMove);
            this.Label_Left.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Label_Left_MouseUp);
            // 
            // Label_Bottom
            // 
            this.Label_Bottom.BackColor = System.Drawing.Color.Gray;
            this.Label_Bottom.Cursor = System.Windows.Forms.Cursors.SizeNS;
            this.Label_Bottom.Location = new System.Drawing.Point(20, 295);
            this.Label_Bottom.Name = "Label_Bottom";
            this.Label_Bottom.Size = new System.Drawing.Size(260, 5);
            this.Label_Bottom.TabIndex = 0;
            this.Label_Bottom.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Label_Bottom_MouseDoubleClick);
            this.Label_Bottom.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Label_Bottom_MouseDown);
            this.Label_Bottom.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Label_Bottom_MouseMove);
            this.Label_Bottom.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Label_Bottom_MouseUp);
            // 
            // Label_BottomRight
            // 
            this.Label_BottomRight.BackColor = System.Drawing.Color.Silver;
            this.Label_BottomRight.Cursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.Label_BottomRight.Location = new System.Drawing.Point(280, 280);
            this.Label_BottomRight.Name = "Label_BottomRight";
            this.Label_BottomRight.Size = new System.Drawing.Size(20, 20);
            this.Label_BottomRight.TabIndex = 0;
            this.Label_BottomRight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Label_BottomRight_MouseDown);
            this.Label_BottomRight.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Label_BottomRight_MouseMove);
            this.Label_BottomRight.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Label_BottomRight_MouseUp);
            // 
            // Label_BottomLeft
            // 
            this.Label_BottomLeft.BackColor = System.Drawing.Color.Silver;
            this.Label_BottomLeft.Cursor = System.Windows.Forms.Cursors.SizeNESW;
            this.Label_BottomLeft.Location = new System.Drawing.Point(0, 280);
            this.Label_BottomLeft.Name = "Label_BottomLeft";
            this.Label_BottomLeft.Size = new System.Drawing.Size(20, 20);
            this.Label_BottomLeft.TabIndex = 0;
            this.Label_BottomLeft.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Label_BottomLeft_MouseDown);
            this.Label_BottomLeft.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Label_BottomLeft_MouseMove);
            this.Label_BottomLeft.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Label_BottomLeft_MouseUp);
            // 
            // Label_Top
            // 
            this.Label_Top.BackColor = System.Drawing.Color.Gray;
            this.Label_Top.Cursor = System.Windows.Forms.Cursors.SizeNS;
            this.Label_Top.Location = new System.Drawing.Point(20, 0);
            this.Label_Top.Name = "Label_Top";
            this.Label_Top.Size = new System.Drawing.Size(260, 5);
            this.Label_Top.TabIndex = 0;
            this.Label_Top.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Label_Top_MouseDoubleClick);
            this.Label_Top.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Label_Top_MouseDown);
            this.Label_Top.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Label_Top_MouseMove);
            this.Label_Top.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Label_Top_MouseUp);
            // 
            // Label_TopRight
            // 
            this.Label_TopRight.BackColor = System.Drawing.Color.Silver;
            this.Label_TopRight.Cursor = System.Windows.Forms.Cursors.SizeNESW;
            this.Label_TopRight.Location = new System.Drawing.Point(280, 0);
            this.Label_TopRight.Name = "Label_TopRight";
            this.Label_TopRight.Size = new System.Drawing.Size(20, 20);
            this.Label_TopRight.TabIndex = 0;
            this.Label_TopRight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Label_TopRight_MouseDown);
            this.Label_TopRight.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Label_TopRight_MouseMove);
            this.Label_TopRight.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Label_TopRight_MouseUp);
            // 
            // Label_Right
            // 
            this.Label_Right.BackColor = System.Drawing.Color.Gray;
            this.Label_Right.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.Label_Right.Location = new System.Drawing.Point(295, 20);
            this.Label_Right.Name = "Label_Right";
            this.Label_Right.Size = new System.Drawing.Size(5, 260);
            this.Label_Right.TabIndex = 0;
            this.Label_Right.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Label_Right_MouseDown);
            this.Label_Right.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Label_Right_MouseMove);
            this.Label_Right.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Label_Right_MouseUp);
            // 
            // Label_TopLeft
            // 
            this.Label_TopLeft.BackColor = System.Drawing.Color.Silver;
            this.Label_TopLeft.Cursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.Label_TopLeft.Location = new System.Drawing.Point(0, 0);
            this.Label_TopLeft.Name = "Label_TopLeft";
            this.Label_TopLeft.Size = new System.Drawing.Size(20, 20);
            this.Label_TopLeft.TabIndex = 0;
            this.Label_TopLeft.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Label_TopLeft_MouseDown);
            this.Label_TopLeft.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Label_TopLeft_MouseMove);
            this.Label_TopLeft.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Label_TopLeft_MouseUp);
            // 
            // Panel_FormBounds
            // 
            this.Panel_FormBounds.BackColor = System.Drawing.Color.Transparent;
            this.Panel_FormBounds.Location = new System.Drawing.Point(5, 5);
            this.Panel_FormBounds.Name = "Panel_FormBounds";
            this.Panel_FormBounds.Size = new System.Drawing.Size(290, 290);
            this.Panel_FormBounds.TabIndex = 0;
            // 
            // Panel_Border
            // 
            this.Panel_Border.BackColor = System.Drawing.Color.Transparent;
            this.Panel_Border.Controls.Add(this.Panel_FormBounds);
            this.Panel_Border.Controls.Add(this.Label_BottomRight);
            this.Panel_Border.Controls.Add(this.Label_BottomLeft);
            this.Panel_Border.Controls.Add(this.Label_Bottom);
            this.Panel_Border.Controls.Add(this.Label_TopRight);
            this.Panel_Border.Controls.Add(this.Label_TopLeft);
            this.Panel_Border.Controls.Add(this.Label_Top);
            this.Panel_Border.Controls.Add(this.Label_Right);
            this.Panel_Border.Controls.Add(this.Label_Left);
            this.Panel_Border.Location = new System.Drawing.Point(0, 0);
            this.Panel_Border.Name = "Panel_Border";
            this.Panel_Border.Size = new System.Drawing.Size(300, 300);
            this.Panel_Border.TabIndex = 0;
            this.Panel_Border.Paint += new System.Windows.Forms.PaintEventHandler(this.Panel_Border_Paint);
            // 
            // BackgroundWorker_UpdateLayoutDelay
            // 
            this.BackgroundWorker_UpdateLayoutDelay.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker_UpdateLayoutDelay_DoWork);
            this.BackgroundWorker_UpdateLayoutDelay.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorker_UpdateLayoutDelay_RunWorkerCompleted);
            // 
            // FormBorder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(1)))), ((int)(((byte)(1)))));
            this.ClientSize = new System.Drawing.Size(300, 300);
            this.Controls.Add(this.Panel_Border);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormBorder";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Load += new System.EventHandler(this.FormBorder_Load);
            this.SizeChanged += new System.EventHandler(this.FormBorder_SizeChanged);
            this.Panel_Border.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label Label_Left;
        private System.Windows.Forms.Label Label_Bottom;
        private System.Windows.Forms.Label Label_BottomRight;
        private System.Windows.Forms.Label Label_BottomLeft;
        private System.Windows.Forms.Label Label_Top;
        private System.Windows.Forms.Label Label_TopRight;
        private System.Windows.Forms.Label Label_Right;
        private System.Windows.Forms.Label Label_TopLeft;
        private System.Windows.Forms.Panel Panel_FormBounds;
        private System.Windows.Forms.Panel Panel_Border;
        private System.ComponentModel.BackgroundWorker BackgroundWorker_UpdateLayoutDelay;
    }
}