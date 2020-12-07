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

            NetworkManager.Instance.OnConnectServerSuccess += OnConnectServerSuccess;
            NetworkManager.Instance.OnConnectServerFailed += OnConnectServerFailed;
            NetworkManager.Instance.OnReceiveMessage += MessageCenter.Instance.HandleMessage;
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
            SelectServerData.Instance.SetExtraInfo((int)SdkManager.Instance.PlatformType, account, password);
            NetworkManager.Instance.EnableConnect = true;
        }

        public void OnGetBSAddress(LSToGC.ServerBSAddr msg) {
            SelectServerData.Instance.Clean();
            for(int i = 0; i < msg.serverinfo.Count; i ++) {
                string address = msg.serverinfo[i].ServerAddr;
                int state = msg.serverinfo[i].ServerState;
                string name = msg.serverinfo[i].ServerName;
                int port = msg.serverinfo[i].ServerPort;
                SelectServerData.Instance.AddServerInfo(i, name, (SelectServerData.EServerState)state, address, port);
            }

            NetworkManager.Instance.Close();
            NetworkManager.Instance.EnableConnect = false;

            SelectServerData.Instance.SetDefaultServer();

            View.ShowLoginUI();

        }

        public void OnConnectServerSuccess() {
            MessageCenter.Instance.AskLoginToLoginServer();
        }

        public void OnConnectServerFailed() {
            UDK.Output.Dump("OnConnectServerFailed");
        }

    }
}


