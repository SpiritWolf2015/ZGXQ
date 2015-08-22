using UnityEngine;
using System.Collections;
using System;
using com.gzc.zgxq.game;

public static class DelayToInvokeUtil {

    public static IEnumerator DelayToInvokeDo ( Action action , float delaySeconds ) {
        yield return new WaitForSeconds(delaySeconds);
        if ( action != null ) {
            action( ); 
        } else {
            Debuger.LogError("action is null");
        }        
    }

}
