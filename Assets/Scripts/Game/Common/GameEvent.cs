/*
 * @Author: iwiniwin
 * @Date: 2020-11-22 22:03:31
 * @LastEditors: iwiniwin
 * @LastEditTime: 2021-01-07 22:09:42
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

        // 连接到GS成功
        OnConnectGateServerSuccess,

        // 接收到战斗信息
        OnReceiveBattleBaseInfo,
        // 收到战斗座位信息
        OnReceiveBattleSeatPosInfo,
        OnReceiveBattleStateChange,
        // 接收到显示GO
        OnReceiveGameObjectAppear,
        OnReceiveGameObjectRunState,

        // 加载游戏
        LoadingGame,

        // 接收到英雄列表
        OnReceiveHeroList,

        // 接收到用户信息
        OnReceiveUserBaseInfo,

        ReconnectToBattle,
        BeginWaiting,
        
        // 显示登录视图
        ShowLoginView,
        // 关闭登录视图
        HideLoginView,
        // 显示用户信息视图
        ShowUserInfoView,
        // 关闭用户信息视图
        HideUserInfoView,
        // 显示大厅视图
        ShowLobbyView,
        // 关闭大厅视图
        HideLobbyView,
        ShowBattleView,
        HideBattleView,
        // 加载视图
        ShowLoadingView,
        HideLoadingView,
        ShowGamePlayView,
        HideGamePlayView,

        // 通知加载游戏场景结束
        LoadGameSceneFinish,
    }
}


