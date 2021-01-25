using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Game 
{
    public class VectorUtil
    {
        public static Vector3 ConvertPosToVector3(GSToGC.Pos pos) {
            float mapHeight = 60.0f;
            if(pos != null) 
                return new Vector3((float)pos.x / 100.0f, mapHeight, (float)pos.z / 100.0f);
            else
                return Vector3.zero;
        }

        public static Vector3 ConvertDirToVector3(GSToGC.Dir dir) {
            float angle = (float)(dir.angle) / 10000;
            return new Vector3((float)Math.Cos(angle), 0, (float)Math.Sin(angle));
        }
    }
}


