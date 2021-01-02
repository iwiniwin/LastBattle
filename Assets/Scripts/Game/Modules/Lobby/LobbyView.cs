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
    public class LobbyView : BaseView
    {
        public new LobbyCtrl Ctrl;

        Toggle mHomeToggle;
        ParticleSystem mHomeParticle;
        Toggle mBattleToggle;
        ParticleSystem mBattleParticle;
        Toggle mMarketToggle;
        ParticleSystem mMarketParticle;
        Toggle mInteractionToggle;
        ParticleSystem mInteractionParticle;

        private Dictionary<string, ParticleSystem> mParticles;

        public LobbyView(){
            ResName = GameConfig.LobbyUIPath;
            IsResident = false;

            mParticles = new Dictionary<string, ParticleSystem>();
        }

        public override void Init()
        {
            Ctrl = base.Ctrl as LobbyCtrl;
        }

        protected override void OnLoad()
        {
            RectTransform transform = Root.gameObject.GetComponent<RectTransform>();
            transform.offsetMin = new Vector2(0.0f, 0.0f);  // left  bottom
            transform.offsetMax = new Vector2(0.0f, 0.0f);  // right top

            mHomeToggle = Root.Find("StartMenu/Home").GetComponent<Toggle>();
            mHomeParticle = Root.Find("StartMenu/Home/press").GetComponent<ParticleSystem>();
            mParticles.Add("Home", mHomeParticle);
            mBattleToggle = Root.Find("StartMenu/Battle").GetComponent<Toggle>();
            mBattleParticle = Root.Find("StartMenu/Battle/press").GetComponent<ParticleSystem>();
            mParticles.Add("Battle", mBattleParticle);
            mMarketToggle = Root.Find("StartMenu/Market").GetComponent<Toggle>();
            mMarketParticle = Root.Find("StartMenu/Market/press").GetComponent<ParticleSystem>();
            mParticles.Add("Market", mMarketParticle);
            mInteractionToggle = Root.Find("StartMenu/Interaction").GetComponent<Toggle>();
            mInteractionParticle = Root.Find("StartMenu/Interaction/press").GetComponent<ParticleSystem>();
            mParticles.Add("Interaction", mInteractionParticle);
            
            EventListener.Get(mHomeToggle.gameObject).onSelect += OnMenuSelect;
            EventListener.Get(mBattleToggle.gameObject).onSelect += OnMenuSelect;
            EventListener.Get(mMarketToggle.gameObject).onSelect += OnMenuSelect;
            EventListener.Get(mInteractionToggle.gameObject).onSelect += OnMenuSelect;
            mHomeToggle.onValueChanged.AddListener(OnHomeToggleChanged);
            mBattleToggle.onValueChanged.AddListener(OnHomeBattleChanged);
            
            // 加载战斗模块
            ModuleManager.Instance.LoadModule<BattleView, BattleCtrl>();
            // 加载商城模块
            // 加载社交模块
        }

        public override void OnEnable()
        {
            
        }

        public override void OnDisable()
        {
           
        }

        public override void Release()
        {
            ModuleManager.Instance.UnloadModule<BattleView>();
        }

        private void DisableParticle(GameObject excludeObject) {
            foreach(var pair in mParticles){
                if(pair.Key != excludeObject.name){
                    pair.Value.Stop();
                    pair.Value.Clear();
                }else{
                    pair.Value.Play();
                }
            }
        }

        /* UI事件响应 */

        // 点击登录按钮回调
        void OnMenuSelect(GameObject gameObject, BaseEventData eventData) {
            DisableParticle(eventData.selectedObject);
        }

        void OnHomeToggleChanged(bool on) {

        }

        void OnHomeBattleChanged(bool on) {
            if(on)
                UDK.Event.EventSystem.Broadcast(EGameEvent.ShowBattleView);
            else
                UDK.Event.EventSystem.Broadcast(EGameEvent.HideBattleView);
        }

    }
}


