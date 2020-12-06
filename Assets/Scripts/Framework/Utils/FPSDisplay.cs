using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDK 
{
    public class FPSDisplay : UnitySingleton<FPSDisplay>
    {
        public float UpdateInterval = 0.5f;
        private float mLastInterval;

        private int mFrames = 0;
        private float mFps;

        private GUIStyle style = new GUIStyle();

        private void Start() {
            Application.targetFrameRate = 60;
            mLastInterval = Time.realtimeSinceStartup;
            mFrames = 0;


            // 字体样式
            style .fontSize = 10;
            style.normal.textColor = new Color(0, 255, 0, 255);
        }

        private void OnGUI() {
            GUI.Label(new Rect(0, 0, 200, 200), "FPS:" + mFps.ToString("f2"), style);
        }

        private void Update() {
            ++ mFrames;
            if(Time.realtimeSinceStartup > mLastInterval + UpdateInterval){
                mFps = mFrames / (Time.realtimeSinceStartup - mLastInterval);
                mFrames = 0;
                mLastInterval = Time.realtimeSinceStartup;
            }
        }
    }
}


