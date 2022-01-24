using Define;
using Framework;
using UnityEngine;

namespace Logic
{
    public class EnterGameProcedure:BaseProcedure
    {
        public override void OnEnter(IFSM<IProcedureModule> fsm)
        {
            Debug.Log("进入测试流程");

            SkyFrameworkEntry.GetModule<IUIModule>().ShowUIByID(WindowID.WindowID_Property);
            EventMessageSender.Instance.AddListener(GameDefine.END_GAME_MSG, (message) =>
            {
                this.ChangeState<EndGameProcedure>(fsm);
            });
            
            PlayerManager.Instance.CreatePlayer<BodyPlayer>();
            PlayerManager.Instance.CreatePlayer<HeartPlayer>();
            Background.instance.Material.mainTextureOffset = Vector2.zero;
            
        }

        public override void OnUpdate(IFSM<IProcedureModule> fsm)
        {
            Debug.Log("测试流程轮询");
            Producer.instance.OnUpdate();
            Background.instance.OnUpdate();
        }

        public override void OnExit(IFSM<IProcedureModule> fsm)
        {
            Debug.Log("退出测试流程");
            EventMessageSender.Instance.RemoveOneListener(GameDefine.END_GAME_MSG);
            PlayerManager.Instance.ClearPlayer();
        }
    }
}

