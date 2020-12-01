using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDK;

namespace UDK.FSM
{
    public class GameStateManager<T> : Singleton<GameStateManager<T>> where T : System.Enum
    {
        Dictionary<T, IGameState<T>> mStatesDic;
        IGameState<T> currentState;

        public GameStateManager(){
            mStatesDic = new Dictionary<T, IGameState<T>>();
        }

        public void Init(params IGameState<T>[] states){
            foreach(IGameState<T> state in states) {
                mStatesDic.Add(state.Type, state);
            }
        }

        public IGameState<T> CurrentState {
            get{
                return currentState;
            }
        }

        public void ChangeStateTo(T stateType){
            
            if(currentState != null && stateType.CompareTo(currentState.Type) == 0)
                return;

            if(mStatesDic.ContainsKey(stateType)){
                if(currentState != null){
                    currentState.Exit();
                }
                currentState = mStatesDic[stateType];
                currentState.Enter();
            }
        }

        public void FixedUpdate(float fixedDeltaTime){
            if(currentState != null){
                currentState.FixedUpdate(fixedDeltaTime);
            }
        }

        public void Update(float deltaTime){
            if(currentState != null){
                bool needChange = currentState.Update(deltaTime);
                ChangeStateTo(currentState.NextStateType);
            }
        }

        public IGameState<T> GetState(T type){
            if(!mStatesDic.ContainsKey(type)){
                return null;
            }
            return mStatesDic[type];
        }
    }
}


