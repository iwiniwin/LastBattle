/*
 * @Author: iwiniwin
 * @Date: 2020-12-03 00:14:13
 * @LastEditors: iwiniwin
 * @LastEditTime: 2020-12-03 01:07:40
 * @Description: 模块管理器
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDK.MVC
{
    public class ModuleManager<T> : Singleton<ModuleManager<T>> where T : System.Enum
    {
        private Dictionary<T, BaseCtrl<T>> mCtrlDic;
        public ModuleManager(){
            mCtrlDic = new Dictionary<T, BaseCtrl<T>>();
        }

        public void Init(params BaseCtrl<T>[] ctrls){
            foreach(BaseCtrl<T> ctrl in ctrls) {
                mCtrlDic.Add(ctrl.Type, ctrl);
            }
        }

        public BaseCtrl<T> GetCtrl(T type){
            if(mCtrlDic.ContainsKey(type))
                return mCtrlDic[type];
            return null;
        }

        public void Update(float deltaTime){
            foreach(BaseCtrl<T> ctrl in mCtrlDic.Values){
                if(ctrl.NeedUpdate){
                    ctrl.Update(deltaTime);
                }
            }
        }

        // 预加载指定模块
        public void Preload(T type){
            BaseCtrl<T> ctrl = GetCtrl(type);
            if(ctrl != null)
                ctrl.Preload();
        }

        // 进入指定模块
        public void Enter(T type){
            BaseCtrl<T> ctrl = GetCtrl(type);
            if(ctrl != null)
                ctrl.Enter();
        }

        // 退出指定模块
        public void Exit(T type){
            BaseCtrl<T> ctrl = GetCtrl(type);
            if(ctrl != null)
                ctrl.Exit();
        }
    }
}


