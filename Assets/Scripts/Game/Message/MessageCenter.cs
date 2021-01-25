/*
 * @Author: iwiniwin
 * @Date: 2020-12-06 16:41:13
 * @LastEditors: iwiniwin
 * @LastEditTime: 2021-01-25 22:14:48
 * @Description: 消息中心，负责消息接收与发送
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDK.Network;
using System;
using System.IO;
using UDK.Event;

namespace Game
{
    public class MessageCenter : UDK.Singleton<MessageCenter>
    {
        public void HandleMessage(Stream stream, int protocalId)
        {
            UDK.Output.Dump(protocalId, "receive msg");
            switch (protocalId)
            {
                case (Int32)LSToGC.MsgID.eMsgToGCFromLS_NotifyServerBSAddr:  // 收到BS地址
                    OnNotifyServerAddress(stream);
                    break;
                case (Int32)BSToGC.MsgID.eMsgToGCFromBS_OneClinetLoginCheckRet:  // 客户端登录校验结果
                    OnCheckLoginBSRet(stream);
                    break;
                case (Int32)BSToGC.MsgID.eMsgToGCFromBS_AskGateAddressRet:  // 请求GS地址结果
                    OnNotifyGateServerInfo(stream);
                    break;
                case (Int32)GSToGC.MsgID.eMsgToGCFromGS_NotifyUserBaseInfo:  // 收到用户信息
                    OnNotifyUserBaseInfo(stream);
                    break;
                case (Int32)GSToGC.MsgID.eMsgToGCFromGS_NotifyBattleBaseInfo:  // 收到战斗基本信息
                    OnNotifyBattleBaseInfo(stream);
                    break;
                case (Int32)GSToGC.MsgID.eMsgToGCFromGS_NotifyBattleSeatPosInfo:
                    OnNotifyBattleSeatPosInfo(stream);
                    break;
                case (Int32)GSToGC.MsgID.eMsgToGCFromGS_NotifyBattleStateChange:
                    OnNetNotifyBattleStateChange(stream);
                    break;
                case (Int32)GSToGC.MsgID.eMsgToGCFromGS_NotifyHeroList:
                    OnNotifyHeroList(stream);
                    break;
                case (Int32)GSToGC.MsgID.eMsgToGCFromGS_NotifyGameObjectAppear:
                    OnNotifyGameObjectAppear(stream);
                    break;
                case (Int32)GSToGC.MsgID.eMsgToGCFromGS_NotifyGameObjectRunState:
                    OnNotifyGameObjectRunState(stream);
                    break;
                case (Int32)GSToGC.MsgID.eMsgToGCFromGS_NotifyGameObjectFreeState:
                    OnNotifyGameObjectFreeState(stream);
                    break;
            }
        }

        /* 消息接收 */

        public void OnNotifyServerAddress(Stream stream)
        {
            LSToGC.ServerBSAddr msg = ProtoBuf.Serializer.Deserialize<LSToGC.ServerBSAddr>(stream);
            EventSystem.Broadcast(EGameEvent.OnReceiveBSAddress, msg);
        }

        public void OnCheckLoginBSRet(Stream stream)
        {
            BSToGC.ClinetLoginCheckRet msg = ProtoBuf.Serializer.Deserialize<BSToGC.ClinetLoginCheckRet>(stream);
            EventSystem.Broadcast(EGameEvent.OnReceiveCheckLoginBSRet, msg);
        }

        public void OnNotifyGateServerInfo(Stream stream)
        {
            BSToGC.AskGateAddressRet msg = ProtoBuf.Serializer.Deserialize<BSToGC.AskGateAddressRet>(stream);
            EventSystem.Broadcast(EGameEvent.OnReceiveGSInfo, msg);
        }

        public void OnNotifyUserBaseInfo(Stream stream)
        {
            GSToGC.UserBaseInfo msg;
            if (!ParseProto(out msg, stream)) return;
            EventSystem.Broadcast(EGameEvent.OnReceiveUserBaseInfo, msg);
        }

        public void OnNotifyBattleBaseInfo(Stream stream)
        {
            GSToGC.BattleBaseInfo msg;
            if(!ParseProto(out msg, stream)) return;
            EventSystem.Broadcast(EGameEvent.OnReceiveBattleBaseInfo, msg);
        }

        public void OnNotifyBattleSeatPosInfo(Stream stream) {
            GSToGC.BattleSeatPosInfo msg;
            if(!ParseProto(out msg, stream)) return;
            EventSystem.Broadcast(EGameEvent.OnReceiveBattleSeatPosInfo, msg);
        }
        
        public void OnNetNotifyBattleStateChange(Stream stream) {
            GSToGC.BattleStateChange msg;
            if(!ParseProto(out msg, stream)) return;
            EventSystem.Broadcast(EGameEvent.OnReceiveBattleStateChange, msg);
        }

        public void OnNotifyHeroList(Stream stream) {
            GSToGC.HeroList msg;
            if(!ParseProto(out msg, stream)) return;
            EventSystem.Broadcast(EGameEvent.OnReceiveHeroList, msg);
        }

        public void OnNotifyGameObjectAppear(Stream stream) {
            GSToGC.GOAppear msg;
            if(!ParseProto(out msg, stream)) return;
            EventSystem.Broadcast(EGameEvent.OnReceiveGameObjectAppear, msg);
        }

        public void OnNotifyGameObjectRunState(Stream stream) {
            GSToGC.RunningState msg;
            if(!ParseProto(out msg, stream)) return;
            EventSystem.Broadcast(EGameEvent.OnReceiveGameObjectRunState, msg);
        }

        public void OnNotifyGameObjectFreeState(Stream stream) {
            GSToGC.FreeState msg;
            if(!ParseProto(out msg, stream)) return;
            EventSystem.Broadcast(EGameEvent.OnReceiveGameObjectFreeState, msg);
        }

        /* 消息发送 */

        public void AskLoginToLoginServer()
        {
            GCToLS.AskLogin msg = new GCToLS.AskLogin();
            msg.platform = (uint)GameServerData.Instance.ServerPlatform;
            msg.uin = GameServerData.Instance.ServerUin;
            msg.sessionid = GameServerData.Instance.ServerSessionId;
            NetworkManager.Instance.SendMsg(msg, (int)msg.msgid);
        }

        public void AskLoginToBalanceServer()
        {
            GCToBS.OneClinetLogin msg = new GCToBS.OneClinetLogin()
            {
                uin = GameServerData.Instance.ServerUin,
                plat = (uint)GameServerData.Instance.ServerPlatform,
                nsid = 0,
                login_success = 0,
                sessionid = GameServerData.Instance.ServerSessionId
            };
            NetworkManager.Instance.SendMsg(msg, (int)msg.msgnum);
        }

        public void AskLoginToGateServer()
        {
            GCToCS.Login msg = new GCToCS.Login
            {
                sdk = (Int32)SdkManager.Instance.PlatformType,
                platform = 0,
                equimentid = "",  // todo
                name = GameServerData.Instance.GateServerUin,
                passwd = GameServerData.Instance.ServerToken,
            };
            NetworkManager.Instance.SendMsg(msg, (int)msg.msgnum);
        }

        public void SendCompleteBaseInfo(byte[] nickName, int headId, byte sex)
        {
            GCToCS.CompleteInfo msg = new GCToCS.CompleteInfo
            {
                nickname = System.Text.Encoding.UTF8.GetString(nickName),
                headid = headId,
                sex = sex,
            };
            NetworkManager.Instance.SendMsg(msg, (int)msg.msgnum);
        }

        public void AskCSToCreateGuideBattle(int mapId, GCToCS.AskCSCreateGuideBattle.guidetype type)
        {
            GCToCS.AskCSCreateGuideBattle msg = new GCToCS.AskCSCreateGuideBattle()
            {
                mapid = mapId,
                ntype = type
            };
            NetworkManager.Instance.SendMsg(msg, (int)msg.msgnum);
        }

        public void AskEnterBattle(Int64 time, UInt64 battleId) {
            GCToSS.AskEnterBattle msg = new GCToSS.AskEnterBattle() {
                battleid = battleId,
                clientTime = time,  
            };
            NetworkManager.Instance.SendMsg(msg, (int)msg.msgnum);
        }

        public void AskSceneLoadComplete() {
            GCToSS.LoadComplete msg = new GCToSS.LoadComplete();
            NetworkManager.Instance.SendMsg(msg, (int)msg.msgnum);
        }

        public void AskMoveDir(Vector3 forward) {
            GCToSS.Dir dir = new GCToSS.Dir() {
                x = forward.x,
                z = forward.z,
                angle = (float)Math.Atan2(forward.z, forward.x)
            };
            GCToSS.MoveDir msg = new GCToSS.MoveDir()
            {
                dir = dir
            };
            NetworkManager.Instance.SendMsg(msg, (int)msg.msgnum);
        }

        public void AskStopMove() {
            GCToSS.StopMove msg = new GCToSS.StopMove();
            NetworkManager.Instance.SendMsg(msg, (int)msg.msgnum);
        }

        // 封装proto解析
        private bool ParseProto<T>(out T msg, Stream stream)
        {
            try
            {
                msg = ProtoBuf.Serializer.Deserialize<T>(stream);
                if (msg == null)
                {
                    UDK.DebugEx.LogError("proto parse error");
                    msg = default(T);
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                UDK.DebugEx.LogError("proto parse exception");
                msg = default(T);
                return false;
            }
        }
    }
}


