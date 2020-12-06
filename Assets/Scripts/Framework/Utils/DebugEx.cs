/*
 * @Author: iwiniwin
 * @Date: 2020-11-08 19:39:41
 * @LastEditors: iwiniwin
 * @LastEditTime: 2020-11-29 23:13:46
 * @Description: 调试日志扩展，便于统一控制与扩展
 */
using System.Collections;
using System.Collections.Generic;

namespace UDK
{
    public class DebugEx
    {
        public static void Log(object msg)
        {
#if (UNITY_IPHONE || UNITY_ANDROID || UNITY_STANDLONE_WIN) && SHOW_LOG

#endif
#if UNITY_EDITOR
            UnityEngine.Debug.Log(msg);
#endif
        }

        public static void Log(object message, UnityEngine.Object context){
#if (UNITY_IPHONE || UNITY_ANDROID || UNITY_STANDALONE_WIN) && SHOW_LOG
#endif
#if UNITY_EDITOR
            UnityEngine.Debug.Log(message, context);
#endif  
        }

        public static void Log(object msg, string typeName)
        {
#if (UNITY_IPHONE || UNITY_ANDROID || UNITY_STANDALONE_WIN) && SHOW_LOG
#endif
#if UNITY_EDITOR
            UnityEngine.Debug.Log(msg);
#endif
        }

        public static void Log(object msg, bool saveToDisk)
        {
#if (UNITY_IPHONE || UNITY_ANDROID || UNITY_STANDALONE_WIN) && SHOW_LOG
#endif
#if UNITY_EDITOR
            UnityEngine.Debug.Log(msg);
#endif
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
#endif
        }

        public static void Log(object msg, string typeName, bool saveToDisk)
        {
#if (UNITY_IPHONE || UNITY_ANDROID || UNITY_STANDALONE_WIN) && SHOW_LOG
#endif
#if UNITY_EDITOR
            UnityEngine.Debug.Log(msg);
#endif
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
#endif
        }

        public static void LogError(object msg)
        {
#if (UNITY_IPHONE || UNITY_ANDROID || UNITY_STANDALONE_WIN) && SHOW_LOG
#endif
#if UNITY_EDITOR
            UnityEngine.Debug.LogError(msg);
#endif
        }

        public static void LogError(object msg, string typeName)
        {
#if (UNITY_IPHONE || UNITY_ANDROID || UNITY_STANDALONE_WIN) && SHOW_LOG
#endif
#if UNITY_EDITOR
            UnityEngine.Debug.LogError(msg);
#endif
        }

        public static void LogError(object msg, bool saveToDisk)
        {
#if (UNITY_IPHONE || UNITY_ANDROID || UNITY_STANDALONE_WIN) && SHOW_LOG
#endif
#if UNITY_EDITOR
            UnityEngine.Debug.LogError(msg);
#endif
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
#endif
        }

        public static void LogError(object msg, string typeName, bool saveToDisk)
        {
#if (UNITY_IPHONE || UNITY_ANDROID || UNITY_STANDALONE_WIN) && SHOW_LOG
#endif
#if UNITY_EDITOR
            UnityEngine.Debug.LogError(msg);
#endif
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
#endif
        }

        public static void LogWarning(string msg)
        {
#if (UNITY_IPHONE || UNITY_ANDROID || UNITY_STANDALONE_WIN) && SHOW_LOG
#endif
#if UNITY_EDITOR
            UnityEngine.Debug.LogWarning(msg);
#endif
        }

        public static void LogException(System.Exception e)
        {
#if (UNITY_IPHONE || UNITY_ANDROID || UNITY_STANDALONE_WIN) && SHOW_LOG
#endif
#if UNITY_EDITOR
            UnityEngine.Debug.LogException(e);
#endif
        }

        public static void LogException(System.Exception e, string typeName)
        {
#if (UNITY_IPHONE || UNITY_ANDROID || UNITY_STANDALONE_WIN) && SHOW_LOG
#endif
#if UNITY_EDITOR
            UnityEngine.Debug.LogException(e);
#endif
        }

        public static void LogException(System.Exception e, bool saveToDisk)
        {
#if (UNITY_IPHONE || UNITY_ANDROID || UNITY_STANDALONE_WIN) && SHOW_LOG
#endif
#if UNITY_EDITOR
            UnityEngine.Debug.LogException(e);
#endif
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
#endif
        }

        public static void LogException(System.Exception e, string typeName, bool saveToDisk)
        {
#if (UNITY_IPHONE || UNITY_ANDROID || UNITY_STANDALONE_WIN) && SHOW_LOG
#endif
#if UNITY_EDITOR
            UnityEngine.Debug.LogException(e);
#endif
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
#endif
        }
    }
}



