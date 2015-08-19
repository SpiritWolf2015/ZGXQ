using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SocialPoint.Examples.MVC {
    public class PlayerDragViewPresenter : GoViewPresenter {        

        private BoxCollider m_selfBoxCollider;


        #region Unity3D Messages propagation

        protected override void StartUnityMsg ( ) {
            m_selfBoxCollider = GetComponent<BoxCollider>( );
        }

        #endregion

        public override void Enable ( ) {
            m_selfBoxCollider.enabled = true;
        }

        public override void Disable ( ) {
            m_selfBoxCollider.enabled = false;
        }

    }
}

