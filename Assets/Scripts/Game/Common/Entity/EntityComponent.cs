using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game 
{
    public class EntityComponent : MonoBehaviour
    {
        public Entity SyncEntity;
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
    }
}


