using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;

namespace SocialPoint.Examples.MVC {
    public class UguiButtonViewPresenter : UguiViewPresenter {

        public Button m_button;
        public Text m_buttonLabel;
        public Image m_buttonImage;


        public string Text {
            get {
                return m_buttonLabel != null ? m_buttonLabel.text : string.Empty;
            }
            set {
                if (m_buttonLabel == null) {
                    return;
                }
                m_buttonLabel.text = value;
            }
        }

        public Sprite ImageSprite {
            get {
                return m_buttonImage != null ? m_buttonImage.sprite : null;
            }

            set {
                if (m_buttonImage == null) {
                    return;
                }

                m_buttonImage.sprite = value;
            }
        }

        /// <summary>
        ///     This will allow to keep track of the status of the button in order to disable
        ///     the events if the button is disabled
        /// </summary>
        public bool IsEnabled {
            get;
            private set;
        }

        public event UnityAction Clicked;

        /// <summary>
        /// 在编辑器里指定UGUI响应事件
        /// </summary> 
        public virtual void OnButtonClicked ( ) {
            // Do not propagate the click event if the button is disabled
            if (!IsEnabled) {
                return;
            }

            if (Clicked != null) {
                Clicked( );
            }
        }

        protected override void AwakeUnityMsg ( ) {
            base.AwakeUnityMsg( );

            WireUIEvents( );

            IsEnabled = m_button.enabled;

            //_buttonOriginalDefaultColor = m_button.defaultColor;
            //_buttonOriginalHoverColor = m_button.hover;
        }

        protected virtual void WireUIEvents ( ) {
            // Programatically add the onClick handler if it is not set
            // so the ButtonClicked event is always called (NGUI specific)
            if (m_button != null && m_button.onClick != null && Clicked != null) {
                m_button.onClick.AddListener(Clicked);
            }
        }

    }
}

