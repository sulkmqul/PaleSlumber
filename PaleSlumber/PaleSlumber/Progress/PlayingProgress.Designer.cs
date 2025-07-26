namespace PaleSlumber
{
    partial class PlayingProgress
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            labelProgress = new Label();
            panel1 = new Panel();
            pictureBoxProgressBar = new PictureBox();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxProgressBar).BeginInit();
            SuspendLayout();
            // 
            // labelProgress
            // 
            labelProgress.Dock = DockStyle.Left;
            labelProgress.Location = new Point(0, 0);
            labelProgress.Name = "labelProgress";
            labelProgress.Size = new Size(105, 15);
            labelProgress.TabIndex = 0;
            labelProgress.Text = "00:00:00 / 99:99:99";
            labelProgress.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            panel1.Controls.Add(pictureBoxProgressBar);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(105, 0);
            panel1.Margin = new Padding(0);
            panel1.Name = "panel1";
            panel1.Size = new Size(281, 15);
            panel1.TabIndex = 1;
            // 
            // pictureBoxProgressBar
            // 
            pictureBoxProgressBar.Dock = DockStyle.Fill;
            pictureBoxProgressBar.Location = new Point(0, 0);
            pictureBoxProgressBar.Name = "pictureBoxProgressBar";
            pictureBoxProgressBar.Size = new Size(281, 15);
            pictureBoxProgressBar.TabIndex = 2;
            pictureBoxProgressBar.TabStop = false;
            pictureBoxProgressBar.Paint += pictureBoxProgressBar_Paint;
            pictureBoxProgressBar.MouseDown += pictureBoxProgressBar_MouseDown;
            pictureBoxProgressBar.MouseMove += pictureBoxProgressBar_MouseMove;
            pictureBoxProgressBar.MouseUp += pictureBoxProgressBar_MouseUp;
            // 
            // PlayingProgress
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panel1);
            Controls.Add(labelProgress);
            Name = "PlayingProgress";
            Size = new Size(386, 15);
            Load += PlayingProgress_Load;
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxProgressBar).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Label labelProgress;
        private Panel panel1;
        private PictureBox pictureBoxProgressBar;
    }
}
