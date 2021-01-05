using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDK.Resource;

namespace UDK
{
    public enum EPoolObjectType
    {
        POT_Effect,
    }

    // 对象池内对象的信息
    public class PoolObjectInfo
    {
        public string mName;

        // 缓存时间
        public float mCacheTime;
        public EPoolObjectType type;

        // 是否可以重用
        public bool mCanUse = true;
        // 重置时间
        public float mResetTime = 0.0f;
    }

    public class ObjectPool
    {
        public Dictionary<GameObject, PoolObjectInfo> mContainer = new Dictionary<GameObject, PoolObjectInfo>();
    }

    public class ObjectPoolManager : Singleton<ObjectPoolManager>
    {
        private Dictionary<string, ObjectPool> mPoolDic = new Dictionary<string, ObjectPool>();
        private GameObject objectsPool;

        private float mCacheTime = 1800.0f;

        // 删除队列
        private List<GameObject> mDestroyPoolObjects = new List<GameObject>();

        private bool TryGetObject(ObjectPool pool, out KeyValuePair<GameObject, PoolObjectInfo> keyValuePair)
        {
            if (pool.mContainer.Count > 0)
            {
                foreach (KeyValuePair<GameObject, PoolObjectInfo> pair in pool.mContainer)
                {
                    PoolObjectInfo info = pair.Value;
                    if (info.mCanUse)
                    {
                        keyValuePair = pair;
                        return true;
                    }
                }
            }
            keyValuePair = new KeyValuePair<GameObject, PoolObjectInfo>();
            return false;
        }

        public GameObject GetObject(string res)
        {
            if (res == null) return null;

            ObjectPool pool = null;
            KeyValuePair<GameObject, PoolObjectInfo> pair;
            if (!mPoolDic.TryGetValue(res, out pool) || !TryGetObject(pool, out pair))
            {
                // 新创建
                ResourceUnit unit = ResourceManager.Instance.LoadImmediate(res, EResourceType.PREFAB);
                if (unit.Asset == null)
                {
                    DebugEx.LogError("can not find the resource " + res);
                }
                return GameObject.Instantiate(unit.Asset) as GameObject;
            }

            GameObject go = pair.Key;
            PoolObjectInfo info = pair.Value;
            pool.mContainer.Remove(go);
            EnablePoolObject(go, info);

            return go;
        }

        public void ReleaseObject(string res, GameObject gameObject, EPoolObjectType type)
        {
            if (objectsPool == null)
            {
                objectsPool = new GameObject("ObjectPool");
                // objectsPool.AddComponent<UIP
                objectsPool.transform.localPosition = new Vector3(0, -5000, 0);
            }

            if (null == res || null == gameObject)
            {
                return;
            }

            ObjectPool pool = null;
            if (!mPoolDic.TryGetValue(res, out pool))
            {
                pool = new ObjectPool();
                mPoolDic.Add(res, pool);
            }

            PoolObjectInfo poolObjectInfo = new PoolObjectInfo();
            poolObjectInfo.type = type;
            poolObjectInfo.mName = res;

            DisablePoolObject(gameObject, poolObjectInfo);

            pool.mContainer[gameObject] = poolObjectInfo;
        }

        //设置缓存物体有效
        public void EnablePoolObject(GameObject go, PoolObjectInfo info)
        {
            go.SetActive(true);
            go.transform.parent = null;

            info.mCacheTime = 0.0f;
        }

        //设置缓存物体有效
        public void DisablePoolObject(GameObject go, PoolObjectInfo info)
        {
            //特效Disable
            if (info.type == EPoolObjectType.POT_Effect)
            {
                ParticleSystem[] particles = go.GetComponentsInChildren<ParticleSystem>(true);
                foreach (ParticleSystem part in particles)
                {
                    part.Clear();
                }

                //解绑到poolGameobject节点
                go.transform.parent = objectsPool.transform;

                //拖尾处理
                if (go.tag == "trail")
                {

                }
                else
                {
                    info.mCanUse = true;
                    go.SetActive(false);
                }

            }

            go.SetActive(false);
            //解绑到poolGameobject节点
            go.transform.parent = objectsPool.transform;

        }

        //清除
        public void Clear()
        {
            mPoolDic.Clear();
            mDestroyPoolObjects.Clear();
            objectsPool = null;
        }

        float mTotalTime = 0;
        public void OnUpdate()
        {
            //每隔0.1更新一次
            mTotalTime += Time.deltaTime;
            if (mTotalTime <= 0.1f)
            {
                return;
            }
            else
            {
                mTotalTime = 0;
            }

            float deltaTime = Time.deltaTime;

            //遍历数据
            foreach (ObjectPool pool in mPoolDic.Values)
            {
                //死亡列表
                mDestroyPoolObjects.Clear();

                foreach (KeyValuePair<GameObject, PoolObjectInfo> pair in pool.mContainer)
                {
                    GameObject obj = pair.Key;
                    PoolObjectInfo info = pair.Value;

                    info.mCacheTime += deltaTime;

                    float allCachTime = mCacheTime;

                    if (info.mCacheTime >= allCachTime)
                    {
                        mDestroyPoolObjects.Add(obj);
                    }

                    if (!info.mCanUse)
                    {
                        info.mResetTime += deltaTime;

                        if (info.mResetTime > 1.0f)
                        {
                            info.mResetTime = .0f;
                            info.mCanUse = true;

                            obj.SetActive(false);
                        }
                    }
                }

                //移除
                for (int k = 0; k < mDestroyPoolObjects.Count; k++)
                {
                    GameObject obj = mDestroyPoolObjects[k];
                    //obj.transform.parent = null;
                    GameObject.DestroyImmediate(obj);

                    pool.mContainer.Remove(obj);
                }
            }
        }
    }
}


