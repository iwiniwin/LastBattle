using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UDK;

namespace Game 
{
    public class Effect 
    {
        public enum EEffectType {
            Passive,
            Buff,
            BeAttack,
            FlyEffect,
            Normal,
            Area,
            Link
        }

        public Int64 ID;  // 特效ID
        public string resPath;  // 特效资源路径
        public float lifeTime;   // 特效生命周期
        public EEffectType Type;
        public GameObject obj = null;  // 特效GameObject物体
        public AudioSource mAudioSource = null;  // 声音

        public uint skillId;  // 特效对应的技能ID

        public string templateName;  // 特效模板名称

        public Vector3 dir;
        public Vector3 fixPosition;

        // 特效创建接口
        public void Create() {
            templateName = FileUtil.GetResourceName(resPath);
            obj = ObjectPoolManager.Instance.GetObject(resPath);
            if(obj == null) {
                DebugEx.LogError("load effect object failed : " + resPath);
                return;
            }
            obj.name = templateName + "_" + ID.ToString();
            OnLoadComplete();
            lifeTime = 2.0f;  // todo

            EEffectLevel level = EEffectLevel.High;
            EEffectLevel curLevel = EffectManager.Instance.LoadLevel;
            if(level != curLevel) {
                // todo 调整特效等级
            }
        }

        public virtual void OnLoadComplete() {
            
        }

    }
}


