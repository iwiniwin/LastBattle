using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDK.UI
{
    public class BaseGUI : MonoBehaviour
    {
        public AudioClip Sound;

        public int PrIe {
            get;
            protected set;
        }

        protected virtual void OnPress(bool pressed){
            if(!pressed || Sound == null)
                return;
            AudioUtil.PlaySound(Sound);
        }

        public delegate void HandleOnPress(int ie, bool pressed);
        public HandleOnPress PressHandler;

        public void AddListener(int ie, HandleOnPress handler){
            PrIe = ie;
            PressHandler += handler;
        }

        public void RemoveListener(HandleOnPress handler){
            PressHandler -= handler;
        }
    }
}


