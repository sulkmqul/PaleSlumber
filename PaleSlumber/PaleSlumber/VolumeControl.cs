using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaleSlumber
{
    public partial class VolumeControl : UserControl
    {
        public VolumeControl()
        {
            InitializeComponent();
        }

        public IObservable<int> VolumeStream
        {
            get
            {
                return this.VolumeSub;
            }
        }

        /// <summary>
        /// 音量の設定
        /// </summary>
        public int Volume
        {
            set
            {
                this.trackBarVolume.Value = value;
            }
        }

        /// <summary>
        /// テキスト表示可否の設定
        /// </summary>
        public bool TextVisible
        {
            get
            {
                return this.labelVolumeText.Visible;
            }
            set
            {
                this.labelVolumeText.Visible = value;
            }
        }


        private Subject<int> VolumeSub = new Subject<int>();

        private void VolumeControl_Load(object sender, EventArgs e)
        {

        }

        private void trackBarVolume_ValueChanged(object sender, EventArgs e)
        {
            int v = this.trackBarVolume.Value;
            this.labelVolumeText.Text = $"{v}";
            this.VolumeSub.OnNext(v);
        }
    }
}
