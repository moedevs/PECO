using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRespawn : MonoBehaviour {

    public Transform correspondingPoint;
    public bool oneTimeOnly;
    public bool triggered = false;

    private void OnTriggerEnter(Collider other) {
        if(!triggered && other.gameObject == PlayerController.pc.controlledPawn) {
            LevelData.data.currentRespawn = correspondingPoint.position;
            if(oneTimeOnly)
                triggered = true;
        }
    }

}
