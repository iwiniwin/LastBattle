using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDK;
using UDK.FSM;

namespace Game 
{
    public class EntityReleaseSkillFSM : Singleton<EntityReleaseSkillFSM>, IFSM<Entity>
    {
        public EFSMState State {
            get {
                return EFSMState.RELEASE;
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
            entity.OnEntityReleaseSkill();
        }

        public void Execute(Entity entity) {
            
        }

        public void Exit(Entity entity) {
           
        }
    }
}


