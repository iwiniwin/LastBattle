﻿/*
 * @Author: iwiniwin
 * @Date: 2020-11-22 22:06:53
 * @LastEditors: iwiniwin
 * @LastEditTime: 2020-11-22 22:11:49
 * @Description: 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UDK.Event
{

    public class Event
    {
        private GameEvent mId;
        private Dictionary<string, object> mParamList;
        public Event()
        {
            mParamList = new Dictionary<string, object>();
        }

        public Event(GameEvent id)
        {
            mId = id;
            mParamList = new Dictionary<string, object>();
        }

        public GameEvent ID
        {
            get
            {
                return mId;
            }
        }

        public void AddParam(string name, object value)
        {
            mParamList[name] = value;
        }

        public object GetParam(string name)
        {
            if (mParamList.ContainsKey(name))
            {
                return mParamList[name];
            }
            return null;
        }

        public bool HasParam(string name)
        {
            return mParamList.ContainsKey(name);
        }

        public int GetParamCount()
        {
            return mParamList.Count;
        }

        public Dictionary<string, object> GetParamList()
        {
            return mParamList;
        }

    }
}
