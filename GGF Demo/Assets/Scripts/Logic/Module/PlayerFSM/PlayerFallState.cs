using Framework;
using UnityEngine;

namespace Logic
{
    public class PlayerFallState:FSMState<Player>
    {
        private static readonly int Fall = Animator.StringToHash("Fall");

        public override void OnInit(IFSM<Player> fsm)
        {
            
        }

        public override void OnEnter(IFSM<Player> fsm)
        {
            fsm.Owner.animator.SetBool(Fall,true);
        }

        public override void OnUpdate(IFSM<Player> fsm)
        {
            
        }

        public override void OnExit(IFSM<Player> fsm)
        {
            fsm.Owner.animator.SetBool(Fall,false);
            
        }

        public override void OnDestroy(IFSM<Player> fsm)
        {
            
        }
    }
}