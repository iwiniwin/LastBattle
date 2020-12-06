/*
 * @Author: iwiniwin
 * @Date: 2020-11-07 22:20:32
 * @LastEditors: iwiniwin
 * @LastEditTime: 2020-11-29 23:15:44
 * @Description: 存档管理
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Xml;

namespace UDK.Resource
{
    class ArchiveManager : Singleton<ArchiveManager>
    {
        internal  Dictionary<string, Archive> mAllArchives;

        public ArchiveManager(){
            mAllArchives = new Dictionary<string, Archive>();
        }

        // 加载存档文件
        public void Init(){
            StreamReader sr = FileUtil.OpenText("Resource");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(sr.ReadToEnd());
            XmlElement root = doc.DocumentElement;
            IEnumerator iter = root.GetEnumerator();
            while(iter.MoveNext()){
                XmlElement childRoot = iter.Current as XmlElement;
                IEnumerator childIter = childRoot.GetEnumerator();
                if(!mAllArchives.ContainsKey(childRoot.Name)){
                    Archive archive = new Archive();
                    mAllArchives.Add(childRoot.Name, archive);
                }
                while(childIter.MoveNext()){
                    XmlElement file = childIter.Current as XmlElement;
                    string name = file.GetAttribute("name");
                    string type = file.GetAttribute("type");
                    mAllArchives[childRoot.Name].Add(name, type);
                }
            }
            sr.Close();
        }

        public string GetPath(string archiveName, string fileName){
            if(mAllArchives.ContainsKey(archiveName)){
                return mAllArchives[archiveName].GetPath(fileName);
            }else{
                DebugEx.LogError("can not find " + archiveName);
            }
            return null;
        }
    }
}


