using System.Collections.Generic;
using Framework;
using UnityEngine;

namespace Logic
{
    public class BodyPlayer : Player
    {
        protected override void Awake()
        {
            m_posType = PosType.Up;
            base.Awake();
        }

        protected override void Update()
        {
            base.Update();
        }


        protected override void InitFSM()
        {
            m_fsm = SkyFrameworkEntry.GetModule<IFSMModule>().CreateFsm("Body", this, new List<FSMState<Player>>()
            {
                new PlayerWalkState(),new PlayerJumpState(),new PlayerFallState(),new PlayerDiveState(),new PlayerSlideState(),
                new PlayerJumpState2()
            }) as FSM<Player>;

            m_fsm.Start<PlayerWalkState>(); //开启状态机

            //PlayerWalkState
            m_fsm.AddTransition<PlayerWalkState, PlayerJumpState>(() => this.input.jumpBtn.isPressed && this.isGrounded);

            //PlayerJumpState
            //m_fsm.AddTransition<PlayerJumpState,PlayerWalkState>(()=>this.isGrounded);
            m_fsm.AddTransition<PlayerJumpState, PlayerJumpState2>(() => this.input.jumpBtn.isPressed && this.curJumpCount < this.maxJumpCount);
            m_fsm.AddTransition<PlayerJumpState, PlayerFallState>(() => !this.isGrounded && this.verticalVel <= 0f);
            m_fsm.AddTransition<PlayerJumpState2, PlayerFallState>(() => !this.isGrounded && this.verticalVel <= 0f);

            //PlayerFallState
            m_fsm.AddTransition<PlayerFallState, PlayerWalkState>(() => this.isGrounded);
            m_fsm.AddTransition<PlayerFallState, PlayerJumpState2>(() => !this.isGrounded && this.input.jumpBtn.isPressed && this.curJumpCount < this.maxJumpCount);



        }

        public override void Dispose()
        {
            base.Dispose();
            SkyFrameworkEntry.GetModule<IFSMModule>().DestroyFsm<Player>("Body");
        }
    }
}