using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDK.Resource;

namespace UDK
{
    public static class AudioUtil
    {

        public static AudioSource mAudioSource;

        public static float SoundVolume {
            get {
                return 1f;
            }
        }
        
        public static AudioSource PlaySound(AudioClip clip, float volume = 1.0f, float pitch = 1.0f) {
            volume *= SoundVolume;
            if(clip != null && volume > 0.0f){
                if(mAudioSource == null){
                    Camera camera = Camera.main;
                    if(camera == null)
                        camera = GameObject.FindObjectOfType(typeof(Camera)) as Camera;
                    if(camera != null)
                        mAudioSource = camera.gameObject.AddComponent<AudioSource>();
                }
                if(mAudioSource != null){
                    mAudioSource.pitch = pitch;
                    mAudioSource.PlayOneShot(clip, volume);
                    return mAudioSource;
                }
            }
            return null;
        }

        public static AudioSource PlaySound(string path) {
            if(path == "") return null;
            ResourceUnit clipUnit = ResourceManager.Instance.LoadImmediate(path, EResourceType.ASSET);
            AudioClip clip = clipUnit.Asset as AudioClip;
            return PlaySound(clip);
        }
    }
}


