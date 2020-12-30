using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UDK 
{
    public static class TimeUtil 
    {
        private static DateTime mBaseTime = new DateTime(1970, 1, 1, 0, 0, 0);

        public static Int64 GetUTCMillisec() {
            TimeSpan timeSpan = DateTime.UtcNow - mBaseTime;
            return (Int64)timeSpan.TotalMilliseconds;
        }
    }
}


