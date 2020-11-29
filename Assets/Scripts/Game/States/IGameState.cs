using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public interface IGameState
    {
        EGameStateType Type{get; set;}

        void SetStateTo(EGameStateType type);
        void Enter();
        EGameStateType Update(float deltaTime);
        void FixedUpdate(float fixedDeltaTime);
        void Exit();
    }
}


