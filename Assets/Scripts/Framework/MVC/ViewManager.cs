using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDK.View
{
    public enum ESceneType {
        ST_None,
        ST_Login,
        ST_Play
    }

    public enum EViewType {
        WT_LoginWindow
    }

    public class ViewManager : Singleton<ViewManager>
    {
        private Dictionary<EViewType, BaseView> mViewDic;
        public ViewManager(){
            mViewDic = new Dictionary<EViewType, BaseView>();
            // mViewDic[EViewType.WT_LoginWindow] = new LoginWindow();
        }

        public BaseView GetView(EViewType type){
            if(mViewDic.ContainsKey(type))
                return mViewDic[type];
            return null;
        }

        public void Update(float deltaTime){
            foreach(BaseView view in mViewDic.Values){
                if(view.IsVisible){
                    view.Update(deltaTime);
                }
            }
        }

        public void Hide(ESceneType front){
            foreach(var item in mViewDic){
                if(front == item.Value.SceneType){
                    item.Value.Hide();
                }
            }
        }

        public void Show(EViewType type){
            BaseView view;
            if(!mViewDic.TryGetValue(type, out view)){
                return;
            }
            view.Show();
        }
    }
}


