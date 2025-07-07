using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaleSlumber
{
    /// <summary>
    /// プレイリストのグリッド管理
    /// </summary>
    internal class PlayListGrid
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="view">管理グリッド</param>
        public PlayListGrid(ListView view)
        {
            this.Grid = view;
        }

        
        #region メンバ変数
        /// <summary>
        /// 管理グリッド
        /// </summary>
        private ListView Grid { get; init; }
        
        /// <summary>
        /// 再生ファイル情報の取得
        /// </summary>
        public List<PlayListFileData> SelectedFileList
        {
            get
            {
                List<PlayListFileData> anslist = new List<PlayListFileData>();

                //対象のファイルを取得
                foreach (ListViewItem item in this.Grid.SelectedItems)
                {
                    PlayListFileData? tag = item.Tag as PlayListFileData;
                    if (tag != null)
                    {
                        anslist.Add(tag);
                    }
                }
                return anslist;
            }
        }

        /// <summary>
        /// マウス情報
        /// </summary>
        private MouseInfo MInfo { get; init; } = new MouseInfo();
        #endregion

        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Init()
        {
            this.Grid.Items.Clear();
            this.Grid.GridLines = true;
            this.Grid.FullRowSelect = true;
            this.Grid.MultiSelect = true;

            //ヘッダーの作成
            this.CreateHeader();

        }

        /// <summary>
        /// プレイリストの描画
        /// </summary>
        public void DisplayList()
        {
            //描画処理
            this.Grid.Items.Clear();
            var plist = PaleGlobal.Mana.PlayList;

            List<ListViewItem> ilist = new List<ListViewItem>();
            foreach (var data in plist.PlayList)
            {
                //表示用データ作成
                var list = this.CreateGridData(data);
                var item = new ListViewItem(list.ToArray());
                item.Tag = data;
                item.Selected = plist.CheckSelected(data);
                ilist.Add(item);                
            }

            try
            {
                this.Grid.SuspendLayout();
                this.Grid.Items.AddRange(ilist.ToArray());
                //選択対象を表示させる
                if (this.Grid.SelectedIndices.Count > 0)
                {
                    this.Grid.EnsureVisible(this.Grid.SelectedIndices[0]);
                }
            }
            finally
            {
                this.Grid.ResumeLayout();
            }


            

        }

        /// <summary>
        /// マウス押された
        /// </summary>
        /// <param name="e"></param>
        public void DownMouse(MouseEventArgs e)
        {
            this.MInfo.DownMouse(e);
        }
        /// <summary>
        /// マウス動いた
        /// </summary>
        /// <param name="e"></param>
        public void MoveMouse(MouseEventArgs e)
        {
            this.MInfo.MoveMouse(e);
            if (this.MInfo.DownFlag == false)
            {
                return;
            }

            int ni = this.CalcuInsertIndex(e.Location);
            this.Grid.InsertionMark.Index = ni;
        }

        /// <summary>
        /// マウス離した
        /// </summary>
        /// <param name="e"></param>
        public void UpMouse(MouseEventArgs e)
        {
            this.MInfo.UpMouse(e);

            //離した位置をメモリに追加しておく
            int ni = this.CalcuInsertIndex(e.Location);
            this.MInfo.SetMemory(ni);

            this.Grid.InsertionMark.Index = -1;            
        }

        /// <summary>
        /// 入れ替え挿入位置を取得
        /// </summary>
        /// <returns></returns>
        public int GetReplaceIndex()
        {
            return this.MInfo.GetMemory<int>(-1);
        }
        //--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
        /// <summary>
        /// ヘッダーの作成
        /// </summary>
        private void CreateHeader()
        {
            ColumnHeader[] hvec = {
                new ColumnHeader() { Text = "タイトル", Width = 250 },
                new ColumnHeader() { Text = "曲長", Width = 50 },
                new ColumnHeader() { Text = "パス", Width = 400 }
            };            

            this.Grid.Columns.Clear();
            this.Grid.Columns.AddRange(hvec);
        }


        

        /// <summary>
        /// 表示データの作成
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private List<string> CreateGridData(PlayListFileData data)
        {
            List<string> anslist = new List<string>();

            //タイトル
            anslist.Add(data.FileName);
            //長さ
            anslist.Add($"{data.TotalTime.Minutes:D2}:{data.TotalTime.Seconds:D2}");
            //ファイルパス
            anslist.Add(data.FilePath);

            return anslist;

        }

        /// <summary>
        /// 挿入位置を計算する
        /// </summary>
        /// <param name="mpos">今のマウス位置</param>
        /// <returns>挿入位置 マイナス=無効</returns>
        private int CalcuInsertIndex(Point mpos)
        {
            //一番近いindexを計算
            int ans = this.Grid.InsertionMark.NearestIndex(mpos);

            //選択範囲の中なら無効とする
            bool f = this.Grid.SelectedIndices.Contains(ans);
            if (f == true)
            {
                ans = -1;
            }
            return ans;

        }
    }
}
