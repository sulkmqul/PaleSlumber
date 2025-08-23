using System.Diagnostics;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

            //情報初期化
            this.paleInfoControl1.Init();

            //波形表示初期化
            this.waveControl1.Init();

            //プレイヤーモード設定
            this.ChangePlayerMode(EPlayerMode.Normal);

            //イベント応答初期化
            this.AddRollEventProc();

            //応答イベント処理
            this.Core.RollEvent.Subscribe(x =>
            {
                this.RollEventTable.Execute(x);
            });

            //音量の変更
            this.volumeControl1.VolumeStream.Select(x => this.CalcuVolumeValue(x)).Subscribe(vol =>
            {
                this.PublishEvent(EPaleSlumberEvent.VolumeChanged, vol);
            });

            //再生中の処理
            this.FData.Player.PlayingStream.Subscribe(x =>
            {
                this.PlayingEventProc(x);
            });

            //初期値の設定
            this.volumeControl1.Volume = 50;

            //初期化
            this.PublishEvent(EPaleSlumberEvent.Initialize);


            //引数指定のものを読み込む
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length <= 1)
            {
                return;
            }
            this.PublishEvent(EPaleSlumberEvent.PlayListAdd, new string[] { args[1] });

        }

        /// <summary>
        /// 応答イベントの追加
        /// </summary>
        private void AddRollEventProc()
        {
            //Playlistの追加応答
            this.RollEventTable.AddEvent(EPaleSlumberEvent.PlayListAdd, (x) =>
            {
                this.Grid.DisplayList();
                this.PlayStart();
            });
            this.RollEventTable.AddEvent(EPaleSlumberEvent.Initialize, (x) => this.Grid.DisplayList());
            this.RollEventTable.AddEvent(EPaleSlumberEvent.PlayListRemove, (x) => this.Grid.DisplayList());
            this.RollEventTable.AddEvent(EPaleSlumberEvent.PlayListOrderManualChanged, (x) => this.Grid.DisplayList());
            this.RollEventTable.AddEvent(EPaleSlumberEvent.PlayNext, (x) => this.Grid.DisplayList());
            this.RollEventTable.AddEvent(EPaleSlumberEvent.PlayPrev, (x) => this.Grid.DisplayList());
            this.RollEventTable.AddEvent(EPaleSlumberEvent.PlayListSortDefault, (x) => this.Grid.DisplayList());
            this.RollEventTable.AddEvent(EPaleSlumberEvent.PlayListSortTitle, (x) => this.Grid.DisplayList());
            this.RollEventTable.AddEvent(EPaleSlumberEvent.PlayListSortRandom, (x) => this.Grid.DisplayList());
            this.RollEventTable.AddEvent(EPaleSlumberEvent.PlayListSortDuration, (x) => this.Grid.DisplayList());
            this.RollEventTable.AddEvent(EPaleSlumberEvent.PlayListClear, (x) => this.Grid.DisplayList());
            this.RollEventTable.AddEvent(EPaleSlumberEvent.PlayListLoadFile, (x) => this.Grid.DisplayList());

            this.RollEventTable.AddEvent(EPaleSlumberEvent.PlayListSaveFile, (x) =>
            {
                bool f = (bool)x.EventParam;
                if (f == false)
                {
                    this.ShowError("書き込み失敗");
                }
            });



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
            //表示可否
            bool[] modevisible = { false, true };

            //コントロール部左側のサイズ
            int[] tablecontrolwidth = { PaleConst.MiniModeControlLeftWidthPixel, PaleConst.NormalModeControlLeftWidthPercent };
            //コントロール部左側のサイズ情報
            SizeType[] tabletype = { SizeType.Absolute, SizeType.Percent };

            //値の設定
            this.MaximumSize = limitsize[n];
            this.MinimumSize = limitsize[n];
            this.Size = fsize[n];
            this.panelControl.Height = ch[n];
            this.listViewPlayList.Visible = modevisible[n];
            this.volumeControl1.TextVisible = modevisible[n];
            this.tableLayoutPanelControl.ColumnStyles[0].Width = tablecontrolwidth[n];
            this.tableLayoutPanelControl.ColumnStyles[0].SizeType = tabletype[n];
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

        /// <summary>
        /// 内部利用の音量を計算する
        /// </summary>
        /// <returns></returns>
        private float CalcuVolumeValue(int vol)
        {
            float ans = 0;

            ans = (float)vol / 100.0f;

            return ans;
        }

        /// <summary>
        /// 再生開始
        /// </summary>
        private void PlayStart()
        {
            if (this.FData.PlayList.SelectedFile == null)
            {
                return;
            }
            this.PublishEvent(EPaleSlumberEvent.PlayStart, this.FData.PlayList.SelectedFile);
        }

        /// <summary>
        /// 再生処理
        /// </summary>
        /// <param name="pf"></param>
        private void PlayingEventProc(PlayingInfo pf)
        {
            if (pf.Event == EPlayingEvent.Start)
            {
                //再生情報初期化
                this.paleInfoControl1.LoadFile(pf.PlayFile);
                this.ChangeFormTitle(pf.PlayFile);
                return;
            }
            if (pf.Event == EPlayingEvent.Stop)
            {
                //再生が終了したら次を再生
                this.PublishEvent(EPaleSlumberEvent.PlayNext);
                return;
            }
            //再生位置の設定
            this.playingProgress1.ProgressPlaying(pf.TotalTime, pf.NowTime);

            //波形処理
            this.waveControl1.PushWave(pf.WaveBuffer);
        }


        /// <summary>
        /// 画面タイトルの変更
        /// </summary>
        /// <param name="pf">表示タイトル null=なし</param>
        private void ChangeFormTitle(PlayListFileData? pf)
        {
            this.Text = PaleConst.ApplicationName;
            if (pf == null)
            {
                return;
            }
            this.Text = $"{pf.DisplayText} - {PaleConst.ApplicationName}";
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
            //システムファイル保存
            PaleGlobal.Mana.SaveSystemConfig();

            //後始末
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
            this.PlayStart();
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
            this.PlayStart();
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
            this.PublishEvent(EPaleSlumberEvent.PlayPrev);
        }

        /// <summary>
        /// 次へボタンが押された時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSmallNext_Click(object sender, EventArgs e)
        {
            this.PublishEvent(EPaleSlumberEvent.PlayNext);
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

        /// <summary>
        /// listviewアイテム選択の変更時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewPlayList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {

        }

        /// <summary>
        /// menu プレイリスト並べ替え 既定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemSortDefault_Click(object sender, EventArgs e)
        {
            this.PublishEvent(EPaleSlumberEvent.PlayListSortDefault);
        }

        /// <summary>
        /// menu プレイリスト並べ替え タイトル
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemSortTitle_Click(object sender, EventArgs e)
        {
            this.PublishEvent(EPaleSlumberEvent.PlayListSortTitle);
        }

        /// <summary>
        /// menu プレイリスト並べ替え ランダム
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemSortRandom_Click(object sender, EventArgs e)
        {
            this.PublishEvent(EPaleSlumberEvent.PlayListSortRandom);
        }

        /// <summary>
        /// menu プレイリスト並べ替え 曲長
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemSortDuration_Click(object sender, EventArgs e)
        {
            this.PublishEvent(EPaleSlumberEvent.PlayListSortDuration);
        }

        /// <summary>
        /// menu プレイリストクリア
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemPlayListClear_Click(object sender, EventArgs e)
        {
            this.PublishEvent(EPaleSlumberEvent.PlayListClear);
        }

        /// <summary>
        /// menu プレイリスト保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemPlayListSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog diag = new SaveFileDialog();
            diag.Filter = $"PaleSlumber PlayList File(*{PaleConst.PaleSumberPlayListFileExtension})|*{PaleConst.PaleSumberPlayListFileExtension}|全てのファイル(*.*)|*.*";
            var dret = diag.ShowDialog(this);
            if (dret != DialogResult.OK)
            {
                return;
            }
            string filepath = diag.FileName;
            this.PublishEvent(EPaleSlumberEvent.PlayListSaveFile, filepath);
        }

        /// <summary>
        /// menu プレイリスト読み込み
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemPlayListLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog diag = new OpenFileDialog();
            diag.Filter = "PaleSlumber PlayList File(*.ppf)|*.ppf|全てのファイル(*.*)|*.*";
            diag.Multiselect = false;
            var dret = diag.ShowDialog(this);
            if (dret != DialogResult.OK)
            {
                return;
            }
            string filepath = diag.FileName;
            this.PublishEvent(EPaleSlumberEvent.PlayListLoadFile, filepath);
        }

        /// <summary>
        /// 対象フォルダを開く
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOpenExplorer_Click(object sender, EventArgs e)
        {
            string filepath = this.FData.PlayList.SelectedFile?.FilePath ?? "";
            this.PublishEvent(EPaleSlumberEvent.ExplorerOpen, filepath);
        }
    }
}
