using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedManager : MonoBehaviour {

    private bool grounded;

    private void FixedUpdate() {
        PlayerController.pc.grounded = grounded || PlayerController.pc.groundedFromCast;
    }

    private void OnTriggerStay(Collider other) {
        if(other.gameObject.layer == 9)
            grounded = true;
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.layer == 9)
            grounded = false;
    }

}
