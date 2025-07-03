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
        #endregion

        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Init()
        {
            this.Grid.Items.Clear();
            this.Grid.GridLines = true;

            //ヘッダーの作成
            this.CreateHeader();

            //描画変更を検知
            PaleGlobal.Mana.EventSub.Where(x =>
            (x == EPaleSlumberEvent.InitPlayList ||
            x == EPaleSlumberEvent.AddPlayList ||
            x == EPaleSlumberEvent.RemovePlayList))
                .Subscribe(x =>
                {
                    this.DisplayList();
                });

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
        /// プレイリストの描画
        /// </summary>
        private void DisplayList()
        {
            this.Grid.Items.Clear();
            var plist = PaleGlobal.Mana.PlayList;
            foreach (var data in plist.PlayList)
            {
                //表示用データ作成
                var list = this.CreateGridData(data);
                this.Grid.Items.Add(new ListViewItem(list.ToArray()));
            }
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
    }
}
