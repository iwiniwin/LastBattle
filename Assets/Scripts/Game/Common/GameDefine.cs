using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDefine
{
    // 场景类型
    public enum ESceneType {
        None,
        Login,
        Play
    }

    // 模块类型
    public enum EModuleType {
        Login,
        User,
        Lobby
    }

    // 服务器类型
    public enum EServerType
    {
        GateServer = 0,
        BalanceServer,
        LoginServer
    }

    // 战斗类型
    public enum EBattleState
    {
        SelectHero = 0,
        SelectRune,
        Loading,
        Playing,
        Finished,
    }

    // 游戏状态类型
    public enum EGameStateType {
        Login,
        User,
        Lobby,
        Room,
        Hero,
        Loading,
        Play,
        Over,
    }
}


