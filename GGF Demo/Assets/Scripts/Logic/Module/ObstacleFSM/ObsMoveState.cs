using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework;
using UnityEngine;

namespace Logic
{
    public class ObsMoveState : FSMState<Obstacle>
    {
        public override void OnInit(IFSM<Obstacle> fsm)
        {

        }

        public override void OnEnter(IFSM<Obstacle> fsm)
        {
        }

        public override void OnUpdate(IFSM<Obstacle> fsm)
        {
            fsm.Owner.transform.position-=Vector3.right* fsm.Owner.Speed*Time.deltaTime;
            if(fsm.Owner.transform.position.x<=fsm.Owner.Out.x)
            {
                fsm.ChangeState<ObsSleepState>();
                fsm.Owner.ObsPool.Recycle(fsm.Owner.gameObject);
            }

        }

        public override void OnExit(IFSM<Obstacle> fsm)
        {
        }

        public override void OnDestroy(IFSM<Obstacle> fsm)
        {
        }
    }
}
