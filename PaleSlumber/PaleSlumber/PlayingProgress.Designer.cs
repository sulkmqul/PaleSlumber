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
            labelTotalTime = new Label();
            labelPlayingPosition = new Label();
            label1 = new Label();
            hScrollBarPlayingPosition = new HScrollBar();
            SuspendLayout();
            // 
            // labelTotalTime
            // 
            labelTotalTime.AutoSize = true;
            labelTotalTime.Location = new Point(61, 0);
            labelTotalTime.Name = "labelTotalTime";
            labelTotalTime.Size = new Size(34, 15);
            labelTotalTime.TabIndex = 1;
            labelTotalTime.Text = "99:99";
            // 
            // labelPlayingPosition
            // 
            labelPlayingPosition.AutoSize = true;
            labelPlayingPosition.Location = new Point(3, 0);
            labelPlayingPosition.Name = "labelPlayingPosition";
            labelPlayingPosition.Size = new Size(34, 15);
            labelPlayingPosition.TabIndex = 1;
            labelPlayingPosition.Text = "00:00";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(43, 0);
            label1.Name = "label1";
            label1.Size = new Size(12, 15);
            label1.TabIndex = 1;
            label1.Text = "/";
            // 
            // hScrollBarPlayingPosition
            // 
            hScrollBarPlayingPosition.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            hScrollBarPlayingPosition.Location = new Point(98, 0);
            hScrollBarPlayingPosition.Maximum = 300;
            hScrollBarPlayingPosition.Name = "hScrollBarPlayingPosition";
            hScrollBarPlayingPosition.Size = new Size(288, 15);
            hScrollBarPlayingPosition.TabIndex = 5;
            // 
            // PlayingProgress
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(label1);
            Controls.Add(labelTotalTime);
            Controls.Add(labelPlayingPosition);
            Controls.Add(hScrollBarPlayingPosition);
            Name = "PlayingProgress";
            Size = new Size(386, 15);
            Load += PlayingProgress_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelTotalTime;
        private Label labelPlayingPosition;
        private Label label1;
        private HScrollBar hScrollBarPlayingPosition;
    }
}
