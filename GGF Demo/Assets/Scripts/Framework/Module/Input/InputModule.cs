using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class InputModule:BaseModule,IInputModule
    {
        private Dictionary<KeyCode, InputButton> m_inputCache;
        private float curStopTime = 0f;

        public override int Priority
        {
            get
            {
                return 6;
            }
        }

        protected override void OnLoadModule()
        {
            m_inputCache = new Dictionary<KeyCode, InputButton>();
        }


        public InputButton Bind(KeyCode keyCode)
        {
            InputButton btn = null;
            if (!m_inputCache.TryGetValue(keyCode,out btn))
            {
                btn = new InputButton(keyCode);
                m_inputCache.Add(keyCode,btn);
            }

            return btn;
        }

        public void StopForWhile(float seconds)
        {
            curStopTime = seconds;
        }

        public void RemoveBtn(KeyCode keyCode)
        {
            if (m_inputCache.ContainsKey(keyCode))
            {
                m_inputCache.Remove(keyCode);
            }
        }

        public override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            if (curStopTime > 0f)
            {
                curStopTime -= elapseSeconds;
                return;
            }
            foreach (var KeybuttonPair in this.m_inputCache)
            {
                KeybuttonPair.Value.Check(Input.GetKey(KeybuttonPair.Key));
            }
        }
    }
}