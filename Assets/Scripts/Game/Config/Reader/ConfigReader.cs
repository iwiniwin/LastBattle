/*
 * @Author: iwiniwin
 * @Date: 2021-01-05 22:48:01
 * @LastEditors: iwiniwin
 * @LastEditTime: 2021-01-05 23:13:04
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

        public static HeroConfigInfo GetHeroConfigInfo(int id) {
            if(!heroXmlInfoDic.ContainsKey(id)){
                return null;
            }
            return heroXmlInfoDic[id];
        }

    }
}


