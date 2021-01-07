using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDK;
using System;
using GameDefine;

namespace Game 
{
    public class EntityManager : Singleton<EntityManager>
    {
        private Dictionary<UInt64, Entity> mAllEntities = new Dictionary<ulong, Entity>();

        public enum CampTag {
            SelfCamp = 1,
            EnemyCamp = 0,
        }

        private List<Entity> homeBaseList = new List<Entity>();

        public Dictionary<UInt64, Entity> GetAllEntities() {
            return mAllEntities;
        }

        public GameObject CreateEntityModel(Entity entity, UInt64 guid, Vector3 dir, Vector3 pos) {
            if(entity == null) return null;
            int id = (int)entity.ObjTypeID;
            SetCommonProperty(entity, id);
            if(entity.ModelName == null || entity.ModelName == "") 
                return null;
            string path = GameConfig.MonsterModelsDir;

            // 创建GameObject
            string resPath = path + entity.ModelName;
            entity.RealObject = ObjectPoolManager.Instance.GetObject(resPath);
            if(entity.RealObject == null) {
                DebugEx.LogError("entity RealObject is null, resPath is " + resPath);
            }

            entity.ResPath = resPath;
            var transform = entity.RealObject.transform;
            transform.localPosition = pos;
            transform.localRotation = Quaternion.LookRotation(dir);

            if(entity is Player) {
                entity.EntityType = EEntityType.Player;
                if(entity is SelfPlayer) {
                    PlayerManager.Instance.LocalPlayer = (SelfPlayer)entity;
                }
            }else{
                entity.EntityType = EEntityType.Monster;  // todo 读取)ConfigReader.GetNpcInfo(id).NpcType;
                if(entity.EntityType == EEntityType.Monster && (int)entity.EntityCamp >= (int)EEntityCampType.A){
                    entity.EntityType = EEntityType.AltarSoldier;
                }
                entity.ColliderRadius = 0 / 100;  // todo read
            }   

            if(entity.NpcCateChild != ENpcCateChild.BuildShop) {
                entity.CreateHealthBar();
            }

            AddEntityComponent(entity);

            return entity.RealObject;
        }

        // 设置entity基本属性，读取配置表
        public virtual void SetCommonProperty(Entity entity, int id) {
            entity.NpcGUIDType = id;
        }   

        public void AddEntityComponent(Entity entity) {
            EntityComponent component = entity.RealObject.GetComponent<EntityComponent>();
            if(component == null) {
                component = entity.RealObject.AddComponent<EntityComponent>();
                // syncEntity.Sync
            }else{
                // Entity syncEntity = entity.RealObject.GetComponent<Entity>() as Entity;
            }
            component.SyncEntity = entity;
        }
    }
}


