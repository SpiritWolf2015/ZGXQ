using UnityEngine;
using System.Collections;

public class DebugLogManagerBehaviour : MonoBehaviour {

    public bool m_enableLog = true;

	void Awake () 
	{
        Debuger.EnableLog = m_enableLog;		
	}
}
