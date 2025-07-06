using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.CompilerServices;



namespace PaleSlumber
{

    /// <summary>
    /// マウス情報
    /// </summary>
    public class MouseInfo
    {
        /// <summary>
        /// マウス押されているか可否
        /// </summary>
        public bool DownFlag
        {
            get
            {
                if (this.DownButton == MouseButtons.None)
                {
                    return false;
                }
                return true;
            }
        }


        /// <summary>
        /// 押されているボタン
        /// </summary>
        public MouseButtons DownButton { get; private set; } = MouseButtons.None;

        /// <summary>
        /// 押された位置
        /// </summary>
        public PointF DownPos { get; private set; } = new PointF();

        /// <summary>
        ///　現在値
        /// </summary>
        private PointF _NowPos = new PointF();
        /// <summary>
        /// 現在値
        /// </summary>
        public PointF NowPos
        {
            get
            {
                return this._NowPos;
            }
            private set
            {
                this.PrevPos = this._NowPos;
                this._NowPos = value;
            }
        }

        /// <summary>
        /// 前回の位置
        /// </summary>
        public PointF PrevPos { get; private set; } = new PointF();


        /// <summary>
        /// 押した位置からの距離
        /// </summary>
        public PointF DownLength
        {
            get
            {
                return new PointF(this.NowPos.X - this.DownPos.X,
                    this.NowPos.Y - this.DownPos.Y);
            }
        }

        /// <summary>
        /// 前回から動いた距離
        /// </summary>
        public PointF PrevMoveLength
        {
            get
            {
                return new PointF(this.NowPos.X - this.PrevPos.X,
                    this.NowPos.Y - this.PrevPos.Y);
            }
        }

        /// <summary>
        /// 押した位置から今の位置までの範囲を取得する
        /// </summary>
        public RectangleF DownRange
        {
            get
            {
                float left = Math.Min(this.NowPos.X, this.DownPos.X);
                float top = Math.Min(this.NowPos.Y, this.DownPos.Y);
                float w = Math.Abs(this.DownPos.X - this.NowPos.X);
                float h = Math.Abs(this.DownPos.Y - this.NowPos.Y);

                return new RectangleF(left, top, w, h);
            }

        }


        /// <summary>
        /// 記憶値
        /// </summary>
        protected Dictionary<int, object> MemoryDic = new Dictionary<int, object>();


        //--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
        /// <summary>
        /// マウス押した
        /// </summary>
        /// <param name="args"></param>
        public void DownMouse(MouseEventArgs args)
        {
            this.DownButton = args.Button;
            this.DownPos = new PointF(args.X, args.Y);
            this.NowPos = new PointF(args.X, args.Y);
        }

        /// <summary>
        /// マウス動いた
        /// </summary>
        /// <param name="args"></param>
        public void MoveMouse(MouseEventArgs args)
        {
            this.NowPos = new PointF(args.X, args.Y);
        }

        /// <summary>
        /// マウス離した
        /// </summary>
        /// <param name="args"></param>
        public void UpMouse(MouseEventArgs args)
        {            
            this.DownButton = MouseButtons.None;
        }
        /// <summary>
        /// 位置の更新
        /// </summary>
        /// <param name="args"></param>
        public void UpdatePositon(MouseEventArgs args)
        {
            this.NowPos = new PointF(args.X, args.Y);
        }

        /// <summary>
        /// マウスホイール
        /// </summary>
        /// <param name="args"></param>
        public void WheelMouse(MouseEventArgs args)
        {
            this.NowPos = new PointF(args.X, args.Y);            
        }


        /// <summary>
        /// メモリ値の設定
        /// </summary>
        /// <param name="data"></param>
        /// <param name="slot">記憶場所</param>
        public void SetMemory(object data, int slot = 0)
        {
            this.MemoryDic[slot] = data;
        }

        /// <summary>
        /// メモリ値の取得
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="slot">記憶場所</param>
        /// <returns></returns>
        public T? GetMemory<T>(T def, int slot = 0)
        {
            if (this.MemoryDic.ContainsKey(slot) == false)
            {
                return def;
            }

            T data = (T)this.MemoryDic[slot];
            return data;
        }
    }
}
