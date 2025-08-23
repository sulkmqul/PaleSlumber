using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PaleSlumber
{
    class PaleSystemConfigData
    {
        /// <summary>
        /// 再生Seq番号
        /// </summary>
        public int CurrentSeq { get; set; } = 0;

        /// <summary>
        /// 保存データ
        /// </summary>
        public List<PlayListItem> PlayListItemList { get; set; } = new List<PlayListItem>();

        
    }


    /// <summary>
    /// アプリケーション設定ファイル
    /// </summary>
    internal class PaleSlumberSystemConfig
    {
        /// <summary>
        /// ファイル名
        /// </summary>
        private const string ConfigFileName = "paleslumber.conf";

        private const string ROOT_NAME = "PaleSlumberSystemConfig";

        private const string GLOBAL_NAME = "Global";
        private const string CURRENT_SEQ_NAME = "CurrentSeq";
        private const string PLAYLIST_NAME = "PlayList";



        /// <summary>
        /// 基本設定保存
        /// </summary>
        public void Save(PaleSystemConfigData sdata)
        {
            try
            {
                //ルート作成
                XElement root = new XElement(ROOT_NAME);

                //全体設定
                var system = this.SaveGlobalConfig(sdata);
                root.Add(system);

                //プレイリスト
                var pele = PlayListFile.SaveElement(PLAYLIST_NAME, sdata.PlayListItemList);
                root.Add(pele);

                //保存フォルダ作成
                this.CreateApplicationDirectory();
                //system設定の保存
                string systempath = this.CreateSystemAppSettingPath();

                using (FileStream fp = new FileStream(systempath, FileMode.Create))
                {
                    root.Save(fp);
                }
            }
            catch(Exception ex)
            {
                throw;
            }            
        }

        /// <summary>
        /// 読み込み
        /// </summary>
        public PaleSystemConfigData Load()
        {
            try
            {
                //読み込みパスの作成
                string syspath = this.CreateSystemAppSettingPath();

                PaleSystemConfigData ans = new PaleSystemConfigData();

                using (FileStream fp = new FileStream(syspath, FileMode.Open))
                {
                    //対象の名前？
                    XElement root = XElement.Load(fp);
                    if (root.Name != ROOT_NAME)
                    {
                        throw new Exception("no root name");
                    }
                    //全体設定読み込み
                    bool f = this.LoadGlobalConfig(root, ref ans);
                    if (f == false)
                    {
                        throw new Exception("no global name");
                    }

                    //プレイリスト読み込み
                    var pe = root.Element(PLAYLIST_NAME);
                    if (pe == null)
                    {
                        throw new Exception("no playlist name");
                    }
                    ans.PlayListItemList = PlayListFile.LoadElement(pe);
                }

                return ans;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        //--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
        /// <summary>
        /// 全体設定の保存
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private XElement SaveGlobalConfig(PaleSystemConfigData data)
        {
            XElement ans = new XElement(GLOBAL_NAME);

            XElement eseq = new XElement(CURRENT_SEQ_NAME);
            eseq.Value = data.CurrentSeq.ToString();
            ans.Add(eseq);

            return ans;
        }

        /// <summary>
        /// 全体設定の読み込み
        /// </summary>
        /// <param name="rele"></param>
        /// <param name="rdata"></param>
        /// <returns></returns>
        private bool LoadGlobalConfig(XElement rele, ref PaleSystemConfigData rdata)
        {
            var gele = rele.Element(GLOBAL_NAME);
            if (gele == null)
            {
                return false;
            }
            var ce = gele.Element(CURRENT_SEQ_NAME);
            if (ce == null)
            {
                return false;
            }
            rdata.CurrentSeq = Convert.ToInt32(ce.Value);


            return true;
        }

        /// <summary>
        /// アプリケーションディレクトリパス作成
        /// </summary>
        /// <returns></returns>
        private string CreateApplicationDirectoryPath()
        {
            string ans = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                System.IO.Path.DirectorySeparatorChar +
                PaleConst.ApplicationDirectoryName;

            return ans;
        }

        /// <summary>
        /// アプリケーションディレクトリの作成
        /// </summary>
        private void CreateApplicationDirectory()
        {
            string adpath = this.CreateApplicationDirectoryPath();
            bool eret = Directory.Exists(adpath);
            if (eret == true)
            {
                return;
            }
            Directory.CreateDirectory(adpath);
        }

        /// <summary>
        /// 全体設定ファイルの保存パス作成
        /// </summary>
        /// <returns></returns>
        private string CreateSystemAppSettingPath()
        {
            //固有設定
            string ans = this.CreateApplicationDirectoryPath();
            ans += System.IO.Path.DirectorySeparatorChar + PaleSlumberSystemConfig.ConfigFileName;

            return ans;
        }


        
    }
}
