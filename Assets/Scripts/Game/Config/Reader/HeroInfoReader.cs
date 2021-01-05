using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDK.Resource;
using UDK;
using System.Xml;
using System;
using GameDefine;

namespace Game
{
    public class HeroInfoReader
    {
        public static Dictionary<int, HeroConfigInfo> Read(string xmlFilePath)
        {
            Dictionary<int, HeroConfigInfo> dic = new Dictionary<int, HeroConfigInfo>();
            ResourceUnit unit = ResourceManager.Instance.LoadImmediate(xmlFilePath, EResourceType.ASSET);
            TextAsset xmlFile = unit.Asset as TextAsset;
            if (!xmlFile)
            {
                DebugEx.LogError("no xml file");
            }
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlFile.text);
            XmlNodeList infoNodeList = doc.SelectSingleNode("herocfg").ChildNodes;
            for (int i = 0; i < infoNodeList.Count; i++)
            {
                if ((infoNodeList[i] as XmlElement).GetAttributeNode("un32ID") == null) continue;
                string typeName = (infoNodeList[i] as XmlElement).GetAttributeNode("un32ID").InnerText;
                HeroConfigInfo info = new HeroConfigInfo();
                info.HeroName = Convert.ToString(typeName);

                foreach (XmlElement xEle in infoNodeList[i].ChildNodes)
                {
                    #region 搜索
                    switch (xEle.Name)
                    {
                        case "szNOStr":
                            info.HeroNum = Convert.ToInt32(xEle.InnerText);
                            break;
                        case "eMagicCate":
                            info.HeroMgicCate = Convert.ToInt32(xEle.InnerText);
                            break;
                        case "XuetiaoHeight":
                            info.HeroXueTiaoHeight = Convert.ToInt32(xEle.InnerText);
                            break;
                        case "n32AttackDist":
                            info.HeroAtkDis = Convert.ToSingle(xEle.InnerText);
                            break;
                        case "n32BaseExp":
                            info.HeroBaseExp = Convert.ToSingle(xEle.InnerText);
                            break;
                        case "n32BasePhyAttPower":
                            info.HeroPhyAtt = Convert.ToSingle(xEle.InnerText);
                            break;
                        case "n32BaseMagAttPower":
                            info.HeroMagAtt = Convert.ToSingle(xEle.InnerText);
                            break;
                        case "n32BasePhyDef":
                            info.HeroPhyDef = Convert.ToSingle(xEle.InnerText);
                            break;
                        case "n32BaseMagDef":
                            info.HeroMagDef = Convert.ToSingle(xEle.InnerText);
                            break;
                        case "n32BaseMoveSpeed":
                            info.HeroMoveSpeed = Convert.ToSingle(xEle.InnerText);
                            break;
                        case "n32BaseMoveSpeedScaling":
                            info.n32BaseMoveSpeedScaling = Convert.ToSingle(xEle.InnerText);
                            break;
                        case "n32BaseAttackCD":
                            info.HeorBaseAtkCd = Convert.ToSingle(xEle.InnerText);
                            break;
                        case "n32BaseMaxHP":
                            info.HeroMaxHp = Convert.ToSingle(xEle.InnerText);
                            break;
                        case "n32BaseMaxMP":
                            info.HeroMaxMp = Convert.ToSingle(xEle.InnerText);
                            break;
                        case "n32BaseHPRecover":
                            info.HeroHpRecover = Convert.ToSingle(xEle.InnerText);
                            break;
                        case "n32BaseMPRecover":
                            info.HeroMpRecover = Convert.ToSingle(xEle.InnerText);
                            break;
                        case "n32BaseReliveTime":
                            info.HeroRelieveTime = Convert.ToSingle(xEle.InnerText);
                            break;
                        case "n32ExpGrowth":
                            info.HeroExpGrowth = Convert.ToSingle(xEle.InnerText);
                            break;
                        case "n32PhyAttGrowth":
                            info.HeroPhyAttGrowth = Convert.ToSingle(xEle.InnerText);
                            break;
                        case "n32MagAttGrowth":
                            info.HeroMagAttGrowth = Convert.ToSingle(xEle.InnerText);
                            break;
                        case "n32PhyDefGrowth":
                            info.HeroPhyDefGrowth = Convert.ToSingle(xEle.InnerText);
                            break;
                        case "n32MagDefGrowth":
                            info.HeroMagDefGrowth = Convert.ToSingle(xEle.InnerText);
                            break;
                        case "n32AttackCDGrowth":
                            info.HeroAtkCdGrowth = Convert.ToSingle(xEle.InnerText);
                            break;
                        case "32MaxHPGrowth":
                            info.HeroMaxHpGrowth = Convert.ToSingle(xEle.InnerText);
                            break;
                        case "32MaxMPGrowth":
                            info.HeroMaxMpGrowth = Convert.ToSingle(xEle.InnerText);
                            break;
                        case "n32HPRecoverGrowth":
                            info.HeroHpRecoverGrowth = Convert.ToSingle(xEle.InnerText);
                            break;
                        case "n32MPRecoverGrowth":
                            info.HeroHpRecoverGrowth = Convert.ToSingle(xEle.InnerText);
                            break;
                        case "n32ReliveGrowth":
                            info.HeroHpRecoverGrowth = Convert.ToSingle(xEle.InnerText);
                            break;
                        case "n32CPRecover":
                            info.HeroCpRecover = Convert.ToSingle(xEle.InnerText);
                            break;
                        case "n32CollideRadious":
                            info.HeroCollideRadious = Convert.ToSingle(xEle.InnerText);
                            break;
                        case "un32DeathSould":
                            info.HeroDeathSould = Convert.ToString(xEle.InnerText);
                            break;
                        case "un32WalkSound":
                            info.un32WalkSound = xEle.InnerText;
                            break;
                        case "un32Script1":
                            info.HeroScript1 = Convert.ToString(xEle.InnerText);
                            break;
                        case "n32Script1Rate":
                            info.HeroScript1Rate = Convert.ToString(xEle.InnerText);
                            break;
                        case "un32SkillType1":
                            info.HeroSkillType1 = Convert.ToInt32(xEle.InnerText);
                            break;
                        case "un32SkillType2":
                            info.HeroSkillType2 = Convert.ToInt32(xEle.InnerText);
                            break;
                        case "un32SkillType3":
                            info.HeroSkillType3 = Convert.ToInt32(xEle.InnerText);
                            break;
                        case "un32SkillType4":
                            info.HeroSkillType4 = Convert.ToInt32(xEle.InnerText);
                            break;
                        case "un32SkillType5":
                            info.HeroSkillType5 = Convert.ToInt32(xEle.InnerText);
                            break;
                        case "un32SkillType6":
                            info.HeroSkillType6 = Convert.ToInt32(xEle.InnerText);
                            break;
                        case "n32LockRadious":
                            info.n32LockRadious = Convert.ToSingle(xEle.InnerText);
                            break;
                        case "n32RandomAttack":
                            info.n32RandomAttack = ResolveToStrList(xEle.InnerText);
                            break;
                        case "un32PreItem":
                            {
                                string str = Convert.ToString(xEle.InnerText);
                                if (str.CompareTo("0") != 0)
                                {
                                    info.HeroPreEquip = ResolveToIntList(str, ',');
                                }
                            }
                            break;
                        case "un32MidItem":
                            {
                                string str = Convert.ToString(xEle.InnerText);
                                if (str.CompareTo("0") != 0)
                                {
                                    info.HeroMidEquip = ResolveToIntList(str, ',');
                                }
                            }
                            break;
                        case "un32ProItem":
                            {
                                string str = Convert.ToString(xEle.InnerText);
                                if (str.CompareTo("0") != 0)
                                {
                                    info.HeroLatEquip = ResolveToIntList(str, ',');
                                }
                            }
                            break;
                        case "HeroKind":
                            {
                                string str = Convert.ToString(xEle.InnerText);
                                List<int> list = ResolveToIntList(str, ',');

                                for (int j = 0; j < list.Count; j++)
                                {
                                    info.heroKind.Add((EHeroType)list[j]);
                                }
                            }
                            break;
                        case "un32SkillTypeP":
                            info.HeroSkillTypeP = Convert.ToInt32(xEle.InnerText);
                            break;
                    }
                    #endregion
                }
                dic.Add(info.HeroNum, info);
            }
            return dic;
        }

        public static List<int> ResolveToIntList(string values, char sp = ',')
        {
            List<int> ItList = new List<int>();
            if (values == "0")
            {
                return ItList;
            }
            string[] value = values.Split(sp);
            foreach(string it in value)
            {
                ItList.Add(Convert.ToInt32(it));
            }
            return ItList;
        }

        public static List<string> ResolveToStrList(string values, char sp = ',')
        {
            string[] value = values.Split(sp);
            List<string> StrList = new List<string>();
            foreach (string it in value)
            {
                StrList.Add(it);
            }
            return StrList;
        }
    }

    public class HeroConfigInfo
    {
        public string HeroName;
        public int HeroNum;
        public int HeroMgicCate;
        public float HeroXueTiaoHeight;
        public float HeroAtkDis;
        public float HeroBaseExp;
        public float HeroPhyAtt;
        public float HeroMagAtt;
        public float HeroPhyDef;
        public float HeroMagDef;
        public float HeroMoveSpeed;
        public float n32BaseMoveSpeedScaling;
        public float HeorBaseAtkCd;
        public float HeroMaxHp;
        public float HeroMaxMp;
        public float HeroHpRecover;
        public float HeroMpRecover;
        public float HeroRelieveTime;
        public float HeroExpGrowth;
        public float HeroPhyAttGrowth;
        public float HeroMagAttGrowth;
        public float HeroPhyDefGrowth;
        public float HeroMagDefGrowth;
        public float HeroAtkCdGrowth;
        public float HeroMaxHpGrowth;
        public float HeroMaxMpGrowth;
        public float HeroHpRecoverGrowth;
        public float HeroMpRecoverGrowth;
        public float HeroReliveGrowth;
        public float HeroCpRecover;
        public float HeroCollideRadious;
        public float n32LockRadious;
        public string un32WalkSound;
        public string HeroDeathSould;
        public string HeroScript1;
        public string HeroScript1Rate;
        public int HeroSkillType1;
        public int HeroSkillType2;
        public int HeroSkillType3;
        public int HeroSkillType4;
        public int HeroSkillType5;
        public int HeroSkillType6;
        public int HeroSkillTypeP;
        public List<string> n32RandomAttack;
        public List<int> HeroPreEquip = new List<int>();
        public List<int> HeroMidEquip = new List<int>();
        public List<int> HeroLatEquip = new List<int>();
        public List<EHeroType> heroKind = new List<EHeroType>();
    }
}


