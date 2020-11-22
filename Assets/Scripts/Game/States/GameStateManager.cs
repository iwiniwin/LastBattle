using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;

namespace Game
{

    public enum GameStateType {
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
        Dictionary<GameStateType, IGameState> mStatesDic;
        IGameState currentState;

        public GameStateManager(){
            mStatesDic = new Dictionary<GameStateType, IGameState>();
        }

        public IGameState CurrentState {
            get{
                return currentState;
            }
        }

        public void ChangeGameStateTo(GameStateType stateType){
            if(currentState != null && currentState.Type != GameStateType.Loading && currentState.Type == stateType)
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
            ChangeGameStateTo(GameStateType.Login);
        }

        public void FixedUpdate(float fixedDeltaTime){
            if(currentState != null){
                currentState.FixedUpdate(fixedDeltaTime);
            }
        }

        public void Update(float deltaTime){
            GameStateType nextStateType = GameStateType.Continue;
            if(currentState != null){
                nextStateType = currentState.Update(deltaTime);
            }
            if(nextStateType > GameStateType.Continue){
                ChangeGameStateTo(nextStateType);
            }
        }

        public IGameState GetState(GameStateType type){
            if(!mStatesDic.ContainsKey(type)){
                return null;
            }
            return mStatesDic[type];
        }
    }
}


