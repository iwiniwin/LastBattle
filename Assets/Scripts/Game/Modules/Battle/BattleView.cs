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

        Toggle mMatchToggle;
        Toggle mCustomToggle;  // 自定义
        Toggle mTrainMarketToggle;

        Transform mMatchInterface;
        Transform mCustomInterface;
        Transform mTrainInterface;

        Button mNoviceMapBtn;  // 新手引导地图选择按钮

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

            mMatchToggle = Root.Find("Menu/ModeSelect/Match").GetComponent<Toggle>();
            mCustomToggle = Root.Find("Menu/ModeSelect/Custom").GetComponent<Toggle>();
            mTrainMarketToggle = Root.Find("Menu/ModeSelect/Training").GetComponent<Toggle>();

            mMatchInterface = Root.Find("Box/MatchInterface");
            mCustomInterface = Root.Find("Box/CustomInterface");
            mTrainInterface = Root.Find("Box/TrainingInterface");

            mNoviceMapBtn = Root.Find("Box/TrainingInterface/MapSelect/Novice").GetComponent<Button>();

            mMatchToggle.onValueChanged.AddListener(OnMatchBattleChanged);
            mCustomToggle.onValueChanged.AddListener(OnCustomToggleChanged);
            mTrainMarketToggle.onValueChanged.AddListener(OnTrainBattleChanged);

            EventListener.Get(mNoviceMapBtn.gameObject).onClick += OnClickNoviceMap;
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
            mMatchInterface.gameObject.SetActive(on);
            mCustomInterface.gameObject.SetActive(!on);
            mTrainInterface.gameObject.SetActive(!on);
        }

        void OnCustomToggleChanged(bool on) {
            mMatchInterface.gameObject.SetActive(!on);
            mCustomInterface.gameObject.SetActive(on);
            mTrainInterface.gameObject.SetActive(!on);
        }

        void OnTrainBattleChanged(bool on) {
            mMatchInterface.gameObject.SetActive(!on);
            mCustomInterface.gameObject.SetActive(!on);
            mTrainInterface.gameObject.SetActive(on);
        }

        void OnClickNoviceMap(GameObject gameObject, PointerEventData eventData) {
            Ctrl.AskCreateNoviceGuideBattle(1000);
        }
    }
}


