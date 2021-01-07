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
        private static PlayerManager _instance;
        public static new PlayerManager Instance {
            get {
                if(_instance == null) {
                    _instance = new PlayerManager();
                }
                return _instance;
            }
        }

        private Dictionary<UInt64, Player> mAllPlayers = new Dictionary<ulong, Player>();

        public SelfPlayer LocalPlayer {
            get;
            set;
        }

        public Entity HandleCreateEntity(UInt64 guid, EEntityCampType campType) {
            Player player = null;
            UDK.Output.Dump(UserInfoModel.Instance.IsLocalPlayer(guid), "88888");
            if(true || UserInfoModel.Instance.IsLocalPlayer(guid)) {
                player = new SelfPlayer(guid, campType);
            }else{
                player = new Player(guid, campType);
            }
            player.GameUserId = guid;
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

        public override void SetCommonProperty(Entity entity, int id)
        {
            base.SetCommonProperty(entity, id);
            HeroConfigInfo info = ConfigReader.GetHeroConfigInfo(id);
            entity.ModelName = info.HeroName;
            
            entity.ColliderRadius = info.HeroCollideRadious / 100;
            Player player = (Player)entity;
            if(player.GameUserNick == "" || player.GameUserNick == null) {
                player.GameUserNick = "rnadomname";  // todo
            }
        }
    }
}


