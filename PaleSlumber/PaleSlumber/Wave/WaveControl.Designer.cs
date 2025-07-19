namespace PaleSlumber.Wave
{
    partial class WaveControl
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
            pictureBoxWave = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBoxWave).BeginInit();
            SuspendLayout();
            // 
            // pictureBoxWave
            // 
            pictureBoxWave.Dock = DockStyle.Fill;
            pictureBoxWave.Location = new Point(0, 0);
            pictureBoxWave.Name = "pictureBoxWave";
            pictureBoxWave.Size = new Size(150, 150);
            pictureBoxWave.TabIndex = 0;
            pictureBoxWave.TabStop = false;
            pictureBoxWave.Paint += pictureBoxWave_Paint;
            // 
            // WaveControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pictureBoxWave);
            DoubleBuffered = true;
            Name = "WaveControl";
            Load += WaveControl_Load;
            Resize += WaveControl_Resize;
            ((System.ComponentModel.ISupportInitialize)pictureBoxWave).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBoxWave;
    }
}
