/*
 * @Author: iwiniwin
 * @Date: 2021-01-02 19:09:22
 * @LastEditors: iwiniwin
 * @LastEditTime: 2021-01-02 19:37:50
 * @Description: 地图加载器
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDK;
using UDK.Resource;
using System.Xml;
using System;

namespace Game
{
    public class MapLoader : Singleton<MapLoader>
    {
        private XmlDocument mXmlDoc = null;
        private Dictionary<uint, MapInfo> mMapDic = new Dictionary<uint, MapInfo>();

        public MapLoader()
        {
            ReadConfig(GameConfig.MapConfigXMlPath);
        }

        public MapInfo GetMapInfo(uint id) {
            if(mMapDic.ContainsKey(id))
                return mMapDic[id];
            return null;
        }

        public MapInfo GetMapInfo(string name) {
            foreach(MapInfo info in mMapDic.Values) {
                if(info.Scene == name) {
                    return info;
                }
            }
            return null;
        }

        public void ReadConfig(string xmlFilePath)
        {
            ResourceUnit xmlFileUnit = ResourceManager.Instance.LoadImmediate(xmlFilePath, EResourceType.ASSET);
            TextAsset xmlFile = xmlFileUnit.Asset as TextAsset;
            if (!xmlFile)
                DebugEx.LogError("unable to load " + xmlFilePath);

            mXmlDoc = new XmlDocument();
            mXmlDoc.LoadXml(xmlFile.text);

            XmlNodeList infoNodeList = mXmlDoc.SelectSingleNode("MapLoadCfg").ChildNodes;
            for (int i = 0; i < infoNodeList.Count; i++)
            {
                var node = (infoNodeList[i] as XmlElement).GetAttributeNode("MapID");
                if(node == null) continue;
                string mapId = node.InnerText;
                MapInfo mapInfo = new MapInfo();
                mapInfo.Id = Convert.ToUInt32(mapId);
                foreach(XmlElement element in infoNodeList[i].ChildNodes) {
                    switch(element.Name) {
                        case "LoadScene":
                            mapInfo.Scene = Convert.ToString(element.InnerText);
                            break;
                        case "MiniMap":
                            mapInfo.MiniMap = Convert.ToString(element.InnerText);
                            break;
                        case "NameCn":
                            mapInfo.Name = Convert.ToString(element.InnerText);
                            break;
                        case "ACameraPos":
                            mapInfo.ACameraPos = Convert.ToString(element.InnerText);
                            break;
                        case "BCameraPos":
                            mapInfo.BCameraPos = Convert.ToString(element.InnerText);
                            break;
                        case "ShowPic":
                            mapInfo.ShowPic = Convert.ToString(element.InnerText);
                            break;
                        case "PlayerNum":
                            mapInfo.PlayerNum = Convert.ToInt32(element.InnerText);
                            break;
                        case "PlayerMode":
                            mapInfo.PlayerMode = Convert.ToString(element.InnerText);
                            break;
                        case "CameraType":
                            mapInfo.CameraType = Convert.ToInt32(element.InnerText);
                            break;
                        case "IsAI":
                            mapInfo.IsAI = Convert.ToBoolean(Convert.ToInt32(element.InnerText));
                            break;
                        case "IsNormal":
                            mapInfo.IsNormal = Convert.ToBoolean(Convert.ToInt32(element.InnerText));
                            break;
                        case "IsRank":
                            mapInfo.IsRank = Convert.ToBoolean(Convert.ToInt32(element.InnerText));
                            break;
                        case "IsTrain":
                            mapInfo.IsTrain = Convert.ToBoolean(Convert.ToInt32(element.InnerText));
                            break;
                        case "IsDungeon":
                            mapInfo.IsDungeon = Convert.ToBoolean(Convert.ToInt32(element.InnerText));
                            break;
                        case "ShopID":
                            mapInfo.ShopId = Convert.ToInt32(element.InnerText);
                            break;
                    }
                }
                mMapDic.Add(mapInfo.Id, mapInfo);
            }
        }

    }

    public class MapInfo
    {
        public uint Id;    // 地图id
        public string Name;
        public string Scene;  // 对应的场景名称
        public string MiniMap;  // 对应小地图
        public string ShowPic;  // 对应的ui地图
        public int PlayerNum;  // 总人数
        public string PlayerMode;  // 战争模式2v2
        public int CameraType;  // 相机类型  1斜45度 2水平
        public bool IsAI;  // 是否人机
        public bool IsNormal;  // 是否普通
        public bool IsRank;  // 是否天梯
        public bool IsTrain;  // 是否新手
        public bool IsDungeon;  // 是否副本
        public int ShopId;

        public string ACameraPos;
        public string BCameraPos;
    }
}


