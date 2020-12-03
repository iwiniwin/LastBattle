/*
 * @Author: iwiniwin
 * @Date: 2020-11-22 22:03:31
 * @LastEditors: iwiniwin
 * @LastEditTime: 2020-12-03 23:03:36
 * @Description: 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public enum EGameEvent
    {
        // 连接服务器成功
        ConnectServerSuccess,
        // 连接服务器失败
        ConnectServerFail,

        ReconnectToBattle,
        BeginWaiting,

        // 显示登录视图
        ShowLoginView,
        // 关闭登录视图
        HideLoginView,
    }
}


