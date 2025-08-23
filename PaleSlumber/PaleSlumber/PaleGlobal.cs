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

        /// <summary>
        /// システムファイルの保存
        /// </summary>
        public void SaveSystemConfig()
        {
            //保存データの作成
            PaleSystemConfigData sdata = new PaleSystemConfigData();
            sdata.CurrentSeq = this.Player.PlayingFile?.SeqNo ?? 0;
            sdata.PlayListItemList = this.PlayList.PlayList.Select(x => new PlayListItem() { Seq = x.SeqNo, FilePath = x.FilePath }).ToList();

            PaleSlumberSystemConfig conf = new PaleSlumberSystemConfig();
            conf.Save(sdata);
        }

        /// <summary>
        /// システムファイルの読み込み
        /// </summary>
        public void LoadSystemConfig()
        {
            try
            {
                PaleSlumberSystemConfig conf = new PaleSlumberSystemConfig();

                //システムファイル読み込み
                PaleSystemConfigData data = conf.Load();

                //プレイリスト読み込み
                this.PlayList.LoadPlayList(data.PlayListItemList);

                //選択の復元
                this.PlayList.SelectSeq(data.CurrentSeq);

                //再生命令
                //if (this.PlayList.SelectedFile != null)
                //{
                //    this.EventSub.OnNext(new PaleEvent(EPaleSlumberEvent.PlayStart, this.PlayList.SelectedFile));
                //}
            }
            catch
            {
                //システムファイルは読めなくても問題ないので放置する。
            }
        }
    }




}
