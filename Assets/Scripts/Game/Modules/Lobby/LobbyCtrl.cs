using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDK.MVC;
using UDK.Event;
using UDK.Network;

namespace Game
{
    public class LobbyCtrl : BaseCtrl
    {
        public new LobbyView View;
        public LobbyCtrl()
        {
        }

        public override void Init()
        {
            View = base.View as LobbyView;

            EventSystem.AddListener(EGameEvent.ShowLobbyView, ShowView);
            EventSystem.AddListener(EGameEvent.HideLobbyView, HideView);
        }

        public override void Release()
        {
            EventSystem.RemoveListener(EGameEvent.ShowLobbyView, ShowView);
            EventSystem.RemoveListener(EGameEvent.HideLobbyView, HideView);
        }

        public override void Update(float deltaTime)
        {
        }

        public void SendCompleteBaseInfo(byte[] nickName, int headId, byte sex){
            MessageCenter.Instance.SendCompleteBaseInfo(nickName, headId, sex);
        }
    }
}


