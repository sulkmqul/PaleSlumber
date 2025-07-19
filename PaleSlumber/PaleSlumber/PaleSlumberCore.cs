using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace PaleSlumber
{
    /// <summary>
    /// 基幹処理
    /// </summary>
    /// <remarks>音楽再生などイベント処理を一括でおこなう</remarks>
    internal class PaleSlumberCore : IDisposable
    {
        public PaleSlumberCore()
        {
        }

        public IObservable<PaleEvent> RollEvent
        {
            get
            {
                return this.RollEventSub;
            }
        }

        private Subject<PaleEvent> RollEventSub { get; init; } = new Subject<PaleEvent>();

        /// <summary>
        /// 処理データ
        /// </summary>
        private PaleGlobal FData
        {
            get
            {
                return PaleGlobal.Mana;
            }
        }

        /// <summary>
        /// 初期化
        /// </summary>
        PaleEventIgniter EventTable { get; init; } = new PaleEventIgniter();

        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Init()
        {
            //プレイリスト初期化
            this.FData.PlayList.Init();

            //イベントテーブルの作成
            //再生周り
            this.EventTable.AddEvent(EPaleSlumberEvent.PlayStart, typeof(PlayListFileData), (ev) => this.StartPlay(ev));
            this.EventTable.AddEvent(EPaleSlumberEvent.PlayStop, (ev) => this.StopPlay(ev));
            this.EventTable.AddEvent(EPaleSlumberEvent.PlayPause, (ev) => this.PausePlay(ev));
            this.EventTable.AddEvent(EPaleSlumberEvent.PlayNext, (ev) => this.PlayNext(ev));
            this.EventTable.AddEvent(EPaleSlumberEvent.PlayPrev, (ev) => this.PlayPrev(ev));
            this.EventTable.AddEvent(EPaleSlumberEvent.PlayingPositionChanged, typeof(int), (ev) => this.ChangePlayPosition(ev));


            //プレイリスト周り
            this.EventTable.AddEvent(EPaleSlumberEvent.PlayListAdd, typeof(string[]), async (ev) => await this.AddPlayListProc(ev));
            this.EventTable.AddEvent(EPaleSlumberEvent.PlayListSelectedChanged, typeof(PlayListFileData[]), (ev) => this.ChangePlayListSelect(ev));
            this.EventTable.AddEvent(EPaleSlumberEvent.PlayListRemove, typeof(PlayListFileData[]), (ev) => this.RemovePlayList(ev));
            this.EventTable.AddEvent(EPaleSlumberEvent.PlayListOrderManualChanged, typeof(int), (ev) => this.ChangeOrderPlayListManual(ev));
            
            this.EventTable.AddEvent(EPaleSlumberEvent.PlayListSortDefault, (ev) => this.SortPlayList(ev));
            this.EventTable.AddEvent(EPaleSlumberEvent.PlayListSortRandom, (ev) => this.SortPlayList(ev));
            this.EventTable.AddEvent(EPaleSlumberEvent.PlayListSortTitle, (ev) => this.SortPlayList(ev));
            this.EventTable.AddEvent(EPaleSlumberEvent.PlayListSortDuration, (ev) => this.SortPlayList(ev));


            this.EventTable.AddEvent(EPaleSlumberEvent.VolumeChanged, (ev) => this.ChangeVolume(ev));

            //関連イベント実行
            this.FData.EventSub.Subscribe(ev =>
            {
                this.EventTable.Execute(ev);
            });
        }

        /// <summary>
        /// 削除処理
        /// </summary>
        public void Dispose()
        {
            this.EventTable.Clear();
        }
        //--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
        //--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
        //--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
        //--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//

        /// <summary>
        /// 再生開始
        /// </summary>
        /// <param name="ev"></param>
        /// <returns></returns>
        private void StartPlay(PaleEvent ev)
        {
            var f = ev.ParamPlayListFile;
            if (f == null)
            {
                return;
            }

            this.FData.Player.StartPlay(f);
        }

        /// <summary>
        /// 再生停止
        /// </summary>
        /// <param name="ev"></param>
        private void StopPlay(PaleEvent ev)
        {
            this.FData.Player.Stop();
        }

        /// <summary>
        /// 一時停止
        /// </summary>
        /// <param name="ev"></param>
        private void PausePlay(PaleEvent ev)
        {
            //一時停止中なら再開する
            if (this.FData.Player.IsPause == true)
            {
                this.FData.Player.StartPlay();
                return;
            }
            //一時停止
            this.FData.Player.Pause();
        }

        /// <summary>
        /// 次を再生
        /// </summary>
        /// <param name="ev"></param>
        private void PlayNext(PaleEvent ev)
        {
            var pf = this.FData.Player.PlayingFile;
            this.FData.PlayList.SelectNext(pf);
            if (this.FData.PlayList.SelectedFile != null)
            {
                this.FData.Player.StartPlay(this.FData.PlayList.SelectedFile);
            }
            this.RollEventSub.OnNext(ev);
        }

        /// <summary>
        /// 前を再生
        /// </summary>
        /// <param name="ev"></param>
        private void PlayPrev(PaleEvent ev)
        {
            var pf = this.FData.Player.PlayingFile;
            this.FData.PlayList.SelectPrev(pf);
            if (this.FData.PlayList.SelectedFile != null)
            {
                this.FData.Player.StartPlay(this.FData.PlayList.SelectedFile);
            }
            this.RollEventSub.OnNext(ev);
        }

        /// <summary>
        /// プレイリスト追加
        /// </summary>
        /// <param name="ev"></param>
        private async Task AddPlayListProc(PaleEvent ev)
        {
            //読み込み
            string[] vec = ev.ParamStringArray;
            await this.FData.PlayList.Load(vec);

            //終わったら完了処理
            this.RollEventSub.OnNext(ev);
        }

        /// <summary>
        /// プレイリストの削除
        /// </summary>
        /// <param name="ev"></param>
        private void RemovePlayList(PaleEvent ev)
        {
            //対象の削除
            PlayListFileData[] flist = ev.ParamPlayListFileArray;
            this.FData.PlayList.RemovePlayList(flist);


            this.RollEventSub.OnNext(ev);
        }

        /// <summary>
        /// プレイリストの選択変更
        /// </summary>
        /// <param name="ev"></param>
        private void ChangePlayListSelect(PaleEvent ev)
        {
            PlayListFileData[] flist = ev.ParamPlayListFileArray;
            //既存選択のクリア
            this.FData.PlayList.ClearSelect();
            //対象を選択リストへ
            foreach (var item in flist)
            {
                this.FData.PlayList.AddSelect(item);
            }
        }
        /// <summary>
        /// プレイリストの順番変更
        /// </summary>
        /// <param name="ev"></param>
        private void ChangeOrderPlayListManual(PaleEvent ev)
        {
            var plist = this.FData.PlayList;
            //順番変更処理
            bool f = plist.ChangeOrder((int)ev.EventParam);
            if (f == false)
            {
                return;
            }
            this.RollEventSub.OnNext(ev);
        }

        /// <summary>
        /// 音量の変更
        /// </summary>
        /// <param name="ev"></param>
        private void ChangeVolume(PaleEvent ev)
        {
            float vol = (float)ev.EventParam;
            this.FData.Player.Volume = vol;
        }

        /// <summary>
        /// 再生位置の変更
        /// </summary>
        /// <param name="ev"></param>
        private void ChangePlayPosition(PaleEvent ev)
        {
            int pos = (int)ev.EventParam;
            this.FData.Player.ChangePosition(pos);
        }

        /// <summary>
        /// プレイリストの並べ替え
        /// </summary>
        /// <param name="ev"></param>
        private void SortPlayList(PaleEvent ev)
        {
            bool f = this.FData.PlayList.SortPlayList(ev.Event);
            if (f == false)
            {
                return;
            }
            this.RollEventSub.OnNext(ev);

        }
    }


}
