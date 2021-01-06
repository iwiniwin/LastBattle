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
    public class GamePlayView : BaseView
    {
        public new GamePlayCtrl Ctrl;

        public GamePlayView(){
            ResName = GameConfig.GamePlayMainUIPath;
            IsResident = false;
        }

        public override void Init()
        {
            Ctrl = base.Ctrl as GamePlayCtrl;
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

        /* UI事件响应 */

        void OnMatchBattleChanged(bool on) {
         
        }

    }
}


