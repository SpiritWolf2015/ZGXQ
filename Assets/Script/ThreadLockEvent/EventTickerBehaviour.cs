using UnityEngine;
using System.Collections;


namespace com.gzc.ThreadLockEvent {
    public class EventTickerBehaviour : MonoBehaviour {

        public float second = 0.05F;

        void Start ( ) {
            this.InvokeRepeating("Tick", 0, second);
        }

        void Tick ( ) {
            EventDispatcher.Instance( ).OnTick( );
        }
    }
}


