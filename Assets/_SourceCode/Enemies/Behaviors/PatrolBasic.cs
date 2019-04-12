using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBasic : BehaviorBase {

    public override void OnIdle() {
        base.OnIdle();
    }

    public override void OnSuspicious() {
        base.OnSuspicious();
        Debug.Log("suspicious");
    }

    public override void OnDetectPlayer() {
        base.OnDetectPlayer();
        Debug.Log("detected");
    }

}
