using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace PaleSlumber
{
    /// <summary>
    /// 再生管理
    /// </summary>
    internal class PlaynigManager : IDisposable
    {
        public PlaynigManager()
        {
        }
        #region メンバ変数
        /// <summary>
        /// 再生場所
        /// </summary>
        private WaveOutEvent? OutputEvent = null;
        /// <summary>
        /// 再生中の音楽ファイル
        /// </summary>
        private AudioFileReader? AudioFile = null;
        #endregion

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="filepath"></param>
        public void Init(string filepath)
        {
        }

        public void Dispose()
        {
            this.OutputEvent?.Dispose();
            this.OutputEvent = null;
            this.AudioFile?.Dispose();
            this.OutputEvent = null;
        }
    }
}
