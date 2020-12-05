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
    public class LoginView : BaseView
    {
        public new LoginCtrl Ctrl;
        // 登录按钮
        Transform mLoginSubmit;
        // 账号输入
        InputField mLoginAccountInput;
        // 密码输入
        InputField mLoginPasswordInput;
        // 等待中
        Transform mWaitingParent;

        public LoginView(){
            ResName = GameConfig.LoginUIPath;
            // EnablePreload = true;
            IsResident = false;
        }

        public override void Init()
        {
            Ctrl = base.Ctrl as LoginCtrl;
        }

        protected override void OnLoad()
        {
            RectTransform transform = Root.gameObject.GetComponent<RectTransform>();
            transform.offsetMin = new Vector2(0.0f, 0.0f);  // left  bottom
            transform.offsetMax = new Vector2(0.0f, 0.0f);  // right top

            mLoginSubmit = Root.Find("ChooseServer/Button");
            mWaitingParent = Root.Find("Connecting");
            mLoginAccountInput = Root.Find("ChooseServer/Loginer/AccountInput").GetComponent<InputField>();
            mLoginPasswordInput = Root.Find("ChooseServer/Loginer/PasswordInput").GetComponent<InputField>();
            EventListener.Get(mLoginSubmit.gameObject).onClick += OnLoginSubmit;
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

        void OnLoginSubmit(GameObject gameObject, PointerEventData eventData) {
            string account = mLoginAccountInput.text;
            string password = mLoginPasswordInput.text;
            if(string.IsNullOrEmpty(account) || string.IsNullOrEmpty(password))
                return;
            mWaitingParent.gameObject.SetActive(true);
            Ctrl.Login(account, password);
        }
    }
}


