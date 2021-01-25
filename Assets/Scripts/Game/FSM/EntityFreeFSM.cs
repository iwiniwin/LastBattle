using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDK;
using UDK.FSM;

namespace Game 
{
    public class EntityFreeFSM : Singleton<EntityFreeFSM>, IFSM<Entity>
    {
        public EFSMState State {
            get {
                return EFSMState.FREE;
            }
        }

        public bool CanChange {
            get;
            set;
        }

        public bool ChangeState(Entity entity, IFSM<Entity> fsm) {
            return CanChange;
        }

        public void Enter(Entity entity, float last) {
            entity.OnEnterFree();
        }

        public void Execute(Entity entity) {
            entity.OnExecuteFree();
        }

        public void Exit(Entity entity) {
        }
    }
}


