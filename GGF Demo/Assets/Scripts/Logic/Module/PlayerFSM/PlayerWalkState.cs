using Framework;
using UnityEngine;


namespace Logic
{
    public class PlayerWalkState:FSMState<Player>
    {
        private static readonly int Walk = Animator.StringToHash("Walk");

        public override void OnInit(IFSM<Player> fsm)
        {
        }

        public override void OnEnter(IFSM<Player> fsm)
        {
            Debug.Log("Enter Walk");
            fsm.Owner.curJumpCount = 0;
            fsm.Owner.animator.SetBool(Walk,true);
            SkyFrameworkEntry.GetModule<IInputModule>().StopForWhile(0.4f);
        }

        public override void OnUpdate(IFSM<Player> fsm)
        {
            
        }

        public override void OnExit(IFSM<Player> fsm)
        {
            Debug.Log("Exit Walk");
            fsm.Owner.animator.SetBool(Walk,false);
        }

        public override void OnDestroy(IFSM<Player> fsm)
        {
            
        }
    }
}

