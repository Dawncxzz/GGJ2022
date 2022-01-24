using System;
using System.Collections.Generic;
using Define;
using Framework;
using UnityEngine;

namespace Logic
{
    public class Player:GameObjectBase
    {
        protected PosType m_posType;
        public PosType PositionType
        {
            get
            {
                return m_posType;
            }
            set
            {
                m_fsm.IsAutoChange = value==PosType.Up;
                m_posType = value;
            }
        }

        public FSM<Player> Fsm
        {
            get => m_fsm;
        }
        
        protected FSM<Player> m_fsm; //状态机
        [HideInInspector]
        public Rigidbody2D rb; 
        [HideInInspector]
        public Animator animator;
        
        protected Transform foot;

        public PlayerInput input;
        public PlayerProperty property;
        public bool isGrounded = false;
        public bool isJumping = false;
        public int maxJumpCount = 2;
        public int curJumpCount = 0;

        [Header("当前垂直速度")]
        public float verticalVel;

        protected override void InitGO()
        {
            base.InitGO();
        }

        public virtual void Dispose()
        {
            
        }

        private void InitParam()
        {
            rb = this.GetComponent<Rigidbody2D>();
            animator = this.GetComponentInChildren<Animator>();
            foot = this.transform.Find("Foot");
        }

        protected virtual void InitFSM()
        {
            
        }

        private void InitExtraParam()
        {
            input = new PlayerInput();
            property = new PlayerProperty(this.m_posType);
            
            PositionType = m_posType; //刷新m_fsm的自动转换
        }


        protected virtual void Awake()
        {
            InitGO();
            InitParam();
            InitFSM();
            InitExtraParam();
        }

        protected virtual void Update()
        {
            HandleStatesByPos();
            CheckGround();
        }
        


        private void CheckGround()
        {
            if(Physics2D.OverlapBox(foot.transform.position, new Vector2(0.5f, 0.01f), 0, LayerMask.GetMask("Plane")))
            {
                isGrounded = true;
            }else
            {
                isGrounded = false;
            }
            
        }

        public void Hurt(int value)
        {
            this.property.HP = Mathf.Clamp(this.property.HP - value,0, this.property.MaxHP );
            SoundHelper.PostSoundEvent("hurt_player",this.gameObject);
        }
        public void Cure(int value)
        {
            this.property.HP = Mathf.Clamp(this.property.HP + value, 0, this.property.MaxHP);
            SoundHelper.PostSoundEvent("heal_player",this.gameObject);
        }
        


        #region Concrete Action

        public virtual void Jump()
        {
            this.verticalVel = GameDefine.JumpSpeed;
            curJumpCount++;
        }

        public virtual void Dive()
        {
            this.verticalVel = GameDefine.DiveSpeed;
        }

        public void HandleStatesByPos()
        {
            switch (PositionType)
            {
                case PosType.Up:
                    if (!this.isGrounded||this.isJumping)
                    {
                        this.isGrounded = false;
                        GameObject.transform.position += new Vector3(0, verticalVel * Time.deltaTime, 0);
                        verticalVel=GravityHelper.CalSpeedWithGravity(verticalVel, Time.deltaTime);
                    }
                    else
                    {
                        verticalVel = 0f;
                    }
                    break;
                case PosType.Down:
                    this.isGrounded = false;
                    PlayerManager.Instance.AsyncPlayerPos(PosType.Up,this,Vector3.zero);
                    PlayerManager.Instance.AsyncPlayerState(PosType.Up,this);
                    break;
            }
            
            // if (!this.isGrounded||this.isJumping)
            // {
            //     switch (PositionType)
            //     {
            //         case PosType.Up:
            //             this.isGrounded = false;
            //             GameObject.transform.position += new Vector3(0, verticalVel * Time.deltaTime, 0);
            //             break;
            //         case PosType.Down:
            //             this.isGrounded = false;
            //             GameObject.transform.position -= new Vector3(0, verticalVel * Time.deltaTime, 0);
            //             break;
            //     }
            //     verticalVel=GravityHelper.CalSpeedWithGravity(verticalVel, Time.deltaTime);
            // }
            // else
            // {
            //     verticalVel = 0f;
            // }
        }
        

        #endregion
    }
}

