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

        public bool IsReconnect {get; set;}
    }
}


