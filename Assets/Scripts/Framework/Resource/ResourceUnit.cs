/*
 * @Author: iwiniwin
 * @Date: 2020-11-07 21:50:59
 * @LastEditors: iwiniwin
 * @LastEditTime: 2020-11-29 23:46:34
 * @Description: 资源单元
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Object = UnityEngine.Object;

namespace UDK.Resource
{
    // 资源类型
    public enum EResourceType
    {
        ASSET,
        PREFAB,
        LEVEL_ASSET,
        LEVEL,
    }

    public class ResourceUnit : IDisposable
    {
        private string mPath;
        private Object mAsset;  // 资源本身
        private EResourceType mResourceType;
        private List<ResourceUnit> mNextLevelAssets;  // 记录下一个场景的资源
        private AssetBundle mAssetBundle;
        private int mAssetBundleSize;
        private int mReferenceCount;

        internal ResourceUnit(AssetBundle assetBundle, int assetBundleSize, Object asset, string path, EResourceType resourceType)
        {
            mAssetBundle = assetBundle;
            mAssetBundleSize = assetBundleSize;
            mAsset = asset;
            mPath = path;
            mResourceType = resourceType;
            mReferenceCount = 0;
            mNextLevelAssets = new List<ResourceUnit>();
        }

        public Object Asset
        {
            get
            {
                return mAsset;
            }
            internal set
            {
                mAsset = value;
            }
        }

        public EResourceType ResourceType
        {
            get
            {
                return mResourceType;
            }
        }

        public List<ResourceUnit> NextLevelAssets
        {
            get
            {
                return mNextLevelAssets;
            }
            internal set
            {
                mNextLevelAssets.Clear();
                foreach (ResourceUnit asset in value)
                {
                    mNextLevelAssets.Add(asset);
                }
            }
        }

        public AssetBundle AssetBundle
        {
            get
            {
                return mAssetBundle;
            }
            set
            {
                mAssetBundle = value;
            }
        }

        public int AssetBundleSize
        {
            get
            {
                return mAssetBundleSize;
            }
        }

        public int ReferenceCount {
            get {
                return mReferenceCount;
            }
        }
        
        // 增加引用计数
        public void AddReferenceCount(){
            mReferenceCount ++;
            foreach(ResourceUnit asset in mNextLevelAssets){
                asset.AddReferenceCount();
            }
        }

        // 减少引用计数
        public void ReduceReferenceCount(){
            mReferenceCount --;
            foreach (ResourceUnit asset in mNextLevelAssets)
            {
                asset.ReduceReferenceCount();
            }
            if(IsCanDestroy()){
                Dispose();
            }
        }

        public bool IsCanDestroy(){
            return 0 == mReferenceCount;
        }

        public void Dispose(){
            if(null != mAssetBundle){
                mAssetBundle = null;
            }
            mNextLevelAssets.Clear();
            mAsset = null;
        }
    }
}


