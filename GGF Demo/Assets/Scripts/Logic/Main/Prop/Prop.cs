using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Framework;

namespace Logic
{
    public class Prop : GameObjectBase
    {
        private FSM<Prop> m_fsm; //状态机

        [HideInInspector]
        public PropPool PropPool;

        protected override void InitGO()
        {
            base.InitGO();

            m_fsm = SkyFrameworkEntry.GetModule<IFSMModule>().CreateFsm(PropHelper.GetPropIndex().ToString(),this, new List<FSMState<Prop>>()
            {
                new PropMoveState(),new PropSleepState()
            }) as FSM<Prop>;

            m_fsm.Start<PropSleepState>(); //开启状态机
        }

        public void WakeUp()
        {
            m_fsm.ChangeState<PropMoveState>();
        }

        private void Awake()
        {
            InitGO();
        }



    }
}
