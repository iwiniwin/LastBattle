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
    public class BattleView : BaseView
    {
        public new BattleCtrl Ctrl;

        public BattleView(){
            ResName = GameConfig.LobbyBattleUIPath;
            IsResident = true;
        }

        public override void Init()
        {
            Ctrl = base.Ctrl as BattleCtrl;
        }

        protected override void OnLoad()
        {
            RectTransform transform = Root.gameObject.GetComponent<RectTransform>();
            transform.offsetMin = new Vector2(0.0f, 0.0f);  // left  bottom
            transform.offsetMax = new Vector2(0.0f, 0.0f);  // right top
            transform.localScale = new Vector3(0.65f, 0.65f, 1.0f);
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


