namespace PaleSlumber
{
    partial class VolumeControl
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
            trackBarVolume = new TrackBar();
            labelVolumeText = new Label();
            ((System.ComponentModel.ISupportInitialize)trackBarVolume).BeginInit();
            SuspendLayout();
            // 
            // trackBarVolume
            // 
            trackBarVolume.Dock = DockStyle.Fill;
            trackBarVolume.LargeChange = 10;
            trackBarVolume.Location = new Point(0, 0);
            trackBarVolume.Maximum = 100;
            trackBarVolume.Name = "trackBarVolume";
            trackBarVolume.Orientation = Orientation.Vertical;
            trackBarVolume.Size = new Size(40, 185);
            trackBarVolume.TabIndex = 13;
            trackBarVolume.TickFrequency = 100;
            trackBarVolume.TickStyle = TickStyle.Both;
            trackBarVolume.ValueChanged += trackBarVolume_ValueChanged;
            // 
            // labelVolumeText
            // 
            labelVolumeText.Dock = DockStyle.Bottom;
            labelVolumeText.Location = new Point(0, 185);
            labelVolumeText.Name = "labelVolumeText";
            labelVolumeText.Size = new Size(40, 15);
            labelVolumeText.TabIndex = 14;
            labelVolumeText.Text = "0";
            labelVolumeText.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // VolumeControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(trackBarVolume);
            Controls.Add(labelVolumeText);
            Name = "VolumeControl";
            Size = new Size(40, 200);
            Load += VolumeControl_Load;
            ((System.ComponentModel.ISupportInitialize)trackBarVolume).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TrackBar trackBarVolume;
        private Label labelVolumeText;
    }
}
