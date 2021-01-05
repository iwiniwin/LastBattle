﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDK.FSM;
using UDK.Event;
using UDK.Resource;
using GameDefine;
using UDK;
using UDK.UI;
using System;

namespace Game
{
    public class PlayState : IGameState<EGameStateType>
    {
        

        GameObject mUIRoot;

        public PlayState(){

        }

        public EGameStateType Type {
            get;
            set;
        } = EGameStateType.Play;

        public EGameStateType NextStateType {
            get;
            set;
        } = EGameStateType.Play;

        
        public void Enter(){
            EventSystem.AddListener<GSToGC.GOAppear>(EGameEvent.OnReceiveGameObjectAppear, OnReceiveGameObjectAppear);
            
            MessageCenter.Instance.AskSceneLoadComplete();
        }

        public void Exit(){
            EventSystem.RemoveListener<GSToGC.GOAppear>(EGameEvent.OnReceiveGameObjectAppear, OnReceiveGameObjectAppear);
        }

        public void FixedUpdate(float fixedDeltaTime){
        }

        public bool Update(float deltaTime){
            return NextStateType != EGameStateType.Play;
        }

        public void OnReceiveGameObjectAppear(GSToGC.GOAppear msg) {
            foreach(GSToGC.GOAppear.AppearInfo info in msg.info) {
                UInt64 masterGuid = info.masterguid;
                UInt64 objGuid = info.objguid;
                if(objGuid < 1) 
                    DebugEx.LogError("objguid : " + objGuid);
                Int32 camp = info.camp;
                Vector3 mvPos = ConvertPosToVector3(info.pos);
                Vector3 mvDir = ConvertDirToVector3(info.dir);
                mvDir.y = 0.0f;
                EEntityCampType type = GetEntityCampType(camp);
                GSToGC.ObjType objType = info.obj_type;

                if(EntityManager.Instance.GetAllEntities().ContainsKey(objGuid)) {
                    if(objType == GSToGC.ObjType.ObjType_Hero) {
                        // PlayerManager
                    }
                    continue;
                }

                Entity entity = null;

                if(IfTypeHero((EObjectType)info.obj_type_id)) {
                    Player player = null;
                    if(!PlayerManager.Instance.GetAllPlayers().TryGetValue(masterGuid, out player)) {
                        player = (Player)PlayerManager.Instance.HandleCreateEntity(masterGuid, type);
                        PlayerManager.Instance.AddPlayer(masterGuid, player);
                    }
                    player.EntityCamp = type;
                    player.ObjTypeID = info.obj_type_id;
                    entity = player;
                    PlayerManager.Instance.CreateEntityModel(entity, objGuid, mvDir, mvPos);
                }
            }
        }

        private bool IfTypeHero(EObjectType eOT)
        {
            if ((Int32)EObjectType.HeroBegin < (Int32)eOT && (Int32)eOT < (Int32)EObjectType.NPCBegin)
            {
                return true;
            }
            return false;
        }

        private EEntityCampType GetEntityCampType(int campId) {
            EEntityCampType type = (EEntityCampType)campId;
            if (campId > 0)
            {
                if (campId % 2 == 0)
                {
                    type = EEntityCampType.B;
                }
                else
                {
                    type = EEntityCampType.A;
                }
            }
            return type;
        }

        private Vector3 ConvertPosToVector3(GSToGC.Pos pos) {
            float mapHeight = 60.0f;
            if(pos != null) 
                return new Vector3((float)pos.x / 100.0f, mapHeight, (float)pos.z / 100.0f);
            else
                return Vector3.zero;
        }

        private Vector3 ConvertDirToVector3(GSToGC.Dir dir) {
            float angle = (float)(dir.angle) / 10000;
            return new Vector3((float)Math.Cos(angle), 0, (float)Math.Sin(angle));
        }
    }
}

