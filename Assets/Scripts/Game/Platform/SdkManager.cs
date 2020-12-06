using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDK;
using UDK.Resource;

namespace Game 
{
    public class SdkManager : Singleton<SdkManager>
    {
        public enum EPlatform {
            //IOS平台SDK
            None = 0,
            IOS_91 = 1,
            IOS_TB = 2,
            IOS_PP = 3,
            IOS_CMGE = 4,
            IOS_UC = 5,
            IOS_iTools = 6,
            IOS_OnlineGame = 7,
            IOS_As = 8,
            IOS_XY = 9,
            IOS_CMGE_ZB = 10,


            //Andriod平台SDK
            Android_UC = 105,
        }

        public EPlatform PlatformType {get; private set;}
        
        void CreateSdk(string path) {
            ResourceUnit unit = ResourceManager.Instance.LoadImmediate(path, EResourceType.PREFAB);
            GameObject connector = GameObject.Instantiate(unit.Asset) as GameObject;
            connector.name = "SdkConnect";
        }

        public void CreateSdkPrefab(EPlatform type) {
            PlatformType = type;
            if(type == EPlatform.None) {
                return;
            }
            CreateSdk(GameConfig.SdkPath);
        }
    }
}


