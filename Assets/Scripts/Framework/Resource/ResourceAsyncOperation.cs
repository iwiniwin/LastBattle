/*
 * @Author: iwiniwin
 * @Date: 2020-11-08 15:06:52
 * @LastEditors: iwiniwin
 * @LastEditTime: 2020-11-29 23:47:13
 * @Description: 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDK.Resource
{
    public class ResourceAsyncOperation 
    {
        internal ERequestType mRequestType;
        internal int mAllDependenciesAssetSize;
        internal int mLoadedDependenciesAssetSize;
        internal bool mComplete;
        public AsyncOperation asyncOperation;

        internal ResourceUnit mResource;

        internal ResourceAsyncOperation(ERequestType requestType){
            mRequestType = requestType;
            mAllDependenciesAssetSize = 0;  // 需要加载的资源总大小
            mLoadedDependenciesAssetSize = 0;
            mComplete = false;
            asyncOperation = null;
            mResource = null;
        }

        public bool Complete {
            get {
                return mComplete;
            }
        }

        public int Progress {
            get {
                if(mComplete)
                    return 100;
                else if(0 == mLoadedDependenciesAssetSize)
                    return 0;
                else{
                    // 使用AssetBundle
                    if(ResourceManager.Instance.UsedAssetBundle){
                        if(ERequestType.LOAD_LEVEL == mRequestType){
                            int depsProgress = (int)(((float)mLoadedDependenciesAssetSize / mAllDependenciesAssetSize) * 100);
                            int levelProgress = asyncOperation != null ? (int)((float)asyncOperation.progress * 100) : 0;
                            return (int)(depsProgress * 0.8) + (int)(levelProgress * 0.2);
                        }else{
                            return (int)(((float)mLoadedDependenciesAssetSize / mAllDependenciesAssetSize) * 100);
                        }
                    }else{
                        if(ERequestType.LOAD_LEVEL == mRequestType){
                            return asyncOperation != null ? (int)((float)asyncOperation.progress * 100) : 0;
                        }else{
                            return 0;
                        }
                    }
                }
            }
        }
    }
}


