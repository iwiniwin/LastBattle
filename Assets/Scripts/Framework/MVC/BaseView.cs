/*
 * @Author: iwiniwin
 * @Date: 2020-11-11 22:48:48
 * @LastEditors: iwiniwin
 * @LastEditTime: 2020-12-05 21:56:56
 * @Description: 视图基类
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDK.UI;

namespace UDK.MVC
{
    public abstract class BaseView
    {
        
        public BaseCtrl Ctrl{get; private set;}  // 对应的控制器
        public Transform Root{get; protected set;}  // 根节点

        public bool EnablePreload {get; protected set;}  // 是否需要预加载

        // public U SceneType{get; protected set;}  // 视图所属的场景类型

        protected string ResName {get; set;}  // 资源名称
        public bool IsResident{get; protected set;}  // 是否常驻
        public bool IsVisible{get; private set;}   // 是否可见

        public abstract void Init();

        // 初始化，视图对应的UI资源被加载时触发
        protected abstract void OnLoad();

         // 视图对象被销毁时触发
        public abstract void Release();

        public abstract void OnEnable();

        public abstract void OnDisable();

        public void Show()
        {
            if (Root == null)
            {
                if (Create())
                {
                    OnLoad();
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
                    OnLoad();
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
            if (ResName == null || ResName == "")
            {
                DebugEx.LogError("window create error res name is empty");
                return false;
            }
            Transform transform = GameObject.Find("Canvas").transform;
            if (transform == null)
            {
                DebugEx.LogError("window create error ui camera is empty");
                return false;
            }
            GameObject obj = UIResourceLoader.Load(transform, ResName);
            if (obj == null)
            {
                DebugEx.LogError("window create error load res failed " + ResName);
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
                UnbindCtrl();
                Release();
                UIResourceLoader.Unload(Root.gameObject);
                Root = null;
            }
        }

        public BaseCtrl BindCtrl<T>() where T : BaseCtrl, new(){
            if(Ctrl != null){
                DebugEx.LogWarning("repeat bind ctrl");
                return Ctrl;
            }
            Ctrl = new T();
            Ctrl.View = this;
            Ctrl.Init();
            return Ctrl;
        }

        public void UnbindCtrl(){
            if(Ctrl != null){
                Ctrl.Release();
                Ctrl = null;
            }
        }
    }
}


