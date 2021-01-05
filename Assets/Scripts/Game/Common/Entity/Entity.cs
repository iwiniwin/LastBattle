using System.Collections;
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

        public IFSM<Entity> FSM {
            private set;
            get;
        } 

        public Vector3 FSMDirection {
            private set;
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
        }

        public void CreateHealthBar() {

        }

        public virtual void OnReborn(){
            DoReborn();
        }

        protected virtual void DoReborn(){

        }
    }
}


