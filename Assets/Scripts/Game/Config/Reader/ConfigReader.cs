/*
 * @Author: iwiniwin
 * @Date: 2021-01-05 22:48:01
 * @LastEditors: iwiniwin
 * @LastEditTime: 2021-01-27 22:59:11
 * @Description: 配置读取器
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game 
{
    public class ConfigReader
    {


        public static void Init() {
            var heroXmlInfoDic = HeroXmlInfoDic;
            var skillManagerInfoDic = SkillManagerInfoDic;
            var skillAreaInfoDic = SkillAreaInfoDic;
        }

        private static Dictionary<int, HeroConfigInfo> heroXmlInfoDic;
        public static Dictionary<int, HeroConfigInfo> HeroXmlInfoDic{
            get {
                if(heroXmlInfoDic == null) {
                    heroXmlInfoDic = HeroInfoReader.Read("Config/HeroCfg"); 
                }
                return heroXmlInfoDic;
            }
        }

        private static Dictionary<int, SkillManagerConfig> skillManagerInfoDic;
        public static Dictionary<int, SkillManagerConfig> SkillManagerInfoDic{
            get {
                if(skillManagerInfoDic == null) {
                    skillManagerInfoDic = SkillManagerConfigReader.Read("Config/SkillCfg_manager"); 
                }
                return skillManagerInfoDic;
            }
        }

        private static Dictionary<uint, SkillAreaConfig> skillAreaInfoDic;
        public static Dictionary<uint, SkillAreaConfig> SkillAreaInfoDic{
            get {
                if(skillAreaInfoDic == null) {
                    skillAreaInfoDic = SkillAreaConfigReader.Read("Config/SkillCfg_area"); 
                }
                return skillAreaInfoDic;
            }
        }


        public static HeroConfigInfo GetHeroConfigInfo(int id) {
            if(!heroXmlInfoDic.ContainsKey(id)){
                return null;
            }
            return heroXmlInfoDic[id];
        }

        public static SkillManagerConfig GetSkillManagerConfigInfo(int id) {
            if(!skillManagerInfoDic.ContainsKey(id)){
                return null;
            }
            return skillManagerInfoDic[id];
        }

        public static SkillAreaConfig GetSkillAreaConfigInfo(uint id) {
            if(!skillAreaInfoDic.ContainsKey(id)){
                return null;
            }
            return skillAreaInfoDic[id];
        }

    }
}


