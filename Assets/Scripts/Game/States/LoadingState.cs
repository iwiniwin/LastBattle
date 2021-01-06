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
            EventSystem.AddListener(EGameEvent.LoadGameSceneFinish, OnLoadGameSceneFinish);
            EventSystem.Broadcast(EGameEvent.ShowLoadingView);
        }

        public void Exit(){
            EventSystem.RemoveListener(EGameEvent.LoadGameSceneFinish, OnLoadGameSceneFinish);
            EventSystem.Broadcast(EGameEvent.HideLoadingView);
        }

        public void OnLoadGameSceneFinish() {
            // 切换到下一状态
            NextStateType = EGameStateType.Play;
        }

        public void FixedUpdate(float fixedDeltaTime){
        }

        public bool Update(float deltaTime){
            return NextStateType != EGameStateType.Loading;
        }
    }
}


