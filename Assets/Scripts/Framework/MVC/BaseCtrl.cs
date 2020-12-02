using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDK.Debug;

namespace UDK.MVC 
{
    public abstract class BaseCtrl<T> where T : System.Enum
    {
        private BaseView mView;

        public T Type {get; protected set;}

        public virtual bool NeedUpdate {
            get {
                if(mView != null && mView.IsVisible) {
                    return true;
                }else{
                    return false;
                }
            }
        }

        public abstract void Enter();

        public abstract void Exit();

        public virtual void Preload() {
            if(mView != null && mView.IsResident) {
                mView.Preload();
            }
        }

        public virtual void Update(float deltaTime){
            if(mView != null){
                mView.Update(deltaTime);
            }
        } 

        public void ShowView() {
            if(mView != null)
                mView.Show();
        }

        public void HideView() {
            if(mView != null)
                mView.Hide();
        }

        public BaseView GetView(){
            return mView;
        } 

        public BaseView BindView<U>() where U : BaseView, new(){
            if(mView != null){
                DebugEx.LogWarning("already bind view");
                return mView;
            }
            mView = new U();
            mView.Init();
            return mView;
        }

        public void UnbindView(){
            if(mView != null){
                mView.Destroy();
                mView = null;
            }
        }
    }
}


