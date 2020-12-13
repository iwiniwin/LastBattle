using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game 
{
    public class StringUtil
    {
        public static bool CheckName(string text)
        {
            char ch;
            for (int i = 0; i < text.Length; i++)
            {
                ch = text[i];
                if (!((ch >= 0x4e00 && ch <= 0x9fbb) || (ch >= 'A' && ch <= 'Z') || (ch >= 'a' && ch <= 'z') || (ch >= '0' && ch <= '9')))
                {
                    return false;
                }
            }

            return true;
        }
    }
}


