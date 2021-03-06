﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UDK.FSM;
using GameDefine;

namespace Game
{
    public class Entity
    {
        public AudioSource absorbSound;

        public int resPath;

        public virtual void OnEnterDead(){

        }

        public virtual void OnExecuteDead(){

        }

        public virtual void OnExecuteMove() {

        }

        public virtual void OnExecuteFree() {

        }

        public virtual void OnUpdate() {
            if(FSM != null) {
                FSM.Execute(this);
            }
        }

        // 表示场景里的对象
        public UInt64 GameObjGuid {
            get;
            set;
        }

        public string ModelName {
            get;
            set;
        }

        public EEntityType EntityType {
            set;
            get;
        }

        public UInt32 ObjTypeID {
            get;
            set;
        }

        public EEntityCampType EntityCamp {
            get;
            set;
        }

        public int NpcGUIDType {
            get;
            set;
        }

        public ENpcCateChild NpcCateChild {
            get;
            set;
        }

        public float ColliderRadius {
            get;
            set;
        }

        public string ResPath {
            get;
            set;
        }

        public GameObject RealObject {
            set;
            get;
        }

        public Entity(UInt64 guid, EEntityCampType campType) {
            GameObjGuid = guid;
            EntityCamp = campType;
            GOSyncInfo = new GameObjectSyncInfo();
        }

        public void CreateHealthBar() {

        }

        public virtual void OnReborn(){
            DoReborn();
        }

        protected virtual void DoReborn(){

        }

        public GameObjectSyncInfo GOSyncInfo {
            get;
            private set;
        }

        public IFSM<Entity> FSM {
            private set;
            get;
        } 

        public Vector3 EntityFSMPosition
        {
            set;
            get;
        }

        public Vector3 EntityFSMDirection
        {
            private set;
            get;
        }

        public float EntityFSMMoveSpeed
        {
            private set;
            get;
        }

        public int EntitySkillID
        {
            set;
            get;
        }

        public Entity SkillTarget
        {
            get;
            set;
        }

        public virtual void OnEnterFree() {
            EntityComponent entityComponent = RealObject.GetComponent<EntityComponent>();
            if(entityComponent == null) return;
            Vector2 serverPos = new Vector2(GOSyncInfo.BeginPos.x, GOSyncInfo.BeginPos.z);
            Vector2 objPos = new Vector2(RealObject.transform.position.x, RealObject.transform.position.z);
            float distToServerPos = Vector2.Distance(serverPos, objPos);
            if(distToServerPos > 10) {
                RealObject.transform.position = GOSyncInfo.BeginPos;
                entityComponent.PlayerFreeAnimation();
                RealObject.transform.rotation = Quaternion.LookRotation(EntityFSMDirection);
            }
        }

        public virtual void OnEntityReleaseSkill() {
            SkillManagerConfig skillManagerConfig = ConfigReader.GetSkillManagerConfigInfo(EntitySkillID);
            if(skillManagerConfig == null) return;
            EntityComponent entityComponent = RealObject.GetComponent<EntityComponent>();
            if(skillManagerConfig.isAbsorb == 1) {  // 吸附技能

            }else {
                if(skillManagerConfig.isNormalAttack == 1) {  // 是否是普通攻击
                    entityComponent.PlayeAttackAnimation();
                }else{
                    entityComponent.PlayerAnimation(skillManagerConfig.rAnimation);
                }
            }
        }

        public void EntityFSMChangedata(Vector3 mvPos, Vector3 mvDir, float mvSpeed) {
            EntityFSMPosition = mvPos;
            EntityFSMDirection = mvDir;
            EntityFSMMoveSpeed = mvSpeed;
        }

        public void EntityFSMChangedata(Vector3 mvPos, Vector3 mvDir) {
            EntityFSMPosition = mvPos;
            EntityFSMDirection = mvDir;
        }

        protected int isAbsorb;  // 1是吸附，0不是
        public void EntityFSMChangeDataOnPrepareSkill(Vector3 mvPos, Vector3 mvDir, int skillID, Entity targetID, int isAbsorbSkill = 0) {
            EntityFSMPosition = mvPos;
            EntityFSMDirection = mvDir;
            EntitySkillID = skillID;
            SkillTarget = targetID;
            isAbsorb = isAbsorbSkill;
        }

        public void OnFSMStateChange(IFSM<Entity> fsm) {
            if(this.FSM != null && this.FSM.ChangeState(this, fsm)) {
                return;
            }

            if(this.FSM != null) {
                this.FSM.Exit(this);
            }

            this.FSM = fsm;

            if(this.FSM != null)
                this.FSM.Enter(this, 0.0f);
        }
    }

    public class GameObjectSyncInfo {
        public float Speed;
        public Vector3 BeginPos;
        public Vector3 SyncPos;
        public Vector3 Dir;
        public Vector3 LocalSyncDir;
        public float LastSyncSecond;
        public float BeginTime;
        public float DistMoved;
        public float ForceMoveSpeed;
    }
}


