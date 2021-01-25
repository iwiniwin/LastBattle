using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDK.Resource;
using UDK;
using System.Xml;
using System;

namespace Game
{
    public class SkillManagerConfigReader
    {
        public static Dictionary<int, SkillManagerConfig> Read(string xmlFilePath)
        {
            Dictionary<int, SkillManagerConfig> dic = new Dictionary<int, SkillManagerConfig>();
            ResourceUnit unit = ResourceManager.Instance.LoadImmediate(xmlFilePath, EResourceType.ASSET);
            TextAsset xmlFile = unit.Asset as TextAsset;
            if (!xmlFile)
            {
                DebugEx.LogError("no xml file");
            }
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlFile.text);
            XmlNodeList infoNodeList = doc.SelectSingleNode("SkillCfg_manager").ChildNodes;
            for (int i = 0; i < infoNodeList.Count; i++)
            {
                if ((infoNodeList[i] as XmlElement).GetAttributeNode("un32ID") == null) continue;
                string typeName = (infoNodeList[i] as XmlElement).GetAttributeNode("un32ID").InnerText;
                SkillManagerConfig info = new SkillManagerConfig();
                info.id = Convert.ToInt32(typeName);

                foreach (XmlElement xEle in infoNodeList[i].ChildNodes)
                {
                    #region 搜索
                    switch (xEle.Name)
                    {
                        case "szName":
                            {
                                info.name = Convert.ToString(xEle.InnerText);
                            }
                            break;
                        case "bIfNormalAttack":
                            {
                                info.isNormalAttack = Convert.ToInt32(xEle.InnerText);
                            }
                            break;
                        case "n32PrepareAction":
                            {
                                info.yAnimation = Convert.ToString(xEle.InnerText);
                            }
                            break;
                        case "n32PrepareTime":
                            {
                                info.yTime = Convert.ToInt32(xEle.InnerText) / 1000.0f;
                            }
                            break;
                        case "n32PrepareEffect":
                            {
                                info.yEffect = Convert.ToString(xEle.InnerText);
                            }
                            break;
                        case "n32ReleaseSound":
                            {
                                info.rSound = Convert.ToString(xEle.InnerText);
                            }
                            break;
                        case "n32ReleaseAction":
                            {
                                info.rAnimation = Convert.ToString(xEle.InnerText);
                            }
                            break;
                        case "n32ReleaseEffect":
                            {
                                info.rEffect = Convert.ToString(xEle.InnerText);
                            }
                            break;
                        case "n32PrepareSound":
                            {
                                info.ySound = Convert.ToString(xEle.InnerText);
                            }
                            break;
                        case "eUseWay":
                            {
                                info.useWay = Convert.ToInt32(xEle.InnerText);
                            }
                            break;
                        case "eTargetType":
                            {
                                info.targetType = Convert.ToInt32(xEle.InnerText);
                            }
                            break;
                        case "SkillIcon":
                            {
                                info.skillIcon = Convert.ToString(xEle.InnerText);
                            }
                            break;
                        case "info":
                            {
                                info.info = Convert.ToString(xEle.InnerText);
                            }
                            break;
                        case "n32CoolDown":
                            {
                                info.coolDown = Convert.ToInt32(xEle.InnerText);
                            }
                            break;
                        case "n32ReleaseDistance":
                            {
                                info.range = Convert.ToInt32(xEle.InnerText) / 100.0f;
                            }
                            break;
                        case "eSummonEffect":
                            {
                                info.absorbRes = Convert.ToString(xEle.InnerText);
                            }
                            break;
                        case "bIsConsumeSkill":
                            {
                                info.isAbsorb = Convert.ToInt32(xEle.InnerText);
                            }
                            break;
                        case "n32UpgradeLevel":
                            {
                                info.n32UpgradeLevel = Convert.ToInt32(xEle.InnerText);
                            }
                            break;
                        case "n32UseMP":
                            {
                                info.mpUse = Convert.ToSingle(xEle.InnerText);
                            }
                            break;
                        case "n32UseHP":
                            {
                                info.hpUse = Convert.ToSingle(xEle.InnerText);
                            }
                            break;
                        case "n32UseCP":
                            {
                                info.cpUse = Convert.ToSingle(xEle.InnerText);
                            }
                            break;
                    #endregion
                    }
                }
                dic.Add(info.id, info);
            }
            return dic;
        }
    }

    public class SkillManagerConfig
    {
        public int id;//id
        public string name;//名字
                           //public int  releaseWay;//释放方法
        public int isNormalAttack;//是否普通攻击
        public string yAnimation;//吟唱动画
        public string ySound;//吟唱声音
        public string yEffect;//吟唱效果
        public string rAnimation;//释放动画
        public string rSound;//释放声音
        public string rEffect;//吟唱效果
        public int useWay;//使用方法
        public int targetType;//目标类型
        public string skillIcon;//
        public string info;//
        public int coolDown;//
        public float range;
        public string absorbRes;
        public int isAbsorb;
        public int n32UpgradeLevel;
        public float yTime;//吟唱时间 
        public float mpUse;
        public float hpUse;
        public float cpUse;
    }
}


