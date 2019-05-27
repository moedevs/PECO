using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour {

    [HideInInspector] public Vector3 hitNormal;

    private void OnControllerColliderHit(ControllerColliderHit hit) {
        hitNormal = hit.normal;
        //Debug.Log(hit.gameObject.name);
    }
}
