using System;
using System.Collections;
using UnityEngine;

namespace Framework
{
    public class CoroutineModule:BaseModule,ICoroutineModule
    {
        private CoroutineManager m_coroutineMgr = null;

        protected override void OnLoadModule()
        {
            InitInstance();
        }
    
        private void InitInstance()
        {
            Transform tran = GameEntry.instance.transform.Find("CoroutineModule");
            if (tran != null)
            {
                m_coroutineMgr=tran.GetComponent<CoroutineManager>();
                if (m_coroutineMgr == null)
                {
                    m_coroutineMgr = tran.gameObject.AddComponent<CoroutineManager>();
                }
                return;
            }
            
            //找不到对应模块实例
            GameObject go = new GameObject("CoroutineModule");
            go.transform.SetParent(GameEntry.instance.transform);
            m_coroutineMgr=go.AddComponent<CoroutineManager>();
        }

        

        public void CreateCoroutine(IEnumerator coroutine, Action callback = null)
        {
            m_coroutineMgr.CreateCoroutine(coroutine,callback);
        }
    }
}
