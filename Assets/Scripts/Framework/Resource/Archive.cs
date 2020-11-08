/*
 * @Author: iwiniwin
 * @Date: 2020-11-07 22:15:40
 * @LastEditors: iwiniwin
 * @LastEditTime: 2020-11-08 22:51:12
 * @Description: 存档
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Utils;

namespace Framework.Resource
{
    public class Archive
    {
        private Dictionary<string, string> mAllFiles;

        public Archive(){
            mAllFiles = new Dictionary<string, string>();
        }

        public Dictionary<string, string> AllFiles{
            get {
                return mAllFiles;
            }
        }

        public void Add(string fileName, string type){
            if(!mAllFiles.ContainsKey(fileName)){
                mAllFiles.Add(fileName, type);
            }
        }

        public string GetPath(string fileName){
            if(mAllFiles.ContainsKey(fileName)){
                return fileName + "." + mAllFiles[fileName];
            }else{
                DebugEx.LogError("can not find " + fileName);
            }
            return null;
        }
    }
}


