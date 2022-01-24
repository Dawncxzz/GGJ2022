using Framework;
using UnityEngine;

namespace Logic
{
    public class PlayerSlideState:FSMState<Player>
    {
        private static readonly int Slide = Animator.StringToHash("Slide");

        public override void OnInit(IFSM<Player> fsm)
        {
            
        }

        public override void OnEnter(IFSM<Player> fsm)
        {
            fsm.Owner.animator.SetBool(Slide,true);
        }

        public override void OnUpdate(IFSM<Player> fsm)
        {
            
        }

        public override void OnExit(IFSM<Player> fsm)
        {
            fsm.Owner.animator.SetBool(Slide,false);
        }

        public override void OnDestroy(IFSM<Player> fsm)
        {
            
        }
    }
}