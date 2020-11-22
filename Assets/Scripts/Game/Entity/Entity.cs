using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Framework.FSM;

namespace Game
{
    public class Entity
    {
        // public Entity(UInt64 guid, EntityCamp)

        public AudioSource absorbSound;

        public int resPath;

        public Transform objTransform = null;

        public virtual void OnEnterDead(){

        }

        public virtual void OnExecuteDead(){

        }

        public IFSM<Entity> FSM {
            private set;
            get;
        } 

        public Vector3 FSMDirection {
            private set;
            get;
        }

        public virtual void OnReborn(){
            DoReborn();
        }

        protected virtual void DoReborn(){

        }
    }
}


