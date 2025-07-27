using PaleSlumber.Progress;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaleSlumber
{
    /// <summary>
    /// 進捗Bar
    /// </summary>
    public partial class PlayingProgress : UserControl
    {
        public PlayingProgress()
        {
            InitializeComponent();



        }

        /// <summary>
        /// 自動設定中 true=自動設定中
        /// </summary>
        private bool AutoSettingFlag = false;

        /// <summary>
        /// 手動設定中 true=手動設定中
        /// </summary>
        private bool ManualSettingFlag = false;

        /// <summary>
        /// マウス情報
        /// </summary>
        private MouseInfo MInfo = new MouseInfo();

        /// <summary>
        /// 描画管理
        /// </summary>
        private ProgressPainter Painter = new ProgressPainter();

        /// <summary>
        /// 演出用のAlpha物
        /// </summary>
        private IDisposable? AlphaRx = null;

        /// <summary>
        /// Barを掴んか否かのフラグ true=掴んでいる
        /// </summary>
        private bool BarGrabFlag = false;

        /// <summary>
        /// 全体時間
        /// </summary>
        double CurrentTotalSeconds = 0.0;
        /// <summary>
        /// 現在時間
        /// </summary>
        double CurrentSeconds = 0.0;

        /// <summary>
        /// 設定
        /// </summary>
        /// <param name="total"></param>
        /// <param name="current"></param>
        public void ProgressPlaying(TimeSpan total, TimeSpan current)
        {
            this.AutoSettingFlag = true;

            //手動でデータを掴んでいる場合は更新しない
            if (this.ManualSettingFlag == false)
            {
                //値の保存
                this.CurrentTotalSeconds = total.TotalSeconds;
                this.CurrentSeconds = current.TotalSeconds;

                this.Invoke(new Action(() =>
                {
                    float parcent = (float)current.TotalMilliseconds / (float)total.TotalMilliseconds;
                    this.Painter.ProgressParcent = parcent;
                    this.DisplayTime();
                    this.Refresh();
                }));
            }

            this.AutoSettingFlag = false;

        }

        /// <summary>
        /// 表示処理
        /// </summary>
        private void DisplayTime()
        {
            TimeSpan total = TimeSpan.FromSeconds(this.CurrentTotalSeconds);
            TimeSpan curt = TimeSpan.FromSeconds(this.CurrentSeconds);

            
            string ts = $"{total.Hours:D2}:{total.Minutes:D2}:{total.Seconds:D2}";
            string cs = $"{curt.Hours:D2}:{curt.Minutes:D2}:{curt.Seconds:D2}";
            this.labelProgress.Text = $"{cs} / {ts}";
        }
        /// <summary>
        /// マニュアル設定
        /// </summary>
        private void ProgressManual()
        {
            //マウス位置の進捗率を算出
            float pc = this.MInfo.NowPos.X / this.Painter.AviableArea.Width;
            pc = Math.Max(0.0f, pc);
            pc = Math.Min(1.0f, pc);

            //値の設定
            this.CurrentSeconds = this.CurrentTotalSeconds * pc;
            this.Painter.ProgressParcent = pc;            

            this.DisplayTime();
        }

        /// <summary>
        /// Stepによる移動
        /// </summary>
        private void ProgressStep()
        {
            double offset = 2;
            if (this.MInfo.DownPos.X < this.Painter.BarCenter.X)
            {
                offset = -offset;
            }

            //適当にオフセット
            this.CurrentSeconds += offset;
            if (this.CurrentSeconds < 0)
            {
                this.CurrentSeconds = 0;
            }

            //通知
            PaleGlobal.Mana.EventSub.OnNext(new PaleEvent(EPaleSlumberEvent.PlayingPositionChanged, (int)this.CurrentSeconds));
        }
        //--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
        //--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
        //--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
        //--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
        /// <summary>
        /// 読み込まれた時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayingProgress_Load(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// 描画されるとき
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBoxProgressBar_Paint(object sender, PaintEventArgs e)
        {
            this.Painter.Render(e.Graphics, this.pictureBoxProgressBar);
        }

        /// <summary>
        /// マウスが押された時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBoxProgressBar_MouseDown(object sender, MouseEventArgs e)
        {
            this.ManualSettingFlag = true;
            this.MInfo.DownMouse(e);

            this.BarGrabFlag = false;
            bool gf = this.Painter.BarArea.Contains(this.MInfo.DownPos);
            if (gf == true)
            {
                this.BarGrabFlag = true;
                return;
            }
            this.ProgressStep();
        }

        /// <summary>
        /// マウスが動いたとき
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBoxProgressBar_MouseMove(object sender, MouseEventArgs e)
        {
            this.MInfo.MoveMouse(e);
            if (this.BarGrabFlag == true)
            {
                this.ProgressManual();
            }
            //マウスオーバー可否の判定
            if (this.MInfo.DownFlag == false)
            {
                this.Painter.MouseOver = this.Painter.BarArea.Contains(this.MInfo.NowPos);
            }
            this.Refresh();

        }

        /// <summary>
        /// マウスが離された時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBoxProgressBar_MouseUp(object sender, MouseEventArgs e)
        {
            this.MInfo.UpMouse(e);

            if (this.BarGrabFlag == true)
            {
                PaleGlobal.Mana.EventSub.OnNext(new PaleEvent(EPaleSlumberEvent.PlayingPositionChanged, (int)this.CurrentSeconds));
            }
            

            this.ManualSettingFlag = false;
            this.BarGrabFlag = false;
            
        }

        /// <summary>
        /// マウスが入った時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBoxProgressBar_MouseEnter(object sender, EventArgs e)
        {
            //alphaの変更を一定時間行ってちょっとリッチに魅せる演出
            this.AlphaRx?.Dispose();
            this.Painter.LargeAlpha = 0;
            this.AlphaRx = Observable.Interval(TimeSpan.FromMilliseconds(30)).
                TakeUntil(Observable.Timer(TimeSpan.FromMilliseconds(200))).
                Subscribe(x =>
                {
                    this.Painter.LargeAlpha += 40;
                    if (this.Painter.LargeAlpha > 255)
                    {
                        this.Painter.LargeAlpha = 255;
                    }
                    this.Invoke(() => this.Refresh());
                }
                );



        }

        /// <summary>
        /// マウスが出たとき
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBoxProgressBar_MouseLeave(object sender, EventArgs e)
        {
            //alphaの変更を一定時間行ってちょっとリッチに魅せる演出
            this.AlphaRx?.Dispose();
            this.AlphaRx = Observable.Interval(TimeSpan.FromMilliseconds(30)).
                TakeUntil(Observable.Timer(TimeSpan.FromMilliseconds(200))).
                Subscribe(x =>
                {
                    this.Painter.LargeAlpha -= 40;
                    if (this.Painter.LargeAlpha < 0)
                    {
                        this.Painter.LargeAlpha = 0;
                    }
                    this.Invoke(() => this.Refresh());
                }
                );
            this.Painter.MouseOver = false;
        }
    }
}
