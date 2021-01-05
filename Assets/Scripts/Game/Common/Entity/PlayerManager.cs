using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDK;
using System;
using GameDefine;

namespace Game
{
    public class PlayerManager : EntityManager
    {
        public static new PlayerManager Instance = (PlayerManager)EntityManager.Instance;

        private Dictionary<UInt64, Player> mAllPlayers = new Dictionary<ulong, Player>();

        public Entity HandleCreateEntity(UInt64 guid, EEntityCampType campType) {
            Player player = null;
            return player;
        }

        public Dictionary<UInt64, Player> GetAllPlayers() {
            return mAllPlayers;
        }

        public void AddPlayer(UInt64 guid, Player player) {
            if(mAllPlayers.ContainsKey(guid)) {
                DebugEx.LogError("has the same guid : " + guid);
                return;
            }
            mAllPlayers.Add(guid, player);
        }
    }
}


