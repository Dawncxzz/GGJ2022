using System;
using System.Collections.Generic;

namespace Framework
{
    public interface IFSM<T> where T:class
    {
        /// <summary>
        /// 获取状态机名称
        /// </summary>
        string Name
        {
            get;
        }
        
        /// <summary>
        /// 获取状态机持有者
        /// </summary>
        T Owner
        {
            get;
        }

        /// <summary>
        /// 获取状态机中状态数量
        /// </summary>
        int FsmStateCount
        {
            get;
        }

        /// <summary>
        /// 获取状态机是否被销毁
        /// </summary>
        bool IsDestroyed
        {
            get;
        }

        /// <summary>
        /// 是否支持自动转换
        /// </summary>
        bool IsAutoChange
        {
            get;
            set;
        }

        /// <summary>
        /// 是否改变状态
        /// </summary>
        bool IsStateChange
        {
            get;
        }

        /// <summary>
        /// 获取状态机当前状态
        /// </summary>
        FSMState<T> CurrentState
        {
            get;
        }

        /// <summary>
        /// 开启有限状态机
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        void Start<TState>() where TState : FSMState<T>;

        /// <summary>
        /// 开启有限状态机
        /// </summary>
        /// <param name="stateType"></param>
        void Start(Type stateType);

        
         /// <summary>
        /// 是否存在有限状态机状态。
        /// </summary>
         bool HasState<TState>() where TState : FSMState<T>;

        /// <summary>
        /// 是否存在有限状态机状态。
        /// </summary>
        bool HasState(Type stateType);

        /// <summary>
        /// 获取有限状态机状态。
        /// </summary>
        TState GetState<TState>() where TState : FSMState<T>;

        /// <summary>
        /// 获取有限状态机状态。
        /// </summary>
        FSMState<T> GetState(Type stateType);

        /// <summary>
        /// 状态机状态转换
        /// </summary>
        /// <param name="stateType"></param>
        /// <returns></returns>
        void ChangeState(Type stateType);

        /// <summary>
        /// 状态机状态转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void ChangeState<TState>() where TState:FSMState<T>;

        /// <summary>
        /// 获取有限状态机的所有状态。
        /// </summary>
        FSMState<T>[] GetAllStates();

        /// <summary>
        /// 获取有限状态机的所有状态。
        /// </summary>
        void GetAllStates(List<FSMState<T>> results);

        /// <summary>
        /// 是否存在有限状态机数据。
        /// </summary>
        bool HasData(string name);

        /// <summary>
        /// 获取有限状态机数据。
        /// </summary>
        TData GetData<TData>(string name);

        /// <summary>
        /// 获取有限状态机数据。
        /// </summary>
        object GetData(string name);

        /// <summary>
        /// 设置有限状态机数据。
        /// </summary>
        void SetData<TData>(string name, TData data);

        /// <summary>
        /// 设置有限状态机数据。
        /// </summary>
        void SetData(string name, object data);

        /// <summary>
        /// 移除有限状态机数据。
        /// </summary>
        bool RemoveData(string name);
    }
}

