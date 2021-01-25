using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameDefine;
using UDK.FSM;

namespace Game
{
    public class Player : Entity
    {
        public UInt64 GameUserId;
        public string GameUserNick
        {
            get;
            set;
        }

        public PlayerBattleData BattleData
        {
            get;
            set;
        }

        public Player(UInt64 guid, EEntityCampType campType) : base(guid, campType)
        {
            BattleData = new PlayerBattleData();
        }

        public override void OnExecuteMove()
        {
            EntityComponent entityComponent = RealObject.GetComponent<EntityComponent>();
            Quaternion destQuaternion = Quaternion.LookRotation(EntityFSMDirection);
            Quaternion midQuater = Quaternion.Lerp(entityComponent.transform.rotation, destQuaternion, 10 * Time.deltaTime);
            entityComponent.transform.rotation = midQuater;

            float timeSpan = Time.realtimeSinceStartup - GOSyncInfo.BeginTime;
            float moveDist = timeSpan * GOSyncInfo.Speed;
            GOSyncInfo.SyncPos = GOSyncInfo.BeginPos + GOSyncInfo.Dir * moveDist;
            entityComponent.PlayerRunAnimation();

            Vector3 serverPos2d = new Vector3(GOSyncInfo.SyncPos.x, 60, GOSyncInfo.SyncPos.z);
            Vector3 entityPos2d = new Vector3(entityComponent.transform.position.x, 60, entityComponent.transform.position.z);

            Vector3 syncDir = serverPos2d - entityPos2d;
            syncDir.Normalize();
            float distToServerPos = Vector3.Distance(serverPos2d, entityPos2d);
            if (distToServerPos > 5)
            {
                entityComponent.transform.position = GOSyncInfo.SyncPos;
                OnCameraUpdatePosition();
                return;
            }
            Vector3 pos = entityPos2d + syncDir * EntityFSMMoveSpeed * Time.deltaTime;
            CharacterController controller = RealObject.GetComponent<CharacterController>();
            controller.Move(syncDir * EntityFSMMoveSpeed * Time.deltaTime);

            entityComponent.transform.position = new Vector3(entityComponent.transform.position.x, 60.0f, entityComponent.transform.position.z);
            OnCameraUpdatePosition();
        }

        public override void OnExecuteFree() {
            EntityComponent entityComponent = RealObject.GetComponent<EntityComponent>();
            Vector3 synPos = new Vector3(GOSyncInfo.SyncPos.x, 60, GOSyncInfo.SyncPos.z);
            Vector3 realPos = new Vector3(RealObject.transform.position.x, 60, RealObject.transform.position.z);
            float distToServerPos = Vector3.Distance(synPos, realPos);
            if(distToServerPos < 0.5f) {

            }else if(distToServerPos > 5) {
                RealObject.transform.position = GOSyncInfo.BeginPos;
            }else {
                Vector3 syncDir = synPos - realPos;
                syncDir.Normalize();
                entityComponent.GetComponent<CharacterController>().Move(syncDir * 2 * Time.deltaTime);
            }
            entityComponent.PlayerFreeAnimation();
            OnCameraUpdatePosition();
            TurnAngle();
        }

        /// <summary>
        /// 转动朝向
        /// </summary>
        private void TurnAngle() {
            float angle = Vector3.Angle(RealObject.transform.forward, EntityFSMDirection);
            if(angle > 2) {
                Quaternion dest = Quaternion.LookRotation(EntityFSMDirection);
                Quaternion mid = Quaternion.Lerp(RealObject.transform.rotation, dest, 5 * Time.deltaTime);
                RealObject.transform.rotation = mid;
            }
        }

        public void OnCameraUpdatePosition()
        {
            SmoothFollow followComponent = Camera.main.GetComponent<SmoothFollow>();
            PlayState playState = GameStateManager<EGameStateType>.Instance.CurrentState as PlayState;
            if (playState == null) return;
            if (true)
            {  // 倾斜45度
                Vector3 euler = Camera.main.transform.eulerAngles;
                EntityComponent entityComponent = RealObject.GetComponent<EntityComponent>();
                if (entityComponent.CampType == EEntityCampType.A)
                {
                    Camera.main.transform.eulerAngles = new Vector3(euler.x, 45.0f, 0);
                }
                else if (entityComponent.CampType == EEntityCampType.B)
                {
                    Camera.main.transform.eulerAngles = new Vector3(euler.x, -135.0f, 0);
                }
                if(followComponent.Target == null)
                    followComponent.Target = entityComponent.transform;
                followComponent.FixedUpdatePosition();
            }
        }
    }

    public class PlayerBattleData
    {

    }
}


