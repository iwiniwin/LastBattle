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

        // 重新登录面板
        Transform mReloginParent;
        // 重新登录按钮
        Transform mReLoginSubmit;

        // 开始游戏
        Transform mPlaySubmitBtn;
        Animator mPlayAnimate;

        Text mChangeAccountName;
        Transform mChangeAccountBtn;

        Text mPlayNameLabel;
        Text mPlayStateLabel;

        Transform mPlayParent;
        Transform mServerParent;
        Transform mLoginParent;

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

            mLoginParent = Root.Find("ChooseServer");
            mLoginSubmit = Root.Find("ChooseServer/Button");
            mWaitingParent = Root.Find("Connecting");
            mLoginAccountInput = Root.Find("ChooseServer/Loginer/AccountInput").GetComponent<InputField>();
            mLoginPasswordInput = Root.Find("ChooseServer/Loginer/PasswordInput").GetComponent<InputField>();

            mPlayParent = Root.Find("LoginBG");
            mPlaySubmitBtn = Root.Find("LoginBG/StartBtn");
            mPlayAnimate = mPlaySubmitBtn.GetComponent<Animator>();
            mServerParent = Root.Find("GameServerUI");
            mChangeAccountBtn = Root.Find("ChangeAccount");

            mChangeAccountName = Root.Find("ChangeAccount/Label1").GetComponent<Text>();

            mPlayNameLabel = Root.Find("LoginBG/CurrentSelection/Label2").GetComponent<Text>();
            mPlayStateLabel = Root.Find("LoginBG/CurrentSelection/Label1").GetComponent<Text>();

            mReloginParent = Root.Find("LogInAgain");
            mReLoginSubmit = Root.Find("LogInAgain/Status/Button");
            
            EventListener.Get(mLoginSubmit.gameObject).onClick += OnLoginSubmit;
            EventListener.Get(mPlaySubmitBtn.gameObject).onClick += OnPlaySubmit;
            EventListener.Get(mReLoginSubmit.gameObject).onClick += OnReloginSubmit;
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

        public void ShowLoginUI() {
            ShowSelectServerInfo();
            mPlayParent.gameObject.SetActive(true);
            mServerParent.gameObject.SetActive(false);
            mLoginParent.gameObject.SetActive(false);
            mChangeAccountName.text = mLoginAccountInput.text;
            mChangeAccountBtn.gameObject.SetActive(true);

            mWaitingParent.gameObject.SetActive(false);
        }

        public void ShowLoginFailUI() {
            mReloginParent.gameObject.SetActive(true);
            mWaitingParent.gameObject.SetActive(false);
            EventListener.Get(mLoginSubmit.gameObject).onClick += OnLoginSubmit;
        }

        public void ShowSelectServerInfo() {
            GameServerData.ServerInfo info = GameServerData.Instance.CurSelectServerInfo;
            mPlayNameLabel.text = info.name;
            var pair = GameServerData.StateDescrption[(int)info.state];
            mPlayStateLabel.text = "(" + pair.Key + ")";
            mPlayStateLabel.color = pair.Value;
        }

        public void ShowServerUI() {

        }

        public void ShowSelectServerUI() {

        }

        public void ShowLoginSuccessUI() {
            EventListener.Get(mPlaySubmitBtn.gameObject).onClick -= OnPlaySubmit;
        }

        /* UI事件响应 */

        // 点击登录按钮回调
        void OnLoginSubmit(GameObject gameObject, PointerEventData eventData) {
            string account = mLoginAccountInput.text;
            string password = mLoginPasswordInput.text;
            if(string.IsNullOrEmpty(account) || string.IsNullOrEmpty(password))
                return;
            mWaitingParent.gameObject.SetActive(true);
            Ctrl.Login(account, password);
        }

        // 点击开始游戏按钮回调
        void OnPlaySubmit(GameObject gameObject, PointerEventData eventData) {
            EventListener.Get(mPlaySubmitBtn.gameObject).onClick -= OnPlaySubmit;
            mWaitingParent.gameObject.SetActive(true);
            Ctrl.StartGame();
        }

        // 点击重新登录回调
        void OnReloginSubmit(GameObject gameObject, PointerEventData eventData) {
            mReloginParent.gameObject.SetActive(false);
            // todo 重新登录
        }
    }
}


