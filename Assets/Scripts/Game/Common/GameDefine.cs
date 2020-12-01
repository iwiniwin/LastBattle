using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDefine
{
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


