using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework;
using UnityEngine;

namespace Logic
{
    public class PropMoveState : FSMState<Prop>
    {
        public override void OnInit(IFSM<Prop> fsm)
        {

        }

        public override void OnEnter(IFSM<Prop> fsm)
        {
        }

        public override void OnUpdate(IFSM<Prop> fsm)
        {
            fsm.Owner.transform.position -= Vector3.right * fsm.Owner.Speed * Time.deltaTime;
            if (fsm.Owner.transform.position.x <= fsm.Owner.Out.x)
            {
                fsm.ChangeState<PropSleepState>();
                fsm.Owner.PropPool.Recycle(fsm.Owner.gameObject);
            }

        }

        public override void OnExit(IFSM<Prop> fsm)
        {
        }

        public override void OnDestroy(IFSM<Prop> fsm)
        {

        }
    }
}
