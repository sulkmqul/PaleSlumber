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

        private PlayListGrid Grid { get; init; }
        //--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
        /// <summary>
        /// ��ʏ�����
        /// </summary>
        private void InitForm()
        {
            //�v���C���X�g������
            this.FData.PlayList.Init();

            //�v���C���[���[�h�ݒ�
            this.ChangePlayerMode(EPlayerMode.Normal);

            //�v���C���X�gGrid�̏�����
            this.Grid.Init();
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
            this.FData.EventSub.OnNext(ev);
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
            //��ʏ�����1
            this.InitForm();

            /*
            {
                this.listViewPlayList.Items.Clear();
                this.listViewPlayList.GridLines = true;
                ColumnHeader[] hvec = {
                new ColumnHeader() { Text = "�^�C�g��", Width = 150 },
                new ColumnHeader() { Text = "�Ȓ�", Width = 100 },
                new ColumnHeader() { Text = "�p�X", Width = 200 }
            };
                this.listViewPlayList.Columns.AddRange(hvec);

                this.listViewPlayList.Items.Add(new ListViewItem(new string[] { "abc", "4:0", "path" }));
            }*/

            
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
                this.ShowError("�ǂݍ��ݎ��s");
            }

        }
    }
}
