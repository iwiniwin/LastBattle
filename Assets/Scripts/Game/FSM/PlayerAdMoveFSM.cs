using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDK;
using UDK.FSM;

namespace Game 
{
    public class PlayerAdMoveFSM : Singleton<PlayerAdMoveFSM>, IFSM<Entity>
    {
        public EFSMState State {
            get {
                return EFSMState.ADMOVE;
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

        }

        public void Execute(Entity entity) {
            // entity.OnEnterEntityAdMove();
        }

        public void Exit(Entity entity) {
            // entity.OnExecuteEntityAdMove();
        }
    }
}


