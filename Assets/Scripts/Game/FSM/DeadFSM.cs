using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.FSM;

namespace Game
{
    public class DeadFSM : IFSM<Entity>
    {
        public static readonly IFSM<Entity> Instance = new DeadFSM();

        public DeadFSM(){

        }

        public FSMState State {
            get {
                return FSMState.DEAD;
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
            if(entity.FSM != null && entity.FSM.State == FSMState.DEAD){
                entity.objTransform.position = entity.FSMDirection;
                entity.objTransform.rotation = Quaternion.LookRotation(entity.FSMDirection);
                entity.OnReborn();
            }
        }
    }
}


