using Define;
using Framework;
using UnityEngine;

namespace Logic
{
    public class PlayerJumpState2:FSMState<Player>
    {

        public override void OnInit(IFSM<Player> fsm)
        {
            
        }

        public override void OnEnter(IFSM<Player> fsm)
        {
            fsm.Owner.isJumping = true;
            fsm.Owner.Jump();
            fsm.Owner.animator.SetTrigger("Jump2");
        }

        public override void OnUpdate(IFSM<Player> fsm)
        {
        }

        public override void OnExit(IFSM<Player> fsm)
        {
            fsm.Owner.isJumping = false;
            fsm.Owner.animator.ResetTrigger("Jump2");
            
        }

        public override void OnDestroy(IFSM<Player> fsm)
        {
            
        }
    }
}