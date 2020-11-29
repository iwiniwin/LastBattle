using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDK.UI
{
    public class ButtonOnPress : BaseGUI
    {
        public enum EventType {
            ClickType,
            PressType
        }

        public HandleOnPress ClickHandler;

        protected void OnClick(){
            if(ClickHandler != null)
                ClickHandler(PrIe, false);
        }

        public void AddListener(int ie, HandleOnPress handler, EventType type = EventType.ClickType) {
            PrIe = ie;
            if(type == EventType.ClickType){
                ClickHandler += handler;
            }else{
                PressHandler += handler;
            }
        }

        public void AddListener(HandleOnPress handler, EventType type = EventType.ClickType){
            if(type == EventType.ClickType){
                ClickHandler += handler;
            }else{
                PressHandler += handler;
            }
        }

        public void RemoveListener(HandleOnPress handler, EventType type = EventType.ClickType){
            if(type == EventType.ClickType){
                ClickHandler -= handler;
            }else{
                PressHandler -= handler;
            }
        }
    }
}


