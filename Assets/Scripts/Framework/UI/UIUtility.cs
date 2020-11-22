using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.UI
{
    public static class UIUtility
    {
        public static Camera GetUICamera() {
            return GUI.Instance.transform.Find("Camera").GetComponent<Camera>();
        }
    }
}


