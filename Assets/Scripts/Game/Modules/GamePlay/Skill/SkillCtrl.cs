using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDK.MVC;
using UDK.Event;
using UDK.Network;
using System;
using UDK.FSM;
using GameDefine;

namespace Game
{
    public class SkillCtrl : BaseCtrl
    {
        public new GamePlayView View;
        public SkillCtrl()
        {
        }

        public override void Init()
        {
            View = base.View as GamePlayView;

            EventSystem.AddListener(EGameEvent.ShowGamePlayView, ShowView);
            EventSystem.AddListener(EGameEvent.HideGamePlayView, HideView);
            EventSystem.AddListener<GSToGC.ReleasingSkillState>(EGameEvent.OnReceiveGameObjectReleaseSkill, OnReceiveGameObjectReleaseSkill);
        }

        public override void Release()
        {
            EventSystem.RemoveListener(EGameEvent.ShowGamePlayView, ShowView);
            EventSystem.RemoveListener(EGameEvent.HideGamePlayView, HideView);
            EventSystem.RemoveListener<GSToGC.ReleasingSkillState>(EGameEvent.OnReceiveGameObjectReleaseSkill, OnReceiveGameObjectReleaseSkill);
        }

        public override void Update(float deltaTime)
        {
        }

        public void OnReceiveGameObjectReleaseSkill(GSToGC.ReleasingSkillState msg) {
            Vector3 pos = VectorUtil.ConvertPosToVector3(msg.pos);
            Vector3 dir = VectorUtil.ConvertDirToVector3(msg.dir);
            dir.y = 0.0f;
            UInt64 targetId = msg.targuid;
            UInt64 guid = msg.objguid;
            Entity target;
            EntityManager.Instance.GetAllEntities().TryGetValue(targetId, out target);
            Entity entity;
            // todo
            entity = PlayerManager.Instance.LocalPlayer;
            // if(EntityManager.Instance.GetAllEntities().TryGetValue(guid, out entity)) {
                pos.y = entity.RealObject.transform.position.y;
                entity.EntityFSMChangeDataOnPrepareSkill(pos, dir, msg.skillid, target);
                entity.OnFSMStateChange(EntityReleaseSkillFSM.Instance);
            // }
        }
    }
}


