using System;
using System.Collections.Generic;

namespace Framework
{
    public sealed class FSM<T> : FSMBase, IFSM<T> where T : class
    {
        private T m_Owner;
        private readonly Dictionary<Type, FSMState<T>> m_states;
        private Dictionary<string, object> m_datas;
        private Dictionary<Type, Dictionary<Type,FSMTransition>> m_transitions;
        private FSMState<T> m_currentState;
        private float m_currentStateTime;
        private bool m_isDestroyed;
        private bool m_autoChange;
        private bool m_isStateChange;
        private Dictionary<Type, FSMTransition> m_temp;


        public FSM()
        {
            m_Owner = null;
            m_states = new Dictionary<Type, FSMState<T>>();
            m_transitions = new Dictionary<Type, Dictionary<Type, FSMTransition>>();
            m_datas = new Dictionary<string, object>();
            m_currentState = null;
            m_currentStateTime = 0f;
            m_isDestroyed = true;
            m_autoChange = true;
            m_isStateChange = false;
        }

        public override Type OwnerType
        {
            get { return typeof(T); }
        }


        public T Owner
        {
            get { return m_Owner; }
        }

        public override int FsmStateCount
        {
            get { return m_states.Count; }
        }


        public bool IsStateChange
        {
            get
            {
                return m_isStateChange;
            }
        }

        public FSMState<T> CurrentState
        {
            get { return m_currentState; }
        }

        /// <summary>
        /// 创建状态机
        /// </summary>
        /// <param name="name"></param>
        /// <param name="owner"></param>
        /// <param name="states"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static FSM<T> Create(string name, T owner, List<FSMState<T>> states)
        {
            if (owner == null)
            {
                throw new Exception($"owner is null!");
            }

            if (states == null || states.Count == 0)
            {
                throw new Exception($"states is null!");
            }

            FSM<T> fsm = new FSM<T>();
            fsm.Name = name;
            fsm.m_Owner = owner;
            fsm.m_isDestroyed = false;
            foreach (var state in states)
            {
                if (state == null)
                {
                    throw new Exception($"{state.GetType().FullName} is invalid!");
                }

                Type stateType = state.GetType();
                if (fsm.m_states.ContainsKey(stateType))
                {
                    throw new Exception($"already has {stateType.FullName}");
                }

                fsm.m_states.Add(stateType, state);
                state.OnInit(fsm);
            }

            return fsm;
        }

        /// <summary>
        /// 清空状态机
        /// </summary>
        internal void Clear()
        {
            if (m_currentState != null)
            {
                m_currentState.OnExit(this);
            }

            foreach (var state in m_states.Values)
            {
                state.OnDestroy(this);
            }

            Name = null;
            m_Owner = null;
            m_states.Clear();
            m_datas.Clear();
            //TODO:后面可能会用引用池统一管理

            m_currentState = null;
            m_currentStateTime = 0f;
            m_isDestroyed = true;
        }

        public void Start<TState>() where TState : FSMState<T>
        {
            if (IsRunning)
            {
                throw new Exception($"FSM is running!");
            }

            FSMState<T> state = GetState<TState>();
            if (state == null)
            {
                throw new Exception($"FSMState not exist!");
            }

            m_currentState = state;
            m_currentStateTime = 0f;
            m_currentState.OnEnter(this);
        }

        public void Start(Type stateType)
        {
            if (!typeof(FSMState<T>).IsAssignableFrom(stateType))
            {
                throw new Exception($"{stateType} type error!");
            }

            if (IsRunning)
            {
                throw new Exception($"FSM is running!");
            }

            FSMState<T> state = GetState(stateType);
            if (state == null)
            {
                throw new Exception($"FSMState not exist!");
            }

            m_currentState = state;
            m_currentStateTime = 0f;
            m_currentState.OnEnter(this);
        }

        public void AddTransition<TState>(Type nextState, CheckTransitionFunc checkFunc)where TState:FSMState<T>
        {
            if (!this.m_transitions.ContainsKey(typeof(TState)))
            {
                this.m_transitions.Add(typeof(TState), new Dictionary<Type, FSMTransition>());
            }

            Dictionary<Type,FSMTransition> transitions = null;
            if (this.m_transitions.TryGetValue(typeof(TState), out transitions))
            {
                if (!transitions.ContainsKey(nextState))
                {
                    FSMTransition transition = new FSMTransition(nextState, checkFunc);
                    transitions.Add(nextState,transition);
                }
            }
        }

        public void AddTransition<FromState, ToState>(CheckTransitionFunc checkFunc)where FromState:FSMState<T> where ToState:FSMState<T>
        {
            AddTransition<FromState>(typeof(ToState),checkFunc);
        }

        public bool HasState<TState>() where TState : FSMState<T>
        {
            return m_states.ContainsKey(typeof(TState));
        }

        public bool HasState(Type stateType)
        {
            return m_states.ContainsKey(stateType);
        }

        public TState GetState<TState>() where TState : FSMState<T>
        {
            FSMState<T> state = null;
            if (m_states.TryGetValue(typeof(TState), out state))
            {
                return state as TState;
            }

            return null;
        }

        public FSMState<T> GetState(Type stateType)
        {
            if (stateType == null)
            {
                throw new Exception("State Type is invalid!");
            }

            FSMState<T> state = null;
            if (m_states.TryGetValue(stateType, out state))
            {
                return state;
            }

            return null;
        }

        public FSMState<T>[] GetAllStates()
        {
            FSMState<T>[] results = new FSMState<T>[m_states.Count];
            int index = 0;
            foreach (var state in m_states.Values)
            {
                results[index++] = state;
            }

            return results;
        }

        public void GetAllStates(List<FSMState<T>> results)
        {
            results.Clear();
            foreach (var state in m_states.Values)
            {
                results.Add(state);
            }

        }

        public bool HasData(string name)
        {
            return m_datas.ContainsKey(name);
        }

        public TData GetData<TData>(string name)
        {
            if (HasData(name))
            {
                return (TData) m_datas[name];
            }

            return default(TData);
        }

        public object GetData(string name)
        {
            if (HasData(name))
            {
                return m_datas[name];
            }

            return null;
        }

        public void SetData<TData>(string name, TData data)
        {
            if (HasData(name))
            {
                m_datas[name] = data;
            }
            else
            {
                m_datas.Add(name, data);
            }
        }

        public void SetData(string name, object data)
        {
            if (HasData(name))
            {
                m_datas[name] = data;
            }
            else
            {
                m_datas.Add(name, data);
            }
        }

        public bool RemoveData(string name)
        {
            if (HasData(name))
            {
                this.m_datas.Remove(name);
                return true;
            }

            return false;
        }

        public override bool IsRunning
        {
            get { return m_currentState != null; }
        }

        public override bool IsDestroyed
        {
            get { return m_isDestroyed; }
        }

        public bool IsAutoChange
        {
            get
            {
                return m_autoChange;
            }
            set
            {
                m_autoChange = value;
            }
        }

        public override string CurrentStateName
        {
            get { return m_currentState != null ? m_currentState.GetType().FullName : string.Empty; }
        }

        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
            if (this.IsRunning)
            {
                m_isStateChange = false;
                if (m_autoChange)
                {
                    //MARKER：先判断转换
                    Type currentStateType = m_currentState.GetType();
                    if (m_transitions.TryGetValue(currentStateType, out m_temp))
                    {
                        foreach (var transition in m_temp.Values)
                        {
                            if (transition.CheckTransition(this, m_currentState)) break;
                        }
                    }
                }
                
                m_currentStateTime += elapseSeconds;
                m_currentState.OnUpdate(this);
            }
        }

        internal override void Shutdown()
        {
            this.Clear();
        }

        public void ChangeState<TState>() where TState : FSMState<T>
        {
            ChangeState(typeof(TState));
        }

        public void ChangeState(Type stateType)
        {
            if (m_currentState == null)
            {
                this.Start(stateType);
                return;
            }

            FSMState<T> state = GetState(stateType);
            if (state == null)
            {
                throw new Exception($"{stateType.FullName} not find!");
            }

            m_currentState.OnExit(this);
            m_currentStateTime = 0f;
            m_currentState = state;
            m_isStateChange = true;
            m_currentState.OnEnter(this);
            
        }
    }
}
