/*
 * @Author: iwiniwin
 * @Date: 2020-11-11 22:13:15
 * @LastEditors: iwiniwin
 * @LastEditTime: 2020-12-05 16:02:16
 * @Description: UI资源加载器
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDK.Resource;

namespace UDK.UI
{

    public class UIResourceLoader
    {
        private static Dictionary<string, GameObject> mLoadedResDic = new Dictionary<string, GameObject>();

        // 加载UI资源
        public static GameObject Load(Transform parent, string path)
        {
            GameObject obj = Find(path);
            if (obj != null)
                return obj;
            ResourceUnit unit = ResourceManager.Instance.LoadImmediate(path, EResourceType.PREFAB);
            if (unit == null || unit.Asset == null)
            {
                DebugEx.LogError("load unit failed " + path);
                return null;
            }
            obj = GameObject.Instantiate(unit.Asset) as GameObject;
            obj.transform.SetParent(parent);
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = Vector3.zero;
            mLoadedResDic.Add(path, obj);
            return obj;
        }

        // 查找UI资源是否已加载
        public static GameObject Find(string path)
        {
            if (mLoadedResDic == null) return null;
            GameObject obj = null;
            mLoadedResDic.TryGetValue(path, out obj);
            return obj;
        }

        // 销毁所有的孩子结点
        public static void Destroy(Transform transform)
        {
            while (transform.childCount > 0)
                GameObject.DestroyImmediate(transform.GetChild(0).gameObject);
            transform.DetachChildren();
        }

        // 销毁名为name的孩子结点
        public static void Destroy(Transform transform, string name)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).gameObject.name == name)
                {
                    GameObject.DestroyImmediate(transform.GetChild(i).gameObject);
                }
            }
        }

        // 卸载加载的UI资源
        public static void Unload(string path)
        {
            GameObject obj = Find(path);
            if (obj != null)
            {
                GameObject.DestroyImmediate(obj);
                mLoadedResDic.Remove(path);
            }
        }

        public static void Unload(GameObject obj)
        {
            if (mLoadedResDic == null || mLoadedResDic.Count == 0)
                return;
            if (obj == null) return;
            foreach (KeyValuePair<string, GameObject> pair in mLoadedResDic)
            {
                if (pair.Value == obj)
                {
                    GameObject.DestroyImmediate(pair.Value);
                    mLoadedResDic.Remove(pair.Key);
                    break; ;
                }
            }
        }

        public static void Clear()
        {
            if (mLoadedResDic == null || mLoadedResDic.Count == 0)
                return;
            foreach (string key in mLoadedResDic.Keys)
            {
                GameObject.DestroyImmediate(mLoadedResDic[key]);
            }
            mLoadedResDic.Clear();
        }
    }
}