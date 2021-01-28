using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDK.MVC;
using UDK.Event;
using UDK.Network;
using System;
using UDK.FSM;
using GameDefine;
using UDK;

namespace Game
{
    public class BattleCtrl : BaseCtrl
    {
        public new BattleView View;
        public BattleCtrl()
        {
        }

        public override void Init()
        {
            View = base.View as BattleView;

            EventSystem.AddListener(EGameEvent.ShowBattleView, ShowView);
            EventSystem.AddListener(EGameEvent.HideBattleView, HideView);
            EventSystem.AddListener<GSToGC.BattleBaseInfo>(EGameEvent.OnReceiveBattleBaseInfo, OnReceiveBattleBaseInfo);
            EventSystem.AddListener<GSToGC.BattleSeatPosInfo>(EGameEvent.OnReceiveBattleSeatPosInfo, OnReceiveBattleSeatPosInfo);
            EventSystem.AddListener<GSToGC.BattleStateChange>(EGameEvent.OnReceiveBattleStateChange, OnReceiveBattleStateChange);
            EventSystem.AddListener<GSToGC.HeroList>(EGameEvent.OnReceiveHeroList, OnReceiveHeroList);
        }

        public override void Release()
        {
            EventSystem.RemoveListener(EGameEvent.ShowBattleView, ShowView);
            EventSystem.RemoveListener(EGameEvent.HideBattleView, HideView);
        }

        public override void Update(float deltaTime)
        {
        }

        public void SendCompleteBaseInfo(byte[] nickName, int headId, byte sex){
            MessageCenter.Instance.SendCompleteBaseInfo(nickName, headId, sex);
        }

        public void AskCreateNoviceGuideBattle(int mapId) {
            MessageCenter.Instance.AskCSToCreateGuideBattle(mapId, GCToCS.AskCSCreateGuideBattle.guidetype.first);
        }

        public void OnReceiveBattleBaseInfo(GSToGC.BattleBaseInfo msg) {
            UserInfoModel.Instance.GameBattleID = msg.battleid;
            UserInfoModel.Instance.GameMapID = msg.mapid;
            UserInfoModel.Instance.IsReconnect = msg.ifReconnect;
            if(msg.ifReconnect) {
                // 请求重连
            }else{
                // 请求进入战斗
                Int64 nowTime = UDK.TimeUtil.GetUTCMillisec();
                MessageCenter.Instance.AskEnterBattle(nowTime, msg.battleid);
            }
        }

        public void OnReceiveBattleSeatPosInfo(GSToGC.BattleSeatPosInfo msg) {

        }

        public void OnReceiveBattleStateChange(GSToGC.BattleStateChange msg) {
            var curStateType = GameStateManager<EGameStateType>.Instance.CurrentState.Type;
            switch((EBattleState)msg.state){
                case EBattleState.SelectHero:
                    break;
                case EBattleState.SelectRune:
                    break;
                case EBattleState.Loading:
                    EventSystem.Broadcast(EGameEvent.LoadingGame, EGameStateType.Play);
                    break;
                case EBattleState.Playing:
                    break;
                case EBattleState.Finished:
                    break;
            }
        }

        public void OnReceiveHeroList(GSToGC.HeroList msg) {

        }

        public void AskMatchBattle(int mapId, EBattleMatchType type) {
            MessageCenter.Instance.AskMatchBattle(mapId, type);

            // todo
            MessageCenter.Instance.AskStartTeamMatch();

            Clock.ScheduleOnce(()=>{
                UDK.Output.Dump("vvvvvvvvvvvvvvv");
                MessageCenter.Instance.AskSelectHero(10002);
            }, 5);
        }

    }
}


