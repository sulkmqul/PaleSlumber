using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaleSlumber
{
    internal class PlayListSort
    {
        public delegate List<PlayListFileData> SortDelegate(List<PlayListFileData> plist);

        /// <summary>
        /// 既定(登録順)に並べ替え
        /// </summary>
        /// <param name="plist"></param>
        /// <returns></returns>
        public static List<PlayListFileData> SortDefault(List<PlayListFileData> plist)
        {
            return plist.OrderBy(x => x.SeqNo).ToList();
        }

        /// <summary>
        /// タイトルでソート
        /// </summary>
        /// <param name="plist"></param>
        /// <returns></returns>
        public static List<PlayListFileData> SortTitle(List<PlayListFileData> plist)
        {
            return plist.OrderBy(x => x.FileName).ToList();
        }

        /// <summary>
        /// ランダム並べ替え
        /// </summary>
        /// <param name="plist"></param>
        /// <returns></returns>
        public static List<PlayListFileData> SortRandom(List<PlayListFileData> plist)
        {
            //Fisher–Yates法というらしい

            Random rand = new Random();
            for (int i = plist.Count - 1; i > 0; i--)
            {
                int rp = rand.Next(i + 1);
                PlayListFileData temp = plist[rp];
                plist[rp] = plist[i];
                plist[i] = temp;
            }

            return plist;
        }

        /// <summary>
        /// 曲長で並べ替え
        /// </summary>
        /// <param name="plist"></param>
        /// <returns></returns>
        public static List<PlayListFileData> SortDuration(List<PlayListFileData> plist)
        {
            return plist.OrderBy(x => x.TotalTime.TotalMilliseconds).ToList();            
        }
    }
}
