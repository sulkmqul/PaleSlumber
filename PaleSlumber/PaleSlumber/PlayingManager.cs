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
    /// <summary>
    /// 再生情報
    /// </summary>
    class PlayingInfo
    {
        public string Title { get; init; } = "";

        
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
        private float _Volume = 0;
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

        private IObservable<PlayingInfo> PlayingStream
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
        /// 解放された時
        /// </summary>
        public void Dispose()
        {
            //念のため停止
            this.Stop();

            //開放
            this.OutputEvent?.Dispose();
            this.OutputEvent = null;
            this.AudioFile?.Dispose();
            this.OutputEvent = null;
        }

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

            //通知
            
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

            this.OutputEvent.Init(this.AudioFile);

            //読み込みファイル保存
            this.PlayingFile = fdata;
        }

        
    }
}
