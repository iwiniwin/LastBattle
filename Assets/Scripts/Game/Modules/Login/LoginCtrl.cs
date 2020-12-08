using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDK.MVC;
using UDK.Event;
using UDK.Network;

namespace Game
{
    public class LoginCtrl : BaseCtrl
    {
        public new LoginView View;
        public LoginCtrl()
        {
        }

        public override void Init()
        {
            View = base.View as LoginView;
            EventSystem.AddListener(EGameEvent.ShowLoginView, ShowView);
            EventSystem.AddListener(EGameEvent.HideLoginView, HideView);
            EventSystem.AddListener<LSToGC.ServerBSAddr>(EGameEvent.OnGetBSAddress, OnGetBSAddress);

            NetworkManager.Instance.OnConnectServerFailed += OnConnectServerFailed;
        }

        public override void Release()
        {
            EventSystem.RemoveListener(EGameEvent.ShowLoginView, ShowView);
            EventSystem.RemoveListener(EGameEvent.HideLoginView, HideView);
        }

        public override void Update(float deltaTime)
        {
            throw new System.NotImplementedException();
        }

        public void Login(string account, string password){
            GameServerData.Instance.SetExtraInfo((int)SdkManager.Instance.PlatformType, account, password);
            NetworkManager.Instance.Configure(GameConfig.LoginServerAddress, GameConfig.LoginServerPort);

            NetworkManager.Instance.OnConnectServerSuccess += OnConnectLSSuccess;
            

            NetworkManager.Instance.EnableConnect = true;
        }

        public void OnGetBSAddress(LSToGC.ServerBSAddr msg) {
            GameServerData.Instance.Clean();
            for(int i = 0; i < msg.serverinfo.Count; i ++) {
                string address = msg.serverinfo[i].ServerAddr;
                int state = msg.serverinfo[i].ServerState;
                string name = msg.serverinfo[i].ServerName;
                int port = msg.serverinfo[i].ServerPort;
                GameServerData.Instance.AddServerInfo(i, name, (GameServerData.EServerState)state, address, port);
            }

            NetworkManager.Instance.Close();
            NetworkManager.Instance.EnableConnect = false;

            GameServerData.Instance.SetDefaultServer();

            View.ShowLoginUI();

        }

        // 开始游戏
        public void StartGame() {
            GameServerData.ServerInfo info = GameServerData.Instance.CurSelectServerInfo;
            NetworkManager.Instance.Configure(info.address, info.port);

            NetworkManager.Instance.OnConnectServerSuccess += OnConnectBSSuccess;

            NetworkManager.Instance.EnableConnect = true;
            PlayerPrefs.SetString(GameServerData.ServerKey, info.name);
        }

        // 连接到登录服务器成功回调
        public void OnConnectLSSuccess() {
            NetworkManager.Instance.OnConnectServerSuccess -= OnConnectLSSuccess;
            MessageCenter.Instance.AskLoginToLoginServer();
        }

        // 连接到负载均衡服务器成功回调
        public void OnConnectBSSuccess() {
            NetworkManager.Instance.OnConnectServerSuccess -= OnConnectBSSuccess;
            UDK.Output.Dump("vvvvvvvvvvvvvvvvv");
        }

        public void OnConnectServerFailed() {
            UDK.DebugEx.LogWarning("服务器连接失败");
        }

    }
}


