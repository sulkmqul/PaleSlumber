using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaleSlumber
{
    /// <summary>
    /// プレイヤーモード
    /// </summary>
    enum EPlayerMode : int
    {
        Mini = 0,
        Normal,
    }

    internal class PaleConst
    {
        /// <summary>
        /// アプリケーション名
        /// </summary>
        public const string ApplicationName = "Pale Slumber";

        /// <summary>
        /// 最小モード画面サイズ
        /// </summary>
        public static readonly Size MiniModeFormSize = new Size(400, 125);
        /// <summary>
        /// 最小モードコントロール部のサイズ
        /// </summary>
        public static readonly int MiniModeControlHeight = 80;

        /// <summary>
        /// 最小モード時のコントロール部左側のpixelサイズ
        /// </summary>
        public static readonly int MiniModeControlLeftWidthPixel = 200;



        /// <summary>
        /// 通常モード 画面サイズ
        /// </summary>
        public static readonly Size NormalModeDefaultSize = new Size(800, 450);
        /// <summary>
        /// 通常モードコントロール部サイズ
        /// </summary>
        public static readonly int NormalModeControlHeight = 200;

        /// <summary>
        /// 通常モード時のコントロール部左側のサイズ(%)
        /// </summary>
        public static readonly int NormalModeControlLeftWidthPercent = 70;


        /// <summary>
        /// 波形バッファの最大長
        /// </summary>
        public const int MaxWaveBufferLength = 1024;

        /// <summary>
        /// 波形描画サイズ
        /// </summary>
        public const int MaxWaveRenderingSize = 128;

        /// <summary>
        /// 波形表示背景色
        /// </summary>
        public static readonly Color WaveBackgroundColor = Color.Black;
        /// <summary>
        /// 波形色
        /// </summary>
        public static readonly Color WaveColor = Color.Azure;
    }
}
