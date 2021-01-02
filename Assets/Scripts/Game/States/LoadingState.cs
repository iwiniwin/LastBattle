using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDK.FSM;
using UDK.Event;
using UDK.Resource;
using GameDefine;
using UDK;
using UDK.UI;

namespace Game
{
    public class LoadingState : IGameState<EGameStateType>
    {
        

        GameObject mUIRoot;

        public LoadingState(){

        }

        public EGameStateType Type {
            get;
            set;
        } = EGameStateType.Loading;

        public EGameStateType NextStateType {
            get;
            set;
        } = EGameStateType.Loading;

        
        public void Enter(){
            if(NextStateType == EGameStateType.Play) {
                EventSystem.Broadcast(EGameEvent.ShowLoadingView);
            }else {
                // 返回登录场景
            }
        }

        public void Exit(){
            EventSystem.Broadcast(EGameEvent.HideLoadingView);
        }

        public void FixedUpdate(float fixedDeltaTime){

        }

        public bool Update(float deltaTime){
            return false;
        }
    }
}


