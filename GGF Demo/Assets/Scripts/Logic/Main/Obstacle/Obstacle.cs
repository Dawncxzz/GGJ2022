using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Framework;

namespace Logic
{
    public class Obstacle:GameObjectBase
    {
        private FSM<Obstacle> m_fsm; //状态机

        [HideInInspector]
        public ObsPool ObsPool;

        Collider Collider;

        public int HurtValue;//伤害值

        public GameObject EndPoint;
        public int OccupancyNum=1;//占用几行格子

        [HideInInspector]
        public Vector3 _endPoint { get => EndPoint.transform.position; }

        public ProduceType ProduceType;

        protected override void InitGO()
        {
            base.InitGO();

            Collider = GetComponent<Collider>();

            m_fsm = SkyFrameworkEntry.GetModule<IFSMModule>().CreateFsm(ObstacleHelper.GetObstacleIndex().ToString(),this, new List<FSMState<Obstacle>>()
            {
                new ObsMoveState(),new ObsSleepState()
            }) as FSM<Obstacle>;

            m_fsm.Start<ObsSleepState>(); //开启状态机
        }

        public void WakeUp()
        {
            m_fsm.ChangeState<ObsMoveState>();
        }

        private void Awake()
        {
            InitGO();
        }

         

        public virtual void Hurt(Player player)
        {
            //TODO:造成伤害
            player.Hurt(HurtValue);
            
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Debug.Log("Hurt");
                Player player = collision.GetComponent<Player>();
                Hurt(player);
            }
        }

    }
}
