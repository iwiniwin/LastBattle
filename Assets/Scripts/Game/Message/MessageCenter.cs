/*
 * @Author: iwiniwin
 * @Date: 2020-12-06 16:41:13
 * @LastEditors: iwiniwin
 * @LastEditTime: 2020-12-10 00:13:22
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
                case (Int32)LSToGC.MsgID.eMsgToGCFromLS_NotifyServerBSAddr:
                    OnNotifyServerAddress(stream);
                    break;
                case (Int32)BSToGC.MsgID.eMsgToGCFromBS_OneClinetLoginCheckRet:
                    OnCheckLoginBSRet(stream);
                    break;
                case (Int32)BSToGC.MsgID.eMsgToGCFromBS_AskGateAddressRet:
                    onNotifyGateServerInfo(stream);
                    break;
                case (Int32)GSToGC.MsgID.eMsgToGCFromGS_NotifyUserBaseInfo:
                    onNotifyUserBaseInfo(stream);
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

        public void onNotifyGateServerInfo(Stream stream)
        {
            BSToGC.AskGateAddressRet msg = ProtoBuf.Serializer.Deserialize<BSToGC.AskGateAddressRet>(stream);
            EventSystem.Broadcast(EGameEvent.OnReceiveGSInfo, msg);
        }

        public void onNotifyUserBaseInfo(Stream stream)
        {
            GSToGC.UserBaseInfo msg;
            if (!ParseProto(out msg, stream)) return;
            EventSystem.Broadcast(EGameEvent.OnReceiveUserBaseInfo, msg);
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

        // 封装proto解析
        private bool ParseProto<T>(out T msg, Stream stream)
        {
            try
            {
                msg = ProtoBuf.Serializer.Deserialize<T>(stream);
                if(msg == null) {
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


