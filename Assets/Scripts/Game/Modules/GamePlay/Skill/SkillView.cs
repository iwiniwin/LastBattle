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
    public class SkillView : BaseView
    {
        public new SkillCtrl Ctrl;

        Button skillButton;
        Button attackButton;

        private enum SkillState {
            Normal,
            Fury
        }

        public SkillView(){
            ResName = GameConfig.SkillUIPath;
            IsResident = false;
        }

        public override void Init()
        {
            Ctrl = base.Ctrl as SkillCtrl;
        }

        protected override void OnLoad()
        {
            RectTransform transform = Root.gameObject.GetComponent<RectTransform>();
            transform.offsetMin = new Vector2(0.0f, 0.0f);  // left  bottom
            transform.offsetMax = new Vector2(0.0f, 0.0f);  // right top

            skillButton = Root.Find("Panel/Button_0").GetComponent<Button>();
            EventListener.Get(skillButton.gameObject).onClick += OnClickSkillBtn;

            attackButton = Root.Find("Panel/Button_6").GetComponent<Button>();
            EventListener.Get(attackButton.gameObject).onClick += OnClickAttackBtn;
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

        private void SendSkill(int btn) {
            ESkillType type = GetSkillType(btn);
            if(type == ESkillType.NULL)
                return;
            PlayerManager.Instance.LocalPlayer.SendPreparePlaySkill(type);
        }

        ESkillType GetSkillType(int btn) {
            ESkillType type = ESkillType.NULL;
            switch(btn) {
                case 0:
                    type = ESkillType.TYPE1;
                    break;
            }
            return type;
        }

        /* UI事件响应 */

        void OnClickSkillBtn(GameObject gameObject, PointerEventData eventData) {
            Player player = PlayerManager.Instance.LocalPlayer;
            if(player.FSM == null || player.FSM.State == UDK.FSM.EFSMState.DEAD)
                return;
            SendSkill(0);
        }

        void OnClickAttackBtn(GameObject gameObject, PointerEventData eventData) {
            Player player = PlayerManager.Instance.LocalPlayer;
            // todo
            player.EntityFSMChangeDataOnPrepareSkill(player.EntityFSMPosition, player.EntityFSMDirection, 1150301, player.SkillTarget);
            player.OnFSMStateChange(EntityReleaseSkillFSM.Instance);
        }
    }
}


