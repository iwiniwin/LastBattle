using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDK.Resource;

namespace Game
{
    public class LoginState : IGameState
    {
        GameStateType type;

        GameObject mSceneRoot;

        public LoginState(){

        }

        public GameStateType Type {
            get;
            set;
        }

        public void SetStateTo(GameStateType type){
            this.type = type;
        }

        public void Enter(){
            SetStateTo(GameStateType.Continue);
            ResourceUnit unit = ResourceManager.Instance.LoadImmediate("", ResourceType.PREFAB);
            mSceneRoot = GameObject.Instantiate(unit.Asset) as GameObject;
            
        }

        public void Exit(){

        }

        public void FixedUpdate(float fixedDeltaTime){

        }

        public GameStateType Update(float deltaTime){
            return type;
        }
    }
}


