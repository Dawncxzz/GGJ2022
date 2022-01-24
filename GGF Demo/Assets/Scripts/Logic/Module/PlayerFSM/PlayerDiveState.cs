using Framework;
using UnityEngine;

namespace Logic
{
    public class PlayerDiveState:FSMState<Player>
    {
        private static readonly int Dive = Animator.StringToHash("Dive");

        public override void OnInit(IFSM<Player> fsm)
        {
            
        }

        public override void OnEnter(IFSM<Player> fsm)
        {
            fsm.Owner.isJumping = true;
            fsm.Owner.Dive();
            fsm.Owner.animator.SetBool(Dive,true);
        }

        public override void OnUpdate(IFSM<Player> fsm)
        {
            
        }

        public override void OnExit(IFSM<Player> fsm)
        {
            fsm.Owner.isJumping = false;
            fsm.Owner.animator.SetBool(Dive,false);
        }

        public override void OnDestroy(IFSM<Player> fsm)
        {
            
        }
    }
}