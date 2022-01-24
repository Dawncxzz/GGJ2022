using Define;
using Framework;
using UnityEngine;

namespace Logic
{
    public class PlayerJumpState:FSMState<Player>
    {
        private static readonly int Jump = Animator.StringToHash("Jump");

        public override void OnInit(IFSM<Player> fsm)
        {
            
        }

        public override void OnEnter(IFSM<Player> fsm)
        {
            fsm.Owner.isJumping = true;
            fsm.Owner.animator.SetBool(Jump,true);
            fsm.Owner.Jump();
            Debug.Log(fsm.Owner.curJumpCount);
        }

        public override void OnUpdate(IFSM<Player> fsm)
        {
        }

        public override void OnExit(IFSM<Player> fsm)
        {
            fsm.Owner.isJumping = false;
            fsm.Owner.animator.SetBool(Jump,false);
            
        }

        public override void OnDestroy(IFSM<Player> fsm)
        {
            
        }
    }
}