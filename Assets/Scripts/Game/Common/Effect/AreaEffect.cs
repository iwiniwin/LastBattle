using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game 
{
    public class AreaEffect : Effect
    {
        public AreaEffect() {
            Type = EEffectType.Area;
        }

        public override void OnLoadComplete()
        {
            SkillAreaConfig skillConfig = ConfigReader.GetSkillAreaConfigInfo(skillId);
            Quaternion rt = Quaternion.LookRotation(dir);
            obj.transform.rotation = rt;
            obj.transform.position = fixPosition;
        }
    }
}


