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
        public void HandleMessage(Stream stream, int protocalId) {
            UDK.Output.Dump(protocalId, "receive msg");
            switch(protocalId) {
                case (Int32)LSToGC.MsgID.eMsgToGCFromLS_NotifyServerBSAddr :
                    OnNotifyServerAddress(stream);
                    break;
                case (Int32)BSToGC.MsgID.eMsgToGCFromBS_OneClinetLoginCheckRet:
                    OnCheckLoginBSRet(stream);
                    break;
                case (Int32)BSToGC.MsgID.eMsgToGCFromBS_AskGateAddressRet:
                    onNotifyGateServerInfo(stream);
                    break;
            }
        }

        /* 消息接收 */

        public void OnNotifyServerAddress(Stream stream) {
            LSToGC.ServerBSAddr msg = ProtoBuf.Serializer.Deserialize<LSToGC.ServerBSAddr>(stream);
            EventSystem.Broadcast(EGameEvent.OnReceiveBSAddress, msg);
        }

        public void OnCheckLoginBSRet(Stream stream) {
            BSToGC.ClinetLoginCheckRet msg = ProtoBuf.Serializer.Deserialize<BSToGC.ClinetLoginCheckRet>(stream);
            EventSystem.Broadcast(EGameEvent.OnReceiveCheckLoginBSRet, msg);
        }

        public void onNotifyGateServerInfo(Stream stream) {
            BSToGC.AskGateAddressRet msg = ProtoBuf.Serializer.Deserialize<BSToGC.AskGateAddressRet>(stream);
            EventSystem.Broadcast(EGameEvent.OnReceiveGSInfo, msg);
        }

        /* 消息发送 */
        
        public void AskLoginToLoginServer() {
            GCToLS.AskLogin msg = new GCToLS.AskLogin();
            msg.platform = (uint)GameServerData.Instance.ServerPlatform;
            msg.uin = GameServerData.Instance.ServerUin;
            msg.sessionid = GameServerData.Instance.ServerSessionId;
            NetworkManager.Instance.SendMsg(msg, (int)msg.msgid);
        }

        public void AskLoginToBalanceServer() {
            GCToBS.OneClinetLogin msg = new GCToBS.OneClinetLogin() {
                uin = GameServerData.Instance.ServerUin,
                plat = (uint)GameServerData.Instance.ServerPlatform,
                nsid = 0,
                login_success = 0,
                sessionid = GameServerData.Instance.ServerSessionId
            };
            NetworkManager.Instance.SendMsg(msg, (int)msg.msgnum);
        }
    }
}


