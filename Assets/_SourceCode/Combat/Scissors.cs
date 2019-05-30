using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scissors : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        /*if(other.CompareTag("PlayerControllable") && other.gameObject != PlayerController.pc.gameObject)
            PlayerController.pc.ChangeControlledPawn(other.gameObject);*/
        if(other.CompareTag("Enemy")) {
            PlayerController.pc.ChangeControlledPawn(PlayerController.Form.Bear);
        } else if(other.CompareTag("CostumeGiver")) {
            try {
                PlayerController.pc.ChangeControlledPawn(other.GetComponent<CostumeDebug>().costume);
            } catch {
                PlayerController.pc.ChangeControlledPawn(PlayerController.Form.Bear);
            }
        }
    }

}
