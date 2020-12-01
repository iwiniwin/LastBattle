using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDK.FSM
{
    public interface IGameState<T> where T : System.Enum
    {
        // 状态类型
        T Type{get; set;}
        // 下一个状态的类型
        T NextStateType{get; set;}
        void Enter();
        bool Update(float deltaTime);
        void FixedUpdate(float fixedDeltaTime);
        void Exit();
    }
}


