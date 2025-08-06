using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaleSlumber
{

    /// <summary>
    /// アプリケーション設定ファイル
    /// </summary>
    internal class PaleAppSetting
    {
        /// <summary>
        /// アプリケーションディレクトリ作成
        /// </summary>
        /// <returns></returns>
        public static string CreateApplicationDirectory()
        {
            string ans = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + 
                System.IO.Path.DirectorySeparatorChar + 
                PaleConst.ApplicationDirectoryName;

            return ans;
        }

        /// <summary>
        /// 基本設定保存
        /// </summary>
        public void Save()
        {
            //system設定の保存
            string systempath = this.CreateSystemAppSettingPath();

            //デフォルトプレイリスト
            string plistpath = this.CreateSystemPlayListFilePath();
        }

        /// <summary>
        /// 読み込み
        /// </summary>
        public void Load()
        {
        }

        /// <summary>
        /// 全体設定ファイルの保存パス作成
        /// </summary>
        /// <returns></returns>
        private string CreateSystemAppSettingPath()
        {
            //固有設定
            string ans = CreateApplicationDirectory();
            ans += System.IO.Path.DirectorySeparatorChar + "paleslumber.setting";

            return ans;
        }

        /// <summary>
        /// 既定プレイリストファイルパスの作成
        /// </summary>
        /// <returns></returns>
        private string CreateSystemPlayListFilePath()
        {
            string ans = CreateApplicationDirectory();
            ans += System.IO.Path.DirectorySeparatorChar + $"default{PaleConst.PaleSumberPlayListFileExtension}";

            return ans;
        }

        
    }
}
