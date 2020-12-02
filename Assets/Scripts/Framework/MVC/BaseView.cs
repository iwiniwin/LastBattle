/*
 * @Author: iwiniwin
 * @Date: 2020-11-11 22:48:48
 * @LastEditors: iwiniwin
 * @LastEditTime: 2020-12-03 01:20:36
 * @Description: 视图基类
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDK.Debug;
using UDK.UI;

namespace UDK.MVC
{
    public abstract class BaseView
    {
        public Transform Root{get; protected set;}  // 根节点

        // public T Type{get; protected set;}  // 视图类型
        // public U SceneType{get; protected set;}  // 视图所属的场景类型

        protected string mResName;  // 资源名称
        public bool IsResident{get; protected set;}  // 是否常驻
        public bool IsVisible{get; private set;}   // 是否可见

        // 视图对象被实例化时触发初始化
        public abstract void Init();

        // 视图对象被销毁时触发
        public abstract void Release();

        // 视图上的控件初始化，视图对应的UI资源被创建时触发
        protected abstract void InitWidget();

        // 视图上的控件释放，视图对象被销毁时触发
        protected abstract void ReleaseWidget();

        public abstract void OnEnable();

        public abstract void OnDisable();

        // 每帧更新
        public virtual void Update(float deltaTime) { }

        public void Show()
        {
            if (Root == null)
            {
                if (Create())
                {
                    InitWidget();
                }
            }
            if (Root && Root.gameObject.activeSelf == false)
            {
                Root.gameObject.SetActive(true);
                IsVisible = true;
                OnEnable();
            }
        }

        public void Hide()
        {
            if (Root && Root.gameObject.activeSelf == true)
            {
                OnDisable();
                if (IsResident)
                {
                    Root.gameObject.SetActive(false);
                }
                else
                {
                    Destroy();
                }
                IsVisible = false;
            }
        }

        // 预加载
        public void Preload()
        {
            if(Root == null){
                if(Create()){
                    InitWidget();
                }
            }
        }

        // 创建，加载视图对应的UI资源
        private bool Create()
        {
            if (Root)
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
            Root = obj.transform;
            Root.gameObject.SetActive(false);
            return true;
        }

        public void Destroy()
        {
            if (Root)
            {
                Release();
                ReleaseWidget();
                UIResourceLoader.Unload(Root.gameObject);
                Root = null;
            }
        }
    }
}


