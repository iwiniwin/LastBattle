/*
 * @Author: iwiniwin
 * @Date: 2020-12-03 00:14:13
 * @LastEditors: iwiniwin
 * @LastEditTime: 2020-12-06 12:19:41
 * @Description: 模块管理器
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDK.MVC
{
    public class ModuleManager : Singleton<ModuleManager>
    {
        private Dictionary<System.Type, BaseView> mViewDic;
        public ModuleManager(){
            mViewDic = new Dictionary<System.Type, BaseView>();
        }

        // 获取指定模块
        public T GetModule<T>() where T : BaseView {
            var type = typeof(T);
            if(mViewDic.ContainsKey(type)){
                return mViewDic[type] as T;
            }
            return null;
        }

        // 加载指定模块
        public T LoadModule<T, U>() where T : BaseView, new() where U : BaseCtrl, new(){
            T view = GetModule<T>();
            if(view != null) {
                DebugEx.LogWarning("repeat load module");
                return view;
            }
            view = new T();
            view.BindCtrl<U>();
            view.Init();
            if(view.EnablePreload){
                view.Preload();
            }
            mViewDic.Add(typeof(T), view);
            return view;
        }

        public T LoadModule<T>() where T : BaseView, new(){
            T view = GetModule<T>();
            if(view != null) {
                DebugEx.LogWarning("repeat load module");
                return view;
            }
            view = new T();
            view.Init();
            if(view.EnablePreload){
                view.Preload();
            }
            mViewDic.Add(typeof(T), view);
            return view;
        }

        // 每帧更新
        public void Update(float deltaTime){
            foreach(BaseView view in mViewDic.Values){
                if(view.Ctrl != null && view.Ctrl.NeedUpdate){
                    view.Ctrl.Update(deltaTime);
                }
            }
        }

        public void UnloadModule<T>() where T : BaseView {
            T view = GetModule<T>();
            if(view != null){
                view.Hide();
                view.Destroy();
                mViewDic.Remove(typeof(T));
            }
        }
    }
}


