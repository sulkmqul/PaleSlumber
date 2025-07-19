using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaleSlumber
{
    public partial class PlayingProgress : UserControl
    {
        public PlayingProgress()
        {
            InitializeComponent();

        }

        /// <summary>
        /// 設定中の処理
        /// </summary>
        private bool AutoSetting = false;


        /// <summary>
        /// 手動設定可否
        /// </summary>
        private bool ManualMovingFlag = false;

        /// <summary>
        /// 設定
        /// </summary>
        /// <param name="total"></param>
        /// <param name="current"></param>
        public void ProgressPlaying(TimeSpan total, TimeSpan current)
        {
            this.AutoSetting = true;

            //手動でデータを掴んでいる場合は更新しない
            if (this.ManualMovingFlag == false)
            {
                this.Invoke(new Action(() =>
                {
                    this.hScrollBarPlayingPosition.Maximum = (int)total.TotalSeconds;

                    int ctime = (int)current.TotalSeconds;
                    if (ctime > this.hScrollBarPlayingPosition.Maximum) {
                        ctime = this.hScrollBarPlayingPosition.Maximum;
                    }
                    this.hScrollBarPlayingPosition.Value = ctime;
                }));
            }
            
            this.AutoSetting = false;

        }

        /// <summary>
        /// 表示処理
        /// </summary>
        private void DisplayTime()
        {
            TimeSpan total = new TimeSpan(0, 0, this.hScrollBarPlayingPosition.Maximum);
            TimeSpan curt = new TimeSpan(0, 0, this.hScrollBarPlayingPosition.Value);


            this.labelTotalTime.Text = $"{total.Hours:D2}:{total.Minutes:D2}:{total.Seconds:D2}";
            this.labelPlayingPosition.Text = $"{curt.Hours:D2}:{curt.Minutes:D2}:{curt.Seconds:D2}";
        }

        /// <summary>
        /// 読み込まれた時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayingProgress_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// スクロールバーの値が変更された時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hScrollBarPlayingPosition_ValueChanged(object sender, EventArgs e)
        {
            //System.Diagnostics.Trace.WriteLine($"position_changed={this.AutoSetting}");

            //表示更新
            this.DisplayTime();
        }

        /// <summary>
        /// スクロールバーをマウスが掴んだ時、離したとき
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hScrollBarPlayingPosition_MouseCaptureChanged(object sender, EventArgs e)
        {
            this.ManualMovingFlag = !this.ManualMovingFlag;
            if (this.ManualMovingFlag == false)
            {
                //再生位置の変更
                PaleGlobal.Mana.EventSub.OnNext(new PaleEvent(EPaleSlumberEvent.PlayingPositionChanged, this.hScrollBarPlayingPosition.Value));
            }
        }
    }
}
