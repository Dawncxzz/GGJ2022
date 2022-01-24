using System.Collections.Generic;
using Define;

namespace Framework
{
    public delegate void MessageEvent(Message message);
    public class EventMessageSender:Singleton<EventMessageSender>
    {
        public Dictionary<string, List<MessageEvent>> messageEventDic = new Dictionary<string, List<MessageEvent>>();
        
        public void AddListener(string messageName, MessageEvent messageEvent)
        {
            List<MessageEvent> messageList = null;
            if (messageEventDic.ContainsKey(messageName))
            {
                messageList = messageEventDic[messageName];
            }
            else
            {
                messageList = new List<MessageEvent>();
                messageEventDic.Add(messageName,messageList);
            }

            if (!messageList.Contains(messageEvent))
            {
                messageList.Add(messageEvent);
            }
        }

        public void RemoveListener(string messageName, MessageEvent messageEvent)
        {
            if (!messageEventDic.ContainsKey(messageName))
            {
                return;
            }

            List<MessageEvent> list = messageEventDic[messageName];
            if (list.Contains(messageEvent))
            {
                list.Remove(messageEvent);
            }

            if (list.Count <= 0)
            {
                messageEventDic.Remove(messageName);
            }
        }

        public void RemoveOneListener(string messageName)
        {
            if (messageEventDic.ContainsKey(messageName))
            {
                messageEventDic.Remove(messageName);
            }
        }

        public void RemoveAllListener()
        {
            messageEventDic.Clear();
        }

        #region Send Message

        public void SendMessage(Message message)
        {
            DispatchMessage(message);
        }

        public void SendMessage(string messageName, object sender, object content)
        {
            Message message = new Message(messageName, sender, content);
            DispatchMessage(message);
        }

        //消息分发
        public void DispatchMessage(Message message)
        {
            List<MessageEvent> list = null;
            if (messageEventDic != null && messageEventDic.ContainsKey(message.Name))
            {
                list = messageEventDic[message.Name];
            }

            if (list != null)
            {
                foreach (var messageEvent in list)
                {
                    messageEvent(message);
                }
            }
        }
        
        
        #endregion
    }
}