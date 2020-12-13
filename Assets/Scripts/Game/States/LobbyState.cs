using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDK.FSM;
using UDK.Event;
using UDK.Resource;
using GameDefine;

namespace Game
{
    public class LobbyState : IGameState<EGameStateType>
    {
        

        GameObject mSceneRoot;

        public LobbyState(){

        }

        public EGameStateType Type {
            get;
            set;
        } = EGameStateType.Lobby;

        public EGameStateType NextStateType {
            get;
            set;
        } = EGameStateType.Lobby;

        public void Enter(){
            UDK.Output.Dump("jinru..............");
            EventSystem.Broadcast(EGameEvent.ShowLobbyView);
        }

        public void Exit(){
            EventSystem.Broadcast(EGameEvent.HideLobbyView);
        }

        public void FixedUpdate(float fixedDeltaTime){

        }

        public bool Update(float deltaTime){
            return false;
        }
    }
}


