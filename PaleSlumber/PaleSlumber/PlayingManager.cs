using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace PaleSlumber
{
    enum EPlayingEvent
    {
        Start,
        Playing,
        Stop,
    }

    /// <summary>
    /// 再生情報
    /// </summary>
    class PlayingInfo
    {
        public PlayingInfo(EPlayingEvent ev, PlayListFileData pf, List<float> wbuf, TimeSpan total, TimeSpan nowt)
        {
            Event = ev;
            PlayFile = pf;
            WaveBuffer = wbuf;
            TotalTime = total;
            NowTime = nowt;
        }
        public PlayingInfo(EPlayingEvent ev, PlayListFileData pf)
        {
            Event = ev;
            PlayFile = pf;
        }

        public EPlayingEvent Event { get; init; } = EPlayingEvent.Playing;

        public PlayListFileData PlayFile { get; init; }

        public List<float> WaveBuffer { get; init; } = new List<float>();

        public TimeSpan TotalTime { get; init; }

        public TimeSpan NowTime { get; init; }

    }

    /// <summary>
    /// 再生管理
    /// </summary>
    internal class PlayingManager : IDisposable
    {
        public PlayingManager()
        {            
        }
        #region メンバ変数
        /// <summary>
        /// 再生中可否
        /// </summary>
        public bool IsPlaying { get; private set; } = false;
        /// <summary>
        /// 一時停止可否
        /// </summary>
        public bool IsPause { get; private set; } = false;

        /// <summary>
        /// 音量(0.0～1.0の間であること)
        /// </summary>
        private float _Volume = 1.0f;
        /// <summary>
        /// 音量(0.0～1.0の間であること)
        /// </summary>
        public float Volume
        {
            get
            {
                return this._Volume;
            }
            set
            {
                this._Volume = value;
                this._Volume = Math.Max(this._Volume, 0);
                this._Volume = Math.Min(this._Volume, 1.0f);
                if (this.AudioFile != null)
                {
                    this.AudioFile.Volume = this._Volume;
                }
            }
        }

        /// <summary>
        /// 再生情報のstream
        /// </summary>
        private Subject<PlayingInfo> PlayingSub { get; init; } = new Subject<PlayingInfo>();

        /// <summary>
        /// 再生ストリーム
        /// </summary>
        public IObservable<PlayingInfo> PlayingStream
        {
            get
            {
                return this.PlayingSub;
            }

        }

        /// <summary>
        /// 再生場所
        /// </summary>
        private WaveOutEvent? OutputEvent = null;
        /// <summary>
        /// 再生中の音楽ファイル
        /// </summary>
        private AudioFileReader? AudioFile = null;
        /// <summary>
        /// 現在の読み込みファイル情報
        /// </summary>
        public PlayListFileData? PlayingFile { get; private set; } = null;

        /// <summary>
        /// 波形サンプラー
        /// </summary>
        private WaveSampler? Sampler = null;


        private IDisposable? WaveTaskMark = null;
        #endregion

        /// <summary>
        /// 再生開始
        /// </summary>
        /// <param name="fdata">再生開始ファイル</param>
        public void StartPlay(PlayListFileData fdata)
        {
            //ファイル読み込み
            this.LoadSound(fdata);

            //再生開始
            this.StartPlay();
        }

        /// <summary>
        /// 再生開始
        /// </summary>        
        public void StartPlay()
        {
            if (this.AudioFile == null)
            {
                return;
            }

            this.IsPlaying = true;
            this.IsPause = false;

            this.AudioFile.Volume = this.Volume;

            if (this.PlayingFile != null)
            {
                this.PlayingSub.OnNext(new PlayingInfo(EPlayingEvent.Start, this.PlayingFile));
            }

            //再生開始
            this.OutputEvent?.Play();

            
        }

        /// <summary>
        /// 再生停止
        /// </summary>
        public void Stop()
        {
            if (this.OutputEvent == null || this.AudioFile == null)
            {
                return;
            }

            this.IsPlaying = false;
            this.IsPause = false;

            this.OutputEvent.Stop();
            this.AudioFile.Position = 0;
        }

        /// <summary>
        /// 一時停止
        /// </summary>
        public void Pause()
        {
            if (this.OutputEvent == null || this.AudioFile == null)
            {
                return;
            }

            this.IsPlaying = false;
            this.IsPause = true;

            this.OutputEvent.Pause();
        }

        /// <summary>
        /// 再生位置の変更
        /// </summary>
        /// <param name="sec"></param>
        public void ChangePosition(int sec)
        {
            if (this.AudioFile == null)
            {
                return;
            }
            this.AudioFile.CurrentTime = new TimeSpan(0, 0, sec);
        }

        /// <summary>
        /// 解放された時
        /// </summary>
        public void Dispose()
        {   

            //開放
            if (this.OutputEvent != null)
            {
                //このイベントを先に解除しないとstop→dispose時にinvalid参照な問題有り
                //よって先に解除してstopする
                this.OutputEvent.PlaybackStopped -= onPlayStopped;

                //念のため停止
                this.Stop();

                this.OutputEvent.Dispose();
                this.OutputEvent = null;
            }
            this.AudioFile?.Dispose();
            this.OutputEvent = null;
            this.WaveTaskMark?.Dispose();
            this.WaveTaskMark = null;
        }

        //--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
        /// <summary>
        /// 停止イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void onPlayStopped(object? sender, StoppedEventArgs e)
        {               
            //再生停止
            this.IsPlaying = false;
            
            if (this.PlayingFile == null || this.AudioFile == null || this.OutputEvent == null)
            {
                return;
            }

            
            System.Diagnostics.Trace.WriteLine($"onPlayStopped state={this.OutputEvent.PlaybackState} pos={this.AudioFile.Position} len={this.AudioFile.Length}");
            System.Diagnostics.Trace.WriteLine($"current_time={this.AudioFile.CurrentTime} total_time={this.AudioFile.TotalTime}");

            //最後まで行ったら再生終了を通知
            //System.Diagnostics.Trace.WriteLine($"Stop   {this.AudioFile.Length}/{this.AudioFile.Position}");
            if (this.AudioFile.Position >= this.AudioFile.Length)
            {
                this.PlayingSub.OnNext(new PlayingInfo(EPlayingEvent.Stop, this.PlayingFile));
            }
            
            
        }

        /// <summary>
        /// ファイル読み込み
        /// </summary>
        /// <param name="fdata"></param>
        private void LoadSound(PlayListFileData fdata)
        {
            //既存の物体をいったん削除
            this.Dispose();

            //ファイル読み込み
            this.AudioFile = new AudioFileReader(fdata.FilePath);
            
            ///再生準備
            this.OutputEvent = new WaveOutEvent();
            //停止イベント設定
            this.OutputEvent.PlaybackStopped += onPlayStopped;

            //読み込みファイル保存
            this.PlayingFile = fdata;

            //波形取得用にサンプラー作成
            this.Sampler = new WaveSampler(this.AudioFile);
            //波形関数設定
            this.WaveTaskMark = this.Sampler.WaveStream.Subscribe(wave =>
            {
                //再生情報伝達
                this.PlayingSub.OnNext(new PlayingInfo(
                    EPlayingEvent.Playing, this.PlayingFile,
                    new List<float>(wave), this.AudioFile.TotalTime, this.AudioFile.CurrentTime));
            });

            //出力読み込み
            //this.OutputEvent.Init(this.AudioFile);
            this.OutputEvent.Init(this.Sampler);            
        }

        
    }

    /// <summary>
    /// 波形取得管理
    /// </summary>
    class WaveSampler : ISampleProvider
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="source"></param>
        public WaveSampler(AudioFileReader source)
        {
            this.SourceFile = source;
        }

        /// <summary>
        /// 管理ファイル
        /// </summary>
        AudioFileReader SourceFile;

        /// <summary>
        /// 波形形式
        /// </summary>
        public WaveFormat WaveFormat => this.SourceFile.WaveFormat;

        public IObservable<float[]> WaveStream
        {
            get
            {
                return this.WaveSub;
            }
        }
        private Subject<float[]> WaveSub = new Subject<float[]>();

        /// <summary>
        /// データ読み取り処理
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public int Read(float[] buffer, int offset, int count)
        {
            int rlen = SourceFile.Read(buffer, offset, count);
            if (rlen <= 0)
            {
                return rlen;
            }
            
            //コピーして送信
            float[] wavebuf = new float[rlen];
            Array.Copy(buffer, offset, wavebuf, 0, rlen);
            //Buffer.BlockCopy(buffer, offset, wavebuf, 0, rlen);            

            this.WaveSub.OnNext(wavebuf);

            return rlen;
        }
    }
}
