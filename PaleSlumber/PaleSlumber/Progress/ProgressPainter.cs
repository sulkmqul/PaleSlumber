using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaleSlumber.Progress
{
    class BarData
    {
        /// <summary>
        /// Bar有効範囲
        /// </summary>
        public RectangleF BarAviableArea { get; private set; } = new RectangleF();

        /// <summary>
        /// 現在のBarの描画エリア(小さい版)
        /// </summary>
        public RectangleF BarRenderingAreaSmall { get; private set; } = new RectangleF();
        /// <summary>
        /// 現在のBarの描画エリア(大きい版)
        /// </summary>
        public RectangleF BarRenderingAreaLarge { get; private set; } = new RectangleF();

        /// <summary>
        /// Barの実際のサイズ(当たり判定サイズ)
        /// </summary>
        public RectangleF BarArea{get; private set; } = new RectangleF();

        /// <summary>
        /// Barの中心位置
        /// </summary>
        public PointF Center { get; private set; } = new PointF();

        /// <summary>
        /// 今回の描画データの計算
        /// </summary>
        /// <param name="sarea">描画エリア全体</param>
        /// <param name="bpos">描画位置</param>
        public void CalcuArea(RectangleF sarea, float bpos)
        {
            //描画幅に適したbarのサイズを計算する
            float w = this.CalcuWidth(sarea);
            float sh = 3.0f;
            float lh = 10.0f;

            //Barの有効範囲を計算
            this.BarAviableArea = new RectangleF(w * 0.5f, 0, sarea.Width - w, sarea.Height);

            //中心位置の計算
            float centerx = (this.BarAviableArea.Width * bpos) + BarAviableArea.Left;
            float centery = (this.BarAviableArea.Height * 0.5f) + this.BarAviableArea.Y + 1;
            this.Center = new PointF(centerx, centery);
            {
                this.BarArea = new RectangleF(centerx - (w * 0.5f), this.BarAviableArea.Y, w, this.BarAviableArea.Height);
            }
            {                
                float sy = centery - (sh * 0.5f);
                this.BarRenderingAreaSmall = new RectangleF(this.BarArea.X, sy, this.BarArea.Width, sh);
            }
            {
                float ly = centery - (lh * 0.5f);
                this.BarRenderingAreaLarge = new RectangleF(this.BarArea.X, ly, this.BarArea.Width, lh);
            }
        }


        /// <summary>
        /// 小アイコン描画
        /// </summary>
        /// <param name="gra"></param>
        /// <param name="alpha"></param>
        /// <param name="bf"></param>
        public void RenderSmall(Graphics gra, int alpha, bool bf = false)
        {
            this.DrawBarRect(gra, this.BarRenderingAreaSmall, alpha, bf);
        }
        /// <summary>
        /// 大アイコン描画
        /// </summary>
        /// <param name="gra"></param>
        /// <param name="alpha"></param>
        /// <param name="bf"></param>
        public void RenderLarge(Graphics gra, int alpha, bool bf = false)
        {
            this.DrawBarRect(gra, this.BarRenderingAreaLarge, alpha, bf);
        }

        /// <summary>
        /// 矩形描画
        /// </summary>
        /// <param name="gra"></param>
        /// <param name="rect"></param>
        /// <param name="alpha"></param>
        /// <param name="bf"></param>
        private void DrawBarRect(Graphics gra, RectangleF rect, int alpha, bool bf)
        {
            Color col = Color.FromArgb(alpha, SystemColors.ControlDark);
            using (SolidBrush bru = new SolidBrush(col))
            {

                gra.FillRectangle(bru, rect);

            }
            if (bf == false)
            {
                return;
            }
            //境界線描画
            Color pcol = Color.FromArgb(alpha, SystemColors.ControlDarkDark);
            using (Pen pe = new Pen(pcol, 1.0f))
            {
                gra.DrawRectangle(pe, rect);
            }
        }


        private float CalcuWidth(RectangleF sarea)
        {
            return 25.0f;
        }


    }

    /// <summary>
    /// 進捗表示の描画
    /// </summary>
    internal class ProgressPainter
    {
        /// <summary>
        /// 描画データ
        /// </summary>
        private BarData Data = new BarData();

        /// <summary>
        /// 現在の進捗度合い(0.0～1.0)
        /// </summary>
        public float ProgressParcent { get; set; } = 0.0f;

        /// <summary>
        /// 大アイコンのアルファ値
        /// </summary>
        public int LargeAlpha { get; set; } = 0;

        /// <summary>
        /// マウスの重なり可否
        /// </summary>
        public bool MouseOver { get; set; } = false;

        /// <summary>
        /// Barの中心位置
        /// </summary>
        public PointF BarCenter
        {
            get
            {
                return this.Data.Center;
            }
        }

        /// <summary>
        /// Barの検出エリア
        /// </summary>
        public RectangleF BarArea
        {
            get
            {
                return this.Data.BarArea;
            }
        }

        /// <summary>
        /// Barの有効範囲(全体)
        /// </summary>
        public RectangleF AviableArea
        {
            get
            {
                return this.Data.BarAviableArea;
            }
        }

        /// <summary>
        /// 描画処理
        /// </summary>
        /// <param name="gra"></param>
        public void Render(Graphics gra, Control con)
        {  

            //描画計算
            this.Data.CalcuArea(new RectangleF(0, 0, con.Width, con.Height), this.ProgressParcent);

            //全体クリア
            gra.Clear(SystemColors.Control);

            //小描画
            this.Data.RenderSmall(gra, 255 - this.LargeAlpha, this.MouseOver);

            //大描画
            this.Data.RenderLarge(gra, this.LargeAlpha, this.MouseOver);
        }
    }
}
