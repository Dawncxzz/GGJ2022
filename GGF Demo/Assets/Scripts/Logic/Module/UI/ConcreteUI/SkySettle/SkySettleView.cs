using UnityEngine;
using UnityEngine.UI;

namespace Logic
{
    public class SkySettleView
    {
        
        public SkySettleView(Transform uiTransform)
        {
            this.uiTransform = uiTransform;
        }
        
        public Button ReturnBtn
        {
            get
            {
                if (m_ReturnBtn == null)
                {
                    m_ReturnBtn=uiTransform.transform.Find("ReturnBtn").GetComponent<Button>();
                }

                return m_ReturnBtn;
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

        public Text CurScoreText
        {
            get
            {
                if (m_curScoreText == null)
                {
                    m_curScoreText = uiTransform.transform.Find("CurScore").Find("score").GetComponent<Text>();
                }

                return m_curScoreText;
            }
        }
        
        public Text MaxScoreText
        {
            get
            {
                if (m_maxScoreText == null)
                {
                    m_maxScoreText = uiTransform.transform.Find("MaxScore").Find("score").GetComponent<Text>();
                }

                return m_maxScoreText;
            }
        }
        
        private Transform uiTransform;
        private Button m_ReturnBtn;
        private Button m_ExitBtn;
        private Text m_curScoreText;
        private Text m_maxScoreText;
    }
}