using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaleSlumber.Wave
{
    /// <summary>
    /// 波形表示コントロール
    /// </summary>
    public partial class WaveControl : UserControl
    {
        public WaveControl()
        {
            InitializeComponent();
            this.WaveProvider = new WaveDataProvider(PaleConst.MaxWaveBufferLength);
        }

        /// <summary>
        /// 波形管理
        /// </summary>
        private WaveDataProvider WaveProvider { get; init; }

        /// <summary>
        /// 描画者　
        /// </summary>
        private WavePainter Painter = new WavePainter();

        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            //更新タイマーを仕掛ける
            var start = TimeSpan.FromSeconds(0);
            var span = TimeSpan.FromSeconds(0.1);
            Observable.Timer(start, span).Subscribe(x =>
            {
                //System.Diagnostics.Trace.WriteLine($"TIMER={x}");
                this.Invoke(new Action(() => this.Refresh()));
            });
        }

        /// <summary>
        /// 波形ADD
        /// </summary>
        /// <param name="wavelist"></param>
        public void PushWave(List<float> wavelist)
        {
            this.WaveProvider.Push(wavelist);
        }

        /// <summary>
        /// 波形の描画
        /// </summary>
        /// <param name="gra"></param>
        private void RenderWave(Graphics gra)
        {
            //描画波形取得
            List<float> wlist = this.WaveProvider.GetData(PaleConst.MaxWaveRenderingSize);            
            /*
            Random rand = new Random();
            List<float> wlist = new List<float>();
            for (int i = 0; i < PaleConst.MaxWaveRenderingSize; i++)
            {
                float f = ((rand.Next() % 2) == 0) ? -1 : 1;
                wlist.Add(Convert.ToSingle(rand.NextDouble() * f));
            }*/

            //描画処理
            this.Painter.Render(gra, this, wlist);
        }


        //--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
        //--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
        /// <summary>
        /// 読み込まれた時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WaveControl_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 描画されるとき
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBoxWave_Paint(object sender, PaintEventArgs e)
        {
            this.RenderWave(e.Graphics);
        }

        /// <summary>
        /// サイズが変更された時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WaveControl_Resize(object sender, EventArgs e)
        {
            this.Painter.ResizeDisplay(this);
        }
    }
}
