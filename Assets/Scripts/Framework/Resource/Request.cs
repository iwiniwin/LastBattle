/*
 * @Author: iwiniwin
 * @Date: 2020-11-08 14:51:07
 * @LastEditors: iwiniwin
 * @LastEditTime: 2020-11-08 18:42:14
 * @Description: 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Resource
{
    public enum RequestType
    {
        LOAD,
        UNLOAD,
        LOAD_LEVEL,
        UNLOAD_LEVEL
    }

    public class Request
    {
        internal string mFileName;
        internal ResourceType mResourceType;
        internal ResourceManager.LoadFinishEventHandle mHandle;
        internal ResourceManager.LoadLevelFinishEventHandle mHandleLevel;
        internal ResourceManager.UnloadLevelFinishEventHandle mHandleUnloadLevel;

        internal RequestType mRequestType;
        internal ResourceAsyncOperation mResourceAsyncOperation;

        internal Request(string fileName, ResourceType resourceType, ResourceManager.LoadFinishEventHandle handle, RequestType requestType, ResourceAsyncOperation operation)
        {
            mFileName = fileName;
            mResourceType = resourceType;
            mHandle = handle;
            mRequestType = requestType;
            mResourceAsyncOperation = operation;
        }

        internal Request(string fileName, ResourceType resourceType, ResourceManager.LoadLevelFinishEventHandle handle, RequestType requestType, ResourceAsyncOperation operation){
            mFileName = fileName;
            mResourceType = resourceType;
            mHandleLevel = handle;
            mRequestType = requestType;
            mResourceAsyncOperation = operation;
        }

        internal Request(string fileName, ResourceType resourceType, ResourceManager.UnloadLevelFinishEventHandle handle, RequestType requestType, ResourceAsyncOperation operation){
            mFileName = fileName;
            mResourceType = resourceType;
            mHandleUnloadLevel = handle;
            mRequestType = requestType;
            mResourceAsyncOperation = operation;
        }
    }
}


