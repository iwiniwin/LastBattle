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
            EventSystem.Broadcast(EGameEvent.ShowLobbyView);

            EventSystem.AddListener<EGameStateType>(EGameEvent.LoadingGame, OnLoadingGame);
        }

        public void Exit(){
            EventSystem.Broadcast(EGameEvent.HideLobbyView);

            EventSystem.RemoveListener<EGameStateType>(EGameEvent.LoadingGame, OnLoadingGame);
        }

        public void FixedUpdate(float fixedDeltaTime){

        }

        public bool Update(float deltaTime){
            return NextStateType != EGameStateType.Lobby;
        }

        public void OnLoadingGame(EGameStateType nextStateType) {
            LoadingState state = GameStateManager<EGameStateType>.Instance.GetState(EGameStateType.Loading) as LoadingState;
            state.NextStateType = nextStateType;
            NextStateType = EGameStateType.Loading;
        }
    }
}


