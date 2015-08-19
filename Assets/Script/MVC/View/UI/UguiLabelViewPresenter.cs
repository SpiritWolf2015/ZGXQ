using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace SocialPoint.Examples.MVC {
    public class UguiLabelViewPresenter : UguiViewPresenter {

        public Text m_textLabel;

        public string Text {
            get { return m_textLabel.text; }
            set { m_textLabel.text = value; }
        }

        public UnityEngine.Color TextColor {
            get { return m_textLabel.color; }
            set { m_textLabel.color = value; }
        }
    }

}
