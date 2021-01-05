using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDK.MVC;
using UDK.Event;
using UDK.Network;
using UDK;
using UDK.Resource;

namespace Game
{
    public class LoadingCtrl : BaseCtrl
    {
        private ResourceAsyncOperation mAsync;
        public new LoadingView View;
        public LoadingCtrl()
        {
            NeedUpdate = true;
        }

        public override void Init()
        {
            View = base.View as LoadingView;
            EventSystem.AddListener(EGameEvent.ShowLoadingView, ShowView);
            EventSystem.AddListener(EGameEvent.HideLoadingView, HideView);
        }

        public override void Release()
        {
            EventSystem.RemoveListener(EGameEvent.ShowLoadingView, ShowView);
            EventSystem.RemoveListener(EGameEvent.HideLoadingView, HideView);
        }

        const float mMaxProgressPercent = 0.95f;

        public override void Update(float deltaTime)
        {
            if(mAsync == null) return;
            float curProgress = View.GetProgress();
            if(curProgress < mMaxProgressPercent) 
                curProgress += 0.1f;
            else
                curProgress = mMaxProgressPercent;
            View.SetProgress(curProgress);
            if(mAsync.Complete) {
                // 与读取数据
                EventSystem.Broadcast(EGameEvent.LoadGameSceneFinish);
                HideView();
            }
        }

        // 加载场景
        public void LoadScene() {
            // 加载场景之前需要进行清除操作

            string name = GetLoadMapName();
            // ObjectPool.instanc.clear
            mAsync = ResourceManager.Instance.LoadLevel("Scenes/" + name, null);
        }

        private string GetLoadMapName() {
            MapInfo map = MapLoader.Instance.GetMapInfo(UserInfoModel.Instance.GameMapID);
            if(map == null){
                DebugEx.LogError("can not find map " + UserInfoModel.Instance.GameMapID);
            }
            return map.Scene;
        }


    }
}


