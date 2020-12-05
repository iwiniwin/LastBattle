using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDefine;
using UDK.MVC;
using UDK.Event;

namespace Game 
{
    public class LoginView : BaseView
    {
        public LoginView(){
            ResName = GameConfig.LoginUIPath;
            // EnablePreload = true;
            IsResident = false;
        }

        public override void Init()
        {
            
        }

        protected override void OnLoad()
        {
            RectTransform transform = Root.gameObject.GetComponent<RectTransform>();
            transform.offsetMin = new Vector2(0.0f, 0.0f);  // left  bottom
            transform.offsetMax = new Vector2(0.0f, 0.0f);  // right top
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


