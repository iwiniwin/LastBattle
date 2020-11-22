using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public interface IGameState
    {
        GameStateType Type{get; set;}

        void SetStateTo(GameStateType type);
        void Enter();
        GameStateType Update(float deltaTime);
        void FixedUpdate(float fixedDeltaTime);
        void Exit();
    }
}


