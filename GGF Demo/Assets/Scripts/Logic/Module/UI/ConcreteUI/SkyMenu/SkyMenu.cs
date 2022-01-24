using Define;
using Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Logic
{
    public class SkyMenu:UIBaseWindow
    {
        private SkyMenuView view;
        
        public override WindowID _WindowID
        {
            get
            {
                return WindowID.WindowID_Menu;
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
            view = new SkyMenuView(uiTransform);
        }

        public override void OnRegisterListener()
        {
            view.StartBtn.AddListener(() =>
            {
                EventMessageSender.Instance.SendMessage(GameDefine.ENTER_GAME_MSG,this,null);
                SkyFrameworkEntry.GetModule<IUIModule>().HideUIByID(WindowID.WindowID_Menu,false);
            });
            
            view.ExitBtn.AddListener(() =>
            {
                Application.Quit();
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
            
        }

        public override void OnHide()
        {
            
        }

        public override void OnUnload()
        {
           
        }
    }
}