/*
 * @Author: iwiniwin
 * @Date: 2020-11-29 22:44:41
 * @LastEditors: iwiniwin
 * @LastEditTime: 2020-11-29 22:51:55
 * @Description: 资源更新模块
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDK.Resource
{
    public class ResourceUpdateManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
#if UNITY_EDITOR
            OnUpdateComplete();
#endif
        }

        void OnUpdateComplete()
        {
            // 初始化资源管理器
            ResourceManager.Instance.Init();
            // 加载登录场景
            ResourceManager.Instance.LoadLevel("Scenes/Login", null);
        }
    }
}


