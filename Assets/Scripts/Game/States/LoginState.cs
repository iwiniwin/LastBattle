using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDK.Resource;

namespace Game
{
    public class LoginState : IGameState
    {
        EGameStateType type;

        GameObject mSceneRoot;

        public LoginState(){

        }

        public EGameStateType Type {
            get;
            set;
        }

        public void SetStateTo(EGameStateType type){
            this.type = type;
        }

        public void Enter(){
            SetStateTo(EGameStateType.Continue);
            ResourceUnit unit = ResourceManager.Instance.LoadImmediate("", EResourceType.PREFAB);
            mSceneRoot = GameObject.Instantiate(unit.Asset) as GameObject;
            
        }

        public void Exit(){

        }

        public void FixedUpdate(float fixedDeltaTime){

        }

        public EGameStateType Update(float deltaTime){
            return type;
        }
    }
}


