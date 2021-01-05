using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDK;
using System;

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
            entity.NpcGUIDType = id;
        }
    }
}


