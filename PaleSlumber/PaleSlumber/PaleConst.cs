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
        /// 通常モード 画面サイズ
        /// </summary>
        public static readonly Size NormalModeDefaultSize = new Size(800, 450);
        /// <summary>
        /// 通常モードコントロール部サイズ
        /// </summary>
        public static readonly int NormalModeControlHeight = 200;
    }
}
