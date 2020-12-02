using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDK.MVC;
using GameDefine;

namespace Game
{
    public class LoginCtrl : BaseCtrl<EModuleType>
    {
        public LoginCtrl()
        {
            BindView<LoginView>();
        }

        public override void Enter()
        {
            ShowView();
        }

        public override void Exit()
        {
            throw new System.NotImplementedException();
        }
    }
}


