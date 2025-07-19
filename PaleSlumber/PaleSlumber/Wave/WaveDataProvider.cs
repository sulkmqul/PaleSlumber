using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaleSlumber.Wave
{
    /// <summary>
    /// 波形データの管理提供者
    /// </summary>
    internal class WaveDataProvider
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="maxlen">最大管理数</param>
        public WaveDataProvider(int maxlen)
        {
            this.MaxLength = maxlen;
        }

        /// <summary>
        /// 最大管理数
        /// </summary>
        public int MaxLength { get; init; } = 0;

        /// <summary>
        /// 一時保管波形数
        /// </summary>
        private ConcurrentQueue<float> WaveTempQue = new ConcurrentQueue<float>();

        /// <summary>
        /// 値の設定
        /// </summary>
        /// <param name="vec"></param>
        public void Push(List<float> wlist)
        {
            foreach (var data in wlist)
            {
                //最大数を超える場合、初めを削除して追加
                if (this.WaveTempQue.Count > this.MaxLength)
                {
                    float f;
                    this.WaveTempQue.TryDequeue(out f);
                }
                this.WaveTempQue.Enqueue(data);
            }
        }

        /// <summary>
        /// データ取得
        /// </summary>
        /// <param name="length">最大取得数 0埋め</param>
        /// <returns></returns>
        public List<float> GetData(int length)
        {
            List<float> anslist = new List<float>(length);
            for (int i = 0; i < length; i++)
            {
                float v = 0;
                if (this.WaveTempQue.Count > 0)
                {
                    bool f = this.WaveTempQue.TryDequeue(out v);
                    if (f == false)
                    {
                        System.Diagnostics.Trace.WriteLine("ここにきてはならない");
                    }
                    
                }
                anslist.Add(v);
                this.WaveTempQue.TryDequeue(out v);
            }
            return anslist;
        }
    }
}
