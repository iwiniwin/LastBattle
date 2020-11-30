using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDK.FSM;
using UDK.Resource;
using GameDefine;

namespace Game
{
    public class LoginState : IGameState<EGameStateType>
    {
        

        GameObject mSceneRoot;

        public LoginState(){

        }

        public EGameStateType Type {
            get;
            set;
        }

        public EGameStateType NextStateType {
            get;
            set;
        }

        public void SetNextState(EGameStateType type){
            NextStateType = type;
        }

        public void Enter(){
            SetNextState(EGameStateType.Continue);
            ResourceUnit unit = ResourceManager.Instance.LoadImmediate("", EResourceType.PREFAB);
            mSceneRoot = GameObject.Instantiate(unit.Asset) as GameObject;
            
        }

        public void Exit(){

        }

        public void FixedUpdate(float fixedDeltaTime){

        }

        public EGameStateType Update(float deltaTime){
            return NextStateType;
        }
    }
}


