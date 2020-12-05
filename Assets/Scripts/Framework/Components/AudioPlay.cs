using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDK.Utils;

namespace UDK.Components 
{
    public class AudioPlay : MonoBehaviour
    {

        public AudioClip clip;

        public void PlayAudio(){
            if(clip != null) {
                AudioUtil.PlaySound(clip);
            }
        }
    }
}


