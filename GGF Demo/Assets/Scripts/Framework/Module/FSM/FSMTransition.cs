using System;

namespace Framework
{
    public delegate bool CheckTransitionFunc();
    
    public class FSMTransition
    {
        public Type nextStateType;
        public CheckTransitionFunc checkFunc;
        
        public FSMTransition(Type _nextStateType,CheckTransitionFunc _checkFunc)
        {
            nextStateType = _nextStateType;
            checkFunc = _checkFunc;
        }

        public bool CheckTransition<T>(FSM<T> fsm,FSMState<T> fsmState) where T : class
        {
            if (checkFunc())
            {
                fsmState.ChangeState(fsm,nextStateType);
                return true;
            }

            return false;
        }
        
    }
}