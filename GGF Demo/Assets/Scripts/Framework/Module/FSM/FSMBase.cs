using System;

namespace Framework
{
    public abstract class FSMBase
    {
        private string m_Name;
    
        public FSMBase()
        {
            m_Name=String.Empty;
        }
    
        public string Name
        {
            get
            {
                return m_Name;
            }
            internal set
            {
                m_Name = value;
            }
        }
    
    
        public abstract Type OwnerType
        {
            get;
        }
        
        public string FullName
        {
            get
            {
                return OwnerType.ToString() + m_Name;
            }
        }
    
    
        public abstract int FsmStateCount
        {
            get;
        }
    
        public abstract bool IsRunning
        {
            get;
        }
    
        public abstract bool IsDestroyed
        {
            get;
        }
    
        public abstract  string CurrentStateName
        {
            get;
        }
    
        internal abstract void Update(float elapseSeconds, float realElapseSeconds);
    
        internal abstract void Shutdown();
    

}

}