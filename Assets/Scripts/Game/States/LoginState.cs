using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDK.FSM;
using UDK.Event;
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
        } = EGameStateType.Login;

        public EGameStateType NextStateType {
            get;
            set;
        } = EGameStateType.Login;  // 等于自身，则不切换状态

        public void Enter(){
            ResourceUnit unit = ResourceManager.Instance.LoadImmediate(GameConfig.LoginPrefabPath, EResourceType.PREFAB);
            mSceneRoot = GameObject.Instantiate(unit.Asset) as GameObject;
            
            EventSystem.Broadcast(EGameEvent.ShowLoginView);

            ResourceUnit audioClipUnit = ResourceManager.Instance.LoadImmediate(GameConfig.UIBGSoundPath, EResourceType.ASSET);
            AudioClip clip = audioClipUnit.Asset as AudioClip;
            
            // 播放背景音乐
            AudioSource source = GameObject.Find("GameLogic").AddComponent<AudioSource>();
            source.loop = true;
            source.playOnAwake = false;
            source.volume = GameSettings.AudioVolume;
            source.clip = clip;
            source.Play();

            EventSystem.AddListener<GSToGC.UserBaseInfo>(EGameEvent.OnReceiveUserBaseInfo, OnReceiveUserBaseInfo);
        }

        public void Exit(){
            EventSystem.RemoveListener<GSToGC.UserBaseInfo>(EGameEvent.OnReceiveUserBaseInfo, OnReceiveUserBaseInfo);
        }

        public void FixedUpdate(float fixedDeltaTime){

        }

        public bool Update(float deltaTime){
            return NextStateType != EGameStateType.Login;
        }

        public void OnReceiveUserBaseInfo(GSToGC.UserBaseInfo msg) {
            if(msg.nickname.Length > 1) {
                // GameUserModel.Instance.SetGameBaseInfo(pMsg);
                // EventCenter.SendEvent(new CEvent(EGameEvent.eGameEvent_IntoLobby));
            }else if(msg.guid > 0) {
                // 玩家没有昵称，进入补充玩家信息界面
                NextStateType = EGameStateType.UserInfo;
            }
        }
    }
}


