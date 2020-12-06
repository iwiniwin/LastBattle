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
        public LoginCtrl()
        {
        }

        public override void Init()
        {
            EventSystem.AddListener(EGameEvent.ShowLoginView, ShowView);
            EventSystem.AddListener(EGameEvent.HideLoginView, HideView);
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
            // Debug.Log(account + " " + password);
            SelectServerData.Instance.SetExtraInfo((int)SdkManager.Instance.PlatformType, account, password);
            NetworkManager.Instance.CanReconnect = false;
            NetworkManager.Instance.Init(GameConfig.LoginServerAddress, GameConfig.LoginServerPort, EServerType.LoginServer);
        }
    }
}


