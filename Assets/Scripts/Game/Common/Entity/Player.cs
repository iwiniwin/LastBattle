using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameDefine;

namespace Game 
{
    public class Player : Entity
    {
        public UInt64 GameUserId;
        public string GameUserNick {
            get;
            set;
        }

        public PlayerBattleData BattleData {
            get;
            set;
        }

        public Player(UInt64 guid, EEntityCampType campType) : base(guid, campType){
            BattleData = new PlayerBattleData();
        }
    }

    public class PlayerBattleData {
        
    }
}


