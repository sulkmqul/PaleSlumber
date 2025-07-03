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
        public int Seq { get; private set; } = 0;
        #endregion

        /// <summary>
        /// プレイリスト初期化
        /// </summary>
        public void Init()
        {
            this.PlayList.Clear();
            this.Seq = 0;
        }

        /// <summary>
        /// 読み込み
        /// </summary>
        /// <param name="pathvec">読み込みパス一覧</param>
        public async Task Load(string[] pathvec)
        {
            List<string> pathlist = new List<string>();

            //対象となるパスを全て取得
            foreach (string path in pathvec)
            {
                var list = await this.GetFilePathRecall(path);
                pathlist.AddRange(list);
            }

            //対象パスの読み込み
            await this.LoadPlayList(pathlist);
        }


        /// <summary>
        /// プレイリストの読み込み
        /// </summary>
        /// <param name="pathlist">読み込みパス一覧</param>
        /// <returns></returns>
        public async Task LoadPlayList(List<string> pathlist)
        {
            List<Task<PlayListFileData?>> tlist = new List<Task<PlayListFileData?>>();

            //全データの概要の読み込み
            foreach (string path in pathlist)
            {
                var t = Task<PlayListFileData?>.Run(() =>
                {
                    try
                    {
                        PlayListFileData data = new PlayListFileData();
                        data.Load(path, this.Seq);
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
                return;
            }

            //読み込みに成功したファイルだけを取得
            foreach (var item in loaddata)
            {
                if (item == null)
                {
                    continue;
                }
                    
                this.PlayList.Add(item);
            }

        }


        //--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
        //--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//

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
    }
}
