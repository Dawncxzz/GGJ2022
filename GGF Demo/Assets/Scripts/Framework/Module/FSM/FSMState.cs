using System;

namespace Framework
{
    public abstract class FSMState<T>where T:class
    {
        public FSMState()
        {
                
        }
        /// <summary>
        /// 初始化时调用
        /// </summary>
        /// <param name="fsm"></param>
        public abstract void OnInit(IFSM<T> fsm);
        
        /// <summary>
        /// 进入时调用
        /// </summary>
        /// <param name="fsm"></param>
        public abstract void OnEnter(IFSM<T> fsm);

        /// <summary>
        /// 轮询时调用
        /// </summary>
        /// <param name="fsm"></param>
        public abstract void OnUpdate(IFSM<T> fsm);

        /// <summary>
        /// 退出时调用
        /// </summary>
        /// <param name="fsm"></param>
        public abstract void OnExit(IFSM<T> fsm);

        /// <summary>
        /// 销毁时调用
        /// </summary>
        /// <param name="fsm"></param>
        public abstract void OnDestroy(IFSM<T> fsm);
        
        
        /// <summary>
        /// 转换状态
        /// </summary>
        /// <param name="fsm"></param>
        /// <typeparam name="TState"></typeparam>
        /// <exception cref="GameFrameworkException"></exception>
        public void ChangeState<TState>(IFSM<T> fsm) where TState : FSMState<T>
        {
            if (fsm == null)
            {
                throw new Exception("FSM is invalid!");
            }

            fsm.ChangeState<TState>();
        }

        public void ChangeState(IFSM<T> fsm, Type stateType)
        {
            if (fsm == null)
            {
                throw new Exception("FSM is invalid!");
            }
            
            fsm.ChangeState(stateType);
        }
    } 
}

