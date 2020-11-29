using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UDK.Network 
{
    public class DataEventArgs : EventArgs
    {
        public byte[] Data {get; set;}
        public byte[] Offset {get; set;}
        public byte[] Length {get; set;}
    }
}


