using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDK.MVC;
using UDK.Event;
using UDK.Network;

namespace Game
{
    public class UserInfoCtrl : BaseCtrl
    {
        public new UserInfoView View;
        public UserInfoCtrl()
        {
        }

        public override void Init()
        {
            View = base.View as UserInfoView;

            EventSystem.AddListener(EGameEvent.ShowUserInfoView, ShowView);
            EventSystem.AddListener(EGameEvent.HideUserInfoView, HideView);
        }

        public override void Release()
        {
            EventSystem.RemoveListener(EGameEvent.ShowUserInfoView, ShowView);
            EventSystem.RemoveListener(EGameEvent.HideUserInfoView, HideView);
        }

        public override void Update(float deltaTime)
        {
        }

        public void SendCompleteBaseInfo(byte[] nickName, int headId, byte sex){
            MessageCenter.Instance.SendCompleteBaseInfo(nickName, headId, sex);
        }
    }
}


