using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDefine;
using UDK.MVC;
using UnityEngine.EventSystems;
using UDK.Event;
using UnityEngine.UI;

namespace Game 
{
    public class LoadingView : BaseView
    {
        public Scrollbar mProgressBar;

        public new LoadingCtrl Ctrl;

        public LoadingView(){
            ResName = GameConfig.LoadProgressPrefabPath;
            // EnablePreload = true;
            IsResident = false;
        }

        public override void Init()
        {
            Ctrl = base.Ctrl as LoadingCtrl;
        }

        protected override void OnLoad()
        {
            RectTransform transform = Root.gameObject.GetComponent<RectTransform>();
            transform.offsetMin = new Vector2(0.0f, 0.0f);  // left  bottom
            transform.offsetMax = new Vector2(0.0f, 0.0f);  // right top

            mProgressBar = Root.Find("ProgressBar").GetComponent<Scrollbar>();

            Ctrl.LoadScene();
        }

        public void SetProgress(float value) {
            if(mProgressBar != null){
                mProgressBar.size = value;
            }
        }

        public float GetProgress() {
            if(mProgressBar != null) {
                return mProgressBar.size;
            }
            return 0.0f;
        }

        public override void OnEnable()
        {
            
        }

        public override void OnDisable()
        {
           
        }

        public override void Release()
        {
            
        }
    }
}


