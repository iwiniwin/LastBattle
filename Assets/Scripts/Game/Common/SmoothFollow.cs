using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game 
{
    public class SmoothFollow : MonoBehaviour
    {
        // 跟随的目标
        public Transform Target;
        // 相机距离目标的高度
        public float Height;

        public float Distance = 10.0f;

        public float HeightDamping = 2.0f;
        public float RotationDamping = 3.0f;

        // 摄像机是否正在移动
        public bool CameraIsMoving;


        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void FixedUpdatePosition() {
            if(Target == null || CameraIsMoving) return;
            // 相机高度固定60
            Vector3 targetPos = new Vector3(Target.position.x, 60, Target.position.z);

            float rotationAngle = Target.eulerAngles.y;
            float height = targetPos.y + Height;

            float curRotationAngle = transform.eulerAngles.y;
            float curHeight = transform.position.y;

            curRotationAngle = Mathf.LerpAngle(curRotationAngle, rotationAngle, RotationDamping * Time.deltaTime);
            curHeight = Mathf.Lerp(curHeight, height, HeightDamping * Time.deltaTime);

            Quaternion rotation = Quaternion.Euler(0, curRotationAngle, 0);

            Vector3 pos = targetPos - rotation * Vector3.forward * Distance;
            pos.y = curHeight;
            transform.position = pos;
            Vector3 dir = targetPos - transform.position;
            dir.Normalize();
            transform.rotation = Quaternion.LookRotation(dir);
        }
    }
}


