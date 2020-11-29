using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDK;

namespace Game
{

    public enum EGameStateType {
        Continue,
        Login,
        User,
        Lobby,
        Room,
        Hero,
        Loading,
        Play,
        Over,
    }

    public class GameStateManager : Singleton<GameStateManager>
    {
        Dictionary<EGameStateType, IGameState> mStatesDic;
        IGameState currentState;

        public GameStateManager(){
            mStatesDic = new Dictionary<EGameStateType, IGameState>();
        }

        public IGameState CurrentState {
            get{
                return currentState;
            }
        }

        public void ChangeGameStateTo(EGameStateType stateType){
            if(currentState != null && currentState.Type != EGameStateType.Loading && currentState.Type == stateType)
                return;

            if(mStatesDic.ContainsKey(stateType)){
                if(currentState != null){
                    currentState.Exit();
                }
                currentState = mStatesDic[stateType];
                currentState.Enter();
            }
        }

        public void EnterDefaultState(){
            ChangeGameStateTo(EGameStateType.Login);
        }

        public void FixedUpdate(float fixedDeltaTime){
            if(currentState != null){
                currentState.FixedUpdate(fixedDeltaTime);
            }
        }

        public void Update(float deltaTime){
            EGameStateType nextStateType = EGameStateType.Continue;
            if(currentState != null){
                nextStateType = currentState.Update(deltaTime);
            }
            if(nextStateType > EGameStateType.Continue){
                ChangeGameStateTo(nextStateType);
            }
        }

        public IGameState GetState(EGameStateType type){
            if(!mStatesDic.ContainsKey(type)){
                return null;
            }
            return mStatesDic[type];
        }
    }
}


