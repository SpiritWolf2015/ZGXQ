using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace SocialPoint.Examples.MVC {
    public class PlayerRootViewPresenter : BaseViewPresenter {

        public Transform ViewRoot { get; set; }

        public event EventHandler ViewDidHide;
        public event EventHandler ViewDidShow;


        private Dictionary<string, BoxCollider> m_hashPlayerQiZis = new Dictionary<string, BoxCollider>( );
        /// <summary>
        /// 玩家（红方）控制的所有棋子
        /// </summary>
        public Dictionary<string, BoxCollider> HashPlayerQiZis {
            get {
                return m_hashPlayerQiZis;
            }
        }


        #region Unity3D Messages propagation

        protected override void AwakeUnityMsg ( ) {
            // This will allow to set the view in the inspector if we want to
            ViewRoot = ViewRoot ?? GetComponent<Transform>( );

            InitValue( );
        }
        #endregion

        public override void Enable ( ) {
            foreach (BoxCollider qizi in m_hashPlayerQiZis.Values) {
                qizi.enabled = true;
            }
        }
        public override void Disable ( ) {
            foreach (BoxCollider qizi in m_hashPlayerQiZis.Values) {
                qizi.enabled = false;
            }
        }

        public override void Show ( ) {
            ViewRoot.gameObject.SetActive(true);
        }
        public override void Hide ( ) {
            ViewRoot.gameObject.SetActive(false);
        }

        void InitValue ( ) {
            BoxCollider[ ] childs = this.gameObject.GetComponentsInChildren<BoxCollider>( );
            foreach (var child in childs) {
                m_hashPlayerQiZis.Add(child.name, child);
            }
        }

    }
}


