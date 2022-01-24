using Define;
using Framework;
using UnityEngine;

namespace Logic
{
    public class EndGameProcedure:BaseProcedure
    {
        public override void OnEnter(IFSM<IProcedureModule> fsm)
        {

            SkyFrameworkEntry.GetModule<IUIModule>().ShowUIByID(WindowID.WindowID_Settle);
            EventMessageSender.Instance.AddListener(GameDefine.INIT_GAME_MSG, (message) =>
            {
                this.ChangeState<InitGameProcedure>(fsm);
            });
            
        }

        public override void OnUpdate(IFSM<IProcedureModule> fsm)
        {
            
        }

        public override void OnExit(IFSM<IProcedureModule> fsm)
        {
            EventMessageSender.Instance.RemoveOneListener(GameDefine.END_GAME_MSG);
        }
    }
}