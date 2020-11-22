/*
 * @Author: iwiniwin
 * @Date: 2020-11-11 22:48:48
 * @LastEditors: iwiniwin
 * @LastEditTime: 2020-11-11 23:46:56
 * @Description: 视图基类
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Utils;
using Framework.UI;

namespace Framework.View
{
    public abstract class BaseView
    {
        protected Transform mRoot;  // 根节点

        protected SceneType mSceneType;  // 场景类型
        protected string mResName;  // 资源名称
        protected bool mResident;  // 是否常驻
        protected bool mVisible;  // 是否可见

        // 类对象初始化
        public abstract void Init();

        // 类对象释放
        public abstract void Release();

        // 控件初始化
        protected abstract void InitWidget();

        // 控件释放
        protected abstract void ReleaseWidget();

        // 游戏事件注册
        protected abstract void OnAddListener();

        // 游戏事件注销
        protected abstract void OnRemoveListener();

        public abstract void OnEnable();

        public abstract void OnDisable();

        // 每帧更新
        public virtual void Update(float deltaTime) { }

        public Transform Root
        {
            get
            {
                return mRoot;
            }
        }

        public SceneType SceneType
        {
            get
            {
                return mSceneType;
            }
        }

        public bool IsVisible
        {
            get
            {
                return mVisible;
            }
        }

        public bool IsResident
        {
            get
            {
                return mResident;
            }
        }

        public void Show()
        {
            if (mRoot == null)
            {
                if (Create())
                {
                    InitWidget();
                }
            }
            if (mRoot && mRoot.gameObject.activeSelf == false)
            {
                mRoot.gameObject.SetActive(true);
                mVisible = true;
                OnEnable();
                OnAddListener();
            }
        }

        public void Hide()
        {
            if (mRoot && mRoot.gameObject.activeSelf == true)
            {
                OnRemoveListener();
                OnDisable();
                if (mResident)
                {
                    mRoot.gameObject.SetActive(false);
                }
                else
                {
                    ReleaseWidget();
                    Destroy();
                }
                mVisible = false;
            }
        }

        // 预加载
        public void PreLoad()
        {
            if(mRoot == null){
                if(Create()){
                    InitWidget();
                }
            }
        }

        public void DelayDestroy()
        {
            if(mRoot){
                ReleaseWidget();
                Destroy();
            }
        }

        // 创建
        private bool Create()
        {
            if (mRoot)
            {
                DebugEx.LogError("window create error exist");
                return false;
            }
            if (mResName == null || mResName == "")
            {
                DebugEx.LogError("window create error res name is empty");
                return false;
            }
            Transform transform = UIUtility.GetUICamera().transform;
            if (transform == null)
            {
                DebugEx.LogError("window create error ui camera is empty");
                return false;
            }
            GameObject obj = UIResourceLoader.Load(transform, mResName);
            if (obj == null)
            {
                DebugEx.LogError("window create error load res failed " + mResName);
                return false;
            }
            mRoot = obj.transform;
            mRoot.gameObject.SetActive(false);
            return true;
        }

        protected void Destroy()
        {
            if (mRoot)
            {
                UIResourceLoader.Unload(mRoot.gameObject);
                mRoot = null;
            }
        }
    }
}


