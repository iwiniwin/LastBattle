using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDK;

namespace Game 
{
    public class SelectServerData : Singleton<SelectServerData>
    {
        public enum EServerState {
            Fluent = 0,  // 流畅
            Busy,  // 繁忙
            HouseFull,  // 爆满
        }

        public static KeyValuePair<string, Color>[] StateDescrption = {
            new KeyValuePair<string, Color>("流畅", Color.green),
            new KeyValuePair<string, Color>("繁忙", Color.yellow),
            new KeyValuePair<string, Color>("爆满", Color.red),
        };

        public class ServerInfo {
            public int index;
            public string name;
            public EServerState state;
            public string address;
            public int port;
        }

        Dictionary<int, ServerInfo> serverInfoDic = new Dictionary<int, ServerInfo>();

        public string ServerAddress {get; set;}
        public int ServerPort {get; set;}
        public string ServerToken {get; set;}
        public int ServerPlatform {private set; get;}
        public string ServerUin {get; set;}
        public string ServerSessionId {get; set;}
        public string GateServerUin {get; set;}
        public int CurSelectIndex {private set; get;}
        public ServerInfo CurSelectServer {private set; get;}

        public const string ServerKey = "Server";
        public const string ServerStateKey = "ServerState";

        public void AddServerInfo(int index, string name, EServerState state, string address, int port) {
            ServerInfo info = new ServerInfo();
            info.index = index;
            info.name = name;
            info.state = state;
            info.address = address;
            info.port = port;
            serverInfoDic.Add(index, info);
        }

        public void SetDefaultServer() {
            int index = 0;
            if(PlayerPrefs.HasKey(ServerKey)) {
                string name = PlayerPrefs.GetString(ServerKey);
                for(int i = 0; i < serverInfoDic.Count; i ++) {
                    if(name.CompareTo(serverInfoDic[i].name) == 0){
                        index = i;
                        break;
                    }
                }
            }else{
                index = 0;
            }
            SetSelectServer(index);
        }

        // uin = account ?
        // SessionId = password ?
        public void SetExtraInfo(int platform, string uin, string sessionId) {
            ServerPlatform = platform;
            ServerUin = uin;
            ServerSessionId = sessionId;
        }

        public void SetSelectServer(int index) {
            CurSelectIndex = index;
            CurSelectServer = serverInfoDic[index];
        }
        
        public void Clean() {
            serverInfoDic.Clear();
        }


    }
}


