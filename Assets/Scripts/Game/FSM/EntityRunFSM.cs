using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDK;
using UDK.FSM;

namespace Game 
{
    public class EntityRunFSM : Singleton<EntityRunFSM>, IFSM<Entity>
    {
        public EFSMState State {
            get {
                return EFSMState.RUN;
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
            // UDK.Output.Dump("execute move.......");
            entity.OnExecuteMove();
        }

        public void Exit(Entity entity) {
        }
    }
}


