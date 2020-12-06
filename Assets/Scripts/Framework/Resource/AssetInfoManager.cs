/*
 * @Author: iwiniwin
 * @Date: 2020-11-07 22:34:26
 * @LastEditors: iwiniwin
 * @LastEditTime: 2020-11-08 23:44:14
 * @Description: 资源信息管理
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;
using System.IO;

namespace UDK.Resource
{
    public class AssetInfo
    {
        private string mName;    // 资源名称
        private int mIndex;      // 资源索引
        private int mLevel;      // 资源层级
        private int mSize;       // AssetBundle大小
        public List<int> mDependencies = new List<int>();  // 依赖的资源索引

        public AssetInfo()
        {
            mSize = 0;
        }

        public int Size {
            get {
                return mSize;
            }
        }

        public string Name {
            get {
                return mName;
            }
        }

        public int Index {
            get {
                return mIndex;
            }
        }

        // 解析资源信息
        public void Import(XmlElement element)
        {
            mName = element.GetAttribute("name");
            mIndex = Int32.Parse(element.GetAttribute("index"));
            mLevel = Int32.Parse(element.GetAttribute("level"));

            string dependenciesStr = element.GetAttribute("dependencies");
            if (dependenciesStr != null)
            {
                string[] dependencies = dependenciesStr.Split(',');
                for (int i = 0; i < dependencies.Length; i++)
                {
                    mDependencies.Add(Int32.Parse(dependencies[i]));
                }
            }
            mSize = Int32.Parse(element.GetAttribute("bundlesize"));
        }
    }

    public class AssetInfoManager
    {
        public Dictionary<string, AssetInfo> mNameAssetInfos = new Dictionary<string, AssetInfo>();
        public Dictionary<int, AssetInfo> mIndexAssetInfos = new Dictionary<int, AssetInfo>();

        public AssetInfoManager()
        {

        }

        // 根据xml文件加载所有资源信息
        public void LoadAssetInfo()
        {
            StreamReader sr = FileUtil.OpenText("AssetInfo");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(sr.ReadToEnd());
            XmlElement root = doc.DocumentElement;
            IEnumerator iter = doc.GetEnumerator();
            while (iter.MoveNext())
            {
                XmlElement childRoot = iter.Current as XmlElement;

                AssetInfo assetInfo = new AssetInfo();
                assetInfo.Import(childRoot);

                mNameAssetInfos.Add(assetInfo.Name, assetInfo);
                mIndexAssetInfos.Add(assetInfo.Index, assetInfo);
            }
            sr.Close();
        }

        public AssetInfo GetAssetInfo(string name)
        {
            if (mNameAssetInfos.ContainsKey(name))
            {
                return mNameAssetInfos[name];
            }
            return null;
        }

        public AssetInfo GetAssetInfo(int index)
        {
            if (mIndexAssetInfos.ContainsKey(index))
                return mIndexAssetInfos[index];
            return null;
        }

        // 获取指定资源包含依赖资源的大小
        public int GetAllAssetSize(AssetInfo assetInfo)
        {
            int totalSize = 0;
            foreach (int index in assetInfo.mDependencies)
            {
                AssetInfo info = GetAssetInfo(index);
                totalSize += info.Size;
            }
            // 加上本包的大小
            totalSize += assetInfo.Size;
            return totalSize;
        }
    }
}


