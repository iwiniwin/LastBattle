using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDK.FSM;
using UDK.Event;
using UDK.Resource;
using GameDefine;

namespace Game
{
    public class UserInfoState : IGameState<EGameStateType>
    {
        

        GameObject mSceneRoot;

        public UserInfoState(){

        }

        public EGameStateType Type {
            get;
            set;
        } = EGameStateType.UserInfo;

        public EGameStateType NextStateType {
            get;
            set;
        } = EGameStateType.UserInfo;

        public void SetNextState(EGameStateType type){
            NextStateType = type;
        }

        public void Enter(){
            EventSystem.Broadcast(EGameEvent.ShowUserInfoView);
            // play bg audio
        }

        public void Exit(){

        }

        public void FixedUpdate(float fixedDeltaTime){

        }

        public bool Update(float deltaTime){
            return false;
        }
    }
}


