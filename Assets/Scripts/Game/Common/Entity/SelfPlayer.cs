using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameDefine;

namespace Game 
{
    public class SelfPlayer : Player
    {
        public SelfPlayer(UInt64 guid, EEntityCampType campType) : base(guid, campType) {
            
        }

        public void SendPreparePlaySkill(ESkillType type) {
            int skillId = GetSkillId(type);

            if(false) {  // 沉默了
                return;
            }
            if(skillId == 0) {  // 出现错误了
                return;
            }
            MessageCenter.Instance.AskUseSkill((uint)skillId);
        }

        private int GetSkillId(ESkillType type) {
            int baseId = 0;
            // if(Skillid)
            // return baseId;
            // todo
            return 1200301;
        }
    }
}


