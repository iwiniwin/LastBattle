using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDK;
using System;

namespace Game 
{
    public class UserInfoModel : Singleton<UserInfoModel>
    {
        public UInt64 GameBattleID {get; set;}

        public UInt32 GameMapID {get; set;}

        public UInt64 GameUserGuid {get; private set;}

        public bool IsReconnect {get; set;}

        public bool IsLocalPlayer(UInt64 guid) {
            return guid == GameUserGuid;
        }

        public void SetGameBaseInfo(GSToGC.UserBaseInfo msg) {
            
        }
    }
}


