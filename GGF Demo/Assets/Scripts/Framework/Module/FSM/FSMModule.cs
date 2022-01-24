using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework
{
    public class FSMModule:BaseModule,IFSMModule
{
    private readonly Dictionary<string, FSMBase> m_allFSMs;
    private readonly List<FSMBase> m_tempFSMs;

    public int Count
    {
        get
        {
            return m_allFSMs.Count;
        }
    }

    public FSMModule()
    {
        m_allFSMs = new Dictionary<string, FSMBase>();
        m_tempFSMs = new List<FSMBase>();
    }

    public override int Priority
    {
        get
        {
            return 2;
        }
    }

    public override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        m_tempFSMs.Clear();
        if(m_allFSMs.Count<=0) return;
        
        //MARKER:防止Update过程中的顺序调整
        foreach (var fsm in m_allFSMs.Values)
        {
            if (!fsm.IsDestroyed)
            {
                m_tempFSMs.Add(fsm);
            }
        }

        foreach (var fsm in m_tempFSMs)
        {
            fsm.Update(elapseSeconds,realElapseSeconds);
        }
    }

    private string ConvertPairToStr(Type type, string name)
    {
        return type.Name +"_"+ name;
    }
    
    public bool HasFSM<T>() where T : class
    {
        return m_allFSMs.ContainsKey(ConvertPairToStr(typeof(T), string.Empty));
    }

    public bool HasFSM<T>(string name) where T : class
    {
        return m_allFSMs.ContainsKey(ConvertPairToStr(typeof(T), name));
    }

    public bool HasFSM(Type ownerType, string name)
    {
        return m_allFSMs.ContainsKey(ConvertPairToStr(ownerType, name));
    }

    public IFSM<T> GetFSM<T>() where T : class
    {
        if (HasFSM<T>())
        {
            return m_allFSMs[ConvertPairToStr(typeof(T), string.Empty)] as IFSM<T>;
        }

        return null;
    }

    public FSMBase GetFSM(Type type)
    {
        if (HasFSM(type,string.Empty))
        {
            return m_allFSMs[ConvertPairToStr(type, string.Empty)];
        }

        return null;
    }

    public IFSM<T> GetFSM<T>(string name) where T : class
    {
        if (HasFSM<T>(name))
        {
            return m_allFSMs[ConvertPairToStr(typeof(T), name)] as IFSM<T>;
        }

        return null;
    }

    public FSMBase GetFSM(Type ownerType, string name)
    {
        if (HasFSM(ownerType, name))
        {
            return m_allFSMs[ConvertPairToStr(ownerType, name)];
        }

        return null;
    }

    public FSMBase[] GetAllFSM()
    {
        FSMBase[] fsms = new FSMBase[this.m_allFSMs.Count];
        int index = 0;
        foreach (var fsm in m_allFSMs.Values)
        {
            fsms[index++] = fsm;
        }

        return fsms;
    }

    public IFSM<T> CreateFsm<T>(T owner, List<FSMState<T>> states) where T : class
    {
        return CreateFsm<T>(string.Empty, owner,states);
    }

    public IFSM<T> CreateFsm<T>(T owner, params FSMState<T>[] states) where T : class
    {
        List<FSMState<T>> stateList = states.ToList();
        return CreateFsm<T>(string.Empty, owner, stateList);
    }

    public IFSM<T> CreateFsm<T>(string name, T owner, List<FSMState<T>> states) where T : class
    {
        string key = ConvertPairToStr(typeof(T), name);
        if (HasFSM<T>())
        {
            throw new Exception($"already has {key} fsm!");
        }

        FSM<T> fsm = FSM<T>.Create(string.Empty, owner, states);
        m_allFSMs.Add(key,fsm);
        return fsm;
    }

    public bool DestroyFsm<T>(string name) where T : class
    {
        return DestroyFsm(typeof(T), name);
    }

    public bool DestroyFsm(Type ownerType, string name)
    {
        if (HasFSM(ownerType, name))
        {
            string key = ConvertPairToStr(ownerType, name);
            FSMBase fsm = m_allFSMs[key];
            fsm.Shutdown();
            m_allFSMs.Remove(key);
            return true;
        }

        return false;
    }
}
}

