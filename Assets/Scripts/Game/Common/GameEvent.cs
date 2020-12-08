/*
 * @Author: iwiniwin
 * @Date: 2020-11-22 22:03:31
 * @LastEditors: iwiniwin
 * @LastEditTime: 2020-12-09 00:04:09
 * @Description: 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public enum EGameEvent
    {
        // 接收到到BS地址
        OnReceiveBSAddress,
        // 接收到登录BS校验结果
        OnReceiveCheckLoginBSRet,
        // 接收到GS信息
        OnReceiveGSInfo,

        ReconnectToBattle,
        BeginWaiting,
        
        // 显示登录视图
        ShowLoginView,
        // 关闭登录视图
        HideLoginView,
    }
}


