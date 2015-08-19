using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace SocialPoint.Examples.MVC {
    public class GoViewPresenter : BaseViewPresenter {

        public Transform ViewRoot { get; set; }

        public event EventHandler ViewDidHide;
        public event EventHandler ViewDidShow;

        /// <summary>
        /// 玩家（红方）控制的所有棋子
        /// </summary>
        public Dictionary<string, GameObject> m_hashPlayerQiZis = new Dictionary<string, GameObject>( );

        #region Unity3D Messages propagation

        protected override void AwakeUnityMsg ( ) {
            // This will allow to set the view in the inspector if we want to
            ViewRoot = ViewRoot ?? GetComponent<Transform>( );

            Transform[ ] childs = this.gameObject.GetComponentsInChildren<Transform>( );
            foreach (var child in childs) {
                m_hashPlayerQiZis.Add(child.name, child.gameObject);
            }
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


