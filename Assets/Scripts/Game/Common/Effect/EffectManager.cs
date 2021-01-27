using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDK;
using System;

namespace Game 
{
    public enum EEffectLevel {
        High,
        Low,
    }

    public class EffectManager : Singleton<EffectManager>
    {
        // 特效加载等级
        public EEffectLevel LoadLevel = EEffectLevel.High;

        public Dictionary<Int64, Effect> mEffectDic = new Dictionary<long, Effect>();

        public AreaEffect CreateAreaEffect(UInt64 owner, uint skillId, UInt32 id, Vector3 skillDir, Vector3 skillPos) {
            SkillAreaConfig skillConfig = ConfigReader.GetSkillAreaConfigInfo(skillId);
            if(skillConfig == null || skillConfig.effect == "0") {
                return null;
            }
            string resourcePath = GameConfig.SkillEffectPath + skillConfig.effect;
            AreaEffect effect = new AreaEffect();
            effect.skillId = skillId;
            effect.dir = skillDir;
            effect.fixPosition = skillPos;
            effect.ID = id;
            effect.resPath = resourcePath;
            effect.Create();
            // 播放声音
            string soundPath = GameConfig.SoundPath + skillConfig.sound;
            effect.mAudioSource = AudioUtil.PlaySound(soundPath);
            AddEffect(effect.ID, effect);
            return effect;
        } 

        public void AddEffect(Int64 id, Effect effect) {
            if(!mEffectDic.ContainsKey(id)) {
                mEffectDic.Add(id, effect);
            }else{
                DebugEx.LogError("effect " + id + " already exist");
            }
        }
    }
}


