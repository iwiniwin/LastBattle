using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDK.MVC;
using UDK.Event;
using UDK.Network;
using System;
using UDK.FSM;
using GameDefine;

namespace Game
{
    public class MiniMapCtrl : BaseCtrl
    {
        public new MiniMapView View;
        public MiniMapCtrl()
        {
        }

        public override void Init()
        {
            View = base.View as MiniMapView;

            EventSystem.AddListener(EGameEvent.ShowGamePlayView, ShowView);
            EventSystem.AddListener(EGameEvent.HideGamePlayView, HideView);
        }

        public override void Release()
        {
            EventSystem.RemoveListener(EGameEvent.ShowGamePlayView, ShowView);
            EventSystem.RemoveListener(EGameEvent.HideGamePlayView, HideView);
        }

        public override void Update(float deltaTime)
        {
        }
    }
}


