using System.Reactive.Linq;
using System.Runtime.CompilerServices;

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

            //�v���C���[���[�h�ݒ�
            this.ChangePlayerMode(EPlayerMode.Normal);


            this.AddRollEventProc();

            //�����C�x���g����
            this.Core.RollEvent.Subscribe(x =>
            {
                this.RollEventTable.Execute(x);
            });
        }

        /// <summary>
        /// �����C�x���g�̒ǉ�
        /// </summary>
        private void AddRollEventProc()
        {
            //Playlist�̒ǉ�����
            this.RollEventTable.AddEvent(EPaleSlumberEvent.PlayListAdd, (x) => this.Grid.DisplayList());
            this.RollEventTable.AddEvent(EPaleSlumberEvent.PlayListRemove, (x) => this.Grid.DisplayList());
            this.RollEventTable.AddEvent(EPaleSlumberEvent.PlayListOrderManualChanged, (x) => this.Grid.DisplayList());

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

            bool[] listvisible = { false, true };

            //�l�̐ݒ�
            this.MaximumSize = limitsize[n];
            this.MinimumSize = limitsize[n];
            this.Size = fsize[n];
            this.panelControl.Height = ch[n];
            this.listViewPlayList.Visible = listvisible[n];
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
            if (this.FData.PlayList.SelectedFile == null)
            {
                return;
            }
            this.PublishEvent(EPaleSlumberEvent.PlayStart, this.FData.PlayList.SelectedFile);
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
            if (this.FData.PlayList.SelectedFile == null)
            {
                return;
            }
            this.PublishEvent(EPaleSlumberEvent.PlayStart, this.FData.PlayList.SelectedFile);
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

        }

        /// <summary>
        /// ���փ{�^���������ꂽ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSmallNext_Click(object sender, EventArgs e)
        {

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

        private void listViewPlayList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {

        }
    }
}
