using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace PaleSlumber
{
    /// <summary>
    /// 全体で使うもの
    /// </summary>
    class PaleGlobal : IDisposable
    {
        private PaleGlobal()
        {
            //デバッグ用に発生イベントを補足
            this.EventSub.Subscribe(x => System.Diagnostics.Trace.WriteLine($"-[{x.Event}]-"));
        }

        private static PaleGlobal Instance = new PaleGlobal();

        public static PaleGlobal Mana
        {
            get
            {
                return Instance;
            }
        }

        /// <summary>
        /// プレイヤーのモード
        /// </summary>
        public EPlayerMode Mode { get; set; } = EPlayerMode.Normal;

        /// <summary>
        /// 再生リスト管理
        /// </summary>
        public PlayListManager PlayList { get; init; } = new PlayListManager();

        /// <summary>
        /// イベントSubject
        /// </summary>
        public Subject<PaleEvent> EventSub { get; init; } = new Subject<PaleEvent>();

        /// <summary>
        /// 再生管理
        /// </summary>
        public PlayingManager Player { get; init; } = new PlayingManager();


        /// <summary>
        /// 削除処理
        /// </summary>
        public void Dispose()
        {
            this.Player.Dispose();
        }
    }



}
