﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDK.FSM;

namespace Game
{
    public class DeadFSM : IFSM<Entity>
    {
        public static readonly IFSM<Entity> Instance = new DeadFSM();

        public DeadFSM(){

        }

        public EFSMState State {
            get {
                return EFSMState.DEAD;
            }
        }

        public bool CanChange{
            set;
            get;
        }

        public bool ChangeState(Entity entity, IFSM<Entity> fsm){
            return CanChange;
        }

        public void Enter(Entity entity, float last)
        {
            entity.OnEnterDead();
        }

        public void Execute(Entity entity){
            entity.OnExecuteDead();
        }

        public void Exit(Entity entity){
            if(entity.FSM != null && entity.FSM.State == EFSMState.DEAD){
                entity.RealObject.transform.position = entity.EntityFSMDirection;
                entity.RealObject.transform.rotation = Quaternion.LookRotation(entity.EntityFSMDirection);
                entity.OnReborn();
            }
        }
    }
}


