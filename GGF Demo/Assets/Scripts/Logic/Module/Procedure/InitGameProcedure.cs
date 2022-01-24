using Define;
using Framework;
using UnityEngine;

namespace Logic
{
    public class InitGameProcedure:BaseProcedure
    {
        //private float duration = 3f;
        public override void OnEnter(IFSM<IProcedureModule> fsm)
        {
            Debug.Log("进入初始化游戏流程");
            EventMessageSender.Instance.AddListener(GameDefine.ENTER_GAME_MSG,(message =>
            {
                this.ChangeState<EnterGameProcedure>(fsm);
            } ));
            
            SkyFrameworkEntry.GetModule<IUIModule>().ShowUIByID(WindowID.WindowID_Menu);
        }

        public override void OnUpdate(IFSM<IProcedureModule> fsm)
        {
            
        }

        public override void OnExit(IFSM<IProcedureModule> fsm)
        {
            Debug.Log("退出初始化游戏流程");
            EventMessageSender.Instance.RemoveOneListener(GameDefine.ENTER_GAME_MSG);
        }
        
    }
}

