/*
 * @Author: iwiniwin
 * @Date: 2020-11-08 17:47:07
 * @LastEditors: iwiniwin
 * @LastEditTime: 2021-01-27 22:37:20
 * @Description: 文件操作工具类
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace UDK
{
    public static class FileUtil
    {
        public static byte[] GetAssetBundleFileBytes(string path, ref int size)
        {
            string localPath;
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                localPath = string.Format("{0}/{1}", Application.persistentDataPath, path + UDK.Config.AssetBundleFileSuffix);
            }
            else
            {
                localPath = UDK.Config.AssetBundleFilePath + path + UDK.Config.AssetBundleFileSuffix;
            }

            if (File.Exists(localPath))
            {
                try
                {
                    FileStream bundleFile = File.Open(localPath, FileMode.Open, FileAccess.Read);
                    byte[] bytes = new byte[bundleFile.Length];
                    bundleFile.Read(bytes, 0, (int)bundleFile.Length);
                    size = (int)bundleFile.Length;
                    bundleFile.Close();
                    return bytes;
                }
                catch (System.Exception e)
                {
                    DebugEx.LogError(e);
                    throw;
                }
            }
            else
            {
                TextAsset bundleFile = Resources.Load(path) as TextAsset;
                if (null == bundleFile)
                {
                    DebugEx.LogError("load " + path + " bundle file error !!!");
                }
                size = bundleFile.bytes.Length;
                return bundleFile.bytes;
            }
        }

        public static string GetFileName(string fileName)
        {
            int index = fileName.IndexOf(".");
            if (index == -1)
            {
                throw new System.Exception("can not find");
            }
            return fileName.Substring(0, index);
        }

        public static string GetFileName(string filePath, bool suffix)
        {
            string path = filePath.Replace("\\", "/");
            int index = path.LastIndexOf("/");
            if (index == -1)
                throw new System.Exception("can not find");
            if (!suffix)
            {
                int index2 = path.LastIndexOf(".");
                if (index2 == -1)
                    throw new System.Exception("can not find");
                return path.Substring(index + 1, index2 - index - 1);
            }
            else
            {
                return path.Substring(index + 1, path.Length - index - 1);
            }
        }

        public static string GetResourceName(string resPath) {
            int index = resPath.LastIndexOf("/");
            if (index == -1)
                return resPath;
            else
            {
                return resPath.Substring(index + 1, resPath.Length - index - 1);
            }
        }

        public static Stream Open(string path)
        {
            string localPath;
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                localPath = string.Format("{0}/{1}", Application.persistentDataPath, path + UDK.Config.AssetBundleFileSuffix);
            }
            else
            {
                localPath = UDK.Config.AssetBundleFilePath + path + UDK.Config.AssetBundleFileSuffix;
            }
            if (File.Exists(localPath))
            {
                return File.Open(localPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
            else
            {
                TextAsset text = Resources.Load(path) as TextAsset;
                if(text == null)
                    DebugEx.LogError("can not find " + path);
                return new MemoryStream(text.bytes);
            }
        }

        public static StreamReader OpenText(string path){
            return new StreamReader(Open(path), System.Text.Encoding.Default);
        }
    }
}


