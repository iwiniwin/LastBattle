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
            switch(protocalId) {
                case (Int32)LSToGC.MsgID.eMsgToGCFromLS_NotifyServerBSAddr :
                    OnNotifyServerAddress(stream);
                    break;
            }
        }

        public void OnNotifyServerAddress(Stream stream) {
            LSToGC.ServerBSAddr msg = ProtoBuf.Serializer.Deserialize<LSToGC.ServerBSAddr>(stream);
            EventSystem.Broadcast(EGameEvent.OnGetBSAddress, msg);
        }

        public void AskLoginToLoginServer() {
            GCToLS.AskLogin msg = new GCToLS.AskLogin();
            msg.platform = (uint)GameServerData.Instance.ServerPlatform;
            msg.uin = GameServerData.Instance.ServerUin;
            msg.sessionid = GameServerData.Instance.ServerSessionId;
            NetworkManager.Instance.SendMsg(msg, (int)msg.msgid);
        }
    }
}


