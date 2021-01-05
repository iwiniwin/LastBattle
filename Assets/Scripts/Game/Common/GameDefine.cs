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
        UserInfo,
        Lobby,
        Room,
        Hero,
        Loading,
        Play,
        Over,
    }

    public enum EEntityCampType {
        Null = -2,
        Bad,
        Kind,
        A,
        B,
        C,
        D,
        E,
        F,
    }

    public enum EObjectType {
        None = 0,
        Guild,
        User,
        HeroBegin = 10000,
        NPCBegin = 20000,
        GoodsBegin = 30000,
        AIRobotBegin = 40000,
    }

    public enum EHeroType {
        All,
        Attack,
        Assist,
        Spell,
        Defend,
    }

    public enum EEntityType {
        Monster = 1,
        Soldier,
        Building,
        Player,
        AltarSoldier,
    }

    public enum ENpcCateChild {
        None = 0,
        PerAtkBuilding,
        PerBomb,
        SmallMonster,
        HugeMonster,
        BuildAltar = 10,
        BuildBase,
        BuildShop,
        BuildTower,
        BuildSummon = 20,
        Other,
    }
}


