
using UnityEngine;
using UnityEngine.UI;

namespace Logic
{
    public class SkyMenuView
    {
        public SkyMenuView(Transform uiTransform)
        {
            this.uiTransform = uiTransform;
        }
        
        public Button StartBtn
        {
            get
            {
                if (m_StartBtn == null)
                {
                    m_StartBtn=uiTransform.transform.Find("StartBtn").GetComponent<Button>();
                }

                return m_StartBtn;
            }
        }
        
        
        public Button ExitBtn
        {
            get
            {
                if (m_ExitBtn == null)
                {
                    m_ExitBtn=uiTransform.transform.Find("ExitBtn").GetComponent<Button>();
                }

                return m_ExitBtn;
            }
        }
        
        
        
        private Transform uiTransform;
        private Button m_StartBtn;
        private Button m_ExitBtn;
    }
}

