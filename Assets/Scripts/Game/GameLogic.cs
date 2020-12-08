using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDK;
using UDK.FSM;
using GameDefine;
using UDK.Network;
using UDK.MVC;
using System.IO;

namespace Game 
{
    public class GameLogic : UnitySingleton<GameLogic>{
        public EBattleState BattleState {
            private set;
            get;
        }

        private bool IsCutLine = false;
        public bool IsInitialize = false;
        public bool IsQuickBattle = false;

        public List<string> IpList = new List<string>();

        public string LoginServerAddress = "";
        public int LoginServerPort = 49996;

        public bool SkipNewsGuide = false;

        private void Awake() {
            Application.runInBackground = true;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            ModuleManager.Instance.LoadModule<LoginView, LoginCtrl>();
        }

        private void Start() {
            // PlayerManager
            // NpcManager
            NetworkManager.Instance.Init(Serialize);
            NetworkManager.Instance.OnReceiveMessage += MessageCenter.Instance.HandleMessage;

            GameStateManager<EGameStateType>.Instance.Init(new LoginState());
            GameStateManager<EGameStateType>.Instance.ChangeStateTo(EGameStateType.Login);

            // 预加载，减少进入游戏资源加载卡顿
            // ConfigReader.Init();
            // GameMethod.FileRead();

            // 预加载特效信息
            // ReadPreLoadConfig.Instance.Init();
            // 需要释放的资源信息
            // ReadReleaseResourceConfig.Instance.Init();
        }

        private void Update() {
            // 更新buff

            // 更新特效

            // 更新提示消失

            // 场景声音更新

            // 声音更新

            // 更新游戏状态机
            GameStateManager<EGameStateType>.Instance.Update(Time.deltaTime);

            // 更新网络模块
            NetworkManager.Instance.Update(Time.deltaTime);

            // 更新界面引导

            // 小地图更新

            // UI更新
            // WindowManager.Instance.Update(Time.deltaTime);

            // 特效后删除机制

            // GameObjectPool更新
            // ObjectPool

            // 游戏时间设置
        }

        private void OnEnable() {
            // 添加事件注册
            // EventSystem.AddListener(EGameEvent.ConnectServerSuccess, OnConnectServerSuccess);
            // EventSystem.AddListener(EGameEvent.ConnectServerFail, OpenConnectUI);
            // EventSystem.AddListener(EGameEvent.ReconnectToBattle, OpenConnectUI);
            // EventSystem.AddListener(EGameEvent.BeginWaiting, OpenWaitingUI);

            // // 读取配置，是否启用声音
            // if(PlayerPrefs.HasKey(UIGameSetting.VoiceKey)){
            //     int voiceValue = PlayerPrefs.GetInt(UIGameSetting.VoiceKey);
            //     bool state = (voiceValue == 1) ? true : false;
                
            // }

            // if(PlayerPrefs.HasKey(UIGameSetting.SoundKey)){
            //     int voiceValue = PlayerPrefs.GetInt(UIGameSetting.SoundKey);
            //     bool state = (voiceValue == 1) ? true : false;
                
            // }
        }

        private void OnDisable() {
            // EventSystem.RemoveListener(EGameEvent.ConnectServerSuccess, OnConnectServerSuccess);
            // EventSystem.RemoveListener(EGameEvent.ConnectServerFail, OpenConnectUI);
            // EventSystem.RemoveListener(EGameEvent.ReconnectToBattle, OpenConnectUI);
            // EventSystem.RemoveListener(EGameEvent.BeginWaiting, OpenWaitingUI);
        }

        

        private void OpenConnectUI(){
            // PlayerManager
            // EntityManager
            GameLogic.Instance.IsInitialize = true;
        }

        private void OpenWaitingUI(){
            // if(Waitinter)
        }

        // 连接服务器成功
        private void OnConnectServerSuccess(){
            StopCoroutine("PingToServer");
            StartCoroutine("PingToServer");
        }

        private IEnumerator PingToServerr(){
            while(true){
                yield return new WaitForSeconds(1);
                // send msg to ask ping
            }
        }

        public void Serialize(MemoryStream stream, object msg) {
            ProtoBuf.Serializer.Serialize(stream, (ProtoBuf.IExtensible)msg);
        }

        // 游戏推出前执行
        private void OnApplicationQuit() {
            NetworkManager.Instance.Close();
        }
    }
}


