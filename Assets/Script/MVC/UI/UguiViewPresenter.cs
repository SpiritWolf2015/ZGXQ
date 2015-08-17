using UnityEngine;
using System.Collections;
using System;

namespace SocialPoint.Examples.MVC {

    public class UguiViewPresenter : BaseViewPresenter {

        public RectTransform ViewRoot { get; set; }

        public event EventHandler ViewDidHide;
        public event EventHandler ViewDidShow;

        #region Unity3D Messages propagation

        protected override void AwakeUnityMsg ( ) {
            // This will allow to set the view in the inspector if we want to
            ViewRoot = ViewRoot ?? GetComponent<RectTransform>( );
        }
        #endregion

        public override void Enable ( ) {
            ViewRoot.gameObject.SetActive(true);
        }
        public override void Disable ( ) {
            ViewRoot.gameObject.SetActive(false);
        }

        public override void Show ( ) {
            ViewRoot.gameObject.SetActive(true);
        }
        public override void Hide ( ) {
            ViewRoot.gameObject.SetActive(false);
        }

    }

}


