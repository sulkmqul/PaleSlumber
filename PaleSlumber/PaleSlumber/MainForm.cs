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

        /// <summary>
        /// コア処理
        /// </summary>
        private PaleSlumberCore Core { get; init; } = new PaleSlumberCore();

        /// <summary>
        /// グリッド管理
        /// </summary>
        private PlayListGrid Grid { get; init; }

        /// <summary>
        /// イベント処理
        /// </summary>
        private PaleEventIgniter RollEventTable { get; init; } = new PaleEventIgniter();
        //--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
        /// <summary>
        /// 画面初期化
        /// </summary>
        private void InitForm()
        {
            //コア処理初期化
            this.Core.Init();

            //プレイリストGridの初期化
            this.Grid.Init();

            //プレイヤーモード設定
            this.ChangePlayerMode(EPlayerMode.Normal);


            this.AddRollEventProc();

            //応答イベント処理
            this.Core.RollEvent.Subscribe(x =>
            {
                this.RollEventTable.Execute(x);
            });
        }

        /// <summary>
        /// 応答イベントの追加
        /// </summary>
        private void AddRollEventProc()
        {
            //Playlistの追加応答
            this.RollEventTable.AddEvent(EPaleSlumberEvent.PlayListAdd, (x) => this.Grid.DisplayList());
            this.RollEventTable.AddEvent(EPaleSlumberEvent.PlayListRemove, (x) => this.Grid.DisplayList());
            this.RollEventTable.AddEvent(EPaleSlumberEvent.PlayListOrderManualChanged, (x) => this.Grid.DisplayList());

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
            this.FData.EventSub.OnNext(new PaleEvent(ev, ""));
        }
        /// <summary>
        /// イベントの発行
        /// </summary>
        /// <param name="ev"></param>
        /// <param name="param"></param>
        private void PublishEvent(EPaleSlumberEvent ev, object param)
        {
            this.FData.EventSub.OnNext(new PaleEvent(ev, param));
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
            //画面初期化
            this.InitForm();
        }

        /// <summary>
        /// 閉じられたとき
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            PaleGlobal.Mana.Dispose();
            this.Core.Dispose();
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
        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            var data = (string[]?)e.Data?.GetData(DataFormats.FileDrop);
            if (data == null)
            {
                return;
            }

            try
            {
                this.PublishEvent(EPaleSlumberEvent.PlayListAdd, data);
            }
            catch (Exception ex)
            {
                this.ShowError("読み込み失敗");
            }

        }

        /// <summary>
        /// Gird上マウスが動いたとき
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewPlayList_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e)
        {

        }

        /// <summary>
        /// Gridをダブルクリックされたとき
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewPlayList_DoubleClick(object sender, EventArgs e)
        {
            if (this.FData.PlayList.SelectedFile == null)
            {
                return;
            }
            this.PublishEvent(EPaleSlumberEvent.PlayStart, this.FData.PlayList.SelectedFile);
        }

        /// <summary>
        /// キーが押された時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                this.PublishEvent(EPaleSlumberEvent.PlayListRemove, this.Grid.SelectedFileList.ToArray());
            }
        }

        /// <summary>
        /// listviewの選択が変更された時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewPlayList_SelectedIndexChanged(object sender, EventArgs e)
        {            
            
        }

        /// <summary>
        /// listviewマウスが押された時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewPlayList_MouseDown(object sender, MouseEventArgs e)
        {
            this.Grid.DownMouse(e);

            //選択の変更
            this.PublishEvent(EPaleSlumberEvent.PlayListSelectedChanged, this.Grid.SelectedFileList.ToArray());
        }
        /// <summary>
        /// listviewマウスが動いたとき
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewPlayList_MouseMove(object sender, MouseEventArgs e)
        {
            this.Grid.MoveMouse(e);
        }

        /// <summary>
        /// listviewマウスが離された時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewPlayList_MouseUp(object sender, MouseEventArgs e)
        {
            this.Grid.UpMouse(e);

            int index = this.Grid.GetReplaceIndex();
            if (index < 0)
            {                
                return;
            }
            //順番変更
            this.PublishEvent(EPaleSlumberEvent.PlayListOrderManualChanged, index);
        }

        /// <summary>
        /// 再生開始ボタンが押された時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSmallPlay_Click(object sender, EventArgs e)
        {
            if (this.FData.PlayList.SelectedFile == null)
            {
                return;
            }
            this.PublishEvent(EPaleSlumberEvent.PlayStart, this.FData.PlayList.SelectedFile);
        }

        /// <summary>
        /// 再生停止ボタンが押された時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSmallStop_Click(object sender, EventArgs e)
        {
            this.PublishEvent(EPaleSlumberEvent.PlayStop);
        }

        /// <summary>
        /// 前へボタンが押された時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSmallPrev_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 次へボタンが押された時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSmallNext_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 一時停止ボタンが押された時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSmallPause_Click(object sender, EventArgs e)
        {
            this.PublishEvent(EPaleSlumberEvent.PlayPause);
        }

        private void listViewPlayList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {

        }
    }
}
