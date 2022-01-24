using Define;
using Framework;
using UnityEngine;
using System.IO;

namespace Logic
{
    public class SkySettle:UIBaseWindow
    {
        private SkySettleView view;
        public override WindowID _WindowID
        {
            get
            {
                return WindowID.WindowID_Settle;
            }
        }

        public override UIWindowType _UIWindowType
        {
            get
            {
                return UIWindowType.Normal;
            }
        }

        public override void OnLoad()
        {
            view = new SkySettleView(this.uiTransform);
        }

        public override void OnRegisterListener()
        {
            view.ExitBtn.AddListener(() =>
            {
                Application.Quit();
            });
            
            view.ReturnBtn.AddListener(() =>
            {
                EventMessageSender.Instance.SendMessage(GameDefine.INIT_GAME_MSG,this,null);
                SkyFrameworkEntry.GetModule<IUIModule>().HideUIByID(WindowID.WindowID_Settle,false);
            });
        }

        public override void OnStart(params object[] _params)
        {
            
        }

        public override void OnResume(params object[] _params)
        {
            
        }

        public override void OnShow(params object[] _params)
        {
            this.view.CurScoreText.text = Background.instance.BaseScore.ToString();

            if(Background.instance.BaseScore> Background.instance.Best)
            {
                Background.instance.Best = Background.instance.BaseScore;
                //File.WriteAllText("Assets/Scripts/Define/Best.txt", Background.instance.Best.ToString());
                PlayerPrefs.SetInt("score",Background.instance.Best);
            }

            this.view.MaxScoreText.text = Background.instance.Best.ToString();
        }

        public override void OnHide()
        {
            
        }

        public override void OnUnload()
        {
            
        }
    }
}