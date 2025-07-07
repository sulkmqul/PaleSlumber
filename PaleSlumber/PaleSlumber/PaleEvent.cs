namespace PaleSlumber
{
    /// <summary>
    /// イベント一覧
    /// </summary>
    enum EPaleSlumberEvent
    {
        /// <summary>
        /// 再生開始
        /// </summary>
        PlayStart,
        /// <summary>
        /// 一時停止
        /// </summary>
        PlayPause,
        /// <summary>
        /// 再生停止
        /// </summary>
        PlayStop,
        /// <summary>
        /// 次の曲へ
        /// </summary>
        PlayNext,
        /// <summary>
        /// 前の曲へ
        /// </summary>
        PlayPrev,
        /// <summary>
        /// プレイリストに追加
        /// </summary>
        PlayListAdd,
        /// <summary>
        /// プレイリストを削除
        /// </summary>
        PlayListRemove,
        /// <summary>
        /// プレイリストを初期化
        /// </summary>
        PlayListInit,   
        /// <summary>
        /// プレイリストの選択変更
        /// </summary>
        PlayListSelectedChanged,
        /// <summary>
        /// プレイリストの手動順番変更
        /// </summary>
        PlayListOrderManualChanged,

        /// <summary>
        /// 音量変更
        /// </summary>
        VolumeChanged,
    }

    /// <summary>
    /// イベント情報
    /// </summary>
    class PaleEvent
    {
        public PaleEvent(EPaleSlumberEvent ev, object param)
        {
            Event = ev;
            EventParam = param;
        }

        /// <summary>
        /// イベント
        /// </summary>
        public EPaleSlumberEvent Event { get; init; }

        /// <summary>
        /// パラメータ
        /// </summary>
        public object EventParam { get; init; }

        #region よく使うパラメータの型変換を定義しておく
        public string ParamString
        {
            get
            {
                string s = this.EventParam.ToString() ?? "";
                return s;
            }
        }
        public string[] ParamStringArray
        {
            get
            {
                var svec = this.EventParam as string[];
                if (svec == null)
                {
                    return new string[] { "" };
                }
                return svec;
                
            }
        }

        /// <summary>
        /// パラメータを取得
        /// </summary>
        public PlayListFileData? ParamPlayListFile
        {
            get
            {
                var svec = this.EventParam as PlayListFileData;
                return svec;
            }
        }
        public PlayListFileData[] ParamPlayListFileArray
        {
            get
            {
                var svec = this.EventParam as PlayListFileData[];
                if (svec == null)
                {
                    return new PlayListFileData[] { };
                }
                return svec;
            }
        }
        #endregion

        /// <summary>
        /// イベント確認
        /// </summary>
        /// <param name="ev"></param>
        /// <returns></returns>
        public bool CheckEvent(EPaleSlumberEvent ev)
        {
            return this.CheckEvent(new EPaleSlumberEvent[] { ev });
        }
        /// <summary>
        /// イベント確認
        /// </summary>
        /// <param name="evec"></param>
        /// <returns></returns>
        public bool CheckEvent(EPaleSlumberEvent[] evec)
        {
            int n = evec.Where(x => x == this.Event).Count();
            if (n > 0)
            {
                return true;
            }
            return false;
        }
    }



    /// <summary>
    /// イベント処理管理
    /// </summary>
    class PaleEventIgniter
    {
        public PaleEventIgniter()
        {
            this.Clear();
        }

        private class IgniterData
        {
            public IgniterData(Action<PaleEvent> ac, Type? dt)
            {
                this.Ac = ac;
                this.DataType = dt;
            }


            /// <summary>
            /// 起動アクション
            /// </summary>
            public Action<PaleEvent> Ac { get; init; }
            /// <summary>
            /// 引数データタイプ
            /// </summary>
            public Type? DataType { get; init; }

        }

        /// <summary>
        /// 処理テーブル
        /// </summary>
        private Dictionary<EPaleSlumberEvent, IgniterData> Dic = new Dictionary<EPaleSlumberEvent, IgniterData>();

        /// <summary>
        /// イベント追加
        /// </summary>
        /// <param name="ev"></param>
        /// <param name="ac"></param>
        public void AddEvent(EPaleSlumberEvent ev, Type type, Action<PaleEvent> ac)
        {
            this.Dic.Add(ev, new IgniterData(ac, type));
        }
        public void AddEvent(EPaleSlumberEvent ev, Action<PaleEvent> ac)
        {
            this.Dic.Add(ev, new IgniterData(ac, null));
        }

        /// <summary>
        /// イベントクリア
        /// </summary>
        public void Clear()
        {
            this.Dic.Clear();
        }

        /// <summary>
        /// イベント削除
        /// </summary>
        /// <param name="ev"></param>
        public void RemoveEvent(EPaleSlumberEvent ev)
        {
            this.Dic.Remove(ev);
        }

        /// <summary>
        /// 実行
        /// </summary>
        /// <param name="ev">実行対象</param>
        /// <returns>実行可否 true=処理できた</returns>
        public bool Execute(PaleEvent ev)
        {
            bool f = this.Dic.ContainsKey(ev.Event);
            if (f == false)
            {
                return false;
            }

            //適切なデータかのチェック
            var data = this.Dic[ev.Event];
            bool df = ev.EventParam.GetType().Equals(data.DataType);
            if (df == false && data.DataType != null)
            {
                return false;
            }

            //起動
            this.Dic[ev.Event].Ac.Invoke(ev);
            return true;
        }
    }
}
