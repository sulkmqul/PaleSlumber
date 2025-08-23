namespace PaleSlumber
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            imageListSmallIcon = new ImageList(components);
            listViewPlayList = new ListView();
            contextMenuStripPaleMenu = new ContextMenuStrip(components);
            toolStripMenuItemSort = new ToolStripMenuItem();
            toolStripMenuItemSortDefault = new ToolStripMenuItem();
            toolStripMenuItemSortTitle = new ToolStripMenuItem();
            toolStripMenuItemSortRandom = new ToolStripMenuItem();
            toolStripMenuItemSortDuration = new ToolStripMenuItem();
            toolStripMenuItemPlayListClear = new ToolStripMenuItem();
            toolStripMenuItemPlayListSave = new ToolStripMenuItem();
            toolStripMenuItemPlayListLoad = new ToolStripMenuItem();
            panelSmallButton = new Panel();
            buttonOpenExplorer = new Button();
            buttonSmallPause = new Button();
            buttonSmallStop = new Button();
            buttonSmallNext = new Button();
            buttonSmallPrev = new Button();
            buttonSmallPlay = new Button();
            playingProgress1 = new PlayingProgress();
            buttonChangePlayerMode = new Button();
            panelControl = new Panel();
            tableLayoutPanelControl = new TableLayoutPanel();
            waveControl1 = new PaleSlumber.Wave.WaveControl();
            panelControlLeft = new Panel();
            paleInfoControl1 = new PaleInfoControl();
            volumeControl1 = new VolumeControl();
            tableLayoutPanel1 = new TableLayoutPanel();
            buttonPlayListShuffle = new Button();
            contextMenuStripPaleMenu.SuspendLayout();
            panelSmallButton.SuspendLayout();
            panelControl.SuspendLayout();
            tableLayoutPanelControl.SuspendLayout();
            panelControlLeft.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // imageListSmallIcon
            // 
            imageListSmallIcon.ColorDepth = ColorDepth.Depth32Bit;
            imageListSmallIcon.ImageStream = (ImageListStreamer)resources.GetObject("imageListSmallIcon.ImageStream");
            imageListSmallIcon.TransparentColor = Color.Transparent;
            imageListSmallIcon.Images.SetKeyName(0, "fast.png");
            imageListSmallIcon.Images.SetKeyName(1, "next.png");
            imageListSmallIcon.Images.SetKeyName(2, "pause.png");
            imageListSmallIcon.Images.SetKeyName(3, "play.png");
            imageListSmallIcon.Images.SetKeyName(4, "prev.png");
            imageListSmallIcon.Images.SetKeyName(5, "rollback.png");
            imageListSmallIcon.Images.SetKeyName(6, "stop.png");
            imageListSmallIcon.Images.SetKeyName(7, "folder.png");
            imageListSmallIcon.Images.SetKeyName(8, "shuffle.png");
            // 
            // listViewPlayList
            // 
            listViewPlayList.Activation = ItemActivation.OneClick;
            listViewPlayList.ContextMenuStrip = contextMenuStripPaleMenu;
            listViewPlayList.Dock = DockStyle.Fill;
            listViewPlayList.FullRowSelect = true;
            listViewPlayList.Location = new Point(0, 200);
            listViewPlayList.Name = "listViewPlayList";
            listViewPlayList.Size = new Size(784, 211);
            listViewPlayList.TabIndex = 6;
            listViewPlayList.UseCompatibleStateImageBehavior = false;
            listViewPlayList.View = View.Details;
            listViewPlayList.ItemMouseHover += listViewPlayList_ItemMouseHover;
            listViewPlayList.ItemSelectionChanged += listViewPlayList_ItemSelectionChanged;
            listViewPlayList.SelectedIndexChanged += listViewPlayList_SelectedIndexChanged;
            listViewPlayList.DoubleClick += listViewPlayList_DoubleClick;
            listViewPlayList.MouseDown += listViewPlayList_MouseDown;
            listViewPlayList.MouseMove += listViewPlayList_MouseMove;
            listViewPlayList.MouseUp += listViewPlayList_MouseUp;
            // 
            // contextMenuStripPaleMenu
            // 
            contextMenuStripPaleMenu.Items.AddRange(new ToolStripItem[] { toolStripMenuItemSort, toolStripMenuItemPlayListClear, toolStripMenuItemPlayListSave, toolStripMenuItemPlayListLoad });
            contextMenuStripPaleMenu.Name = "contextMenuStripPaleMenu";
            contextMenuStripPaleMenu.Size = new Size(121, 92);
            // 
            // toolStripMenuItemSort
            // 
            toolStripMenuItemSort.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItemSortDefault, toolStripMenuItemSortTitle, toolStripMenuItemSortRandom, toolStripMenuItemSortDuration });
            toolStripMenuItemSort.Name = "toolStripMenuItemSort";
            toolStripMenuItemSort.Size = new Size(120, 22);
            toolStripMenuItemSort.Text = "並べ替え";
            // 
            // toolStripMenuItemSortDefault
            // 
            toolStripMenuItemSortDefault.Name = "toolStripMenuItemSortDefault";
            toolStripMenuItemSortDefault.Size = new Size(110, 22);
            toolStripMenuItemSortDefault.Text = "既定";
            toolStripMenuItemSortDefault.Click += toolStripMenuItemSortDefault_Click;
            // 
            // toolStripMenuItemSortTitle
            // 
            toolStripMenuItemSortTitle.Name = "toolStripMenuItemSortTitle";
            toolStripMenuItemSortTitle.Size = new Size(110, 22);
            toolStripMenuItemSortTitle.Text = "タイトル";
            toolStripMenuItemSortTitle.Click += toolStripMenuItemSortTitle_Click;
            // 
            // toolStripMenuItemSortRandom
            // 
            toolStripMenuItemSortRandom.Name = "toolStripMenuItemSortRandom";
            toolStripMenuItemSortRandom.Size = new Size(110, 22);
            toolStripMenuItemSortRandom.Text = "ランダム";
            toolStripMenuItemSortRandom.Click += toolStripMenuItemSortRandom_Click;
            // 
            // toolStripMenuItemSortDuration
            // 
            toolStripMenuItemSortDuration.Name = "toolStripMenuItemSortDuration";
            toolStripMenuItemSortDuration.Size = new Size(110, 22);
            toolStripMenuItemSortDuration.Text = "曲長";
            toolStripMenuItemSortDuration.Click += toolStripMenuItemSortDuration_Click;
            // 
            // toolStripMenuItemPlayListClear
            // 
            toolStripMenuItemPlayListClear.Name = "toolStripMenuItemPlayListClear";
            toolStripMenuItemPlayListClear.Size = new Size(120, 22);
            toolStripMenuItemPlayListClear.Text = "クリア";
            toolStripMenuItemPlayListClear.Click += toolStripMenuItemPlayListClear_Click;
            // 
            // toolStripMenuItemPlayListSave
            // 
            toolStripMenuItemPlayListSave.Name = "toolStripMenuItemPlayListSave";
            toolStripMenuItemPlayListSave.Size = new Size(120, 22);
            toolStripMenuItemPlayListSave.Text = "保存";
            toolStripMenuItemPlayListSave.Click += toolStripMenuItemPlayListSave_Click;
            // 
            // toolStripMenuItemPlayListLoad
            // 
            toolStripMenuItemPlayListLoad.Name = "toolStripMenuItemPlayListLoad";
            toolStripMenuItemPlayListLoad.Size = new Size(120, 22);
            toolStripMenuItemPlayListLoad.Text = "読み込み";
            toolStripMenuItemPlayListLoad.Click += toolStripMenuItemPlayListLoad_Click;
            // 
            // panelSmallButton
            // 
            panelSmallButton.Controls.Add(buttonPlayListShuffle);
            panelSmallButton.Controls.Add(buttonOpenExplorer);
            panelSmallButton.Controls.Add(buttonSmallPause);
            panelSmallButton.Controls.Add(buttonSmallStop);
            panelSmallButton.Controls.Add(buttonSmallNext);
            panelSmallButton.Controls.Add(buttonSmallPrev);
            panelSmallButton.Controls.Add(buttonSmallPlay);
            panelSmallButton.Dock = DockStyle.Top;
            panelSmallButton.Location = new Point(0, 0);
            panelSmallButton.Name = "panelSmallButton";
            panelSmallButton.Size = new Size(366, 50);
            panelSmallButton.TabIndex = 7;
            // 
            // buttonOpenExplorer
            // 
            buttonOpenExplorer.ImageIndex = 7;
            buttonOpenExplorer.ImageList = imageListSmallIcon;
            buttonOpenExplorer.Location = new Point(217, 6);
            buttonOpenExplorer.Margin = new Padding(0);
            buttonOpenExplorer.Name = "buttonOpenExplorer";
            buttonOpenExplorer.Size = new Size(32, 32);
            buttonOpenExplorer.TabIndex = 4;
            buttonOpenExplorer.UseVisualStyleBackColor = false;
            buttonOpenExplorer.Click += buttonOpenExplorer_Click;
            // 
            // buttonSmallPause
            // 
            buttonSmallPause.ImageIndex = 2;
            buttonSmallPause.ImageList = imageListSmallIcon;
            buttonSmallPause.Location = new Point(159, 6);
            buttonSmallPause.Margin = new Padding(0);
            buttonSmallPause.Name = "buttonSmallPause";
            buttonSmallPause.Size = new Size(32, 32);
            buttonSmallPause.TabIndex = 3;
            buttonSmallPause.UseVisualStyleBackColor = false;
            buttonSmallPause.Click += buttonSmallPause_Click;
            // 
            // buttonSmallStop
            // 
            buttonSmallStop.ImageIndex = 6;
            buttonSmallStop.ImageList = imageListSmallIcon;
            buttonSmallStop.Location = new Point(38, 6);
            buttonSmallStop.Margin = new Padding(0);
            buttonSmallStop.Name = "buttonSmallStop";
            buttonSmallStop.Size = new Size(32, 32);
            buttonSmallStop.TabIndex = 2;
            buttonSmallStop.UseVisualStyleBackColor = false;
            buttonSmallStop.Click += buttonSmallStop_Click;
            // 
            // buttonSmallNext
            // 
            buttonSmallNext.ImageIndex = 1;
            buttonSmallNext.ImageList = imageListSmallIcon;
            buttonSmallNext.Location = new Point(114, 6);
            buttonSmallNext.Margin = new Padding(0);
            buttonSmallNext.Name = "buttonSmallNext";
            buttonSmallNext.Size = new Size(32, 32);
            buttonSmallNext.TabIndex = 1;
            buttonSmallNext.UseVisualStyleBackColor = false;
            buttonSmallNext.Click += buttonSmallNext_Click;
            // 
            // buttonSmallPrev
            // 
            buttonSmallPrev.ImageIndex = 4;
            buttonSmallPrev.ImageList = imageListSmallIcon;
            buttonSmallPrev.Location = new Point(82, 6);
            buttonSmallPrev.Margin = new Padding(0);
            buttonSmallPrev.Name = "buttonSmallPrev";
            buttonSmallPrev.Size = new Size(32, 32);
            buttonSmallPrev.TabIndex = 0;
            buttonSmallPrev.UseVisualStyleBackColor = false;
            buttonSmallPrev.Click += buttonSmallPrev_Click;
            // 
            // buttonSmallPlay
            // 
            buttonSmallPlay.ImageIndex = 3;
            buttonSmallPlay.ImageList = imageListSmallIcon;
            buttonSmallPlay.Location = new Point(6, 6);
            buttonSmallPlay.Margin = new Padding(0);
            buttonSmallPlay.Name = "buttonSmallPlay";
            buttonSmallPlay.Size = new Size(32, 32);
            buttonSmallPlay.TabIndex = 0;
            buttonSmallPlay.UseVisualStyleBackColor = false;
            buttonSmallPlay.Click += buttonSmallPlay_Click;
            // 
            // playingProgress1
            // 
            playingProgress1.Dock = DockStyle.Bottom;
            playingProgress1.Location = new Point(0, 166);
            playingProgress1.Name = "playingProgress1";
            playingProgress1.Padding = new Padding(0, 0, 3, 0);
            playingProgress1.Size = new Size(784, 19);
            playingProgress1.TabIndex = 9;
            // 
            // buttonChangePlayerMode
            // 
            buttonChangePlayerMode.BackColor = SystemColors.ControlDark;
            buttonChangePlayerMode.Dock = DockStyle.Fill;
            buttonChangePlayerMode.Location = new Point(327, 0);
            buttonChangePlayerMode.Margin = new Padding(0);
            buttonChangePlayerMode.Name = "buttonChangePlayerMode";
            buttonChangePlayerMode.Size = new Size(130, 15);
            buttonChangePlayerMode.TabIndex = 10;
            buttonChangePlayerMode.UseVisualStyleBackColor = false;
            buttonChangePlayerMode.Click += buttonChangePlayerMode_Click;
            // 
            // panelControl
            // 
            panelControl.Controls.Add(tableLayoutPanelControl);
            panelControl.Controls.Add(volumeControl1);
            panelControl.Controls.Add(playingProgress1);
            panelControl.Controls.Add(tableLayoutPanel1);
            panelControl.Dock = DockStyle.Top;
            panelControl.Location = new Point(0, 0);
            panelControl.Margin = new Padding(0);
            panelControl.Name = "panelControl";
            panelControl.Size = new Size(784, 200);
            panelControl.TabIndex = 11;
            // 
            // tableLayoutPanelControl
            // 
            tableLayoutPanelControl.ColumnCount = 2;
            tableLayoutPanelControl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanelControl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanelControl.Controls.Add(waveControl1, 1, 0);
            tableLayoutPanelControl.Controls.Add(panelControlLeft, 0, 0);
            tableLayoutPanelControl.Dock = DockStyle.Fill;
            tableLayoutPanelControl.Location = new Point(0, 0);
            tableLayoutPanelControl.Name = "tableLayoutPanelControl";
            tableLayoutPanelControl.RowCount = 1;
            tableLayoutPanelControl.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanelControl.Size = new Size(744, 166);
            tableLayoutPanelControl.TabIndex = 14;
            // 
            // waveControl1
            // 
            waveControl1.Dock = DockStyle.Fill;
            waveControl1.Location = new Point(375, 3);
            waveControl1.Name = "waveControl1";
            waveControl1.Size = new Size(366, 160);
            waveControl1.TabIndex = 8;
            // 
            // panelControlLeft
            // 
            panelControlLeft.Controls.Add(paleInfoControl1);
            panelControlLeft.Controls.Add(panelSmallButton);
            panelControlLeft.Dock = DockStyle.Fill;
            panelControlLeft.Location = new Point(3, 3);
            panelControlLeft.Name = "panelControlLeft";
            panelControlLeft.Size = new Size(366, 160);
            panelControlLeft.TabIndex = 9;
            // 
            // paleInfoControl1
            // 
            paleInfoControl1.Dock = DockStyle.Fill;
            paleInfoControl1.Location = new Point(0, 50);
            paleInfoControl1.Name = "paleInfoControl1";
            paleInfoControl1.Size = new Size(366, 110);
            paleInfoControl1.TabIndex = 8;
            // 
            // volumeControl1
            // 
            volumeControl1.Dock = DockStyle.Right;
            volumeControl1.Location = new Point(744, 0);
            volumeControl1.Name = "volumeControl1";
            volumeControl1.Size = new Size(40, 166);
            volumeControl1.TabIndex = 13;
            volumeControl1.TextVisible = true;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(buttonChangePlayerMode, 1, 0);
            tableLayoutPanel1.Dock = DockStyle.Bottom;
            tableLayoutPanel1.Location = new Point(0, 185);
            tableLayoutPanel1.Margin = new Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(784, 15);
            tableLayoutPanel1.TabIndex = 11;
            // 
            // buttonPlayListShuffle
            // 
            buttonPlayListShuffle.ImageIndex = 8;
            buttonPlayListShuffle.ImageList = imageListSmallIcon;
            buttonPlayListShuffle.Location = new Point(274, 6);
            buttonPlayListShuffle.Margin = new Padding(0);
            buttonPlayListShuffle.Name = "buttonPlayListShuffle";
            buttonPlayListShuffle.Size = new Size(32, 32);
            buttonPlayListShuffle.TabIndex = 5;
            buttonPlayListShuffle.UseVisualStyleBackColor = false;
            buttonPlayListShuffle.Click += buttonPlayListShuffle_Click;
            // 
            // MainForm
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 411);
            Controls.Add(listViewPlayList);
            Controls.Add(panelControl);
            DoubleBuffered = true;
            Icon = (Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Name = "MainForm";
            Text = "Pale Slumber";
            FormClosed += MainForm_FormClosed;
            Load += MainForm_Load;
            DragDrop += MainForm_DragDrop;
            DragEnter += MainForm_DragEnter;
            KeyDown += MainForm_KeyDown;
            contextMenuStripPaleMenu.ResumeLayout(false);
            panelSmallButton.ResumeLayout(false);
            panelControl.ResumeLayout(false);
            tableLayoutPanelControl.ResumeLayout(false);
            panelControlLeft.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private ImageList imageListSmallIcon;
        private ListView listViewPlayList;
        private Panel panelSmallButton;
        private Button buttonSmallPlay;
        private Button buttonSmallPause;
        private Button buttonSmallStop;
        private Button buttonSmallNext;
        private Button buttonSmallPrev;
        private PlayingProgress playingProgress1;
        private Button buttonChangePlayerMode;
        private Panel panelControl;
        private TableLayoutPanel tableLayoutPanel1;
        private VolumeControl volumeControl1;
        private TableLayoutPanel tableLayoutPanelControl;
        private Wave.WaveControl waveControl1;
        private Panel panelControlLeft;
        private PaleInfoControl paleInfoControl1;
        private ContextMenuStrip contextMenuStripPaleMenu;
        private ToolStripMenuItem toolStripMenuItemSort;
        private ToolStripMenuItem toolStripMenuItemSortDefault;
        private ToolStripMenuItem toolStripMenuItemSortTitle;
        private ToolStripMenuItem toolStripMenuItemSortRandom;
        private ToolStripMenuItem toolStripMenuItemPlayListSave;
        private ToolStripMenuItem toolStripMenuItemPlayListClear;
        private ToolStripMenuItem toolStripMenuItemSortDuration;
        private ToolStripMenuItem toolStripMenuItemPlayListLoad;
        private Button buttonOpenExplorer;
        private Button buttonPlayListShuffle;
    }
}
