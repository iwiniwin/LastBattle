/*
 * @Author: iwiniwin
 * @Date: 2020-12-03 00:14:13
 * @LastEditors: iwiniwin
 * @LastEditTime: 2020-12-03 23:23:27
 * @Description: 模块管理器
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDK.Debug;

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
            T view = LoadModule<T>();
            if(view != null){
                view.BindCtrl<U>();
            }
            return view;
        }

        public T LoadModule<T>() where T : BaseView, new(){
            T view = GetModule<T>();
            if(view != null) {
                DebugEx.LogWarning("repeat load module");
                return view;
            }
            view = new T();
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


