using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaleSlumber.Wave
{
    /// <summary>
    /// 波形描画コア
    /// </summary>
    internal class WavePainter
    {
        public WavePainter()
        {
            
        }

        class RenderInfo
        {
            /// <summary>
            /// 表示サイズ
            /// </summary>
            public SizeF DisplaySize { get; private set; }
            /// <summary>
            /// 中心サイズ
            /// </summary>
            public float CenterY { get; private set; }

            /// <summary>
            /// 画像描画の高さ
            /// </summary>
            public float HalfWaveHeight { get; private set; }

            /// <summary>
            /// 横描画オフセットX
            /// </summary>
            public float WaveStartX;
            /// <summary>
            /// 波形描画範囲
            /// </summary>
            public float WaveRenderWidth;


            /// <summary>
            /// 描画情報計算
            /// </summary>
            /// <param name="con"></param>
            public void Init(Control con)
            {                
                this.DisplaySize = new SizeF(con.Width, con.Height);
                this.CenterY = this.DisplaySize.Height * 0.5f;
                this.HalfWaveHeight = (this.DisplaySize.Height * 0.9f) * 0.5f;
                

                this.WaveStartX = 0;
                this.WaveRenderWidth = this.DisplaySize.Width;
            }

        }
        /// <summary>
        /// 描画情報
        /// </summary>
        RenderInfo Info = new RenderInfo();


        /// <summary>
        /// 表示のリサイズ
        /// </summary>
        /// <param name="con"></param>
        public void ResizeDisplay(Control con)
        {
            this.Info.Init(con);
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="gra">描画場所</param>
        /// <param name="par">親コントロール</param>
        /// <param name="wlist">波形情報</param>
        public void Render(Graphics gra, Control par, List<float> wlist)
        {
            //描画クリア
            gra.Clear(PaleConst.WaveBackgroundColor);

            //波形描画
            this.RenderWave(gra, wlist);

        }

        //--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="gra"></param>
        /// <param name="wlist"></param>
        private void RenderWave(Graphics gra, List<float> wlist)
        {
            using (Pen pe = new Pen(PaleConst.WaveColor, 1.0f))
            {
                //棒で描画                
                //for (int x = 0; x < wlist.Count; x++)
                //{
                //    PointF wpos = this.CalcuPointWave(x, wlist);
                //    PointF bpos = new PointF(wpos.X, this.Info.DisplaySize.Height);
                //    gra.DrawLine(pe, bpos, wpos);
                //}

                //線で描画
                PointF prevpos = new PointF(float.MinValue, float.MinValue);
                for (int x = 0; x < wlist.Count; x++)
                {
                    PointF wpos = this.CalcuPointWave(x, wlist);
                    if (prevpos.X < -100)
                    {
                        prevpos = wpos;
                    }
                    gra.DrawLine(pe, prevpos, wpos);
                    prevpos = wpos;
                }
            }
        }

        public PointF CalcuPointWave(int x, List<float> wlist)
        {
            //波形位置のpixel位置を算出
            float fwpos = (float)x * this.Info.WaveRenderWidth / wlist.Count;
            fwpos += this.Info.WaveStartX;

            float ypos = ((wlist[x] / 256.0f) - 0.5f) * (this.Info.HalfWaveHeight * 2.0f);
            ypos *= -1.0f;
            //ypos = this.Info.DisplaySize.Height - ypos;
            ypos += this.Info.CenterY;

            return new PointF(fwpos, ypos);
        }
    }
}
