using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaleSlumber
{
    /// <summary>
    /// イベント一覧
    /// </summary>
    enum EPaleSlumberEvent
    {
        /// <summary>
        /// 再生開始
        /// </summary>
        PlayStart,
        /// <summary>
        /// 一時停止
        /// </summary>
        PlayPause,
        /// <summary>
        /// 再生停止
        /// </summary>
        PlayStop,
        /// <summary>
        /// 次の曲へ
        /// </summary>
        PlayNext,
        /// <summary>
        /// 前の曲へ
        /// </summary>
        PlayPrev,
        /// <summary>
        /// プレイリストに追加
        /// </summary>
        AddPlayList,
        /// <summary>
        /// プレイリストを削除
        /// </summary>
        RemovePlayList,
        /// <summary>
        /// プレイリストを初期化
        /// </summary>
        InitPlayList,

        

    }

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
