using System.Diagnostics;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PaleSlumber
{
    /// <summary>
    /// ���C�����
    /// </summary>
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            //�v���C���X�gGrid�Ǘ�������
            this.Grid = new PlayListGrid(this.listViewPlayList);
        }



        /// <summary>
        /// ��ʃf�[�^
        /// </summary>
        private PaleGlobal FData
        {
            get
            {
                return PaleGlobal.Mana;
            }
        }

        /// <summary>
        /// �R�A����
        /// </summary>
        private PaleSlumberCore Core { get; init; } = new PaleSlumberCore();

        /// <summary>
        /// �O���b�h�Ǘ�
        /// </summary>
        private PlayListGrid Grid { get; init; }

        /// <summary>
        /// �C�x���g����
        /// </summary>
        private PaleEventIgniter RollEventTable { get; init; } = new PaleEventIgniter();
        //--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
        /// <summary>
        /// ��ʏ�����
        /// </summary>
        private void InitForm()
        {
            //�R�A����������
            this.Core.Init();

            //�v���C���X�gGrid�̏�����
            this.Grid.Init();

            //��񏉊���
            this.paleInfoControl1.Init();

            //�g�`�\��������
            this.waveControl1.Init();

            //�v���C���[���[�h�ݒ�
            this.ChangePlayerMode(EPlayerMode.Normal);

            //�C�x���g����������
            this.AddRollEventProc();

            //�����C�x���g����
            this.Core.RollEvent.Subscribe(x =>
            {
                this.RollEventTable.Execute(x);
            });

            //���ʂ̕ύX
            this.volumeControl1.VolumeStream.Select(x => this.CalcuVolumeValue(x)).Subscribe(vol =>
            {
                this.PublishEvent(EPaleSlumberEvent.VolumeChanged, vol);
            });

            //�Đ����̏���
            this.FData.Player.PlayingStream.Subscribe(x =>
            {
                this.PlayingEventProc(x);
            });

            //�����l�̐ݒ�
            this.volumeControl1.Volume = 50;

            //������
            this.PublishEvent(EPaleSlumberEvent.Initialize);


            //�����w��̂��̂�ǂݍ���
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length <= 1)
            {
                return;
            }
            this.PublishEvent(EPaleSlumberEvent.PlayListAdd, new string[] { args[1] });

        }

        /// <summary>
        /// �����C�x���g�̒ǉ�
        /// </summary>
        private void AddRollEventProc()
        {
            //Playlist�̒ǉ�����
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
                    this.ShowError("�������ݎ��s");
                }
            });



        }

        /// <summary>
        /// �\�����[�h���ύX���ꂽ��
        /// </summary>
        /// <param name="mode"></param>
        private void ChangePlayerMode(EPlayerMode mode)
        {
            this.FData.Mode = mode;
            int n = (int)mode;

            //�e���[�h�ɂ�����ύX�l���`
            //��ʃT�C�Y
            Size[] fsize = { PaleConst.MiniModeFormSize, PaleConst.NormalModeDefaultSize };
            //����p�l������
            int[] ch = { PaleConst.MiniModeControlHeight, PaleConst.NormalModeControlHeight };
            //��ʃT�C�Y����
            Size[] limitsize = { PaleConst.MiniModeFormSize, new Size(0, 0) };
            //�\����
            bool[] modevisible = { false, true };

            //�R���g���[���������̃T�C�Y
            int[] tablecontrolwidth = { PaleConst.MiniModeControlLeftWidthPixel, PaleConst.NormalModeControlLeftWidthPercent };
            //�R���g���[���������̃T�C�Y���
            SizeType[] tabletype = { SizeType.Absolute, SizeType.Percent };

            //�l�̐ݒ�
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
        /// �G���[�̕\��
        /// </summary>
        /// <param name="message"></param>
        private void ShowError(string message)
        {
            MessageBox.Show(this, message, PaleConst.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// �C�x���g�̔��s
        /// </summary>
        /// <param name="ev"></param>
        private void PublishEvent(EPaleSlumberEvent ev)
        {
            this.FData.EventSub.OnNext(new PaleEvent(ev, ""));
        }
        /// <summary>
        /// �C�x���g�̔��s
        /// </summary>
        /// <param name="ev"></param>
        /// <param name="param"></param>
        private void PublishEvent(EPaleSlumberEvent ev, object param)
        {
            this.FData.EventSub.OnNext(new PaleEvent(ev, param));
        }

        /// <summary>
        /// �������p�̉��ʂ��v�Z����
        /// </summary>
        /// <returns></returns>
        private float CalcuVolumeValue(int vol)
        {
            float ans = 0;

            ans = (float)vol / 100.0f;

            return ans;
        }

        /// <summary>
        /// �Đ��J�n
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
        /// �Đ�����
        /// </summary>
        /// <param name="pf"></param>
        private void PlayingEventProc(PlayingInfo pf)
        {
            if (pf.Event == EPlayingEvent.Start)
            {
                //�Đ���񏉊���
                this.paleInfoControl1.LoadFile(pf.PlayFile);
                this.ChangeFormTitle(pf.PlayFile);
                return;
            }
            if (pf.Event == EPlayingEvent.Stop)
            {
                //�Đ����I�������玟���Đ�
                this.PublishEvent(EPaleSlumberEvent.PlayNext);
                return;
            }
            //�Đ��ʒu�̐ݒ�
            this.playingProgress1.ProgressPlaying(pf.TotalTime, pf.NowTime);

            //�g�`����
            this.waveControl1.PushWave(pf.WaveBuffer);
        }


        /// <summary>
        /// ��ʃ^�C�g���̕ύX
        /// </summary>
        /// <param name="pf">�\���^�C�g�� null=�Ȃ�</param>
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
        /// �ǂݍ��܂ꂽ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MainForm_Load(object sender, EventArgs e)
        {
            //��ʏ�����
            this.InitForm();
        }

        /// <summary>
        /// ����ꂽ�Ƃ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //�V�X�e���t�@�C���ۑ�
            PaleGlobal.Mana.SaveSystemConfig();

            //��n��
            PaleGlobal.Mana.Dispose();
            this.Core.Dispose();
        }

        /// <summary>
        /// ��ʃ��[�h�̐؂�ւ��{�^���������ꂽ��
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
        /// �h���b�O�A���h�h���b�v�����Ƃ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            //�t�@�C���p�X�ȊO�͖�������
            var f = e.Data?.GetDataPresent(DataFormats.FileDrop);
            if (f != true)
            {
                return;
            }

            e.Effect = DragDropEffects.Link;

        }

        /// <summary>
        /// �h���b�O�h���b�v���ꂽ��
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
                this.ShowError("�ǂݍ��ݎ��s");
            }

        }

        /// <summary>
        /// Gird��}�E�X���������Ƃ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewPlayList_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e)
        {

        }

        /// <summary>
        /// Grid���_�u���N���b�N���ꂽ�Ƃ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewPlayList_DoubleClick(object sender, EventArgs e)
        {
            this.PlayStart();
        }

        /// <summary>
        /// �L�[�������ꂽ��
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
        /// listview�̑I�����ύX���ꂽ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewPlayList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// listview�}�E�X�������ꂽ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewPlayList_MouseDown(object sender, MouseEventArgs e)
        {
            this.Grid.DownMouse(e);

            //�I���̕ύX
            this.PublishEvent(EPaleSlumberEvent.PlayListSelectedChanged, this.Grid.SelectedFileList.ToArray());
        }
        /// <summary>
        /// listview�}�E�X���������Ƃ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewPlayList_MouseMove(object sender, MouseEventArgs e)
        {
            this.Grid.MoveMouse(e);
        }

        /// <summary>
        /// listview�}�E�X�������ꂽ��
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
            //���ԕύX
            this.PublishEvent(EPaleSlumberEvent.PlayListOrderManualChanged, index);
        }

        /// <summary>
        /// �Đ��J�n�{�^���������ꂽ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSmallPlay_Click(object sender, EventArgs e)
        {
            this.PlayStart();
        }

        /// <summary>
        /// �Đ���~�{�^���������ꂽ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSmallStop_Click(object sender, EventArgs e)
        {
            this.PublishEvent(EPaleSlumberEvent.PlayStop);
        }

        /// <summary>
        /// �O�փ{�^���������ꂽ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSmallPrev_Click(object sender, EventArgs e)
        {
            this.PublishEvent(EPaleSlumberEvent.PlayPrev);
        }

        /// <summary>
        /// ���փ{�^���������ꂽ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSmallNext_Click(object sender, EventArgs e)
        {
            this.PublishEvent(EPaleSlumberEvent.PlayNext);
        }

        /// <summary>
        /// �ꎞ��~�{�^���������ꂽ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSmallPause_Click(object sender, EventArgs e)
        {
            this.PublishEvent(EPaleSlumberEvent.PlayPause);
        }

        /// <summary>
        /// listview�A�C�e���I���̕ύX��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewPlayList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {

        }

        /// <summary>
        /// menu �v���C���X�g���בւ� ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemSortDefault_Click(object sender, EventArgs e)
        {
            this.PublishEvent(EPaleSlumberEvent.PlayListSortDefault);
        }

        /// <summary>
        /// menu �v���C���X�g���בւ� �^�C�g��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemSortTitle_Click(object sender, EventArgs e)
        {
            this.PublishEvent(EPaleSlumberEvent.PlayListSortTitle);
        }

        /// <summary>
        /// menu �v���C���X�g���בւ� �����_��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemSortRandom_Click(object sender, EventArgs e)
        {
            this.PublishEvent(EPaleSlumberEvent.PlayListSortRandom);
        }

        /// <summary>
        /// menu �v���C���X�g���בւ� �Ȓ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemSortDuration_Click(object sender, EventArgs e)
        {
            this.PublishEvent(EPaleSlumberEvent.PlayListSortDuration);
        }

        /// <summary>
        /// menu �v���C���X�g�N���A
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemPlayListClear_Click(object sender, EventArgs e)
        {
            this.PublishEvent(EPaleSlumberEvent.PlayListClear);
        }

        /// <summary>
        /// menu �v���C���X�g�ۑ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemPlayListSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog diag = new SaveFileDialog();
            diag.Filter = $"PaleSlumber PlayList File(*{PaleConst.PaleSumberPlayListFileExtension})|*{PaleConst.PaleSumberPlayListFileExtension}|�S�Ẵt�@�C��(*.*)|*.*";
            var dret = diag.ShowDialog(this);
            if (dret != DialogResult.OK)
            {
                return;
            }
            string filepath = diag.FileName;
            this.PublishEvent(EPaleSlumberEvent.PlayListSaveFile, filepath);
        }

        /// <summary>
        /// menu �v���C���X�g�ǂݍ���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemPlayListLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog diag = new OpenFileDialog();
            diag.Filter = "PaleSlumber PlayList File(*.ppf)|*.ppf|�S�Ẵt�@�C��(*.*)|*.*";
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
        /// �Ώۃt�H���_���J��
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
