using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDK.MVC 
{
    public abstract class BaseCtrl
    {
        public BaseView View {get; internal set;}  // 对应的视图

        public bool NeedUpdate {get; protected set;}  // 是否需要更新

        public abstract void Init();

        public abstract void Release();

        // 每帧更新
        public abstract void Update(float deltaTime);

        public void ShowView() {
            if(View != null)
                View.Show();
        }

        public void HideView() {
            if(View != null)
                View.Hide();
        }
    }
}


