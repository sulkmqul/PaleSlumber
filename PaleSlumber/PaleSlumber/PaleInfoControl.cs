using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaleSlumber
{
    /// <summary>
    /// 画面表示コントロール
    /// </summary>
    public partial class PaleInfoControl : UserControl
    {
        public PaleInfoControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            Label[] lvec = {
                this.labelTitle,
                this.labelPath,                
            };
            foreach (var label in lvec)
            {
                label.Text = "";
            }
        }

        internal void LoadFile(PlayListFileData fdata)
        {
            this.labelTitle.Text = fdata.FileName;
            this.labelPath.Text = fdata.FilePath;


        }

        private void PaleInfoControl_Load(object sender, EventArgs e)
        {

        }
    }
}
