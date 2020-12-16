using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDK.MVC;
using UDK.Event;
using UDK.Network;

namespace Game
{
    public class BattleCtrl : BaseCtrl
    {
        public new BattleView View;
        public BattleCtrl()
        {
        }

        public override void Init()
        {
            View = base.View as BattleView;

            EventSystem.AddListener(EGameEvent.ShowBattleView, ShowView);
            EventSystem.AddListener(EGameEvent.HideBattleView, HideView);
        }

        public override void Release()
        {
            EventSystem.RemoveListener(EGameEvent.ShowBattleView, ShowView);
            EventSystem.RemoveListener(EGameEvent.HideBattleView, HideView);
        }

        public override void Update(float deltaTime)
        {
        }

        public void SendCompleteBaseInfo(byte[] nickName, int headId, byte sex){
            MessageCenter.Instance.SendCompleteBaseInfo(nickName, headId, sex);
        }
    }
}


