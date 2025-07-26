using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace PaleSlumber
{
    /// <summary>
    /// プレイリストファイル
    /// </summary>
    internal class PlayListFile
    {

        private const string ROOT_NAME = "PaleSlumberPlayList";
        private const string ITEM_NAME = "Path";

        private const string ATTR_SEQ = "seq";

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
                    //全要素読み込み
                    var itemvec = root.Elements(ITEM_NAME);
                    foreach (var item in itemvec)
                    {
                        int seq = Convert.ToInt32(item.Attribute(ATTR_SEQ)?.Value ?? "0");
                        string path = item.Value;

                        //読み込みを行う
                        PlayListFileData ans = new PlayListFileData();
                        ans.Load(path, seq);
                        anslist.Add(ans);                     
                        
                    }

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
                using (FileStream fp = new FileStream(filepath, FileMode.Create, FileAccess.Write))
                {
                    XElement rele = new XElement(ROOT_NAME);
                    foreach (var pdata in plist)
                    {
                        var ele = new XElement(ITEM_NAME);
                        ele.SetAttributeValue(ATTR_SEQ, pdata.SeqNo);
                        ele.Value = pdata.FilePath;
                        rele.Add(ele);
                    }

                    rele.Save(fp);
                }
            }
            catch (Exception ex)
            {                
                throw;
            }
        }
    }
}
