/*
 * @Author: iwiniwin
 * @Date: 2020-11-08 14:48:52
 * @LastEditors: iwiniwin
 * @LastEditTime: 2020-11-08 23:43:44
 * @Description: 资源管理器
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Utils;
using System.IO;
using UnityEngine.SceneManagement;

namespace Framework.Resource
{
    public class ResourceManager : UnitySingleton<ResourceManager>
    {
        // 是否使用AssetBundle加载资源
        public bool UsedAssetBundle = false;

        private bool mInit = false;
        private int mFrameCount = 0;
        private Request mCurrentRequest = null;
        private Queue<Request> mAllRequests = new Queue<Request>();

        // 保存读取的资源信息
        private AssetInfoManager mAssetInfoManager = null;
        private Dictionary<string, string> mResources = new Dictionary<string, string>();
        // 已加载的资源
        private Dictionary<string, ResourceUnit> mLoadedResourceUnit = new Dictionary<string, ResourceUnit>();

        public delegate void LoadFinishEventHandle(ResourceUnit resource);
        public delegate void LoadLevelFinishEventHandle();
        public delegate void UnloadLevelFinishEventHandle();

        public void Init()
        {
            if (UsedAssetBundle)
            {
                mAssetInfoManager = new AssetInfoManager();
                mAssetInfoManager.LoadAssetInfo();

                ArchiveManager.Instance.Init();
            }
            mInit = true;
        }

        private void Update()
        {
            if (!mInit)
                return;
            
            // 每帧处理一次请求
            if (null == mCurrentRequest && mAllRequests.Count > 0)
                HandleRequest();

            ++mFrameCount;

            if (mFrameCount == 300)
            {
                mFrameCount = 0;
            }
        }

        private void HandleRequest()
        {
            mCurrentRequest = mAllRequests.Dequeue();
            string fileName = mCurrentRequest.mFileName;
            if (UsedAssetBundle)
            {
                switch (mCurrentRequest.mRequestType)
                {
                    case RequestType.LOAD:
                        {
                            switch (mCurrentRequest.mResourceType)
                            {
                                case ResourceType.ASSET:
                                case ResourceType.PREFAB:
                                    {
                                        if (mLoadedResourceUnit.ContainsKey(fileName))  // 资源已加载
                                        {
                                            mCurrentRequest.mResourceAsyncOperation.mComplete = true;
                                            mCurrentRequest.mResourceAsyncOperation.mResource = mLoadedResourceUnit[fileName];
                                            if (null != mCurrentRequest.mHandle)
                                            {
                                                mCurrentRequest.mHandle(mLoadedResourceUnit[fileName]);
                                            }
                                            HandleResponse();
                                        }
                                        else
                                        {
                                             DebugEx.LogError("unknow");
                                        }
                                    }
                                    break;
                                case ResourceType.LEVEL_ASSET:
                                    DebugEx.LogError("unknow");
                                    break;
                                case ResourceType.LEVEL:
                                    DebugEx.LogError("unknow");
                                    break;
                            }
                        }
                        break;
                    case RequestType.UNLOAD:
                        {
                            if (!mLoadedResourceUnit.ContainsKey(fileName))
                                DebugEx.LogError("can not find " + fileName);
                            else
                            {
                                mLoadedResourceUnit[fileName].ReduceReferenceCount();
                            }
                            HandleResponse();
                        }
                        break;
                    case RequestType.LOAD_LEVEL:
                        {
                            StartCoroutine(_LoadLevel(fileName, mCurrentRequest.mHandleLevel, ResourceType.LEVEL, mCurrentRequest.mResourceAsyncOperation));
                        }
                        break;
                    case RequestType.UNLOAD_LEVEL:
                        {
                            if (!mLoadedResourceUnit.ContainsKey(fileName))
                                DebugEx.LogError("can not find levle " + fileName);
                            else
                            {
                                mLoadedResourceUnit[fileName].ReduceReferenceCount();
                                if (null != mCurrentRequest.mHandleUnloadLevel)
                                    mCurrentRequest.mHandleUnloadLevel();
                                HandleResponse();
                            }
                        }
                        break;
                }
            }
            else
            {
                switch (mCurrentRequest.mRequestType)
                {
                    case RequestType.LOAD:
                        {
                            switch(mCurrentRequest.mResourceType){
                                case ResourceType.ASSET:
                                case ResourceType.PREFAB:
                                    {
                                        DebugEx.LogError("unknow");
                                    }
                                    break;
                                case ResourceType.LEVEL_ASSET:
                                    {
                                        DebugEx.LogError("unknow");
                                    }
                                    break;
                                case ResourceType.LEVEL:
                                    {
                                        DebugEx.LogError("this is impossible !!!");
                                    }
                                    break;
                            }
                            
                        }
                        break;
                    case RequestType.UNLOAD:
                        {
                            HandleResponse();
                        }
                        break;
                    case RequestType.LOAD_LEVEL:
                        {
                            StartCoroutine(_LoadLevel(fileName, mCurrentRequest.mHandleLevel, ResourceType.LEVEL, mCurrentRequest.mResourceAsyncOperation));
                        }
                        break;
                    case RequestType.UNLOAD_LEVEL:
                        {
                            if (null != mCurrentRequest.mHandleUnloadLevel)
                                mCurrentRequest.mHandleUnloadLevel();
                            HandleResponse();
                        }
                        break;
                }
            }
        }

        private void HandleResponse()
        {
            mCurrentRequest = null;
        }

        public ResourceUnit LoadImmediate(string filePath, ResourceType resourceType, string archiveName = "Resources")
        {
            if (UsedAssetBundle)
            {
                string fullPath = "Resources/" + filePath;
                string fullName = ArchiveManager.Instance.GetPath("Resources", fullPath);

                AssetInfo sceneAssetInfo = mAssetInfoManager.GetAssetInfo(fullName);

                // 加载依赖的资源
                foreach (int index in sceneAssetInfo.mDependencies)
                {
                    AssetInfo dependencyAsset = mAssetInfoManager.GetAssetInfo(index);
                    string dependencyAssetName = dependencyAsset.Name;
                    return _LoadImmediate(fullName, resourceType);
                }

                // 加载自身
                ResourceUnit unit = _LoadImmediate(fullName, resourceType);
                return unit;
            }
            else
            {
                Object asset = Resources.Load(filePath);
                return new ResourceUnit(null, 0, asset, null, resourceType);
            }
        }

        // 加载场景
        public ResourceAsyncOperation LoadLevel(string fileName, LoadLevelFinishEventHandle handle, string archiveName = "Level")
        {
            if (UsedAssetBundle)
            {
                string fullName = ArchiveManager.Instance.GetPath(archiveName, fileName);
                if (mLoadedResourceUnit.ContainsKey(fullName))
                {
                    DebugEx.LogError("load same level twice : " + fullName);
                    return null;
                }
                else
                {
                    ResourceAsyncOperation operation = new ResourceAsyncOperation(RequestType.LOAD_LEVEL);
                    mAllRequests.Enqueue(new Request(fullName, ResourceType.LEVEL, handle, RequestType.LOAD_LEVEL, operation));
                    return operation;
                }
            }
            else
            {
                ResourceAsyncOperation operation = new ResourceAsyncOperation(RequestType.LOAD_LEVEL);
                mAllRequests.Enqueue(new Request(fileName, ResourceType.LEVEL, handle, RequestType.LOAD_LEVEL, operation));
                return operation;
            }
        }

        private IEnumerator _LoadLevel(string path, LoadLevelFinishEventHandle handle, ResourceType resourceType, ResourceAsyncOperation operation)
        {
            if (UsedAssetBundle)
            {
                AssetInfo sceneAssetInfo = mAssetInfoManager.GetAssetInfo(path);
                // 获取该资源总大小（包括依赖资源）
                operation.mAllDependenciesAssetSize = mAssetInfoManager.GetAllAssetSize(sceneAssetInfo);

                // 加载依赖资源
                foreach (int index in sceneAssetInfo.mDependencies)
                {
                    AssetInfo dependencyAsset = mAssetInfoManager.GetAssetInfo(index);
                    string dependencyAssetName = dependencyAsset.Name;

                    ResourceUnit unit = _LoadImmediate(dependencyAssetName, ResourceType.LEVEL);
                    operation.mLoadedDependenciesAssetSize += unit.AssetBundleSize;
                }

                // 加载场景的AssetBundle
                int sceneAssetBundleSize = 0;
                byte[] binary = FileUtil.GetAssetBundleFileBytes(path, ref sceneAssetBundleSize);
                AssetBundle assetBundle = AssetBundle.LoadFromMemory(binary);
                if (!assetBundle)
                    DebugEx.LogError("create scene assetbundle " + path + " failed !!!");

                operation.mLoadedDependenciesAssetSize += sceneAssetBundleSize;

                AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(FileUtil.GetFileName(path, false));
                operation.asyncOperation = asyncOperation;
                yield return asyncOperation;

                HandleResponse();

                operation.asyncOperation = null;
                operation.mComplete = true;
                operation.mResource = null;
                if (handle != null)
                    handle();
            }
            else
            {
                ResourceUnit level = new ResourceUnit(null, 0, null, path, resourceType);

                string sceneName = FileUtil.GetFileName(path, true);
                AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(FileUtil.GetFileName(path, false));
                operation.asyncOperation = asyncOperation;
                yield return asyncOperation;
                operation.asyncOperation = null;
                operation.mComplete = true;
                if (handle != null)
                    handle();
            }
        }

        // 加载单个资源
        ResourceUnit _LoadImmediate(string fileName, ResourceType resourceType)
        {
            if (!mLoadedResourceUnit.ContainsKey(fileName))
            {
                int assetBundleSize = 0;
                byte[] binary = FileUtil.GetAssetBundleFileBytes(fileName, ref assetBundleSize);
                AssetBundle assetBundle = AssetBundle.LoadFromMemory(binary);
                if (!assetBundle)
                {
                    DebugEx.LogError("create assetbundle " + fileName + " failed !!!");
                }
                Object asset = assetBundle.LoadAsset(fileName);
                if (!asset)
                    DebugEx.LogError("load assetbundle failed");

                ResourceUnit unit = new ResourceUnit(assetBundle, assetBundleSize, asset, fileName, resourceType);
                mLoadedResourceUnit.Add(fileName, unit);
                return unit;
            }
            else
            {
                return mLoadedResourceUnit[fileName];
            }
        }
    }
}
