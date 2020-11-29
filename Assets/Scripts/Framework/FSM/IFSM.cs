using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDK.FSM
{
    public interface IFSM<T>
    {
        bool CanChange {get; set;}  // 是否可以改变状态
        EFSMState State {get;}  // 当前状态
        void Enter(T obj, float lastState);  
        bool ChangeState(T obj, IFSM<T> state);
        void Execute(T obj);
        void Exit(T obj);
    }
}


