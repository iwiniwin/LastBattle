using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDefine;
using UDK.MVC;
using UnityEngine.EventSystems;
using UDK.Event;
using UnityEngine.UI;
using System.Text;

namespace Game 
{
    public class UserInfoView : BaseView
    {
        public new UserInfoCtrl Ctrl;

        Button mSubmitBtn;
        InputField mNickNameInput;
        Image mHeadSprite;
        

        public UserInfoView(){
            ResName = GameConfig.RegisterUIPath;
            IsResident = false;
        }

        public override void Init()
        {
            Ctrl = base.Ctrl as UserInfoCtrl;
        }

        protected override void OnLoad()
        {
            RectTransform transform = Root.gameObject.GetComponent<RectTransform>();
            transform.offsetMin = new Vector2(0.0f, 0.0f);  // left  bottom
            transform.offsetMax = new Vector2(0.0f, 0.0f);  // right top

            // 提交按钮
            mSubmitBtn = Root.Find("Commit").GetComponent<Button>();
            // mSubmitBtn.enabled = false;
            // 昵称输入框
            mNickNameInput = Root.Find("NickName").GetComponent<InputField>();
            mHeadSprite = Root.Find("HeadSelect/BG").GetComponent<Image>();

            EventListener.Get(mSubmitBtn.gameObject).onClick += OnRegisterSubmit;
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

        private bool CheckNameInput() {
            if(string.IsNullOrEmpty(mNickNameInput.text)){
                ShowTip("昵称不能为空");
                return false;
            }
            if(StringUtil.CheckName(mNickNameInput.text)){
                ShowTip("无效昵称名");
                return false;
            }
            return true;
        }   

        private void ShowTip(string text) {

        }

        /* UI事件响应 */

        // 点击提交按钮回调
        void OnRegisterSubmit(GameObject gameObject, PointerEventData eventData) {
            // if(!CheckNameInput()){
            //     return;
            // }

            byte sex = (byte)1;  // boy 1 girl 2
            int headId = 17;  // 头像
            byte[] bytes = Encoding.UTF8.GetBytes(mNickNameInput.text);
            Ctrl.SendCompleteBaseInfo(bytes, headId, sex);
        }
    }
}


