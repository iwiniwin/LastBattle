using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDK.Resource;
using System.Xml;
using UDK;
using System;

namespace Game
{
    public class SkillAreaConfigReader
    {
        public static Dictionary<uint, SkillAreaConfig> Read(string xmlFilePath)
        {
            Dictionary<uint, SkillAreaConfig> dic = new Dictionary<uint, SkillAreaConfig>();
            ResourceUnit unit = ResourceManager.Instance.LoadImmediate(xmlFilePath, EResourceType.ASSET);
            TextAsset xmlFile = unit.Asset as TextAsset;
            if (!xmlFile)
            {
                DebugEx.LogError("no xml file");
            }
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlFile.text);
            XmlNodeList infoNodeList = doc.SelectSingleNode("SkillCfg_area").ChildNodes;
            for (int i = 0; i < infoNodeList.Count; i++)
            {
                if ((infoNodeList[i] as XmlElement).GetAttributeNode("un32ID") == null) continue;
                string typeName = (infoNodeList[i] as XmlElement).GetAttributeNode("un32ID").InnerText;
                SkillAreaConfig info = new SkillAreaConfig();
                info.id = Convert.ToUInt32(typeName);

                foreach (XmlElement xEle in infoNodeList[i].ChildNodes)
                {
                    switch (xEle.Name)
                    {
                        #region 搜索
                        case "szName:":
                            {
                                info.name = Convert.ToString(xEle.InnerText);
                            }
                            break;
                        case "eLifeTime":
                            {
                                info.lifeTime = Convert.ToInt32(xEle.InnerText);
                            }
                            break;
                        case "attackEffect":
                            {
                                info.effect = Convert.ToString(xEle.InnerText);
                            }
                            break;
                        case "FlySound":
                            {
                                info.sound = Convert.ToString(xEle.InnerText);
                            }
                            break;
                        case "eAoeType":
                            {
                                info.aoeType = Convert.ToInt32(xEle.InnerText);
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

    public class SkillAreaConfig
    {
        public uint id;//id
        public int aoeType;//
        public string name;//名字
        public string effect;//特效
        public string sound;//声音
        public float lifeTime;//生命周期

    }
}


