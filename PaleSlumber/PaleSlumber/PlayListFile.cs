using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace PaleSlumber
{
    class PlayListItem
    {        
        public int Seq { get; set; } = 0;
        public string FilePath { get; set; } = "";
    }


    /// <summary>
    /// プレイリストファイル
    /// </summary>
    internal class PlayListFile
    {

        private const string ROOT_NAME = "PaleSlumberPlayList";
        private const string ITEM_NAME = "Path";

        private const string ATTR_SEQ = "seq";

        /// <summary>
        /// プレイリストかの確認
        /// </summary>
        /// <param name="fpath">確認ファイルパス</param>
        /// <returns>true=プレイリストファイルである</returns>
        public static bool CheckPlayListFile(string fpath)
        {
            string ext = Path.GetExtension(fpath);
            if (ext == PaleConst.PaleSumberPlayListFileExtension)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// ファイル読み込み
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static List<PlayListFileData> Load(string filepath)
        {
            try
            {
                List<PlayListFileData> anslist = new List<PlayListFileData>();

                using (FileStream fp = new FileStream(filepath, FileMode.Open, FileAccess.Read))
                {
                    XElement root = XElement.Load(fp);
                    //ROOT要素の検索                    
                    if (root.Name != ROOT_NAME)
                    {
                        throw new Exception("not playlist files");
                    }

                    //読み込み
                    List<PlayListItem> plist = PlayListFile.LoadElement(root);
                    
                    anslist = plist.Select(x =>
                    {
                        PlayListFileData data = new PlayListFileData();
                        data.Load(x.FilePath, x.Seq);
                        return data;
                    }).ToList();
                }

                return anslist;
            }
            catch (Exception)
            {
                throw;
            }
        }

        

        /// <summary>
        /// ファイル保存
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="plist"></param>
        public static void Save(string filepath, List<PlayListFileData> plist)
        {
            try
            {
                //保存データ作成
                List<PlayListItem> ilist =
                    plist.Select(x => new PlayListItem() { FilePath = x.FilePath, Seq = x.SeqNo }).ToList();

                using (FileStream fp = new FileStream(filepath, FileMode.Create, FileAccess.Write))
                {
                    XElement rele = PlayListFile.SaveElement(ROOT_NAME, ilist);
                    rele.Save(fp);
                }
            }
            catch (Exception ex)
            {                
                throw;
            }
        }

        /// <summary>
        /// Element読み込み
        /// </summary>
        /// <param name="rname">ルート名</param>
        /// <param name="root">エレメント</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static List<PlayListItem> LoadElement(XElement root)
        {
            List<PlayListItem> anslist = new List<PlayListItem>();

            //全要素読み込み
            var itemvec = root.Elements(ITEM_NAME);
            foreach (var item in itemvec)
            {
                int seq = Convert.ToInt32(item.Attribute(ATTR_SEQ)?.Value ?? "0");
                string path = item.Value;

                //読み込みを行う
                PlayListItem ans = new PlayListItem(){ Seq = seq, FilePath = path };                
                anslist.Add(ans);

            }

            return anslist;
        }

        /// <summary>
        /// 保存Element作成
        /// </summary>
        /// <param name="rname">作成するroot名</param>
        /// <param name="plist">作成データ</param>
        /// <returns></returns>
        public static XElement SaveElement(string rname, List<PlayListItem> plist)
        {
            XElement ans = new XElement(rname);
            foreach (var pdata in plist)
            {
                var ele = new XElement(ITEM_NAME);
                ele.SetAttributeValue(ATTR_SEQ, pdata.Seq);
                ele.Value = pdata.FilePath;
                ans.Add(ele);
            }

            return ans;
        }
    }
}
