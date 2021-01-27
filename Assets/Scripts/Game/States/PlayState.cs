using System.Collections;
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
            EventSystem.Broadcast(EGameEvent.ShowGamePlayView);

            EventSystem.AddListener<GSToGC.RunningState>(EGameEvent.OnReceiveGameObjectRunState, OnReceiveGameObjectRunState);
            EventSystem.AddListener<GSToGC.FreeState>(EGameEvent.OnReceiveGameObjectFreeState, OnReceiveGameObjectFreeState);
        }

        public void Exit(){
            EventSystem.Broadcast(EGameEvent.HideGamePlayView);
            EventSystem.RemoveListener<GSToGC.GOAppear>(EGameEvent.OnReceiveGameObjectAppear, OnReceiveGameObjectAppear);

            EventSystem.RemoveListener<GSToGC.RunningState>(EGameEvent.OnReceiveGameObjectRunState, OnReceiveGameObjectRunState);
            EventSystem.RemoveListener<GSToGC.FreeState>(EGameEvent.OnReceiveGameObjectFreeState, OnReceiveGameObjectFreeState);
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
                Vector3 mvPos = VectorUtil.ConvertPosToVector3(info.pos);
                Vector3 mvDir = VectorUtil.ConvertDirToVector3(info.dir);
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
                    CreateCharacterController(entity);
                }
                if(entity != null) {
                    entity.GameObjGuid = objGuid;
                    EntityManager.Instance.AddEntity(objGuid, entity);
                    entity.GOSyncInfo.BeginPos = mvPos;
                    entity.GOSyncInfo.SyncPos = mvPos;
                    entity.EntityFSMChangedata(mvPos, mvDir);
                    entity.OnFSMStateChange(EntityFreeFSM.Instance);
                }
            }
        }

        private void CreateCharacterController(Entity entity) {
            CharacterController controller = entity.RealObject.GetComponent<CharacterController>();
            if(controller == null) {
                controller = entity.RealObject.AddComponent<CharacterController>();
            }
            controller.center = new Vector3(0f, 1f, 0f);
            controller.radius = 0.01f;
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

        // 对象移动
        public void OnReceiveGameObjectRunState(GSToGC.RunningState msg) {
            
            if(msg.dir == null || msg.pos == null) return;
            UInt64 guid = msg.objguid;
            Vector3 mvPos = VectorUtil.ConvertPosToVector3(msg.pos);
            Vector3 mvDir = VectorUtil.ConvertDirToVector3(msg.dir);
            float mvSpeed = msg.movespeed / 100.0f;
            Entity entity = null;

            //todo
            entity = PlayerManager.Instance.LocalPlayer;

            // if(EntityManager.Instance.GetAllEntities().TryGetValue(guid, out entity)) {

                mvPos.y = entity.RealObject.transform.position.y;
                entity.GOSyncInfo.BeginPos = mvPos;
                entity.GOSyncInfo.SyncPos = mvPos;
                entity.GOSyncInfo.Dir = mvDir;
                entity.GOSyncInfo.Speed = mvSpeed;
                entity.GOSyncInfo.BeginTime = Time.realtimeSinceStartup;
                entity.GOSyncInfo.LastSyncSecond = Time.realtimeSinceStartup;
                entity.EntityFSMChangedata(mvPos, mvDir, mvSpeed);
                entity.OnFSMStateChange(EntityRunFSM.Instance);
            // }
        }

        public void OnReceiveGameObjectFreeState(GSToGC.FreeState msg) {
            if(msg.dir == null || msg.pos == null) return;
            UInt64 guid = msg.objguid;
            Vector3 mvPos = VectorUtil.ConvertPosToVector3(msg.pos);
            Vector3 mvDir = VectorUtil.ConvertDirToVector3(msg.dir);
            Entity entity = null;
            if(EntityManager.Instance.GetAllEntities().TryGetValue(guid, out entity)) {
                Vector3 lastSyncPos = entity.GOSyncInfo.SyncPos;
                mvPos.y = entity.RealObject.transform.position.y;
                entity.GOSyncInfo.BeginPos = mvPos;
                entity.GOSyncInfo.SyncPos = mvPos;
                entity.GOSyncInfo.Dir = mvDir;
                entity.GOSyncInfo.BeginTime = Time.realtimeSinceStartup;
                entity.GOSyncInfo.LastSyncSecond = Time.realtimeSinceStartup;
                entity.EntityFSMChangedata(mvPos, mvDir);
                entity.OnFSMStateChange(EntityFreeFSM.Instance);
            }

        }
    }
}


