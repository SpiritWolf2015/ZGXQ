using UnityEngine;
using System.Collections;

namespace SocialPoint.Examples.MVC {

    public abstract class BaseViewPresenter : MonoBehaviour {

        public virtual void Show ( ) { }
        public virtual void Hide ( ) { }

        public virtual void Enable ( ) { }
        public virtual void Disable ( ) { }

        #region Unity3D messages
        void Awake ( ) {
            AwakeUnityMsg( );
        }

        void Start ( ) {
            StartUnityMsg( );
        }

        void OnDestroy ( ) {
            OnDestroyUnityMsg( );
        }
        #endregion

        #region Unity3D Messages propagation
        protected virtual void AwakeUnityMsg ( ) {
        }

        protected virtual void StartUnityMsg ( ) {
        }

        protected virtual void OnValidateUnityMsg ( ) {
        }

        protected virtual void OnDestroyUnityMsg ( ) {
        }
        #endregion
    }
}

