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
            PlayerAnimation("walk");
        }

        public void PlayerFreeAnimation() {
            PlayerAnimation("free");
        }

        public void PlayeAttackAnimation() {
            var animation = GetComponent<Animation>();
            string name = "attack";
            if(SyncEntity != null && SyncEntity.EntityType == EEntityType.Player) {
                int id = Random.Range(0, ConfigReader.HeroXmlInfoDic[SyncEntity.NpcGUIDType].n32RandomAttack.Count);
                name = ConfigReader.HeroXmlInfoDic[SyncEntity.NpcGUIDType].n32RandomAttack[id];
            }
            if(animation.isPlaying) {
                animation.Stop();
            }
            PlayerAnimation(name);
        }

        public void PlayerAnimation(string name) {
            GetComponent<Animation>().CrossFade(name);
        }
    }
}


