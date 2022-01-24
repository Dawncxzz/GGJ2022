using System.Collections.Generic;

namespace Framework
{
    public class Message
    {
        private Dictionary<string, object> dataDic = null;

        /// <summary>
        /// 消息名
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 发送方
        /// </summary>
        public object Sender { get; private set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public object Content { get; set; }

        private bool ContainsData(string key)
        {
            return dataDic.ContainsKey(key);
        }

        #region Indexer

        //MARKER:索引器存储消息的数据
        public object this[string key]
        {
            get
            {
                if (dataDic != null && ContainsData(key))
                {
                    return dataDic[key];
                }
                else return null;
            }
            set
            {
                if (dataDic == null)
                {
                    dataDic = new Dictionary<string, object>();
                }

                dataDic[key] = value;
            }
        }


        #endregion

        #region Construct

        public Message(string name, object sender, object content = null)
        {
            this.Name = name;
            this.Sender = sender;
            this.Content = content;
        }

        public Message(string name, object sender, object content, params object[] _dataDic)
        {
            this.Name = name;
            this.Sender = sender;
            this.Content = content;

            if (_dataDic.GetType() == typeof(Dictionary<string, object>))
            {
                foreach (KeyValuePair<string, object> _dataParam in _dataDic)
                {
                    dataDic[_dataParam.Key] = _dataParam.Value;
                }
            }
        }

        #endregion

        #region Add &Remove

        public void Add(string key, object value)
        {
            this[key] = value;
        }

        public void Remove(string key)
        {
            if (dataDic != null && ContainsData(key))
            {
                dataDic.Remove(key);
            }
        }

        #endregion
    }

}