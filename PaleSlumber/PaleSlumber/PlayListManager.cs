using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaleSlumber
{
    /// <summary>
    /// 管理ファイル情報
    /// </summary>
    internal class PlayListFileData
    {   

        /// <summary>
        /// これのファイル名(拡張子なし)
        /// </summary>
        public string FileName { get; protected set; } = "";
        /// <summary>
        /// ファイルのフルパス
        /// </summary>
        public string FilePath { get; protected set; } = "";
        /// <summary>
        /// 拡張子
        /// </summary>
        public string Extension { get; protected set; } = "";

        /// <summary>
        /// 時間
        /// </summary>
        public TimeSpan TotalTime { get; protected set; }

        /// <summary>
        /// 波形情報
        /// </summary>
        public string WaveFormatString { get; protected set; } = "";

        /// <summary>
        /// 表示順
        /// </summary>
        public int OrderNo { get; protected set; } = 0;

        /// <summary>
        /// 曲のタイトル
        /// </summary>
        [Obsolete("not implemented yet")]
        public string Title { get; protected set; } = "";
        /// <summary>
        /// アーティスト名
        /// </summary>
        [Obsolete("not implemented yet")]
        public string ArtistName { get; protected set; } = "";
        /// <summary>
        /// アルバム名
        /// </summary>
        [Obsolete("not implemented yet")]
        public string AlbumName { get; protected set; } = "";


        /// <summary>
        /// 読み込み
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="n">表示順</param>
        public void Load(string filepath, int n = 0)
        {
            //ファイルパスの情報を取得
            this.FileName = Path.GetFileNameWithoutExtension(filepath);
            this.FilePath = Path.GetFullPath(filepath);
            this.Extension = Path.GetExtension(filepath);

            //表示順設定
            this.OrderNo = n;

            //音楽情報の取得
            using (NAudio.Wave.AudioFileReader ar = new NAudio.Wave.AudioFileReader(filepath))
            {
                this.TotalTime = new TimeSpan(ar.TotalTime.Ticks);                
                this.WaveFormatString = ar.WaveFormat.ToString();
            }

            //その他の情報を取得
            //アーティスト名とかタグとかを取得せよ
        }
    }

    /// <summary>
    /// 再生リスト管理
    /// </summary>
    internal class PlayListManager
    {
        public PlayListManager()
        {
        }

        #region メンバ変数
        /// <summary>
        /// 再生リスト情報
        /// </summary>
        public List<PlayListFileData> PlayList{ get; init; } = new List<PlayListFileData>();

        /// <summary>
        /// 表示順のシーケンス
        /// </summary>
        private int Seq { get; set; } = 0;

        /// <summary>
        /// 現在の選択中データ一覧
        /// </summary>
        private List<PlayListFileData> SelectedList { get; init; } = new List<PlayListFileData>();

        /// <summary>
        /// 選択ファイル
        /// </summary>
        public PlayListFileData? SelectedFile
        {
            get
            {
                return this.SelectedList.FirstOrDefault();
            }
        }
        #endregion

        /// <summary>
        /// プレイリスト初期化
        /// </summary>
        public void Init()
        {
            this.PlayList.Clear();
            this.SelectedList.Clear();
            this.Seq = 0;
        }

        /// <summary>
        /// 読み込み
        /// </summary>
        /// <param name="pathvec">読み込みパス一覧</param>
        /// <returns>読み込めたもの一覧</returns>
        public async Task<List<PlayListFileData>> Load(string[] pathvec)
        {
            List<string> pathlist = new List<string>();

            //対象となるパスを全て取得
            foreach (string path in pathvec)
            {
                var list = await this.GetFilePathRecall(path);
                pathlist.AddRange(list);
            }

            //対象パスの読み込み
            var rlist = await this.LoadPlayList(pathlist);

            //選択を行う
            this.SelectedList.Clear();
            if (rlist.Count > 0)
            {
                this.SelectedList.Add(rlist.First());
            }

            return rlist;
        }

                
        /// <summary>
        /// 対象のデータが選択されているかを確認する
        /// </summary>
        /// <param name="fdata"></param>
        /// <returns></returns>
        public bool CheckSelected(PlayListFileData fdata)
        {
            int c = this.SelectedList.Where(x => x.OrderNo == fdata.OrderNo).Count();
            if (c <= 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 選択に追加
        /// </summary>
        /// <param name="fdata"></param>
        public void AddSelect(PlayListFileData fdata)
        {
            this.SelectedList.Add(fdata);
        }

        /// <summary>
        /// 対象を選択
        /// </summary>
        /// <param name="fdata"></param>
        public void Select(PlayListFileData fdata)
        {
            this.ClearSelect();
            this.SelectedList.Add(fdata);
        }
        /// <summary>
        /// 対象を選択
        /// </summary>
        /// <param name="index"></param>
        public void Select(int index)
        {
            this.ClearSelect();

            //そもそもデータがない
            if (this.PlayList.Count <= 0)
            {
                return;
            }
            //indexが範囲外だった
            if (index >= this.PlayList.Count)
            {
                //最後を選択
                this.SelectedList.Add(this.PlayList.Last());
                return;
            }            
            if (index < 0)
            {
                //最初を選択
                this.SelectedList.Add(this.PlayList.First());
                return;
            }

            //対象に追加
            this.SelectedList.Add(this.PlayList[index]);
        }

        /// <summary>
        /// 選択のクリア
        /// </summary>
        public void ClearSelect()
        {
            this.SelectedList.Clear();
        }

        /// <summary>
        /// 次を選択する、無いときは適切な選択を行う
        /// </summary>
        /// <param name="fdata">基準ファイル</param>
        public void SelectNext(PlayListFileData? fdata)
        {
            //プレイリスト内位置の計算
            int index = this.CalcuPlayListIndex(fdata);
            if (index < 0)
            {
                //無効なら選択を変えない
                return;
            }
            //次を選択
            index += 1;
            this.Select(index);

        }
        /// <summary>
        /// 前を選択する、無いときは適切な選択を行う
        /// </summary>
        /// <param name="fdata">基準ファイル</param>
        public void SelectPrev(PlayListFileData? fdata)
        {
            //プレイリスト内位置の計算
            int index = this.CalcuPlayListIndex(fdata);
            if (index < 0)
            {
                //無効なら選択を変えない
                return;
            }
            //前を選択
            index -= 1;
            this.Select(index);
        }

        /// <summary>
        /// プレイリストの削除
        /// </summary>
        /// <param name="vec"></param>
        public void RemovePlayList(PlayListFileData[] vec)
        {
            int rm = 0;

            foreach (var data in vec)
            {
                rm = this.PlayList.IndexOf(data);
                this.PlayList.Remove(data);
            }

            //対象の選択
            this.Select(rm);            
        }

        /// <summary>
        /// 選択を対象indexの場所にする
        /// </summary>
        /// <param name="index">挿入位置　マイナス=無効</param>
        /// <returns>入れ替え成功可否</returns>
        public bool ChangeOrder(int index)
        {
            //対象があるか？
            if (index < 0 || this.SelectedList.Count <= 0)
            {
                return false;
            }

            int fi = this.PlayList.IndexOf(this.SelectedList.First());
            int ipos = index;
            //既存を削除
            this.SelectedList.ForEach(x => this.PlayList.Remove(x));

            //挿入位置の計算
            //順番を下げるときは既存で削除した数=選択数を差し引いて考える
            if (index > fi)
            {
                ipos = index - this.SelectedList.Count;
                if (ipos < 0)
                {
                    ipos = 0;
                }
            }

            this.PlayList.InsertRange(ipos, this.SelectedList);

            return true;
        }
        //--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
        //--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
        /// <summary>
        /// プレイリストの読み込み
        /// </summary>
        /// <param name="pathlist">読み込みパス一覧</param>
        /// <returns>読み込めたもの一覧</returns>
        private async Task<List<PlayListFileData>> LoadPlayList(List<string> pathlist)
        {
            List<Task<PlayListFileData?>> tlist = new List<Task<PlayListFileData?>>();

            //全データの概要の読み込み
            foreach (string path in pathlist)
            {
                //コピーを取っておく
                int n = this.Seq;
                var t = Task<PlayListFileData?>.Run(() =>
                {
                    try
                    {
                        PlayListFileData data = new PlayListFileData();

                        data.Load(path, n);

                        return data;
                    }
                    catch
                    {
                        return null;
                    }

                });

                this.Seq += 1;

                tlist.Add(t);
            }

            //読み込み終了待ち            
            var loaddata = await Task.WhenAll(tlist);
            if (loaddata == null)
            {
                return new List<PlayListFileData>();
            }

            //null除外
            List<PlayListFileData> rlist = loaddata.Where(x => x != null).Select(x => x!).ToList();
            rlist.ForEach(x => this.PlayList.Add(x));


            return rlist;

        }

        /// <summary>
        /// 対象のパスの全ファイルを取得
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private async Task<List<string>> GetFilePathRecall(string path)
        {
            List<string> anslist = new List<string>();

            bool f = this.CheckDirectory(path);
            //対象がファイルだった
            if (f == false)
            {
                //隠されていない？
                bool nf = this.CheckPathHidden(path);
                if (nf == false)
                {
                    anslist.Add(path);
                }
                return anslist;            
            }

            //以下のフォルダ全てのファイルパスを取得する
            string[] pathlist = await Task.Run(() => Directory.GetFileSystemEntries(path, "*.*"));
            foreach (var fpath in pathlist)
            {
                var list = await this.GetFilePathRecall(fpath);
                anslist.AddRange(list);
            }

            return anslist;

        }

        /// <summary>
        /// 対象のパスがディレクトリか否かを調べる
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool CheckDirectory(string path)
        {            
            FileAttributes at =  File.GetAttributes(path);
            if ((at & FileAttributes.Directory) == FileAttributes.Directory)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 隠しファイルか否かの確認
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool CheckPathHidden(string path)
        {
            FileAttributes at = File.GetAttributes(path);
            if ((at & FileAttributes.Hidden) == FileAttributes.Hidden)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 対象ファイルのプレイリストのindexを取得する
        /// </summary>
        /// <param name="fdata">検索対象</param>
        /// <returns>対象index マイナス=無効</returns>
        private int CalcuPlayListIndex(PlayListFileData? fdata)
        {
            //基準がないとき
            if (fdata == null)
            {
                return -1;
            }
            //playlist内基準位置の検索
            var nowsel = this.PlayList.Where(x => x.OrderNo == fdata.OrderNo);
            if (nowsel.Count() <= 0)
            {
                //playlist内にないとき
                return -1;
            }

            //ここまで来たら存在するので適切なものを選択
            PlayListFileData fb = nowsel.First();
            int index = this.PlayList.IndexOf(fb);
            return index;
        }
    }
}
