using UnityEngine;
using System.Collections;
using com.gzc.ThreadLockEvent;

public class EventTicker : MonoBehaviour {

    public float second = 0.05F;

    void Start ( ) {
        this.InvokeRepeating("Tick", 0, second);
    }

    void Tick ( ) {
        EventDispatcher.Instance( ).OnTick( );
    }

}
