using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDefine;
using UDK.MVC;
using UnityEngine.EventSystems;
using UDK.Event;
using UnityEngine.UI;
using System.Text;

namespace Game 
{
    public enum StickState {
        Active,
        Move,
        InActive
    }

    public class GamePlayView : BaseView
    {
        public new GamePlayCtrl Ctrl;

        public float StickMoveRadius = 100.0f;

        RectTransform mJoystickTransform;
        RectTransform mStickTransform;
        RectTransform mStickPointTransform;
        private Vector3 mOrigionJoystickPosition;
        private StickState mStickState;

        public GamePlayView(){
            ResName = GameConfig.GamePlayMainUIPath;
            IsResident = false;
        }

        public override void Init()
        {
            Ctrl = base.Ctrl as GamePlayCtrl;
        }

        protected override void OnLoad()
        {
            RectTransform transform = Root.gameObject.GetComponent<RectTransform>();
            transform.offsetMin = new Vector2(0.0f, 0.0f);  // left  bottom
            transform.offsetMax = new Vector2(0.0f, 0.0f);  // right top

            mJoystickTransform = Root.Find("Joystick").GetComponent<RectTransform>();
            mStickTransform = Root.Find("Joystick/Stick").GetComponent<RectTransform>();
            mStickPointTransform = Root.Find("Joystick/Point").GetComponent<RectTransform>();
            mOrigionJoystickPosition = mJoystickTransform.localPosition;
            
            mStickState = StickState.InActive;
            EventListener.Get(mJoystickTransform.gameObject).onDrag += OnStickDrag;
            EventListener.Get(mJoystickTransform.gameObject).onEndDrag += onStickEndDrag;
        }


        public override void OnEnable()
        {
            
        }

        public override void OnDisable()
        {
           
        }

        public override void Release()
        {
            
        }

        private float moveSendTime = 0f;
        private const float SendMoveInterval = 0.05f;
        public void SendStickMove() {
            Vector3 direction = GetDirection();
            // Entity entity = PlayerManager.Instance.LocalPlayer.
            // mJoystickTransform.gameObject.transform.LookAt();

            EntityComponent entityComponent = PlayerManager.Instance.LocalPlayer.RealObject.GetComponent<EntityComponent>();
            if(entityComponent == null)
                return;

            // 运动正方向
            Vector3 dir = new Vector3(0, 0, 0);

            // 斜45度
            Quaternion rot = Quaternion.Euler(0, 45f, 0);
            dir = rot * new Vector3(0.0f, 0.0f, 1.0f);

            float EntityFSMMoveSpeed = 100f;

            // Vector3 dealPos = entityComponent.transform.position + dir * Time.deltaTime * EntityFSMMoveSpeed;
            // Vector3 dealPos1 = dealPos 

            if(dir != Vector3.zero && Time.time - moveSendTime >= SendMoveInterval) {
                moveSendTime = Time.time;
                MessageCenter.Instance.AskMoveDir(dir);
                // PlayerAdMove(dir);
            }

        }

        public void PlayerAdMove(Vector3 direction) {
            SelfPlayer player = PlayerManager.Instance.LocalPlayer;
            // if 有stop buf 则 return
            // if state == dead || run || relive 则 return

            float moveSpeed = player.EntityFSMMoveSpeed;
            if(moveSpeed <= 0)
                moveSpeed = 3.0f;
            // player.EntityFSMChangedata(player.RealObject.transform.position, direction, moveSpeed);
            // player.OnFSMStateChange();
        }

        public Vector3 GetDirection() {
            Vector2 dir = mStickPointTransform.anchoredPosition - mStickPointTransform.anchoredPosition;
            Vector3 direction = new Vector3(dir.x, 0f, dir.y);
            direction.Normalize();
            return direction;
        }

        public void SetStickPos(Vector2 pos) {
            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(mStickPointTransform, pos, null, out localPos);

            if(Vector2.Distance(localPos, Vector2.zero) > StickMoveRadius) {
                localPos.Normalize();
                localPos = localPos * StickMoveRadius;
            }

            mStickTransform.anchoredPosition = localPos;
        }

        /* UI事件响应 */

        void OnStickDrag(GameObject gameObject, PointerEventData eventData) {
            
            Vector2 touchPos = eventData.position;
            
            SetStickPos(touchPos);
            mStickState = StickState.Move;
            SendStickMove();
        }

        void onStickEndDrag(GameObject gameObject, PointerEventData eventDat) {
            mStickTransform.anchoredPosition = mStickPointTransform.anchoredPosition;
        }

    }
}


