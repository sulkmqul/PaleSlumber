using System.Reactive.Linq;
using System.Runtime.CompilerServices;

namespace PaleSlumber
{
    /// <summary>
    /// メイン画面
    /// </summary>
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            //プレイリストGrid管理初期化
            this.Grid = new PlayListGrid(this.listViewPlayList);
        }

        

        /// <summary>
        /// 画面データ
        /// </summary>
        private PaleGlobal FData
        {
            get
            {
                return PaleGlobal.Mana;
            }
        }

        private PlayListGrid Grid { get; init; }
        //--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
        /// <summary>
        /// 画面初期化
        /// </summary>
        private void InitForm()
        {
            //プレイリスト初期化
            this.FData.PlayList.Init();

            //プレイヤーモード設定
            this.ChangePlayerMode(EPlayerMode.Normal);

            //プレイリストGridの初期化
            this.Grid.Init();
        }

        /// <summary>
        /// 表示モードが変更された時
        /// </summary>
        /// <param name="mode"></param>
        private void ChangePlayerMode(EPlayerMode mode)
        {
            this.FData.Mode = mode;
            int n = (int)mode;

            //各モードにおける変更値を定義
            //画面サイズ
            Size[] fsize = { PaleConst.MiniModeFormSize, PaleConst.NormalModeDefaultSize };
            //操作パネル高さ
            int[] ch = { PaleConst.MiniModeControlHeight, PaleConst.NormalModeControlHeight };
            //画面サイズ制限
            Size[] limitsize = { PaleConst.MiniModeFormSize, new Size(0, 0) };

            bool[] listvisible = { false, true };

            //値の設定
            this.MaximumSize = limitsize[n];
            this.MinimumSize = limitsize[n];
            this.Size = fsize[n];
            this.panelControl.Height = ch[n];
            this.listViewPlayList.Visible = listvisible[n];
        }

        /// <summary>
        /// エラーの表示
        /// </summary>
        /// <param name="message"></param>
        private void ShowError(string message)
        {
            MessageBox.Show(this, message, PaleConst.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// イベントの発行
        /// </summary>
        /// <param name="ev"></param>
        private void PublishEvent(EPaleSlumberEvent ev)
        {
            this.FData.EventSub.OnNext(ev);
        }

        //--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
        //--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
        //--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
        //--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
        /// <summary>
        /// 読み込まれた時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MainForm_Load(object sender, EventArgs e)
        {
            //画面初期化1
            this.InitForm();

            /*
            {
                this.listViewPlayList.Items.Clear();
                this.listViewPlayList.GridLines = true;
                ColumnHeader[] hvec = {
                new ColumnHeader() { Text = "タイトル", Width = 150 },
                new ColumnHeader() { Text = "曲長", Width = 100 },
                new ColumnHeader() { Text = "パス", Width = 200 }
            };
                this.listViewPlayList.Columns.AddRange(hvec);

                this.listViewPlayList.Items.Add(new ListViewItem(new string[] { "abc", "4:0", "path" }));
            }*/

            
        }

        /// <summary>
        /// 画面モードの切り替えボタンが押された時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonChangePlayerMode_Click(object sender, EventArgs e)
        {
            EPlayerMode mode = EPlayerMode.Normal;
            if (this.FData.Mode == EPlayerMode.Normal)
            {
                mode = EPlayerMode.Mini;
            }
            this.ChangePlayerMode(mode);
        }

        /// <summary>
        /// ドラッグアンドドロップされるとき
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            //ファイルパス以外は無視する
            var f = e.Data?.GetDataPresent(DataFormats.FileDrop);
            if (f != true)
            {
                return;
            }

            e.Effect = DragDropEffects.Link;

        }

        /// <summary>
        /// ドラッグドロップされた時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            var data = (string[]?)e.Data?.GetData(DataFormats.FileDrop);
            if (data == null)
            {
                return;
            }

            try
            {
                await this.FData.PlayList.Load(data);

                this.PublishEvent(EPaleSlumberEvent.AddPlayList);
                
            }
            catch (Exception ex)
            {
                this.ShowError("読み込み失敗");
            }

        }
    }
}
