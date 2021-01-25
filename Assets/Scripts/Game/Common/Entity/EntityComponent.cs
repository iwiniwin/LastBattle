using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDefine;

namespace Game 
{
    public class EntityComponent : MonoBehaviour
    {
        public Entity SyncEntity;
        public EEntityCampType CampType;
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if(SyncEntity != null) {
                SyncEntity.OnUpdate();  
            }
        }

        public void PlayerRunAnimation() {
            GetComponent<Animation>().CrossFade("walk");
        }

        public void PlayerFreeAnimation() {
            GetComponent<Animation>().CrossFade("free");
        }

        public void PlayeAttackAnimation() {

        }

        public void PlayerAnimation(string name) {
            GetComponent<Animation>().CrossFade(name);
        }
    }
}


