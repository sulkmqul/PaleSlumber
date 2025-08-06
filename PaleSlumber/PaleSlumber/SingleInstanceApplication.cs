using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PaleSlumber
{
    /// <summary>
    /// 二重起動防止アプリケーションクラス
    /// </summary>
    /// <remarks>
    /// エントリポイントでusingして使います。DuplicateFlagをチェックしてtrueなら二重起動しています。
    /// </remarks>
    internal class SingleInstanceApplication : IDisposable
    {
        private static Mutex? SMu = null;

        /// <summary>
        /// 二重起動可否 true=二重起動中 false=新規
        /// </summary>
        public bool DuplicateFlag { get; private set; } = false;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="uname">固有名</param>
        public SingleInstanceApplication(string uname)
        {
            if (SingleInstanceApplication.SMu != null)
            {
                return;
            }

            bool f = false;
            SingleInstanceApplication.SMu = new Mutex(true, uname, out f);
            this.DuplicateFlag = !f;

        }

        /// <summary>
        /// 他の自分と同一のアプリを有効化する。
        /// </summary>
        public void ActivateAnother()
        {
            //自分のプロセス取得
            Process mpro = Process.GetCurrentProcess();

            //同じ名前の別プロセスを探す
            Process[] provec = Process.GetProcessesByName(mpro.ProcessName);
            foreach (var proc in provec)
            {
                //自分以外
                if (proc.Id == mpro.Id)
                {
                    continue;
                }

                // プロセスのメインウィンドウハンドルを取得
                var hWnd = proc.MainWindowHandle;
                if (hWnd != IntPtr.Zero)
                {
                    // ウィンドウを元のサイズに戻す（最小化解除）
                    ShowWindow(hWnd, SW_RESTORE);
                    // ウィンドウをアクティブ化
                    SetForegroundWindow(hWnd);
                    break;
                }
                
            }
        }
            

        /// <summary>
        /// 開放
        /// </summary>
        public void Dispose()
        {
            SingleInstanceApplication.SMu?.ReleaseMutex();
            SingleInstanceApplication.SMu =null;
        }


        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_RESTORE = 9;
    }
}
