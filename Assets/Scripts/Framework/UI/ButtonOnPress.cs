using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDK.UI
{
    public class ButtonOnPress : BaseGUI
    {
        public enum EEventType {
            ClickType,
            PressType
        }

        public HandleOnPress ClickHandler;

        protected void OnClick(){
            if(ClickHandler != null)
                ClickHandler(PrIe, false);
        }

        public void AddListener(int ie, HandleOnPress handler, EEventType type = EEventType.ClickType) {
            PrIe = ie;
            if(type == EEventType.ClickType){
                ClickHandler += handler;
            }else{
                PressHandler += handler;
            }
        }

        public void AddListener(HandleOnPress handler, EEventType type = EEventType.ClickType){
            if(type == EEventType.ClickType){
                ClickHandler += handler;
            }else{
                PressHandler += handler;
            }
        }

        public void RemoveListener(HandleOnPress handler, EEventType type = EEventType.ClickType){
            if(type == EEventType.ClickType){
                ClickHandler -= handler;
            }else{
                PressHandler -= handler;
            }
        }
    }
}


