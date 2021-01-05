using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public static class GameConfig
    {
        // server
        public static string LoginServerAddress = "127.0.0.1";
        public static int LoginServerPort = 49996;

        public static string LoginPrefabPath = "Game/GameLogin";
        public static string LoginUIPath = "UI/LoginUI";
        public static string RegisterUIPath = "UI/RegisterUI";
        public static string LobbyUIPath = "UI/LobbyUI";
        public static string LobbyBattleUIPath = "UI/LobbyBattleUI";
        public static string LoadProgressPrefabPath = "UI/LoadProgressUI";

        // map
        public static string MapConfigXMlPath = "Config/MapLoadCfg";


        // 音频
        public static string UIBGSoundPath = "Audio/EnvironAudio/mus_fb_login_lp";

        // entity
        public static string MonsterModelsDir = "Monsters/";

        // sdk
        public static string SdkPath = "";
    }
}


