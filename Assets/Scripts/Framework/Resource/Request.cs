/*
 * @Author: iwiniwin
 * @Date: 2020-11-08 14:51:07
 * @LastEditors: iwiniwin
 * @LastEditTime: 2020-11-29 23:47:23
 * @Description: 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDK.Resource
{
    public enum ERequestType
    {
        LOAD,
        UNLOAD,
        LOAD_LEVEL,
        UNLOAD_LEVEL
    }

    public class Request
    {
        internal string mFileName;
        internal EResourceType mResourceType;
        internal ResourceManager.LoadFinishEventHandle mHandle;
        internal ResourceManager.LoadLevelFinishEventHandle mHandleLevel;
        internal ResourceManager.UnloadLevelFinishEventHandle mHandleUnloadLevel;

        internal ERequestType mRequestType;
        internal ResourceAsyncOperation mResourceAsyncOperation;

        internal Request(string fileName, EResourceType resourceType, ResourceManager.LoadFinishEventHandle handle, ERequestType requestType, ResourceAsyncOperation operation)
        {
            mFileName = fileName;
            mResourceType = resourceType;
            mHandle = handle;
            mRequestType = requestType;
            mResourceAsyncOperation = operation;
        }

        internal Request(string fileName, EResourceType resourceType, ResourceManager.LoadLevelFinishEventHandle handle, ERequestType requestType, ResourceAsyncOperation operation){
            mFileName = fileName;
            mResourceType = resourceType;
            mHandleLevel = handle;
            mRequestType = requestType;
            mResourceAsyncOperation = operation;
        }

        internal Request(string fileName, EResourceType resourceType, ResourceManager.UnloadLevelFinishEventHandle handle, ERequestType requestType, ResourceAsyncOperation operation){
            mFileName = fileName;
            mResourceType = resourceType;
            mHandleUnloadLevel = handle;
            mRequestType = requestType;
            mResourceAsyncOperation = operation;
        }
    }
}


